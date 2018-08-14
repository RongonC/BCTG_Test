using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Models;
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
    public partial class Entity_AppList : ContentPage
    {
        private ChillerEntity _chillerEntity;
        EntityListMBView _entityListMBView;
        List<EntityAppRelatedTypes> BindList = new List<EntityAppRelatedTypes>();
        //string NavScreenname = string.Empty;

        public Entity_AppList(ChillerEntity chillerEntity, EntityListMBView _entityList)
        {
            InitializeComponent();

            _chillerEntity = chillerEntity;
            Title = _chillerEntity?.Name ?? "Relational Grid";
            _entityListMBView = _entityList;
            App.SetConnectionFlag();
            //NavScreenname = _navscreenname;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            #region Get Entity Related Application
            List<string> templist = new List<string>();
            txt_seacrhbar.Text = "";
            try
            {

                //bool Onlineflag = false;
                //if (!string.IsNullOrEmpty(NavScreenname))
                //    Onlineflag = false;
                //else
                //    Onlineflag = App.Isonline;

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(() =>
                 {
                     Task<List<EntityAppRelatedTypes>> list = EntitySyncAPIMethods.GetEntityRelatedTypes(App.Isonline, _chillerEntity?.Name, _entityListMBView.EntityDetails.EntityID.ToString(), _entityListMBView.EntityDetails.EntityTypeID.ToString(), Functions.UserName, App.DBPath);
                     list.Wait();
                     BindList = list.Result;
                 });
                if (BindList.Count > 0)
                {
                    List_entityapp.ItemsSource = BindList;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception)
            {
            }
            #endregion
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void List_entityapp_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new EntityRealatedApplist(((ListView)sender).SelectedItem as EntityAppRelatedTypes, _entityListMBView, _chillerEntity?.Name.ToLower()));
        }

        private async void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    List_entityapp.ItemsSource = BindList;
                }
                else
                {
                    List_entityapp.ItemsSource = BindList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
                    int cnt = BindList.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList().Count;
                    if (cnt <= 0)
                    {
                        await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                        ((SearchBar)sender).Text = "";
                    }
                }
            }
            catch (Exception)
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
    }
}
