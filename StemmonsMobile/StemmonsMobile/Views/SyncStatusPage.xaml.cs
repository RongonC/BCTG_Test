using Rg.Plugins.Popup.Services;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncStatusPage : ContentPage
    {
        public SyncStatusPage()
        {
            InitializeComponent();
            UpdateIcons();
        }

        private void UpdateIcons()
        {
            if (App.SyncSuccessFlagArr[0] == 1)
            {
                Sync1.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync1.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[1] == 1)
            {
                Sync2.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync2.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[2] == 1)
            {
                Sync3.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync3.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[3] == 1)
            {
                Sync4.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync4.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[4] == 1)
            {
                Sync5.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync5.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[5] == 1)
            {
                Sync6.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync6.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[6] == 1)
            {
                Sync7.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync7.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[7] == 1)
            {
                Sync8.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync8.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[8] == 1)
            {
                Sync9.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync9.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[9] == 1)
            {
                Sync10.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync10.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[10] == 1)
            {
                Sync11.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync11.Source = "Assets/cross_2.png";
            }

            if (App.SyncSuccessFlagArr[11] == 1)
            {
                Sync12.Source = "Assets/tick_2.png";
            }
            else
            {
                Sync12.Source = "Assets/cross_2.png";
            }

        }
    }


}