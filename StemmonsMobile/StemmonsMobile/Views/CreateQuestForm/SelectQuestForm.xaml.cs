using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StemmonsMobile.Commonfiles;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.DataTypes.DataType.Quest;
using static StemmonsMobile.DataTypes.DataType.Quest.ItemsByAreaIDResponse;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Views.LoginProcess;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectQuestForm : ContentPage
    {
        int AreaId;

        List<ItemsByAreaID> AreaIdlst = new List<ItemsByAreaID>();
        int ViewCount;
        public SelectQuestForm(int areaid)
        {
            InitializeComponent();
            AreaId = areaid;
            ViewCount = 0;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            if (ViewCount == 0)
            {

                ViewCount = ViewCount + 1;
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                try
                {
                    await Task.Run(action: () =>
                    {
                        var result = QuestSyncAPIMethods.GetItemsByAreaIDFormList(App.Isonline, Functions.UserName, Convert.ToString(AreaId), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                        AreaIdlst = result.Result;
                    });

                    FormList.ItemsSource = AreaIdlst;
                }
                catch (Exception ex)
                {
                }
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
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

        //private void QuestForms_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    try
        //    {
        //        var listView = (ListView)sender;
        //        var cell = (ViewCell)listView.TemplatedItems.First(t => t.BindingContext == e.Item);
        //        var tapGestureRecognizer = new TapGestureRecognizer();
        //        var value = e.Item as ItemsByAreaID;


        //        tapGestureRecognizer.Tapped += (obj, args) => this.Navigation.PushAsync(new QuestItemPage((String)value.strItemName, value.intItemID));
        //        cell.View.GestureRecognizers.Add(tapGestureRecognizer);
        //        var longPressGestureRecognizer = new LongPressGestureRecognizer();
        //        longPressGestureRecognizer.OnAction +=
        //            (gestureRecognizer, state) =>
        //            {
        //                if (state == GestureRecognizerState.Began)
        //                {
        //                    var hopperCenterDetailsDialog = new HopperCenterDetailDialog((string)e.Item);
        //                    hopperCenterDetailsDialog.Content.HorizontalOptions = LayoutOptions.Center;
        //                    hopperCenterDetailsDialog.Content.WidthRequest = this.Width * 9 / 10;
        //                    hopperCenterDetailsDialog.ListHeight = this.Height / 2;
        //                    this.Navigation.PushPopupAsync(hopperCenterDetailsDialog);
        //                }
        //            };
        //        cell.View.GestureRecognizers.Add(longPressGestureRecognizer);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private void QuestForms_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    try
        //    {
        //        var listView = (ListView)sender;
        //        var cell = (ViewCell)listView.TemplatedItems.First(t => t.BindingContext == e.Item);
        //        cell.View.GestureRecognizers.Clear();
        //        foreach (var menuItem in cell.ContextActions)
        //        {
        //            menuItem.Clicked -= MenuItem_Clicked;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar sh = (SearchBar)sender;
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    FormList.ItemsSource = AreaIdlst;
                    sh.Unfocus();
                }
                else
                {
                    //  FormList.ItemsSource = AreaIdlst.Where(x => x.strItemName.StartsWith(e.NewTextValue));
                    FormList.ItemsSource = AreaIdlst.Where(x => x.strItemName.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
            catch (Exception ex)
            {

            }
        }
 
        void CreateQuest_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var mi = ((Button)sender);
                var value = mi.CommandParameter as ItemsByAreaID;
                if (value.securityType.ToUpper().Contains("C") || value.securityType.ToUpper().Contains("OPEN"))
                {
                    this.Navigation.PushAsync(new NewQuestForm(value.intItemID, Convert.ToString(AreaId)));
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

        private void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                var mi = ((MenuItem)sender);
                var value = mi.CommandParameter as ItemsByAreaID;
                Functions.tempitemid = value.intItemID;
                if (value.securityType.ToUpper().Contains("R") || value.securityType.ToUpper().Contains("OPEN"))
                {
                    this.Navigation.PushAsync(new HopperCenterDetailPage((string)value.strItemName));
                }
                else
                {
                    DisplayAlert("Quest Form", "You dont have sufficient right to view this page.", "Ok");
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void FormList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //try
            //{
            //    ListView l = (ListView)sender;
            //    var sd = (ItemsByAreaID)l.SelectedItem;
            //    if (sd.securityType.ToUpper().Contains("R") || sd.securityType.ToUpper().Contains("OPEN"))
            //    {
            //        this.Navigation.PushAsync(new QuestItemPage((String)sd.strItemName, sd.intItemID, Convert.ToString(AreaId)));
            //    }
            //    else
            //    {
            //        DisplayAlert("Quest Form", "You dont have sufficient rights to view this page.", "Ok");
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            //ArpanB
            try
            {
                ListView l = (ListView)sender;
                var sd = (ItemsByAreaID)l.SelectedItem;

                if (LandingPage.IsCreateEntity)
                {
                    if (sd.securityType.ToUpper().Contains("C") || sd.securityType.ToUpper().Contains("OPEN"))
                    {
                        this.Navigation.PushAsync(new NewQuestForm(sd.intItemID, Convert.ToString(AreaId)));
                    }
                    else
                    {
                        DisplayAlert("Quest Form", "You dont have suffiecient rights to view this page.", "Ok");
                    }
                }
                else
                {
                    if (sd.securityType.ToUpper().Contains("R") || sd.securityType.ToUpper().Contains("OPEN"))
                    {
                        //this.Navigation.PushAsync(new QuestItemPage((String)sd.strItemName, sd.intItemID, Convert.ToString(AreaId)));
                        this.Navigation.PushAsync(new QuestItemPage((String)sd.strItemName, sd.intItemID, Convert.ToString(AreaId), (string)sd.securityType));
                    }
                    else
                    {
                        DisplayAlert("Quest Form", "You dont have sufficient rights to view this page.", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //ArpanB
        }
    }

}