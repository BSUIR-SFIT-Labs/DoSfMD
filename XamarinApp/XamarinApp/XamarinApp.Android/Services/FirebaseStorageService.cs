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

        public async Task<string> LoadImage(Stream fileStream, string fileName, string extension)
        {
            return await _firebaseStorage
                .Child("images")
                .Child($"{fileName}.{extension}")
                .PutAsync(fileStream, CancellationToken.None, "image/jpeg");
        }

        public async Task<string> LoadVideo(Stream fileStream, string fileName, string extension)
        {
            return await _firebaseStorage
                .Child("videos")
                .Child($"{fileName}.{extension}")
                .PutAsync(fileStream, CancellationToken.None, "video/mp4");
        }

        public async Task RemoveImage(string fileName)
        {
            await _firebaseStorage
                .Child("images")
                .Child($"{fileName}.jpg")
                .DeleteAsync();
        }

        public async Task RemoveVideo(string fileName)
        {
            await _firebaseStorage
                .Child("videos")
                .Child($"{fileName}.mp4")
                .DeleteAsync();
        }
    }
}