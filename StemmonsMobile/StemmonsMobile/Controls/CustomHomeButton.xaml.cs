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
    public partial class CustomHomeButton : ContentView
    {
        public CustomHomeButton()
        {
            InitializeComponent();
            btn_Home.Clicked += Button_clicked;
        }

        public void Button_clicked(object b, EventArgs e)
        {
            HomeButton();
        }

        public void HomeButton()
        {
            App.GotoHome();
            Application.Current.MainPage.Navigation.PushAsync(new LandingPage());
        }
    }
}