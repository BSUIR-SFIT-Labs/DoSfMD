namespace XamarinApp.Settings
{
    public class DefaultSettings
    {
        public static string FontFamily { get; set; }
        public static string Language { get; set; }
        public static double FontSize { get; set; }

        public DefaultSettings()
        {
            FontFamily = "serif";
            FontSize = 18;
        }
    }
}