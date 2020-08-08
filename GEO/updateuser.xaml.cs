using Plugin.Geolocator.Abstractions;
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
        public updateuser( double id,string nomprenom, string cin, String dateofbirth,string adress,string email,int tel, string pass, string role )
        {
            InitializeComponent();
            idusertxt.Text = id.ToString();
            nom_prenom.Text = nomprenom;
            cintxt.Text = cin;
           fakelbl.Text = dateofbirth;
            adresstxt.Text = adress;
            emailuser.Text = email;
            teluser.Text = tel.ToString();
            passwordEdittxt.Text = pass;
            roleEditPicker.SelectedItem = role.ToString();
        }

        DatePicker datePicker = new DatePicker
        {
            MinimumDate = new DateTime(2018, 1, 1),
            MaximumDate = new DateTime(2018, 12, 31),
            Date = new DateTime(2018, 6, 21)

        };
        private void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idusertxt.Text) ||
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
                    double key = Convert.ToInt64(idusertxt.Text);
                    SqlCommand command = new SqlCommand
                        (
                        "update  [dbo].[user] set [nomETprenom]=@nomprenom,[cin]=@cin,[date_naiss]=@dateOFbirth,[adress]=@adress,[email]=@email,[tel]=@tel,[password]=@password,[role]=@role where [id]=@id",
                        con
                        );
                    command.Parameters.AddWithValue("@id", key);
                    command.Parameters.AddWithValue("@nomprenom", nom_prenom.Text);
                    command.Parameters.AddWithValue("@cin", cintxt.Text);
                    command.Parameters.AddWithValue("@dateOFbirth", fakelbl.Text);
                    command.Parameters.AddWithValue("@adress", adresstxt.Text);
                    command.Parameters.AddWithValue("@email", emailuser.Text);
                    command.Parameters.AddWithValue("@tel", teluser.Text);
                   command.Parameters.AddWithValue("@password", passwordEdittxt.Text);
                    command.Parameters.AddWithValue("@role", roleEditPicker.SelectedItem.ToString());
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    DisplayAlert("", "technicien modifié", "Ok");
                    App.Current.MainPage = new AllTechnicians();
                }
            }
        }

        private void dateOfBirth_DateSelected(object sender, DateChangedEventArgs e)
        {
            fakelbl.Text = e.NewDate.ToString();
        }
    }
}