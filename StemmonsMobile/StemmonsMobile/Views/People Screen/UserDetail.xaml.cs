using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.PeopleScreen;
using StemmonsMobile.Views.View_Case_Origination_Center;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.People_Screen
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserDetail : ContentPage
    {
        GetUserInfoResponse.UserInfo User_Details = new GetUserInfoResponse.UserInfo();
        String UserName = string.Empty;
        String navScreenNAme = string.Empty;

        public UserDetail(String _userName, string NavigationFrom = "")
        {
            InitializeComponent();
            User_Details = new GetUserInfoResponse.UserInfo();
            UserName = _userName;
            navScreenNAme = NavigationFrom;
            //ProfileImg.Source = UriImageSource.FromUri(new Uri("http://services.boxerproperty.com/userphotos/DownloadPhoto.aspx?username=" + Functions.UserName));

            if (App.Isonline)
                ProfileImg.Source = new UriImageSource
                {
                    Uri = new Uri(DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl + "/userphotos/DownloadPhotomobile.aspx?username=" + UserName),
                    CachingEnabled = true,
                };
            else
                ProfileImg.Source = ImageSource.FromFile("Assets/userIcon.png");
           
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            List<UserDetaildata> lst = new List<UserDetaildata>();
            try
            {
                await Task.Run(() =>
                {
                    User_Details = CasesSyncAPIMethods.GetUserInfo(App.Isonline, UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserName);
                });

                if (User_Details != null)
                {
                    if (App.Isonline)
                    {
                        lst = new List<UserDetaildata>
                        {
                         new UserDetaildata("Assigned Cases", Convert.ToString(User_Details.ASSIGNED_COUNT)),
                         new UserDetaildata("Owned Cases",Convert.ToString(User_Details.OWNED_COUNT)),
                         new UserDetaildata("Created Cases",Convert.ToString(User_Details.CREATED_COUNT)),
                         new UserDetaildata("Related Items",Convert.ToString(User_Details.Entity_Count))
                        };

                        if (Functions.HasTeam)
                            lst.Add(new UserDetaildata("Direct Reports", Convert.ToString(User_Details.Team_count)));

                        listdata.ItemsSource = lst;
                    }

                    //labelDisplayName.Text = User_Details.DisplayName;
                    //labelRealName.Text = User_Details.DisplayName;
                    //labelDepartmentName.Text = User_Details.Department;
                    //labelJobTitle.Text = User_Details.PrimaryJobTitle;
                    //OfficePhone.Text = User_Details.OfficePhone;
                   // CellPhone.Text = User_Details.CellPhone;
                    //CityState.Text = User_Details.City + ", " + User_Details.State;
                   // EmailAddress.Text = User_Details.Email;
                    //lbl_supervisor.Text = User_Details.Supervisor;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception)
            {
            }
            BindingContext = User_Details;
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        void OfficePhoneClick(object sender, System.EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(OfficePhone.Text))
                    AttempPhoneCall(OfficePhone.Text);
            }
            catch (Exception ex)
            {
            }
        }

        void CellPhoneClicked(object sender, System.EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(CellPhone.Text))
                    AttempPhoneCall(CellPhone.Text);
            }
            catch (Exception ex)
            {

            }
        }

        void Manager_clicked(object sender, System.EventArgs e)
        {
            try
            {
                this.Navigation.PushAsync(new UserDetail(User_Details.SupervisorSAM));
            }
            catch (Exception)
            {
            }
        }

        void SendMail(object sender, System.EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(EmailAddress.Text))
                {
                    Uri t = new Uri("mailto:" + EmailAddress.Text);
                    Device.OpenUri(t);
                }
            }
            catch (Exception ex)
            {

            }
        }
        void AttempPhoneCall(String Number)
        {
            try
            {


                Device.OpenUri(new Uri("tel:" + Number));
            }
            catch (Exception ex)
            {
            }
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {

            try
            {
                ListView lst = (ListView)sender;
                var value = lst.SelectedItem as UserDetaildata;

                if (App.Isonline)
                {
                    if (value.Category == "Assigned Cases")
                    {
                        string sNAME = string.Empty;

                        if (string.IsNullOrEmpty(navScreenNAme))
                        {
                            //asssame
                            sNAME = "caseAssgnSAM";
                        }
                        else
                        {
                            //assteam
                            sNAME = "caseAssgnTM";
                        }


                        //if (Functions.UserName == User_Details.UserID)
                        //{
                        //    sNAME = "caseAssgnSAM";
                        //}
                        //else
                        //{
                        //    //                            sNAME="tm"
                        //}
                        this.Navigation.PushAsync(new CaseList(sNAME, User_Details.UserID, "", "", User_Details.UserID));
                    }
                    else if (value.Category == "Owned Cases")
                    {
                        this.Navigation.PushAsync(new CaseList("caseOwnerSAM", User_Details.UserID, "", "", User_Details.UserID));
                    }
                    else if (value.Category == "Created Cases")
                    {
                        this.Navigation.PushAsync(new CaseList("caseCreateBySAM", User_Details.UserID, "", "", User_Details.UserID));
                    }
                    else if (value.Category == "Direct Reports")
                    {
                        this.Navigation.PushAsync(new DirectReports(User_Details.UserID));
                    }
                    else
                    {
                        Navigation.PushAsync(new RelatedItems(User_Details.UserID, User_Details.FirstName + " " + User_Details.LastName));
                    }
                }
            }
            catch (Exception)
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

        void Manager_clickd(object sender, System.EventArgs e)
        {
            try
            {
                this.Navigation.PushAsync(new UserDetail(User_Details.SupervisorSAM));
            }
            catch (Exception)
            {
            }
        }
    }

    public class UserDetaildata
    {
        public string Category { get; set; }
        public string Details { get; set; }

        public UserDetaildata(String category, string detail)
        {
            Category = category;
            Details = detail;
        }
    }
}
