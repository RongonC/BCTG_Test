using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.View_Case_Origination_Center;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.GetFavoriteResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.OriginationCenterDataResponse;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCaseType : ContentPage
    {
        string select_favoriteId;
        string sfav_casetypeid = "";
        string json = "";
        ObservableCollection<OriginationCenterData> list = new ObservableCollection<OriginationCenterData>();
        ObservableCollection<GetFavorite> list_favorite = new ObservableCollection<GetFavorite>();

        ObservableCollection<Group_CaseType> Master_list = new ObservableCollection<Group_CaseType>();
        private ObservableCollection<Group_CaseType> _expandedGroups;

        public SelectCaseType()
        {
            InitializeComponent();
            App.SetConnectionFlag();
            Case_type_List.ItemsSource = Master_list;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            txt_seacrhbar.Text = "";
            Master_list.Clear();
            try
            {
                //Case_type_List.IsVisible = false;

                List<OriginationCenterDataResponse.OriginationCenterData> result = new List<OriginationCenterDataResponse.OriginationCenterData>();
                List<GetFavoriteResponse.GetFavorite> result_fav = new List<GetFavoriteResponse.GetFavorite>();

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                #region API Call
                await Task.Run(async () =>
                {
                    Task<List<OriginationCenterDataResponse.OriginationCenterData>> results = CasesSyncAPIMethods.GetOriginationCenterForUser(Functions.UserName, App.DBPath);
                    results.Wait();
                    result = results.Result;
                    if (result == null || result?.Count == 0)
                    {
                        if (App.Isonline)
                        {
                            result = CasesSyncAPIMethods.GetOriginationCenterForUseragain(Functions.UserName, "Y", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath).Result;
                        }
                    }

                    //result_fav = CasesSyncAPIMethods.GetFavorite(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath).Result;
                    result_fav = CasesSyncAPIMethods.GetFavorite(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, Convert.ToString((int)Applications.Cases), App.DBPath).Result;

                });
                #endregion

                #region Favorite List

                try
                {
                    Group_CaseType fav_case = new Group_CaseType("My Favourites");
                    if (result_fav.Count > 0)
                    {
                        list_favorite = new ObservableCollection<GetFavorite>(result_fav as List<GetFavorite>);
                        //Case_type_List.IsVisible = true;

                        foreach (var item in list_favorite)
                        {
                            fav_case.Add(new Sel_CaseTypeList()
                            {
                                TypeId = item.FavoriteId,
                                TypeName = item.FavoriteName,
                                TypeFlag = true
                            });
                        }
                    }
                    Master_list.Add(fav_case);
                }
                catch (Exception)
                {
                }

                #endregion

                #region Select Case Type

                if (result?.Count > 0)
                {
                    list = new ObservableCollection<OriginationCenterData>(result as List<OriginationCenterData>);

                    Group_CaseType sel_casetype = new Group_CaseType("Case Types");

                    foreach (var item in list)
                    {
                        sel_casetype.Add(new Sel_CaseTypeList()
                        {
                            TypeId = (int)item.CaseTypeID,
                            TypeName = item.CaseTypeName,
                            TypeFlag = false
                        });
                    }
                    Master_list.Add(sel_casetype);
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }

                #endregion

            }
            catch (Exception ex)
            {
                // 
            }
            //Case_type_List.ItemsSource = Master_list;
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            UpdateListContent();
        }

        #region  Select Case Type Opertion
        private async void Detail_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem mt = (MenuItem)sender;
                var sd = (OriginationCenterData)mt.BindingContext;

                //  await this.Navigation.PushAsync(new CaseDetailsView((string)sd.CaseTypeName, (string)sd.CaseTypeID.ToString(), (string)sd.OpenCaseCount.ToString(), (string)sd.ClosedCaseCount.ToString(), (string)sd.TotalCaseCount.ToString(), (string)sd.TotalPastDueCaseCount.ToString(), (string)sd.AssignedToMeCount.ToString(), (string)sd.PastDueAssignedToMeCount.ToString(), (string)sd.CreatedByMeCount.ToString(), (string)sd.ClosedByMeCount.ToString(), (string)sd.DefaultHopperName, (string)sd.CaseTypeOwnerName));
                await this.Navigation.PushAsync(new CaseDetailsView(sd));

                //CaseTypeList.SelectedItem = null;
            }
            catch (Exception ex)
            {
                // 
            }
        }
        private async void NewCase_Clicked(object sender, EventArgs e)
        {
            try
            {
                var Miii = ((Button)sender);
                var sddd = (OriginationCenterDataResponse.OriginationCenterData)Miii.CommandParameter;
                await this.Navigation.PushAsync(new NewCase("0", Convert.ToString(sddd.CaseTypeID)));
                //CaseTypeList.SelectedItem = null;
            }
            catch (Exception ex)
            {
                // 
            }
        }
        private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            try
            {
                ListView l = (ListView)sender;
                var sd = (OriginationCenterDataResponse.OriginationCenterData)l.SelectedItem;
                await Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(sd.CaseTypeID), "", sd.CaseTypeName));
                //CaseTypeList.SelectedItem = null;
            }
            catch (Exception ex)
            {
                // 
            }
        }
        #endregion

        #region  Favorite Opertion
        private async void MyFavDetail_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem ls = (MenuItem)sender;
                //var bind = ls.BindingContext as GetFavorite;
                var bind = ls.BindingContext as Sel_CaseTypeList;
                if (bind.TypeFlag == true)
                {
                    if (list_favorite.Count >= 0)
                    {
                        var fav = list_favorite.Where(t => t.FavoriteId.ToString() == bind.TypeId.ToString());
                        var StrFavName = "";
                        var FavCaseTypeID = "";

                        foreach (var item in fav)
                        {
                            if (item.FavoriteId == bind.TypeId)
                            {
                                json = item.FieldValues;
                                JObject jobject = JObject.Parse(json);
                                StrFavName = item.FavoriteName;
                                FavCaseTypeID = jobject.GetValue("caseType").ToString().ToLower();
                                ;
                            }
                        }
                        var CreateCaseData = JsonConvert.DeserializeObject<CreateCaseOptimizedRequest.CreateCaseModelOptimized>(json);

                        var result1 = CasesSyncAPIMethods.GetTypesByCaseTypeIDRaw(App.Isonline, FavCaseTypeID, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserName);
                        List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();

                        List<spi_MobileApp_GetTypesByCaseTypeResult> metadata = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
                        metadata = result1.Result;
                        string DisplayAlertStr = "";
                        foreach (var iitem in metadata)
                        {
                            KeyValuePair<string, string> name = new KeyValuePair<string, string>(iitem.ASSOC_TYPE_ID.ToString(), iitem.NAME);
                            kvpList.Add(name);

                            if (iitem.ASSOC_FIELD_TYPE.ToUpper() == "E" || iitem.ASSOC_FIELD_TYPE.ToUpper() == "O" || iitem.ASSOC_FIELD_TYPE.ToUpper() == "D")
                            {

                                var record = CreateCaseData.metaDataValues.Where(v => v.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();

                                DisplayAlertStr += Convert.ToString(iitem.NAME) + " : " + Convert.ToString(record.Value.Value);
                                DisplayAlertStr = DisplayAlertStr + "\n";
                            }
                            else
                            {
                                var record = CreateCaseData.textValues.Where(v => v.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();

                                DisplayAlertStr += Convert.ToString(iitem.NAME) + " : " + Convert.ToString(record?.Value);
                                DisplayAlertStr = DisplayAlertStr + "\n";
                            }

                        }
                        DisplayAlert(StrFavName, DisplayAlertStr, "Close");

                    }
                }
                else
                {
                    var sd = ls.BindingContext as Sel_CaseTypeList;
                    var casetype_item = (list.Where(v => v.CaseTypeID == sd.TypeId).FirstOrDefault()) as OriginationCenterData;
                    await this.Navigation.PushAsync(new CaseDetailsView(casetype_item));
                }
                Case_type_List.SelectedItem = null;
            }
            catch (Exception ex)
            {

                // 
            }
        }
        private async void NewCase_FavClicked(object sender, ItemTappedEventArgs e)
        {
            try
            {

                var Miii = ((Button)sender);
                var sddd = (GetFavorite)Miii.CommandParameter;

                if (list_favorite.Count >= 0)
                {
                    var fav = list_favorite.Where(t => t.FavoriteId.ToString() == sddd.FavoriteId.ToString());
                    foreach (var item in fav)
                    {
                        if (item.FavoriteId == sddd.FavoriteId)
                        {
                            json = item.FieldValues;
                            JObject jobject = JObject.Parse(json);
                            sfav_casetypeid = jobject.GetValue("caseType").ToString().ToLower();
                        }
                    }
                }
                await this.Navigation.PushAsync(new NewCase(sddd.FavoriteId.ToString(), sfav_casetypeid));
                Case_type_List.SelectedItem = null;
            }
            catch (Exception ex)
            {

                // 
            }
        }
        private void MyFavDelete_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem ls = (MenuItem)sender;
                var bind = ls.BindingContext as GetFavorite;

                //var result = CasesAPIMethods.RemoveFavorite(bind.FavoriteId.ToString());
                var result = CasesSyncAPIMethods.RemoveFavorite(App.Isonline, bind.FavoriteId.ToString(), App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID.ToString(), Functions.UserName);

                //var temp = result.GetValue("ResponseContent");

                if (!string.IsNullOrEmpty(result.Result.ToString()) && result.Result.ToString() != "[]")
                {
                    var sd = list_favorite.Remove((list_favorite.Where(t => t.FavoriteId == bind.FavoriteId).ToList())[0]);
                }

                Case_type_List.SelectedItem = null;
            }
            catch (Exception ex)
            {

                // 
            }
        }
        #endregion

        #region  Search Function 

        ObservableCollection<Group_CaseType> Search_List = new ObservableCollection<Group_CaseType>();

        private void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search_List.Clear();
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    Case_type_List.ItemsSource = Master_list;
                }
                else
                {
                    int i = 0;

                    foreach (var item in Master_list)
                    {
                        string name = string.Empty;
                        if (i == 0)
                        {
                            name = "My Favourites";
                        }
                        else
                        {
                            name = "Case Types";
                        }
                        // Root name
                        var forMe = new Group_CaseType(Title = name);
                        var t = item.Where(x => x.TypeName.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                        //forMe.Add(item);
                        foreach (var TT in t)
                        {
                            forMe.Add(TT);
                        }
                        Search_List.Add(forMe);
                        i++;
                    }
                    Case_type_List.ItemsSource = Search_List;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void HomeMenu_Click(object sender, EventArgs e)
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
        #endregion

        private void HeaderTapped(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = _expandedGroups.IndexOf(
               ((Group_CaseType)((Button)sender).CommandParameter));
                Master_list[selectedIndex].Expanded = !Master_list[selectedIndex].Expanded;
                UpdateListContent();

            }
            catch (Exception ex)
            {

            }
        }
        private void UpdateListContent()
        {
            try
            {
                _expandedGroups = new ObservableCollection<Group_CaseType>();
                foreach (Group_CaseType group in Master_list)
                {
                    //Create new FoodGroups so we do not alter original list
                    Group_CaseType newGroup = new Group_CaseType(group.Title, group.Expanded);
                    //Add the count of food items for Lits Header Titles to use

                    if (group.Expanded)
                    {
                        foreach (Sel_CaseTypeList food in group)
                        {
                            newGroup.Add(food);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                Case_type_List.ItemsSource = _expandedGroups;
               // Case_type_List.HeightRequest = 250;
            }
            catch (Exception)
            {
            }
        }

        private async void Case_lst_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var Miii = ((ListView)sender);
                var sddd = (Sel_CaseTypeList)Miii.SelectedItem;
                if (sddd.TypeFlag == true)
                {
                    if (list_favorite.Count >= 0)
                    {
                        var fav = list_favorite.Where(t => t.FavoriteId.ToString() == sddd.TypeId.ToString()).FirstOrDefault();
                        await this.Navigation.PushAsync(new NewCase(fav.FavoriteId.ToString(), fav.TypeID.ToString()));
                        Case_type_List.SelectedItem = null;
                    }
                }
                else
                {
                    //Case Types
                    Case_List _SelectedNodes = new Case_List();
                    _SelectedNodes.CaseTypeId = (e.Item as Sel_CaseTypeList).TypeId;
                    _SelectedNodes.CaseTypeName = (e.Item as Sel_CaseTypeList).TypeName;
                    if (LandingPage.IsCreateEntity)
                    {
                        await this.Navigation.PushAsync(new NewCase("0", Convert.ToString(_SelectedNodes.CaseTypeId)));
                    }
                    else
                    {
                        await Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(_SelectedNodes.CaseTypeId), "", _SelectedNodes.CaseTypeName));
                    }
                    Miii.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {

                // 
            }
        }

    }
}