using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminTabbedPage : TabbedPage
    {
        private  string ch=string.Empty;
        private  string tech=string.Empty;
        public AdminTabbedPage()
        {
            InitializeComponent();
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

        private void Add_1(object sender, EventArgs e)
        {
         
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
                        getchamberToPicker();
                        DisplayAlert("alert", "you are added the new chambre", "ok");
                        longitude_txt.Text = string.Empty;
                        nom_ch_txt.Text = string.Empty;
                        latitude_txt.Text = string.Empty;
                    }
                else
                {
                    DisplayAlert("Oops", "fill the blanck before click", "Ok");

                }
            }
            catch
            {
                
                DisplayAlert("Oops", "something went wrong", "ok");
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
                DisplayAlert("Oops", "chose the chambre that you want to delete", "Ok");
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
                    DisplayAlert("alert", "the chambre deleted successfuly", "Ok");
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
                    DisplayAlert("Oops", "chose the chambre that you want to update", "Ok");
                }
                else
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        Int32 key = Convert.ToInt32(idtxt.Text);
                        string lomgtude = (longtudetxt.Text);
                        string latitude = (latitudetxt.Text);
                        SqlCommand command = new SqlCommand
                            (
                            "update  [dbo].[Chambre] set [name_Chambre]=@name,[Longitude]=@lo,[Latitude]=@la where [id_Chambre]=@id",
                            con
                            );
                        command.Parameters.AddWithValue("@id", key);
                        command.Parameters.AddWithValue("@name", namechambretxt.Text);
                        command.Parameters.AddWithValue("@lo", lomgtude);
                        command.Parameters.AddWithValue("@la", latitude);
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("alert", "the chambre updated successfuly", "Ok");
                    }
                }
            }
            catch
            {
                DisplayAlert("Oops", "something went wrong", "ok");
            }
            //we miss something here look
          
        }

        private void ChambreListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (Chambre)e.Item;
            Int32 id= Convert.ToInt32(item.id_Chambre);
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
                    
                     cmd.Parameters.AddWithValue("@username",usernametxt.Text);
                    cmd.Parameters.AddWithValue("@password", passwordtxt.Text);
                    cmd.Parameters.AddWithValue("@role", rolePicker.SelectedItem.ToString());
                    cmd.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("alert", "you are added new user", "ok");
                    usernametxt.Text = string.Empty;
                    passwordtxt.Text = string.Empty;
                    rolePicker.Title = "select user type";
                    //done 
                    }
                
            }
            catch
            {
                DisplayAlert("Oops", "something went wrong", "ok");
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
                    u.username = reader["username"].ToString();
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
                DisplayAlert("Oops", "chose the user that you want to delete", "Ok");
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
                    DisplayAlert("alert", "the user deleted successfuly", "Ok");
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
                DisplayAlert("Oops", "chose the user that you want to update", "Ok");
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
                    DisplayAlert("alert", "the user updated successfuly", "Ok");
                }
            }
        }

        private void userListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (user)e.Item;
            Int32 id = Convert.ToInt32(item.id);
            string username = Convert.ToString(item.username);
            string password = Convert.ToString(item.password);
            string role = item.role;
            idusertxt.Text = id.ToString();
            usernameEdittxt.Text = username.ToString();
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
                    u.username = reader["username"].ToString();
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
                        cmd.Parameters.AddWithValue("@status",true);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("alert", "you are added the new task", "ok");
                        chambrePicker.Title = "select chambre";
                        TechnichianPicker.Title = "select Technichian";
                        taskDescriptiontxt.Text = string.Empty;
                    }
                else
                {
                    DisplayAlert("Oops", "fill the blanck before click", "Ok");

                }
            }
            catch
            {

                DisplayAlert("Oops", "something went wrong", "ok");
            }
        }

        private void TechnichianPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (user)picker.SelectedItem;
            tech = selectedCategory.username;
        }

        private void chambrePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = sender as Picker;
            var selectedCategory = (Chambre)picker.SelectedItem;
            ch = selectedCategory.name_Chambre;
        }

        private void MyTasksButton_Clicked(object sender, EventArgs e)
        {

        }

        private void addchambrButton_Clicked(object sender, EventArgs e)
        {

        }

        private void allchambrButton_Clicked(object sender, EventArgs e)
        {

        }

        private void addUuserButton_Clicked(object sender, EventArgs e)
        {

        }

        private void adduserButton_Clicked(object sender, EventArgs e)
        {

        }

        private void alluserButton_Clicked(object sender, EventArgs e)
        {

        }

        private void addtaskButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}

