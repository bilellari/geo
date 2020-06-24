using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class updateuser : ContentPage
    {
        public updateuser( int id,string nom,string prenom , int tel,string login, string pass, string role )
        {
            InitializeComponent();
            idusertxt.Text = id.ToString();
            nomuser.Text = nom;
            prenomuser.Text = prenom;
            teluser.Text = tel.ToString();
            usernameEdittxt.Text = login;
            passwordEdittxt.Text = pass;
            roleEditPicker.SelectedItem = role.ToString();
        }

        private void Button_Clicked(object sender, EventArgs e)
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
            if (passwordEdittxt.Text!=confimrerpadd.Text)
                {
                DisplayAlert("", "Saisir le meme mot  de pass ", "Ok");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    Int32 key = Convert.ToInt32(idusertxt.Text);
                    SqlCommand command = new SqlCommand
                        (
                        "update  [dbo].[user] set [nom]=@nom,[prenom]=@prenom,[tel]=@tel,[username]=@username,[password]=@password,[role]=@role where [id]=@id",
                        con
                        );
                    command.Parameters.AddWithValue("@id", key);
                    command.Parameters.AddWithValue("@nom", nomuser.Text);
                    command.Parameters.AddWithValue("@prenom", prenomuser.Text);
                    command.Parameters.AddWithValue("@tel", teluser.Text);
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
    }
}