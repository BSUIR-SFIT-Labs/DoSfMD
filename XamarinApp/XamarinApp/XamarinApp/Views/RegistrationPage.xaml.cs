using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            var registrationViewModel = new RegistrationViewModel();
            BindingContext = registrationViewModel;

            InitializeComponent();
        }
    }
}