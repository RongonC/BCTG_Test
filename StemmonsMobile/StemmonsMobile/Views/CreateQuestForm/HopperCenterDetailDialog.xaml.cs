using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.CreateQuestForm
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HopperCenterDetailDialog : PopupPage
    {
        public HopperCenterDetailDialog()
        {
            InitializeComponent();
            App.SetConnectionFlag();
        }

        public HopperCenterDetailDialog(string footer) : this()
        {
            this.listView.Footer = $"{footer} Form";
            this.buttonFormOriginationDetails.Text = $"{footer} {this.buttonFormOriginationDetails.Text}";
        }

        public double ListHeight
        {
            get { return this.listView.Height; }
            set { this.listView.HeightRequest = value; }
        }

        private async void MainLayout_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.RemovePageAsync(this);
        }
    }
}
