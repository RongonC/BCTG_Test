using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.Cases;
using StemmonsMobile.Views.Entity;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.Standards;
using StemmonsMobile.Views.View_Case_Origination_Center;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        string searchtype;
        public SearchPage(string searchCriteria)
        {
            InitializeComponent();

            searchtype = searchCriteria;
            if (searchtype == "Search Cases")
            {
                searchlabel.Text = "Search Cases";
            }
            else if (searchtype == "Search Entities")
            {
                searchlabel.Text = "Search Entities";
            }
            else if (searchtype == "Search Quest")
            {
                searchlabel.Text = "Search Quest";
            }

            SearchText.Focus();
            (SearchText as Entry).Completed += btn_search_Clicked;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SearchText.Focus();
        }

        async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
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

        private async void lstSearchitem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var listitem = ((ListView)sender).SelectedItem as StemmonsMobile.DataTypes.DataType.Cases.SearchResult;

                if (searchtype == "Search Cases")
                {
                    await Navigation.PushAsync(new ViewCasePage(Convert.ToString(listitem.LINK_ID), Convert.ToString(listitem.TypeID), "View Case"));
                }
                else if (searchtype == "Search Entities")
                {
                    EntityClass md = new EntityClass();
                    //md.Title = listitem.TYPE_NAME;
                    md= new EntityClass();
                    md.EntityTypeID = Convert.ToInt32(listitem.TypeID);
                    md.EntityID = Convert.ToInt32(listitem.LINK_ID);

                    await Navigation.PushAsync(new Entity_View(md));
                }
                else if (searchtype == "Search Quest")
                {
                    ////Need ItemInstanceTranID

                    //(string itemId, string ItemInstanceTranID, string security_area, string security_item, string security_tran, bool isEditable, string CatId = "")
                    //await Navigation.PushAsync(new QuestItemDetailPage(listitem.ITEM_ID, 0, listitem.SECURITY_AREA, listitem.SECURITY_ITEM, listitem.SECURITY_TRAN, listitem.IS_EDIT, listitem.AREA_ID));
                }
                else if (searchtype == "Search Employee")
                {

                }
            }
            catch (Exception)
            {
            }
        }


        private async void btn_search_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText.Text))
                {
                    this.DisplayAlert("Alert", "Input is not valid", "OK");
                }
                else
                {
                    int Appid = 0;
                    if (searchtype == "Search Cases")
                    {
                        Appid = 3;
                    }
                    else if (searchtype == "Search Entities")
                    {
                        Appid = 1;
                    }
                    else if (searchtype == "Search Quest")
                    {
                        Appid = 2;
                    }

                    List<DataTypes.DataType.Cases.SearchResult> list = new List<DataTypes.DataType.Cases.SearchResult>();

                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                    await Task.Run(() =>
                    {
                        list = CasesSyncAPIMethods.GetSearchResult(App.Isonline, Functions.UserName, Appid, null, null, null, SearchText.Text,0, 10, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    });

                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                    if (list.Count > 0)
                    {
                        lstSearchitem.ItemsSource = list;
                    }
                    else
                    {
                        DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}