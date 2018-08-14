using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class DefaultAPIMethod
    {

        #region Get User Info  
        public static JObject LoginAuthenticate(string UserName, string UserPassword)
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.LoginAuthenticate)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("UserName", UserName),
                  new KeyValuePair<string, string>("UserPassword", UserPassword),
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion


        #region GetProfilePicture 
        public static JObject GetProfilePicture(string UserName)
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.GetProfilePicture)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("UserName", UserName)
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region GetLogo 
        public static JObject GetLogo()
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.GetLogo)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region GetImageList 
        public static JObject GetImageList()
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.GetImageList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Decrypt 
        public static JObject Decrypt(string StringValue)
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.Decrypt)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("StringValue", StringValue)
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Encrypt 
        public static JObject Encrypt(string StringValue)
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.Encrypt)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("StringValue", StringValue)
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion


        #region HomeScreenCount 
        public static JObject HomeScreenCount(string username)
        {
            MobileAPIMethods Mapi = new MobileAPIMethods();
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.HomeScreenCount)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("username", username)
            };
            #endregion
            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region HomeScreenCount 
        public static JObject AddLog(string txt, string isSuccess, string appName, string CreatedBy, string createdDateTime)
        {
            try
            {
                MobileAPIMethods Mapi = new MobileAPIMethods();
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.AddLog)
            };
                #endregion

                #region API Body Details
                var Body_value = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("txt", txt.Length<=3999?txt:txt.Substring(0,3995)),
                    new KeyValuePair<string, string>("isSuccess", isSuccess),
                    new KeyValuePair<string, string>("appName", appName),
                    new KeyValuePair<string, string>("CreatedBy", CreatedBy),
                    new KeyValuePair<string, string>("createdDateTime", createdDateTime),
                };

                #endregion
                var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
                if (Result != null)
                {
                    return Result;
                }
                else
                    return null;
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion
        //#region AddLog 
        //public static JObject AddLog(string txt, string isSuccess, string appName, string CreatedBy, string createdDateTime)
        //{
        //    try
        //    {
        //        MobileAPIMethods Mapi = new MobileAPIMethods();
        //        #region API Details
        //        var API_value = new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Constants.AddLog)
        //    };
        //        #endregion
        //        JObject Result = null;

        //        if (txt.Length >= 3999)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            int startIndex = 0;
        //            for (int i = 0; i <= txt.Length / 3995; i++)
        //            {
        //                var st = txt.Length - startIndex;
        //                string s = txt.Substring(startIndex, st >= 3999 ? 3995 : st);
        //                sb.Append(s);
        //                startIndex += s.Length; //(txt.Length - sb.ToString().Length);
        //                #region API Body Details
        //                var Body_value = new List<KeyValuePair<string, string>>
        //                {
        //                    new KeyValuePair<string, string>("txt",s),
        //                    new KeyValuePair<string, string>("isSuccess", isSuccess),
        //                    new KeyValuePair<string, string>("appName", appName),
        //                    new KeyValuePair<string, string>("CreatedBy", CreatedBy),
        //                    new KeyValuePair<string, string>("createdDateTime", createdDateTime),
        //                };
        //                #endregion
        //                Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
        //            }
        //        }
        //        if (Result != null)
        //        {
        //            return Result;
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return null;
        //}
        //#endregion

        public static bool CheckInstance(string Instance)
        {
            return string.IsNullOrEmpty(MobileAPIMethods.RequestToken(Instance));
        }

    }
}
