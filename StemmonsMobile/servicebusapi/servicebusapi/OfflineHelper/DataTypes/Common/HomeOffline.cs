using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Common
{
    public class HomeOffline
    {

        #region Get All Home Page Count
        public static string GetAllHomeCount(string username, int INSTANCE_ID, string _DBPath, int? INSTANCE_USER_ASSOC_ID)
        {
            string sError = string.Empty;

            JObject Result = null;
            try
            {
                Result = DefaultAPIMethod.HomeScreenCount(username);

                if (Result != null)
                {
                    string ResponseContent = Convert.ToString(Result.GetValue("ResponseContent"));
                    sError = ResponseContent;
                    //DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(Result), "Y", "GetAllHomeCount", username, DateTime.Now.ToString());

                    INSTANCE_USER_ASSOC _AppTypeInfoList = new INSTANCE_USER_ASSOC();
                    _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = INSTANCE_USER_ASSOC_ID ?? default(int);
                    ;
                    _AppTypeInfoList.HOME_SCREEN_INFO = ResponseContent;
                    _AppTypeInfoList.USER = username;
                    _AppTypeInfoList.INSTANCE_ID = INSTANCE_ID;

                    DBHelper.Save_InstanceUserAssoc(_AppTypeInfoList, _DBPath).Wait();
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(Result), "N", "GetAllHomeCount", username, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAllHomeCount", username, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(Result), "N", "GetAllHomeCount", username, DateTime.Now.ToString());
            }

            return sError;
        }
        #endregion

        #region  Home Count Offline 
        public static HomeScreenCount GetHomeScreenCount(bool _IsOnline, string username, int INSTANCE_ID, string _DBPath, int? INSTANCE_USER_ASSOC_ID)
        {
            HomeScreenCount Response = new HomeScreenCount();
            try
            {
                if (INSTANCE_USER_ASSOC_ID != null)
                {
                    var result = DBHelper.GetinstanceuserassocListByUsername_Id(username, INSTANCE_ID, _DBPath);
                    result.Wait();
                    if (result.Result != null)
                        Response = JsonConvert.DeserializeObject<HomeScreenCount>(result.Result.HOME_SCREEN_INFO);

                    if (string.IsNullOrEmpty(result.Result.HOME_SCREEN_INFO))
                    {
                        try
                        {
                            HomeOffline.GetAllHomeCount(username, INSTANCE_ID, _DBPath, INSTANCE_USER_ASSOC_ID);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Response;
        }
        #endregion
    }
}
