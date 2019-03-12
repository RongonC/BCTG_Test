using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.Models;
using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyLandingPage : ContentPage
    {
        public PropertyLandingPage()
        {
            InitializeComponent();
            App.IsPropertyPage = true;
            SettingButton settings = new SettingButton(null);
            settings.HorizontalOptions = LayoutOptions.EndAndExpand;
            settings.Margin = new Thickness(0, 10, 5, 0);
            grdLogo.Children.Add(settings);
            Grid.SetColumn(settings, 2);
            Grid.SetRow(settings, 0);

            NavigationPage.SetHasNavigationBar(this, false);

            MasterSyncPopup.HeaderButtonClick += new EventHandler(OnBackGround_Click);
            MasterSyncPopup.PropertyChange += new PropertyChangingEventHandler(OnPropertyChage);

            this.BindingContext = new PropLandingViewModel();
        }

        protected override void OnAppearing()
        {
            if (App.IsLoginCall)
            {
                if (Functions.AppStartCount <= 1)
                {
                    MasterSyncpopupModel.IsClosed = true;
                    MasterSyncpopupModel.Is_Popup_Open = false;
                    MainGrid.BackgroundColor = new Color(211, 211, 211);
                    MainGrid.Opacity = 0.4;
                    MainGrid.IsEnabled = false;
                    cnt_syncPopup.IsVisible = true;
                    Cont_sync.IsVisible = true;
                    Cont_sync.BindingContext = new MasterSyncpopupModel();
                    Grd_sync.IsVisible = false;
                }
                else
                {
                    MainGrid.BackgroundColor = new Color(255, 255, 255);
                    MainGrid.Opacity = 1;
                    MainGrid.IsEnabled = true;
                    cnt_syncPopup.IsVisible = false;
                    Grd_sync.IsVisible = true;
                    Grd_sync.BindingContext = new MasterSyncpopupModel();
                    Cont_sync.IsVisible = false;

                }

                App.IsLoginCall = false;
            }
        }

        public void OnBackGround_Click(object sender, EventArgs e)
        {
            MasterSyncpopupModel.IsClosed = true;
            Grd_sync.IsVisible = false;
        }
        public void OnPropertyChage(object sender, PropertyChangingEventArgs e)
        {
            IsMainGridVisibility();
        }
        public void IsMainGridVisibility()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (MasterSyncpopupModel.Is_Popup_Open)
                {
                    MasterSyncpopupModel.IsClosed = true;
                    MasterSyncpopupModel.Is_Popup_Open = false;
                    cnt_syncPopup.IsVisible = false;
                    MainGrid.IsEnabled = true;
                    MainGrid.BackgroundColor = new Color(255, 255, 255);
                    MainGrid.Opacity = 1;
                }
                else
                {
                    MainGrid.IsEnabled = true;
                    MainGrid.BackgroundColor = new Color(255, 255, 255);
                    MainGrid.Opacity = 1;
                    Grd_sync.IsVisible = false;
                }
            });
        }

        private void Setting_Clicked(object sender, EventArgs e)
        {

        }
    }
}