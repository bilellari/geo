using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChambreDetailAndMap : ContentPage
    {
        private Chambre Chambre;
        public ChambreDetailAndMap(Chambre chambre)
        {
            InitializeComponent();
            this.Chambre = chambre;
            btnTakeToLocation_Clicked(null, null);
        }

        private void btnTakeToLocation_Clicked(object sender, EventArgs e)
        {
            double lat = Chambre.Latitude;
            double lon = Chambre.Longitude;
            Position mapPosition = new Position(lon, lat);
            Device.BeginInvokeOnMainThread(() =>
            {

                formMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(3)));

                var mapPin = new Pin
                {
                    Type = PinType.Place,
                    Position = mapPosition,
                    Label = "city",
                    Address = "country"
                };

                formMap.Pins.Add(mapPin);

            });


            Pin boardwalkPin = new Pin
            {
                Position = new Position(lon, lat),
                Label = "city",
                Address = "country",
                Type = PinType.Place
            };
            boardwalkPin.MarkerClicked += OnMarkerClickedAsync;

            Pin wharfPin = new Pin
            {
                Position = new Position(lon, lat),
                Label = "city",
                Address = "country",
                Type = PinType.Place
            };
            wharfPin.InfoWindowClicked += OnInfoWindowClickedAsync;

            formMap.Pins.Add(boardwalkPin);
            formMap.Pins.Add(wharfPin);
        }

        async void OnMarkerClickedAsync(object sender, PinClickedEventArgs e)
        {
            e.HideInfoWindow = true;
            string pinName = ((Pin)sender).Label;
            await DisplayAlert("Pin Clicked", $"{pinName} was clicked.", "Ok");
        }

        async void OnInfoWindowClickedAsync(object sender, PinClickedEventArgs e)
        {
            string pinName = ((Pin)sender).Label;
            await DisplayAlert("Info Window Clicked", $"The info window was clicked for {pinName}.", "Ok");
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (formMap.VisibleRegion != null)
            {
                formMap.MoveToRegion(new MapSpan(formMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = sender as Button;
            switch (button.Text)
            {
                case "Street":
                    formMap.MapType = MapType.Street;
                    break;
                case "Satellite":
                    formMap.MapType = MapType.Satellite;
                    break;
                case "Hybrid":
                    formMap.MapType = MapType.Hybrid;
                    break;
            }
        }

    }
}