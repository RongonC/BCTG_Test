using StemmonsMobile.Commonfiles;
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

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get
            {
                return Functions.GetImageFromEntityAssoc(EntityFieldVM.SelectedEntity.AssociationFieldCollection);
            }
        }

        EntityClass Entdetail = new EntityClass();
        List<string> AcsyStemcode = new List<string>();
        List<string> AssocTypeid = new List<string>();
        public EntityFieldListView(EntityClass _entdetail, List<string> _acsystemcode = null, List<string> _assoctypeid = null)
        {
            InitializeComponent();
            Entdetail = _entdetail;
            AcsyStemcode = _acsystemcode;
            AssocTypeid = _assoctypeid;
            
          
            try
            {
                EntityFieldVM = new EntityFieldViewModel(Entdetail, AcsyStemcode, AssocTypeid);
                EntityFieldVM.SelectedEntity = Entdetail;
            }
            catch (System.Exception ec)
            {

            }
            BindingContext = EntityFieldVM;
            
        }
    }
}