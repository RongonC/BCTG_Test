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
    public partial class MyPublication : ContentPage
    {
        List<Grp_StandardDetails> lstResult = new List<Grp_StandardDetails>();
        public MyPublication()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            try
            {
                App.SetConnectionFlag();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                await Task.Run(async () =>
                {
                    lstResult = await StandardsSyncAPIMethods.GetPublishedAppByUserBasedOnSAM(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                });

                if (lstResult.Count > 0)
                {
                    List_standard.ItemsSource = lstResult;
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

        private async void List_standard_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            var item = ((ListView)sender).SelectedItem as Grp_StandardDetails;
            await Navigation.PushAsync(new BookContentsPage(item));
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
