using Acr.UserDialogs;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLStorage;
using Plugin.Connectivity;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.DataTypes.DataType.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            HTMLCode = HTMLCode.Replace("\n", Environment.NewLine);

            // Remove tab spaces
            HTMLCode = HTMLCode.Replace("\t", " ");

            // Remove multiple white spaces from HTML
            HTMLCode = Regex.Replace(HTMLCode, "\\s+", " ");

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
   "&gt;", "&reg;", "&copy;", "&bull;", "&trade;"};
            string[] NewWords = { " ", "&", "\"", "<", ">", "Â®", "Â©", "â€¢", "â„¢" };
            for (int i = 0; i < OldWords.Length; i++)
            {
                sbHTML.Replace(OldWords[i], NewWords[i]);
            }

            // Check if there are line breaks (<br>) or paragraph (<p>)
            sbHTML.Replace("<br>", Environment.NewLine);
            sbHTML.Replace("<br ", Environment.NewLine);
            sbHTML.Replace("<p ", Environment.NewLine);
            sbHTML.Replace("<p> ", Environment.NewLine);

            // Finally, remove all HTML tags and return plain text
            return System.Text.RegularExpressions.Regex.Replace(
              sbHTML.ToString(), "<[^>]*>", "");
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
                //Uri url = new Uri(Url);
                //var client = new HttpClient();

                //var fol = await App.rootFolder.GetFolderAsync("Entity");
                //IFile file = await App.rootFolder.CreateFileAsync(UserName + ".png", CreationCollisionOption.ReplaceExisting);
                //using (var fileHandler = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                //{
                //    var httpResponse = await client.GetAsync(url);
                //    byte[] dataBuffer = await httpResponse.Content.ReadAsByteArrayAsync();
                //    await fileHandler.WriteAsync(dataBuffer, 0, dataBuffer.Length);
                //}
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

                //Functions.Baseurl = string.Empty;
                //Application.Current.Properties["Baseurl"] = string.Empty;

                //Application.Current.Properties["Selected_Instance"] = 0;
                //Functions.Selected_Instance = 0;


                //Application.Current.Properties["InstanceName"] = string.Empty;
                //Functions.InstanceName = string.Empty;

            }
            catch (Exception)
            {

                throw;
            }

        }


        #region Messages For Whole APp

        public static string Appinfomsg = "Stemmons Central to Go\nCopyright © 2018 by Stemmons Enterprise LLC.";
        public static string Goonline_forFunc = "Please Go online to use this functionality!";

        public static string nRcrdOffline = "No Record Found.\nPlease go online to view full list.";
        public static string nRcrdOnline = "No Record Found.";

        #endregion


    }
}
