using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.View_Case_Origination_Center;
using Xamarin.Forms;

namespace StemmonsMobile.Views.PeopleScreen
{
    public partial class EntityRoleAssociation : ContentPage
    {
        string username;
        List<AllEntityRoleRelationshipByEmp> EntityAssociation = new List<AllEntityRoleRelationshipByEmp>();
        List<AllEntityRoleRelationshipByEmp> lst = new List<AllEntityRoleRelationshipByEmp>();
        public EntityRoleAssociation(string EmpName)
        {
            InitializeComponent();
            username = EmpName;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                lst = new List<AllEntityRoleRelationshipByEmp>();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(() =>
                {
                    Task<List<AllEntityRoleRelationshipByEmp>> result = EntitySyncAPIMethods.GetAllEntityRoleRelationshipByEmp(App.Isonline, username, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    result.Wait();
                    lst = result.Result;
                });

                if (lst.Count > 0)
                    entitydata.ItemsSource = lst;
                else
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }


        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                SearchBar sh = (SearchBar)sender;
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    entitydata.ItemsSource = lst;
                    sh.Unfocus();
                }
                else
                {
                    //  FormList.ItemsSource = AreaIdlst.Where(x => x.strItemName.StartsWith(e.NewTextValue));
                    entitydata.ItemsSource = lst.FindAll(x => x.ENTITYTITLE.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
            catch (Exception ex)
            {

                // throw;
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