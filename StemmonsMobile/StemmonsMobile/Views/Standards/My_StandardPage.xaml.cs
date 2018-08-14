using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Standards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class My_StandardPage : ContentPage
    {
        public My_StandardPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                Task<List<Grp_StandardDetails>> Response = null;
                await Task.Run(() =>
                {
                    Response = StandardsSyncAPIMethods.GetAllAppForUserSync(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    Response.Wait();
                });
                if (Response.Result.Count > 0)
                {
                    BookData.ItemsSource = Response.Result;
                }
                else
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    await Navigation.PopAsync();
                }
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void BookData_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView lst = (ListView)sender;
            this.Navigation.PushAsync(new BookContentsPage(e.Item as Grp_StandardDetails));
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

        private   void btn_more_Clicked(object sender, EventArgs e)
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
    public class Mystandarddata
    {
        public int AppID { get; set; }
        public string AppName { get; set; }
        public string Description { get; set; }
        public string ModifyDate { get; set; }
        public string sModifyDate { get; set; }
        public string LastVisitDate { get; set; }
        public string sLastVisitDate { get; set; }
    }
}