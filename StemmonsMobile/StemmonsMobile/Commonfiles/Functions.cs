using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLStorage;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.DataTypes.DataType.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile.Commonfiles
{
    class Functions
    {

        public static int AppStartCount = 1;
        public static int tempitemid;
        public static Boolean FromHomePage = true;
        public static string UserName = "";
        public static string UserFullName = "";
        public static string Platformtype = "";
        public static string InstanceName = "";
        public static Boolean HasTeam = false;
        public static string access_token = "";
        public static int Selected_Instance = 0;
        public static bool IsPWDRemember = false;
        public static bool IsLogin = false;
        public static bool IsLogoutSuccess = false;
        public static string UserLoginName = "";
        public static string UserPassword = "";
        public static Add_Questions_MetadataRequest questObjectData = null;

        public enum BookCategoryTypes
        {
            All, MostPopular, Whatsnew, ForMe
        }

        public static bool IsEditEntity = false;
        public static bool CheckInternetWithAlert()
        {
            bool Isonline = true;
            Page p = new Page();
            Isonline = CrossConnectivity.Current.IsConnected;
            if (!Isonline)
                p.DisplayAlert("Internet Connection!", "Please, Check Your Internet Connectivity!", "Ok");
            return Isonline;
        }

        public static string HTMLToText(string HTMLCode)
        {
            if (HTMLCode == null)
                return "";

            // Remove new lines since they are not visible in HTML
            HTMLCode = HTMLCode.Replace("\r\n", Environment.NewLine);
            HTMLCode = HTMLCode.Replace("\r", Environment.NewLine);
            HTMLCode = HTMLCode.Replace("\n", Environment.NewLine);

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            //HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

            // Remove HEAD tag
            HTMLCode = Regex.Replace(HTMLCode, "<head.*?</head>", ""
                                , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Remove any JavaScript
            HTMLCode = Regex.Replace(HTMLCode, "<script.*?</script>", ""
              , RegexOptions.IgnoreCase | RegexOptions.Singleline);

            // Replace special characters like &, <, >, " etc.
            StringBuilder sbHTML = new StringBuilder(HTMLCode);
            // Note: There are many more special characters, these are just
            // most common. You can add new characters in this arrays if needed
            string[] OldWords = {"&nbsp;", "&amp;", "&quot;", "&lt;",
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;", "&#39;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢", "'" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", Environment.NewLine);
            sbHTML.Replace("<br/>", Environment.NewLine);
            sbHTML.Replace("<br />", Environment.NewLine);
            //sbHTML.Replace("<br ", Environment.NewLine);

            sbHTML.Replace("<p>", Environment.NewLine);
            sbHTML.Replace("<p/>", Environment.NewLine);
            // sbHTML.Replace("<p ", Environment.NewLine);


            // Finally, remove all HTML tags and return plain text
            return Regex.Replace(sbHTML.ToString(), "<[^>]*>", "");
        }

        public static void ShowtoastAlert(string Message)
        {
            ToastConfig t = new ToastConfig(Message);
            t.SetDuration(2000);
            UserDialogs.Instance.Toast(t);
        }



        public static async Task DownloadImageFile(string Url)
        {
            try
            {
                Uri url = new Uri(Url);
                var client = new HttpClient();

                IFile file = await FileSystem.Current.LocalStorage.CreateFileAsync(UserName + ".png", CreationCollisionOption.ReplaceExisting);
                using (var fileHandler = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                {
                    var httpResponse = await client.GetAsync(url);
                    byte[] dataBuffer = await httpResponse.Content.ReadAsByteArrayAsync();
                    await fileHandler.WriteAsync(dataBuffer, 0, dataBuffer.Length);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public static void ShowOverlayView_Grid(ContentView ActInd, bool IsHide, Grid grd)
        {
            ActInd.IsVisible = IsHide;
            grd.Opacity = IsHide ? 0.5 : 1;
        }

        public static void ShowOverlayView_StackLayout(ContentView ActInd, bool IsHide, StackLayout Stack)
        {
            ActInd.IsVisible = IsHide;
            Stack.Opacity = IsHide ? 0.5 : 1;
        }


        public static void ShowIndicator(ActivityIndicator ActInd, StackLayout Stack_indicator, bool IsHide, StackLayout Mainstack, double Opacity)
        {
            ActInd.IsEnabled = IsHide;
            ActInd.IsRunning = IsHide;
            Stack_indicator.IsVisible = IsHide;
            Mainstack.IsEnabled = !IsHide;
            Mainstack.Opacity = Opacity;
        }

        public static void ShowIndicatorUpdate(ActivityIndicator ActInd, StackLayout Stack_indicator, bool IsHide, AbsoluteLayout Mainstack, double Opacity, Page Pg)
        {
            ActInd.IsEnabled = IsHide;
            ActInd.IsRunning = IsHide;
            Stack_indicator.IsVisible = IsHide;
            Mainstack.IsEnabled = !IsHide;
            Mainstack.Opacity = Opacity;
            Pg.IsBusy = IsHide;
        }

        public static string GetDecodeConnectionString(string Connectionstr)
        {
            string Con = "";
            try
            {
                if (!Connectionstr.Contains("Data Source ="))
                {
                    var res = DefaultAPIMethod.Decrypt(Connectionstr);
                    var temp = res.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        Con = temp.ToString();
                    }
                }
                else
                    Con = Connectionstr;
            }
            catch (Exception)
            {
            }
            return Con;

        }

        public static List<MobileBranding> GetImageDownloadURL()
        {
            List<MobileBranding> lst = new List<MobileBranding>();
            if (Functions.CheckInternetWithAlert())
            {
                try
                {
                    Task.Run(() =>
                    {
                        var Res = DefaultAPIMethod.GetImageList();
                        var Result = Res.GetValue("ResponseContent");
                        if (!string.IsNullOrEmpty(Convert.ToString(Result)))
                        {
                            lst = JsonConvert.DeserializeObject<List<MobileBranding>>(Result.ToString());
                        }
                    }).Wait();

                    var Check = DBHelper.UserScreenRetrive("SYSTEMCODES", App.DBPath, "SYSTEMCODES");
                    AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();

                    _AppTypeInfoList = new AppTypeInfoList
                    {
                        ASSOC_FIELD_INFO = JsonConvert.SerializeObject(lst),
                        LAST_SYNC_DATETIME = DateTime.Now,
                        SYSTEM = "SYSTEMCODES",
                        TYPE_ID = 0,
                        ID = 0,
                        CategoryId = 0,
                        CategoryName = "",
                        TransactionType = "M",
                        TYPE_SCREEN_INFO = "SYSTEMCODES",
                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                        IS_ONLINE = true
                    };

                    if (Check == null)
                        _AppTypeInfoList.APP_TYPE_INFO_ID = 0;
                    else
                        _AppTypeInfoList.APP_TYPE_INFO_ID = Check.APP_TYPE_INFO_ID;
                    var y = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, App.DBPath);

                }
                catch (Exception)
                {
                }
            }
            return lst;
        }

        public static void ClearApplocalData()
        {
            try
            {
                Functions.UserName = string.Empty;
                Application.Current.Properties["UserName"] = string.Empty;

                Functions.UserFullName = string.Empty;
                Application.Current.Properties["UserFullName"] = string.Empty;

                Application.Current.Properties["IsLogin"] = false;
                Functions.IsLogin = false;

                Application.Current.Properties["IsPWDRemember"] = false;
                Functions.IsPWDRemember = false;

                Functions.UserLoginName = string.Empty;
                Application.Current.Properties["UserLoginName"] = string.Empty;

                Functions.UserPassword = string.Empty;
                Application.Current.Properties["UserPassword"] = string.Empty;

                Application.Current.Properties["Baseurl"] = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl;

                Application.Current.Properties["Selected_Instance"] = Functions.Selected_Instance;

                Application.Current.Properties["InstanceName"] = Functions.InstanceName;

            }
            catch (Exception)
            {
            }

        }


        #region Messages For Whole APp

        public static string Appinfomsg = "Stemmons Central to Go (v" + CrossDeviceInfo.Current.AppVersion + ")\nCopyright © 2018 by Stemmons Enterprise LLC.";
        public static string Goonline_forFunc = "Please Go online to use this functionality!";

        public static string nRcrdOffline = "No Record Found.\nPlease go online to view full list.";
        public static string nRcrdOnline = "No Record Found.";

        #endregion


    }
}
