using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StemmonsMobile.Views.View_Case_Origination_Center;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Views.LoginProcess;
using static StemmonsMobile.DataTypes.DataType.Cases.OriginationCenterDataResponse;
using System.Globalization;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseDetailsView : ContentPage
    {
        string CASETYPEID;

        OriginationCenterData CaseDetails = new OriginationCenterData();
        public CaseDetailsView(OriginationCenterData _caseDetails)
        {
            InitializeComponent();
            App.SetConnectionFlag();
            CaseDetails = _caseDetails;

            try
            {
                this.Title = CaseDetails.CaseTypeName;

                //CASETYPEID = Convert.ToString(CaseDetails.CaseTypeID);

                //List<CaseTypeData> lst = new List<CaseTypeData>
                //{
                //    new CaseTypeData("Open Cases",Convert.ToString(CaseDetails.OpenCaseCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Closed Cases",Convert.ToString(CaseDetails.ClosedCaseCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Total Cases",Convert.ToString(CaseDetails.TotalCaseCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Past Due Total Cases",Convert.ToString(CaseDetails.TotalPastDueCaseCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Assigned To Me",Convert.ToString(CaseDetails.AssignedToMeCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("My Past Due Cases",Convert.ToString(CaseDetails.PastDueAssignedToMeCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Created By Me",Convert.ToString(CaseDetails.CreatedByMeCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Closed By Me",Convert.ToString(CaseDetails.ClosedByMeCount),"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Default Hopper",CaseDetails.DefaultHopperName,"Assets/dropdowniconClose.png"),
                //    new CaseTypeData("Owner",CaseDetails.CaseTypeOwnerName,"Assets/dropdowniconClose.png"),
                //};
                List<CaseTypeData> lst = new List<CaseTypeData>
                {
                    new CaseTypeData("Open Cases",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.OpenCaseCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Closed Cases",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.ClosedCaseCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Total Cases",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.TotalCaseCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Past Due Total Cases",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.TotalPastDueCaseCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Assigned To Me",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.AssignedToMeCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("My Past Due Cases",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.PastDueAssignedToMeCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Created By Me",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.CreatedByMeCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Closed By Me",Convert.ToString(string.Format(CultureInfo.InvariantCulture,"{0:#,#}",(CaseDetails.ClosedByMeCount))),"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Default Hopper",CaseDetails.DefaultHopperName,"Assets/dropdowniconClose.png"),
                    new CaseTypeData("Owner",CaseDetails.CaseTypeOwnerName,"Assets/dropdowniconClose.png"),
                };


                listCaseDetails.ItemsSource = lst;
            }
            catch (Exception ex)
            {

                //  
            }
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                ListView lst = (ListView)sender;
                var data = (CaseTypeData)lst.SelectedItem;
                string value = data.Description;
                string seacrchvalue = string.Empty;
                switch (data.Caseoptions)
                {
                    case "Assigned To Me":

                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailAssignedToMe";

                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailAssignedToMe"));
                        //}

                        break;
                    case "Created By Me":
                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailCreatedByMe";

                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailCreatedByMe"));
                        //}

                        break;
                    case "Closed By Me":
                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailClosedByMe";
                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailClosedByMe"));
                        //}

                        break;
                    case "Open Cases":
                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailOpenCases";
                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailOpenCases"));
                        //}

                        break;
                    case "Total Cases":

                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailTotalCases";
                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailTotalCases"));
                        //}

                        break;
                    case "Closed Cases":
                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailClosesCases";
                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailClosesCases"));
                        //}

                        break;
                    case "Past Due Total Cases":
                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailPastTCases";

                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailPastTCases"));
                        //}

                        break;
                    case "My Past Due Cases":

                        if (!string.IsNullOrEmpty(data.Description))
                            seacrchvalue = "caseDetailPastMyCases";
                        //if (string.IsNullOrEmpty(data.Description))
                        //{

                        //}
                        //else
                        //{
                        //    this.Navigation.PushAsync(new CaseList("casetypeid", CASETYPEID, "caseDetailPastMyCases"));
                        //}

                        break;
                }

                 this.Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(CaseDetails.CaseTypeID), seacrchvalue));
            }
            catch (Exception ex)
            {
            }
        }

        public async void NewCaseCreat_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NewCase("0", Convert.ToString(CaseDetails.CaseTypeID)));
                listCaseDetails.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        private async void Footer_CreateCase(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NewCase("0", Convert.ToString(CaseDetails.CaseTypeID)));
                listCaseDetails.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        private async void HomeIcon_Click(object sender, EventArgs e)
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

    public class CaseTypeData
    {
        public string Caseoptions { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public CaseTypeData(string Caseoptions, string Description, string Icon)
        {
            this.Caseoptions = Caseoptions;
            this.Description = Description;
            this.Icon = Icon;
        }
    }
}
