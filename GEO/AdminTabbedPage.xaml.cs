using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminTabbedPage : TabbedPage
    {
        private string ch = string.Empty;
        private string tech = string.Empty;
        

        public AdminTabbedPage()
        {
            
            InitializeComponent();
            Position position = new Position(34.751440, 10.675077);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            Map map = new Map(mapSpan);

            ////this method refresh the page evry 10 second
            //Device.StartTimer(TimeSpan.FromSeconds(10), () => {
            //    // If you want to update UI, make sure its on the on the
            //    Device.BeginInvokeOnMainThread(() => getchamberToPicker());
            //    return true;
            //});
            //Device.StartTimer(TimeSpan.FromSeconds(10), () => {
            //    // If you want to update UI, make sure its on the on the
            //    Device.BeginInvokeOnMainThread(() => getTechnichianToPicker());
            //    return true;
            //});
            TechnichianPicker.ItemsSource = getTechnichianToPicker();
            chambrePicker.ItemsSource = getchamberToPicker();
            ChambreListView.ItemsSource = getChambre();
            userListView.ItemsSource = GetUsers();
            ChambreListView.RefreshCommand = new Command(() =>
           {
               ChambreListView.ItemsSource = getChambre();
               ChambreListView.IsRefreshing = false;

           });
            userListView.RefreshCommand = new Command(() =>
             {
                 userListView.ItemsSource = GetUsers();
                 userListView.IsRefreshing = false;
             });
        }

        public object ChamberNameEntry { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            getchamberToPicker();
            getTechnichianToPicker();
        }
       
        private void Add_1(object sender, EventArgs e)
        {
            //we need to get the pin here while adding chamber
           
            try
            {
                if (!string.IsNullOrEmpty(nom_ch_txt.Text) && !string.IsNullOrEmpty(longitude_txt.Text) && !string.IsNullOrEmpty(latitude_txt.Text))
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        double longitude = double.Parse(longitude_txt.Text.ToString());
                        double latitude = double.Parse(latitude_txt.Text.ToString());
                        con.Open();
                        String st = "INSERT INTO [chambre] ([name_Chambre],[Longitude],[Latitude]) values (@name_chambre,@longitude,@latitude)";
                        SqlCommand cmd = new SqlCommand(st, con);
                        cmd.Parameters.AddWithValue("@name_chambre", nom_ch_txt.Text);
                        cmd.Parameters.AddWithValue("@longitude", longitude);
                        cmd.Parameters.AddWithValue("@latitude", latitude);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Application.Current.MainPage=new NavigationPage(new AdminTabbedPage());
                        DisplayAlert("", "chambre ajoutée", "ok");
                        longitude_txt.Text = string.Empty;
                        nom_ch_txt.Text = string.Empty;
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

       
                public IEnumerable<Chambre> getChambre()
        {
            List<Chambre> chambrelist = new List<Chambre>();
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select * from [dbo].[Chambre]", con);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Chambre c = new Chambre();
                    c.id_Chambre = Convert.ToInt32(reader["id_Chambre"].ToString());
                    c.name_Chambre = reader["name_Chambre"].ToString();
                    c.Longitude = double.Parse(reader["Longitude"].ToString());
                    c.Latitude = double.Parse(reader["Latitude"].ToString());
                    chambrelist.Add(c);
                }
                return chambrelist;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idtxt.Text))
            {
                DisplayAlert("Oops", "choisissez la chambre que vous souhaitez supprimer", "Ok");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    Int32 key = Convert.ToInt32(idtxt.Text);
                    SqlCommand command = new SqlCommand
                        ("delete from [dbo].[Chambre] where [id_Chambre]=@id", con);
                    command.Parameters.AddWithValue("@id", key);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    DisplayAlert("", "chambre supprimée", "Ok");
                }
            }
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(idtxt.Text) ||
              string.IsNullOrEmpty(longtudetxt.Text) ||
              string.IsNullOrEmpty(latitudetxt.Text) ||
              string.IsNullOrEmpty(namechambretxt.Text)
              )
                {
                    DisplayAlert("", "choisissez la chambre que vous souhaitez modifier", "Ok");
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
                            "update  [dbo].[Chambre] set [name_Chambre]=@name,[Longitude]=@lo,[Latitude]=@la where [id_Chambre]=@id",
                            con
                            );
                        command.Parameters.AddWithValue("@id", key);
                        command.Parameters.AddWithValue("@name", namechambretxt.Text);
                        command.Parameters.AddWithValue("@lo", longitude);
                        command.Parameters.AddWithValue("@la", latitude);
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("", "chambre modifiée", "Ok");
                    }
                }
            }
            catch (Exception exp)
            {
                DisplayAlert("Oops", exp.Message, "ok");
            }
            //we miss something here look

        }



        private void ChambreListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (Chambre)e.Item;
            Int32 id = Convert.ToInt32(item.id_Chambre);
            double lomgtude = double.Parse(item.Longitude.ToString());
            double latitude = double.Parse(item.Latitude.ToString());
            idtxt.Text = id.ToString();
            namechambretxt.Text = item.name_Chambre;
            longtudetxt.Text = lomgtude.ToString();
            latitudetxt.Text = latitude.ToString();
        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(connect.c))
                {


                    con.Open();
                    String st = "INSERT INTO [user]  ([username],[password],[role]) values (@username,@password,@role)";
                    SqlCommand cmd = new SqlCommand(st, con);

                    cmd.Parameters.AddWithValue("@username", usernametxt.Text);
                    cmd.Parameters.AddWithValue("@password", passwordtxt.Text);
                    cmd.Parameters.AddWithValue("@role", rolePicker.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Application.Current.MainPage = new NavigationPage(new AdminTabbedPage());
                    DisplayAlert("", "utilisateur ajouté", "ok");
                    usernametxt.Text = string.Empty;
                    passwordtxt.Text = string.Empty;
                    rolePicker.Title = "Choisir le type d'utilisateur";
                    //done 
                }

            }
            catch
            {
                DisplayAlert("", "erreur", "ok");
            }

        }

        public IEnumerable<user> GetUsers()
        {
            List<user> usersList = new List<user>();
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select * from [dbo].[user]", con);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user u = new user();
                    u.id = Convert.ToInt32(reader["id"].ToString());
                    u.nomprenom = reader["nomETprenom"].ToString();
                    u.password = reader["password"].ToString();
                    u.role = reader["role"].ToString();
                    usersList.Add(u);
                }
                return usersList;
            }
        }

        private void btnDeleteUser_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idusertxt.Text))
            {
                DisplayAlert("Oops", "Choisir utilisateur", "Ok");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    Int32 key = Convert.ToInt32(idusertxt.Text);
                    SqlCommand command = new SqlCommand
                        ("delete from [dbo].[user] where [id]=@id", con);
                    command.Parameters.AddWithValue("@id", key);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    DisplayAlert("alert", "utilisateur supprimé", "Ok");
                }
            }
        }

        private void btnEditUser_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idusertxt.Text) ||
               string.IsNullOrEmpty(usernameEdittxt.Text) ||
               string.IsNullOrEmpty(passwordEdittxt.Text) ||
               string.IsNullOrEmpty(roleEditPicker.SelectedItem.ToString())
               )
            {
                DisplayAlert("", "Choisir un utilisateur", "Ok");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    Int32 key = Convert.ToInt32(idusertxt.Text);
                    SqlCommand command = new SqlCommand
                        (
                        "update  [dbo].[user] set [username]=@username,[password]=@password,[role]=@role where [id]=@id",
                        con
                        );
                    command.Parameters.AddWithValue("@id", key);
                    command.Parameters.AddWithValue("@username", usernameEdittxt.Text);
                    command.Parameters.AddWithValue("@password", passwordEdittxt.Text);
                    command.Parameters.AddWithValue("@role", roleEditPicker.SelectedItem.ToString());
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    DisplayAlert("", "utilisateur modifié", "Ok");
                }
            }
        }

        private void userListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (user)e.Item;
            Int32 id = Convert.ToInt32(item.id);
            string nomprenom = Convert.ToString(item.nomprenom);
            string password = Convert.ToString(item.password);
            string role = item.role;
            idusertxt.Text = id.ToString();
            usernameEdittxt.Text = nomprenom.ToString();
            passwordEdittxt.Text = password.ToString();
            roleEditPicker.SelectedItem = role.ToString();
        }

        public List<user> getTechnichianToPicker()
        {
            List<user> usersList = new List<user>();
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("getTechnichian", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user u = new user();
                    u.nomprenom = reader["nomETprenom"].ToString();
                    usersList.Add(u);
                }
                return usersList;
            }
        }

        //getchambre
        public List<Chambre> getchamberToPicker()
        {
            List<Chambre> usersList = new List<Chambre>();
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select name_Chambre from [dbo].[Chambre]", con);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Chambre u = new Chambre();
                    u.name_Chambre = reader["name_Chambre"].ToString();
                    usersList.Add(u);
                }
                return usersList;
            }
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(chambrePicker.SelectedItem.ToString())
                    && !string.IsNullOrEmpty(TechnichianPicker.SelectedItem.ToString())
                    && !string.IsNullOrEmpty(taskDescriptiontxt.Text))
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("spaddTask", con);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@tech", tech.ToString());
                        cmd.Parameters.AddWithValue("@chambre", ch.ToString());
                        cmd.Parameters.AddWithValue("@taskDescription", taskDescriptiontxt.Text);
                        cmd.Parameters.AddWithValue("@status", true);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("",
                            "Tache ajoutée", "ok");
                        chambrePicker.Title = "Choisir chambre";
                        TechnichianPicker.Title = "Choisir Technichien";
                        taskDescriptiontxt.Text = string.Empty;
                    }
                else
                {
                    DisplayAlert("", "SVP Remplissez tout le formulaire", "Ok");

                }
            }
            catch
            {

                DisplayAlert("", "il ya une erreur", "ok");
            }
        }

        private void TechnichianPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            userListView.RefreshCommand = new Command(() =>
            {
                userListView.ItemsSource = GetUsers();
                userListView.IsRefreshing = true;
            });
            var picker = sender as Picker;
            var selectedCategory = (user)picker.SelectedItem;
            tech = selectedCategory.nomprenom;

        }

        private void chambrePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (Chambre)picker.SelectedItem;
            ch = selectedCategory.name_Chambre;
        }

        

        private void Button_Clicked_5(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Page1());
    
        }
        
        private void formMap_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
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

