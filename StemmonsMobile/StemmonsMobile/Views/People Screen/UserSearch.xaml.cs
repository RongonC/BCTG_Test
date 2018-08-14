using DataServiceBus.OfflineHelper.DataTypes.Cases;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseLastAssigneeResponse;

namespace StemmonsMobile.Views.People_Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSearch : ContentPage
    {
        public UserSearch()
        {
            InitializeComponent();
            Title = "Employee Search";
            SearchText.Focus();
        }

        private void btn_search_Clicked(object sender, EventArgs e)
        {
            try
            {
                string search = SearchText.Text;
                var result = CasesSyncAPIMethods.GetEmployeesBySearch(search, App.DBPath,Functions.UserName);

                result.Wait();

                var resultList = result.Result.Where(x => x.FirstName != null && !x.FirstName.ToUpper().Contains("[HOPPER:"));
                if (resultList.Count() > 0)
                {
                    lstSearchitem.ItemsSource = null;
                    lstSearchitem.ItemsSource = resultList;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? StemmonsMobile.Commonfiles.Functions.nRcrdOnline : StemmonsMobile.Commonfiles.Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async Task lstSearchitem_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                ListView l = (ListView)sender;
                var sd = (GetUserInfoResponse.UserInfo)l.SelectedItem;


                await Navigation.PushAsync(new UserDetail(sd.UserID));

                lstSearchitem.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }
        private void btn_home_Clicked(object sender, EventArgs e)
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
}