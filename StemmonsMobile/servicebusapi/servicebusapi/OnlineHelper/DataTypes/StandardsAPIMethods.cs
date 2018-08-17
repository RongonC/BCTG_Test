using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class StandardsAPIMethods
    {

        #region GetBookList
        public static JObject GetBookList(string pUserSAM)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetBookList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pUserSAM", pUserSAM.ToString())


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

        #region security_GetAllAppForUser
        public static JObject security_GetAllAppForUser(string pUserSAM)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.security_GetAllAppForUser)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("sUserSAM", pUserSAM.ToString())


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

        #region Web View
        public static JObject WebView(string pAPP_ASSOC_META_DATA_ID, string pUserSAM, string pIncludeInactiveNodes)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.WebView)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pUserSAM", pUserSAM.ToString()),
                new KeyValuePair<string, string>("pAPP_ASSOC_META_DATA_ID", pAPP_ASSOC_META_DATA_ID.ToString()),
                new KeyValuePair<string, string>("pIncludeInactiveNodes", pIncludeInactiveNodes.ToString())


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

        #region security_SearchMetadata
        public static JObject security_SearchMetadata(string sUserName, string AppID, string Search)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.security_SearchMetadata)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("sUserName", sUserName.ToString()),
                new KeyValuePair<string, string>("AppID", null),
                new KeyValuePair<string, string>("Search", Search.ToString())


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


        #region Book View
        public static JObject BookView(string pAPP_ASSOC_META_DATA_ID, string pUserSAM, string pIncludeInactiveNodes)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.BookView)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pUserSAM", pUserSAM.ToString()),
                new KeyValuePair<string, string>("pAPP_ASSOC_META_DATA_ID", pAPP_ASSOC_META_DATA_ID.ToString()),
                new KeyValuePair<string, string>("pIncludeInactiveNodes", pIncludeInactiveNodes.ToString())


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


        #region Get Published App By User Based On SAM
        public static JObject GetPublishedAppByUserBasedOnSAM(string sUserName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetPublishedAppByUserBasedOnSAM)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("sUserName", sUserName.ToString()),
               

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


        #region GetAppCreatedByUserBasedOnSAM
        public static JObject GetAppCreatedByUserBasedOnSAM(string sUserSAM)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetAppCreatedByUserBasedOnSAM)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("sUserSAM", sUserSAM.ToString()),
             

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

    }
}
