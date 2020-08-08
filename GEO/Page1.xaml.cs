using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        private List<Chambre> Chambres;
        public Page1()
        {
            InitializeComponent();
            Chambres = getChambre().ToList();
            ChambreListView.RefreshCommand = new Command(() => ChambreListView_Refreshing());
            ChambreListView.ItemsSource = Chambres;
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
                    c.Longitude = ((double)reader["Longitude"]);
                    c.Latitude = ((double)reader["Latitude"]);
                    chambrelist.Add(c);
                }
                return chambrelist;

            }
        }

        private void ChambreListView_Refreshing()
        {
            Chambres = getChambre().ToList();
            if (!String.IsNullOrEmpty(TaskSearchBar.Text))
                Chambres = Chambres.Where(x => x.name_Chambre.StartsWith(TaskSearchBar.Text)).ToList();
            ChambreListView.ItemsSource = Chambres;
            ChambreListView.IsRefreshing = false;
        }

        private void TaskSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!String.IsNullOrEmpty(TaskSearchBar.Text))
                ChambreListView.ItemsSource = Chambres.Where(x => x.name_Chambre.StartsWith(TaskSearchBar.Text));
            else
                ChambreListView.ItemsSource = Chambres;
        }

        private void ChambreListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var chambreDetails = new ChambreDetailAndMap((Chambre)e.SelectedItem);
            Navigation.PushModalAsync(chambreDetails);
        }
    }
}