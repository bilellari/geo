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
    public partial class addchamber : ContentPage
    {
        public addchamber()
        {
            InitializeComponent();
            Position position = new Position(34.751440, 10.675077);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            Map map = new Map(mapSpan);

        }

      public void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(nomch_txt.Text) && !string.IsNullOrEmpty(longitude_txt.Text) && !string.IsNullOrEmpty(latitude_txt.Text))
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        double longitude = double.Parse(longitude_txt.Text.ToString());
                        double latitude = double.Parse(latitude_txt.Text.ToString());
                        con.Open();
                        String st = "INSERT INTO [chambre] ([name_Chambre],[Longitude],[Latitude]) values (@name_chambre,@longitude,@latitude)";
                        SqlCommand cmd = new SqlCommand(st, con);
                        cmd.Parameters.AddWithValue("@name_chambre", nomch_txt.Text);
                        cmd.Parameters.AddWithValue("@longitude", longitude);
                        cmd.Parameters.AddWithValue("@latitude", latitude);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        
                        DisplayAlert("", "chambre ajoutée", "ok");
                        longitude_txt.Text = string.Empty;
                        nomch_txt.Text = string.Empty;
                        latitude_txt.Text = string.Empty;
                    }
                else
                {
                    DisplayAlert("", "remplir tous le formulaire", "Ok");

                }
            }
            catch
            {

                DisplayAlert("", "erreur", "ok");
            }
        }

        private void formMap_MapClicked(object sender, MapClickedEventArgs e)
        {
            formMap.Pins.Clear();
            double latt = e.Position.Latitude;
            double longg = e.Position.Longitude;
            latitude_txt.Text = longg.ToString();
            longitude_txt.Text = latt.ToString();

            Position mapPosition = new Position(latt, longg);
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
                Position = new Position(latt, longg),
                Label = "",
                Address = "",
                Type = PinType.Place
            };
            boardwalkPin.MarkerClicked += OnMarkerClickedAsync;

            Pin wharfPin = new Pin
            {
                Position = new Position(latt, longg),
                Label = "",
                Address = "",
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
            await DisplayAlert("", $"{pinName} .", "Ok");
        }

        async void OnInfoWindowClickedAsync(object sender, PinClickedEventArgs e)
        {
            string pinName = ((Pin)sender).Label;
            await DisplayAlert("", $" {pinName}.", "Ok");
        }

        void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            double zoomLevel = e.NewValue;
            double latlongDegrees = 360 / (Math.Pow(2, zoomLevel));
            if (formMap.VisibleRegion != null)
            {

            }
        }
    }
}