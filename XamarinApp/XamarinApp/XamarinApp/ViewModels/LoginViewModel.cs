using System;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public Action DisplayInvalidLoginPrompt { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private void OnLoginClicked(object obj)
        {
            DisplayInvalidLoginPrompt();
        }
    }
}