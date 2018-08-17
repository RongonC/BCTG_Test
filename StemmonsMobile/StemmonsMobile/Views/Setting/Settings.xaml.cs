using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Setting
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            List<Settingsdata> lst = new List<Settingsdata>
            {
                new Settingsdata(0,"Enable Background-In App sync","","Assets/switchon.png"),
                new Settingsdata(1,"Sync All Entity Data","Assets/alertIcon.png","Assets/switchon.png"),
                new Settingsdata(2,"Entity Type to Sync All Data","","Assets/dropdowniconClose.png"),
                new Settingsdata(3,"Case Type to Sync All Data","","Assets/dropdowniconClose.png"),
                new Settingsdata(4,"Quest Form to Sync all Data","","Assets/dropdowniconClose.png"),
                new Settingsdata(5,"Sync Images in Original Size","Assets/alertIcon.png","Assets/switchoff.png"),

            };

            listdata.ItemsSource = lst;
        }

        void Actionbtn_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var applicationtype = button.BindingContext as Settingsdata;

            if (applicationtype.settingName == "Enable Background-In App sync")
            {
                if (button.Image == "Assets/switchoff.png")
                {
                    button.Image = "Assets/switchon.png";
                }
                else
                {
                    button.Image = "Assets/switchoff.png";
                }
            }
            else if (applicationtype.settingName == "Sync All Entity Data")
            {
                if (button.Image == "Assets/switchoff.png")
                {
                    button.Image = "Assets/switchon.png";
                }
                else
                {
                    button.Image = "Assets/switchoff.png";
                }
            }
            else if (applicationtype.settingName == "Sync Images in Original Size")
            {
                if (button.Image == "Assets/switchoff.png")
                {
                    button.Image = "Assets/switchon.png";
                }
                else
                {
                    button.Image = "Assets/switchoff.png";
                }
            }
        }

        private void listdata_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var listItem = (ListView)sender;
            var item = listItem.SelectedItem as Settingsdata;
            if (Functions.CheckInternetWithAlert())
            {
                switch (item.ActionId)
                {
                    case 0://backgroud Sync
                        break;
                    case 1://Enity all sync
                        break;
                    case 2://entity Type Sync to all data
                        try
                        {
                            App.isFirstcall = true;
                            App.OnlineSyncRecord();
                        }
                        catch (Exception)
                        {
                        }

                        break;
                    case 3://Cases type sync to all data
                        try
                        {
                            App.isFirstcall = true;
                            App.OnlineSyncRecord();
                        }
                        catch (Exception)
                        {
                        }
                        listItem.SelectedItem = null;
                        break;
                    case 4://Quest type tpsync all data
                        try
                        {
                            App.isFirstcall = true;
                            App.OnlineSyncRecord();
                            //HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);
                        }
                        catch (Exception)
                        {
                        }
                        listItem.SelectedItem = null;
                        break;
                    case 5:// Sync All image to original
                        break;
                    default:
                        break;
                }
                listItem.SelectedItem = null;
            }
        }
    }




    public class Settingsdata
    {
        public int ActionId { get; set; }
        public string settingName { get; set; }
        public string alertImage { get; set; }
        public string actionImage { get; set; }

        public Settingsdata(int actionId, string sname, string alertImg, string actionImg)
        {
            ActionId = actionId;
            settingName = sname;
            alertImage = alertImg;
            actionImage = actionImg;
        }
    }
}
