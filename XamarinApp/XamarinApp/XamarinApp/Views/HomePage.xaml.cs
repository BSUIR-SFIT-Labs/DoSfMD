using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Models;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public async void OnItemClicked(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new ComputerDetailsPage(((Computer)e.Item).Id));
        }
    }
}