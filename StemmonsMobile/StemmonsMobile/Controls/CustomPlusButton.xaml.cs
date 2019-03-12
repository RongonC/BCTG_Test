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
	public partial class CustomPlusButton : ContentView
	{
        public event EventHandler CustomPlusButtonHandler;
        public CustomPlusButton ()
		{
			InitializeComponent ();
            btn_add.Clicked += PlusBtn_click;
        }
        public void PlusBtn_click(object b, EventArgs e)
        {
            if (CustomPlusButtonHandler != null)
            {
                btn_add.Clicked += CustomPlusButtonHandler;
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Fucntionality not given..!", "Ok");
            }
        }
    }
}