using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IFirebaseAuthentication _firebaseAuthentication;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand RedirectToRegistrationPage { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            RedirectToRegistrationPage = new Command(OnRedirectToRegistrationPageClicked);

            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();
        }

        private async void OnLoginClicked(object obj)
        {
            bool isAuthSuccessful = await _firebaseAuthentication.LoginWithEmailAndPasswordAsync(Email, Password);

            if (isAuthSuccessful)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Authentication Failed", 
                    "Email or password are incorrect. Try again!", "OK");
            }
        }

        private void OnRedirectToRegistrationPageClicked(object obj)
        {
            Application.Current.MainPage = new RegistrationPage();
        }
    }
}