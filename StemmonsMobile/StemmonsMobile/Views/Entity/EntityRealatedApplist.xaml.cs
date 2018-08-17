using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.Cases;
using StemmonsMobile.Views.CreateQuestForm;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityRealatedApplist : ContentPage
    {
        EntityAppRelatedTypes EntityAppRelatedTypes;
        EntityListMBView EntityListMBView;
        // string NavScreenname = string.Empty;
        string Appname = "";
        public EntityRealatedApplist(EntityAppRelatedTypes _entityAppRelatedTypes, EntityListMBView _entityListMBView, string _appname)
        {
            InitializeComponent();
            App.SetConnectionFlag();
            EntityAppRelatedTypes = _entityAppRelatedTypes;
            EntityListMBView = _entityListMBView;
            Title = "Entity Relationship";
            Appname = _appname;
            // NavScreenname = _navscreenname;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                //bool Onlineflag = false;
                //if (!string.IsNullOrEmpty(NavScreenname))
                //    Onlineflag = false;
                //else
                //    Onlineflag = App.Isonline;

                #region Get Entity Related Application Data
                GetEntitiesRelationDataRequest req = new GetEntitiesRelationDataRequest
                {
                    User = Functions.UserName,
                };
                FILTER_VALUE fv = new FILTER_VALUE
                {
                    SHOW_ENTITIES_ACTIVE_INACTIVE = "ALL",
                    SHOW_ENTITIES_ASSIGNED_TO_USER = "N",
                    SHOW_ENTITIES_CREATED_BY_USER = "N",
                    SHOW_ENTITIES_OWNED_BY_USER = "N",
                    SHOW_ENTITIES_INACTIVE_BY_USER = "N",
                    EXTERNAL_DATASOURCE_OBJECT_ID_ENTITY = EntityListMBView.EntityDetails.EntityID,
                    ENTITY_TYPE = new List<int>
                    {
                        Convert.ToInt32(EntityAppRelatedTypes.Id)
                    }
                };

                req.EntityTypeSchema = fv;
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                List<EntityRelationData> lst = new List<EntityRelationData>();

                if (Appname.ToLower() == "entity")
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            var _temp = EntitySyncAPIMethods.GetEntitiesRelationData(App.Isonline, Convert.ToString(EntityListMBView.EntityDetails.EntityID), EntityAppRelatedTypes.Id, Functions.UserName, App.DBPath, req);
                            _temp.Wait();
                            lst = _temp.Result;
                        });
                        if (lst?.Count > 0)
                        {
                            list_EntityrelatedApp.ItemsSource = lst;
                        }
                        list_EntityrelatedApp.IsVisible = true;
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (Appname.ToLower() == "cases")
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            var _temp = EntitySyncAPIMethods.GetCasesRelationData(App.Isonline, Convert.ToString(EntityListMBView.EntityDetails.EntityID), EntityAppRelatedTypes.Id, Functions.UserName, "", "", "", "", "", "", "", App.DBPath);
                            lst = _temp.Result;
                        });
                        if (lst?.Count > 0)
                        {
                            list_EntityrelatedApp.ItemsSource = lst;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (Appname.ToLower() == "quest")
                {
                    try
                    {
                        await Task.Run(() =>
                        {
                            var _temp = EntitySyncAPIMethods.GetQuestRelationData(App.Isonline, Convert.ToString(EntityListMBView.EntityDetails.EntityID), EntityAppRelatedTypes.AreaId, EntityAppRelatedTypes.Id, Functions.UserName, App.DBPath);
                            lst = _temp.Result;
                        });
                        if (lst?.Count > 0)
                        {
                            list_EntityrelatedApp.ItemsSource = lst;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                #endregion
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void list_EntityrelatedApp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as EntityRelationData;

            if (Appname.ToLower() == "entity")
            {
                EntityListMBView mbView = new EntityListMBView();
                mbView.EntityDetails = new EntityClass();
                mbView.EntityDetails.EntityID = Convert.ToInt32(item.EntityID);
                mbView.EntityDetails.EntityTypeID = Convert.ToInt32(item.ListID);
                await Navigation.PushAsync(new Entity_View(mbView));
            }
            else if (Appname.ToLower() == "cases")
            {
                await Navigation.PushAsync(new ViewCasePage(Convert.ToString(item.EntityID), Convert.ToString(item.ListID), ""));
            }
            else if (Appname.ToLower() == "quest")
            {
                await Navigation.PushAsync(new QuestItemDetailPage(EntityAppRelatedTypes.Id, Convert.ToString(item.ListID), "", "", "", true, EntityAppRelatedTypes.AreaId));
            }
        }

        private void txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {

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

    }

    public class History
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string ListId { get; set; }

        public History(string _Name, string _addrs, string _field2, string _field3, string _field4, string _listid)
        {
            Title = _Name;
            Address = _addrs;
            Field2 = _field2;
            Field3 = _field3;
            Field4 = _field4;
            ListId = _listid;
        }
    }

}