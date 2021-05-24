using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XamarinApp.Settings;

namespace XamarinApp.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
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

        public string CurrentLanguage => _languages.First(x => x.Value == DefaultSettings.Language).Key;
        public string CurrentFontFamily => DefaultSettings.FontFamily;
        public string CurrentFontSize => DefaultSettings.FontSize.ToString(CultureInfo.InvariantCulture);

        public SettingsViewModel()
        {
            Languages = _languages;
            Fonts = _fonts;
        }
    }
}