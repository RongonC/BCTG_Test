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

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpInfoPage : ContentPage
    {

        private EmpInfoViewModel _empinfovm;

        public EmpInfoViewModel EmpInfoVM
        {
            get { return _empinfovm; }
            set { _empinfovm = value; }
        }


        public EmpInfoPage(EntityClass _entity)
        {
            InitializeComponent();

            _empinfovm = new EmpInfoViewModel(_entity.EntityID.ToString());
            _empinfovm.SelectedEntity = _entity;

            this.BindingContext = EmpInfoVM;

            try
            {
                ProfileImg.Source = Functions.GetImageFromEntityAssoc(_entity.AssociationFieldCollection);
            }
            catch (Exception)
            {
                ProfileImg.Source = "Assets/PropertyImage.png";
            }
        }
    }
}