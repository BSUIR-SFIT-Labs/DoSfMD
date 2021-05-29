using System;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Resources;
using XamarinApp.Services;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class ComputerDetailsViewModel : BaseViewModel
    {
        public Computer Computer { get; set; }

        private readonly IFirebaseDbService _firebaseDbService;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ProcessorModel { get; set; }
        public string RamSize { get; set; }
        public string SsdSize { get; set; }
        public string Price { get; set; }

        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public string VideoUrl { get; set; }
        public string VideoName { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public ICommand ViewImage { get; }
        public ICommand UpdateImage { get; }
        public ICommand UpdateVideo { get; }
        public ICommand ViewVideo { get; }
        public ICommand Save { get; }

        public ComputerDetailsViewModel(string id)
        {
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();
            _firebaseStorageService = DependencyService.Get<IFirebaseStorageService>();

            Computer = _firebaseDbService.GetComputerById(id);

            Id = id;
            Name = Computer.Name;
            Description = Computer.Description;
            Type = Computer.Type;
            ProcessorModel = Computer.ProcessorModel;
            RamSize = Computer.RamSize.ToString();
            SsdSize = Computer.SsdSize.ToString();
            Price = Computer.Price.ToString(CultureInfo.InvariantCulture);
            ImageUrl = Computer.Image.DownloadUrl;
            ImageName = Computer.Image.FileName;
            VideoUrl = Computer.Video.DownloadUrl;
            VideoName = Computer.Video.FileName;
            Latitude = Computer.MapPoint.Latitude.ToString(CultureInfo.InvariantCulture);
            Longitude = Computer.MapPoint.Longitude.ToString(CultureInfo.InvariantCulture);

            ViewImage = new Command(OnViewImageButtonClicked);
            UpdateImage = new Command(OnUpdateImageButtonClicked);
            UpdateVideo = new Command(OnUpdateVideoButtonClicked);
            ViewVideo = new Command(OnViewVideoButtonClicked);
            Save = new Command(OnSaveButtonClicked);
        }

        private async void OnViewImageButtonClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ViewImagePage(ImageUrl));
        }

        private async void OnUpdateImageButtonClicked()
        {
            if (!string.IsNullOrEmpty(ImageName))
            {
                await _firebaseStorageService.RemoveImage(ImageName);
            }

            var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Select image" });

            if (photo is null)
            {
                return;
            }

            string extension = photo.FileName.Split('.')[1];
            var stream = await photo.OpenReadAsync();
            ImageName = Guid.NewGuid().ToString();
            ImageUrl = await _firebaseStorageService.LoadImage(stream, ImageName, extension);
        }

        private async void OnUpdateVideoButtonClicked()
        {
            if (!string.IsNullOrEmpty(VideoName))
            {
                await _firebaseStorageService.RemoveVideo(VideoName);
            }

            var video = await MediaPicker.PickVideoAsync(new MediaPickerOptions { Title = "Select video" });

            if (video is null)
            {
                return;
            }

            string extension = video.FileName.Split('.')[1];
            var stream = await video.OpenReadAsync();
            VideoName = Guid.NewGuid().ToString();
            VideoUrl = await _firebaseStorageService.LoadVideo(stream, VideoName, extension);
        }

        private async void OnViewVideoButtonClicked()
        {
            if (!string.IsNullOrEmpty(VideoUrl))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ViewVideoPage(VideoUrl));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppContentText.NotFoundTitle,
                    AppContentText.VideoNotFoundMessage, AppContentText.OkButton);
            }
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
                    Image = new CloudFileData
                    {
                        FileName = ImageName,
                        DownloadUrl = ImageUrl
                    },
                    Video = new CloudFileData
                    {
                        FileName = VideoName,
                        DownloadUrl = VideoUrl
                    }
                };

                await _firebaseDbService.UpdateComputer(Id, computer);
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