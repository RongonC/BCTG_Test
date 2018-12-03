using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Standards;
using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Standards
{
    public class StandardsSyncAPIMethods
    {

        #region Get All Standards Data By UserName
        public static string GetAllStandards(string pUserSAM, string _DBPath)
        {
            string sError = string.Empty;
            JObject Result = null;
            try
            {
                MobileAPIMethods Mapi = new MobileAPIMethods();
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + ConstantsSync.GetAllStandards)
            };
                #endregion

                #region API Body Details
                var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pUserSAM", pUserSAM)
            };
                #endregion
                Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");

                if (Result != null)
                {
                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(Result), "Y", "GetAllStandards", pUserSAM, DateTime.Now.ToString());
                    string ResponseContent = Convert.ToString(Result.GetValue("ResponseContent"));

                    sError = ResponseContent;

                    if (!string.IsNullOrEmpty(ResponseContent) && Convert.ToString(ResponseContent) != "[]" && Convert.ToString(ResponseContent) != "{}" && Convert.ToString(ResponseContent) != "[ ]" && Convert.ToString(ResponseContent) != "{ }" && Convert.ToString(ResponseContent) != "[{ }]" && Convert.ToString(ResponseContent) != "[{}]")
                    {

                        #region Delete Data Before Master Sync
                        var CaseDate = DBHelper.GetAppTypeInfoListBySystemName(ConstantsSync.StandardInstance, "I1_I2_AllStandardData", _DBPath);
                        CaseDate.Wait();
                        if (CaseDate.Result.Count > 0)
                        {
                            var MultiId = string.Join(",", CaseDate.Result.Select(x => x.APP_TYPE_INFO_ID).ToList().ToArray());

                            CasesSyncAPIMethods.DeleteRecordBeforeSync(_DBPath, MultiId);

                            //foreach (var item in CaseDate.Result)
                            //{
                            //    DBHelper.DeleteAppTypeInfoListById(item, _DBPath).Wait();

                            //    var EDS = DBHelper.GetEDSResultListwithAPP_TYPE_INFO_ID(item.APP_TYPE_INFO_ID, _DBPath);
                            //    EDS.Wait();
                            //    foreach (var itm in EDS.Result)
                            //    {
                            //        DBHelper.DeleteEDSResultListById(itm, _DBPath).Wait();
                            //    }
                            //}

                        }
                        #endregion
                        CommonConstants.MasterOfflineStore(ResponseContent, _DBPath);
                    }
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(Result), "N", "GetAllStandards", pUserSAM, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAllStandards", pUserSAM, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(Result), "N", "GetAllStandards", pUserSAM, DateTime.Now.ToString());
            }
            return sError;
        }
        #endregion

        #region  GetBookList
        public async static Task<standards> GetBookList(bool _IsOnline, string Sys_Name, string username, string _DBPath)
        {
            standards Response;
            JObject Result;
            try
            {
                if (_IsOnline)
                {
                    Result = StandardsAPIMethods.GetBookList(username);

                    string jsonValue = Convert.ToString(Result.GetValue("ResponseContent"));
                    Response = JsonConvert.DeserializeObject<standards>(jsonValue);

                    //AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList
                    //{
                    //    APP_TYPE_INFO_ID = 0,
                    //    ASSOC_FIELD_INFO = jsonValue,
                    //    LAST_SYNC_DATETIME = DateTime.Now,
                    //    SYSTEM = Sys_Name,
                    //    TYPE_ID = 0,
                    //    TYPE_NAME = "",
                    //    CategoryId = 0,
                    //    CategoryName = "",
                    //    TransactionType = "M",
                    //    IS_ONLINE = _IsOnline,

                    //    TYPE_SCREEN_INFO = ConstantsSync.GetBookDetailToUser,
                    //    INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID
                    //};

                    //Task<int> i = DBHelper.UpdateAppTypeInfoList(_AppTypeInfoList, _DBPath);
                    //i.Wait();
                }
                else
                {
                    AppTypeInfoList result = DBHelper.UserScreenRetrive(Sys_Name, _DBPath, "I1_I2_AllStandardData");

                    Response = JsonConvert.DeserializeObject<standards>(result.ASSOC_FIELD_INFO);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Response;
        }
        #endregion

        #region  BookViewOnlineOffline
        public async static Task<standardsBookView> BookViewOnlineOffline(bool _IsOnline, int TypeId, string Sys_Name, string username, string _DBPath)
        {
            standardsBookView Response = new standardsBookView();
            JObject Result = null;
            try
            {
                if (_IsOnline)
                {
                    Result = StandardsAPIMethods.BookView(TypeId.ToString(), username, "Y"); ;

                    string jsonValue = Convert.ToString(Result.GetValue("ResponseContent"));

                    Response = JsonConvert.DeserializeObject<standardsBookView>(jsonValue);


                    Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(Sys_Name, ConstantsSync.BookView, TypeId, _DBPath, null);
                    result.Wait();
                    AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();

                    if (result.Result != null)
                    {
                        _AppTypeInfoList.APP_TYPE_INFO_ID = result.Result.APP_TYPE_INFO_ID;
                    }
                    else
                        _AppTypeInfoList.APP_TYPE_INFO_ID = 0;

                    _AppTypeInfoList.ASSOC_FIELD_INFO = jsonValue;
                    _AppTypeInfoList.LAST_SYNC_DATETIME = DateTime.Now;
                    _AppTypeInfoList.SYSTEM = Sys_Name;
                    _AppTypeInfoList.TYPE_ID = TypeId;
                    _AppTypeInfoList.TYPE_NAME = "";
                    _AppTypeInfoList.CategoryId = 0;
                    _AppTypeInfoList.CategoryName = "";
                    _AppTypeInfoList.TYPE_SCREEN_INFO = ConstantsSync.BookView;
                    _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    _AppTypeInfoList.IS_ONLINE = _IsOnline;

                    Task<int> i = DBHelper.UpdateAppTypeInfoList(_AppTypeInfoList, _DBPath);
                    i.Wait();
                }
                else
                {
                    Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(Sys_Name, ConstantsSync.BookView, TypeId, _DBPath, null);
                    result.Wait();
                    if (result.Result != null)
                    {
                        Response = JsonConvert.DeserializeObject<standardsBookView>(result.Result.ASSOC_FIELD_INFO);
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

        #region GetCaseListSync
        public async static Task<JObject> GetAllAppForUserSync(bool _IsOnline, string username, string _DBPath)
        {

            JObject Result;
            try
            {

                if (_IsOnline)
                {
                    Result = StandardsAPIMethods.security_GetAllAppForUser(username);

                    int TypeID = 0;

                    string jsonValue = Convert.ToString(Result.GetValue("ResponseContent"));

                    AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();

                    _AppTypeInfoList.APP_TYPE_INFO_ID = 0;
                    _AppTypeInfoList.ASSOC_FIELD_INFO = jsonValue;
                    _AppTypeInfoList.LAST_SYNC_DATETIME = DateTime.Now;
                    _AppTypeInfoList.SYSTEM = ConstantsSync.StandardInstance;
                    _AppTypeInfoList.TYPE_ID = TypeID;
                    _AppTypeInfoList.TYPE_NAME = "";
                    _AppTypeInfoList.CategoryId = 0;
                    _AppTypeInfoList.CategoryName = "";
                    _AppTypeInfoList.TYPE_SCREEN_INFO = ConstantsSync.GetAllAppForUser;
                    _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    _AppTypeInfoList.IS_ONLINE = _IsOnline;

                    Task<int> i = DBHelper.UpdateAppTypeInfoList(_AppTypeInfoList, _DBPath);
                    i.Wait();
                }
                else
                {
                    AppTypeInfoList result = DBHelper.UserScreenRetrive(ConstantsSync.StandardInstance, _DBPath, ConstantsSync.GetAllAppForUser);

                    security_GetAllAppForUserResponse obj = new security_GetAllAppForUserResponse();

                    obj.ResponseContent = JsonConvert.DeserializeObject<List<ForMe>>(result.ASSOC_FIELD_INFO);

                    Result = (JObject)JToken.FromObject(obj);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Result;
        }
        #endregion

        #region B1_Screen GetAllAppForUserSync
        public static async Task<List<Grp_StandardDetails>> GetAllAppForUserSync(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<Grp_StandardDetails> lstResult = new List<Grp_StandardDetails>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.StandardInstance, ConstantsSync.GetAllAppForUser, _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = StandardsAPIMethods.security_GetAllAppForUser(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<Grp_StandardDetails>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), ConstantsSync.StandardInstance, ConstantsSync.GetAllAppForUser, _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<Grp_StandardDetails>(ConstantsSync.StandardInstance, ConstantsSync.GetAllAppForUser, _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region B1_Screen GetPublishedAppByUserBasedOnSAM
        public static async Task<List<Grp_StandardDetails>> GetPublishedAppByUserBasedOnSAM(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<Grp_StandardDetails> lstResult = new List<Grp_StandardDetails>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.StandardInstance, "B1_GetPublishedAppByUserBasedOnSAM", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = StandardsAPIMethods.GetPublishedAppByUserBasedOnSAM(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<Grp_StandardDetails>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), ConstantsSync.StandardInstance, "B1_GetPublishedAppByUserBasedOnSAM", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<Grp_StandardDetails>(ConstantsSync.StandardInstance, "B1_GetPublishedAppByUserBasedOnSAM", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region B1_Screen GetAppCreatedByUserBasedOnSAM
        public static async Task<List<Grp_StandardDetails>> GetAppCreatedByUserBasedOnSAM(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<Grp_StandardDetails> lstResult = new List<Grp_StandardDetails>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.StandardInstance, "B1_GetAppCreatedByUserBasedOnSAM", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = StandardsAPIMethods.GetAppCreatedByUserBasedOnSAM(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<Grp_StandardDetails>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), ConstantsSync.StandardInstance, "B1_GetAppCreatedByUserBasedOnSAM", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<Grp_StandardDetails>(ConstantsSync.StandardInstance, "B1_GetAppCreatedByUserBasedOnSAM", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region B1_Screen GetAppRelateToUserBasedOnSAM
        public static async Task<List<Grp_StandardDetails>> GetAppRelateToUserBasedOnSAM(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<Grp_StandardDetails> lstResult = new List<Grp_StandardDetails>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.StandardInstance, "B1_GetAppRelateToUserBasedOnSAM", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = StandardsAPIMethods.GetAppRelateToUserBasedOnSAM(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<Grp_StandardDetails>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), ConstantsSync.StandardInstance, "B1_GetAppRelateToUserBasedOnSAM", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<Grp_StandardDetails>(ConstantsSync.StandardInstance, "B1_GetAppRelateToUserBasedOnSAM", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region SearchMetadata
        public static async Task<List<MetaData>> SearchMetadata(bool _IsOnline, string searchText, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<MetaData> lstResult = new List<MetaData>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.StandardInstance, ConstantsSync.GetAllAppForUser, _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = StandardsAPIMethods.security_SearchMetadata(_UserName, null, searchText);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<MetaData>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            //var inserted = CommonConstants.AddRecordOfflineStore(JsonConvert.SerializeObject(lstResult), ConstantsSync.StandardInstance, ConstantsSync.security_SearchMetadata, _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }
                }
                else
                {
                    //lstResult = CommonConstants.ReturnListResult<MetaData>(ConstantsSync.StandardInstance, ConstantsSync.GetAllAppForUser, _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion
    }
}
