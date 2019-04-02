using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using StemmonsMobile.Models;
using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;
using static StemmonsMobile.DataTypes.DataType.Quest.GetFavoriteResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemInstanceTranResponse;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestItemPage : ContentPage
    {
        public int ItemIdData, ViewCount;
        public static string FromPage = string.Empty;
        public static string Username = string.Empty;
        public static string sType = string.Empty;

        List<GetItemInstanceTranResponse.ItemInstanceTran> QuestItemlst = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
        public static string Itmid = string.Empty;
        string scatId = string.Empty;
        string Sec_Type = string.Empty;


        //ObservableCollection<GetItemInstanceTranResponse.ItemInstanceTran> list = new ObservableCollection<GetItemInstanceTranResponse.ItemInstanceTran>();

        ObservableCollection<Group_QuestForm> Master_list = new ObservableCollection<Group_QuestForm>();
        ObservableCollection<GetFavorite> list_favorite = new ObservableCollection<GetFavorite>();
        private ObservableCollection<Group_QuestForm> _expandedGroups;



        public QuestItemPage(string frompage, string username, string type, string itemid)
        {
            InitializeComponent();
            FromPage = frompage;
            Username = username;
            sType = type;
            Itmid = itemid;
            ViewCount = 0;
        }
        //ArpanB
        public QuestItemPage(string questArea, int intItemId, string catId = "", string secType = "") : this(FromPage, Username, sType, Itmid)
        {
            this.Title = questArea;
            ItemIdData = intItemId;
            scatId = catId;
            Sec_Type = secType;
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            List<GetFavoriteResponse.GetFavorite> result_fav = new List<GetFavoriteResponse.GetFavorite>();
            string id = "";
            if (FromPage.ToLower() == "detailpage")
            {
                id = Itmid;
            }
            else
            {
                id = Convert.ToString(ItemIdData);
            }

            try
            {
                dynamic result = null;

                await Task.Run(() =>
                {
                    if (FromPage.ToLower() == "detailpage")
                    {
                        result = QuestSyncAPIMethods.GetQuestItemByStatus(App.Isonline, Itmid, Username, sType, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    }
                    else
                    {
                        result = QuestSyncAPIMethods.GetItemInstanceTran(App.Isonline, "", Convert.ToString(ItemIdData), "", "", "", "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, scatId, Functions.UserName);
                        result.Wait();
                    }

                    result_fav = QuestSyncAPIMethods.GetFavorite(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, Convert.ToString((int)Applications.Quest), Convert.ToString(scatId), Convert.ToString(id), App.DBPath).Result;
                });

                QuestItemlst = result.Result;


                Master_list = new ObservableCollection<Group_QuestForm>();

                #region Favorite List
                try
                {
                    Group_QuestForm fav_Quest = new Group_QuestForm("Favorite Quest items");
                    if (result_fav.Count > 0)
                    {
                        list_favorite = new ObservableCollection<GetFavorite>(result_fav as List<GetFavorite>);

                        foreach (var item in list_favorite)
                        {
                            fav_Quest.Add(new Sel_QuestFormList()
                            {
                                TypeId = item.FavoriteId,
                                strItemNAme = item.FavoriteName,
                                TypeFlag = true
                            });
                        }
                    }
                    Master_list.Add(fav_Quest);
                }
                catch (Exception)
                {
                }

                #endregion

                #region Select Item List

                if (QuestItemlst?.Count > 0)
                {
                    Group_QuestForm sel_questform = new Group_QuestForm("Quest Items Types");

                    foreach (var item in QuestItemlst)
                    {
                        sel_questform.Add(new Sel_QuestFormList()
                        {
                            intItemInstanceTranID = item.intItemInstanceTranID,
                            TypeId = (int)item.intItemID,
                            strItemNAme = item.strItemNAme,
                            dcOverallScore = item.dcOverallScore,
                            strCol1ItemInfoFieldValue = item.strCol1ItemInfoFieldValue,
                            strCol2ItemInfoFieldValue = item.strCol2ItemInfoFieldValue,
                            strCol3ItemInfoFieldValue = item.strCol3ItemInfoFieldValue,
                            SECURITY_TYPE_AREA = item.SECURITY_TYPE_AREA,
                            SECURITY_TYPE_ITEM = item.SECURITY_TYPE_ITEM,
                            SECURITY_TYPE_TRAN = item.SECURITY_TYPE_TRAN,
                            blnIsEdit = item.blnIsEdit,
                            TypeFlag = false
                        });
                    }
                    Master_list.Add(sel_questform);

                    UpdateListContent();
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }

                #endregion

            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }


        void listitemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                var tmp = e.Item as Sel_QuestFormList;
                this.Navigation.PushAsync(new QuestItemDetailPage(Convert.ToString(tmp.TypeId), Convert.ToString(tmp.intItemInstanceTranID), tmp.SECURITY_TYPE_AREA, tmp.SECURITY_TYPE_ITEM, tmp.SECURITY_TYPE_TRAN, Convert.ToBoolean(tmp.blnIsEdit), scatId));
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

        //ArpanB
        private void btn_add_Clicked(object sender, EventArgs e)
        {
            try
            {
                //var mi = ((Button)sender);
                //var value = mi.CommandParameter as ItemInstanceTran;
                if (Sec_Type.ToUpper().Contains("C") || Sec_Type.ToUpper().Contains("OPEN"))
                {
                    this.Navigation.PushAsync(new NewQuestForm(ItemIdData, scatId));
                }
                else
                {
                    DisplayAlert("Quest Form", "You dont have suffiecient rights to view this page.", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }
        //ArpanB

        ObservableCollection<Group_QuestForm> Search_List = new ObservableCollection<Group_QuestForm>();
        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar sh = (SearchBar)sender;
                string searchKey = e.NewTextValue.Trim().ToLower();
                //Search_List.Clear();
                //try
                //{
                //    if (string.IsNullOrEmpty(searchKey))
                //    {
                //        listdata.ItemsSource = Master_list;
                //    }
                //    else
                //    {
                //        int i = 0;

                //        foreach (var item in Master_list)
                //        {
                //            string name = string.Empty;
                //            if (i == 0)
                //            {
                //                name = "Favorite Quest items";
                //            }
                //            else
                //            {
                //                name = "Quest Items Types";
                //            }
                //            // Root name
                //            var forMe = new Group_QuestForm(Title = name);
                //            var t = item.Where(x => x.strCol1ItemInfoFieldValue.ToLower().Contains(searchKey)).ToList();
                //            //forMe.Add(item);
                //            foreach (var TT in t)
                //            {
                //                forMe.Add(TT);
                //            }
                //            Search_List.Add(forMe);
                //            i++;
                //        }
                //        listdata.ItemsSource = Search_List;
                //    }
                //}
                //catch (Exception ex)
                //{
                //}









                //if (string.IsNullOrEmpty(e.NewTextValue.Trim()))
                //{
                //    listdata.ItemsSource = QuestItemlst;
                //    sh.Unfocus();
                //}
                //else
                //{
                //    listdata.ItemsSource = QuestItemlst.Where(x => x.strCol1ItemInfoFieldValue.ToLower().Contains(e.NewTextValue.ToLower()));
                //    //listdata.ItemsSource = QuestItemlst.Where(x => x.strItemName.ToLower().Contains(e.NewTextValue.ToLower()));
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void HeaderTapped(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = _expandedGroups.IndexOf(
               ((Group_QuestForm)((Button)sender).CommandParameter));
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
                listdata.ItemsSource = null;
                _expandedGroups = new ObservableCollection<Group_QuestForm>();
                foreach (Group_QuestForm group in Master_list)
                {
                    //Create new FoodGroups so we do not alter original list
                    Group_QuestForm newGroup = new Group_QuestForm(group.Title, group.Expanded);
                    //Add the count of food items for Lits Header Titles to use

                    if (group.Expanded)
                    {
                        foreach (Sel_QuestFormList food in group)
                        {
                            newGroup.Add(food);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                listdata.ItemsSource = _expandedGroups;
                // Case_type_List.HeightRequest = 250;
            }
            catch (Exception)
            {
            }
        }
    }
}
