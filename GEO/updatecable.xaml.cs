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
    public partial class updatecable : ContentPage
    {
        public updatecable(int ID, string nom, string nb, string bd)
        {
            InitializeComponent();
            id_txt.Text = ID.ToString();
            nomcable_txt.Text = nom.ToString();
            brin_txt.Text = nb.ToString();
            bandepassante_txt.Text = bd.ToString();
        }



        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(id_txt.Text) ||
            string.IsNullOrEmpty(nomcable_txt.Text) ||
             string.IsNullOrEmpty(brin_txt.Text)
             )
            {
                DisplayAlert("", "Choisir un cable", "Ok");
            }

            else

            {
                try
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        double key = Convert.ToInt64(id_txt.Text);
                        SqlCommand command = new SqlCommand
                            (
                            "update  [dbo].[cable] set [Nom_Cable]=@nom,[nombre_Brin]=@nbr,[bande_passante]=@bd where [ID_Cable]=@id", con);

                        command.Parameters.AddWithValue("@id", key);
                        command.Parameters.AddWithValue("@nom", nomcable_txt.Text);
                        command.Parameters.AddWithValue("@nbr", brin_txt.Text);
                        command.Parameters.AddWithValue("@bd", bandepassante_txt.Text);

                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();
                        DisplayAlert("", "le cable est  modifié", "Ok");
                        Navigation.PushModalAsync(new cable());
                    }
                }
                catch(Exception exp)
                {
                    DisplayAlert("", exp.Message, "ok");
                }
            }
        }
    }
}