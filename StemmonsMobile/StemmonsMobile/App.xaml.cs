using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;
using StemmonsMobile.Commonfiles;
using Plugin.Connectivity;
using System;
using DataServiceBus.OfflineHelper.DataTypes;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using System.Linq;
using System.Globalization;
using System.Text;
using Acr.UserDialogs;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using StemmonsMobile.Views.Cases;

namespace StemmonsMobile
{
    public partial class App : Application
    {
        public static bool UseMockDataStore = true;
        public static string BackendUrl = "https://localhost:5000";
        public static string localfolderpath = "";
        public static string DBPath = "";

        public static bool Isonline = false;
        public static bool IsLoginCall = false;

        public static bool IsForceOnline = true;
        public App()
        {
            InitializeComponent();

            GetAppLocalData();

            //MainPage = new ViewcasePage_New();

            //return;

            if (Functions.IsPWDRemember && Functions.IsLogin)
            {
                InstanceList inta = new InstanceList();
                inta.InstanceUrl = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl;
                inta.InstanceID = Functions.Selected_Instance;
                inta.InstanceName = Functions.InstanceName;
                MainPage = new NavigationPage(new SelectInstancePage())
                {
                    BarTextColor = Color.White,
                    BarBackgroundColor = Color.FromHex("696969"),
                };
            }
            else
            {
                MainPage = new NavigationPage(new SelectInstancePage())
                {
                    BarBackgroundColor = Color.FromHex("696969"),
                    BindingContext = new InstanceList(),
                    BarTextColor = Color.White,
                };
            }
            SetConnectionFlag();

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }
        private void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            try
            {
                Isonline = e.IsConnected;
                isFirstcall = true;
                OnlineSyncRecord();
            }
            catch (Exception)
            {
            }
        }
        public static bool isFirstcall = true;
        public static void OnlineSyncRecord()
        {
            if (Isonline && isFirstcall)
            {
                isFirstcall = false;
                var list = DBHelper.GetItemTranInfoListList(DBPath);
                list.Wait();
                int QueueCount = list.Result.Where(v => v.PROCESS_ID == 0).Count();
                if (QueueCount > 0)
                {
                    IProgressDialog dlg = UserDialogs.Instance.Progress(new ProgressDialogConfig());
                    try
                    {
                        ProgressDialogConfig config = new ProgressDialogConfig();

                        try
                        {
                            int MainQueueCount = QueueCount;

                            config = new ProgressDialogConfig().SetTitle("Please Wait...")
                                                                 // setting false will just create a spinner
                                                                 .SetIsDeterministic(true)
                                                                 // prevents user from clicking background
                                                                 .SetMaskType(MaskType.Black)
                            // cancel text and action
                            .SetCancel("Run in Background", () => { dlg.Hide(); });

                            dlg.Show();

                            Task.Run(() =>
                            {
                                HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);
                            });

                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                dlg.PercentComplete = Convert.ToInt32(Math.Round(Convert.ToDecimal((QueueCount / MainQueueCount) * 100)));
                                dlg.Title = "Sync Operation " + 1 + " of " + MainQueueCount + " in Progress";
                            });

                            for (int i = 0; i < MainQueueCount; i++)
                            {
                                if (QueueCount > MainQueueCount)
                                    break;

                                var list1 = DBHelper.GetItemTranInfoListByID(list.Result[i].ITEM_TRAN_INFO_ID, DBPath);
                                list1.Wait();
                                int PROCESS_ID = list1.Result.PROCESS_ID;

                                if (PROCESS_ID > 0)
                                {
                                    QueueCount = QueueCount - 1;
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        dlg.PercentComplete = Convert.ToInt32(Math.Round(Convert.ToDecimal((QueueCount / MainQueueCount) * 100)));
                                        dlg.Title = "Sync Operation " + (MainQueueCount - QueueCount) + " of " + MainQueueCount + " completed";
                                    });
                                }
                                else
                                {
                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        dlg.PercentComplete = Convert.ToInt32(Math.Round(Convert.ToDecimal((QueueCount / MainQueueCount) * 100)));
                                        dlg.Title = "Sync Operation " + i + " of " + MainQueueCount + " in Progress";
                                    });
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                            {
                                dlg.Hide();
                            });
                        }
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            dlg.Hide();
                        });
                    }
                    catch (Exception)
                    {
                        Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        {
                            dlg.Hide();
                        });
                    }
                    Functions.ShowtoastAlert("Online Data Sync Operation Success!");
                }
                else
                {
                    Functions.ShowtoastAlert("No data avilable for sync.");
                }
            }
        }

        public void CreateDataBase()
        {
            try
            {
                DBPath = DependencyService.Get<IDatalayer>().GetLocalFilePath("StemmonsMobile.db");

                //Required To create Database
                DBHelper dh = new DBHelper(DependencyService.Get<IDatalayer>().GetLocalFilePath("StemmonsMobile.db"));
            }
            catch (System.Exception e)
            {

            }
        }

        public void GetAppLocalData()
        {
            Task.Run(() => { Getlocl(); }).Wait();
        }
        public void Getlocl()
        {
            try
            {
                if (Application.Current.Properties.ContainsKey("IsLogin"))
                {
                    if (Application.Current.Properties["IsLogin"] != null)
                        Functions.IsLogin = (bool)Application.Current.Properties["IsLogin"];
                    else
                        Functions.IsLogin = false;
                }
                else
                    Functions.IsLogin = false;




                if (Application.Current.Properties.ContainsKey("UserName"))
                    Functions.UserName = Convert.ToString(Application.Current.Properties["UserName"]);

                if (Application.Current.Properties.ContainsKey("UserFullName"))
                    Functions.UserFullName = Convert.ToString(Application.Current.Properties["UserFullName"]);

                if (Application.Current.Properties.ContainsKey("IsLogin"))
                {
                    if (Application.Current.Properties["IsLogin"] != null)
                        Functions.IsLogin = (bool)Application.Current.Properties["IsLogin"];
                    else
                        Functions.IsLogin = false;
                }
                else
                    Functions.IsLogin = false;

                if (Application.Current.Properties.ContainsKey("HasTeam"))
                {
                    if (Application.Current.Properties["HasTeam"] != null)
                        Functions.HasTeam = (bool)Application.Current.Properties["HasTeam"];
                    else
                        Functions.HasTeam = false;
                }
                else
                    Functions.HasTeam = false;

                if (Application.Current.Properties.ContainsKey("IsPWDRemember"))
                {
                    if (Application.Current.Properties["IsPWDRemember"] != null)
                        Functions.IsPWDRemember = (bool)Application.Current.Properties["IsPWDRemember"];
                    else
                        Functions.IsPWDRemember = false;
                }

                if (Application.Current.Properties.ContainsKey("UserLoginName"))
                    Functions.UserLoginName = Convert.ToString(Application.Current.Properties["UserLoginName"]);

                if (Application.Current.Properties.ContainsKey("UserPassword"))
                    Functions.UserPassword = Convert.ToString(Application.Current.Properties["UserPassword"]);

                if (Application.Current.Properties.ContainsKey("Baseurl"))
                    DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl = Convert.ToString(Application.Current.Properties["Baseurl"]);



                if (Current.Properties.ContainsKey("AppStartCount"))
                {
                    if (Application.Current.Properties["AppStartCount"] != null)
                        Functions.AppStartCount = (int)Application.Current.Properties["AppStartCount"];
                    else
                        Functions.AppStartCount = 1;
                }
                else
                    Functions.AppStartCount = 1;



                if (Current.Properties.ContainsKey("Selected_Instance"))
                {
                    if (Application.Current.Properties["Selected_Instance"] != null)
                        Functions.Selected_Instance = (int)Application.Current.Properties["Selected_Instance"];
                    else
                        Functions.Selected_Instance = 0;
                }
                else
                    Functions.Selected_Instance = 0;

                if (Application.Current.Properties.ContainsKey("InstanceName"))
                {
                    if (Application.Current.Properties["InstanceName"] != null)
                        Functions.InstanceName = (string)Application.Current.Properties["InstanceName"];
                    else
                        Functions.InstanceName = string.Empty;
                }
                else
                    Functions.InstanceName = string.Empty;

            }
            catch (Exception)
            {

            }
        }

        public static string EntityImgURL = string.Empty;
        public static string CasesImgURL = string.Empty;
        public static string StandardImgURL = string.Empty;

        protected async override void OnStart()
        {
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            CreateDataBase();
            Functions.Platformtype = Xamarin.Forms.Device.RuntimePlatform;

            //Crashes Report 
            AppCenter.Start("ios=2c8cf8f9-a000-49f8-9a5b-113cfa176e20;" +
                "uwp=a94fe04c-6909-4387-a1fe-f8ab422f8e71;" +
                "android=fca1ff00-e443-4444-9305-d79b3255abd0;", typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            // Handle when your app resumes
        }

        public static string DateFormatStringToString(string inputDate, string inputDateFormate = "MM/dd/yyyy", string outputDateFormate = "yyyy/MM/dd")
        {
            string sInput = inputDate;
            string sOutPut = string.Empty;
            try
            {
                //if (inputDate.IndexOf('-') > -1)
                //{
                //    try
                //    {
                //        inputDate = inputDate.Replace('-', '/');
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //}

                CultureInfo ci = new CultureInfo("en-US");
                var tdt = Convert.ToDateTime(inputDate);
                sOutPut = tdt.ToString("MM/dd/yyyy", ci);


                //inputDate = SeperatorValueReplace(inputDate);
                //sOutPut = DateTime.ParseExact(inputDate, inputDateFormate, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString(outputDateFormate);
            }
            catch (Exception ex)
            {
                return sInput;
            }
            return sOutPut;
        }

        public static string SeperatorValueReplace(string sInput)
        {
            try
            {
                char cSeperator = '/';
                if (sInput.IndexOf('/') > -1)
                {
                    cSeperator = '/';
                }
                else if (sInput.IndexOf("-") > -1)
                {
                    cSeperator = '-';
                }
                else if (sInput.IndexOf(".") > -1)
                {
                    cSeperator = '.';
                }
                string[] arrinput = sInput.Split(cSeperator);
                string[] arrOutPut = new string[arrinput.Length];


                for (int i = 0; i <= arrinput.Length - 1; i++)
                {
                    if (arrinput[i].Length == 1)
                    {
                        arrOutPut[i] = "0" + arrinput[i] + cSeperator;
                    }
                    else
                    {
                        if (i != arrinput.Length - 1)
                        {
                            arrOutPut[i] = arrinput[i] + cSeperator;
                        }
                        else
                        {
                            arrOutPut[i] = arrinput[i];
                        }
                    }
                }
                return String.Concat(arrOutPut);
            }
            catch (Exception)
            {
                return sInput;
            }

        }

        public static void SetConnectionFlag()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (IsForceOnline)
                {
                    //it should be always true for live Environment
                    Isonline = true;
                }
                else
                {
                    //it should be always false for live Environment
                    Isonline = false;
                }
            }
            else
            {
                //it should be always false for live Environment
                Isonline = false;
            }
        }

        public static void SetForceFullOnLineOffline(bool bIsForceOnline)
        {
            IsForceOnline = bIsForceOnline;
        }

        public static void GotoHome(Page Pg)
        {
            try
            {
                Functions.FromHomePage = false;
                var existingPages = Pg.Navigation.NavigationStack.ToList();
                Page abc = new LandingPage();
                if (existingPages.Count > 0)
                {
                    Pg.Navigation.InsertPageBefore(abc, Pg.Navigation.NavigationStack[0]);

                    for (int i = 0; i < existingPages.Count; i++)
                    {
                        Pg.Navigation.RemovePage(existingPages[i]);
                    }
                }
                else
                {
                    Pg.Navigation.PopAsync();
                }
            }
            catch (Exception)
            {
            }
        }

        public async static void BtnHumburger(Page pg)
        {
            try
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Switch to ");
                    if (App.Isonline)
                        sb.Append("Offline Mode");
                    else
                        sb.Append("Online Mode");

                    var Action = await pg.DisplayActionSheet(null, "Cancel", sb.ToString(), "Run Synchronization", "About", "Logout");

                    if (Action.ToLower().Contains("offline"))
                        Action = "offline";
                    else if (Action.ToLower().Contains("online"))
                        Action = "online";

                    switch (Action)
                    {
                        case "offline"://Switch to offline
                            App.SetForceFullOnLineOffline(false);
                            SetConnectionFlag();
                            break;
                        case "online"://Switch to online
                            App.SetForceFullOnLineOffline(true);
                            SetConnectionFlag();
                            break;
                        case "Run Synchronization":
                            if (Functions.CheckInternetWithAlert())
                            {
                                isFirstcall = true;
                                OnlineSyncRecord();
                            }
                            break;
                        case "About":
                            pg.DisplayAlert("App info", Functions.Appinfomsg, "Ok");
                            break;
                        case "Logout":
                            Logout(pg);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public async static void Logout(Page Pg)
        {
            try
            {
                await Task.Run(() => { Functions.ClearApplocalData(); });

                var existingPages = Pg.Navigation.NavigationStack.ToList();
                Page DEF = new SelectInstancePage();

                if (existingPages.Count > 0)
                {
                    Pg.Navigation.InsertPageBefore(DEF, Pg.Navigation.NavigationStack[0]);

                    for (int i = 0; i < existingPages.Count; i++)
                    {
                        Pg.Navigation.RemovePage(existingPages[i]);
                    }
                }
                else
                {
                    Pg.Navigation.PopAsync();
                }

                Functions.IsLogin = false;
                Application.Current.Properties["IsLogin"] = false;
            }
            catch (Exception ex)
            {
            }
        }

        public static void BackPageNavigation(string PagetoNavigate, Page pg)
        {
            try
            {
                var existingPages = pg.Navigation.NavigationStack.ToList();

                if (existingPages.Count > 0)
                {
                    int Counter = 0;
                    for (int i = 0; i < existingPages.Count; i++)
                    {
                        var typ = existingPages[i].GetType().Name;
                        if (typ == PagetoNavigate)
                        {
                            Counter = i;
                            break;
                        }
                    }

                    for (int i = Counter + 1; i <= existingPages.Count; i++)
                    {
                        var ph = existingPages[i];
                        pg.Navigation.RemovePage(ph);
                        var ta = pg.Navigation.NavigationStack.ToList();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void HomeMenu_Click(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(Application.Current.MainPage);
            }
            catch (Exception)
            {
            }
        }

        private void Navigation_Click(object sender, EventArgs e)
        {

        }
    }
}
