using System.IO;
using System.Threading.Tasks;

namespace XamarinApp.Services
{
    public interface IFirebaseStorageService
    {
        Task<string> LoadImage(Stream fileStream, string extension);

        Task<string> LoadVideo(Stream fileStream, string extension);
    }
}