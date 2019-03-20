using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using StemmonsMobile.ViewModels.EntityViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TenantInfoPage : ContentPage
    {
        string fileStr = string.Empty;
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



        public TenantInfoPage(EntityClass entityDetails, string _pagecode)
        {
            InitializeComponent();

            //Titlelbl.Text = entityDetails.EntityTitle;
            EntityListVM.ScreenCode = _pagecode;
            EntityListVM.EntityID = entityDetails.EntityID;
            if (_pagecode == "TNTLIST")
            {
                Title = "Tenant List";
                EntityListVM.SystemCodeEntityType = "CSTMR";
                EntityListVM._Viewtype = "";
            }
            else
            {
                Title = "Available Units";
             
                //Must pass Entity Id
                //EntityListVM.ScreenCode = "TNTLIST";
                EntityListVM.SystemCodeEntityType = "UNITS";
                EntityListVM._Viewtype = "";
            }



            try
            {
                // ProfileImg.Source = Functions.GetImageFromEntityAssoc(entityDetails.AssociationFieldCollection);
            }
            catch (Exception)
            {
                // ProfileImg.Source = "Assets/PropertyImage.png";
            }
        }

        int? _pageindex = 1;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            //Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            IsBusy = true;
            try
            {
                for (int i = 1; i <= _pageindex; i++)
                {
                    EntityListVM.PageIndex = i;
                    await EntityListVM.GetEntityListwithCall();
                }
            }
            catch (Exception ex)
            {
            }
            IsBusy = false;
            //Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

        }
    }
}