using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaskDetailandMap : ContentPage
    {
        double lo, la;
        public TaskDetailandMap(int idcame)
        {
            InitializeComponent();
            getTask(idcame);
            getLongtudeandLatitude();
        }
        public void getTask(int id)
        {
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select * from [TaskforTech] where id=@id", con);
                command.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lblChamber.Text = reader["chambre"].ToString();
                    lblTask.Text = reader["taskdescription"].ToString();
                }
            }
        }

        public void getLongtudeandLatitude()
        {
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select* from[dbo].[Chambre] where[name_Chambre] = @namechambre", con);
                command.Parameters.AddWithValue("@namechambre", lblChamber.Text);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lo = double.Parse(reader["Longitude"].ToString());
                    la = double.Parse( reader["Latitude"].ToString());
                    string changedlongtude = lo.ToString("#,##0.00");
                    string changedlatitude = la.ToString("#,##0.00");
                    longtudetxt.Text = changedlongtude.ToString();
                    latitudetxt.Text = changedlatitude.ToString();                }
            }
        }

        private void btnTakeToLocation_Clicked(object sender, EventArgs e)
        {
            double lat = double.Parse(latitudetxt.Text.ToString());
            double lon = double.Parse(longtudetxt.Text.ToString());
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