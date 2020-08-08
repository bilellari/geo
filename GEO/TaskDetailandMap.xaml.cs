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
        public class MyTache
        {

            public int id { get; set; }
            public string technichian { get; set; }
            public string chambre { get; set; }
            public string TaskDescription { get; set; }
            public bool status { get; set; }
            public Color BackgroundColor { get; internal set; }
            public bool isAdded { get; set; } = false;

        }
        double lo, la;
        public TaskDetailandMap(int idcame)
        {
            InitializeComponent();
            getTask(idcame);
            getLongtudeandLatitude();
            Position position = new Position(34.751440, 10.675077);
            formMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromMiles(3)));
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
                    string changedlongtude = lo.ToString();
                    string changedlatitude = la.ToString();
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
                    Label = "",
                    Address = ""
                };

                formMap.Pins.Add(mapPin);

            });


            Pin boardwalkPin = new Pin
            {
                Position = new Position(lon, lat),
                Label = "",
                Address = "",
                Type = PinType.Place
            };
            boardwalkPin.MarkerClicked += OnMarkerClickedAsync;

            Pin wharfPin = new Pin
            {
                Position = new Position(lon, lat),
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
                formMap.MoveToRegion(new MapSpan(formMap.VisibleRegion.Center, latlongDegrees, latlongDegrees));
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            MyTache c = new MyTache();
            c.status = false;
            DisplayAlert("", "la tache est marqueé finie", "ok");
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