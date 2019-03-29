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
using Plugin.DeviceInfo;
using Xamarin.Essentials;
using DataServiceBus.OnlineHelper.DataTypes;
using System.IO;
using StemmonsMobile.CustomControls;
using Newtonsoft.Json;
using System.Collections.Generic;
using StemmonsMobile.DataTypes.DataType.Default;
using Newtonsoft.Json.Linq;

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

        public static bool IsPropertyPage = true;

        public static bool IsForceOnline = true;


        public static int[] SyncSuccessFlagArr = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static bool SyncProgressFlag = false;

        public App()
        {
            InitializeComponent();

            //MainPage = new ViewcasePage_New();

            //return;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;


            //if (Functions.IsPWDRemember && Functions.IsLogin)
            //{
            //    InstanceList inta = new InstanceList();
            //    inta.InstanceUrl = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl;
            //    inta.InstanceID = Functions.Selected_Instance;
            //    inta.InstanceName = Functions.InstanceName;
            //    MainPage = new NavigationPage(new SelectInstancePage())
            //    {
            //        BarTextColor = Color.White,
            //        BarBackgroundColor = Color.FromHex("696969"),
            //    };
            //}
            //else
            {
                MainPage = new NavigationPage(new SelectInstancePage())
                {
                    BarBackgroundColor = Color.FromHex("696969"),
                    BindingContext = new InstanceList(),
                    BarTextColor = Color.White,
                };
            }
            SetConnectionFlag();
            //CrossConnectivity.Current.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                Isonline = true;
                isFirstcall = true;
                OnlineSyncRecord();
            }
            else
            {
                Isonline = false;
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
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
        public async static void OnlineSyncRecord()
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

                            await Task.Run(() =>
                             {
                                 Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                 {
                                     dlg.PercentComplete = 25;
                                     dlg.Title = "Online Data Sync Operation in Progress";
                                 });
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
                if (!IsPropertyPage)
                {
                    DBPath = DependencyService.Get<IDatalayer>().GetLocalFilePath("StemmonsMobile.db");

                    //Required To create Database
                    DBHelper dh = new DBHelper(DependencyService.Get<IDatalayer>().GetLocalFilePath("StemmonsMobile.db"));
                }
                else
                {
                    DBPath = DependencyService.Get<IDatalayer>().GetLocalFilePath("BCTGMobile.db");

                    //Required To create Database
                    DBHelper dh = new DBHelper(DependencyService.Get<IDatalayer>().GetLocalFilePath("BCTGMobile.db"));
                }
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

                var res = DBHelper.GetinstanceuserassocListByUsername_Id(Functions.UserName, Functions.Selected_Instance, DBPath);
                res.Wait();
                if (res.Result != null)
                    ConstantsSync.INSTANCE_USER_ASSOC_ID = res.Result.INSTANCE_USER_ASSOC_ID;
            }
            catch (Exception)
            {

            }
        }

        public static string EntityImgURL = string.Empty;
        public static string CasesImgURL = string.Empty;
        public static string StandardImgURL = string.Empty;
        public static string CurretVer = string.Empty;

        protected override void OnStart()
        {
            CreateDataBase();
            Functions.Platformtype = Xamarin.Forms.Device.RuntimePlatform;
            GetAppLocalData();
            //Crashes Report 



            try
            {
                AppCenter.Start("6bdbd476-5df5-48d0-8e86-721448d10a42", typeof(Analytics), typeof(Crashes));
            }
            catch (Exception wd)
            {
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
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

                //CultureInfo ci = new CultureInfo("en-US");
                //var tdt = Convert.ToDateTime(inputDate);
                //sOutPut = tdt.ToString("MM/dd/yyyy", ci);
                DateTime Dout = new DateTime();
                DateTime.TryParse(inputDate, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Dout);

                sOutPut = Dout.Date.ToString();

                // DateTime dateTime16 = DateTime.ParseExact(inputDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                // string json = null;

                // DateTime dt2 = DateTime.ParseExact(inputDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);


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
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            //  if (CrossConnectivity.Current.IsConnected)
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

        public static void GotoHome(Page Pg = null)
        {
            try
            {
                Functions.FromHomePage = false;
                var existingPages = Application.Current.MainPage.Navigation.NavigationStack.ToList();
                Page abc = new Page();

                if (App.IsPropertyPage)
                {
                    abc = new PropertyLandingPage();
                }
                else
                {
                    abc = new LandingPage();
                }
                if (existingPages.Count > 0)
                {
                    Application.Current.MainPage.Navigation.InsertPageBefore(abc, Application.Current.MainPage.Navigation.NavigationStack[0]);

                    for (int i = 0; i < existingPages.Count; i++)
                    {
                        Application.Current.MainPage.Navigation.RemovePage(existingPages[i]);
                    }
                }
                else
                {
                    Application.Current.MainPage.Navigation.PopAsync();
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
                            pg.DisplayAlert("App Info", Functions.Appinfomsg, "Ok");
                            break;
                        case "Logout":
                            Logout(pg);
                            break;

                        default:
                            break;
                    }
                }
            }
            catch (Exception rt)
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

        //public static void GetImgLogo()
        //{
        //    try
        //    {
        //        Task.Run(() =>
        //          {
        //              App.DownloadCompanyLog();
        //              DownloadUserPicture();
        //              GetBaseURLFromSQLServer();
        //          });
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        public static void DownloadUserPicture()
        {
            try
            {
                var UPro = DBHelper.GetinstanceuserassocListByID(ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                UPro.Wait();
                string urlString = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl + "/userphotos/DownloadPhotomobile.aspx?username=" + Functions.UserName;

                ImageHelperClass iHeleper = new ImageHelperClass();
                var Src = iHeleper.DownloadImage(Convert.ToString(urlString));
                Src.Wait();

                UPro.Result.Profile_Picture = Src.Result;

                DBHelper.SaveInstanceUserAssoc(UPro.Result, App.DBPath).Wait();
            }
            catch (Exception)
            {

            }
        }

        public static void DownloadCompanyLog()
        {
            try
            {
                var Check = DBHelper.UserScreenRetrive("SYSTEMCODES", App.DBPath, "SYSTEMCODES");
                List<MobileBranding> MBrand = new List<MobileBranding>();
                if (!string.IsNullOrEmpty(Check.ASSOC_FIELD_INFO))
                    MBrand = JsonConvert.DeserializeObject<List<MobileBranding>>(Check.ASSOC_FIELD_INFO.ToString());
                else
                {
                    var result = DefaultAPIMethod.GetLogo("MLOGO");
                    var urlString = result.GetValue("ResponseContent");
                    MBrand = JsonConvert.DeserializeObject<List<MobileBranding>>(urlString.ToString());
                }

                ImageHelperClass iHeleper = new ImageHelperClass();
                var Src = iHeleper.DownloadImage(Convert.ToString(MBrand.Where(x => x.SYSTEM_CODE == "MLOGO")?.FirstOrDefault().VALUE));

                Src.Wait();
                var itm = DBHelper.GetInstanceListByID(Functions.Selected_Instance, App.DBPath);
                itm.Wait();
                InstanceList ils = new InstanceList();
                byte[] byt = Src.Result as byte[];

                Image image = new Image();
                Stream stream = new MemoryStream(byt);

                ils.Instance_Logo = byt;
                ils.InstanceID = itm.Result.InstanceID;
                ils.InstanceName = itm.Result.InstanceName;
                ils.InstanceUrl = itm.Result.InstanceUrl;

                DBHelper.SaveInstance(ils, App.DBPath).ConfigureAwait(false);
            }
            catch (Exception)
            {
            }
        }
    }
}
