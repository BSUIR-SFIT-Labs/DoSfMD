using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewImagePage : ContentPage
    {
        public ViewImagePage(string imageUrl)
        {
            var viewModel = new ViewImageViewModel();
            viewModel.Image = imageUrl;
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}