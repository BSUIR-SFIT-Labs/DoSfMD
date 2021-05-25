using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddComputerPage : ContentPage
    {
        public AddComputerPage()
        {
            InitializeComponent();
        }

        public void OnImageClick(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new Gallery(smartphone));
        }

        public void OnMapClick(object sender, EventArgs e)
        {
            //CommonSmart.smartphones = new List<Smartphones>() { this.smartphone };
            //Navigation.PushAsync(new GoogleMapPage());
        }
    }
}