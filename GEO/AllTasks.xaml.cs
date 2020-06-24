using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTasks : ContentPage
    {
        private string ch = string.Empty;
        private string tech = string.Empty;
       

        public AllTasks()
        {
            InitializeComponent();
            TechnichianPicker.ItemsSource = getTechnichianToPicker();
            chambrePicker.ItemsSource = getchamberToPicker();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(chambrePicker.SelectedItem.ToString())
                    && !string.IsNullOrEmpty(TechnichianPicker.SelectedItem.ToString())
                    && !string.IsNullOrEmpty(taskDescriptiontxt.Text))
                {
                    
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
                      
                       
                    }
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
            Page page;
            page = new AllTasks() as AllTasks;
        }

      

        private void TechnichianPicker_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            userListView.RefreshCommand = new Command(() =>
            {
                userListView.ItemsSource = GetUsers();
                userListView.IsRefreshing = true;
            });
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
    }
}