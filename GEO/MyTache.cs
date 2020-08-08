using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GEO
{
    public class MyTache 
    {

        public int id { get; set; }
        public string technichian { get; set; }
        public string chambre { get; set; }
        public string TaskDescription { get; set; }
        public bool status { get; set; } = true;
        public Color BackgroundColor { get; internal set; }
        public bool isAdded { get; set; } = false;

}
}
