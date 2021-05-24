using Xamarin.Forms;
using XamarinApp.Settings;

namespace XamarinApp.Views
{
    public class ListCell : ViewCell
    {
        Label LblName;
        Label LblDescription;
        Label LblPrice;
        Image ImgComputer;

        public ListCell()
        {
            LblName = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                Padding = 10
            };

            LblDescription = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Padding = 10
            };

            LblPrice = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                Padding = 10
            };

            ImgComputer = new Image
            {
                HeightRequest = 75,
                WidthRequest = 75,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Margin = 10
            };

            var cell = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            var title = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            var head = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            head.Children.Add(LblName);
            title.Children.Add(LblDescription);
            cell.Children.Add(ImgComputer);
            cell.Children.Add(LblPrice);

            View = cell;
        }

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }

        public string Description
        {
            get => (string)GetValue(DescriptionProperty);
            set => SetValue(DescriptionProperty, value);
        }

        public int Price
        {
            get => (int)GetValue(PriceProperty);
            set => SetValue(PriceProperty, value);
        }

        public string ComputerId
        {
            get => (string)GetValue(ComputerIdProperty);
            set => SetValue(ComputerIdProperty, value);
        }

        public ImageSource Image
        {
            get => (ImageSource)GetValue(ImageProperty);
            set => SetValue(ImageProperty, value);
        }

        public static readonly BindableProperty NameProperty = BindableProperty.Create($"{nameof(Name)}", typeof(string), typeof(ListCell), "");
        public static readonly BindableProperty DescriptionProperty = BindableProperty.Create($"{nameof(Description)}", typeof(string), typeof(ListCell), "");
        public static readonly BindableProperty PriceProperty = BindableProperty.Create($"{nameof(Price)}", typeof(int), typeof(ListCell), 0);
        public static readonly BindableProperty ComputerIdProperty = BindableProperty.Create($"{nameof(ComputerId)}", typeof(string), typeof(ListCell), "");
        public static readonly BindableProperty ImageProperty = BindableProperty.Create($"{nameof(Image)}", typeof(ImageSource), typeof(ListCell), null);

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                LblName.Text = Name;
                LblDescription.Text = Description;
                LblPrice.Text = Price.ToString();
                ImgComputer.Source = Image;
                LblName.FontFamily = DefaultSettings.FontFamily;
                LblDescription.FontFamily = DefaultSettings.FontFamily;
                LblPrice.FontFamily = DefaultSettings.FontFamily;
                LblName.FontSize = DefaultSettings.FontSize;
                LblDescription.FontSize = DefaultSettings.FontSize;
                LblPrice.FontSize = DefaultSettings.FontSize;
            }
        }
    }
}