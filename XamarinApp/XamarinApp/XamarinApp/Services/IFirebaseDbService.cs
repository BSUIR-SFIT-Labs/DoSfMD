using System.Threading.Tasks;
using XamarinApp.Models;

namespace XamarinApp.Services
{
    public interface IFirebaseDbService
    {
        Task AddUserInfo(User userDto);
    }
}