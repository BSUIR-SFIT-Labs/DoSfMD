using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Auth;
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

        public List<User> GetAllUsers()
        {
            var taskGetAllUsers = _databaseClient
                .Child("Users")
                .OnceAsync<User>();

            taskGetAllUsers.Wait();

            if (taskGetAllUsers.Exception != null)
            {
                Console.WriteLine(taskGetAllUsers.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<User>> resultUsers = taskGetAllUsers.Result;

            return resultUsers.Select(item => new User
            {
                Email = item.Object.Email,
                IsAdmin = item.Object.IsAdmin,
                IsBlocked = item.Object.IsBlocked
            }).ToList();
        }

        public User GetCurrentUser()
        {
            string currentUserEmail = FirebaseAuth.Instance.CurrentUser.Email;

            var taskGetAllUsers = _databaseClient
                .Child("Users")
                .OnceAsync<User>();

            taskGetAllUsers.Wait();

            if (taskGetAllUsers.Exception != null)
            {
                Console.WriteLine(taskGetAllUsers.Exception.Message);
                return null;
            }

            IEnumerable<FirebaseObject<User>> resultUsers = taskGetAllUsers.Result;

            return resultUsers.Select(item => new User
            {
                Email = item.Object.Email,
                IsAdmin = item.Object.IsAdmin,
                IsBlocked = item.Object.IsBlocked
            }).First(u => u.Email == currentUserEmail);
        }

        public async Task BanUser(string email)
        {
            var userToBan = (await _databaseClient
                .Child("Users")
                .OnceAsync<User>()).FirstOrDefault(u => u.Object.Email == email);

            var newUser = new User
            {
                Email = email,
                IsAdmin = false,
                IsBlocked = true
            };

            await _databaseClient
                .Child("Users")
                .Child(userToBan?.Key)
                .PutAsync(newUser);
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
                Id = item.Object.Id,
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
                Image = new CloudFileData
                {
                    FileName = item.Object.Image?.FileName ?? "",
                    DownloadUrl = item.Object.Image?.DownloadUrl ??
                                  "https://www.generationsforpeace.org/wp-content/uploads/2018/03/empty.jpg"
                },
                Video = new CloudFileData
                {
                    FileName = item.Object.Video?.FileName ?? "",
                    DownloadUrl = item.Object.Video?.DownloadUrl ?? ""
                }
            }).ToList();
        }

        public Computer GetComputerById(string id)
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

            return resultComputers.Where(c => c.Object.Id == id).Select(item => new Computer
            {
                Id = item.Object.Id,
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
                Image = new CloudFileData
                {
                    FileName = item.Object.Image?.FileName ?? "",
                    DownloadUrl = item.Object.Image?.DownloadUrl ??
                                  "https://www.generationsforpeace.org/wp-content/uploads/2018/03/empty.jpg"
                },
                Video = new CloudFileData
                {
                    FileName = item.Object.Video?.FileName ?? "",
                    DownloadUrl = item.Object.Video?.DownloadUrl ?? ""
                }
            }).FirstOrDefault();
        }

        public async Task UpdateComputer(string id, Computer computerDto)
        {
            var toUpdateComputer = (await _databaseClient
                .Child("Computers")
                .OnceAsync<Computer>()).FirstOrDefault(c => c.Object.Id == id);

            await _databaseClient
                .Child("Computers")
                .Child(toUpdateComputer?.Key)
                .PutAsync(computerDto);
        }
    }
}