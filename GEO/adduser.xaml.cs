using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
         
        }
        DatePicker datePicker = new DatePicker
        {
            MinimumDate = new DateTime(2018, 1, 1),
            MaximumDate = new DateTime(2018, 12, 31),
            Date = new DateTime(2018, 6, 21)
            
        };
        private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (passwordEdittxt.Text != confimrerpadd.Text)
                {
                    DisplayAlert("", "saisir le meme mot de pass", "ok");
                }
                else
                  if (cintxt.Text.Length != 8)
                {
                    DisplayAlert("", "le numero de CIN doit etre egale a 8 chiffres", "ok");
                }
                else
                if (!emailuser.Text.Contains("@"))
                {
                    emailuser.Text = string.Empty;
                    DisplayAlert("", "l'email doit etre de la forme exemple@****.com(fr)", "ok");
                        }
              else
              if(true)
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        con.Open();
                        String st = "INSERT INTO [user]  ([nomETprenom],[cin],[date_naiss],[adress],[email],[tel],[password],[role]) values (@nomprenom,@cin,@dateOFbirth,@adress,@email,@tel,@password,@role)";
                        SqlCommand cmd = new SqlCommand(st, con);
                        cmd.Parameters.AddWithValue("@nomprenom", nomprenom.Text);
                        cmd.Parameters.AddWithValue("@cin",cintxt.Text);
                        cmd.Parameters.AddWithValue("@dateOFbirth", fakelbl.Text);
                        cmd.Parameters.AddWithValue("@adress", adresse.Text);
                        cmd.Parameters.AddWithValue("@email",emailuser.Text);
                        cmd.Parameters.AddWithValue("@tel",teluser.Text);
                        cmd.Parameters.AddWithValue("@password",passwordEdittxt.Text);
                        cmd.Parameters.AddWithValue("@role",roleEditPicker.SelectedItem.ToString());
                        cmd.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("","Technicien ajouté","ok");
                        App.Current.MainPage = new AllTechnicians();



                        //done 
                    }
                  }
            }
            catch(Exception ex)
            {
                DisplayAlert("", ex.Message, "ok");
            }

        }

        private void dateOfBirth_DateSelected(object sender, DateChangedEventArgs e)
        {
            fakelbl.Text = e.NewDate.ToString();
        }
    }
}