using StemmonsMobile.Commonfiles;
using StemmonsMobile.CustomControls;
using StemmonsMobile.Views.Cases;
using StemmonsMobile.Views.Cases_Hopper_Center;
using StemmonsMobile.Views.People_Screen;
using StemmonsMobile.Views.Search;
using StemmonsMobile.Views.Standards;
using StemmonsMobile.Views.View_Case_Origination_Center;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels
{
    class PropLandingViewModel
    {
        public ICommand PropertyButtonCmd { get; private set; }
        public ICommand NewCaseButtonCmd { get; private set; }
        public ICommand AssignButtonCmd { get; private set; }
        public ICommand CreateButtonCmd { get; private set; }
        public ICommand OwnedButtonCmd { get; private set; }
        public ICommand AdvanceButtonCmd { get; private set; }
        public ICommand StandardsButtonCmd { get; private set; }
        public ICommand HopperButtonCmd { get; private set; }

        public ICommand EmpSearchButtonCmd { get; private set; }

   
        public PropLandingViewModel()
        {
            PropertyButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Propertypage());
            });

            NewCaseButtonCmd = new Command(async () =>
            {
                LandingPage.IsCreateEntity = true;
                await Application.Current.MainPage.Navigation.PushAsync(new SelectCaseType());
            });

            AssignButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));
            });

            CreateButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CaseList("caseCreateBySAM", Functions.UserName, ""));
            });

            OwnedButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new CaseList("caseOwnerSAM", Functions.UserName, ""));
            });

            AdvanceButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new SearchPage("Search Cases"));
            });

            StandardsButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new StandardsBookPage(""));
            });

            HopperButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new SelectCaseHopper(Functions.UserName, ""));
            });

            EmpSearchButtonCmd = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new UserSearch());
            });
        }
    }
}
