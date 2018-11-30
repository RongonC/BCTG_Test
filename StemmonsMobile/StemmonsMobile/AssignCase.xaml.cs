using DataServiceBus.OfflineHelper.DataTypes.Cases;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using StemmonsMobile.Views.Cases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class AssignCase : ContentPage
    {

        string CaseId = "";
        GetUserInfoResponse.UserInfo userdata;
        string sCaseTypeId = "";
        object oCreatecase = null;
        string sCaseNotes = "";
        string sUserName = "";
        object oSaveCase = null;
        string sMode = "";
        string Screemname = "";
        bool bApproveCase = false;
        bool bDeclineCase = false;
        bool bOnlineflag = false;

        public AssignCase(string caseid, string caseTypeId = "", object createcase = null, string caseNotes = "", string userName = "", string mode = "", object saveCase = null, bool isApproveCase = false, bool isdeclineCase = false, string screemname = "", bool Onlineflag = false)
        {
            this.InitializeComponent();
            CaseId = caseid;
            sCaseTypeId = caseTypeId;
            oCreatecase = createcase;
            sCaseNotes = caseNotes;
            sUserName = userName;
            oSaveCase = saveCase;
            sMode = mode;
            bApproveCase = isApproveCase;
            bDeclineCase = isdeclineCase;
            searchTxt.Focus();
            (searchTxt as Entry).Completed += Search_Clicked;
            Screemname = screemname;
            bOnlineflag = Onlineflag;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            searchTxt.Focus();
        }
        private void Search_Clicked(object sender, EventArgs e)
        {
            Task<List<GetUserInfoResponse.UserInfo>> val = CasesSyncAPIMethods.GetEmployeesBySearch(searchTxt.Text ?? "", App.DBPath, Functions.UserName);
            val.Wait();

            if (val?.Result?.Count > 0)
            {
                List<GetUserInfoResponse.UserInfo> d = new List<GetUserInfoResponse.UserInfo>();
                ObservableCollection<UserGroup> Groups = new ObservableCollection<UserGroup>();
                ObservableCollection<UserGroup> Groups1 = new ObservableCollection<UserGroup>();


                ObservableCollection<Group> groupedItems = new ObservableCollection<Group>();
                Group group1 = new Group("Result - Frequently Used");
                groupedItems.Add(group1);


                Group group = new Group("Result - Full List");
                groupedItems.Add(group);
                foreach (var item in val.Result)
                {
                    userdata = item;
                    group.Add(userdata);
                }

                listViewFoundUsers.ItemsSource = groupedItems;
            }
        }

        private async void Assign_Clicked(object sender, EventArgs e)
        {
            var mi = ((Button)sender);
            var value = mi.CommandParameter as GetUserInfoResponse.UserInfo;

            await AssignTouser(value);
        }

        private async Task AssignTouser(GetUserInfoResponse.UserInfo value)
        {
            Task<int> iRecord = null;

            string NoteTypeID = "19";
            try
            {
                if (sMode != "C")
                {
                    if ((oSaveCase as SaveCaseTypeRequest).noteTypeID > 0)
                    {
                        NoteTypeID = (oSaveCase as SaveCaseTypeRequest).noteTypeID.ToString();
                    }
                }
            }
            catch { }
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            if (sMode == "C")
            {
                try
                {
                    await Task.Run(() =>
                    {
                        iRecord = CasesSyncAPIMethods.StoreAndcreateCase(bOnlineflag, Convert.ToInt32(sCaseTypeId), oCreatecase, sCaseNotes, sUserName, App.DBPath, value, Functions.UserFullName, NoteTypeID, Screemname);
                        iRecord.Wait();
                    });
                }
                catch (Exception)
                {
                }
            }
            else if (bApproveCase && sMode != "C")
            {
                try
                {
                    await Task.Run(() =>
                    {
                        iRecord = CasesSyncAPIMethods.StoreAndUpdateCase(bOnlineflag, Convert.ToInt32(sCaseTypeId), oSaveCase, sCaseNotes, sUserName, App.DBPath, oCreatecase, value, null, NoteTypeID, bApproveCase, false, Functions.UserFullName, Screemname);
                        iRecord.Wait();
                    });
                }
                catch (Exception)
                {
                }
            }
            else if (bDeclineCase && sMode != "C")
            {
                try
                {
                    await Task.Run(() =>
                    {
                        iRecord = CasesSyncAPIMethods.StoreAndUpdateCase(bOnlineflag, Convert.ToInt32(sCaseTypeId), oSaveCase, sCaseNotes, sUserName, App.DBPath, oCreatecase, value, null, NoteTypeID, false, true, Functions.UserFullName, Screemname);
                        iRecord.Wait();
                    });
                }
                catch (Exception)
                {
                }
            }
            else
            {
                try
                {
                    await Task.Run(() =>
                    {
                        iRecord = CasesSyncAPIMethods.StoreAndUpdateCase(bOnlineflag, Convert.ToInt32(sCaseTypeId), oSaveCase, sCaseNotes, sUserName, App.DBPath, oCreatecase, value, null, NoteTypeID, false, false, Functions.UserFullName, Screemname);
                        iRecord.Wait();
                    });
                }
                catch (Exception)
                {
                }
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

            if (iRecord != null && iRecord.Result > 0)
            {
                int AssignCAseID = iRecord.Result;
                Functions.ShowtoastAlert("Case Assigned Successfully.");
                if (sMode == "C")
                {
                    this.Navigation.PushAsync(new ViewCasePage(AssignCAseID.ToString(), sCaseTypeId, ""));
                }
                else
                    this.Navigation.PopAsync();
            }
            else
            {
                Functions.ShowtoastAlert("Case Not Assigned. please Try Again Later.");
                this.Navigation.PopAsync();
            }
        }
        async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
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

        private void AssignCase_Completed(object sender, EventArgs e)
        {
            var text = (sender as Entry).Text;
        }

        private async void ListViewFoundUsers_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var tem = ((ListView)sender).SelectedItem as GetUserInfoResponse.UserInfo;
            await AssignTouser(tem);
        }
    }



    public class Group : ObservableCollection<GetUserInfoResponse.UserInfo>
    {
        public String Title { get; private set; }

        public Group(String title)
        {
            this.Title = title;
        }
    }

    public class AssignCasePageData
    {
        public string ID { get; set; }
        public string MiddleName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string PrimaryJobTitle { get; set; }
        public string CellPhone { get; set; }

        public string Department { get; set; }
        public string OfficePhone { get; set; }

        public string Email { get; set; }
        public string City { get; set; }

        public string State { get; set; }
        public string DisplayName { get; set; }

        public string UserID { get; set; }
        public string Supervisor { get; set; }

        public string FirstLastName
        {
            get
            {

                if (PrimaryJobTitle != null)
                {
                    return $"{FirstName} {LastName}  {"(" + PrimaryJobTitle + ")"}";
                }
                else
                {
                    return $"{FirstName} {LastName} ";
                }


            }
        }
    }

}