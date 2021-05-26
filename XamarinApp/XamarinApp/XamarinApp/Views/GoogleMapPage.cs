using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using XamarinApp.Models;
using XamarinApp.Services;

namespace XamarinApp.Views
{
    public class GoogleMapPage : ContentPage
    {
        private readonly IFirebaseDbService _firebaseDbService;

        public List<Computer> Computers { get; set; }

        public GoogleMapPage()
        {
            _firebaseDbService = DependencyService.Get<IFirebaseDbService>();

            Computers = _firebaseDbService.GetAllComputers();

            var map = new Map
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            Content = new StackLayout
            {
                Children =
                {
                    map
                }
            };

            if (Computers is null)
            {
                return;
            }

            if (Computers.Count > 0)
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Position(Computers[0].MapPoint.Latitude, Computers[0].MapPoint.Longitude),
                    Distance.FromKilometers(300)));
            }

            foreach (var computer in Computers)
            {
                var pin = new Pin()
                {
                    Label = computer.Name + " | " + computer.Description,
                    Position = new Position(computer.MapPoint.Latitude, computer.MapPoint.Longitude)
                };
                map.Pins.Add(pin);
            }
        }
    }
}