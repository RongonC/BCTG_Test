using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
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
    public partial class Entity_CategoryType : ContentPage
    {
        List<Entity_categoryList> EntityCategoryList = new List<Entity_categoryList>();

        public Entity_CategoryType()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            txt_seacrhbar.Text = "";
            try
            {
             
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(async () =>
                 {
                     EntityCategoryList = await EntitySyncAPIMethods.GetEntitycatogoryList(App.Isonline, ConstantsSync.EntityInstance, 0, null, App.DBPath, Functions.UserName);
                 });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
      
                if (EntityCategoryList.Count > 1)
                    list_entitycategory.ItemsSource = EntityCategoryList;
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (System.Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        private async void List_entitycategory_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var list = e.Item as Entity_categoryList;
                await Navigation.PushAsync(new Select_EntityList(list.EntityTypeCategoryID == 0 ? null : list.EntityTypeCategoryID));
            }
            catch (System.Exception ex)
            {
            }
        }

        private async void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                list_entitycategory.ItemsSource = EntityCategoryList;
            }
            else
            {
                list_entitycategory.ItemsSource = EntityCategoryList.Where(x => x.EntityTypeCategoryName.ToLower().Contains(e.NewTextValue.ToLower()));
                if (EntityCategoryList.Where(x => x.EntityTypeCategoryName.ToLower().Contains(e.NewTextValue.ToLower())).ToList().Count <= 0)
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    ((SearchBar)sender).Text = "";
                }
            }
        }

        private void btn_home_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private void btn_more_Clicked(object sender, System.EventArgs e)
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