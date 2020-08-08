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
         string cb1;
        string cb2;
        string cb3;


        public addchamber()
        {
            InitializeComponent();
            cable1Picker.ItemsSource = getcableToPicker();
            cable2Picker.ItemsSource = getcableToPicker();
            cable3Picker.ItemsSource = getcableToPicker();
            Position position = new Position(34.751440, 10.675077);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            Map map = new Map(mapSpan);

        }

      public void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(nomch_txt.Text) && !string.IsNullOrEmpty(longitude_txt.Text) && !string.IsNullOrEmpty(latitude_txt.Text) && !string.IsNullOrEmpty(cable1Picker.SelectedItem.ToString()))
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        double longitude = double.Parse(longitude_txt.Text.ToString());
                        double latitude = double.Parse(latitude_txt.Text.ToString());
                        con.Open();
                        String st = "INSERT INTO [chambre] ([name_Chambre],[Longitude],[Latitude],[cable1],[cable2],[cable3]) values (@name_chambre,@longitude,@latitude,@cable1,@cable2,@cable3)";
                        SqlCommand cmd = new SqlCommand(st, con);
                        cmd.Parameters.AddWithValue("@name_chambre", nomch_txt.Text);
                        cmd.Parameters.AddWithValue("@longitude", longitude); 
                        cmd.Parameters.AddWithValue("@latitude", latitude);
                        cmd.Parameters.AddWithValue("@cable1",cb1.ToString());
                        cmd.Parameters.AddWithValue("@cable2", cb2.ToString());
                        cmd.Parameters.AddWithValue("@cable3", cb3.ToString()); 
                        cmd.ExecuteNonQuery();
                        con.Close();
                        
                        DisplayAlert("", "chambre ajoutée", "ok");
                        App.Current.MainPage = new Chambers();
                    }
                else
                {
                    DisplayAlert("", "remplir tous le formulaire", "Ok");

                }
            }
            catch(Exception p)
            {

                DisplayAlert("", p.Message, "ok");
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

        public void cable1Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (mcable)picker.SelectedItem;
            cb1  = selectedCategory.all;
        }

       public void cable2Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (mcable)picker.SelectedItem;
             cb2 = selectedCategory.all;

        }

       public void cable3Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (mcable)picker.SelectedItem;
             cb3 = selectedCategory.all;
        }
        public List<mcable> getcableToPicker()
        {
            List<mcable> cableList = new List<mcable>();
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select * from [dbo].[cable]", con);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    mcable u = new mcable();
                    u.Nom_Cable = reader["Nom_Cable"].ToString();
                    u.Nombre_brin = reader["Nombre_brin"].ToString();
                    u.all = u.Nom_Cable + u.Nombre_brin;



                    cableList.Add(u);
                }
                return cableList;
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new cablage());
        }
    }
}