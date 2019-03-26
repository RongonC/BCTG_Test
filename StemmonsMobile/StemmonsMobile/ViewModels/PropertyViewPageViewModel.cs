using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using StemmonsMobile.CustomControls;
using StemmonsMobile.DataTypes.DataType.Entity;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;
using StemmonsMobile.Views.View_Case_Origination_Center;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Commonfiles;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using StemmonsMobile.Views.Entity;
using System.ComponentModel;

namespace StemmonsMobile.ViewModels
{
    public class PropertyViewPageViewModel
    {
        public ICommand PropertyCaseCmd { get; private set; }
        public ICommand PropertyinfomationCmd { get; private set; }
        public ICommand PropertyEmpolyeeCmd { get; private set; }
        public ICommand PropertyTenantsCmd { get; private set; }
        public ICommand AvailableUnitsCmd { get; private set; }
        public ICommand PropertyEntityCmd { get; private set; }

        public EntityClass EntityDetails { get; set; }

        private bool _isbusy;

        public bool IsBUSY
        {
            get { return _isbusy; }
            set { _isbusy = value; }
        }

        public PropertyViewPageViewModel()
        {
            PropertyCaseCmd = new Command(async (parameter) =>
            {
                if (parameter != null)
                {
                    var cm = parameter as PropertyViewPageViewModel;
                    await Application.Current.MainPage.Navigation.PushAsync(new CaseList("RELATEDCASES", cm.EntityDetails.EntityID.ToString(), "", "", "", true));

                }
            });


            PropertyinfomationCmd = new Command(async (parameter) =>
            {
                if (parameter != null)
                {
                    var cm = parameter as PropertyViewPageViewModel;
                    IsBUSY = true;
                    await Application.Current.MainPage.Navigation.PushAsync(new PropertyInfoPage(cm.EntityDetails));
                    IsBUSY = false;
                }
            });
            PropertyEmpolyeeCmd = new Command((parameter) =>
            {
                if (parameter != null)
                {
                    var cm = parameter as PropertyViewPageViewModel;
                    IsBUSY = true;
                    Application.Current.MainPage.Navigation.PushAsync(new EmpInfoPage(cm.EntityDetails));
                    IsBUSY = false;
                }
            });

            PropertyTenantsCmd = new Command((parameter) =>
            {
                if (parameter != null)
                {
                    IsBUSY = true;
                    var cm = parameter as PropertyViewPageViewModel;
                    Application.Current.MainPage.Navigation.PushAsync(new TenantInfoPage(cm.EntityDetails, "TNTLIST"));
                    IsBUSY = false;
                }
            });

            AvailableUnitsCmd = new Command((parameter) =>
            {
                if (parameter != null)
                {
                    IsBUSY = true;
                    var cm = parameter as PropertyViewPageViewModel;
                    Application.Current.MainPage.Navigation.PushAsync(new TenantInfoPage(cm.EntityDetails, "UNITS"));
                    IsBUSY = false;
                }
            });
            // same as Tanent Page should pass System code only

            PropertyEntityCmd = new Command((parameter) =>
            {
                if (parameter != null)
                {
                    IsBUSY = true;
                    var cm = parameter as PropertyViewPageViewModel;
                    Application.Current.MainPage.Navigation.PushAsync(new Entity_View(cm.EntityDetails));
                    IsBUSY = false;
                }
            });
            // Entity View Page
        }

        public ImageSource ImgProperty
        {
            get
            {
                return Functions.GetImageFromEntityAssoc(EntityDetails.AssociationFieldCollection);
            }
        }
    }
}
