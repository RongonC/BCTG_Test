using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Quest.AreaResponse;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectQuestArea : ContentPage
    {
        List<AreaResponse.Area> Arealst = new List<AreaResponse.Area>();
        int ViewCount;
        public SelectQuestArea()
        {
            InitializeComponent();

            ViewCount = 0;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

            if (ViewCount == 0)
            {
                ViewCount = ViewCount + 1;

                Arealst = new List<AreaResponse.Area>();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                try
                {
					await Task.Run(action: () =>
					{

						Task<List<AreaResponse.Area>> Arealstt = QuestSyncAPIMethods.GetAreaList(App.Isonline, Functions.UserName, null, null, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
						Arealstt.Wait();
						
						Arealst = Arealstt.Result;
                    });
					AreaList.ItemsSource = Arealst;
                }
                catch (Exception ex)
                {

                }
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            }
        }

        void FocusedEvent(object sender, Xamarin.Forms.FocusEventArgs e)
        {
            SearchBar sh = (SearchBar)sender;

            if (sh.IsFocused)
            {
                sh.Text = " ";
            }
            else
            {
                sh.Unfocus();
            }
        }

        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {

                SearchBar sh = (SearchBar)sender;

                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    AreaList.ItemsSource = Arealst;
                    sh.Unfocus();
                }
                else
                {
                    AreaList.ItemsSource = Arealst.Where(x => x.strAreaName.ToLower().Contains(e.NewTextValue.ToLower().TrimStart()));
                }
            }
            catch (Exception ex)
            {
            }
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        void FormList_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {

                ListView l = (ListView)sender;
                var sd = (Area)l.SelectedItem;
                if (sd.securityType.ToUpper().Contains("R") || sd.securityType.ToUpper().Contains("C") || sd.securityType.ToUpper().Contains("OPEN"))
                {
                    this.Navigation.PushAsync(new SelectQuestForm(sd.intAreaID));
                }
            }
            catch (Exception ex)
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
