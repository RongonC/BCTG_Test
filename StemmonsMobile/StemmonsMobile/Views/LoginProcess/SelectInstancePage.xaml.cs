using DataServiceBus.OfflineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Models;
using System;
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
        private async Task ContentPage_Appearing(object sender, EventArgs e)
        {
            var _temp = await DBHelper.GetInstanceList(App.DBPath);
            InstanceList.ItemsSource = _temp;
            if (Functions.IsPWDRemember && Functions.IsLogin)
            {
                App.IsLoginCall = true;
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
            var value = e.Item as InstanceList;
            //  Constants.Selected_Instance = value.InstanceId;
            this.Navigation.PushAsync(new LoginPage(value) { BindingContext = value });
            InstanceList.SelectedItem = null;
        }
    }
}
