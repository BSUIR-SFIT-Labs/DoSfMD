using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.Settings;

namespace XamarinApp.Services
{
    [ContentProperty("FontFamily")]
    public class FontFamilyService : IMarkupExtension
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return DefaultSettings.FontFamily;
        }
    }
}