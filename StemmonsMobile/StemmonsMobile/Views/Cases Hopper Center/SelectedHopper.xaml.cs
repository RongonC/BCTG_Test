using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Cases_Hopper_Center
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectedHopper : ContentPage
    {
        public SelectedHopper()
        {
            InitializeComponent();
            App.SetConnectionFlag();
        }

        public SelectedHopper(string hopperName, string opencasecount, string createbymecount, string ownedbymecount, string casetypeownername) : this()
        {
			try{
            this.Title = hopperName;
            List<HopperData> lst = new List<HopperData>
            {
                new HopperData("Open Cases",opencasecount, "Assets/dropdowniconClose.png"),
                new HopperData("Created By Me",createbymecount, ""),
                new HopperData("Owned By Me",ownedbymecount, "Assets/dropdowniconClose.png"),
                new HopperData("Owner",casetypeownername, "Assets/dropdowniconClose.png"),

            };

            hopperview.ItemsSource = lst;
			}
			catch(Exception ex){
				
			}
        }

        private async void HomeIcon_Click(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private void btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);
            }
            catch (Exception)
            {
            }
        }
    }

    public class HopperData
    {
        public string Title { get; set; }
        public string CountData { get; set; }
        public string Img { get; set; }
        public HopperData(string name, string pos, string _img)
        {
            Title = name;
            CountData = pos;
            Img = _img;
        }
    }
}
