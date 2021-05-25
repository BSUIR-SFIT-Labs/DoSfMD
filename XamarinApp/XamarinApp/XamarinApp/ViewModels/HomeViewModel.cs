using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public List<Computer> Computers { get; set; }

        public ICommand RedirectToAddComputerPage { get; }

        public HomeViewModel()
        {
            var firebaseDbService = DependencyService.Get<IFirebaseDbService>();

            RedirectToAddComputerPage = new Command(OnAddComputer);

            Computers = firebaseDbService.GetAllComputers();
        }

        private async void OnAddComputer(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddComputerPage));
        }
    }
}