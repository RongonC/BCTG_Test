
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

        ObservableCollection<GetCaseTypesResponse.BasicCase> BasicCase_lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>();
        List<string> TeamUserList = new List<string>();
        string parametername; string value; string searchvalue;
        string Username = string.Empty;
        string sTitle = string.Empty;
        string scrnName = string.Empty;
        bool isOnlineCall = true;
        int? _pageindex = 1;
        int? _pagenumber = 20;

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

            listdata.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            {
                try
                {
                    if (App.Isonline)
                    {
                        var item = e.Item as GetCaseTypesResponse.BasicCase;
                        int index = 0;
                        try
                        {
                            index = BasicCase_lst.IndexOf(item);
                        }
                        catch (Exception eqs)
                        {

                        }
                        if (BasicCase_lst.Count - 2 <= index)
                        {
                            if (BasicCase_lst.Count == (_pagenumber * _pageindex))
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    lstfooter_indicator.IsVisible = true;
                                });
                                await Task.Run(() =>
                                {
                                    _pageindex++;

                                    var result = CasesSyncAPIMethods.GetCaseList(true, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, sTitle, saveRec, scrnName, _pageindex, _pagenumber);
                                    result.Wait();

                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        try
                                        {
                                            foreach (var ite in result.Result)
                                            {
                                                BasicCase_lst.Add(ite);
                                            }

                                            this.listdata.ItemsSource = BasicCase_lst;
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    });

                                });
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    lstfooter_indicator.IsVisible = false;
                                });
                            }
                            //    try
                            //    {
                            //        int cn = lst.Count;
                            //        for (int i = 0; i < 20; i++)
                            //        {
                            //            try
                            //            {
                            //                GetCaseTypesResponse.BasicCase ob = new GetCaseTypesResponse.BasicCase();
                            //                ob.CaseTitle = "lazy Load - " + (cn++);
                            //                ob.CaseTypeID = 15;
                            //                ob.ListID = i;

                            //                lst.Add(ob);
                            //            }
                            //            catch (Exception f)
                            //            {
                            //            }
                            //        }

                            //    }
                            //    catch (Exception ex)
                            //    {
                            //    }
                            //    Device.BeginInvokeOnMainThread(() =>
                            //    {
                            //        listdata.ItemsSource = null;
                            //        this.listdata.ItemsSource = lst;
                            //    });
                        }
                    }
                }
                catch (Exception)
                {
                }
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Title = sTitle;
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
                BasicCase_lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>();

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
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                }

                if (!string.IsNullOrEmpty(scrnName))
                {
                    this.ToolbarItems.Remove(Filter);
                }


                await Task.Run(() =>
                {
                    var result = CasesSyncAPIMethods.GetCaseList(isOnlineCall, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, sTitle, saveRec, scrnName, 1, _pagenumber);
                    result.Wait();

                    BasicCase_lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        listdata.IsRefreshing = true;
                    });
                });

                if (_pageindex > 1)
                {
                    for (int ind = 0; ind < _pageindex - 1; ind++)
                    {
                        Task.Run(() =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                lstfooter_indicator.IsVisible = true;
                            });
                            var result = CasesSyncAPIMethods.GetCaseList(isOnlineCall, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, sTitle, saveRec, scrnName, ind, _pagenumber);
                            result.Wait();


                            Device.BeginInvokeOnMainThread(() =>
                            {
                                try
                                {
                                    lstfooter_indicator.IsVisible = false;
                                    if (result.Result != null)
                                        foreach (var ite in result.Result)
                                        {
                                            BasicCase_lst.Add(ite);
                                        }
                                }
                                catch (Exception)
                                {
                                }
                            });
                        });
                    }
                }
                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    listdata.IsRefreshing = false;
                    return false; // True = Repeat again, False = Stop the timer
                });
                this.listdata.ItemsSource = null;
                if (BasicCase_lst.Count > 0)
                    this.listdata.ItemsSource = BasicCase_lst;
                else
                {
                    listdata.IsRefreshing = false;
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void SearchCase_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (BasicCase_lst.Count > 0)
                {
                    if (string.IsNullOrEmpty(e.NewTextValue))
                    {
                        listdata.ItemsSource = BasicCase_lst;
                    }
                    else
                    {
                        var rt = BasicCase_lst.Where(v => v.CaseTitle != null).ToList();
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
            }
            catch (Exception ex)
            {
            }
        }

        private async void listdata_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView g = (ListView)sender;
            var sd = g.SelectedItem as GetCaseTypesResponse.BasicCase;
            await Navigation.PushAsync(new ViewCasePage(Convert.ToString(sd.CaseID), Convert.ToString(sd.CaseTypeID), sd.CaseTypeName, scrnName));
            listdata.SelectedItem = null;
        }

        private async void Filter_Clicked(object sender, EventArgs e)
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
                        this.listdata.ItemsSource = BasicCase_lst.Where(x => x.CaseAssignedToSAM.ToLower().Contains(Functions.UserName));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    case "Created by me":
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        this.listdata.ItemsSource = BasicCase_lst.Where(x => x.CaseCreatedSAM.ToLower().Contains(Functions.UserName));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        Getcaselistdatafromapi("caseCreateBySAM", Functions.UserName, "");
                        break;
                    case "Owned by me":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        this.listdata.ItemsSource = BasicCase_lst.Where(x => x.CaseOwnerSAM.ToLower().Contains(Functions.UserName));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                        break;

                }
            }
            catch (Exception ex)
            {
            }
        }

        //private async Task SelectUser1(string name)
        //{
        //    try
        //    {
        //        var result1 = CasesAPIMethods.GetTeamMembers(name);
        //        var temp = result1.GetValue("ResponseContent");

        //        List<TeamMateData> lst1 = new List<TeamMateData>();
        //        foreach (var item in temp)
        //        {
        //            TeamMateData casestdata;
        //            casestdata = Newtonsoft.Json.JsonConvert.DeserializeObject<TeamMateData>(item.ToString());
        //            lst1.Add(casestdata);
        //            TeamUserList.Add(casestdata.SAMName);
        //        }

        //        var count = TeamUserList.Count();
        //        var options = new List<string>(TeamUserList.Take(count + 1));
        //        options.Add("Search for User...");
        //        var userAction = await this.DisplayActionSheet(null, "Cancel", null, options.ToArray());
        //        if (userAction == "Search for User...")
        //        {

        //            var result = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
        //                  "Please search for a user to show the cases assigned to.").SetInputMode(
        //                  InputType.Name).SetOkText("Find"));

        //            if (result.Ok)
        //            {
        //                SearchAgain(result.Text);
        //            }
        //        }
        //        else
        //        {
        //            Getcaselistdatafromapi("caseAssgnSAM", userAction, "");
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }

        //}

        private async Task SelectUser(string name)
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                List<GetUserInfoResponse.UserInfo> Userlist = new List<GetUserInfoResponse.UserInfo>();
                await Task.Run(() =>
                {
                    var itemRec = CasesSyncAPIMethods.GetTeamMembers(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    itemRec.Wait();
                    Userlist = itemRec.Result;
                });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                if (Userlist?.Count > 0)
                {
                    var options = new List<string>();
                    options.AddRange(Userlist.Select(v => v.DisplayName?.Trim() != null ? v.DisplayName?.Trim() : v.SAMName?.Trim()));

                    //var count = project.Count();
                    //var options = new List<string>(project.Take(count + 1));
                    options.Add("Search for User.");
                    var userAction = await this.DisplayActionSheet(null, "Cancel", null, options.ToArray());


                    if (userAction == "Search for User.")
                    {

                        var result = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                              "Please search for a user to show the cases assigned to.").SetInputMode(
                              InputType.Name).SetOkText("Find"));

                        if (result.Ok)
                        {
                            SearchAgain(result.Text);
                        }
                    }
                    else if (userAction != "Cancel")
                    {
                        userAction = Userlist.Where(v => v.DisplayName != null ? v.DisplayName.ToLower() == userAction.ToLower() : v.SAMName.ToLower() == userAction.ToLower())?.FirstOrDefault().SAMName;

                        Getcaselistdatafromapi("caseAssgnSAM", userAction, "");
                    }
                }
            }
            catch (Exception)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        public async void SearchAgain(string Username)
        {
            try
            {
                TeamUserList = new List<string>();
                List<UserDataCall> data = new List<UserDataCall>();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(() =>
                 {
                     var otheruserApicall = CasesAPIMethods.GetEmployeesBySearch(Username);
                     var responseData = otheruserApicall.GetValue("ResponseContent");
                     data = JsonConvert.DeserializeObject<List<UserDataCall>>(responseData.ToString());
                 });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                //if (responseData != null && responseData.ToString() != "[]")
                //{
                //    UserDataCall userdata;
                //    List<UserDataCall> d = new List<UserDataCall>();

                //    foreach (var item in responseData)
                //    {
                //        userdata = JsonConvert.DeserializeObject<UserDataCall>(item.ToString());
                //        d.Add(userdata);
                //        TeamUserList.Add(userdata.UserID);
                //    }
                //}

                //var count1 = TeamUserList.Count();
                //var options1 = new List<string>(TeamUserList.Take(count1 + 1));
                //options1.Add("Search Again...");

                var options = new List<string>();

                options.AddRange(data.Select(v => v.DisplayName?.Trim() != null ? v.DisplayName?.Trim() : v.UserID?.Trim()));

                //var count = project.Count();
                //var options = new List<string>(project.Take(count + 1));
                options.Add("Search for User.");


                var userAction1 = await this.DisplayActionSheet(null, "Cancel", null, options.ToArray());
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

                            result = CasesSyncAPIMethods.GetCaseList(App.Isonline, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, sTitle, saveRec, scrnName, 0, 0);
                            result.Wait();
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                try
                                {
                                    listdata.IsRefreshing = false;
                                    //  WarningLabel.IsVisible = false;
                                    if (result.Result.Count > 0)
                                    {
                                        var t = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                                        BasicCase_lst = t;
                                    }
                                    this.listdata.ItemsSource = BasicCase_lst;
                                }
                                catch (Exception)
                                {
                                }
                            });
                        });

                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
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