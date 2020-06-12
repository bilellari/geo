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
                if (!string.IsNullOrEmpty(usernametxt.Text) && !string.IsNullOrEmpty(passwordtxt.Text))
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        SqlCommand command = new SqlCommand
                            (
                            "SELECT * FROM [user] WHERE username=@username AND password=@password "
                            , con
                            );
                        command.Parameters.AddWithValue("@username", usernametxt.Text);
                        command.Parameters.AddWithValue("@password", passwordtxt.Text);
                        con.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            if (reader["role"].ToString() == "Admin")
                            {
                                Navigation.PushModalAsync(new  AdminTabbedPage());
                            }
                            if (reader["role"].ToString() == "Technicien")
                            {
                                Navigation.PushModalAsync(new TechnicianPage());
                            }
                            Preferences.Set("username", usernametxt.Text);
                        }
                        else
                        {
                            DisplayAlert("Oops", "invalid username or password", "Ok");
                        }
                    }
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

        private void usernametxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}