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
    public partial class TaskDetailandMap : ContentPage
    {
        public TaskDetailandMap(int idcame)
        {
            InitializeComponent();
            getTask(idcame);
        }
        public void getTask(int id)
        {
            using (SqlConnection con = new SqlConnection(connect.c))
            {
                SqlCommand command = new SqlCommand("select * from [TaskforTech] where id=@id", con);
                command.Parameters.AddWithValue("@id", id);
                con.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lblChamber.Text = reader["chambre"].ToString();
                    lblTask.Text = reader["taskdescription"].ToString();
                }
            }
        }
    }
}