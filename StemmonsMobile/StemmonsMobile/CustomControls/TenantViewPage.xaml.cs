using DataServiceBus.OfflineHelper.DataTypes.Entity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.DataTypes.DataType.Entity;
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
    public partial class TenantViewPage : ContentPage
    {

        private EntityFieldViewModel _entityFieldVM;

        public EntityFieldViewModel EntityFieldVM
        {
            get { return _entityFieldVM; }
            set { _entityFieldVM = value; }
        }

        EntityClass Entdetail = new EntityClass();
        public TenantViewPage(EntityClass _entdetail)
        {
            InitializeComponent();
            Entdetail = _entdetail;
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            await Task.Run(() =>
             {
                 var res = EntitySyncAPIMethods.GetEntityByEntityID(true, Entdetail.EntityID.ToString(), Functions.UserName, Entdetail.EntityTypeID.ToString(), App.DBPath);
                 res.Wait();
                 Entdetail = res.Result;
             });

            Title = Entdetail.EntityTitle;
            SetData(Entdetail);

            IsBusy = false;
        }


        public void SetData(EntityClass _entdetail)
        {
            try
            {
                EntityFieldListView ent = new EntityFieldListView(_entdetail, new List<string>() { "EXTPK", "TITLE", "STTUS" }, new List<string>() { "5605", "5608", "5609", "5611", "5613", "5615", "5628", "5635", "5636", "5637", "7895", "7896", "7897" });
                frmField.Content = ent;
            }
            catch (Exception e)
            {
            }
        }
    }
}

