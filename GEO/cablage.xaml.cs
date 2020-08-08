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
    public partial class cablage : ContentPage
    {
        public cablage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(connect.c))
            {


                con.Open();
                String st = "INSERT INTO [cable]  ([Nom_Cable],[nombre_Brin],[bande_passante]) values (@nom,@brin,@bd)";
                SqlCommand cmd = new SqlCommand(st, con);
                cmd.Parameters.AddWithValue("@nom", nomcable_txt.Text);
                cmd.Parameters.AddWithValue("@brin", brin_txt.Text);
                cmd.Parameters.AddWithValue("@bd", bandepassante_txt.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                App.Current.MainPage = new MasterDetailPage1();
                DisplayAlert("", "cable ajouté", "ok");
                


            }
        }
    }
}