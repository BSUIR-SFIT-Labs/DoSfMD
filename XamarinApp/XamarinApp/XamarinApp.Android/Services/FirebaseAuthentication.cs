using System;
using System.Threading.Tasks;
using Firebase.Auth;
using XamarinApp.Services;

namespace XamarinApp.Droid.Services
{
    public class FirebaseAuthentication : IFirebaseAuthentication
    {
        public async Task<bool> RegisterWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var registrationResult = await FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                return true;
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return false;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> LoginWithEmailAndPasswordAsync(string email, string password)
        {
            try
            {
                var user = await FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                return IsSignIn();
            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return false;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool SignOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool IsSignIn()
        {
            var user = FirebaseAuth.Instance.CurrentUser;
            return user != null;
        }
    }
}