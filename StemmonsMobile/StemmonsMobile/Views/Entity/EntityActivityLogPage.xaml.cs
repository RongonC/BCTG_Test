using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.DataTypes.DataType.Entity;
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
    public partial class EntityActivityLogPage : ContentPage
    {
        ObservableCollection<ActivityInstance> ls = new ObservableCollection<ActivityInstance>();
        EntityListMBView SelectedEntityList;
        public EntityActivityLogPage(EntityListMBView _select)
        {
            InitializeComponent();
            SelectedEntityList = _select;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (App.Isonline)
            {
                try
                {
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                    await Task.Run(() =>
                    {
                        var ActivityApiCall = EntityAPIMethods.GetActivity(SelectedEntityList.EntityDetails.EntityID.ToString(), Functions.UserName);
                        var apicallresponse = ActivityApiCall.GetValue("ResponseContent");
                        if (!string.IsNullOrEmpty(apicallresponse?.ToString()) && apicallresponse.ToString() != "[]")
                            ls = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<ActivityInstance>>(apicallresponse.ToString());
                    });
                    listViewActivityLog.ItemsSource = ls;

                }
                catch (Exception)
                {
                }
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            }
            else
            {
                DisplayAlert("", "Please Go online to View full Activity Log!", "Ok");
            }
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void btn_home_Clickd(object sender, EventArgs e)
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
