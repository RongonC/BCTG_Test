using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyViewPage : ContentPage
    {
        string fileStr = string.Empty;

        private PropertyViewPageViewModel _propertyviewvm;

        public PropertyViewPageViewModel PropertyViewVM
        {
            get
            {
                if (_propertyviewvm == null)
                    _propertyviewvm = new PropertyViewPageViewModel();
                return _propertyviewvm;
            }

            set
            {
                if (_propertyviewvm == value)
                    return;
                _propertyviewvm = value;
            }
        }
        EntityClass Entdetail = new EntityClass();
        public PropertyViewPage(EntityClass _entdetail)
        {
            InitializeComponent();
            Entdetail = _entdetail;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            PropertyViewVM.IsBUSY = true;

            await Task.Run(() =>
             {
                 var res = EntitySyncAPIMethods.GetEntityByEntityID(true, Entdetail.EntityID.ToString(), Functions.UserName, Entdetail.EntityTypeID.ToString(), App.DBPath);
                 res.Wait();
                 Entdetail = res.Result;
             });
            PropertyViewVM.EntityDetails = Entdetail;

            PropertyViewVM.IsBUSY = false;

            this.BindingContext = PropertyViewVM;

            try
            {
                PropertyPicturecodebtn.Source = Functions.GetImageFromEntityAssoc(PropertyViewVM.EntityDetails.AssociationFieldCollection);
            }
            catch (Exception)
            {
                PropertyPicturecodebtn.Source = "Assets/PropertyImage.png";
            }
        }
    }
}