using System;
using System.Collections.Generic;
using System.Linq;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using StemmonsMobile.Models;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace StemmonsMobile.Views.Standards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyBookListPage : ContentPage
    {
        public MyBookListPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            Task<List<Grp_StandardDetails>> Response = null;

            try
            {
                await Task.Run(() =>
                {
                    Response = StandardsSyncAPIMethods.GetAppRelateToUserBasedOnSAM(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    Response.Wait();
                });
                if (Response.Result.Count > 0)
                {
                    List_MyBook.ItemsSource = Response.Result;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void List_MyBook_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var _item = ((ListView)sender).SelectedItem as Grp_StandardDetails;
                await Navigation.PushAsync(new BookContentsPage(_item));
            }
            catch (Exception)
            {
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
}