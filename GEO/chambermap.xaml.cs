using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class chambermap : ContentPage
    {
        public chambermap(string nom, string longg, string latt)
        {
            
            
            InitializeComponent();
            namechambretxt.Text = nom;
            longtudetxt.Text = longg;
            latitudetxt.Text = latt;
            double longgg = double.Parse(longtudetxt.Text); 
            double lat = double.Parse(latitudetxt.Text); 
            Position mapPosition = new Position(longgg, lat);
            Device.BeginInvokeOnMainThread(() =>
            {
                
                formMap.MoveToRegion(MapSpan.FromCenterAndRadius(mapPosition, Distance.FromMiles(3)));

                var mapPin = new Pin
                {
                    Type = PinType.Place,
                    Position = mapPosition,
                    Label = "",
                    Address = ""
                };

                formMap.Pins.Add(mapPin);

            });


            Pin boardwalkPin = new Pin
            {
                Position = new Position(longgg, lat),
                Label = "",
                Address = "",
                Type = PinType.Place
            };
           

            Pin wharfPin = new Pin
            {
                Position = new Position(longgg, lat),
                Label = "",
                Address = "",
                Type = PinType.Place
            };
            

            formMap.Pins.Add(boardwalkPin);
            formMap.Pins.Add(wharfPin);
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
            button.BackgroundColor = Color.FromHex("#487CEA");
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
