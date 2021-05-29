using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Resources;
using XamarinApp.Settings;

namespace XamarinApp.Services
{
    [ContentProperty("Text")]
    public class LanguageService : IMarkupExtension
    {
        private const string LanguageResource = "XamarinApp.Resources.AppContentText";
        private readonly CultureInfo _cultureInfo;

        public string Text { get; set; }

        public LanguageService()
        {
            _cultureInfo = new CultureInfo(DefaultSettings.Language);
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
            {
                return "";
            }

            var resourceManager = new ResourceManager(LanguageResource, typeof(LanguageService).GetTypeInfo().Assembly);

            string translation = resourceManager.GetString(Text, _cultureInfo);

            if (translation == null)
            {
                translation = Text;
            }

            return translation;
        }
    }
}