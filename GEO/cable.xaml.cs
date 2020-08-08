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
    public partial class cable : ContentPage
    {
        public cable()
        {
            InitializeComponent();
            CableListView2.ItemsSource = getCable();
           

            CableListView2.RefreshCommand = new Command(() =>
            {
                CableListView2.ItemsSource = getCable();
                CableListView2.IsRefreshing = false;

            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new cablage());
        }

       async private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idtxt.Text))
            {
                await
                   DisplayAlert("Oops", "choisissez le câble que vous souhaitez supprimer", "Ok");
            }
            else
            {
                bool answer = await DisplayAlert("Confirmer Suppression", "vouler vous suprimmer ce câble", "Yes", "No");
                Debug.WriteLine("Answer: " + answer);
                if (answer)
                {
                    using (SqlConnection con = new SqlConnection(connect.c))
                    {
                        Int32 key = Convert.ToInt32(idtxt.Text);
                        SqlCommand command = new SqlCommand
                            ("delete from [dbo].[Cable] where [ID_Cable]=@id", con);
                        command.Parameters.AddWithValue("@id", key);
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();
                        await DisplayAlert("", "le câble est supprimé", "Ok");
                        CableListView2.ItemsSource = getCable();
                    }
                }
                else
                {
                    await DisplayAlert("", "vous avez annuler la suppression", "Ok");
                }

            }
            
        }

       
        

        private void CableListView2_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (mcable)e.Item;
            Int32 id = Convert.ToInt32(item.ID_Cable);
            
            idtxt.Text = id.ToString();
            nomtxt.Text = item.Nom_Cable;
            nbrtxt.Text = item.Nombre_brin;
            bandetxt.Text = item.bande_passante;
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
            async void OnAlertYesNoClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Confirmer Suppression", "vouler vous suprimmer cette chambre", "Yes", "No");
            Debug.WriteLine("Answer: " + answer);
        }
            public IEnumerable<mcable> getCable()
            {
                List<mcable> cablelist = new List<mcable>();
                using (SqlConnection con = new SqlConnection(connect.c))
                {
                    SqlCommand command = new SqlCommand("select * from [dbo].[Cable]", con);
                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        mcable c = new mcable();
                        c.ID_Cable = Convert.ToInt32(reader["ID_Cable"].ToString());
                        c.Nom_Cable = reader["Nom_Cable"].ToString();
                       
                        c.Nombre_brin = reader["Nombre_brin"].ToString();
                        c.bande_passante = reader["bande_passante"].ToString();
                        

                        cablelist.Add(c);

                    }

                    return cablelist;

                }

            }

        private void Button_Clicked_3(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(idtxt.Text))
            {
                DisplayAlert("Oops", "choisissez le câble que vous souhaitez modifier", "Ok");
            }
            else
            {
                Navigation.PushModalAsync(new updatecable(int.Parse(idtxt.Text), nomtxt.Text, nbrtxt.Text, bandetxt.Text));
            }
        }
    }
}