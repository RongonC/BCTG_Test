using StemmonsMobile.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels
{
    class MyPropertypageViewModel
    {
        public ICommand MyPropertiesbtnCmd { get; private set; }
        public ICommand PropertyListbtnCmd { get; private set; }
        public ICommand CompusesbuttonCmd { get; private set; }

        public MyPropertypageViewModel()
        {
            MyPropertiesbtnCmd = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new MyPropertyList("MYPROPLIST"));
            });
            PropertyListbtnCmd = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new MyPropertyList("PROPLIST"));
            });
            CompusesbuttonCmd = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new MyPropertyList("CAMPS"));
            });
        }

        public void MoveToNext()
        {
            //Application.Current.MainPage.Navigation.PushAsync(new Property_Page_Custom());
        }

    }
}
