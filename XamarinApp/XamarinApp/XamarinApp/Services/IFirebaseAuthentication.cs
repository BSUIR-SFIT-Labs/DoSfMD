using System.Threading.Tasks;

namespace XamarinApp.Services
{
    public interface IFirebaseAuthentication
    {
        Task<bool> LoginWithEmailAndPassword(string email, string password);
        bool SignOut();
        bool IsSignIn();
    }
}