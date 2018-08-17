using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using Xamarin.Forms;

namespace StemmonsMobile.Views.PeopleScreen
{
    public partial class MyHoopers : ContentPage
    {
        string username;
        List<GetCaseTypesResponse.HopperInfo> MyHooper = new List<GetCaseTypesResponse.HopperInfo>();
        public MyHoopers(string UserName)
        {
            InitializeComponent();
            username = UserName;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                await Task.Run(() =>
                 {
                     var result = CasesSyncAPIMethods.GetHopperWithOwnerByUsername(App.Isonline, username, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                     MyHooper = result.Result;
                 });

                if (MyHooper.Count > 0)
                    HopperList.ItemsSource = MyHooper;
                else
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
            }
            catch (Exception ex)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
    }
}
