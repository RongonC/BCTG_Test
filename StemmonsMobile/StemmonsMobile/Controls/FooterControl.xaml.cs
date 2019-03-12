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
    public partial class FooterControl : ContentView
    {
        //public CustomHomeButton home_Button = new CustomHomeButton();
        //public CustomPlusButton plus_Button = new CustomPlusButton ();
        //public CustomHamburgerButton hamburger_Button = new CustomHamburgerButton();
        //public Frame blankFrame = new Frame();

        public FooterControl()
        {
            InitializeComponent();

            //home_Button.HorizontalOptions = LayoutOptions.StartAndExpand;
            //FooterBtnLayout.Children.Add(home_Button);

            //blankFrame.HorizontalOptions = LayoutOptions.CenterAndExpand;
            //blankFrame.BackgroundColor = Color.Transparent;
            //FooterBtnLayout.Children.Add(blankFrame);

            //hamburger_Button.HorizontalOptions = LayoutOptions.EndAndExpand;
            //FooterBtnLayout.Children.Add(hamburger_Button);
        }
    }
}