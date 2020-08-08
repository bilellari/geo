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
    public partial class Chambers : ContentPage
    {


       
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
                    c.Cable1= reader["cable1"].ToString();
                    c.Cable2 = reader["cable3"].ToString();
                    c.Cable3 = reader["cable3"].ToString();
                    c.allcable = reader["cable1"].ToString() + reader["cable3"].ToString() + reader["cable3"].ToString();

                    chambrelist.Add(c);

                }
                
                return chambrelist;
                
            }

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new addchamber());
        }
      
       async void Button_Clicked_1(object sender, EventArgs e)
        {



            if (string.IsNullOrEmpty(idtxt.Text))
            {await
                DisplayAlert("Oops", "choisissez la chambre que vous souhaitez supprimer", "Ok");
            }
            else
           {
                bool answer = await DisplayAlert("Confirmer Suppression", "vouler vous suprimmer cette chambre", "Yes", "No");
                Debug.WriteLine("Answer: " + answer);
                if(answer)
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
                      await  DisplayAlert("", "la chambre est supprimée", "Ok");
                        ChambreListView2.ItemsSource = getChambre();
                }
              }
                else
                    {
                       await DisplayAlert("", "vous avez annuler la suppression", "Ok");
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
                , double.Parse(latitudetxt.Text), cable1etxt.Text, cable2txt.Text, cable3txt.Text)); 
            }
            
        }

      public void ChambreListView2_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
            Chambre item = (Chambre)e.Item;
            Int32 id = Convert.ToInt32(item.id_Chambre);
            double lomgtude = double.Parse(item.Longitude.ToString());
            double latitude = double.Parse(item.Latitude.ToString());
            
            idtxt.Text = id.ToString();
            namechambretxt.Text = item.name_Chambre;
            longtudetxt.Text = lomgtude.ToString();
            latitudetxt.Text = latitude.ToString();
            cable1etxt.Text = item.Cable1;
            cable2txt.Text = item.Cable2;
            cable3txt.Text = item.Cable3;
            
        }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idtxt.Text))
            {
                DisplayAlert("", "Veuiller choisir une chambre ", "Ok");
            }
            else { Navigation.PushModalAsync(new chambermap(namechambretxt.Text, longtudetxt.Text, latitudetxt.Text)); 
            }
           
        }
        ViewCell lastCell;
        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("#FFFFFF");
                lastCell = viewCell;
                
            }
        }
        async void OnAlertYesNoClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirmer Suppression", "vouler vous suprimmer cette chambre", "Yes", "No");
            Debug.WriteLine("Answer: " + answer);
        }

        private void chbrSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Chambre> chambrelist = new List<Chambre>();
            chambrelist = getChambre().ToList();
            if (!String.IsNullOrEmpty(chbrSearchBar.Text))
                ChambreListView2.ItemsSource = chambrelist.Where(x => x.name_Chambre.Contains(chbrSearchBar.Text)|| x.Cable1.Contains(chbrSearchBar.Text));
            else
                ChambreListView2.ItemsSource = chambrelist;
        }
    }
}
    