using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Services;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IFirebaseAuthentication _firebaseAuthentication;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public Action DisplayInvalidLoginPrompt { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);

            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();
        }

        private async void OnLoginClicked(object obj)
        {
            bool isAuthSuccessful = await _firebaseAuthentication.LoginWithEmailAndPassword(Email, Password);

            if (isAuthSuccessful)
            {
                Application.Current.MainPage = new AppShell();
            }
            else
            {
                DisplayInvalidLoginPrompt();
            }
        }
    }
}