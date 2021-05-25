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
    }
}