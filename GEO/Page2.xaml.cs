﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Forms.Markup;

namespace GEO
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page2 : ContentPage
    {

        public Page2()
        {
            InitializeComponent();
            TasksListView.ItemsSource = getTask();
            TasksListView.RefreshCommand = new Command(() =>
            {
                TasksListView.ItemsSource = getTask();
                TasksListView.IsRefreshing = false;

            });


        }
        public IEnumerable<MyTache> getTask()
        {
            List<MyTache> tasklist = new List<MyTache>();
            using (SqlConnection con = new SqlConnection(connect.c)) 
            {
                SqlCommand command = new SqlCommand("getSpecialTask", con);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@techPrefrence", Preferences.Get("username", string.Empty));
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    foreach (var items in tasklist)
                    {
                        if (items.isAdded) { items.BackgroundColor = Color.White; }
                        
                    }                         
                    MyTache c = new MyTache();
                    c.id = Convert.ToInt32(reader["id"]);
                    c.chambre = reader["chambre"].ToString();
                    c.TaskDescription = reader["taskdescription"].ToString();
                    c.isAdded = true;
                   
                        tasklist.Add(c);
                        c.BackgroundColor = Color.Red;
                   



                }
                tasklist.Reverse();
                return tasklist;
            }
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        public void TasksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            var selectedItem = e.SelectedItem as MyTache;
           
            if (selectedItem == null) return;
            selectedItem.BackgroundColor = Color.White;
            Navigation.PushModalAsync(new TaskDetailandMap(selectedItem.id));
            ((ListView)sender).SelectedItem = null;
        }
    }
}
  