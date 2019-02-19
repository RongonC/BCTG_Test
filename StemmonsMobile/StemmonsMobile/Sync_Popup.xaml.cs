using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using DataServiceBus.OnlineHelper.DataTypes;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using StemmonsMobile.Commonfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sync_Popup : PopupPage
    {
        bool IsClosed = false;
        public Sync_Popup()
        {
            InitializeComponent();
            //progress();
        }

        public async void progress()
        {
            if (Functions.AppStartCount <= 1)
            {
                btn_close.IsVisible = false;
            }
            else
            {
                btn_close.IsVisible = true;
            }
            //btn_close.IsVisible = true;
            int itemCount = 12;
            int Percentage = 100 / 13;
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {

                    #region Get item counts operation
                    //Home Count Sync
                    if (!IsClosed)
                    {
                        try
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Sync_ProgressBar.Progress = 0.04;
                                lbl_Percentage.Text = "5%";
                                lbl_status.Text = "Get item counts operation " + 1 + " of " + itemCount;
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
                                    Functions.ShowtoastAlert("Item counts operation Sync Success.");
                                else
                                    Functions.ShowtoastAlert("Item counts operation Sync having issue.");
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
                                Sync_ProgressBar.Progress = 0.08;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 1) + "%";
                                lbl_status.Text = "Get item counts operation " + 1 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Cases Origination Center Sync Operation " + 2 + " of " + itemCount;
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
                                Sync_ProgressBar.Progress = 0.16;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 2) + "%";
                                lbl_status.Text = "Cases Origination Center Sync Operation " + 2 + " of " + itemCount + " completed";
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }
                    #endregion

                    #region Cases Sync
                    //Cases Sync
                    if (!IsClosed)
                    {
                        try
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                //Sync_ProgressBar.Progress = 0.16;
                                //lbl_Percentage.Text = Convert.ToString(Percentage * 2) + "%";
                                lbl_status.Text = "Case Types and Items Sync Operation " + 3 + " of " + itemCount;
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
                                Sync_ProgressBar.Progress = 0.24;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 3) + "%";
                                lbl_status.Text = "Case Types and Items Sync Operation " + 3 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Get CaseList Assigned To Me List Sync Operation " + 4 + " of " + itemCount;
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
                                string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", "", Functions.UserName, "", "", "", "", "0", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_AssignedToMe");

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
                                Sync_ProgressBar.Progress = 0.32;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 4) + "%";
                                lbl_status.Text = "Get CaseList Assigned To Me List Sync Operation " + 4 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Get CaseList Created By Me List Sync Operation " + 5 + " of " + itemCount;
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

                                string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", "", "", "", Functions.UserName, "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_CreatedByMe");

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
                                Sync_ProgressBar.Progress = 0.40;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 5) + "%";
                                lbl_status.Text = "Get CaseList Created By Me List Sync Operation " + 5 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Get CaseList Owned By Me List Sync Operation " + 6 + " of " + itemCount;
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
                                string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", Functions.UserName, "", "", "", "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_OwnedByMe");

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
                                Sync_ProgressBar.Progress = 0.48;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 6) + "%";
                                lbl_status.Text = "Get CaseList Owned By Me List Sync Operation " + 6 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Get CaseList Assigned To My Team List Sync Operation " + 7 + " of " + itemCount;
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
                                    string str = CasesSyncAPIMethods.GetCaseListSync(App.Isonline, Functions.UserName, "", Functions.UserName, "", "", "", "", "", "", new char(), new char(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Functions.UserFullName, true, "_AssignedToMyTeam");

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
                                Sync_ProgressBar.Progress = 0.56;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 7) + "%";
                                lbl_status.Text = "Get CaseList Assigned To My Team List Sync Operation " + 7 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Entity Types and Items Sync Operation " + 8 + " of " + itemCount;
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
                                Sync_ProgressBar.Progress = 0.64;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 8) + "%";
                                lbl_status.Text = "Entity Types and Items Sync Operation " + 8 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Get Entity Associated List Sync Operation " + 9 + " of " + itemCount;
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
                                Sync_ProgressBar.Progress = 0.72;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 9) + "%";
                                lbl_status.Text = "Get Entity Associated List Sync Operation " + 9 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Quest Area and Item List Sync Operation " + 10 + " of " + itemCount;
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
                                Sync_ProgressBar.Progress = 0.80;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 10) + "%";
                                lbl_status.Text = "Quest Area and Item List Sync Operation " + 10 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Standard Application Data Sync Operation " + 11 + " of " + itemCount;
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
                                Sync_ProgressBar.Progress = 0.88;
                                lbl_Percentage.Text = Convert.ToString(Percentage * 11) + "%";
                                lbl_status.Text = "Standard Application Data Sync Operation " + 11 + " of " + itemCount + " completed";
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
                                lbl_status.Text = "Employee Data Sync Operation " + 12 + " of " + itemCount;
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }

                    try
                    {
                        await Task.Run(async () =>
                        {
                            try
                            {

                                await CasesSyncAPIMethods.GetTeamMembers(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

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
                                Sync_ProgressBar.Progress = 1;
                                lbl_Percentage.Text = "100%";
                                lbl_status.Text = "Employee Data Sync Operation " + 12 + " of " + itemCount + " completed";
                            });
                        }
                        catch (Exception)
                        {
                        }
                    }
                    #endregion

                }
                await PopupNavigation.Instance.PopAsync();
                Functions.AppStartCount = Functions.AppStartCount + 1;
                Application.Current.Properties["AppStartCount"] = Functions.AppStartCount;
            }
            catch (Exception ex)
            {
                Application.Current.Properties["AppStartCount"] = 1;
                Functions.AppStartCount = 1;
            }
        }
        private async void OnBackGround_Click(object sender, EventArgs e)
        {
            IsClosed = true;
            await PopupNavigation.Instance.PopAsync();
        }
    }
}