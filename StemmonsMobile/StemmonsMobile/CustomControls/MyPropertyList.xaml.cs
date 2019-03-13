using StemmonsMobile.ViewModels.EntityViewModel;
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
    public partial class MyPropertyList : ContentPage
    {
        private EntityListViewModel _EntityListviewmodel = new EntityListViewModel();

        public EntityListViewModel EntityListVM
        {
            get
            {
                if (_EntityListviewmodel == null)
                    _EntityListviewmodel = new EntityListViewModel();
                return _EntityListviewmodel;
            }

            set
            {
                if (_EntityListviewmodel == value)
                    return;
                _EntityListviewmodel = value;
            }
        }
        public MyPropertyList(string _pageFor)
        {

            InitializeComponent();
            if (_pageFor == "MYPROPLIST")
            {
                Title = "My Properties";
                EntityListVM.ScreenCode = "MYPROPLIST";
            }
            else if (_pageFor == "CAMPS")
            {
                Title = "Campus List";
                EntityListVM.ScreenCode = "CAMPS";
                EntityListVM.SystemCodeEntityType = "CAMPS";
            }
            else
            {
                Title = "Property List";
                EntityListVM.ScreenCode = "PROPLIST";
                EntityListVM.SystemCodeEntityType = "PROPY";
            }

            EntityListVM._Viewtype = "";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            EntityListVM.PageIndex = 1;
            EntityListVM.ListEntityitem.Clear();
            await EntityListVM.GetEntityListwithCall();
            EntityListVM.ListEntityitem.OrderBy(x => Title);
            IsBusy = false;
        }
    }
}