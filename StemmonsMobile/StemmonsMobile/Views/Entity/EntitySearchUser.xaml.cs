using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Models;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntitySearchUser : ContentPage
    {
        EntityClass EntityDetails;
        string ButtonName;
        public EntitySearchUser(EntityClass _select, string _ButtonName)
        {
            InitializeComponent();
            EntityDetails = _select;
            Title = "Search User";
            ButtonName = _ButtonName;
            App.SetConnectionFlag();
            searchTxt.Focus();
        }

        private async void Lst_Assignuser_Clicked(object sender, EventArgs e)
        {
            var btn = ((Button)sender).BindingContext as GetUserInfoResponse.UserInfo;
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

            try
            {
                bool IsSucces = false;

                if (btn.ButtonName.ToLower().Contains("assign"))
                {
                    await Task.Run(() =>
                    {
                        IsSucces = EntitySyncAPIMethods.AssignEntityItem(App.Isonline, Functions.UserName, btn.SAMName, EntityDetails.EntityID, EntityDetails.EntityTypeID, App.DBPath);
                    });

                    if (IsSucces)
                    {
                        EntityDetails.EntityAssignedToFullName = btn.DisplayName;
                        EntityDetails.EntitiyAssignedToUserName = btn.SAMName;
                        EntityDetails.EntityAssignedToDateTime = DateTime.Now.ToString();
                        EntityDetails.EntityAssignedToEmail = "";
                        EntityDetails.EntityAssignedToPhone = "";
                        EntityDetails.EntityModifiedByFullName = Functions.UserFullName;
                        EntityDetails.EntityModifiedDateTime = DateTime.Now.ToString();
                        EntityDetails.EntityModifiedByUserName = Functions.UserName;
                        EntityOrgCenterList t = new EntityOrgCenterList();
                        t.EntityTypeID = EntityDetails.EntityTypeID;
                        IsSucces = true;
                    }
                }

                else if (btn.ButtonName.ToLower().Contains("forward"))
                {
                    await Task.Run(() => { IsSucces = EntitySyncAPIMethods.ForwardEntityItem(App.Isonline, Functions.UserName, btn.SAMName, EntityDetails.EntityID, EntityDetails.EntityTypeID, App.DBPath); });

                    if (IsSucces)
                    {
                        EntityDetails.EntityAssignedToFullName = btn.DisplayName;
                        EntityDetails.EntityAssignedToDateTime = DateTime.Now.ToString();
                        EntityDetails.EntityOwnedByFullName = btn.DisplayName;
                        EntityDetails.EntityOwnedByDateTime = DateTime.Now.ToString();
                        EntityDetails.EntitiyOwnedByUserName = btn.SAMName;
                        EntityDetails.EntityModifiedByFullName = Functions.UserFullName;
                        EntityDetails.EntityModifiedDateTime = DateTime.Now.ToString();
                        EntityDetails.EntityModifiedByUserName = Functions.UserName;

                        EntityOrgCenterList t = new EntityOrgCenterList();
                        t.EntityTypeID = EntityDetails.EntityTypeID;
                        IsSucces = true;
                    }
                }
                if (IsSucces)
                {
                    var TempList = DBHelper.GetAppTypeInfoList(ConstantsSync.EntityInstance, EntityDetails.EntityID, EntityDetails.EntityTypeID, ConstantsSync.EntityItemView, App.DBPath, null);
                    TempList.Wait();

                    string TrasnType = string.Empty;

                    if (!string.IsNullOrEmpty(EntityDetails.TransactionType))
                    {
                        TrasnType = EntityDetails.TransactionType;
                    }
                    else
                        TrasnType = App.Isonline ? "M" : "T";

                    CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(EntityDetails), ConstantsSync.EntityInstance, ConstantsSync.EntityItemView, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, TempList.Result.APP_TYPE_INFO_ID, EntityDetails.EntityTypeID.ToString(), TrasnType, EntityDetails.EntityID.ToString(), 0, EntityDetails.EntityTypeName, "").Wait();
                    await Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void Search_Clicked(object sender, EventArgs e)
        {
            try
            {
                Task<List<GetUserInfoResponse.UserInfo>> val = CasesSyncAPIMethods.GetEmployeesBySearch(searchTxt.Text ?? "", App.DBPath, Functions.UserName);
                val.Wait();
                var List = val.Result.Select(i =>
                {
                    i.ButtonName = ButtonName;
                    return i;
                }).ToList();
                if (List.Count > 0)
                    listViewFoundUsers.ItemsSource = List;
                else
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");

                searchTxt.Text = "";
            }
            catch (Exception)
            {
            }
        }

        private void btn_Home_Clicked(object sender, EventArgs e)
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
    }
}