using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemInstanceTranResponse;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestItemPage : ContentPage
    {
        int ItemIdData, ViewCount;
        string FromPage = string.Empty;
        string Username = string.Empty;
        string sType = string.Empty;

        List<GetItemInstanceTranResponse.ItemInstanceTran> QuestItemlst = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
        string Itmid = string.Empty;
        string scatId = string.Empty;
        public QuestItemPage(string frompage, string username, string type, string itemid)
        {
            InitializeComponent();
            FromPage = frompage;
            Username = username;
            sType = type;
            Itmid = itemid;
            ViewCount = 0;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            dynamic result = null;
            try
            {
                await Task.Run(action: () =>
                {
                    if (FromPage.ToLower() == "detailpage")
                    {
                        result = QuestSyncAPIMethods.GetQuestItemByStatus(App.Isonline, Itmid, Username, sType, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    }
                    else
                    {
                        result = QuestSyncAPIMethods.GetItemInstanceTran(App.Isonline, "", Convert.ToString(ItemIdData), "", "", "", "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, scatId, Functions.UserName);
                    }
                });

                listdata.ItemsSource = result.Result;
                QuestItemlst = result.Result;
                if (QuestItemlst?.Count == 0)
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay,false, masterGrid);
        }

        void listitemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                var tmp = e.Item as ItemInstanceTran;
                this.Navigation.PushAsync(new QuestItemDetailPage(Convert.ToString(tmp.intItemID), Convert.ToString(tmp.intItemInstanceTranID), tmp.SECURITY_TYPE_AREA, tmp.SECURITY_TYPE_ITEM, tmp.SECURITY_TYPE_TRAN, Convert.ToBoolean(tmp.blnIsEdit), scatId));
            }
            catch (Exception ex)
            {
            }
        }

        public QuestItemPage(string questArea, int intItemId, string catId = "")
        {
            InitializeComponent();
            this.Title = questArea;
            ItemIdData = intItemId;
            scatId = catId;
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
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

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar sh = (SearchBar)sender;
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    listdata.ItemsSource = QuestItemlst;
                    sh.Unfocus();
                }
                else
                {
                    listdata.ItemsSource = QuestItemlst.Where(x => x.strCol1ItemInfoFieldValue.ToLower().Contains(e.NewTextValue.ToLower()));
                    //listdata.ItemsSource = QuestItemlst.Where(x => x.strItemName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
