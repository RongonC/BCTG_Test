using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
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
    public partial class CampusPage : ContentPage
    {
        private EntityListViewModel _entitylistVM = new EntityListViewModel();

        public EntityListViewModel EntityListVM
        {
            get
            {
                if (_entitylistVM == null)
                    _entitylistVM = new EntityListViewModel();
                return _entitylistVM;
            }

            set
            {
                if (_entitylistVM == value)
                    return;
                _entitylistVM = value;
            }
        }


        public CampusPage(EntityClass _entdetail)
        {
            InitializeComponent();
            try
            {

                Task.Run(() =>
                {
                    var res = EntitySyncAPIMethods.GetEntityByEntityID(App.Isonline, _entdetail.EntityID.ToString(), Functions.UserName, _entdetail.EntityTypeID.ToString(), App.DBPath);
                    res.Wait();
                    _entdetail = res.Result;
                }).Wait();


                EntityFieldListView ent = new EntityFieldListView(_entdetail, new List<string>() { "EXTPK", "TITLE" }, new List<string>() { "1769" });
                frmField.Children.Add(ent);

                EntityListCustomControl lstEntity = new EntityListCustomControl();
                frmList.Children.Add(lstEntity);

                EntityListVM.EntityID = _entdetail.EntityID;
                EntityListVM.ScreenCode = "CAMPSRELATED";

                EntityListVM.GetEntityListwithCall().Wait();

                lstEntity.BindingContext = EntityListVM;

                BindingContext = _entdetail;
            }
            catch (Exception e)
            {
            }
        }
    }

}