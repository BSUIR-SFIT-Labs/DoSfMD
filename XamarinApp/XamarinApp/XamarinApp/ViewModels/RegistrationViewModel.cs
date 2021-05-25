using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Resources;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IFirebaseDbService _firebaseDbService;
        private readonly IFirebaseAuthentication _firebaseAuthentication;

        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }

        public ICommand Register { get; }
        public ICommand RedirectToLoginPage { get; }

        public RegistrationViewModel()
        {
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();
            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();

            Register = new Command(OnRegisterClicked);
            RedirectToLoginPage = new Command(OnRedirectToLoginPageClicked);
        }

        private async void OnRegisterClicked(object obj)
        {
            if (Password == RePassword)
            {
                bool isRegistrationSuccessful = await _firebaseAuthentication.RegisterWithEmailAndPasswordAsync(Email, Password);

                if (isRegistrationSuccessful)
                {
                    var user = new User
                    {
                        Email = Email,
                        IsAdmin = false,
                        IsBlocked = false
                    };

                    await _firebaseDbService.AddUserInfo(user);

                    Application.Current.MainPage = new LoginPage();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert(AppContentText.RegistrationErrorTitle,
                        AppContentText.RegistrationError, AppContentText.OkButton);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppContentText.RegistrationErrorTitle,
                    AppContentText.PasswordsError, AppContentText.OkButton);
            }
        }

        private void OnRedirectToLoginPageClicked(object obj)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}