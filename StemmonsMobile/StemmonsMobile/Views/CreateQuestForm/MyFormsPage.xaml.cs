using System;
using System.Linq;
using System.Collections.Generic;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using Xamarin.Forms;
using StemmonsMobile.Views.LoginProcess;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;

namespace StemmonsMobile.Views.CreateQuestForm
{
    public partial class MyFormsPage : ContentPage
    {
        String formtype = string.Empty;
        String pagetitle = string.Empty;
        List<ItemInstanceTranToProcessCase> lst = new List<ItemInstanceTranToProcessCase>();
        dynamic FormsData = null;

        public MyFormsPage(String FromType, string PageTitle)
        {
            InitializeComponent();
            formtype = FromType;
            pagetitle = PageTitle;
            this.Title = pagetitle;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(async () =>
                {
                    var formdatacall = QuestSyncAPIMethods.GetQuestFormsForUser(App.Isonline, Functions.UserName, App.DBPath);
                    lst = formdatacall?.Result;
                });

                if (lst.Count > 0)
                {

                    if (formtype == "Open")
                    {
                        FormsData = lst.FindAll(x => x.strIsLock.ToLower() == "unlocked");
                        FormData.ItemsSource = FormsData;
                    }
                    else if (formtype == "Completed")
                    {
                        FormsData = lst.FindAll(x => x.strIsLock.ToString() == "Locked");
                        FormData.ItemsSource = FormsData;
                    }
                    else
                    {
                        FormData.ItemsSource = lst;
                    }
                }
                else
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    await this.Navigation.PopAsync();

                }
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            }
            catch (Exception ex)
            {

            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                ListView l = (ListView)sender;
                var sd = (ItemInstanceTranToProcessCase)l.SelectedItem;

                this.Navigation.PushAsync(new QuestItemDetailPage(Convert.ToString(sd.intItemID), Convert.ToString(sd.intItemInstanceTranID), "", "", "", Convert.ToBoolean(sd.blnIsEdit), Convert.ToString(sd.intAreaID)));
            }
            catch (Exception ex)
            {

            }
        }

        void FocusedEvent(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            SearchBar sh = (SearchBar)sender;

            if (sh.IsFocused)
            {
                sh.Text = " ";
            }
            else
            {
                sh.Unfocus();
            }
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            SearchBar sh = (SearchBar)sender;
            try
            {

                if (string.IsNullOrEmpty(e.NewTextValue.Trim()))
                {
                    FormData.ItemsSource = lst;
                    if (!sh.IsFocused)
                        sh.Unfocus();
                }
                else
                {


                    FormData.ItemsSource = lst.FindAll(x => x.strAreaName.ToLower().Contains(e.NewTextValue.ToLower()));

                }
            }
            catch (Exception ex)
            {

                sh.Unfocus();
                sh.Text = "";
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

    public class MyFormData
    {
        public string intItemInstanceTranID { get; set; }
        public string intItemID { get; set; }
        public string intListID { get; set; }
        public string blnIsEdit { get; set; }
        public string strItemName { get; set; }
        public string strOtherComments { get; set; }
        public string intItemInstanceTranCaseID { get; set; }
        public string strNotes { get; set; }
        public string intCaseID { get; set; }
        public string intAreaID { get; set; }
        public string strAreaName { get; set; }
        public string strInstanceUser { get; set; }
        public string strInstanceUserMGR { get; set; }
        public string dcOverAllScore { get; set; }
        public string intItemThresholdID { get; set; }
        public string intThresholdTypeID { get; set; }
        public string dcPointsEarnedThreshold { get; set; }
        public string strAssignCaseTo { get; set; }
        public string blnIsUserCreatedCMECase { get; set; }
        public string blnIsContinueWithRules { get; set; }
        public string intOrderPriority { get; set; }
        public string strType { get; set; }
        public string strManager { get; set; }
        public string strLead { get; set; }
        public string strPropID { get; set; }
        public string strProperty { get; set; }
        public string strPropCode { get; set; }
        public string strMarketID { get; set; }
        public string strMarket { get; set; }
        public string strInspectedBy { get; set; }
        public string strInspectedBySam { get; set; }
        public string strSuite { get; set; }
        public string Date { get; set; }
        public string Overall { get; set; }
        public string CreatedDatetime { get; set; }
        public string strCreatedBy { get; set; }
        public string intQuarter { get; set; }
        public string intYear { get; set; }
        public string intTenantId { get; set; }
        public string strTenantName { get; set; }
        public string CreatedDateTime { get; set; }
        public string strIsLock { get; set; }
    }
}
