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
    public partial class AllTechnicians : ContentPage
    {
        public class user
        {

            public int id { get; set; }
            public string nom { get; set; }
            public string prenom { get; set; }
            public string tel { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string role { get; set; }
        }
        public AllTechnicians()
        {
            InitializeComponent();
            userListView.ItemsSource = GetUsers();
            userListView.RefreshCommand = new Command(() =>
            {
                userListView.ItemsSource = GetUsers();
                userListView.IsRefreshing = false;

            });
        }
        public IEnumerable<user> GetUsers()
        {
            List<user> usersList = new List<user>();
            try
            {
               
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    SqlCommand command = new SqlCommand("select * from [dbo].[user]", con);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user u = new user();
                        u.id = Convert.ToInt32(reader["id"].ToString());
                        u.nom = reader["nom"].ToString();
                        u.prenom = reader["prenom"].ToString();
                        u.tel = reader["tel"].ToString();
                        u.username = reader["username"].ToString();
                        u.password = reader["password"].ToString();
                        u.role = reader["role"].ToString();
                        usersList.Add(u);
                    }
                    
                }
            }
            catch(Exception exp)
            {
                DisplayAlert("", exp.Message, "");
                    }
            return usersList;
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
            if(string.IsNullOrEmpty(idusertxt.Text))
            {
                DisplayAlert("Oops", "Choisir utilisateur", "Ok");
            }
           
            else
            Navigation.PushModalAsync(new updateuser(int.Parse(idusertxt.Text),nomuser.Text,prenomuser.Text,int.Parse(teluser.Text),usernameEdittxt.Text,passwordEdittxt.Text,roleEditPicker.SelectedItem.ToString()));
        }

        private void btnaddUser_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new adduser());
        }

        private void userListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (user)e.Item;
            Int32 id = Convert.ToInt32(item.id);
            string nom = Convert.ToString(item.nom);
            string prenom = Convert.ToString(item.prenom);
            string tel = Convert.ToString(item.tel);
            string username = Convert.ToString(item.username);
            string password = Convert.ToString(item.password);
            string role = item.role;
            idusertxt.Text = id.ToString();
            nomuser.Text = nom.ToString();
            prenomuser.Text = prenom.ToString();
            teluser.Text = tel.ToString();
            usernameEdittxt.Text = username.ToString();
            passwordEdittxt.Text = password.ToString();
            roleEditPicker.SelectedItem = role.ToString();
        }
        ViewCell lastCell;
      

        private void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.Azure;
                lastCell = viewCell;
            }
        }
    }
}