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
    public partial class adduser : ContentPage
    {
        public adduser()
        {
            InitializeComponent();
            teluser.Text = "+216";
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
            try
            {
              if (passwordEdittxt.Text!=confimrerpadd.Text)
               {
                    DisplayAlert("", "saisir le meme mot de pass", "ok");
                }
               else{
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {


                        con.Open();
                        String st = "INSERT INTO [user]  ([nom],[prenom],[tel],[username],[password],[role]) values (@nom,@prenom,@tel,@username,@password,@role)";
                        SqlCommand cmd = new SqlCommand(st, con);
                        cmd.Parameters.AddWithValue("@nom",nomuser.Text);
                        cmd.Parameters.AddWithValue("@prenom", prenomuser.Text);
                        cmd.Parameters.AddWithValue("@tel", teluser.Text);
                        cmd.Parameters.AddWithValue("@username", usernameEdittxt.Text);
                        cmd.Parameters.AddWithValue("@password", passwordEdittxt.Text);
                        cmd.Parameters.AddWithValue("@role", roleEditPicker.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("", "utilisateur ajouté", "ok");
                        Navigation.PushModalAsync(new adduser());
                        //done 
                    }
                  }
            }
            catch(Exception ex)
            {
                DisplayAlert("", ex.Message, "ok");
            }

        }
    }
}