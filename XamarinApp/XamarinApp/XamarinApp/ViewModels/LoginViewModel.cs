using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Resources;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IFirebaseAuthentication _firebaseAuthentication;
        private readonly IFirebaseDbService _firebaseDbService;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand RedirectToRegistrationPage { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RedirectToRegistrationPage = new Command(OnRedirectToRegistrationPageClicked);

            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();
        }

        private async void OnLoginClicked(object obj)
        {
            bool isAuthSuccessful = await _firebaseAuthentication.LoginWithEmailAndPasswordAsync(Email, Password);

            if (isAuthSuccessful)
            {
                var currentUser = _firebaseDbService.GetCurrentUser();

                if (currentUser.IsBlocked)
                {
                    _firebaseAuthentication.SignOut();

                    await Application.Current.MainPage.DisplayAlert("Blocked",
                        "You are blocked!", AppContentText.OkButton);
                    Application.Current.MainPage = new LoginPage();
                }
                else
                {
                    Application.Current.MainPage = new AppShell();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppContentText.LogOutErrorTitle, 
                    AppContentText.LogOutError, AppContentText.OkButton);
            }
        }

        private void OnRedirectToRegistrationPageClicked(object obj)
        {
            Application.Current.MainPage = new RegistrationPage();
        }
    }
}