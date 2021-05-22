using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            registrationViewModel.DisplayInvalidRegistrationPrompt += () => DisplayAlert("Registration Failed", "Email or passwords are incorrect. Try again!", "OK");

            InitializeComponent();
        }
    }
}