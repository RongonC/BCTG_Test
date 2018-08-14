using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchList : ContentPage
    {
        public SearchList()
        {
            InitializeComponent();

            //InstanceList.ItemsSource = new string[] { "Search People", "Search Cases", "Search Entities", "Search Standards" };
            InstanceList.ItemsSource = new string[] { "Search People", "Search Cases" };
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView l = (ListView)sender;
            var sd = l.SelectedItem;

            Navigation.PushAsync(new SearchPage(l.SelectedItem.ToString()));
        }

        void BackClicked_Clicked(object sender, System.EventArgs e)
        {
            this.Navigation.PopAsync();
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
                //var Action = await DisplayActionSheet(null, "Cancel", "Switch to online Mode", "Run Synchronization", "About", "Log Out");

                //switch (Action)
                //{
                //    case "Switch to online Mode":
                //        if (App.Isonline == true)
                //        {
                //            DisplayAlert("", "You already Online.", "OK");
                //        }
                //        else
                //        {
                //            DisplayAlert("", "Please Go Online.", "OK");
                //        }
                //        break;
                //    case "Run Synchronization":
                //        DisplayAlert("", "Online Synchronization Process in Progress.", "OK");
                //        HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);
                //        break;
                //    case "About":
                //        DisplayAlert("App info", Functions.Appinfomsg, "Ok");
                //        break;
                //    case "Log Out":
                //        try
                //        {
                //            await Task.Run(() => { Functions.ClearApplocalData(); });

                //            var existingPages = Navigation.NavigationStack.ToList();
                //            Page DEF = new SelectInstancePage();

                //            if (existingPages.Count > 0)
                //            {
                //                Navigation.InsertPageBefore(DEF, Navigation.NavigationStack[0]);

                //                for (int i = 0; i < existingPages.Count; i++)
                //                {
                //                    Navigation.RemovePage(existingPages[i]);
                //                }
                //            }
                //            else
                //            {
                //                this.Navigation.PopAsync();
                //            }

                //            Functions.IsLogin = false;
                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //        break;

                //    default:
                //        break;
                //}
            }
            catch (Exception)
            {
            }
        }
    }
}