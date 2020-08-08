using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                BusyIndicator.IsRunning = true;
                if (!string.IsNullOrEmpty(usernametxt.Text) && !string.IsNullOrEmpty(passwordtxt.Text))
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        SqlCommand command = new SqlCommand
                            (
                            "SELECT * FROM [user] WHERE nomETprenom=@nomETprenom AND password=@password "
                            , con
                            );
                        command.Parameters.AddWithValue("@nomETprenom", usernametxt.Text);
                        command.Parameters.AddWithValue("@password", passwordtxt.Text);
                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader["role"].ToString() == "Admin")
                            {
                                Navigation.PushAsync(new MasterDetailPage1());
                                BusyIndicator.IsRunning = false;
                            }
                            if (reader["role"].ToString() == "Technicien")
                            {
                                Navigation.PushAsync(new TechnicianPage());
                            }
                            Preferences.Set("username", usernametxt.Text);
                        }
                        else
                        {
                            DisplayAlert("", "Login ou mot de passe incorrectes", "Ok");
                            BusyIndicator.IsRunning = false;
                        }
                    }
                }
                else
                {
                    DisplayAlert("Login ou mot de passe manquant", "Veuillez entrer votre login et votre mot de passe", "Ok");
                    BusyIndicator.IsRunning = false;
                }
            }
            catch
            {
                DisplayAlert("", "impossible de se connecter", "ok");
            }

        }

        private void usernametxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}