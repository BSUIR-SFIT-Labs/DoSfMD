using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Services;
using XamarinApp.Settings;
using XamarinApp.ViewModels;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private readonly SettingsViewModel _settingsViewModel;
        private readonly IFirebaseDbService _firebaseDbService;

        public SettingsPage()
        {
            _settingsViewModel = new SettingsViewModel();
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();

            InitializeComponent();

            AdminPanel.IsVisible = _settingsViewModel.CurrentUser.IsAdmin;

            foreach (string language in _settingsViewModel.Languages.Keys)
            {
                LanguagePicker.Items.Add(language);
            }

            foreach (string font in _settingsViewModel.Fonts)
            {
                FontFamilyPicker.Items.Add(font);
            }

            for (int i = 1; i <= 25; i++)
            {
                FontSizePicker.Items.Add(i.ToString());
            }

            foreach (string emailToBan in _settingsViewModel.UserEmails)
            {
                UserPicker.Items.Add(emailToBan);
            }
        }

        public void OnLanguageChanged(object sender, EventArgs e)
        {
            if (sender != null && (sender as Picker).SelectedItem != _settingsViewModel.CurrentLanguage)
            {
                DefaultSettings.Language = _settingsViewModel.Languages[LanguagePicker.Items[LanguagePicker.SelectedIndex]];
                Preferences.Set("Language", DefaultSettings.Language);
                MessagingCenter.Send(this, "LanguageChanged");
            }
        }

        public void OnFontFamilyChanged(object sender, EventArgs e)
        {
            if (sender != null && (sender as Picker).SelectedItem != _settingsViewModel.CurrentFontFamily)
            {
                DefaultSettings.FontFamily = FontFamilyPicker.Items[FontFamilyPicker.SelectedIndex];
                Preferences.Set("FontFamily", DefaultSettings.FontFamily);
                MessagingCenter.Send(this, "FontFamilyChanged");
            }
        }

        public void OnFontSizeChanged(object sender, EventArgs e)
        {
            if (sender != null && (sender as Picker).SelectedItem != _settingsViewModel.CurrentFontSize)
            {
                DefaultSettings.FontSize = double.Parse(FontSizePicker.Items[FontSizePicker.SelectedIndex]);
                Preferences.Set("FontSize", DefaultSettings.FontSize);
                MessagingCenter.Send(this, "FontSizeChanged");
            }
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            string userEmailToBan = (string) UserPicker.SelectedItem;

            if (string.IsNullOrWhiteSpace(userEmailToBan))
            {
                return;
            }

            _firebaseDbService.BanUser(userEmailToBan);
        }
    }
}