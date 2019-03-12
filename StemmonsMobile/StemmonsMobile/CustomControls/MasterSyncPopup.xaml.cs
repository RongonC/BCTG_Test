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
    public partial class MasterSyncPopup : Grid
    {

        public static event EventHandler HeaderButtonClick;
        public static event PropertyChangingEventHandler PropertyChange;
        public MasterSyncPopup()
        {
            InitializeComponent();
            btn_close.Clicked += Btn_close_Clicked;

            Sync_ProgressBar.PropertyChanging += Sync_ProgressBar_PropertyChanging;
        }

        private void Sync_ProgressBar_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (PropertyChange != null)
            {
                if (Sync_ProgressBar.Progress == 1)
                {
                    PropertyChange(sender, e);
                }
            }
        }

        private void Btn_close_Clicked(object sender, EventArgs e)
        {
            if (HeaderButtonClick != null)
            {
                HeaderButtonClick(sender, e);
            }
        }
    }
}