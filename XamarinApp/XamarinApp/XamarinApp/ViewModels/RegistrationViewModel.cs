using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly IFirebaseAuthentication _firebaseAuthentication;

        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }

        public ICommand Register { get; }
        public ICommand RedirectToLoginPage { get; }

        public Action DisplayInvalidRegistrationPrompt { get; set; }

        public RegistrationViewModel()
        {
            Register = new Command(OnRegisterClicked);
            RedirectToLoginPage = new Command(OnRedirectToLoginPageClicked);

            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();
        }

        private async void OnRegisterClicked(object obj)
        {
            
        }

        private void OnRedirectToLoginPageClicked(object obj)
        {
            Application.Current.MainPage = new LoginPage();
        }
    }
}