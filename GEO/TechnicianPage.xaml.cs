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
    public partial class TechnicianPage : ContentPage
    {
        public TechnicianPage()
        {
            InitializeComponent();
           
        }

        
        private void MyTasksButton_Clicked(object sender, EventArgs e)
        {

            Navigation.PushModalAsync(new Page2());
        }

        private void ChambersButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new Page1());
        }
    }
}