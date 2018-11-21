using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.Connectivity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.Models;
using StemmonsMobile.ViewModels;
using StemmonsMobile.Views.Cases;
using StemmonsMobile.Views.Cases_Hopper_Center;
using StemmonsMobile.Views.CreateQuestForm;
using StemmonsMobile.Views.Entity;
using StemmonsMobile.Views.People_Screen;
using StemmonsMobile.Views.PeopleScreen;
using StemmonsMobile.Views.Search;
using StemmonsMobile.Views.Setting;
using StemmonsMobile.Views.Standards;
using StemmonsMobile.Views.View_Case_Origination_Center;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LandingPage : ContentPage
    {
        ApplicationListViewModel cm;
        List<string> project = new List<string>();

        public LandingPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.SizeChanged += LandingPage_SizeChanged;

            LandingPageCountDisplay();


            btn_usericon.GestureRecognizers.Add(new TapGestureRecognizer(img_profile_OnTap));

            cm = new ApplicationListViewModel();

            if (Functions.Platformtype == "UWP")
            {
                if (this.Width <= 320)
                {
                    for (int i = 0; i < cm.appdata.Count; i++)
                    {
                        cm.appdata[i].My_Icon = "Assets/320/actor1.png";
                        cm.appdata[i].New_Icon = "Assets/320/plusGrey.png";
                        cm.appdata[i].Find_Icon = "Assets/320/magnify.png";
                        cm.appdata[i].Arrow_Icon = "Assets/320/downarrow.png";
                    }

                    cm.appdata[0].ApplicationIcon = "Assets/320/Casesblack_1.png";
                    cm.appdata[1].ApplicationIcon = "Assets/320/entitiesblack_1.png";
                    cm.appdata[2].ApplicationIcon = "Assets/320/Departmentblack_1.png";
                    cm.appdata[3].ApplicationIcon = "Assets/320/standardsblack.png";
                    cm.appdata[4].ApplicationIcon = "Assets/320/Questblack_1.png";
                }
            }

            Applicationlist.ItemsSource = cm.appdata;

            GetBaseURLFromSQLServer();
        }
        private void LandingPage_SizeChanged(object sender, EventArgs e)
        {

            if (Functions.Platformtype == "UWP")
            {
                if (this.Width <= 480)
                {
                    #region Profile Icon Grid 

                    FileImageSource img = new FileImageSource
                    {
                        File = "Assets/320/userIcon.png"
                    };
                    btn_usericon.Source = img;

                    //btn_mycases
                    //CaseNotification
                    img = new FileImageSource();
                    img.File = "Assets/320/casesblack.png";
                    btn_mycases.Image = img;
                    btn_mycases.WidthRequest = 44;
                    btn_mycases.HeightRequest = 44;
                    CaseNotification.Margin = new Thickness(-5, 19, 0, 0);

                    //btn_association
                    //associationNoti
                    img = new FileImageSource();
                    img.File = "Assets/320/entitiesblack.png";
                    btn_association.Image = img;
                    associationNotification.Margin = new Thickness(-5, 19, 0, 0);
                    btn_association.WidthRequest = 44;
                    btn_association.HeightRequest = 44;


                    //btn_team
                    //teamNotification
                    img = new FileImageSource();
                    img.File = "Assets/320/Departmentblack.png";
                    btn_team.Image = img;
                    teamNotification.Margin = new Thickness(-5, 19, 0, 0);
                    btn_team.WidthRequest = 44;
                    btn_team.HeightRequest = 44;

                    //btn_standard
                    //standardNotification
                    img = new FileImageSource();
                    img.File = "Assets/320/standardsblack.png";
                    btn_standard.Image = img;
                    standardNotification.Margin = new Thickness(-5, 19, 0, 0);
                    btn_standard.WidthRequest = 44;
                    btn_standard.HeightRequest = 44;


                    //btn_quests
                    //formNotification
                    img = new FileImageSource();
                    img.File = "Assets/320/questblack.png";
                    btn_quest.Image = img;
                    btn_quest.WidthRequest = 44;
                    btn_quest.HeightRequest = 44;
                    formNotification.Margin = new Thickness(-5, 19, 0, 0);
                    #endregion
                }
            }
        }

        //Master API Call
        public async void SyncAllRecorsToSQLite()
        {
            try
            {
                bool IsClosed = false;
                ProgressDialogConfig config = new ProgressDialogConfig();
                IProgressDialog dlg = null;

                if (Functions.AppStartCount <= 1)
                {
                    config = new ProgressDialogConfig().SetTitle("Please Wait...")
                                                         // setting false will just create a spinner
                                                         .SetIsDeterministic(true)
                                                         // prevents user from clicking background
                                                         .SetMaskType(MaskType.Black);
                    // cancel text and action
                    //.SetCancel("Run in Background", () => { SyncAllRecorsToSQLite_Backgroud(); });

                    dlg = UserDialogs.Instance.Progress(config);
                    dlg.Show();
                }
                else
                {
                    config = new ProgressDialogConfig().SetTitle("Please Wait...")
                                                                        // setting false will just create a spinner
                                                                        .SetIsDeterministic(true)
                                                                        // prevents user from clicking background
                                                                        .SetMaskType(MaskType.Black)
                                   // cancel text and action
                                   .SetCancel("Run in Background", () => { IsClosed = true; dlg.Hide(); });

                    dlg = UserDialogs.Instance.Progress(config);
                    dlg.Show();
                }

                int itemCount = 12;
                int Percentage = 100 / 13;

                try
                {
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        #region Cases Sync
                        //Cases Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.PercentComplete = Percentage;
                                                        dlg.Title = "Cases Items Sync Operation " + 1 + " of " + itemCount + " in Progress";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = CasesSyncAPIMethods.GetAllCaseTypeWithID(null, Functions.UserName, "Frequent", App.DBPath);
                                    if (!string.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Cases Items Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Cases Items Sync having issue.");
                                }
                                catch (Exception ex)
                                {

                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 2;
                                                       dlg.Title = "Case Types and Items Sync Operation " + 1 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Cases Origination Sync
                        //Cases Originaation Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    dlg.Title = "Case Types Sync Operation " + 2 + " of " + itemCount + " in Progress";
                                });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = CasesSyncAPIMethods.GetOriginationCenterForUserSync(Functions.UserName, "Y", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

                                    if (!string.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Case Types Sync Operation Success.");
                                    else
                                        Functions.ShowtoastAlert("Case Types Sync Operation having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.PercentComplete = Percentage * 3;
                                                        dlg.Title = "Case Types Sync Operation Sync Operation " + 2 + " of " + itemCount + " completed";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Entity Sync
                        //Entity Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Entity Types and Items Sync Operation " + 3 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = EntitySyncAPIMethods.GetAllEntityTypeWithID(null, Functions.UserName, App.DBPath);

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Entity Types and Items Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Entity Types and Items Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 4;
                                                       dlg.Title = "Entity Types and Items Sync Operation " + 3 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Quest Sync
                        //Quest Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Quest Area and Item List Sync Operation " + 4 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = QuestSyncAPIMethods.GetAllQuest(null, Functions.UserName, App.DBPath);

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Quest Area and Item List Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Quest Area and Item List Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 5;
                                                       dlg.Title = "Quest Area and Item List Sync Operation " + 4 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Standard Sync
                        //Standard Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.Title = "Standard Application Data Sync Operation " + 5 + " of " + itemCount + " in Progress";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = StandardsSyncAPIMethods.GetAllStandards(Functions.UserName, App.DBPath);

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Standard Application Data Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Standard Application Data Sync having issue.");

                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 6;
                                                       dlg.Title = "Standard Application Data Sync Operation " + 5 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Employee Search Sync
                        //Employee Search Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Employee Data Sync Operation " + 6 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {

                                    CasesSyncAPIMethods.GetTeamMembers(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

                                    string str = CasesSyncAPIMethods.GetAllEmployeeUser(App.DBPath, Functions.UserName);

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Employee Data Sync Success.");
                                    else
                                    {
                                        Functions.ShowtoastAlert("Employee Data Sync having issue.");
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 7;
                                                       dlg.Title = "Employee Data Sync Operation " + 6 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Home Count Sync
                        //Home Count Sync
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Origination Center Data Sync Operation " + 7 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    var str = HomeOffline.GetAllHomeCount(Functions.UserName, Functions.Selected_Instance, App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Origination Center Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Origination Center Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.PercentComplete = Percentage * 8;
                                                        dlg.Title = "Origination Center Sync Operation " + 7 + " of " + itemCount + " completed";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region CaseList Assigned To Me
                        //Get CaseList Assigned To Me
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Get CaseList Assigned To Me List Sync Operation " + 8 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", "", Functions.UserName, "", "", "", "", "0", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, "", true, "_AssignedToMe");

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Get CaseList Assigned To Me List Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Get CaseList Assigned To Me List Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.PercentComplete = Percentage * 8;
                                                        dlg.Title = "Get CaseList Assigned To Me List Sync Operation " + 8 + " of " + itemCount + " completed";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region CaseList Created By Me
                        //Get CaseList Created By Me
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.Title = "Get CaseList Created By Me List Sync Operation " + 9 + " of " + itemCount + " in Progress";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {

                                    string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", "", "", "", Functions.UserName, "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, "", true, "_CreatedByMe");

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Get CaseList Created By Me List Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Get CaseList Created By Me List Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });

                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 9;
                                                       dlg.Title = "Get CaseList Created By Me List Sync Operation " + 9 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region CaseList Owned By Me
                        //Get CaseList Owned By Me
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Get CaseList Owned By Me List Sync Operation " + 10 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {
                                    string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", Functions.UserName, "", "", "", "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, "", true, "_OwnedByMe");

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Get CaseList Owned By Me List Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Get CaseList Owned By Me List Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });

                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.PercentComplete = Percentage * 10;
                                                        dlg.Title = "Get CaseList Owned By Me List Sync Operation " + 10 + " of " + itemCount + " completed";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region CaseList Assigned To My Team

                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.Title = "Get CaseList Assigned To My Team List Sync Operation " + 11 + " of " + itemCount + " in Progress";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            if (Functions.HasTeam)
                                await Task.Run(() =>
                                {
                                    try
                                    {
                                        string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", Functions.UserName, "", "", "", "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, "", true, "_AssignedToMyTeam");

                                        if (!String.IsNullOrEmpty(str))
                                            Functions.ShowtoastAlert("Get CaseList Assigned To My Team List Sync Success.");
                                        else
                                            Functions.ShowtoastAlert("Get CaseList Assigned To My Team List Sync having issue.");
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                });
                            else
                            {
                                DefaultAPIMethod.AddLog("No Team to bind Data", "Y", "GetCaseList_AssignedToMyTeam", Functions.UserName, DateTime.Now.ToString());
                                Functions.ShowtoastAlert("No Team to bind Data for your team.");
                            }
                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                   {
                                                       dlg.PercentComplete = Percentage * 11;
                                                       dlg.Title = "Get CaseList Assigned To My Team List Sync Operation " + 11 + " of " + itemCount + " completed";
                                                   });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion

                        #region Entity associated with Me
                        //Get CaseList Owned By Me
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                  {
                                                      dlg.Title = "Get Entity Associated List Sync Operation " + 12 + " of " + itemCount + " in Progress";
                                                  });
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            await Task.Run(() =>
                            {
                                try
                                {

                                    string str = EntitySyncAPIMethods.GetAssociatedEntityList(Functions.UserName, App.DBPath);

                                    if (!String.IsNullOrEmpty(str))
                                        Functions.ShowtoastAlert("Get Entity Associated List Sync Success.");
                                    else
                                        Functions.ShowtoastAlert("Get Entity Associated List Sync having issue.");
                                }
                                catch (Exception ex)
                                {
                                }
                            });

                        }
                        catch (Exception)
                        {
                        }
                        if (!IsClosed)
                        {
                            try
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dlg.PercentComplete = 100;
                                                        dlg.Title = "Get Entity Associated List Sync Operation " + 12 + " of " + itemCount + " completed";
                                                    });
                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion
                    }

                    dlg.Hide();

                    Functions.AppStartCount = Functions.AppStartCount + 1;
                    Application.Current.Properties["AppStartCount"] = Functions.AppStartCount;
                }
                catch (Exception ex)
                {
                    Application.Current.Properties["AppStartCount"] = 1;
                    Functions.AppStartCount = 1;
                }

                HomePageCount();
            }
            catch (Exception ex)
            {
                Functions.AppStartCount = 1;
                Application.Current.Properties["AppStartCount"] = 1;
            }
        }

        void LandingPageCountDisplay()
        {
            if (Functions.Platformtype.ToUpper() == "IOS")
            {

                if ((Application.Current.MainPage.Width >= 768 && Application.Current.MainPage.Width < 1020))
                {
                    CaseNotification.Margin = new Thickness(7, 4, 0, 0);
                    associationNotification.Margin = new Thickness(40, 4, 0, 0);
                    teamNotification.Margin = new Thickness(10, 4, 0, 0);
                    standardNotification.Margin = new Thickness(30, 4, 0, 0);
                    formNotification.Margin = new Thickness(15, 4, 0, 0);
                }
                if (Application.Current.MainPage.Width >= 1024)
                {
                    CaseNotification.Margin = new Thickness(20, 4, 0, 0);
                    associationNotification.Margin = new Thickness(70, 4, 0, 0);
                    teamNotification.Margin = new Thickness(25, 4, 0, 0);
                    standardNotification.Margin = new Thickness(50, 6, 0, 0);
                    formNotification.Margin = new Thickness(36, 4, 0, 0);
                }
            }
        }

        void HomePageCount()
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                HomeScreenCount appCount = new HomeScreenCount();

                Task.Run(() =>
                {
                    Task.Run(() =>
                    {
                        HomeOffline.GetAllHomeCount(Functions.UserName, Functions.Selected_Instance, App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);
                    }).Wait();

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UpdateCount(appCount);
                    });
                });

                //UpdateCount(appCount);
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        public async void UpdateCount(HomeScreenCount appCount)
        {
            await Task.Run(() =>
            {
                appCount = HomeOffline.GetHomeScreenCount(App.Isonline, Functions.UserName, Functions.Selected_Instance, App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);
            });
            if (appCount == null)
            {
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                return;
            }

            if (!string.IsNullOrEmpty(appCount.CasesCount.ToString()))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CaseNotification.Text = appCount.CasesCount.ToString();
                });
            }
            else
            {
                CaseNotification.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(appCount.EntityCount.ToString()))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    associationNotification.Text = appCount.EntityCount.ToString();
                });
            }
            else
            {
                associationNotification.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(appCount.StandardCount.ToString()))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    standardNotification.Text = appCount.StandardCount.ToString();
                });
            }
            else
            {
                standardNotification.IsVisible = false;
            }

            if (!string.IsNullOrEmpty(appCount.QuestCount.ToString()))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    formNotification.Text = appCount.QuestCount.ToString();
                });

            }
            else
            {
                formNotification.IsVisible = false;
            }
            if (!string.IsNullOrEmpty(appCount.DepartmentCount.ToString()))
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    teamNotification.Text = appCount.DepartmentCount.ToString();
                });

                teamNotification.IsVisible = true;
            }
            else
            {
                teamNotification.IsVisible = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // For App_Logo 
            SetCompanyLogo();

            SetUserPicture();

            // btn_usericon.Source = ImageSource.FromFile("Assets/deletebutton.png");
            project = new List<string>();
            if (App.IsLoginCall)
            {
                SyncAllRecorsToSQLite();
                App.IsLoginCall = false;
            }
            HomePageCount();
        }

        private void SetUserPicture()
        {
            try
            {
                if (App.Isonline)
                {
                    var UPro = DBHelper.GetinstanceuserassocListByID(ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    UPro.Wait();
                    string urlString = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl + "/userphotos/DownloadPhotomobile.aspx?username=" + Functions.UserName;


                    ImageHelperClass iHeleper = new ImageHelperClass();
                    var Src = iHeleper.DownloadImage(Convert.ToString(urlString));
                    Src.Wait();

                    UPro.Result.Profile_Picture = Src.Result;

                    DBHelper.SaveInstanceUserAssoc(UPro.Result, App.DBPath).Wait();

                    btn_usericon.Source = ImageSource.FromUri(new Uri(urlString));
                }
                else
                {
                    var UPro = DBHelper.GetinstanceuserassocListByID(ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    UPro.Wait();

                    byte[] byt = UPro.Result.Profile_Picture as byte[];

                    Image image = new Image();
                    Stream stream = new MemoryStream(byt);
                    btn_usericon.Source = ImageSource.FromStream(() => { return stream; });

                    //btn_usericon.Source = ImageSource.FromFile("Assets/userIcon.png");
                }
            }
            catch (Exception)
            {
                btn_usericon.Source = ImageSource.FromFile("Assets/userIcon.png");
            }
        }

        private void SetCompanyLogo()
        {
            try
            {
                if (App.Isonline)
                {
                    var result = DefaultAPIMethod.GetLogo();
                    var urlString = result.GetValue("ResponseContent");

                    ImageHelperClass iHeleper = new ImageHelperClass();
                    var Src = iHeleper.DownloadImage(Convert.ToString(urlString));
                    Src.Wait();

                    var itm = DBHelper.GetInstanceListByID(Functions.Selected_Instance, App.DBPath);
                    itm.Wait();
                    InstanceList ils = new InstanceList();
                    byte[] byt = Src.Result as byte[];

                    Image image = new Image();
                    Stream stream = new MemoryStream(byt);
                    App_Logo.Source = ImageSource.FromStream(() => { return stream; });

                    ils.Instance_Logo = byt;
                    ils.InstanceID = itm.Result.InstanceID;
                    ils.InstanceName = itm.Result.InstanceName;
                    ils.InstanceUrl = itm.Result.InstanceUrl;

                    DBHelper.SaveInstance(ils, App.DBPath).ConfigureAwait(false);

                }
                else
                {
                    var itm = DBHelper.GetInstanceListByID(Functions.Selected_Instance, App.DBPath);
                    itm.Wait();
                    InstanceList ils = new InstanceList();
                    byte[] byt = itm.Result.Instance_Logo as byte[];

                    Image image = new Image();
                    Stream stream = new MemoryStream(byt);
                    App_Logo.Source = ImageSource.FromStream(() => { return stream; });
                    //App_Logo.Source = ImageSource.FromFile("Assets/boxerlogo.png");
                }
            }
            catch (Exception)
            {
                App_Logo.Source = ImageSource.FromFile("Assets/boxerlogo.png");
            }
        }

        private async void GetBaseURLFromSQLServer()
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                await Task.Run(() =>
                {
                    var lst = Functions.GetImageDownloadURL();
                    App.EntityImgURL = lst.Where(v => v.SYSTEM_CODE.ToUpper() == "ENTHM").FirstOrDefault().VALUE;
                    App.CasesImgURL = lst.Where(v => v.SYSTEM_CODE.ToUpper() == "CSHOM").FirstOrDefault().VALUE;
                    App.StandardImgURL = lst.Where(v => v.SYSTEM_CODE.ToUpper() == "STHOM").FirstOrDefault().VALUE;
                });
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        //List<string> project = new List<string>();
        private async Task SelectUser(string name)
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                List<GetUserInfoResponse.UserInfo> Userlist = new List<GetUserInfoResponse.UserInfo>();
                await Task.Run(() =>
                 {
                     var itemRec = CasesSyncAPIMethods.GetTeamMembers(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                     itemRec.Wait();
                     Userlist = itemRec.Result;
                 });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                if (Userlist?.Count > 0)
                {
                    var options = new List<string>();
                    options.AddRange(Userlist.Select(v => v.DisplayName?.Trim() != null ? v.DisplayName?.Trim() : v.SAMName?.Trim()));

                    // var count = project.Count();
                    //var options = new List<string>(project.Take(count + 1));
                    options.Add("Search for User...");
                    var userAction = await this.DisplayActionSheet(null, "Cancel", null, options.ToArray());


                    if (userAction == "Search for User...")
                    {

                        var result = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                              "Please search for a user to show the cases assigned to.").SetInputMode(
                              InputType.Name).SetOkText("Find"));

                        if (result.Ok)
                        {
                            SearchAgain(result.Text);
                        }
                    }
                    else if (userAction != "Cancel")
                    {
                        userAction = Userlist.Where(v => v.DisplayName != null ? v.DisplayName.ToLower() == userAction.ToLower() : v.SAMName.ToLower() == userAction.ToLower())?.FirstOrDefault().SAMName;

                        await this.Navigation.PushAsync(new CaseList("caseAssgnTM", userAction, ""));
                    }
                }
            }
            catch (Exception)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        public async void SearchAgain(string Username)
        {
            try
            {
                project = new List<string>();

                var otheruserApicall = CasesAPIMethods.GetEmployeesBySearch(Username);
                var responseData = otheruserApicall.GetValue("ResponseContent");

                List<UserDataCall> d = new List<UserDataCall>();

                if (responseData != null && responseData.ToString() != "[]")
                {
                    UserDataCall userdata;

                    foreach (var item in responseData)
                    {
                        userdata = JsonConvert.DeserializeObject<UserDataCall>(item.ToString());
                        d.Add(userdata);
                        project.Add(userdata.DisplayName);
                    }
                }

                var count1 = project.Count();
                var options1 = new List<string>(project.Take(count1 + 1));
                options1.Add("Search Again...");
                var userAction1 = await this.DisplayActionSheet(null, "Cancel", null, options1.ToArray());
                if (userAction1 == "Search Again...")
                {
                    var result = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                         "Please search for a user to show the cases assigned to.").SetInputMode(
                         InputType.Name).SetOkText("Find"));

                    if (result.Ok)
                    {
                        SearchAgain(result.Text);
                    }
                }
                else if (userAction1 != "Cancel")
                {
                    var act = d.Where(v => v.DisplayName != null ? v.DisplayName.ToLower() == userAction1.ToLower() : v.UserID.ToLower() == userAction1.ToLower())?.FirstOrDefault().UserID;

                    await this.Navigation.PushAsync(new CaseList("caseAssgnSAM", act, ""));
                }
            }
            catch (Exception)
            {

            }
        }

        async void MyButton_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var applicationtype = button.BindingContext as ApplicationList;
            if (applicationtype.ApplicationName == "Cases")
            {
                string[] buttons;
                if (Functions.HasTeam)
                    buttons = new string[] { "Assigned to Me", "Created by Me", "Owned by Me", "Assigned to My team", "My Hoppers" };
                else
                    buttons = new string[] { "Assigned to Me", "Created by Me", "Owned by Me", "My Hoppers" };


                var action = await DisplayActionSheet("Select Option", "Cancel", null, buttons);

                switch (action)
                {
                    case "Assigned to Me":
                        await this.Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));

                        break;

                    case "Created by Me":
                        await this.Navigation.PushAsync(new CaseList("caseCreateBySAM", Functions.UserName, ""));
                        break;

                    case "Owned by Me":
                        await this.Navigation.PushAsync(new CaseList("caseOwnerSAM", Functions.UserName, ""));
                        break;

                    case "Assigned to My team":
                        await SelectUser(Functions.UserName);
                        break;

                    case "My Hoppers":
                        await this.Navigation.PushAsync(new SelectCaseHopper(Functions.UserName, "MyHopper"));
                        break;

                    default:
                        break;

                }
            }
            else if (applicationtype.ApplicationName == "Entities")
            {
                var action = await DisplayActionSheet("Select Option", "Cancel", null, "Assigned to Me");
                switch (action)
                {
                    case "Assigned to Me":
                        await Navigation.PushAsync(new MyAssociations("Assigned to Me", Functions.UserName));
                        break;


                    default:
                        break;

                }
            }
            else if (applicationtype.ApplicationName == "Departments")
            {
                string[] buttons;

                if (Functions.HasTeam)
                    buttons = new string[] { "My Team", "My Profile" };
                else
                    buttons = new string[] { "My Profile" };

                var action = await DisplayActionSheet("Select Option", "Cancel", null, buttons);
                switch (action)
                {
                    case "My Team":

                        await this.Navigation.PushAsync(new DirectReports(Functions.UserName));
                        break;

                    case "My Profile":
                        await this.Navigation.PushAsync(new UserDetail(Functions.UserName));

                        break;

                    default:
                        break;

                }

            }
            else if (applicationtype.ApplicationName == "Standards")
            {
                var action = await DisplayActionSheet("Select Option", "Cancel", null, "My Book", "My Publications");
                switch (action)
                {
                    case "My Book":
                        await this.Navigation.PushAsync(new MyBookListPage());
                        break;

                    case "My Publications":
                        await this.Navigation.PushAsync(new MyPublicationBookListPage());
                        break;

                    default:
                        break;
                }
            }
            else if (applicationtype.ApplicationName == "Quest")
            {
                var action = await DisplayActionSheet("Select Option", "Cancel", null, "My Open Items", "My Completed Items");
                switch (action)
                {
                    case "My Open Items":
                        await this.Navigation.PushAsync(new MyFormsPage("Open", "My Open Items"));
                        break;

                    case "My Completed Items":
                        await this.Navigation.PushAsync(new MyFormsPage("Completed", "My Completed Items"));
                        break;

                    default:
                        break;

                }
            }
        }
        async void MoreClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var applicationtype = button.BindingContext as ApplicationList;
            if (applicationtype.ApplicationName == "Cases")
            {

                var action = await DisplayActionSheet("Select Option", "Cancel", null, "Cases Home", "Cases Origination Center", "Hopper Center");
                switch (action)
                {
                    case "Cases Home":
                        await this.Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));
                        break;

                    case "Cases Origination Center":
                        await this.Navigation.PushAsync(new CaseOriginationCenter());
                        break;

                    case "Hopper Center":
                        await this.Navigation.PushAsync(new SelectCaseHopper(Functions.UserName, ""));
                        break;

                    default:
                        break;

                }
            }
            else if (applicationtype.ApplicationName == "Entities")
            {
                var action = await DisplayActionSheet("Select Option", "Cancel", null, "Entities Home", "Entities Origination Center");
                switch (action)
                {
                    case "Entities Home":
                        await this.Navigation.PushAsync(new MyAssociations("Entities Home", Functions.UserName));
                        break;

                    case "Entities Origination Center":
                        IsCreateEntity = false;
                        await this.Navigation.PushAsync(new Entity_CategoryType());
                        break;
                    default:
                        break;

                }

            }
            else if (applicationtype.ApplicationName == "Departments")
            {
                //var action = await DisplayActionSheet("Select Option", "Cancel", null, "Departments Home");
                //switch (action)
                //{
                //    case "Departments Home":

                //        break;



                //    default:
                //        break;

                //}

            }
            else if (applicationtype.ApplicationName == "Standards")
            {
                var action = await DisplayActionSheet("Select Option", "Cancel", null, "Standards Home", "Popular Standards", "Recently Updated");
                switch (action)
                {
                    case "Standards Home":
                        await Navigation.PushAsync(new StandardsBookPage(""));
                        break;
                    case "Popular Standards":
                        await Navigation.PushAsync(new StandardsBookPage("MostPopular"));
                        break;
                    case "Recently Updated":
                        await Navigation.PushAsync(new StandardsBookPage("Whatsnew"));
                        break;


                    default:
                        break;

                }
            }
            else if (applicationtype.ApplicationName == "Quest")
            {
                var action = await DisplayActionSheet("Select Option", "Cancel", null, "Quests Home");
                switch (action)
                {
                    case "Quests Home":
                        await this.Navigation.PushAsync(new MyFormsPage("Open", "Quest Home"));
                        break;

                    default:
                        break;

                }
            }
        }

        private void img_profile_OnTap(View arg1, object arg2)
        {
            profile_clicked(null, null);
        }

        async void Setting_Clicked(object sender, System.EventArgs e)
        {
            var sb = new StringBuilder();
            sb.Append("Switch to ");
            if (App.Isonline)
                sb.Append("Offline Mode");
            else
                sb.Append("Online Mode");

            try
            {

                var action = await DisplayActionSheet("Select Option", "Cancel", sb.ToString(), "Run Synchronization", "Setting", "About", "Logout");

                if (action.ToLower().Contains("offline"))
                    action = "offline";
                else if (action.ToLower().Contains("online"))
                    action = "online";

                switch (action)
                {
                    case "Logout":
                        App.Logout(this);
                        break;
                    case "About":
                        DisplayAlert("App Info", Functions.Appinfomsg, "Ok");
                        break;
                    case "Setting":
                        await Navigation.PushAsync(new Settings());
                        break;

                    case "offline"://Switch to offline
                        App.SetForceFullOnLineOffline(false);
                        App.SetConnectionFlag();
                        break;
                    case "online"://Switch to online
                        App.SetForceFullOnLineOffline(true);
                        App.SetConnectionFlag();
                        break;

                    case "Run Synchronization":
                        if (Functions.CheckInternetWithAlert())
                        {
                            App.isFirstcall = true;
                            App.OnlineSyncRecord();
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        async void cases_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var applicationtype = button.BindingContext as ApplicationList;
            if (applicationtype.ApplicationName == "Cases")
            {
                await this.Navigation.PushAsync(new SelectCaseType());
            }
            else if (applicationtype.ApplicationName == "Entities")
            {
                IsCreateEntity = false;
                await this.Navigation.PushAsync(new Entity_CategoryType());
            }
            else if (applicationtype.ApplicationName == "Departments")
            {
                await Navigation.PushAsync(new UserSearch());
            }
            else if (applicationtype.ApplicationName == "Standards")
            {
                await Navigation.PushAsync(new StandardsBookPage(""));
            }
            else if (applicationtype.ApplicationName == "Quest")
            {
                await Navigation.PushAsync(new SelectQuestArea());
            }

        }

        async void MyCases_Clicked(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));
        }

        async void MyForm_Clicked(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new MyFormsPage("Open", "MyForms"));
        }

        async void MyStandard_Clicked(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new My_StandardPage());
        }

        async void MyAssociation_Clicked(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new MyAssociations("My Association", Functions.UserName));
        }

        async void MyTeam_Clicked(object sender, System.EventArgs e)
        {
            if (Functions.HasTeam)
                await this.Navigation.PushAsync(new DirectReports(Functions.UserName));
        }

        async void NewCaseClicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var applicationtype = button.BindingContext as ApplicationList;
            if (applicationtype.ApplicationName == "Cases")
            {
                await this.Navigation.PushAsync(new SelectCaseType());
            }
            else if (applicationtype.ApplicationName == "Entities")
            {
                IsCreateEntity = true;
                await this.Navigation.PushAsync(new Entity_CategoryType());
            }
            else if (applicationtype.ApplicationName == "Departments")
            {
                button.IsEnabled = false;
            }
            else if (applicationtype.ApplicationName == "Standards")
            {
                button.IsEnabled = false;
            }
            else if (applicationtype.ApplicationName == "Quest")
            {
                await this.Navigation.PushAsync(new SelectQuestArea());
            }
        }

        public static bool IsCreateEntity = false;

        async void Search_Clicked(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var applicationtype = button.BindingContext as ApplicationList;
            string SearchSystem = string.Empty;

            if (applicationtype.ApplicationName == "Cases")
            {
                SearchSystem = "Search Cases";
            }
            else if (applicationtype.ApplicationName == "Entities")
            {
                SearchSystem = "Search Entities";
            }
            else if (applicationtype.ApplicationName == "Departments")
            {
                SearchSystem = "Search Employee";
            }
            else if (applicationtype.ApplicationName == "Standards")
            {
                SearchSystem = "Search Standards";
            }
            else if (applicationtype.ApplicationName == "Quest")
            {
                SearchSystem = "Search Quest";
            }


            if (SearchSystem == "Search Standards")
            {
                await this.Navigation.PushAsync(new StandardSearch());
            }
            else if (SearchSystem == "Search Employee")
            {
                await this.Navigation.PushAsync(new UserSearch());
            }
            else if (!string.IsNullOrEmpty(SearchSystem))
                await this.Navigation.PushAsync(new SearchPage(SearchSystem));

            Applicationlist.SelectedItem = null;
        }

        async void profile_clicked(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new UserDetail(Functions.UserName));
        }

        private void Applicationlist_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            Applicationlist.SelectedItem = null;
        }

        private void Applicationlist_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Applicationlist.SelectedItem = null;
        }
    }
}