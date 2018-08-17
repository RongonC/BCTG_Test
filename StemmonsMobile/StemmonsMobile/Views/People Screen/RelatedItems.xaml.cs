using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Models;
using StemmonsMobile.Views.Cases_Hopper_Center;
using StemmonsMobile.Views.Entity;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.PeopleScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.People_Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelatedItems : ContentPage
    {
        string UserName;
        string Fullname;
        public RelatedItems(string username, string fullname)
        {
            InitializeComponent();
            UserName = username;
            Fullname = fullname;
            List<RelatedItemData> lst = new List<RelatedItemData>{
                new RelatedItemData("My Hoppers",""),
                new RelatedItemData("Entity Association",""),
                new RelatedItemData("Entity Role Association",""),
            };

            relateditemlist.ItemsSource = lst;
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView g = (ListView)sender;
            var sd = g.SelectedItem as RelatedItemData;

            if (sd.RelatedItemName == "My Hoppers")
            {
                this.Navigation.PushAsync(new SelectCaseHopper(Fullname, "MyHopper"));
            }
            else if (sd.RelatedItemName == "Entity Association")
            {
                this.Navigation.PushAsync(new MyAssociations("Entity Association", UserName));
            }
            else if (sd.RelatedItemName == "Entity Role Association")
            {
                this.Navigation.PushAsync(new EntityRoleAssociation(UserName));
            }
        }

        private void btn_home_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private async void btn_more_Clicked(object sender, EventArgs e)
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

    public class RelatedItemData
    {
        public string RelatedItemName { get; set; }
        public string RelatedItemdetail { get; set; }
        public RelatedItemData(string name, string detail)
        {
            RelatedItemName = name;
            RelatedItemdetail = detail;
        }
    }


}
