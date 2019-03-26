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

            EmpInfoVM = new EmpInfoViewModel(_entity.EntityID.ToString());
            EmpInfoVM.SelectedEntity = _entity;

            this.BindingContext = EmpInfoVM;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                ProfileImg.Source = Functions.GetImageFromEntityAssoc(EmpInfoVM.SelectedEntity.AssociationFieldCollection);
            }
            catch (Exception)
            {
                ProfileImg.Source = "Assets/PropertyImage.png";
            }

            lst_Employee_Role.SelectedItem = null;
        }
    }
}