using StemmonsMobile.Commonfiles;
using StemmonsMobile.CustomControls;
using StemmonsMobile.Views;
using StemmonsMobile.Views.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingButton : ContentView
    {
        private event EventHandler CustomButtonClickEvent;
        public SettingButton(EventHandler settingButton_Click = null)
        {
            InitializeComponent();

            if (settingButton_Click != null)
            {
                btnSetting.Clicked += settingButton_Click;
            }
            else
            {
                btnSetting.Clicked += button_click;
            }
        }

        public void button_click(object sender, EventArgs e)
        {
            DefaultClick();
        }
        public async Task DefaultClick()
        {
            var sb = new StringBuilder();
            sb.Append("Switch to ");
            if (App.Isonline)
                sb.Append("Offline Mode");
            else
                sb.Append("Online Mode");

            try
            {


                var action = await Application.Current.MainPage.DisplayActionSheet("Select Option", "Cancel", sb.ToString(), "Run Synchronization", "Setting", "About", "Logout", "Check Sync Status"/* ,"Landing Page"*/);

                if (action.ToLower().Contains("offline"))
                    action = "offline";
                else if (action.ToLower().Contains("online"))
                    action = "online";

                switch (action)
                {

                    case "Check Sync Status":
                        if (App.SyncProgressFlag)
                        {
                            await Application.Current.MainPage.Navigation.PushAsync(new SyncStatusPage(Functions.lstSyncAPIStatus));
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Alert", "Please wait for sync to complete", "OK");
                        }
                        break;
                    case "Landing Page":
                        if (App.IsPropertyPage)
                            Application.Current.MainPage.Navigation.PushAsync(new LandingPage());
                        else
                            Application.Current.MainPage.Navigation.PushAsync(new PropertyLandingPage());
                        break;
                    case "Logout":
                        App.Logout(Application.Current.MainPage);
                        break;
                    case "About":
                        await Application.Current.MainPage.DisplayAlert("App Info", Functions.Appinfomsg, "Ok");
                        break;
                    case "Setting":
                        await Application.Current.MainPage.Navigation.PushAsync(new Settings());
                        break;

                    case "offline"://Switch to offline
                        App.SetForceFullOnLineOffline(false);
                        App.SetConnectionFlag();
                        break;
                    case "online"://Switch to online
                        App.SetForceFullOnLineOffline(true);
                        App.SetConnectionFlag();
                        break;

                    case "Run Synchronization":
                        if (Functions.CheckInternetWithAlert())
                        {
                            App.isFirstcall = true;
                            App.OnlineSyncRecord();
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                string st = ex.ToString();
            }
        }
    }

}