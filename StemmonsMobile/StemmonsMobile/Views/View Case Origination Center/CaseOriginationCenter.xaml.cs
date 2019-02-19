using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StemmonsMobile.Commonfiles;
using DataServiceBus.OnlineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.LoginProcess;

namespace StemmonsMobile.Views.View_Case_Origination_Center
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseOriginationCenter : ContentPage
    {
        List<GetCaseTypesResponse.CaseType> lst = new List<GetCaseTypesResponse.CaseType>();
        public CaseOriginationCenter()
        {
            InitializeComponent();
            App.SetConnectionFlag();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Searchbar1.Text = "";
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            await Task.Run(() =>
             {
                 var result = CasesSyncAPIMethods.GetCaseTypes(App.Isonline, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                 result.Wait();
                 lst = result.Result;
             });
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

            listCaseType.ItemsSource = lst;
        }
        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            try
            {
                if (lst.Count > 0)
                {
                    if (string.IsNullOrEmpty(e.NewTextValue))
                    {
                        listCaseType.ItemsSource = lst;
                    }
                    else
                    {
                        listCaseType.ItemsSource = lst.Where(x => x.Name.Contains(e.NewTextValue));
                        if (lst.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower())).ToList().Count <= 0)
                        {
                            DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                            ((SearchBar)sender).Text = "";
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            ListView l = (ListView)sender;
            var sd = (GetCaseTypesResponse.CaseType)l.SelectedItem;
            this.Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(sd.CaseTypeID), ""));
            listCaseType.SelectedItem = null;

        }

        private void HomeIcon_Click(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private void btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);
            }
            catch (Exception)
            {
            }
        }
    }

    //public class CasesOriginationData
    //{

    //    public string CaseTypeID { get; set; }
    //    public string Name { get; set; }
    //    public string InstanceName { get; set; }
    //    public string InstanceNamePlural { get; set; }
    //    public string Description { get; set; }
    //    public string SPOriginationOrverrideUrl { get; set; }
    //    public string IsActive { get; set; }
    //    public string CreatedDateTime { get; set; }
    //    public string CreatedBy { get; set; }
    //    public string ModifiedDateTime { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public string HasSecurity { get; set; }
    //    public string SecurityGroups { get; set; }
    //    public string LockWhenUnowned { get; set; }
    //    public string SceurityType { get; set; }
    //    public string HasRightsToConfigSecurity { get; set; }
    //}
}
