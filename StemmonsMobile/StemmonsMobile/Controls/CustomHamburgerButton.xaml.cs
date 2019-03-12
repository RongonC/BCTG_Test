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
	public partial class CustomHamburgerButton : ContentView
    {
        public event EventHandler Custom_Humburger_Clicked_Event;
        public CustomHamburgerButton ()
		{
			InitializeComponent ();
            btn_more.Clicked += Custom_Hamburger_Button_Clicked;
        }

        private void Custom_Hamburger_Button_Clicked(object sender, EventArgs e)
        {
            if (Custom_Humburger_Clicked_Event != null)
            {
                btn_more.Clicked += Custom_Humburger_Clicked_Event;
            }
            else
            {
                App.BtnHumburger(Application.Current.MainPage);
            }
        }
    }
}