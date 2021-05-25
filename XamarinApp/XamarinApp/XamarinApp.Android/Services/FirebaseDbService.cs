using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using XamarinApp.Models;
using XamarinApp.Services;

namespace XamarinApp.Droid.Services
{
    public class FirebaseDbService : IFirebaseDbService
    {
        private readonly FirebaseClient _databaseClient =
            new FirebaseClient("https://mobilecomputers-f4c00-default-rtdb.firebaseio.com/");

        public async Task AddUserInfo(User userDto)
        {
            await _databaseClient
                .Child("Users")
                .PostAsync(userDto);
        }

        public async Task AddComputer(Computer computerDto)
        {
            await _databaseClient
                .Child("Computers")
                .PostAsync(computerDto);
        }

        public List<Computer> GetAllComputers()
        {
            var taskGetAllComputers = _databaseClient
                .Child("Computers")
                .OnceAsync<Computer>();

            taskGetAllComputers.Wait();

            if (taskGetAllComputers.Exception != null)
            {
                Console.WriteLine(taskGetAllComputers.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<Computer>> resultComputers = taskGetAllComputers.Result;
            return resultComputers.Select(item => new Computer
            {
                Name = item.Object.Name,
                Description = item.Object.Description,
                Type = item.Object.Type,
                ProcessorModel = item.Object.ProcessorModel,
                RamSize = item.Object.RamSize,
                SsdSize = item.Object.SsdSize,
                Price = item.Object.Price,
                MapPoint = new MapPoint
                {
                    Latitude = item.Object.MapPoint.Latitude,
                    Longitude = item.Object.MapPoint.Longitude
                },
                ImageUrl = item.Object.ImageUrl ?? "https://www.generationsforpeace.org/wp-content/uploads/2018/03/empty.jpg",
                VideoUrl = item.Object.VideoUrl
            }).ToList();
        }
    }
}