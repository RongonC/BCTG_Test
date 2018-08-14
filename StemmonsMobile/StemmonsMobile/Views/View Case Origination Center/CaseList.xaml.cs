
using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Plugin.Connectivity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.Cases;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.Search;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.View_Case_Origination_Center
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseList : ContentPage
    {

        List<GetCaseTypesResponse.BasicCase> lst = new List<GetCaseTypesResponse.BasicCase>();
        List<string> TeamUserList = new List<string>();
        string parametername; string value; string searchvalue;
        string Username = string.Empty;
        string sTitle = string.Empty;
        string scrnName = string.Empty;
        bool isOnlineCall = true;

        public CaseList(string _parametername, string _value, string _searchvalue, string _Titile = "", string _uname = "")
        {
            InitializeComponent();
            App.SetConnectionFlag();
            parametername = _parametername;
            value = _value;
            Username = _uname;
            searchvalue = _searchvalue;
            sTitle = _Titile == "" ? "Case List" : _Titile;

            listdata.RefreshCommand = RefreshCommand;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Title = sTitle;
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                await SyncSqlitetoOnlineFromViewcaseonly(true, overlay, masterGrid);

                Getcaselistdatafromapi(parametername, value, searchvalue);
            }
            catch (Exception ex)
            {

            }
        }
        public static async Task SyncSqlitetoOnlineFromViewcaseonly(bool isMainthreadcall, ContentView overlay, Grid grd)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    Functions.ShowOverlayView_Grid(overlay, true, grd);

                    var list = DBHelper.GetItemTranInfoListList(App.DBPath);
                    list.Wait();
                    var QueueCount = list?.Result?.Where(v => v.PROCESS_ID == 0).Count();
                    if (QueueCount > 0)
                    {
                        if (isMainthreadcall)
                        {
                            await Task.Run(() =>
                            {
                                HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);
                            });
                        }
                        else
                        {
                            var _task = Task.Run(() =>
                            {
                                HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);
                            });
                        }
                    }

                    Functions.ShowOverlayView_Grid(overlay, false, grd);

                }
            }
            catch (Exception ex)
            {

            }
        }

        List<spi_MobileApp_GetCaseTypeDataByUserResult> User_CaseTypeData = new List<spi_MobileApp_GetCaseTypeDataByUserResult>();

        #region variables
        string CasetypeSecurity = string.Empty;
        string casetypeid = string.Empty;
        string caseOwnerSam = string.Empty;
        string caseAssgnSam = string.Empty;
        string caseClosebySam = string.Empty;
        string CaseCreateBySam = string.Empty;
        string propertyId = string.Empty;
        string tenant_id = string.Empty;
        string tenant_code = string.Empty;
        string showOpenClosetype = string.Empty;
        string searchquery = string.Empty;
        string OwnerSam = string.Empty;
        string AssgnSam = string.Empty;
        string AssgnSamTM = string.Empty;
        string CreateBySam = string.Empty;
        string showpastcase = string.Empty;
        string Samusername = string.Empty;
        bool saveRec = false;
        #endregion
        async void Getcaselistdatafromapi(string param, string val, string searchvalue)
        {
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                lst = new List<GetCaseTypesResponse.BasicCase>();

                #region Variable Settings

                switch (param)
                {
                    case "casetypeid":
                        casetypeid = val;
                        switch (searchvalue)
                        {
                            case "caseDetailAssignedToMe":
                                caseAssgnSam = Functions.UserName;
                                break;
                            case "caseDetailCreatedByMe":
                                CaseCreateBySam = Functions.UserName;
                                break;
                            case "caseDetailClosedByMe":
                                caseClosebySam = Functions.UserName;
                                break;
                            case "caseDetailClosesCases":
                                showOpenClosetype = "C";
                                break;
                            case "caseDetailOpenCases":
                                showOpenClosetype = "O";
                                break;
                            case "caseDetailTotalCases":
                                showOpenClosetype = "A";
                                break;
                            case "caseDetailPastTCases":
                                showpastcase = "Y";
                                break;
                            case "caseDetailPastMyCases":
                                showpastcase = "Y";
                                caseAssgnSam = Functions.UserName;
                                break;
                        }

                        break;
                    case "caseOwnerSAM":
                        caseOwnerSam = val;
                        scrnName = "_OwnedByMe";
                        OwnerSam = Functions.UserName;

                        break;
                    case "caseAssgnSAM":
                        caseAssgnSam = val;
                        scrnName = "_AssignedToMe";
                        AssgnSam = Functions.UserName;

                        break;
                    case "caseAssgnTM":
                        caseAssgnSam = val;
                        scrnName = "_AssignedToMyTeam";
                        AssgnSamTM = Functions.UserName;

                        break;
                    case "caseCloseBySAM":
                        caseClosebySam = val;
                        break;
                    case "caseCreateBySAM":
                        CaseCreateBySam = val;
                        scrnName = "_CreatedByMe";
                        CreateBySam = Functions.UserName;

                        break;
                    case "propertyId":
                        propertyId = val;
                        break;
                    case "tenant_Id":
                        tenant_id = val;
                        break;
                    case "tenant_Code":
                        tenant_code = val;
                        break;
                    case "showOpenClosedCasesType":
                        showOpenClosetype = val;
                        break;
                    case "searchQuery":
                        searchquery = val;
                        break;
                    case "showpastcase":
                        showpastcase = val;
                        break;
                    default:
                        //  showpastcase = val;
                        break;
                }

                if (string.IsNullOrEmpty(Username))
                {
                    Samusername = Functions.UserName;
                    saveRec = true;
                }
                else
                {
                    Samusername = Username;
                    saveRec = false;
                }
                #endregion

                if (!string.IsNullOrEmpty(OwnerSam))
                {
                    scrnName = "_OwnedByMe";
                    isOnlineCall = false;
                }
                else if (!string.IsNullOrEmpty(AssgnSam))
                {
                    isOnlineCall = false;
                }
                else if (!string.IsNullOrEmpty(CreateBySam))
                {
                    isOnlineCall = false;
                }
                else if (!string.IsNullOrEmpty(AssgnSamTM))
                {
                    isOnlineCall = false;
                }
                else
                {
                    isOnlineCall = App.Isonline;
                }

                await Task.Run(() =>
                {
                    dynamic result = null;

                    result = CasesSyncAPIMethods.GetCaseList(isOnlineCall, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, sTitle, saveRec, scrnName);
                    result.Wait();

                    lst = result.Result;
                });

                this.listdata.ItemsSource = null;
                if (lst.Count > 0)
                    this.listdata.ItemsSource = lst;
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    listdata.ItemsSource = lst;
                }
                else
                {
                    var rt = lst.Where(v => v.CaseTitle != null).ToList();
                    var list = rt.Where(x => x.CaseTitle != null && x.CaseTitle.Contains(e.NewTextValue)).ToList();
                    if (list.Count > 0)
                    {
                        listdata.ItemsSource = list;
                    }
                    else
                    {
                        listdata.ItemsSource = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView g = (ListView)sender;
            var sd = g.SelectedItem as GetCaseTypesResponse.BasicCase;
            await Navigation.PushAsync(new ViewCasePage(Convert.ToString(sd.CaseID), Convert.ToString(sd.CaseTypeID), sd.CaseTypeName, scrnName));
            listdata.SelectedItem = null;
        }

        private async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {

                dynamic action = null;
                if (Functions.HasTeam)
                {
                    action = await this.DisplayActionSheet(null, "Cancel", null, "Assigned to me", "Created by me", "Owned by me", "Assigned to my team");
                }
                else
                {
                    action = await this.DisplayActionSheet(null, "Cancel", null, "Assigned to me", "Created by me", "Owned by me");
                }

                switch (action)
                {
                    case "Assigned to my team":
                        await SelectUser(Functions.UserName);
                        break;
                    case "Assigned to me":

                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        this.listdata.ItemsSource = lst.Where(x => x.CaseAssignedToSAM.ToLower().Contains(Functions.UserName));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    case "Created by me":
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        this.listdata.ItemsSource = lst.Where(x => x.CaseCreatedSAM.ToLower().Contains(Functions.UserName));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        Getcaselistdatafromapi("caseCreateBySAM", Functions.UserName, "");
                        break;
                    case "Owned by me":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        this.listdata.ItemsSource = lst.Where(x => x.CaseOwnerSAM.ToLower().Contains(Functions.UserName));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                        break;

                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task SelectUser(string name)
        {
            try
            {
                var result1 = CasesAPIMethods.GetTeamMembers(name);
                var temp = result1.GetValue("ResponseContent");

                List<TeamMateData> lst1 = new List<TeamMateData>();
                foreach (var item in temp)
                {
                    TeamMateData casestdata;
                    casestdata = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamMateData>(item.ToString());
                    lst1.Add(casestdata);
                    TeamUserList.Add(casestdata.SAMName);
                }

                var count = TeamUserList.Count();
                var options = new List<string>(TeamUserList.Take(count + 1));
                options.Add("Search for User...");
                var userAction = await this.DisplayActionSheet(null, "Cancel", null, options.ToArray());
                if (userAction == "Search for User...")
                {

                    var result = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                          "Please search for a user to show the cases assigned to.").SetInputMode(
                          InputType.Name).SetOkText("Find"));

                    if (result.Ok)
                    {
                        SearchAgain(result.Text);
                    }
                }
                else
                {
                    Getcaselistdatafromapi("caseAssgnSAM", userAction, "");
                }
            }
            catch (Exception)
            {
            }

        }

        public async void SearchAgain(string Username)
        {
            try
            {
                TeamUserList = new List<string>();

                var otheruserApicall = CasesAPIMethods.GetEmployeesBySearch(Username);
                var responseData = otheruserApicall.GetValue("ResponseContent");

                if (responseData != null && responseData.ToString() != "[]")
                {
                    UserDataCall userdata;
                    List<UserDataCall> d = new List<UserDataCall>();

                    foreach (var item in responseData)
                    {
                        userdata = JsonConvert.DeserializeObject<UserDataCall>(item.ToString());
                        d.Add(userdata);
                        TeamUserList.Add(userdata.UserID);
                    }

                }

                var count1 = TeamUserList.Count();
                var options1 = new List<string>(TeamUserList.Take(count1 + 1));
                options1.Add("Search Again...");
                var userAction1 = await this.DisplayActionSheet(null, "Cancel", null, options1.ToArray());
                if (userAction1 == "Search Again...")
                {
                    var result = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                         "Please search for a user to show the cases assigned to.").SetInputMode(
                         InputType.Name).SetOkText("Find"));

                    if (result.Ok)
                    {
                        SearchAgain(result.Text);
                    }
                }
                else
                {
                    Getcaselistdatafromapi("caseAssgnSAM", userAction1, "");
                }
            }
            catch (Exception)
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

        private async void btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);
            }
            catch (Exception)
            {
            }
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    try
                    {
                        //Getcaselistdatafromapi(parametername, value, searchvalue, true);
                        Task.Run(() =>
                        {
                            dynamic result = null;

                            result = CasesSyncAPIMethods.GetCaseList(App.Isonline, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, sTitle, saveRec, scrnName);
                            result.Wait();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                listdata.IsRefreshing = false;
                                //  WarningLabel.IsVisible = false;
                                lst = result.Result;
                                this.listdata.ItemsSource = lst;
                            });
                        });

                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }

        private async void listdata_Refreshing(object sender, EventArgs e)
        {
            // listdata.IsRefreshing = true;
            //await Task.Run(async () =>
            //{
            // Getcaselistdatafromapi(parametername, value, searchvalue, true);
            Device.BeginInvokeOnMainThread(() =>
            {
                //listdata.IsRefreshing = false;
            });
            //});
        }
    }
    public class getcaselistdata
    {
        public string rowID { get; set; }
        public string CaseID { get; set; }
        public string ListID { get; set; }
        public string CaseTypeID { get; set; }
        public string CaseTypeName { get; set; }
        public string CaseOwnerDateTime { get; set; }
        public string CaseOwnerDateTimeDateOnly { get; set; }
        public string CaseOwnerSAM { get; set; }
        public string CaseOwnerDisplayName { get; set; }
        public string CaseAssignDateTime { get; set; }
        public string CaseAssignDateTimeDateOnly { get; set; }
        public string CaseAssignedToSAM { get; set; }
        public string CaseAssignedToDisplayName { get; set; }
        public string CaseClosedDateTime { get; set; }
        public string CaseClosedDateTimeDateOnly { get; set; }
        public string CaseClosedBySAM { get; set; }
        public string CaseClosedByDisplayName { get; set; }
        public string CaseCreatedDateTime { get; set; }
        public string CaseCreatedDateTimeDateOnly { get; set; }
        public string CaseCreatedSAM { get; set; }
        public string CaseCreatedDisplayName { get; set; }
        public string CaseModifiedDateTime { get; set; }
        public string CaseModifiedDateTimeDateOnly { get; set; }
        public string CaseTitleLink { get; set; }
        public string CaseMobileSummeryLink { get; set; }
        public string AdditionalTokenQueryString { get; set; }
        public string CaseModifiedBySAM { get; set; }
        public string CaseModifiedByDisplayName { get; set; }
        public string CaseLifeDHM { get; set; }
        public string CaseTitle { get; set; }
        public string CaseStatusSystemName { get; set; }
        public string CaseStatusValue { get; set; }
        public string CaseStatusSystemCode { get; set; }
        public string CasePriorityValue { get; set; }
        public string CasePrioritySystemCode { get; set; }
        public string CaseCost { get; set; }
        public string CaseDue { get; set; }
        public string CategoryName { get; set; }
        public string CategorySystemCode { get; set; }
        public string PropertyName { get; set; }
        public string TenantName { get; set; }
        public string RegionName { get; set; }
        public string MarketName { get; set; }
        public string SubMarketName { get; set; }
        public string PropertyID { get; set; }
        public string TenantCode { get; set; }
        public string TenantID { get; set; }
        public string DepartmentName { get; set; }
        public string CaseTypeInstanceName { get; set; }
        public string CaseTypeInstanceNamePlural { get; set; }
        public string DebugMode { get; set; }
        public string CaseTitleModal { get; set; }
        public string DisplayListID { get; set; }
        public string PropertyLink { get; set; }
        public string PropertyLinkImage { get; set; }
        public string PropertyURL { get; set; }
        public string TenantURL { get; set; }
        public string RegionURL { get; set; }
        public string MarketURL { get; set; }
        public string SubMarketURL { get; set; }
        public string DaysOverDue { get; set; }
        public string ThresholdDays { get; set; }
        public string CssStyel { get; set; }
        public string SortOnTop { get; set; }
        public string CaseDisplayFormat { get; set; }
        public string SecurityType { get; set; }

        public string PriorityValue
        {
            get
            {
                if (CasePriorityValue != "")
                {
                    return $"{"Priority:"}{CasePriorityValue}";
                }
                else
                    return $"{"Priority:0"}";
            }
        }

        public string AssignedTo
        {
            get
            {
                return $"{"Assigned To:"}{CaseAssignedToDisplayName}";
            }
        }

        public string CaseStatus
        {
            get
            {
                return $"{"Status:"}{CaseStatusSystemName}";
            }
        }
    }


    public class TeamMateData
    {

        public string EmployeeId { get; set; }
        public string EmployeeGuid { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string SAMName { get; set; }
    }

    public class HopperData
    {
        public string User_Name { get; set; }
        public string User_Position { get; set; }
        public string Img { get; set; }
        public HopperData(string name, string pos, string _img)
        {
            User_Name = name;
            User_Position = pos;
            Img = _img;
        }
    }

    public class UserDataCall
    {
        public string ID { get; set; }
        public string MiddleName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PrimaryJobTitle { get; set; }
        public string CellPhone { get; set; }

        public string Department { get; set; }
        public string OfficePhone { get; set; }

        public string Email { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string DisplayName { get; set; }

        public string UserID { get; set; }
        public string Supervisor { get; set; }
    }


}