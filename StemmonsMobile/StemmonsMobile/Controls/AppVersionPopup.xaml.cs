using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppVersionPopup : PopupPage
    {
        public AppVersionPopup()
        {
            InitializeComponent();
        }

        private void PopUpDismiss_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();

            UpdateAppURLs();
        }

        public static void UpdateAppURLs()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=com.boxerproperty.s_bctg1"));
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Device.OpenUri(new Uri("https://itunes.apple.com/in/app/stemmons-central-to-go/id1359237791?mt=8"));
            }
        }
    }
}