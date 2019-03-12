using DataServiceBus.OfflineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.CustomControls;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.LoginProcess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectInstancePage : ContentPage
    {

        public SelectInstancePage()
        {
            InitializeComponent();

            //loader.IsRunning = true;
        }
        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var _temp = await DBHelper.GetInstanceList(App.DBPath);
            InstanceList.ItemsSource = _temp;

            if (Functions.IsPWDRemember && Functions.IsLogin)
            {
                Task.Run(() =>
                {
                    Functions.GetBaseURLfromSQLite();
                });
                App.IsLoginCall = true;
                if (App.IsPropertyPage)
                    await Navigation.PushAsync(new PropertyLandingPage());
                else
                    await Navigation.PushAsync(new LandingPage());
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            Functions.Selected_Instance = 0;
            await Navigation.PushAsync(new CreateNewInstance());
        }
        void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Task.Run(() =>
            {
                App.DownloadCompanyLog();
            });

            var value = e.Item as InstanceList;
            Functions.Selected_Instance = value.InstanceID;
            DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl = value.InstanceUrl;
            this.Navigation.PushAsync(new LoginPage(value) { BindingContext = value });
            InstanceList.SelectedItem = null;
        }
    }
}
