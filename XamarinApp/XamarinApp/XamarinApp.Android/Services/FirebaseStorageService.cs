using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Storage;
using XamarinApp.Services;

namespace XamarinApp.Droid.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private readonly FirebaseStorage _firebaseStorage =
            new FirebaseStorage("mobilecomputers-f4c00.appspot.com");

        public async Task<string> LoadImage(Stream fileStream, string extension)
        {
            return await _firebaseStorage
                .Child("images")
                .Child($"{Guid.NewGuid().ToString().ToUpper()}.{extension}")
                .PutAsync(fileStream, CancellationToken.None, "image/jpeg");
        }

        public async Task<string> LoadVideo(Stream fileStream, string extension)
        {
            return await _firebaseStorage
                .Child("videos")
                .Child($"{Guid.NewGuid().ToString().ToUpper()}.{extension}")
                .PutAsync(fileStream, CancellationToken.None, "video/mp4");
        }
    }
}