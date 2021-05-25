using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        //public List<ComputerWithImage> Computers { get; set; }

        public ICommand RedirectToAddComputerPage { get; }

        public HomeViewModel()
        {
            RedirectToAddComputerPage = new Command(OnAddComputer);
        }

        private async void OnAddComputer(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddComputerPage));
        }
    }
}