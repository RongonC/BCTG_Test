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
        public static SyncStatus GetAllHomeCount(string username, int INSTANCE_ID, string _DBPath, int? INSTANCE_USER_ASSOC_ID)
        {
            string sError = string.Empty;
            SyncStatus sn = new SyncStatus();
            JObject Result = null;
            try
            {
                Result = DefaultAPIMethod.HomeScreenCount(username);

                if (Result != null)
                {
                    string ResponseContent = Convert.ToString(Result.GetValue("ResponseContent"));
                    sError = ResponseContent;

                    sn.ApiCallSuccess = true;
                    sn.FailDesc = ResponseContent.ToString();

                    INSTANCE_USER_ASSOC _AppTypeInfoList = new INSTANCE_USER_ASSOC();
                    _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = INSTANCE_USER_ASSOC_ID ?? default(int);
                    _AppTypeInfoList.HOME_SCREEN_INFO = ResponseContent;
                    _AppTypeInfoList.USER = username;
                    _AppTypeInfoList.INSTANCE_ID = INSTANCE_ID;

                    DBHelper.Save_InstanceUserAssoc(_AppTypeInfoList, _DBPath).Wait();
                    //int a2 = 1;
                    //int a = a2 / 0;
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(Result), "N", "GetAllHomeCount", username, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                sn.ApiCallSuccess = false;
                sn.FailDesc = ex.Message.ToString();
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAllHomeCount", username, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(Result), "N", "GetAllHomeCount", username, DateTime.Now.ToString());
            }

            return sn;
        }
        #endregion

        #region  Home Count Offline 
        public static HomeScreenCount GetHomeScreenCount(bool _IsOnline, string username, int INSTANCE_ID, string _DBPath, int? INSTANCE_USER_ASSOC_ID)
        {
            // HomeScreenCount Response = new HomeScreenCount();
            try
            {
                if (INSTANCE_USER_ASSOC_ID != null)
                {
                    var result = DBHelper.GetinstanceuserassocListByUsername_Id(username, INSTANCE_ID, _DBPath);
                    //result.Wait();
                    if (result.Result != null)
                        return JsonConvert.DeserializeObject<HomeScreenCount>(result.Result.HOME_SCREEN_INFO);

                    if (string.IsNullOrEmpty(result.Result.HOME_SCREEN_INFO))
                    {
                        try
                        {
                            var Result = HomeOffline.GetAllHomeCount(username, INSTANCE_ID, _DBPath, INSTANCE_USER_ASSOC_ID);
                            if (Result != null)
                            {
                                return JsonConvert.DeserializeObject<HomeScreenCount>(Result.FailDesc);
                            }
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
            return null;
        }
        #endregion
    }
}
