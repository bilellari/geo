using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailPage1 : MasterDetailPage
    {
        public MasterDetailPage1()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try 
            {
                var item = ((MasterDetailPage1MasterMenuItem)e.SelectedItem).Title;
                if (item == null)
                    return;
                Page page;
                switch (item)
                {
                    case "Chambres":
                        {
                            page = new Chambers();
                            break;
                        }
                    case "Tâches":
                        {
                            page = new AllTasks() as AllTasks;
                            break;
                        }
                    case "Techniciens":
                        {
                            page = new AllTechnicians() as AllTechnicians;
                            break;
                        }
                    
                    default: return;
                }
                page.Title = item;

                Detail = new NavigationPage(page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            catch
            {
                
                return;
            }
        }
    }
}