using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using DataServiceBus.OnlineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Views.LoginProcess;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseActivityLog : ContentPage
    {

        List<GetCaseActivityResponse.Activity> List_CasesActivityLog = new List<GetCaseActivityResponse.Activity>();

        string CASEID = string.Empty;
        int CaseTypeID = 0;
        bool Onlineflag = true;
        public CaseActivityLog(string caseid, int _CaseTypeID, bool _onlineflag)
        {

            InitializeComponent();
            Onlineflag = _onlineflag;
            App.SetConnectionFlag();
            CaseTypeID = _CaseTypeID;
            CASEID = caseid;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //if (App.Isonline)
            {
                try
                {
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    Task.Run(() =>
                    {
                        CasesSyncAPIMethods.GetCaseActivity(App.Isonline, CASEID, CaseTypeID, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    });

                    await Task.Run(async () =>
                    {
                        List_CasesActivityLog = await CasesSyncAPIMethods.GetCaseActivity(Onlineflag, CASEID, CaseTypeID, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    });
                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                    if (List_CasesActivityLog.Count > 0)
                        listViewActivityLog.ItemsSource = List_CasesActivityLog;
                    else
                    {
                        await DisplayAlert(null, Onlineflag ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void HomeIcon_Click(object sender, EventArgs e)
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
