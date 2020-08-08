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
    public partial class UpdateChamber : ContentPage
    {
        string cb1;
        string cb2;
        string cb3;

        public UpdateChamber(int id,string nom, double lon, double lat, string cable1, string cable2, string cable3)
        {
            InitializeComponent();
            idtxt.Text = id.ToString();
            namechambretxt.Text =nom ;
            longtudetxt.Text = lon.ToString();
            latitudetxt.Text = lat.ToString();
            cable1Picker.SelectedItem = cable1;
            cable1Picker.SelectedItem = cable2;
            cable1Picker.SelectedItem = cable3;
            cable1Picker.ItemsSource = getcableToPicker();
            cable2Picker.ItemsSource = getcableToPicker();
            cable3Picker.ItemsSource = getcableToPicker();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(idtxt.Text) ||
                  string.IsNullOrEmpty(longtudetxt.Text) ||
                  string.IsNullOrEmpty(latitudetxt.Text) ||
                  string.IsNullOrEmpty(namechambretxt.Text)
                   )
                {
                    DisplayAlert("Oops", "", "Ok");
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        Int32 key = Convert.ToInt32(idtxt.Text);
                        double longitude = double.Parse(longtudetxt.Text.ToString());
                        double latitude = double.Parse(latitudetxt.Text.ToString());
                        SqlCommand command = new SqlCommand
                            (
                            "update  [dbo].[Chambre] set [name_Chambre]=@name,[Longitude]=@lo,[Latitude]=@la,[cable1]=@c1,[cable2]=@c2,[cable3]=@c3 where [id_Chambre]=@id",
                            con
                            );
                        command.Parameters.AddWithValue("@id", key);
                        command.Parameters.AddWithValue("@name", namechambretxt.Text);
                        command.Parameters.AddWithValue("@lo", longitude);
                        command.Parameters.AddWithValue("@la", latitude);
                        command.Parameters.AddWithValue("@c1", cb1.ToString());
                        command.Parameters.AddWithValue("@c2", cb2.ToString());
                        command.Parameters.AddWithValue("@c3", cb3.ToString());
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("", "chambre modifiée", "Ok");
                        Application.Current.MainPage = new NavigationPage(new MasterDetailPage1());
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayAlert("Oops", exp.Message, "ok");
            }
        }

        private void formMap_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            formMap.Pins.Clear();
            double latt = e.Position.Latitude;
            double longg = e.Position.Longitude;
            latitudetxt.Text = longg.ToString();
            longtudetxt.Text = latt.ToString();

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

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new cablage());
        }

        private void cable1Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (mcable)picker.SelectedItem;
            cb1 = selectedCategory.all;
        }

        private void cable2Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (mcable)picker.SelectedItem;
            cb2 = selectedCategory.all;
        }

        private void cable3Picker_SelectedIndexChanged(object sender, EventArgs e)
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

      
    }
}