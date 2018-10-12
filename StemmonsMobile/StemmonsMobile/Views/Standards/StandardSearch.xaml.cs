using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;

namespace StemmonsMobile.Views.Standards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandardSearch : ContentPage
    {
        public StandardSearch()
        {
            InitializeComponent();
            Title = "Standard Search";
            SearchText.Focus();
        }

        private async void btn_search_Clicked(object sender, EventArgs e)
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                //var mi = ((Button)sender);
                string search = SearchText.Text;
                List<DataTypes.DataType.Standards.DataTypes.MetaData> list = new List<DataTypes.DataType.Standards.DataTypes.MetaData>();
                await Task.Run(() =>
                {
                    var result = StandardsSyncAPIMethods.SearchMetadata(App.Isonline, search, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    result.Wait();
                    list = result.Result;
                });
                if (list.Count > 0)
                {
                    lstSearchitem.ItemsSource = null;
                    lstSearchitem.ItemsSource = list;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void lstSearchitem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ListView l = (ListView)sender;
                var sd = (StemmonsMobile.DataTypes.DataType.Standards.DataTypes.MetaData)l.SelectedItem;
                Grp_StandardDetails gp = new Grp_StandardDetails();
                gp.APP_ID = Convert.ToString(sd.AppID);
                gp.NAME = sd.Name;
                gp.APP_ASSOC_META_DATA_ID = Convert.ToString(sd.AppAssocMetaDataID);
                gp.DESCRIPTIONS = sd.MetaDataDesc;
                gp.PARENT_META_DATA_ID = Convert.ToString(sd.ParentMetaDataID);
                gp.PARENT_LEVEL = Convert.ToString(sd.ParentLevel);



                await Navigation.PushAsync(new BookContentsPage(gp));
                //await Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(sd.CaseTypeID), "", sd.CaseTypeName));
                lstSearchitem.SelectedItem = null;
            }
            catch (Exception ex)
            {
                // 
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
    }
}