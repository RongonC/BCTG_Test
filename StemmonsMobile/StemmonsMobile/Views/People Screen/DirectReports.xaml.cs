using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.People_Screen;
using StemmonsMobile.Views.View_Case_Origination_Center;
using Xamarin.Forms;

namespace StemmonsMobile.Views.PeopleScreen
{
    public partial class DirectReports : ContentPage
    {
        string _userId = "";
        public DirectReports(string UserId)
        {
            InitializeComponent();
            _userId = UserId;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                List<GetUserInfoResponse.UserInfo> Teammatelst = new List<GetUserInfoResponse.UserInfo>();

                await Task.Run(() =>
                {
                    var result1 = CasesSyncAPIMethods.GetTeamMembers(App.Isonline, _userId, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    Teammatelst = result1.Result;
                });

                if (Teammatelst == null || Teammatelst.Count == 0)
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
                else
                {
                    TeamMemberList.ItemsSource = Teammatelst;
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView lst = (ListView)sender;
            var data = (GetUserInfoResponse.UserInfo)lst.SelectedItem;

            await Navigation.PushAsync(new UserDetail(data.SAMName));
        }
    }
}
