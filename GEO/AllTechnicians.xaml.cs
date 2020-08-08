 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
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

            public double id { get; set; }
            public string nomprenom { get; set; }

            public string cin { get; set; }
            public string datebirth { get; set; }
            public string adress { get; set; }
            public string email { get; set; }
            public string tel { get; set; }


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
                    SqlCommand command = new SqlCommand("select * from [dbo].[user] where [role]='Technicien'", con);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        user u = new user();
                        u.id = Convert.ToInt32(reader["id"].ToString());
                        u.nomprenom = reader["nomETprenom"].ToString();
                        u.cin = reader["cin"].ToString();
                        u.datebirth = reader["date_naiss"].ToString();
                        u.adress= reader["adress"].ToString();
                        u.email = reader["email"].ToString();
                        u.tel = reader["tel"].ToString();
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

        async void btnDeleteUser_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idusertxt.Text))
            {
               await DisplayAlert("", "Choisir utilisateur!", "Ok");
            }
            else
            {
                bool answer = await DisplayAlert("Confirmer Suppression", "vouler vous suprimmer ce technicien", "Oui", "Non");
                Debug.WriteLine("Answer: " + answer);           
                if (answer)
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
                       await DisplayAlert("alert", "utilisateur supprimé", "Ok");
                        userListView.ItemsSource = GetUsers();


                    }
                }
                else
                {
                    await DisplayAlert("", "vous avez annuler la suppression", "Ok");
                }
                   
            }
        }
 
        private void btnEditUser_Clicked(object sender, EventArgs e)
        {
            try { 
            if(string.IsNullOrEmpty(idusertxt.Text))
            {
                DisplayAlert("", "Choisir utilisateur", "Ok");
            }
           
            else
                  
            Navigation.PushModalAsync(new updateuser(double.Parse(idusertxt.Text),nom_prenom.Text, cincxt.Text,fakelbl.Text,adresstxt.Text,emailtxt.Text,int.Parse(teluser.Text),passwordEdittxt.Text,roleEditPicker.SelectedItem.ToString()));
        }
            catch(Exception exp)
            {
                DisplayAlert("", exp.Message, "ok");
            }

            }

        private void btnaddUser_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new adduser());
        }

        private void userListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (user)e.Item;
            double id = Convert.ToInt64(item.id);
            string nomprenom = Convert.ToString(item.nomprenom);
          string cin= Convert.ToString(item.cin);
            
             
            string email = Convert.ToString(item.email);
            
            string tel = Convert.ToString(item.tel);
            string password = Convert.ToString(item.password);
            string role = item.role;
            string ddate = item.datebirth;
            idusertxt.Text = id.ToString();
            nom_prenom.Text = nomprenom.ToString();
            fakelbl.Text = ddate.ToString();
            adresstxt.Text= item.adress;
            cincxt.Text = cin.ToString();
            emailtxt.Text = email.ToString();
            teluser.Text = tel.ToString();
            
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

        private void techSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<user> userlist = new List<user>();
            userlist = GetUsers().ToList();
            if (!String.IsNullOrEmpty(techSearchBar.Text))
                userListView.ItemsSource = userlist.Where(x => x.nomprenom .Contains(techSearchBar.Text) || x.tel .Contains(techSearchBar.Text) || x.email.Contains(techSearchBar.Text) || x.cin.Contains(techSearchBar.Text));
            else
                userListView.ItemsSource = userlist;
        }
    }
}