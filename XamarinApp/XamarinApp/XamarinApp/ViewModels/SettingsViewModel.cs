using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Resources;
using XamarinApp.Services;
using XamarinApp.Settings;
using XamarinApp.Views;

namespace XamarinApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IFirebaseAuthentication _firebaseAuthentication;

        private readonly Dictionary<string, string> _languages = new Dictionary<string, string>()
        {
            ["English"] = "en-US",
            ["Russian"] = "ru-RU"
        };

        private readonly string[] _fonts = {
            "casual",
            "cursive",
            "monospace",
            "sans-serif-thin",
            "sans-serif-medium",
            "serif",
            "fantasy"
        };

        public Dictionary<string, string> Languages { get; }
        public string[] Fonts { get; set; }

        public ICommand LogOutCommand { get; }

        public string CurrentLanguage => _languages.First(x => x.Value == DefaultSettings.Language).Key;
        public string CurrentFontFamily => DefaultSettings.FontFamily;
        public string CurrentFontSize => DefaultSettings.FontSize.ToString(CultureInfo.InvariantCulture);

        public SettingsViewModel()
        {
            Languages = _languages;
            Fonts = _fonts;

            _firebaseAuthentication = DependencyService.Get<IFirebaseAuthentication>();

            LogOutCommand = new Command(OnLogOutClicked);
        }

        private async void OnLogOutClicked(object obj)
        {
            bool isLogOutSuccessful = _firebaseAuthentication.SignOut();

            if (isLogOutSuccessful)
            {
                Application.Current.MainPage = new LoginPage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(AppContentText.AuthErrorTitle,
                    AppContentText.AuthError, AppContentText.OkButton);
            }
        }
    }
}