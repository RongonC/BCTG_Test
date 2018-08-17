using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.View_Case_Origination_Center;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Cases_Hopper_Center
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCaseHopper : ContentPage
    {
        string _username = string.Empty;
        string _displayfor = string.Empty;
        List<GetCaseListByHopperResponse.HopperCenterData> lst = new List<GetCaseListByHopperResponse.HopperCenterData>();
        public SelectCaseHopper(string username, string displayfor)
        {
            InitializeComponent();
            try
            {
                App.SetConnectionFlag();
                _username = username;
                _displayfor = displayfor;
            }
            catch (Exception ex)
            {
            }
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                lst = new List<GetCaseListByHopperResponse.HopperCenterData>();


                await Task.Run(() =>
                {
                    var result = CasesSyncAPIMethods.GetHopperCenterByUser(App.Isonline, _username, "Y", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    lst = result.Result;
                });

                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                if (lst != null && lst.Count > 0)
                {
                    if (_displayfor == "MyHopper")
                    {
                        lst = lst.Where(x => x.CaseTypeOwnerName != null && x.CaseTypeOwnerName.Contains(_username)).ToList();
                    }

                    if (lst == null || lst.Count == 0)
                    {
                        await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    }

                    HopperList.ItemsSource = lst;
                }
                else
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }

            }
            catch (Exception ex)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        void SubscribeClicked(object sender, System.EventArgs e)
        {
            try
            {
                if (App.Isonline)
                {
                    var mi = ((MenuItem)sender);
                    var sd = mi.CommandParameter as GetCaseListByHopperResponse.HopperCenterData;

                    var APICallSubscriber = CasesAPIMethods.SubscribeToHopper(sd.HopperUsername, Functions.UserName);
                    var ApiResult = APICallSubscriber.GetValue("Success");

                    if ((bool)ApiResult)
                    {
                        this.DisplayAlert("Subscription", "You have subscribed Successfully.", "Ok");
                    }
                    else
                    {
                        this.DisplayAlert("Subscription", "You have not subscribed please try again later.", "Ok");
                    }
                }
                else
                    this.DisplayAlert("Subscription", "Please Go Online to use this functionality.", "Ok");
            }
            catch (Exception ex)
            {

            }
        }

        void DetailClicked(object sender, System.EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var sd = mi.CommandParameter as GetCaseListByHopperResponse.HopperCenterData;


                this.Navigation.PushAsync(new SelectedHopper(Convert.ToString(sd.HopperName), Convert.ToString(sd.OpenCaseCount), Convert.ToString(sd.CreatedByMeCount), Convert.ToString(sd.OwnedByMeCount), Convert.ToString(sd.CaseTypeOwnerName)));
            }
            catch (Exception ex)
            {

            }
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar sh = (SearchBar)sender;
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    HopperList.ItemsSource = lst;
                    sh.Unfocus();
                }
                else
                {
                    //  FormList.ItemsSource = AreaIdlst.Where(x => x.strItemName.StartsWith(e.NewTextValue));
                    HopperList.ItemsSource = lst.Where(x => x.HopperName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
            catch (Exception ex)
            {

                // throw;
            }
        }


        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                ListView g = (ListView)sender;
                var sd = (GetCaseListByHopperResponse.HopperCenterData)g.SelectedItem;
                this.Navigation.PushAsync(new CaseList("caseAssgnSAM", sd.HopperUsername, ""));
            }
            catch (Exception ex)
            {

            }
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


    public class CasesHopperData
    {

        public string HopperName { get; set; }
        public string HopperUsername { get; set; }
        public string HopperID { get; set; }
        public string OpenCaseCount { get; set; }
        public string CreatedByMeCount { get; set; }
        public string OwnedByMeCount { get; set; }
        public string SPORGField { get; set; }
        public string CaseTypeOwnerEmail { get; set; }
        public string CaseTypeOwnerName { get; set; }
        public string CaseTypeOwnerHelpUrl { get; set; }
        public string HasSecurity { get; set; }
        public string OpenCaseCountLink { get; set; }
        public string CreatedByMeCountLink { get; set; }
        public string OwnedByMeCountLink { get; set; }
        public string SecurityGroups { get; set; }
        public string SecurityType { get; set; }
    }
}
