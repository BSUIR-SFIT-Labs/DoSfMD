using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewVideoPage : ContentPage
    {
        public ViewVideoPage(string videoUrl)
        {
            var viewModel = new ViewVideoViewModel();
            viewModel.Video = videoUrl;
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}