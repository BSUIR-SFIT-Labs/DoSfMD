using Xamarin.Forms;
using XamarinApp.Settings;

namespace XamarinApp.Views
{
    public class ListCell : ViewCell
    {
        private readonly Label _lblName;
        private readonly Label _lblDescription;
        private readonly Label _lblPrice;
        private readonly Image _imgComputer;

        public ListCell()
        {
            _lblName = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(5, 10, 0, 0)
            };

            _lblDescription = new Label
            {
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Padding = new Thickness(0, 10, 5, 5)
            };

            _lblPrice = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(5, 5, 0, 10)
            };

            _imgComputer = new Image
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

            var head = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var content = new StackLayout
            {
                Orientation = StackOrientation.Vertical
            };

            head.Children.Add(_lblName);
            head.Children.Add(_lblDescription);
            content.Children.Add(head);
            content.Children.Add(_lblPrice);
            cell.Children.Add(_imgComputer);
            cell.Children.Add(content);

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

        public decimal Price
        {
            get => (decimal)GetValue(PriceProperty);
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
        public static readonly BindableProperty PriceProperty = BindableProperty.Create($"{nameof(Price)}", typeof(decimal), typeof(ListCell), 0.00m);
        public static readonly BindableProperty ComputerIdProperty = BindableProperty.Create($"{nameof(ComputerId)}", typeof(string), typeof(ListCell), "");
        public static readonly BindableProperty ImageProperty = BindableProperty.Create($"{nameof(Image)}", typeof(ImageSource), typeof(ListCell), null);

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                _lblName.Text = Name + " | ";
                _lblDescription.Text = Description;
                _lblPrice.Text = Price.ToString();
                _imgComputer.Source = Image;
                _lblName.FontFamily = DefaultSettings.FontFamily;
                _lblDescription.FontFamily = DefaultSettings.FontFamily;
                _lblPrice.FontFamily = DefaultSettings.FontFamily;
                _lblName.FontSize = DefaultSettings.FontSize;
                _lblDescription.FontSize = DefaultSettings.FontSize;
                _lblPrice.FontSize = DefaultSettings.FontSize;
            }
        }
    }
}