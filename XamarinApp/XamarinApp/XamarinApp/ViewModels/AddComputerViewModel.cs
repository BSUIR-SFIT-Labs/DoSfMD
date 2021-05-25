using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinApp.ViewModels
{
    public class AddComputerViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ProcessorModel { get; set; }
        public string RamSize { get; set; }
        public string SsdSize { get; set; }
        public string Price { get; set; }
        public ImageSource Image { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public ICommand Save { get; }
        public ICommand AddImage { get; }
        public ICommand AddVideo { get; }
        public ICommand ShowVideo { get; }

        public AddComputerViewModel()
        {
            Save = new Command(OnSaveButtonClicked);
            AddImage = new Command(OnAddImageButtonClicked);
            AddVideo = new Command(OnAddVideoButtonClicked);
            ShowVideo = new Command(OnShowVideoButtonClicked);
        }

        private void OnSaveButtonClicked()
        {
           
        }


        private void OnAddImageButtonClicked()
        {
           
        }

        private void OnAddVideoButtonClicked()
        {
            
        }

        private void OnShowVideoButtonClicked()
        {

        }
    }
}
