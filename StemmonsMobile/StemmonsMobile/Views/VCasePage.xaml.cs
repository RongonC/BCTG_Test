using StemmonsMobile.Controls;
using StemmonsMobile.ViewModels;
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
    public partial class VCasePage : ContentPage
    {

        private CustomlistViewModel _EntityListviewmodel = new CustomlistViewModel();
        public CustomlistViewModel EntityListVM
        {
            get
            {
                if (_EntityListviewmodel == null)
                    _EntityListviewmodel = new CustomlistViewModel();
                return _EntityListviewmodel;
            }
            set
            {
                if (_EntityListviewmodel == value)
                    return;
                _EntityListviewmodel = value;
            }
        }

        public VCasePage()
        {
            InitializeComponent();

            CustomList.ItemTemplate = new DataTemplate(typeof(CustomCell));
           // CustomList.ItemTapped += EntityListVM.ItemTapped;
            BindingContext = EntityListVM.MainList;
        }
    }
}