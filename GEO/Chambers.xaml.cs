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
    public partial class Chambers : ContentPage
    {


        public class Chambre
        {
            public int id_Chambre { get; set; }
            public string name_Chambre { get; set; }
            public double Longitude { get; set; }
            public double Latitude { get; set; }
        }
        public Chambers()
        {

            InitializeComponent();
            ChambreListView2.ItemsSource = getChambre();
            BusyIndicator.IsRunning = false;

            ChambreListView2.RefreshCommand = new Command(() =>
            {
                ChambreListView2.ItemsSource = getChambre();
                ChambreListView2.IsRefreshing = false;

            });

        }

        public IEnumerable<Chambre> getChambre()
        {
            List<Chambre> chambrelist = new List<Chambre>();
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select * from [dbo].[Chambre]", con);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Chambre c = new Chambre();
                    c.id_Chambre = Convert.ToInt32(reader["id_Chambre"].ToString());
                    c.name_Chambre = reader["name_Chambre"].ToString();
                    c.Longitude = double.Parse(reader["Longitude"].ToString());
                    c.Latitude = double.Parse(reader["Latitude"].ToString());
                    chambrelist.Add(c);

                }
                
                return chambrelist;
                
            }




        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
            Navigation.PushModalAsync(new addchamber());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {



            if (string.IsNullOrEmpty(idtxt.Text))
            {
                DisplayAlert("Oops", "choisissez la chambre que vous souhaitez supprimer", "Ok");
            }
            else
            {
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    Int32 key = Convert.ToInt32(idtxt.Text);
                    SqlCommand command = new SqlCommand
                        ("delete from [dbo].[Chambre] where [id_Chambre]=@id", con);
                    command.Parameters.AddWithValue("@id", key);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    DisplayAlert("", "chambre supprimée", "Ok");
                }
            }

        }

        private void Button_Clicked_2(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idtxt.Text))
            {
                DisplayAlert("Oops", "choisissez la chambre que vous souhaitez modifier", "Ok");
            }
            else
            { 
                Navigation.PushModalAsync(new UpdateChamber(int.Parse(idtxt.Text),namechambretxt.Text,
                double.Parse(longtudetxt.Text)
                , double.Parse(latitudetxt.Text))); 
            }
            
        }

      public void ChambreListView2_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            var item = (Chambre)e.Item;
            Int32 id = Convert.ToInt32(item.id_Chambre);
            double lomgtude = double.Parse(item.Longitude.ToString());
            double latitude = double.Parse(item.Latitude.ToString());
            idtxt.Text = id.ToString();
            namechambretxt.Text = item.name_Chambre;
            longtudetxt.Text = lomgtude.ToString();
            latitudetxt.Text = latitude.ToString();
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Page1());
        }
        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#6993C6");
                lastCell = viewCell;
                
            }
        }
    }
}
    