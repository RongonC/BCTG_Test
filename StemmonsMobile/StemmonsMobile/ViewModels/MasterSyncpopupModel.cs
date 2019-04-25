using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using DataServiceBus.OnlineHelper.DataTypes;
using Plugin.Connectivity;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels
{
    class MasterSyncpopupModel : INotifyPropertyChanged
    {
        public static bool Is_Popup_Open = false;
        public static bool IsClosed = false;
        private string title1;
        public string Title1
        {
            get => title1;
            set
            {
                if (title1 != value)
                {
                    title1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title1"));
                }
            }
        }

        private string title2;
        public string Title2
        {
            get => title2;
            set
            {
                if (title2 != value)
                {
                    title2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title2"));
                }
            }
        }

        private string percentageValue;
        public string PercentageValue
        {
            get => percentageValue;
            set
            {
                if (percentageValue != value)
                {
                    percentageValue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PercentageValue"));
                }
            }
        }

        private double progressValue;
        public double ProgressValue
        {
            get => progressValue;
            set
            {
                if (progressValue != value)
                {
                    progressValue = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProgressValue"));
                }
            }
        }

        private bool btnVisibility;
        public bool BtnVisibility
        {
            get => btnVisibility;
            set
            {
                //if (btnVisibility != value)
                {
                    btnVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BtnVisibility"));
                }
            }
        }

        private bool grdVisibility;
        public bool GrdVisibility
        {
            get => grdVisibility;
            set
            {
                //if (grdVisibility != value)
                {
                    grdVisibility = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GrdVisibility"));
                }
            }
        }

        public MasterSyncpopupModel(ObservableCollection<SyncStatus> lst)
        {
            progress1(lst);
        }

        public async void progress1(ObservableCollection<SyncStatus> lst)
        {
            if (Functions.AppStartCount <= 1)
            {
                BtnVisibility = false;

            }
            else
            {
                BtnVisibility = true;
            }
            GrdVisibility = true;
            int itemCount = 12;
            int Percentage = 100 / lst.Count + 1;
            try
            {
                 //Functions.UserName = "guillermon";
                if (CrossConnectivity.Current.IsConnected)
                {
                   // Task.Run(() =>
                   // {
                    //    Functions.GetSystemCodesfromSqlServer();
                   // });


                    for (int i = 0; i < lst.Count; i++)
                    {
                        switch (lst[i].APIName)
                        {

                            #region Home Count Sync
                            case "Home Count":
                                //Home Count Sync
                                try
                                {
                                    SyncPopupRunTimeChange(0.04, "5%", "Origination Center Data Sync", " Operation " + 1 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = HomeOffline.GetAllHomeCount(Functions.UserName, Functions.Selected_Instance, App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[0] = 1;
                                                Functions.ShowtoastAlert("Origination Center Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Origination Center Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                    //HomePageCount();
                                    SyncPopupRunTimeChange(0.08, Convert.ToString(Percentage * 1) + "%", "Origination Center Sync", "Operation " + 1 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region Cases Origination Sync
                            case "Cases Origination":
                                //Cases Originaation Sync
                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Cases Origination Center Sync", "Operation " + 2 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var str = CasesSyncAPIMethods.GetOriginationCenterForUserSync(Functions.UserName, "Y", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

                                            Functions.lstSyncAPIStatus[i].FailDesc = str.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = str.ApiCallSuccess;

                                            if (str.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[1] = 1;
                                                Functions.ShowtoastAlert("Case Types Sync Operation Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Case Types Sync Operation failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                    SyncPopupRunTimeChange(0.16, Convert.ToString(Percentage * 2) + "%", "Cases Origination Center Sync", "Operation " + 2 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }

                                break;
                            #endregion

                            #region Cases Sync
                            case "Cases Sync":
                                //Cases Sync

                                try
                                {

                                    SyncPopupRunTimeChangeTitle("Case Types and Items Sync", "Operation " + 3 + " of " + itemCount + "");

                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = CasesSyncAPIMethods.GetAllCaseTypeWithID(null, Functions.UserName, "Frequent", App.DBPath);

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[2] = 1;
                                                Functions.ShowtoastAlert("Cases Items Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Cases Items Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    });
                                    SyncPopupRunTimeChange(0.24, Convert.ToString(Percentage * 3) + "%", "Case Types and Items Sync", "Operation " + 3 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }

                                break;
                            #endregion

                            #region CaseList Assigned To Me
                            case "CaseList Assigned To Me":
                                //Get CaseList Assigned To Me
                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Get CaseList Assigned To Me List Sync", "Operation " + 4 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", "", Functions.UserName, "", "", "", "", "0", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_AssignedToMe");

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                Functions.ShowtoastAlert("Get CaseList Assigned To Me List Sync Success.");
                                                App.SyncSuccessFlagArr[3] = 1;
                                            }
                                            else
                                                Functions.ShowtoastAlert("Get CaseList Assigned To Me List Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                    SyncPopupRunTimeChange(0.32, Convert.ToString(Percentage * 4) + "%", "Get CaseList Assigned To Me List Sync", "Operation " + 4 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region CaseList Created By Me
                            case "CaseList Created By Me":
                                //Get CaseList Created By Me

                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Get CaseList Created By Me List Sync", "Operation " + 5 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {

                                            var SyncST = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", "", "", "", Functions.UserName, "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_CreatedByMe");

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[4] = 1;
                                                Functions.ShowtoastAlert("Get CaseList Created By Me List Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Get CaseList Created By Me List Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                    SyncPopupRunTimeChange(0.40, Convert.ToString(Percentage * 5) + "%", "Get CaseList Created By Me List Sync", "Operation " + 5 + " of " + itemCount + " completed");

                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region CaseList Owned By Me
                            case "CaseList Owned By Me":

                                //Get CaseList Owned By Me

                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Get CaseList Owned By Me List Sync", "Operation " + 6 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", Functions.UserName, "", "", "", "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_OwnedByMe");

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[5] = 1;
                                                Functions.ShowtoastAlert("Get CaseList Owned By Me List Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Get CaseList Owned By Me List Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });

                                    SyncPopupRunTimeChange(0.48, Convert.ToString(Percentage * 6) + "%", "Get CaseList Owned By Me List Sync", "Operation " + 6 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region CaseList Assigned To My Team
                            case "CaseList Assigned To My Team":

                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Get CaseList Assigned To My Team List Sync", "Operation " + 7 + " of " + itemCount + "");
                                    if (Functions.HasTeam)
                                    {
                                        await Task.Run(() =>
                                        {
                                            try
                                            {
                                                var SyncST = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", Functions.UserName, "", "", "", "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_AssignedToMyTeam");

                                                Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                                Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                                if (SyncST.ApiCallSuccess)
                                                {
                                                    App.SyncSuccessFlagArr[6] = 1;
                                                    Functions.ShowtoastAlert("Get CaseList Assigned To My Team List Sync Success.");
                                                }
                                                else
                                                    Functions.ShowtoastAlert("Get CaseList Assigned To My Team List Sync failed.");
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        });
                                    }
                                    else
                                    {
                                        Functions.lstSyncAPIStatus[i].FailDesc = "No Team to bind Data";
                                        Functions.lstSyncAPIStatus[i].ApiCallSuccess = false;

                                        DefaultAPIMethod.AddLog("No Team to bind Data", "Y", "GetCaseList_AssignedToMyTeam", Functions.UserName, DateTime.Now.ToString());
                                        Functions.ShowtoastAlert("No Team to bind Data for your team.");
                                    }
                                    SyncPopupRunTimeChange(0.56, Convert.ToString(Percentage * 7) + "%", "Get CaseList Assigned To My Team List Sync", "Operation " + 7 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }

                                break;
                            #endregion

                            #region Entity Sync
                            case "Entity Sync":
                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Entity Types and Items Sync", "Operation " + 8 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = EntitySyncAPIMethods.GetAllEntityTypeWithID(null, Functions.UserName, App.DBPath);

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[7] = 1;
                                                Functions.ShowtoastAlert("Entity Types and Items Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Entity Types and Items Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                    SyncPopupRunTimeChange(0.64, Convert.ToString(Percentage * 8) + "%", "Entity Types and Items Sync", "Operation " + 8 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region Entity associated with Me
                            case "Entity Associated With Me":
                                //Get CaseList Owned By Me

                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Get Entity Associated List Sync", "Operation " + 9 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {

                                            var SyncST = EntitySyncAPIMethods.GetAssociatedEntityList(Functions.UserName, App.DBPath);

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[8] = 1;
                                                Functions.ShowtoastAlert("Get Entity Associated List Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Get Entity Associated List Sync failed.");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });

                                    SyncPopupRunTimeChange(0.72, Convert.ToString(Percentage * 9) + "%", "Get Entity Associated List Sync", "Operation " + 9 + " of " + itemCount + " completed");
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region Quest Sync
                            case "Quest Sync":
                                //Quest Sync
                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Quest Area and Item List Sync", "Operation " + 10 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = QuestSyncAPIMethods.GetAllQuest(null, Functions.UserName, App.DBPath);


                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[9] = 1;
                                                Functions.ShowtoastAlert("Quest Area and Item List Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Quest Area and Item List Sync failed.");
                                            SyncPopupRunTimeChange(0.80, Convert.ToString(Percentage * 10) + "%", "Quest Area and Item List Sync", "Operation " + 10 + " of " + itemCount + " completed");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region Standard Sync
                            case "Standard Sync":

                                //Standard Sync

                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Standard Application Data Sync", "Operation " + 11 + " of " + itemCount + "");
                                    await Task.Run(() =>
                                    {
                                        try
                                        {
                                            var SyncST = StandardsSyncAPIMethods.GetAllStandards(Functions.UserName, App.DBPath);

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                App.SyncSuccessFlagArr[10] = 1;
                                                Functions.ShowtoastAlert("Standard Application Data Sync Success.");
                                            }
                                            else
                                                Functions.ShowtoastAlert("Standard Application Data Sync failed.");
                                            SyncPopupRunTimeChange(0.88, Convert.ToString(Percentage * 11) + "%", "Standard Application Data Sync", "Operation " + 11 + " of " + itemCount + " completed");

                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            #endregion

                            #region Employee Search Sync
                            case "Employee Search Sync":
                                //Employee Search Sync
                                try
                                {
                                    SyncPopupRunTimeChangeTitle("Employee Data Sync", "Operation " + 12 + " of " + itemCount + "");
                                    await Task.Run(async () =>
                                    {
                                        try
                                        {

                                            await CasesSyncAPIMethods.GetTeamMembers(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

                                            var SyncST = CasesSyncAPIMethods.GetAllEmployeeUser(App.DBPath, Functions.UserName);

                                            Functions.lstSyncAPIStatus[i].FailDesc = SyncST.FailDesc;
                                            Functions.lstSyncAPIStatus[i].ApiCallSuccess = SyncST.ApiCallSuccess;

                                            if (SyncST.ApiCallSuccess)
                                            {
                                                Functions.ShowtoastAlert("Employee Data Sync Success.");
                                                App.SyncSuccessFlagArr[11] = 1;
                                            }
                                            else
                                            {
                                                Functions.ShowtoastAlert("Employee Data Sync failed.");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    });
                                    SyncPopupRunTimeChange(1, "100%", "Employee Data Sync", "Operation " + 12 + " of " + itemCount + " completed");
                                }
                                catch (Exception ex)
                                {
                                }
                                break;
                            #endregion
                            default:
                                break;
                        }
                    }
                    App.SyncProgressFlag = true;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    Functions.AppStartCount = Functions.AppStartCount + 1;
                    Application.Current.Properties["AppStartCount"] = Functions.AppStartCount;
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.Properties["AppStartCount"] = 1;
                    Functions.AppStartCount = 1;
                });
            }
        }
        private void SyncPopupRunTimeChange(double prog, string per, string Title, string Count)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ProgressValue = prog;
                PercentageValue = per;
                Title1 = Title;
                Title2 = Count;
                BtnVisibility = BtnVisibility;
                GrdVisibility = GrdVisibility;
            });
        }
        private void SyncPopupRunTimeChangeTitle(string Title, string Count)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                ProgressValue = ProgressValue;
                PercentageValue = PercentageValue;
                Title1 = Title;
                Title2 = Count;
                BtnVisibility = BtnVisibility;
                GrdVisibility = GrdVisibility;
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
