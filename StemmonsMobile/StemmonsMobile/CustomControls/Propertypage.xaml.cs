using StemmonsMobile.Controls;
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
    public partial class Propertypage : ContentPage
    {
        public Propertypage()
        {
            InitializeComponent();
           
            this.BindingContext = new MyPropertypageViewModel();
        }
    }
}