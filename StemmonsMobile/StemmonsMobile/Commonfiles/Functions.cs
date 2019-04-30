using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Default;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.DataTypes.DataType.Quest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
        public static List<EntityClass> MyProperty = new List<EntityClass>();
        public static List<EntityClass> PropertyList = new List<EntityClass>();
        public static List<EntityClass> CampusList = new List<EntityClass>();


        public static ObservableCollection<SyncStatus> lstSyncAPIStatus = new ObservableCollection<SyncStatus>()
        {
            new SyncStatus() { APIName = "Home Count" },
            new SyncStatus() { APIName = "Cases Origination" },
            new SyncStatus() { APIName = "Cases Sync" },
            new SyncStatus() { APIName = "CaseList Assigned To Me" },
            new SyncStatus() { APIName = "CaseList Created By Me" },
            new SyncStatus() { APIName = "CaseList Owned By Me" },
            new SyncStatus() { APIName = "CaseList Assigned To My Team" },
            new SyncStatus() { APIName = "Entity Sync" },
            new SyncStatus() { APIName = "Entity Associated With Me" },
            new SyncStatus() { APIName = "Quest Sync" },
            new SyncStatus() { APIName = "Standard Sync" },
            new SyncStatus() { APIName = "Employee Search Sync" },
        };

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

        public static string StripHTML(string source)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&bull;", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lsaquo;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&rsaquo;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&trade;", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&frasl;", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lt;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&gt;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&copy;", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&reg;", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result;
            }
            catch
            {
                //MessageBox.Show("Error");
                return source;
            }
        }

        public static void ShowtoastAlert(string Message)
        {
            ToastConfig t = new ToastConfig(Message);
            t.SetDuration(2000);
            UserDialogs.Instance.Toast(t);
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

        //public static void ShowIndicator(ActivityIndicator ActInd, StackLayout Stack_indicator, bool IsHide, StackLayout Mainstack, double Opacity)
        //{
        //    ActInd.IsEnabled = IsHide;
        //    ActInd.IsRunning = IsHide;
        //    Stack_indicator.IsVisible = IsHide;
        //    Mainstack.IsEnabled = !IsHide;
        //    Mainstack.Opacity = Opacity;
        //}

        //public static void ShowIndicatorUpdate(ActivityIndicator ActInd, StackLayout Stack_indicator, bool IsHide, AbsoluteLayout Mainstack, double Opacity, Page Pg)
        //{
        //    ActInd.IsEnabled = IsHide;
        //    ActInd.IsRunning = IsHide;
        //    Stack_indicator.IsVisible = IsHide;
        //    Mainstack.IsEnabled = !IsHide;
        //    Mainstack.Opacity = Opacity;
        //    Pg.IsBusy = IsHide;
        //}

        public static string GetDecodeConnectionString(string Connectionstr)
        {
            string Con = "";
            try
            {
                if (!Connectionstr.Contains("Data Source =") && !Connectionstr.Contains("Data Source="))
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

        public static void GetSystemCodesfromSqlServerAsync()
        {
            try
            {
                List<MobileBranding> MBrand = new List<MobileBranding>();
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        //var Check = DBHelper.UserScreenRetrive("SYSTEMCODES", App.DBPath, "SYSTEMCODES");
                        //if (Check != null)
                        //{
                        //    if (!string.IsNullOrEmpty(Check?.ASSOC_FIELD_INFO))
                        //    {
                        //        MBrand = JsonConvert.DeserializeObject<List<MobileBranding>>(Check.ASSOC_FIELD_INFO.ToString());
                        //    }
                        //}
                        //else
                        // {
                        var Res = DefaultAPIMethod.GetImageList();
                        var Result = Res.GetValue("ResponseContent");
                        if (!string.IsNullOrEmpty(Convert.ToString(Result)))
                        {
                            MBrand = JsonConvert.DeserializeObject<List<MobileBranding>>(Result.ToString());
                        }
                        //}

                        App.EntityImgURL = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "ENTHM").FirstOrDefault().VALUE;
                        App.CasesImgURL = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "CSHOM").FirstOrDefault().VALUE;
                        App.StandardImgURL = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "STHOM").FirstOrDefault().VALUE;

                        string Code = App.IsCustomLandingPage ? "B2VER" : "MBVER";
                        App.CurretVer = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == Code).FirstOrDefault().VALUE;

                       // App.CLientID = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLCL").FirstOrDefault().VALUE;
                       // App.IsSAMLAuth = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "MSAML").FirstOrDefault().VALUE == "Y" ? true : false;
                       // //App.ClientSecret = "c&;^#*%&.a{;+./V|v{$^?Y#[@.t&)8&])$!-";
                       // App.ClientSecret = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLSR")?.FirstOrDefault()?.VALUE;
                        ////App.Scope = "https://graph.microsoft.com/.default";
                        //App.Scope = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLSC")?.FirstOrDefault()?.VALUE;
                       // App.AuthorizeUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLAU").FirstOrDefault().VALUE;
                       // App.AccessTokenUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLTU").FirstOrDefault().VALUE;
                       // App.UserinfoUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLUL").FirstOrDefault().VALUE;
                       // App.RedirectUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLRU").FirstOrDefault().VALUE;

                        //var CheckRec = DBHelper.UserScreenRetrive("SYSTEMCODES", App.DBPath, "SYSTEMCODES");
                        var CheckRec = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Functions.Selected_Instance, "SYSTEMCODES", "SYSTEMCODES", App.DBPath);

                        AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();

                        _AppTypeInfoList = new AppTypeInfoList
                        {
                            ASSOC_FIELD_INFO = JsonConvert.SerializeObject(MBrand),
                            LAST_SYNC_DATETIME = DateTime.Now,
                            SYSTEM = "SYSTEMCODES",
                            TYPE_ID = Functions.Selected_Instance,
                            ID = 0,
                            CategoryId = 0,
                            CategoryName = "",
                            TransactionType = "M",
                            TYPE_SCREEN_INFO = "SYSTEMCODES",
                            INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                            IS_ONLINE = true
                        };

                        if (CheckRec.Result == null)
                            _AppTypeInfoList.APP_TYPE_INFO_ID = 0;
                        else
                            _AppTypeInfoList.APP_TYPE_INFO_ID = CheckRec.Result.APP_TYPE_INFO_ID;
                        var y = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, App.DBPath);

                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    try
                    {
                        GetBaseURLfromSQLite();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public static void GetBaseURLfromSQLite()
        {
            try
            {
                var Result = DBHelper.UserScreenRetrive("SYSTEMCODES", App.DBPath, "SYSTEMCODES");

                if (!string.IsNullOrEmpty(Result.ASSOC_FIELD_INFO))
                {
                    var MBrand = JsonConvert.DeserializeObject<List<MobileBranding>>(Result.ASSOC_FIELD_INFO.ToString());
                    App.EntityImgURL = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "ENTHM").FirstOrDefault().VALUE;
                    App.CasesImgURL = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "CSHOM").FirstOrDefault().VALUE;
                    App.StandardImgURL = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "STHOM").FirstOrDefault().VALUE;

                    string Code = App.IsCustomLandingPage ? "B2VER" : "MBVER";
                    App.CurretVer = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == Code).FirstOrDefault().VALUE;

                    //App.CLientID = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLCL").FirstOrDefault().VALUE;
                    //App.IsSAMLAuth = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "MSAML").FirstOrDefault().VALUE == "Y" ? true : false;
                   // //App.ClientSecret = "c&;^#*%&.a{;+./V|v{$^?Y#[@.t&)8&])$!-";
                   // App.ClientSecret = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLSR")?.FirstOrDefault()?.VALUE;
                    //App.Scope = "https://graph.microsoft.com/.default";
                    //App.Scope = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLSC")?.FirstOrDefault()?.VALUE;
                   // App.AuthorizeUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLAU").FirstOrDefault().VALUE;
                    //App.AccessTokenUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLTU").FirstOrDefault().VALUE;
                    //App.UserinfoUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLUL").FirstOrDefault().VALUE;
                    //App.RedirectUrl = MBrand.Where(v => v.SYSTEM_CODE.ToUpper() == "SMLRU").FirstOrDefault().VALUE;
                }
            }
            catch (Exception)
            {
            }
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

        public static string Appinfomsg = "Boxer Central to Go v" + CrossDeviceInfo.Current.AppVersion + "(" + CrossDeviceInfo.Current.AppBuild + ")\nCopyright © 2018 by Stemmons Enterprise LLC.";
        public static string Goonline_forFunc = "Please Go online to use this functionality!";

        public static string nRcrdOffline = "No Record Found.\nPlease go online to view full list.";
        public static string nRcrdOnline = "No Record Found.";

        #endregion

        public static string GenerateEntityFullURL(string Note)
        {
            string URl;
            if (Note.ToLower().Contains("/download.aspx"))
                URl = Note.ToLower().Replace("/download.aspx", App.EntityImgURL.ToLower() + "/download.aspx");
            else if (Note.ToLower().Contains("'download.aspx"))
                URl = Note.ToLower().Replace("'download.aspx", "'" + App.EntityImgURL.ToLower() + "/download.aspx");
            else if (Note.ToLower().Contains("'/download.aspx"))
                URl = Note.ToLower().Replace("'/download.aspx", App.EntityImgURL.ToLower() + "/download.aspx");
            else if (Note.ToLower().Contains("download.aspx"))
                URl = Note.ToLower().Replace("download.aspx", "'" + App.EntityImgURL.ToLower() + "/download.aspx");
            else if (Note.ToLower().Contains("/download.aspx"))
                URl = Note.ToLower().Replace("/download.aspx", App.EntityImgURL.ToLower() + "/download.aspx");
            else if (Note.ToLower().Contains("'download.aspx"))
                URl = Note.ToLower().Replace("'download.aspx", "'" + App.EntityImgURL.ToLower() + "/download.aspx");
            else
                URl = Note.ToLower();
            return URl;
        }

        public static string GenerateCasesFullURL(string Note)
        {
            string URl = string.Empty;

            if (Note.Contains("/DownloadFile.aspx"))
                URl = Note.Replace("/DownloadFile.aspx", App.CasesImgURL + "/DownloadFile.aspx");
            else if (Note.Contains("/downloadfile.aspx"))
                URl = Note.Replace("/downloadfile.aspx", App.CasesImgURL + "/DownloadFile.aspx");
            else if (Note.ToString().Contains("'DownloadFile.aspx"))
                Note = Note.Replace("'DownloadFile.aspx", App.CasesImgURL + "/DownloadFile.aspx");
            else if (Note.ToString().Contains("'downloadFile.aspx"))
                Note = Note.Replace("'downloadFile.aspx", App.CasesImgURL + "/DownloadFile.aspx");
            else
                URl = Note;

            return URl.ToString();
        }

        public static ImageSource GetImageFromEntityAssoc(List<AssociationField> AssociationFieldCollection)
        {
            try
            {
                var FileID = AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityFileID;

                var EntityID = AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityID;

                string fileStr = string.Empty;

                Task.Run(() =>
                {
                    var d = EntityAPIMethods.GetFileFromEntity(EntityID.ToString(), FileID.ToString(), Functions.UserName);
                    fileStr = d.GetValue("ResponseContent").ToString();
                }).Wait();

                FileItem fileResp = JsonConvert.DeserializeObject<FileItem>(fileStr);

                return ImageSource.FromStream(() => new MemoryStream(fileResp.BLOB));
            }
            catch (Exception ex)
            {
                return ImageSource.FromFile("Assets/na.png");
            }
        }
    }
}
