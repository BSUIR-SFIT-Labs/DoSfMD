using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Resources;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class AddComputerViewModel : BaseViewModel
    {
        private readonly IFirebaseDbService _firebaseDbService;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ProcessorModel { get; set; }
        public string RamSize { get; set; }
        public string SsdSize { get; set; }
        public string Price { get; set; }
        
        public string ImageUrl { get; set; }
        public string VideoUrl { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public ICommand AddImage { get; }
        public ICommand AddVideo { get; }
        public ICommand Save { get; }

        public AddComputerViewModel()
        {
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();
            _firebaseStorageService = DependencyService.Get<IFirebaseStorageService>();

            Save = new Command(OnSaveButtonClicked);
            AddImage = new Command(OnAddImageButtonClicked);
            AddVideo = new Command(OnAddVideoButtonClicked);
        }

        private async void OnAddImageButtonClicked()
        {
            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Select image" });

            if (photo is null)
            {
                return;
            }

            string extension = photo.FileName.Split('.')[1];
            var stream = await photo.OpenReadAsync();
            ImageUrl = await _firebaseStorageService.LoadImage(stream, extension);
        }

        private async void OnAddVideoButtonClicked()
        {
            var video = await MediaPicker.PickVideoAsync(new MediaPickerOptions { Title = "Select video" });

            if (video is null)
            {
                return;
            }

            string extension = video.FileName.Split('.')[1];
            var stream = await video.OpenReadAsync();
            VideoUrl = await _firebaseStorageService.LoadVideo(stream, extension);
        }

        private async void OnSaveButtonClicked()
        {
            if (IsCorrectFields())
            {
                var computer = new Computer
                {
                    Name = Name,
                    Description = Description,
                    Type = Type,
                    ProcessorModel = ProcessorModel,
                    RamSize = int.Parse(RamSize),
                    SsdSize = int.Parse(SsdSize),
                    Price = decimal.Parse(Price),
                    MapPoint = new MapPoint
                    {
                        Latitude = double.Parse(Latitude),
                        Longitude = double.Parse(Longitude)
                    },
                    ImageUrl = ImageUrl,
                    VideoUrl = VideoUrl
                };

                await _firebaseDbService.AddComputer(computer);
                await Shell.Current.GoToAsync(nameof(HomePage));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppContentText.IncorrectFieldsTitle,
                    AppContentText.IncorrectFieldsMessage, AppContentText.OkButton);
            }
        }

        private bool IsCorrectFields()
        {
            if (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(Description) ||
                !string.IsNullOrWhiteSpace(Type) || !string.IsNullOrWhiteSpace(ProcessorModel))
            {
                try
                {
                    _ = int.Parse(RamSize);
                    _ = int.Parse(SsdSize);
                    _ = decimal.Parse(Price);
                    _ = double.Parse(Latitude);
                    _ = double.Parse(Longitude);

                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}