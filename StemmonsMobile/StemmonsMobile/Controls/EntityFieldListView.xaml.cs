using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
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
    public partial class EntityFieldListView : ContentView
    {

        private EntityFieldViewModel _entityfieldVM;

        public EntityFieldViewModel EntityFieldVM
        {
            get { return _entityfieldVM; }
            set { _entityfieldVM = value; }
        }

        public EntityFieldListView(EntityClass _entdetail, List<string> _acsystemcode = null, List<string> _assoctypeid = null)
        {
            InitializeComponent();
            try
            {
                EntityFieldVM = new EntityFieldViewModel(_entdetail, _acsystemcode, _assoctypeid);
                EntityFieldVM.SelectedEntity = _entdetail;
                EntityFieldVM.AcSystemCode = null;
                BindingContext = EntityFieldVM;
            }
            catch (System.Exception ec)
            {

            }
        }
    }
}