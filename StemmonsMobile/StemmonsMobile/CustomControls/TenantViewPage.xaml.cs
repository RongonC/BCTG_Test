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
        public TenantViewPage(EntityClass _entdetail)
        {
            InitializeComponent();
            Title = _entdetail.EntityTitle;
            SetData(_entdetail);
        }

        public void SetData(EntityClass _entdetail)
        {
            try
            {
                // EntityFieldVM = new EntityFieldViewModel(_entdetail);

                EntityFieldListView ent = new EntityFieldListView(_entdetail, new List<string>() { "EXTPK", "TITLE", "STTUS" }, new List<string>() { "5605", "5608", "5609", "5611", "5613", "5615", "5628", "5635", "5636", "5637", "7895", "7896", "7897" });
                frmField.Content = ent;
            }
            catch (Exception e)
            {
            }
        }
    }
}

