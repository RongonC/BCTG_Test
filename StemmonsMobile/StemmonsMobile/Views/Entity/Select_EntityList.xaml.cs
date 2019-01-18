using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class Select_EntityList : ContentPage
    {
        public Select_EntityList(int? _catId)
        {
            InitializeComponent();
            CategoryId = _catId;
        }

        List<EntityOrgCenterList> lstResult = new List<EntityOrgCenterList>();
        int? CategoryId;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            txt_seacrhbar.Text = "";
            App.SetConnectionFlag();
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(async () =>
                {
                    lstResult = await EntitySyncAPIMethods.GetEntityTypeList(App.Isonline, CategoryId, Functions.UserName, null, App.DBPath);
                });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                if (LandingPage.IsCreateEntity)
                {
                    lstResult = lstResult.Where(v => v.SecurityType.ToLower() != "r").ToList();
                }

                list_entity.ItemsSource = lstResult.OrderBy(v => v.EntityTypeName);
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        private async void List_entity_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (LandingPage.IsCreateEntity)
                {
                    GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest Lazyload_request = new GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest
                    {
                        user = Functions.UserName,
                    };

                    Lazyload_request.isActive = 'Y';

                    FILTER_VALUE fv = new FILTER_VALUE
                    {
                        SHOW_ENTITIES_ACTIVE_INACTIVE = "ALL",
                        ENTITY_TYPE = new List<int>
                        {
                            Convert.ToInt32((e.Item as EntityOrgCenterList).EntityTypeID)
                        }
                    };
                    Lazyload_request.entityTypeSchema = fv;
                    EntityListMBView mb = new EntityListMBView();
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    await Task.Run(async () =>
                    {
                        var EntityLists = await EntitySyncAPIMethods.GetEntitiesBySystemCodeKeyValuePair_LazyLoadCommon(App.Isonline, Functions.UserName, Lazyload_request, App.DBPath, Convert.ToInt32((e.Item as EntityOrgCenterList).EntityTypeID), Functions.UserFullName, "");

                        if (EntityLists.Count > 0)
                            mb.EntityDetails = EntityLists[0];
                        else
                            mb.EntityDetails = new EntityClass();
                    });

                    Functions.IsEditEntity = false;
                    var eORG = (e.Item as EntityOrgCenterList);

                    await Navigation.PushAsync(new CreateEntityPage(eORG.EntityTypeID, eORG.EntityID, eORG.NewEntityText, mb));
                }
                else
                {
                    await Navigation.PushAsync(new EntityDetailsSubtype(e.Item as EntityOrgCenterList, null));
                }
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            list_entity.SelectedItem = null;
        }

        private async void Menu_Details_Clicked(object sender, EventArgs e)
        {
            MenuItem mt = (MenuItem)sender;
            await Navigation.PushAsync(new Entity_TypeDetails(mt.BindingContext as EntityOrgCenterList));
            list_entity.SelectedItem = null;
        }

        private void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
                list_entity.ItemsSource = lstResult;
            else
            {
                list_entity.ItemsSource = lstResult.Where(x => x.EntityTypeName.ToLower().Contains(e.NewTextValue.ToLower()));
                if (lstResult.Where(x => x.EntityTypeName.ToLower().Contains(e.NewTextValue.ToLower())).ToList().Count <= 0)
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    ((SearchBar)sender).Text = "";
                }
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
