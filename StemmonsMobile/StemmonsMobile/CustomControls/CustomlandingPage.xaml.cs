using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Rg.Plugins.Popup.Services;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.DataTypes.DataType.Default;
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
    public partial class CustomlandingPage : ContentPage
    {
        public CustomlandingPage()
        {
            InitializeComponent();

            BackgroundColor = Color.Transparent;

            App.IsCustomLandingPage = true;
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

            try
            {
                HeaderLayout.Children.Clear();

                var grid = new Grid();
                grid.RowSpacing = 0;
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(17) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(10) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });

                SettingButton updateBtn = new SettingButton(UpdateBtn_Clicked);
                var sBtn = updateBtn.FindByName("btnSetting") as Button;
                sBtn.Image = "Assets/available_update.png";
                updateBtn.HorizontalOptions = LayoutOptions.Center;

                Label updateText = new Label();
                updateText.Text = "New Update";
                updateText.TextColor = Color.Red;
                updateText.HorizontalOptions = LayoutOptions.Center;
                updateText.FontSize = 8;

                grid.Children.Add(updateBtn, 0, 1);
                grid.Children.Add(updateText, 0, 2);
                grid.IsVisible = false;

                HeaderLayout.Children.Add(grid);

                SettingButton csb = new SettingButton(null);
                csb.HorizontalOptions = LayoutOptions.EndAndExpand;
                csb.Margin = new Thickness(0, 10, 5, 0);
                HeaderLayout.Children.Add(csb);

                if (App.CurretVer != CrossDeviceInfo.Current.AppVersion)
                {
                    grid.IsVisible = true;
                    PopupNavigation.Instance.PushAsync(new AppVersionPopup());
                }
            }
            catch
            {
            }
        }

        protected override void OnAppearing()
        {
            App.Isonline = CrossConnectivity.Current.IsConnected;

            if (App.Isonline)
            {
                if (App.IsLoginCall)
                {
                    if (Functions.AppStartCount <= 1)
                    {
                     
                        MainGrid.BackgroundColor = new Color(211, 211, 211);
                        MainGrid.Opacity = 0.4;
                        MainGrid.IsEnabled = false;
                        cnt_syncPopup.IsVisible = true;
                        Cont_sync.IsVisible = true;
                        Cont_sync.BindingContext = new MasterSyncpopupModel(Functions.lstSyncAPIStatus);
                        Grd_sync.IsVisible = false;
                        MasterSyncpopupModel.IsClosed = true;
                        MasterSyncpopupModel.Is_Popup_Open = true;
                        // MainGrid.Opacity = 1;
                    }
                    else
                    {
                        MainGrid.BackgroundColor = new Color(255, 255, 255);
                        MainGrid.Opacity = 1;
                        MainGrid.IsEnabled = true;
                        cnt_syncPopup.IsVisible = false;
                        Grd_sync.IsVisible = true;
                        Grd_sync.BindingContext = new MasterSyncpopupModel(Functions.lstSyncAPIStatus);
                        Cont_sync.IsVisible = false;
                    }

                    App.IsLoginCall = false;
                }
            }
            else //No internet available
            {
                MainGrid.BackgroundColor = new Color(255, 255, 255);
                MainGrid.Opacity = 1;
                MainGrid.IsEnabled = true;
                cnt_syncPopup.IsVisible = false;
                Grd_sync.IsVisible = false;
                Grd_sync.BindingContext = new MasterSyncpopupModel(Functions.lstSyncAPIStatus);
                Cont_sync.IsVisible = false;
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

        private void UpdateBtn_Clicked(object sender, EventArgs e)
        {
            //PopupNavigation.Instance.PushAsync(new AppVersionPopup());
            AppVersionPopup.UpdateAppURLs();
        }
    }
}