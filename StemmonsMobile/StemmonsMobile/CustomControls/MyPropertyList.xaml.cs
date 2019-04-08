using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.DataTypes.DataType.Entity;
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

        public MyPropertyList(string _pageFor, EntityClass Eclass = null)
        {

            InitializeComponent();
            if (_pageFor == "MKT")
            {
                Title = "Select Market";
                EntityListVM.ScreenCode = "MKT";
                EntityListVM.SystemCodeEntityType = "MKT";
            }
            else if (_pageFor == "MYPROPLIST")
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
                Title = Eclass.EntityTitle + " Property List";
                EntityListVM.ScreenCode = "PROPLIST";
                EntityListVM.EntityID = Eclass.EntityID;
                EntityListVM.SystemCodeEntityType = "PROPY";
            }

            EntityListVM._Viewtype = "";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            List_entity_subtypes.SelectedItem = null;
            IsBusy = true;
            if (EntityListVM.ListEntityitem.Count == 0)
            {
                EntityListVM.PageIndex = 1;
                EntityListVM.ListEntityitem.Clear();
               
                await EntityListVM.GetEntityListwithCall();
                EntityListVM.ListEntityitem.OrderByDescending(x => x.EntityTitle);
            }
            IsBusy = false;
        }

    }
}