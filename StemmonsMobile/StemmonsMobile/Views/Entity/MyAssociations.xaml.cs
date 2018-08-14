using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;

namespace StemmonsMobile.Views.Entity
{
    public partial class MyAssociations : ContentPage
    {
        List<BoxerCentralHomePage_EntityList_Mob> MyAssociation = new List<BoxerCentralHomePage_EntityList_Mob>();

        string _username = string.Empty;
        public MyAssociations(string _title, string username)
        {
            InitializeComponent();
            Title = _title;
            _username = username;
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar sh = (SearchBar)sender;
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    FormData.ItemsSource = MyAssociation;
                    sh.Unfocus();
                }
                else
                {
                    FormData.ItemsSource = MyAssociation.Where(x => x.ENTITY_TITLE.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
            catch (Exception ex)
            {

                // throw;
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(async () =>
                {
                    MyAssociation = await EntitySyncAPIMethods.GetEntityAssociationList(App.Isonline, _username, App.DBPath);
                });
                if (MyAssociation.Count > 0)
                    FormData.ItemsSource = MyAssociation;
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void FormData_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var Item = FormData.SelectedItem as BoxerCentralHomePage_EntityList_Mob;
                EntityListMBView Entity = new EntityListMBView();
                Entity.EntityDetails = new EntityClass();
                Entity.EntityDetails.EntityTypeID = Convert.ToInt32(Item.ENTITY_TYPE_ID);
                Entity.EntityDetails.EntityID = Convert.ToInt32(Item.ENTITY_ID);
                Entity.EntityDetails.EntityTypeName = Item.ENTITY_TYPE_NAME;
                Entity.Title = Item.ENTITY_TITLE;
                await Navigation.PushAsync(new Entity_View(Entity, "MyAssociations"));
            }
            catch (Exception)
            {

            }

            FormData.SelectedItem = null;
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
