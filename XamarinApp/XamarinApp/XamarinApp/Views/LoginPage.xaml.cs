using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            var loginViewModel = new LoginViewModel();
            BindingContext = loginViewModel;
            loginViewModel.DisplayInvalidLoginPrompt += () => DisplayAlert("Authentication Failed", "Email or password are incorrect. Try again!", "OK");

            InitializeComponent();
        }
    }
}