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
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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

        public SelectCaseType()
        {
            InitializeComponent();

            App.SetConnectionFlag();
            //For search by keybord
            //   (txt_seacrhbar as SearchBar).TextChanged += Txt_seacrhbar_TextChanged;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            txt_seacrhbar.Text = "";
            try
            {
                MyFavoriteList.IsVisible = false;

                List<OriginationCenterDataResponse.OriginationCenterData> result = new List<OriginationCenterDataResponse.OriginationCenterData>();
                List<GetFavoriteResponse.GetFavorite> result_fav = new List<GetFavoriteResponse.GetFavorite>();

                //Functions.ShowIndicator(ActInd, Stack_indicator, true, Main_Stack, 0.5);
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

                    result_fav = CasesSyncAPIMethods.GetFavorite(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath).Result;
                });
                #endregion

                //Functions.ShowIndicator(ActInd, Stack_indicator, false, Main_Stack, 1);

                #region Favorite List

                try
                {
                    if (result_fav.Count > 0)
                    {
                        list_favorite = new ObservableCollection<GetFavorite>(result_fav as List<GetFavorite>);
                        MyFavoriteList.IsVisible = true;
                        MyFavoriteList.ItemsSource = list_favorite;
                    }
                }
                catch (Exception)
                {
                }

                int heightRowList = 120;
                int iq = (list_favorite.Count * heightRowList);
                MyFavoriteList.HeightRequest = iq;

                #endregion

                #region Select Case Type

                if (result?.Count > 0)
                {
                    list = new ObservableCollection<OriginationCenterData>(result as List<OriginationCenterData>);
                    CaseTypeList.ItemsSource = list;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }

                #endregion
                int heightRowList11 = 90;
                int iq1 = (list.Count * heightRowList11);
                CaseTypeList.HeightRequest = iq1;


            }
            catch (Exception ex)
            {
                // 
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            //Functions.ShowIndicator(ActInd, Stack_indicator, false, Main_Stack, 1);
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

                CaseTypeList.SelectedItem = null;
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
                CaseTypeList.SelectedItem = null;
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
                CaseTypeList.SelectedItem = null;
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
                var bind = ls.BindingContext as GetFavorite;
                if (list_favorite.Count >= 0)
                {
                    var fav = list_favorite.Where(t => t.FavoriteId.ToString() == bind.FavoriteId.ToString());
                    var StrFavName = "";
                    var FavCaseTypeID = "";

                    foreach (var item in fav)
                    {
                        if (item.FavoriteId == bind.FavoriteId)
                        {
                            json = item.FieldValues;
                            JObject jobject = JObject.Parse(json);
                            StrFavName = item.FavoriteName;
                            FavCaseTypeID = jobject.GetValue("caseType").ToString().ToLower(); ;
                        }
                    }
                    var CreateCaseData = JsonConvert.DeserializeObject<CreateCaseOptimizedRequest.CreateCaseModelOptimized>(json);

                    var result1 = CasesSyncAPIMethods.GetTypesByCaseTypeIDRaw(App.Isonline, FavCaseTypeID, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserName);
                    List<KeyValuePair<string, string>> kvpList = new List<KeyValuePair<string, string>>();

                    List<spi_MobileApp_GetTypesByCaseTypeResult> metadata = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
                    spi_MobileApp_GetTypesByCaseTypeResult itemTypes = new spi_MobileApp_GetTypesByCaseTypeResult();
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
                MyFavoriteList.SelectedItem = null;
            }
            catch (Exception ex)
            {

                // 
            }
        }
        private async void NewCase_FavClicked(object sender, EventArgs e)
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
                MyFavoriteList.SelectedItem = null;
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

                MyFavoriteList.SelectedItem = null;
            }
            catch (Exception ex)
            {

                // 
            }
        }
        #endregion

        #region  Search Function 
        private void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    CaseTypeList.ItemsSource = list;
                    MyFavoriteList.ItemsSource = list_favorite;
                }
                else
                {
                    CaseTypeList.ItemsSource = list.Where(x => x.CaseTypeName.ToLower().Contains(e.NewTextValue.ToLower()));
                    MyFavoriteList.ItemsSource = list_favorite.Where(x => x.FavoriteName.ToLower().Contains(e.NewTextValue.ToLower()));
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

    }
}