using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Models;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ComputerDetailsPage : ContentPage
    {
        public ComputerDetailsPage(string id)
        {
            var viewModel = new ComputerDetailsViewModel(id);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}