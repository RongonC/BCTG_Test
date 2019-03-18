using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Plugin.Connectivity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using StemmonsMobile.ViewModels;
using StemmonsMobile.ViewModels.CaseListViewModels;
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

        private GroupedViewModel caseListViewModel = new GroupedViewModel();

        public GroupedViewModel CaseListVM
        {
            get
            {
                if (caseListViewModel == null)
                    caseListViewModel = new GroupedViewModel();
                return caseListViewModel;
            }
            set
            {
                if (caseListViewModel == value)
                    return;
                caseListViewModel = value;
            }
        }

        private UnGroupedViewModel simpleCaseListModel = new UnGroupedViewModel();

        public UnGroupedViewModel SimpleCaseListVM
        {
            get
            {
                if (simpleCaseListModel == null)
                    simpleCaseListModel = new UnGroupedViewModel();
                return simpleCaseListModel;
            }
            set
            {
                if (simpleCaseListModel == value)
                    return;
                simpleCaseListModel = value;
            }
        }


        //ObservableCollection<GetCaseTypesResponse.BasicCase> CaseListVM.Master_list = new ObservableCollection<GetCaseTypesResponse.BasicCase>();
        ObservableCollection<Group_Caselist> Master_list = new ObservableCollection<Group_Caselist>();
        private ObservableCollection<Group_Caselist> _expandedGroups;
        List<string> TeamUserList = new List<string>();
        string parametername; string value; string searchvalue;
        string Team_Username = string.Empty;
        /// <summary>
        /// scrnName = "" means call comes from Assigned to me and so on
        string scrnName = string.Empty;
        /// </summary>
        bool isOnlineCall = true;
        int? _pageindex = 1;
        bool IsListGrouped = false;
        int? _pagenumber = 50;
        bool IsEntityRelationalData = false;
        bool isFirstAppearing = true;

        public CaseList(string _parametername, string _value, string _searchvalue, string _Titile = "", string _uname = "", bool isEntityRelationalData = false)
        {
            InitializeComponent();
            App.SetConnectionFlag();
            IsEntityRelationalData = isEntityRelationalData;
            parametername = _parametername;
            value = _value;
            Team_Username = _uname;
            searchvalue = _searchvalue;
            Title = _Titile == "" ? "Case List" : _Titile;

            if (parametername == "caseAssgnSAM" || parametername == "caseCreateBySAM" || parametername == "caseOwnerSAM")
            {
                //This shoudl be Grouped List
                if (_value != Functions.UserName)
                {
                    App.Isonline = true;
                }
                else
                {
                    App.Isonline = false;
                }
                IsListGrouped = true;
                btn_add.IsVisible = false;
                simpleCaseList.IsVisible = false;
                CaselistGrouped.IsVisible = true;
            }
            else if (parametername == "RELATEDCASES")
            {
                //This shoudl be Grouped List
                App.Isonline = true;
                IsListGrouped = true;
                btn_add.IsVisible = true;
                CaselistGrouped.IsVisible = true;
                simpleCaseList.IsVisible = false;
            }
            else
            {
                //it will no to be Grouped List
                App.Isonline = true;
                IsListGrouped = false;
                btn_add.IsVisible = true;
                CaselistGrouped.IsVisible = false;
                simpleCaseList.IsVisible = true;
            }

            #region Page List Actions
            //listdata.RefreshCommand = RefreshCommand;

            //listdata.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            //{
            //    try
            //    {
            //        if (App.Isonline)
            //        {
            //            var item = e.Item as GetCaseTypesResponse.BasicCase;
            //            int index = 0;
            //            try
            //            {
            //                index = CaseListVM.Master_list.IndexOf(item);
            //            }
            //            catch (Exception eqs)
            //            {

            //            }
            //            if (CaseListVM.Master_list.Count - 2 <= index)
            //            {
            //                if (CaseListVM.Master_list.Count == (_pagenumber * _pageindex))
            //                {
            //                    Device.BeginInvokeOnMainThread(() =>
            //                    {
            //                        lstfooter_indicator.IsVisible = true;
            //                    });
            //                    await Task.Run(() =>
            //                    {
            //                        _pageindex++;

            //                        var result = CasesSyncAPIMethods.GetCaseList(true, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, saveRec, scrnName, _pageindex, _pagenumber);
            //                        result.Wait();

            //                        Device.BeginInvokeOnMainThread(() =>
            //                        {
            //                            try
            //                            {
            //                                foreach (var ite in result.Result)
            //                                {
            //                                    CaseListVM.Master_list.Add(ite);
            //                                }
            //                                Master_List_Function(CaseListVM.Master_list);
            //                                //this.listdata.ItemsSource = CaseListVM.Master_list;
            //                            }
            //                            catch (Exception)
            //                            {
            //                            }
            //                        });

            //                    });
            //                    Device.BeginInvokeOnMainThread(() =>
            //                    {
            //                        lstfooter_indicator.IsVisible = false;
            //                    });
            //                }
            //            }
            //        }
            //    }
            //    catch (Exception)
            //    {
            //    }
            //}; 
            #endregion
        }
        int count = 0;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                //Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                CaselistGrouped.SelectedItem = null;
                if (isFirstAppearing)
                {
                    isFirstAppearing = false;
                    if (count == 0)
                    {
                        SyncSqlitetoOnlineFromViewcaseonly(true, overlay, masterGrid);

                        Getcaselistdatafromapi(parametername, value, searchvalue);
                    }
                }
                //Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                //UpdateListContent();
            }
            catch (Exception ex)
            {
            }
        }
        private async Task GetCaseListFromVM()
        {
            if (IsEntityRelationalData)
            {
                CaseListVM.EntityID = 0;
                CaseListVM.Casetypeid = this.casetypeid;
                CaseListVM.CaseOwnerSam = this.caseOwnerSam;
                CaseListVM.CaseAssgnSam = this.caseAssgnSam;
                CaseListVM.CaseCloseBySam = this.caseClosebySam;
                CaseListVM.CaseCreateBySam = this.CaseCreateBySam;
                CaseListVM.PropertyId = this.propertyId;
                CaseListVM.TenantCode = this.tenant_code;
                CaseListVM.TenantId = this.tenant_id;
                CaseListVM.ShowOpenCloseType = this.showOpenClosetype;
                CaseListVM.ShowPastCase = this.showpastcase;
                CaseListVM.SearchQuery = this.searchquery;
                CaseListVM.SaveRec = this.saveRec;
                CaseListVM.ScrnName = this.scrnName;
                CaseListVM.PageNumber = this._pagenumber;
                await CaseListVM.GetCaseListByEntityID();
            }
            else
            {
                if (IsListGrouped)
                {
                    CaseListVM.Casetypeid = this.casetypeid;
                    CaseListVM.CaseOwnerSam = this.caseOwnerSam;
                    CaseListVM.CaseAssgnSam = this.caseAssgnSam;
                    CaseListVM.CaseCloseBySam = this.caseClosebySam;
                    CaseListVM.CaseCreateBySam = this.CaseCreateBySam;
                    CaseListVM.PropertyId = this.propertyId;
                    CaseListVM.TenantCode = this.tenant_code;
                    CaseListVM.TenantId = this.tenant_id;
                    CaseListVM.ShowOpenCloseType = this.showOpenClosetype;
                    CaseListVM.ShowPastCase = this.showpastcase;
                    CaseListVM.SearchQuery = this.searchquery;
                    CaseListVM.SaveRec = this.saveRec;
                    CaseListVM.ScrnName = this.scrnName;
                    CaseListVM.PageNumber = this._pagenumber;

                    await CaseListVM.GetCaseListWithCall();
                }
                else
                {
                    SimpleCaseListVM.Casetypeid = this.casetypeid;
                    SimpleCaseListVM.CaseOwnerSam = this.caseOwnerSam;
                    SimpleCaseListVM.CaseAssgnSam = this.caseAssgnSam;
                    SimpleCaseListVM.CaseCloseBySam = this.caseClosebySam;
                    SimpleCaseListVM.CaseCreateBySam = this.CaseCreateBySam;
                    SimpleCaseListVM.PropertyId = this.propertyId;
                    SimpleCaseListVM.TenantCode = this.tenant_code;
                    SimpleCaseListVM.TenantId = this.tenant_id;
                    SimpleCaseListVM.ShowOpenCloseType = this.showOpenClosetype;
                    SimpleCaseListVM.ShowPastCase = this.showpastcase;
                    SimpleCaseListVM.SearchQuery = this.searchquery;
                    SimpleCaseListVM.SaveRec = this.saveRec;
                    SimpleCaseListVM.ScrnName = this.scrnName;
                    SimpleCaseListVM.PageNumber = this._pagenumber;

                    await SimpleCaseListVM.GetCaseListWithCall();
                }
            }

        }

        public static async void SyncSqlitetoOnlineFromViewcaseonly(bool isMainthreadcall, ContentView overlay, Grid grd)
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
                // CaseListVM.Master_list.Clear();

                #region Variable Settings

                switch (param)
                {
                    case "casetypeid":
                        casetypeid = val;
                        break;
                    case "RELATEDCASES":
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
                                showOpenClosetype = "y";
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
                        if(caseAssgnSam == Functions.UserName)
                        { scrnName = "_AssignedToMe";
                            AssgnSam = Functions.UserName; }
                        else{
                            scrnName = "_AssignedToSAM";
                            AssgnSam = val;
                        }

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

                if (string.IsNullOrEmpty(Team_Username))
                {
                    Samusername = Functions.UserName;
                    saveRec = true;
                }
                else
                {
                    Samusername = Team_Username;
                    saveRec = false;
                }

                if (scrnName == "_AssignedToSAM")
                {
                    saveRec = false;
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                }
                #endregion

                if ((!string.IsNullOrEmpty(OwnerSam)) || (!string.IsNullOrEmpty(AssgnSam)) || (!string.IsNullOrEmpty(CreateBySam)) || (!string.IsNullOrEmpty(AssgnSamTM)))
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

                await GetCaseListFromVM(); //Calling function from VM to get case list

                if (IsListGrouped)
                {
                    if (CaseListVM.Master_list.Count <= 0)
                    {
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            isOnlineCall = App.Isonline;
                            await Task.Run(() =>
                            {
                                var result = GetCaseListFromVM();

                                result.Wait();

                                //Device.BeginInvokeOnMainThread(() =>
                                //{
                                //    CaseListVM.Master_list = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                                //    //listdata.IsRefreshing = true;
                                //});
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    if (SimpleCaseListVM.BasicCase_lst.Count <= 0)
                    {
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            isOnlineCall = App.Isonline;
                            await Task.Run(() =>
                            {
                                var result = GetCaseListFromVM();

                                result.Wait();

                                //Device.BeginInvokeOnMainThread(() =>
                                //{
                                //    CaseListVM.Master_list = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                                //    //listdata.IsRefreshing = true;
                                //});
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                //if (CaseListVM.Master_list.Count <= 0)
                //{
                //    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                //    try
                //    {
                //        isOnlineCall = App.Isonline;
                //        await Task.Run(() =>
                //        {
                //            var result = CasesSyncAPIMethods.GetCaseList(App.Isonline, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, saveRec, scrnName, 1, _pagenumber);
                //            result.Wait();

                //            Device.BeginInvokeOnMainThread(() =>
                //            {
                //                CaseListVM.Master_list = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
                //               //listdata.IsRefreshing = true;
                //            });
                //        });
                //    }
                //    catch (Exception)
                //    {
                //    }
                //}

                #region To manage the Pageindex to get All data as we have in last
                if (_pageindex > 1)
                {
                    //for (int ind = 0; ind < _pageindex - 1; ind++)
                    //{
                    //    Task.Run(() =>
                    //    {
                    //        Device.BeginInvokeOnMainThread(() =>
                    //        {
                    //            lstfooter_indicator.IsVisible = true;
                    //        });
                    //        var result = CasesSyncAPIMethods.GetCaseList(isOnlineCall, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, saveRec, scrnName, ind, _pagenumber);
                    //        result.Wait();


                    //        Device.BeginInvokeOnMainThread(() =>
                    //        {
                    //            try
                    //            {
                    //                lstfooter_indicator.IsVisible = false;
                    //                if (result.Result != null)
                    //                    foreach (var ite in result.Result)
                    //                    {
                    //                        CaseListVM.Master_list.Add(ite);
                    //                    }
                    //            }
                    //            catch (Exception)
                    //            {
                    //            }
                    //        });
                    //    });
                    //}
                }
                #endregion

                Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                {
                    //listdata.IsRefreshing = false;
                    return false; // True = Repeat again, False = Stop the timer
                });
                //if (CaseListVM.Master_list.Count > 0)
                //    Master_List_Function(CaseListVM.Master_list);
                //else
                //{
                //    //listdata.IsRefreshing = false;
                //    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                //}
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
                //if (CaseListVM.Master_list.Count > 0)
                //{
                //    if (string.IsNullOrEmpty(e.NewTextValue))
                //    {
                //        Master_List_Function(CaseListVM.Master_list);
                //        //listdata.ItemsSource = CaseListVM.Master_list;
                //    }
                //    else
                //    {
                //        var rt = CaseListVM.Master_list.Where(v => v.CaseTitle != null).ToList();
                //        var list = rt.Where(x => x.CaseTitle != null && x.CaseTitle.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                //        if (list.Count > 0)
                //        {
                //            Master_List_Function(new ObservableCollection<GetCaseTypesResponse.BasicCase>(list));
                //            //listdata.ItemsSource = list;
                //        }
                //        else
                //        {
                //            //listdata.ItemsSource = null;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private async void listdata_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!string.IsNullOrEmpty(scrnName))
                count = 1;
            ListView g = (ListView)sender;
            var sd = g.SelectedItem as GetCaseTypesResponse.BasicCase;
            await Navigation.PushAsync(new ViewCasePage(Convert.ToString(sd.CaseID), Convert.ToString(sd.CaseTypeID), sd.CaseTypeName, scrnName));
            //listdata.SelectedItem = null;
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
                        var lst = new ObservableCollection<GetCaseTypesResponse.BasicCase>();
                        foreach (var item in CaseListVM.Master_list)
                        {
                            foreach (var Bscitem in item)
                            {
                                if (Bscitem.CaseAssignedToSAM.ToLower().Contains(Functions.UserName))
                                    lst.Add(Bscitem);
                            }
                        }

                        if (IsListGrouped)
                        {

                        }

                        Master_List_Function(lst);

                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    case "Created by me":
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        //this.listdata.ItemsSource = CaseListVM.Master_list.Where(x => x.CaseCreatedSAM.ToLower().Contains(Functions.UserName));
                        // Master_List_Function(new ObservableCollection<GetCaseTypesResponse.BasicCase>(CaseListVM.Master_list.Where(x => x.CaseCreatedSAM.ToLower().Contains(Functions.UserName))));
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        Getcaselistdatafromapi("caseCreateBySAM", Functions.UserName, "");
                        break;
                    case "Owned by me":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        //this.listdata.ItemsSource = CaseListVM.Master_list.Where(x => x.CaseOwnerSAM.ToLower().Contains(Functions.UserName));
                        //Master_List_Function(new ObservableCollection<GetCaseTypesResponse.BasicCase>(CaseListVM.Master_list.Where(x => x.CaseOwnerSAM.ToLower().Contains(Functions.UserName))));
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

                var options = new List<string>();

                options.AddRange(data.Select(v => v.DisplayName?.Trim() != null ? v.DisplayName?.Trim() : v.UserID?.Trim()));

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

        //New Code start - ArpanB
        private async void btn_add_Clicked(object sender, EventArgs e)
        {
            //count = 1;
            await this.Navigation.PushAsync(new NewCase("0", Convert.ToString(casetypeid)));
        }
        //New Code end

        #region Pull To refresh Case List
        //private bool _isRefreshing = false;
        //public bool IsRefreshing
        //{
        //    get { return _isRefreshing; }
        //    set
        //    {
        //        _isRefreshing = value;
        //        OnPropertyChanged(nameof(IsRefreshing));
        //    }
        //}

        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            IsRefreshing = true;

        //            try
        //            {
        //                //Getcaselistdatafromapi(parametername, value, searchvalue, true);
        //                Task.Run(() =>
        //                {
        //                    dynamic result = null;

        //                    result = CasesSyncAPIMethods.GetCaseList(App.Isonline, Samusername, casetypeid, caseOwnerSam, caseAssgnSam, caseClosebySam, CaseCreateBySam, propertyId, tenant_code, tenant_id, showOpenClosetype, showpastcase, searchquery, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, saveRec, scrnName, 0, _pagenumber);
        //                    result.Wait();
        //                    Device.BeginInvokeOnMainThread(() =>
        //                    {
        //                        try
        //                        {
        //                            //listdata.IsRefreshing = false;
        //                            //  WarningLabel.IsVisible = false;
        //                            if (result.Result.Count > 0)
        //                            {
        //                                var t = new ObservableCollection<GetCaseTypesResponse.BasicCase>(result.Result);
        //                                CaseListVM.Master_list = t;
        //                            }
        //                            Master_List_Function(CaseListVM.Master_list);
        //                            //this.listdata.ItemsSource = CaseListVM.Master_list;
        //                        }
        //                        catch (Exception)
        //                        {
        //                        }
        //                    });
        //                });

        //            }
        //            catch (Exception)
        //            {
        //            }
        //        });
        //    }
        //}
        #endregion

        private void Master_List_Function(ObservableCollection<GetCaseTypesResponse.BasicCase> basicCase_Item)
        {
            Master_list.Clear();
            var Grp = basicCase_Item.GroupBy(v => v.CaseTypeName);
            foreach (var item in Grp)
            {
                Group_Caselist fav_case = new Group_Caselist(item.Key);
                foreach (var ite in item)
                {
                    fav_case.Add(ite);
                }
                Master_list.Add(fav_case);
            }
            if (IsListGrouped)
            {

            }
            else
            {

            }

            UpdateListContent();
        }

        private void UpdateListContent()
        {
            try
            {
                _expandedGroups = new ObservableCollection<Group_Caselist>();
                foreach (var group in Master_list)
                {
                    //Create new FoodGroups so we do not alter original list
                    Group_Caselist newGroup = new Group_Caselist(group.Title, group.Expanded);
                    //Add the count of food items for Lits Header Titles to use

                    if (group.Expanded)
                    {
                        foreach (var ite in group)
                        {
                            newGroup.Add(ite);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                //listdata.SelectedItem = null;

                //listdata.ItemsSource = _expandedGroups;
                // Case_type_List.HeightRequest = 250;
            }
            catch (Exception)
            {
            }
        }

        private void HeaderTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                int selectedIndex = _expandedGroups.IndexOf(
               ((Group_Caselist)((Button)sender).CommandParameter));
                Master_list[selectedIndex].Expanded = !Master_list[selectedIndex].Expanded;
                UpdateListContent();

            }
            catch (Exception ex)
            {

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