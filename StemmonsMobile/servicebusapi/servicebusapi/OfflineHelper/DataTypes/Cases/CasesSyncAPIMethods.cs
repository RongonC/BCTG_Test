using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using StemmonsMobile.DataTypes.DataType.Cases;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;

namespace DataServiceBus.OfflineHelper.DataTypes.Cases
{
    public class CasesSyncAPIMethods
    {
        #region Get All Case Type With ID
        public static string GetAllCaseTypeWithID(string _CaseTypeId, string _UserName, string _ListType, string _DBPath)
        {
            string sError = string.Empty;
            JObject Result = null;
            try
            {
                MobileAPIMethods Mapi = new MobileAPIMethods();
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl  + Common.ConstantsSync.GetAllCaseTypeWithID)
            };
                #endregion

                #region API Body Details
                var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CaseTypeId", _CaseTypeId),
                new KeyValuePair<string, string>("UserName", _UserName),
                new KeyValuePair<string, string>("ListType", _ListType),
            };
                #endregion

                Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
                if (Result != null)
                {
                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(Result), "Y", "GetAllCaseTypeWithID", _UserName, DateTime.Now.ToString());

                    string Res = Convert.ToString(Result.GetValue("ResponseContent"));

                    if (!string.IsNullOrEmpty(Res) && Convert.ToString(Res) != "[]" && Convert.ToString(Res) != "{}" && Convert.ToString(Res) != "[ ]" && Convert.ToString(Res) != "{ }" && Convert.ToString(Res) != "[{ }]" && Convert.ToString(Res) != "[{}]")
                    {
                        sError = Convert.ToString(Result);
                        //CommonConstants.MasterOfflineStore(Res, _DBPath);

                        Task.Run(() =>
                        {
                            #region Delete Data Before Master Sync
                            var CaseDate = DBHelper.GetAppTypeInfoListBySystemName("CASES", "C1_C2_CASES_CASETYPELIST", _DBPath);
                            CaseDate.Wait();
                            if (CaseDate.Result.Count > 0)
                            {
                                var MultiId = string.Join(",", CaseDate.Result.Select(x => x.APP_TYPE_INFO_ID).ToList().ToArray());

                                DeleteRecordBeforeSync(_DBPath, MultiId);
                            }
                            #endregion
                            CommonConstants.MasterOfflineStore_withEDSTable(Res, _DBPath);
                        });
                    }
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(Result), "N", "GetAllCaseTypeWithID", _UserName, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exception Log => " + ex.Message.ToString(), "N", "GetAllCaseTypeWithID", _UserName, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exception Log => " + Convert.ToString(Result), "N", "GetAllCaseTypeWithID", _UserName, DateTime.Now.ToString());
            }
            return sError;
        }

        public static void DeleteRecordBeforeSync(string _DBPath, string MultiId)
        {
            try
            {
                var db = new SQLiteAsyncConnection(_DBPath);
                db.QueryAsync<AppTypeInfoList>("Delete from AppTypeInfoList where APP_TYPE_INFO_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "").Wait();

                //var db2 = new SQLiteAsyncConnection(_DBPath);
                var EDS = db.QueryAsync<EDSResultList>("Select * from EDSResultList where APP_TYPE_INFO_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "");
                EDS.Wait();

                MultiId = string.Join(",", EDS.Result.Select(x => x.EDS_RESULT_ID).ToList().ToArray());
                //var db1 = new SQLiteAsyncConnection(_DBPath);
                db.QueryAsync<EDSResultList>("Delete from EDSResultList where EDS_RESULT_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "").Wait();
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Get CaseList
        public static string GetCaseListSync(bool _IsOnline, string _user, string _CaseTypeID, string _CaseOwnerSAM, string _AssignedToSAM, string _ClosedBySAM, string _CreatedBySAM,
                                                    string _PropertyID, string _TenantCode, string _TenantID, char _showOpenClosedCasesType, char _showPastDueDate, string _SearchQuery, int _InstanceUserAssocId, string _DBPath, string _FullName, string sTitle = "", bool SaveSql = true, string screenName = "")
        {
            List<GetCaseTypesResponse.BasicCase> lstResult = new List<GetCaseTypesResponse.BasicCase>();
            List<KeyValuePair<string, string>> idAndDateTime = new List<KeyValuePair<string, string>>();

            try
            {
                Task<List<AppTypeInfoList>> AppTypeInforesult = DBHelper.GetAppTypeInfoListBySystemName(ConstantsSync.CasesInstance, "E2_GetCaseList" + screenName, _DBPath);
                AppTypeInforesult.Wait();
                if (AppTypeInforesult.Result != null)
                {
                    foreach (var item in AppTypeInforesult.Result)
                    {
                        try
                        {
                            var it = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(item.ASSOC_FIELD_INFO).FirstOrDefault();

                            if (it?.CaseAssignedToSAM?.ToLower() == _user.ToLower())
                                idAndDateTime.Add(new KeyValuePair<string, string>(Convert.ToString(it.CaseID), Convert.ToString(Convert.ToDateTime(it.CaseModifiedDateTime))));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            string ResponseContent = string.Empty;
            JObject result = null;
            try
            {
                result = CasesAPIMethods.GetCaseListSync(_user, _CaseTypeID, _CaseOwnerSAM, _AssignedToSAM, _ClosedBySAM, _CreatedBySAM, idAndDateTime, _PropertyID, _TenantCode, _TenantID, _showOpenClosedCasesType, _showPastDueDate, _SearchQuery, screenName, 0, 0);
                var temp = result.GetValue("ResponseContent");

                //Debug.WriteLine("GetCaseListSync||" + screenName + " ==> " + Convert.ToString(temp));

                if (!string.IsNullOrEmpty(Convert.ToString(temp)) && temp?.ToString() != "[]")
                {
                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(result), "Y", "GetCaseList " + screenName, _user, DateTime.Now.ToString());
                    ResponseContent = Convert.ToString(temp);

                    MasterSyncGetAllCaseType Output = JsonConvert.DeserializeObject<MasterSyncGetAllCaseType>(ResponseContent);
                    var DeleteItem = Output.RemoveItemList;

                    ResponseContent = JsonConvert.SerializeObject(Output.GetAllCaseType);

                    Task.Run(() =>
                    {
                        #region Delete Data Before Master Sync
                        var CaseDate = DBHelper.GetAppTypeInfoListBySystemName(CasesInstance, "E2_GetCaseList" + screenName, _DBPath);
                        CaseDate.Wait();
                        if (CaseDate.Result.Count > 0)
                        {
                            try
                            {
                                var MultiId = string.Join(",", CaseDate.Result.Select(x => x.APP_TYPE_INFO_ID).ToList().ToArray());

                                var db = new SQLiteAsyncConnection(_DBPath);
                                db.QueryAsync<AppTypeInfoList>("Delete from AppTypeInfoList where APP_TYPE_INFO_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "").Wait();

                                //  //var db2 = new SQLiteAsyncConnection(_DBPath);
                                //var EDS = db.QueryAsync<EDSResultList>("Select * from EDSResultList where APP_TYPE_INFO_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "");
                                //EDS.Wait();

                                //MultiId = string.Join(",", EDS.Result.Select(x => x.EDS_RESULT_ID).ToList().ToArray());
                                ////var db1 = new SQLiteAsyncConnection(_DBPath);
                                //db.QueryAsync<EDSResultList>("Delete from EDSResultList where EDS_RESULT_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "").Wait();

                                MultiId = string.Join(",", CaseDate.Result.Select(x => x.ID).ToList().ToArray());
                                // var db2 = new SQLiteAsyncConnection(_DBPath);
                                var Appinf = db.QueryAsync<AppTypeInfoList>("Select * from AppTypeInfoList where TYPE_SCREEN_INFO in ('C8_GetCaseBasicInfo','C4_GetCaseNotes','C10_GetCaseActivity') and ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "");
                                Appinf.Wait();

                                MultiId = string.Join(",", Appinf.Result.Select(x => x.APP_TYPE_INFO_ID).ToList().ToArray());

                                //var db3 = new SQLiteAsyncConnection(_DBPath);
                                db.QueryAsync<AppTypeInfoList>("Delete from AppTypeInfoList where APP_TYPE_INFO_ID in (" + MultiId + ") and INSTANCE_USER_ASSOC_ID=" + ConstantsSync.INSTANCE_USER_ASSOC_ID + "").Wait();



                            }
                            catch (Exception)
                            {
                            }
                        }
                        #endregion
                        CommonConstants.MasterOfflineStore(ResponseContent, _DBPath);
                    });

                }
                else
                {
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(result), "N", "GetCaseList " + screenName, _user, DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetCaseList " + screenName, _user, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(result), "N", "GetCaseList " + screenName, _user, DateTime.Now.ToString());
            }
            return ResponseContent;
        }
        #endregion

        #region Get Origination CenterForUser
        public static string GetOriginationCenterForUserSync(string _User, string _ShowAll, int _InstanceUserAssocId, string _DBPath)
        {
            string sError = string.Empty;
            JObject result = null;
            List<OriginationCenterDataResponse.OriginationCenterData> lstResult = new List<OriginationCenterDataResponse.OriginationCenterData>();
            int id = CommonConstants.GetResultBySytemcode(CasesInstance, "C1_GetOriginationCenterForUser", _DBPath);

            try
            {
                result = CasesAPIMethods.GetOriginationCenterForUser(_User, _ShowAll);

                if (result != null)
                {
                    var ResponseContent = result.GetValue("ResponseContent");

                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(result), "Y", "GetOriginationCenterForUserSync", _User, DateTime.Now.ToString());

                    if (!string.IsNullOrEmpty(Convert.ToString(ResponseContent)) && Convert.ToString(ResponseContent) != "[]" && Convert.ToString(ResponseContent) != "{}" && Convert.ToString(ResponseContent) != "[ ]" && Convert.ToString(ResponseContent) != "{ }" && Convert.ToString(ResponseContent) != "[{ }]" && Convert.ToString(ResponseContent) != "[{}]")
                    {
                        sError = ResponseContent.ToString();
                        lstResult = JsonConvert.DeserializeObject<List<OriginationCenterDataResponse.OriginationCenterData>>(ResponseContent.ToString());
                        if (lstResult.Count > 0)
                        {
                            Task.Run(() =>
                            {
                                #region Delete Data Before Master Sync
                                var CaseDate = DBHelper.GetAppTypeInfoListBySystemName(CasesInstance, "C1_GetOriginationCenterForUser", _DBPath);
                                CaseDate.Wait();
                                foreach (var item in CaseDate.Result)
                                {
                                    DBHelper.DeleteAppTypeInfoListById(item, _DBPath).Wait();
                                }
                                #endregion

                                CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C1_GetOriginationCenterForUser", INSTANCE_USER_ASSOC_ID, _DBPath, 0, "", "M").Wait();
                            });
                        }
                    }
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(result), "N", "GetOriginationCenterForUserSync", _User, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetOriginationCenterForUserSync", _User, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(result), "N", "GetOriginationCenterForUserSync", _User, DateTime.Now.ToString());

            }
            return sError;
        }

        public static async Task<List<OriginationCenterDataResponse.OriginationCenterData>> GetOriginationCenterForUseragain(string _User, string _ShowAll, int _InstanceUserAssocId, string _DBPath)
        {
            List<OriginationCenterDataResponse.OriginationCenterData> lstResult = new List<OriginationCenterDataResponse.OriginationCenterData>();
            int id = CommonConstants.GetResultBySytemcode(CasesInstance, "C1_GetOriginationCenterForUser", _DBPath);

            try
            {
                var result = CasesAPIMethods.GetOriginationCenterForUser(_User, _ShowAll);
                var temp = result.GetValue("ResponseContent");

                if (temp != null && temp.ToString() != "[]")
                {
                    lstResult = JsonConvert.DeserializeObject<List<OriginationCenterDataResponse.OriginationCenterData>>(temp.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }

        public static async Task<List<OriginationCenterDataResponse.OriginationCenterData>> GetOriginationCenterForUser(string _User, string _DBPath)
        {
            List<OriginationCenterDataResponse.OriginationCenterData> lstResult = new List<OriginationCenterDataResponse.OriginationCenterData>();

            try
            {
                var GetResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(CasesInstance, "C1_GetOriginationCenterForUser", _DBPath);
                GetResult.Wait();
                lstResult = GetResult.Result?.ASSOC_FIELD_INFO == null ? new List<OriginationCenterDataResponse.OriginationCenterData>() : JsonConvert.DeserializeObject<List<OriginationCenterDataResponse.OriginationCenterData>>(GetResult.Result.ASSOC_FIELD_INFO.ToString());

                // lstResult = CommonConstants.ReturnListResult<OriginationCenterDataResponse.OriginationCenterData>(CasesInstance, "C1_GetOriginationCenterForUser", _DBPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get All Employee User
        public static string GetAllEmployeeUser(string _DBPath, string pUserSAM)
        {
            string sError = string.Empty;
            JObject result = null;
            List<GetUserInfoResponse.UserInfo> lstResult = new List<GetUserInfoResponse.UserInfo>();
            int id = CommonConstants.GetResultBySytemcode(UserDetailsInstance, "U1_GetAllEmployeeUser", _DBPath);
            try
            {
                result = CasesAPIMethods.GetAllEmployeeUser();

                if (result != null)
                {
                    var ResponseContent = result.GetValue("ResponseContent");

                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(result), "Y", "GetAllEmployeeUser", pUserSAM, DateTime.Now.ToString());

                    if (!string.IsNullOrEmpty(Convert.ToString(ResponseContent)) && Convert.ToString(ResponseContent) != "[]" && Convert.ToString(ResponseContent) != "{}" && Convert.ToString(ResponseContent) != "[ ]" && Convert.ToString(ResponseContent) != "{ }" && Convert.ToString(ResponseContent) != "[{ }]" && Convert.ToString(ResponseContent) != "[{}]")
                    {
                        sError = Convert.ToString(ResponseContent);
                        lstResult = JsonConvert.DeserializeObject<List<GetUserInfoResponse.UserInfo>>(ResponseContent.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), UserDetailsInstance, "U1_GetAllEmployeeUser", INSTANCE_USER_ASSOC_ID, _DBPath, id
                                , "", "M");
                        }
                    }
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(result), "N", "GetAllEmployeeUser", pUserSAM, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAllEmployeeUser", pUserSAM, DateTime.Now.ToString());

                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(result), "N", "GetAllEmployeeUser", pUserSAM, DateTime.Now.ToString());
            }
            return sError;
        }
        #endregion

        #region Favorite Offline Sync
        public static async Task<List<GetFavoriteResponse.GetFavorite>> FavoriteOfflineSync(bool _IsOnline, string _CreatedBy, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetFavoriteResponse.GetFavorite> lstResult = new List<GetFavoriteResponse.GetFavorite>();

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetFavorite(Convert.ToString(_CreatedBy));
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetFavoriteResponse.GetFavorite>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            foreach (var item in lstResult)
                            {
                                var inserted = CommonConstants.FavoriteOfflineStore(0, Convert.ToString(item.TypeID), Convert.ToString(item.QuestAreaID), item.FavoriteName, item.FieldValues, item.IsActive, item.CreatedBy, Convert.ToString(item.CreatedDateTime), Convert.ToString(item.ModifiedDateTime), Convert.ToString(item.ApplicationID), Convert.ToString(item.LastSyncDateTime), Convert.ToString(_InstanceUserAssocId), _DBPath);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Assign Controls
        public static async Task<List<spi_MobileApp_GetTypesByCaseTypeResult>> AssignControlsAsync(bool _IsOnline, int _caseTypeID, string _DBPath, string username)
        {
            List<spi_MobileApp_GetTypesByCaseTypeResult> lstResult = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
            List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LISTFREQUENT> lstExd = new List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LISTFREQUENT>();
            spi_MobileApp_GetTypesByCaseTypeResult getypeResult = new spi_MobileApp_GetTypesByCaseTypeResult();
            int id = CommonConstants.GetResultBySytemcode(CasesInstance, C8_GetTypesByCaseTypeIDRaw, _DBPath);
            try
            {
                //if (_IsOnline)
                //{
                //    var result = CasesAPIMethods.GetTypesByCaseTypeIDRaw(Convert.ToString(_caseTypeID), username);
                //    var temp = result.GetValue("ResponseContent");

                //    if (temp != null && temp.ToString() != "[]")
                //    {
                //        lstResult = JsonConvert.DeserializeObject<List<spi_MobileApp_GetTypesByCaseTypeResult>>(temp.ToString());
                //    }
                //}
                //else
                {
                    var GetAppTypeInfo = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(CasesInstance, "C1_C2_CASES_CASETYPELIST", _caseTypeID, _DBPath, null);
                    GetAppTypeInfo.Wait();
                    if (!string.IsNullOrEmpty(GetAppTypeInfo?.Result?.ASSOC_FIELD_INFO))
                    {
                        lstResult = JsonConvert.DeserializeObject<List<spi_MobileApp_GetTypesByCaseTypeResult>>(GetAppTypeInfo.Result.ASSOC_FIELD_INFO);
                    }
                    else
                    {
                        if (_IsOnline)
                        {
                            var result = CasesAPIMethods.GetTypesByCaseTypeIDRaw(Convert.ToString(_caseTypeID), username);
                            var temp = result.GetValue("ResponseContent");

                            if (temp != null && temp.ToString() != "[]")
                            {
                                lstResult = JsonConvert.DeserializeObject<List<spi_MobileApp_GetTypesByCaseTypeResult>>(temp.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Add Case Notes
        public static async void AddCasNotes(bool _IsOnline, int _caseTypeID, string _caseID, string _Casenotes, string _UserName, string _Notestypeid, string _DBPath, string FullName)
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, "C1_C2_CASES_CASETYPELIST", _caseTypeID, _DBPath);


                AddCaseNotesRequest AddCaseNote = new AddCaseNotesRequest();
                AddCaseNote.noteTypeId = Convert.ToInt32(_Notestypeid);
                AddCaseNote.note = _Casenotes;
                AddCaseNote.isActive = 'Y';
                AddCaseNote.forceDateTime = DateTime.Now;
                AddCaseNote.currentUser = _UserName;
                AddCaseNote.createDateTime = DateTime.Now;
                AddCaseNote.caseID = Convert.ToInt32(_caseID);
                AddCaseNote.currentUserFullName = FullName;

                if (_IsOnline)
                {
                    var result = CasesAPIMethods.AddCaseNotes(AddCaseNote);
                    var temp = result.GetValue("ResponseContent");

                    //var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, AddCaseNote, Constants.AddCaseNotes, ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), Convert.ToInt32(_caseID), Convert.ToInt32(_caseID), _DBPath, CasesInstance, id, "C4_AddNotes");
                }
                else
                {
                    var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, AddCaseNote, Constants.AddCaseNotes, ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), 0, Convert.ToInt32(_caseID), _DBPath, ConstantsSync.CasesInstance, id);
                }
                SaveViewJsonSqlite(AddCaseNote, "C4_AddNotes", _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, CasesInstance, _IsOnline, _IsOnline ? "M" : "T");
            }
            catch (Exception)
            {


            }
        }
        #endregion

        #region Assign Case
        public static async void storeAssignCase(bool _IsOnline, object objassign, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string UserFullName = "", string Screenname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objassign as GetUserInfoResponse.UserInfo;

                AssignCaseRequest ac = new AssignCaseRequest();
                ac.CaseId = Convert.ToInt32(_caseID);
                ac.newCaseOwner = objtemp.SAMName;
                ac.username = _UserName;


                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.AssignCase(ac);
                    temp = result.GetValue("ResponseContent");
                }
                else
                {
                    var view = objview as GetCaseTypesResponse.BasicCase;
                    view.CaseAssignedToSAM = objtemp.SAMName;
                    view.CaseAssignedToDisplayName = objtemp.DisplayName;
                    view.CaseAssignDateTime = DateTime.Now.ToString();
                    view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                    view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? UserFullName : view.CaseCreatedDisplayName;
                    view.CaseModifiedDateTime = DateTime.Now.ToString();
                    view.CaseID = Convert.ToInt32(_caseID);
                    view.CaseOwnerDisplayName = view.CaseOwnerDisplayName == "" ? UserFullName : view.CaseOwnerDisplayName;
                    view.CaseOwnerDateTime = DateTime.Now.ToString();
                    view.CaseModifiedByDisplayName = UserFullName;

                    string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[3]);

                    storeCommonRibbon<AssignCaseRequest>(_IsOnline, ac, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.AssignCase, Screenname);

                    ChangeFooterValues(_caseID, _caseTypeID, _DBPath, Screenname, objtemp.SAMName, objtemp.DisplayName);
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Approved & Assign Case
        public static async void storeApprovedAssignCase(bool _IsOnline, object objassign, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string notes = "", string FullName = "", string Screenname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objassign as GetUserInfoResponse.UserInfo;

                ApproveCaseRequest ac = new ApproveCaseRequest();
                ac.CaseId = Convert.ToInt32(_caseID);
                ac.newCaseOwner = objtemp?.SAMName;
                ac.username = _UserName;
                //ac.caseNote = notes;


                if (!_IsOnline)
                {
                    AddCaseNotesRequest AddCaseNote = new AddCaseNotesRequest();
                    AddCaseNote.noteTypeId = 18;
                    AddCaseNote.note = "Approved";
                    AddCaseNote.isActive = 'Y';
                    AddCaseNote.forceDateTime = DateTime.Now;
                    AddCaseNote.currentUser = _UserName;
                    AddCaseNote.createDateTime = DateTime.Now;
                    AddCaseNote.caseID = Convert.ToInt32(_caseID);
                    AddCaseNote.currentUserFullName = FullName;

                    SaveViewJsonSqlite(AddCaseNote, "C4_AddNotes", _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, CasesInstance);
                }




                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.AssignCase(ac);
                    temp = result.GetValue("ResponseContent");
                }
                else
                {
                    var view = objview as GetCaseTypesResponse.BasicCase;
                    view.CaseAssignedToSAM = objtemp.SAMName;
                    view.CaseAssignedToDisplayName = objtemp.DisplayName;
                    view.CaseAssignDateTime = DateTime.Now.ToString();
                    view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                    view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? FullName : view.CaseCreatedDisplayName;
                    view.CaseModifiedDateTime = DateTime.Now.ToString();
                    view.CaseID = Convert.ToInt32(_caseID);
                    view.CaseOwnerDisplayName = view.CaseOwnerDisplayName == "" ? FullName : view.CaseOwnerDisplayName;
                    view.CaseOwnerDateTime = DateTime.Now.ToString();
                    view.CaseModifiedByDisplayName = FullName;
                    view.CaseModifiedBySAM = _UserName;

                    string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[15]);

                    string MethodName = string.Empty;

                    if (typescreninfo == "C4_ApprovedAssignCase")
                    {
                        MethodName = Constants.ApproveandAssign;
                    }
                    else if (typescreninfo == "C4_AssignCase")
                    {
                        MethodName = Constants.AssignCase;
                    }

                    storeCommonRibbon<ApproveCaseRequest>(_IsOnline, ac, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, MethodName, Screenname);

                    ChangeFooterValues(_caseID, _caseTypeID, _DBPath, Screenname, objtemp.SAMName, objtemp.DisplayName);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        static void ChangeFooterValues(string _caseID, int _caseTypeID, string _DBPath, string Screenname, string SAMName, string DisplayName)
        {
            try
            {
                var Record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "C8_GetCaseBasicInfo", _DBPath, null);
                Record.Wait();
                if (Record.Result != null)
                {
                    var res = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(Record.Result.ASSOC_FIELD_INFO.ToString());

                    var ress = res.Where(av => av.CaseID == Convert.ToInt32(_caseID) && av.CaseTypeID == Convert.ToInt32(_caseTypeID)).Select(v =>
                    {
                        v.CaseAssignedTo = SAMName;
                        v.CaseAssignedToDisplayName = DisplayName;
                        v.ModifiedDateTime = DateTime.Now;
                        v.CaseAssignedDateTime = DateTime.Now;
                        return v;
                    });


                    SaveViewJsonSqlite(ress, "C8_GetCaseBasicInfo", _DBPath, Convert.ToString(_caseTypeID), _caseID, Record.Result.APP_TYPE_INFO_ID, "", 0, CasesInstance, Record.Result.IS_ONLINE, Record.Result.TransactionType, null);
                }
            }
            catch (Exception)
            {
            }
        }

        #region Decline & Assign Case
        public static async void storeDeclineAssignCase(bool _IsOnline, object objassign, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string notes = "", string FullName = "", string Scrname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objassign as GetUserInfoResponse.UserInfo;

                DeclineAndAssignToRequest ac = new DeclineAndAssignToRequest();
                ac.caseId = Convert.ToInt32(_caseID);
                ac.userName = _UserName;
                //ac.notes = notes;
                ac.newUserName = objtemp.SAMName;

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.DeclienAndAssign(ac);
                    temp = result.GetValue("ResponseContent");
                }
                else
                {
                    AddCaseNotesRequest AddCaseNote = new AddCaseNotesRequest();
                    AddCaseNote.noteTypeId = 18;
                    AddCaseNote.note = "Decline";
                    AddCaseNote.isActive = 'Y';
                    AddCaseNote.forceDateTime = DateTime.Now;
                    AddCaseNote.currentUser = _UserName;
                    AddCaseNote.createDateTime = DateTime.Now;
                    AddCaseNote.caseID = Convert.ToInt32(_caseID);
                    AddCaseNote.currentUserFullName = FullName;

                    SaveViewJsonSqlite(AddCaseNote, "C4_AddNotes", _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, CasesInstance);

                    var view = objview as GetCaseTypesResponse.BasicCase;
                    view.CaseAssignedToSAM = objtemp.SAMName;
                    view.CaseAssignedToDisplayName = objtemp.DisplayName;
                    view.CaseAssignDateTime = DateTime.Now.ToString();
                    view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                    view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? _UserName : view.CaseCreatedDisplayName;
                    view.CaseModifiedDateTime = DateTime.Now.ToString();
                    view.CaseID = Convert.ToInt32(_caseID);
                    view.CaseOwnerDisplayName = view.CaseOwnerDisplayName == "" ? _UserName : view.CaseOwnerDisplayName;
                    //view.CaseOwnerDateTime = DateTime.Now.ToString();
                    view.CaseModifiedByDisplayName = FullName;

                    string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[17]);

                    storeCommonRibbon<DeclineAndAssignToRequest>(_IsOnline, ac, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.DeclienAndAssign, Scrname);

                    ChangeFooterValues(_caseID, _caseTypeID, _DBPath, Scrname, objtemp.SAMName, objtemp.DisplayName);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Assign Case  storeAssignCaseold
        /* public static async void storeAssignCaseold(bool _IsOnline, object objassign, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "")
         {
             try
             {
                 int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                 var objtemp = objassign as GetUserInfoResponse.UserInfo;

                 AssignCaseRequest ac = new AssignCaseRequest();
                 ac.CaseId = Convert.ToInt32(_caseID);
                 ac.newCaseOwner = objtemp.DisplayName;
                 ac.username = _UserName;


                 var view = objview as GetCaseTypesResponse.BasicCase;
                 view.CaseAssignedToSAM = objtemp.SAMName;
                 view.CaseAssignedToDisplayName = objtemp.DisplayName;
                 //view.CaseOwnerSAM = _UserName;
                 view.CaseAssignDateTime = DateTime.Now.ToString();
                 view.CaseCreatedSAM = _UserName;
                 view.CaseCreatedDisplayName = _UserName;
                 view.CaseModifiedDateTime = DateTime.Now.ToString();
                 view.CaseID = Convert.ToInt32(_caseID);
                 view.CaseOwnerDisplayName = _UserName;
                 view.CaseOwnerDateTime = DateTime.Now.ToString();


                 if (_IsOnline)
                 {
                     var result = CasesAPIMethods.AssignCase(ac);
                     var temp = result.GetValue("ResponseContent");

                     var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, ac, Constants.AssignCase, ((Enum.GetNames(typeof(ActionTypes)))[0] + "," + (Enum.GetNames(typeof(ActionTypes)))[3]), Convert.ToInt32(_caseID), Convert.ToInt32(_caseID), _DBPath, instanceName, id, typescreninfo);
                 }
                 else
                 {
                     var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, ac, Constants.AssignCase, ((Enum.GetNames(typeof(ActionTypes)))[0] + "," + (Enum.GetNames(typeof(ActionTypes)))[3]), 0, Convert.ToInt32(_caseID), _DBPath, instanceName, id);
                     //SaveViewJsonSqlite(objtemp, typescreninfo, _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, instanceName);
                     int pkId = 0;
                     Task<AppTypeInfoList> record = null;

                     record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "E2_GetCaseList", _DBPath, tm_uname);
                     record.Wait();
                     if (record?.Result == null)
                     {
                         record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "C8_GetCaseBasicInfo", _DBPath);
                         record.Wait();
                         pkId = record.Result.APP_TYPE_INFO_ID;
                     }
                     else
                         pkId = record.Result.APP_TYPE_INFO_ID;


                     SaveViewJsonSqlite(objview, "E2_GetCaseList", _DBPath, Convert.ToString(_caseTypeID), record?.Result?.TransactionType?.ToUpper() == "M" ? Convert.ToString(record?.Result?.ID) : Convert.ToString(insertedRecordid), pkId, _CaseTitle, 0, instanceName);
                 }
             }
             catch (Exception)
             {


             }
         }*/
        #endregion

        #region store Return To Last Assignee
        public static async void storeReturnToLastAssignee(bool _IsOnline, object objReturnToLastAssignee, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string UserFullName = "", string Scrname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objReturnToLastAssignee as ReturnCaseToLastAssigneeRequest;

                ReturnCaseToLastAssigneeRequest ac = new ReturnCaseToLastAssigneeRequest();
                ac.caseID = Convert.ToInt32(_caseID);
                ac.username = _UserName;


                var view = objview as GetCaseTypesResponse.BasicCase;
                view.CaseAssignedToSAM = objtemp.username;
                view.CaseAssignedToDisplayName = UserFullName;
                view.CaseAssignDateTime = DateTime.Now.ToString();
                view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? UserFullName : view.CaseCreatedDisplayName;
                view.CaseModifiedDateTime = DateTime.Now.ToString();
                view.CaseModifiedBySAM = _UserName;
                view.CaseID = Convert.ToInt32(_caseID);
                //view.CaseOwnerDisplayName = view.CaseOwnerDisplayName==""? _UserName: view.CaseOwnerDisplayName;
                //view.CaseOwnerDateTime = DateTime.Now.ToString();

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.ReturnCaseToLastAssignee(ac);
                    temp = result.GetValue("ResponseContent");
                }

                string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[13]);

                storeCommonRibbon<ReturnCaseToLastAssigneeRequest>(_IsOnline, objReturnToLastAssignee, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.ReturnCaseToLastAssignee, Scrname);
            }
            catch (Exception)
            {

            }

        }
        #endregion

        #region store Approve and Return
        public static async void storeApproveandReturn(bool _IsOnline, object objReturnToLastAssignee, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", string CaseNote = "", object objview = null, string _CaseTitle = "", string FullName = "", string Scrname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objReturnToLastAssignee as ReturnCaseToLastAssigneeRequest;

                DeclineAndReturnRequest ac = new DeclineAndReturnRequest();
                ac.caseid = Convert.ToInt32(_caseID);
                ac.Username = _UserName;
                // ac.notes = "Approved";

                if (!_IsOnline)
                {
                    AddCaseNotesRequest AddCaseNote = new AddCaseNotesRequest();
                    AddCaseNote.noteTypeId = 18;
                    AddCaseNote.note = "Approved";
                    AddCaseNote.isActive = 'Y';
                    AddCaseNote.forceDateTime = DateTime.Now;
                    AddCaseNote.currentUser = _UserName;
                    AddCaseNote.createDateTime = DateTime.Now;
                    AddCaseNote.caseID = Convert.ToInt32(_caseID);
                    AddCaseNote.currentUserFullName = FullName;

                    SaveViewJsonSqlite(AddCaseNote, "C4_AddNotes", _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, CasesInstance);
                }


                var view = objview as GetCaseTypesResponse.BasicCase;
                view.CaseAssignedToSAM = objtemp.username;
                view.CaseAssignedToDisplayName = FullName;
                view.CaseAssignDateTime = DateTime.Now.ToString();
                view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? FullName : view.CaseCreatedDisplayName;
                view.CaseModifiedDateTime = DateTime.Now.ToString();
                view.CaseID = Convert.ToInt32(_caseID);
                //view.CaseOwnerDisplayName = FullName;
                //view.CaseOwnerDateTime = DateTime.Now.ToString();

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.ApproveandReturn(ac);
                    temp = result.GetValue("ResponseContent");
                }

                string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[12]);

                storeCommonRibbon<DeclineAndReturnRequest>(_IsOnline, objReturnToLastAssignee, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.ApproveandReturn, Scrname);

            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region store Return To Last Assigner
        public static async void storeReturnToLastAssigner(bool _IsOnline, object objReturnCaseToLastAssigner, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string UserFullName = "", string Scrname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objReturnCaseToLastAssigner as ReturnCaseToLastAssignerRequest;

                ReturnCaseToLastAssignerRequest ac = new ReturnCaseToLastAssignerRequest();
                ac.caseid = Convert.ToInt32(_caseID);
                ac.username = _UserName;


                var view = objview as GetCaseTypesResponse.BasicCase;
                view.CaseAssignedToSAM = objtemp.username;
                view.CaseAssignedToDisplayName = UserFullName;
                view.CaseAssignDateTime = DateTime.Now.ToString();
                view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? _UserName : view.CaseCreatedDisplayName;
                view.CaseModifiedDateTime = DateTime.Now.ToString();
                view.CaseID = Convert.ToInt32(_caseID);
                //view.CaseOwnerDisplayName = _UserName;
                //view.CaseOwnerDateTime = DateTime.Now.ToString();

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.ReturnCaseToLastAssigner(ac);
                    temp = result.GetValue("ResponseContent");
                }

                string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[14]);

                storeCommonRibbon<ReturnCaseToLastAssignerRequest>(_IsOnline, objReturnCaseToLastAssigner, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.ReturnCaseToLastAssigner, Scrname);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region store Decline And Return
        public static async void storeDeclineAndReturn(bool _IsOnline, object objDeclineAndReturn, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string FullName = "", string Scrname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objDeclineAndReturn as DeclineAndReturnRequest;

                DeclineAndReturnRequest ac = new DeclineAndReturnRequest();
                ac.caseid = Convert.ToInt32(_caseID);
                ac.Username = _UserName;
                ac.notes = objtemp.notes;

                if (!_IsOnline)
                {
                    AddCaseNotesRequest AddCaseNote = new AddCaseNotesRequest();
                    AddCaseNote.noteTypeId = 18;
                    AddCaseNote.note = "Decline";
                    AddCaseNote.isActive = 'Y';
                    AddCaseNote.forceDateTime = DateTime.Now;
                    AddCaseNote.currentUser = _UserName;
                    AddCaseNote.createDateTime = DateTime.Now;
                    AddCaseNote.caseID = Convert.ToInt32(_caseID);
                    AddCaseNote.currentUserFullName = FullName;

                    SaveViewJsonSqlite(AddCaseNote, "C4_AddNotes", _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, CasesInstance);
                }

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.DeclienAndReturn(ac);
                    temp = result.GetValue("ResponseContent");
                }
                else
                {
                    var view = objview as GetCaseTypesResponse.BasicCase;
                    view.CaseAssignedToSAM = objtemp.Username;
                    view.CaseAssignedToDisplayName = FullName;
                    view.CaseAssignDateTime = DateTime.Now.ToString();
                    view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                    view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? FullName : view.CaseCreatedDisplayName;
                    view.CaseModifiedDateTime = DateTime.Now.ToString();
                    view.CaseID = Convert.ToInt32(_caseID);
                    view.CaseOwnerDisplayName = view.CaseOwnerDisplayName == "" ? FullName : view.CaseOwnerDisplayName;
                    //view.CaseOwnerDateTime = DateTime.Now.ToString();
                    //view.CaseModifiedByDisplayName = FullName;

                    string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[16]);

                    storeCommonRibbon<DeclineAndReturnRequest>(_IsOnline, objDeclineAndReturn, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.DeclineAndReturn, Scrname);
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region store Close
        public static async void storeClose(bool _IsOnline, object objClose, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string FullName = "", string Scrname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objClose as CloseCaseRequest;

                CloseCaseRequest ac = new CloseCaseRequest();
                ac.caseID = Convert.ToInt32(_caseID);
                ac.user = _UserName;
                ac.userFullName = objtemp.userFullName;
                //Ple
                var view = objview as GetCaseTypesResponse.BasicCase;
                view.CaseAssignedToSAM = objtemp.user;
                view.CaseAssignedToDisplayName = objtemp.userFullName;
                view.CaseAssignDateTime = DateTime.Now.ToString();
                view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? FullName : view.CaseCreatedDisplayName;
                view.CaseModifiedDateTime = DateTime.Now.ToString();
                view.CaseModifiedByDisplayName = FullName;
                view.CaseID = Convert.ToInt32(_caseID);
                //view.CaseOwnerDisplayName = FullName;
                //view.CaseOwnerDateTime = DateTime.Now.ToString();

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.CloseCase(ac);
                    temp = result.GetValue("ResponseContent");
                }

                string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[8] + "," + (Enum.GetNames(typeof(ActionTypes)))[18]);

                storeCommonRibbon<CloseCaseRequest>(_IsOnline, objClose, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.CloseCase, Scrname);

                var Response = DBHelper.GetAppTypeInfo_tmname(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "E2_GetCaseList" + Scrname, _DBPath);
                Response.Wait();
                if (Response.Result != null)
                {
                    var Cur_Obj = Response.Result;
                    Cur_Obj.TYPE_SCREEN_INFO = "E2_GetCaseList";

                    DBHelper.SaveAppTypeInfo(Cur_Obj, _DBPath).Wait();
                }


            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region Take Owership Case
        public static async void storeTakeOwershipCase(bool _IsOnline, object objTakeOwnership, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "", string FullName = "", string Screenname = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objTakeOwnership as AcceptCaseRequest;

                AcceptCaseRequest ac = new AcceptCaseRequest();
                ac.CaseId = Convert.ToInt32(_caseID);
                ac.caseOwnerSAM = objtemp.caseOwnerSAM;
                ac.username = _UserName;


                var view = objview as GetCaseTypesResponse.BasicCase;
                view.CaseAssignedToSAM = objtemp.username;
                view.CaseAssignedToDisplayName = view.CaseAssignedToDisplayName;
                view.CaseOwnerSAM = _UserName;
                view.CaseAssignDateTime = DateTime.Now.ToString();
                view.CaseCreatedSAM = view.CaseCreatedSAM == "" ? _UserName : view.CaseCreatedSAM;
                view.CaseCreatedDisplayName = view.CaseCreatedDisplayName == "" ? FullName : view.CaseCreatedDisplayName;
                view.CaseModifiedDateTime = DateTime.Now.ToString();
                view.CaseModifiedBySAM = _UserName;
                view.CaseID = Convert.ToInt32(_caseID);
                view.CaseOwnerDisplayName = FullName;
                view.CaseOwnerDateTime = DateTime.Now.ToString();

                JToken temp = null;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.AcceptCase(ac);
                    temp = result.GetValue("ResponseContent");
                }

                string sEventNames = ((Enum.GetNames(typeof(ActionTypes)))[0] + "," + (Enum.GetNames(typeof(ActionTypes)))[11]);

                storeCommonRibbon<AcceptCaseRequest>(_IsOnline, objTakeOwnership, _caseTypeID, _caseID, _UserName, _DBPath, ac, systemscreenName, typescreninfo, CasesInstance, _CaseTitle, view, temp, sEventNames, Constants.AcceptCase, Screenname);


            }
            catch (Exception)
            {
            }

        }
        #endregion

        #region Take Owership Case
        /*
        public static async void storeTakeOwershipCaseOld(bool _IsOnline, object objTakeOwnership, int _caseTypeID, string _caseID, string _UserName, string _DBPath, string systemscreenName = "", string typescreninfo = "", string instanceName = "", object objview = null, string _CaseTitle = "")
        {
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

                var objtemp = objTakeOwnership as AcceptCaseRequest;

                AcceptCaseRequest ac = new AcceptCaseRequest();
                ac.CaseId = Convert.ToInt32(_caseID);
                ac.caseOwnerSAM = objtemp.caseOwnerSAM;
                ac.username = _UserName;


                var view = objview as GetCaseTypesResponse.BasicCase;
                view.CaseAssignedToSAM = objtemp.username;
                view.CaseAssignedToDisplayName = objtemp.username;
                //view.CaseOwnerSAM = _UserName;
                view.CaseAssignDateTime = DateTime.Now.ToString();
                view.CaseCreatedSAM = _UserName;
                view.CaseCreatedDisplayName = _UserName;
                view.CaseModifiedDateTime = DateTime.Now.ToString();
                view.CaseID = Convert.ToInt32(_caseID);
                view.CaseOwnerDisplayName = _UserName;
                view.CaseOwnerDateTime = DateTime.Now.ToString();


                if (_IsOnline)
                {
                    var result = CasesAPIMethods.AcceptCase(ac);
                    var temp = result.GetValue("ResponseContent");

                    var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, ac, Constants.TakeOwnership, ((Enum.GetNames(typeof(ActionTypes)))[0] + "," + (Enum.GetNames(typeof(ActionTypes)))[11]), Convert.ToInt32(_caseID), Convert.ToInt32(_caseID), _DBPath, instanceName, id, typescreninfo);
                }
                else
                {
                    var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, ac, Constants.TakeOwnership, ((Enum.GetNames(typeof(ActionTypes)))[0] + "," + (Enum.GetNames(typeof(ActionTypes)))[11]), 0, Convert.ToInt32(_caseID), _DBPath, instanceName, id);
                    //SaveViewJsonSqlite(objtemp, typescreninfo, _DBPath, Convert.ToString(_caseTypeID), _caseID, 0, "", 0, instanceName);
                    int pkId = 0;
                    Task<AppTypeInfoList> record = null;

                    record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "E2_GetCaseList", _DBPath, tm_uname);
                    record.Wait();
                    if (record?.Result == null)
                    {
                        record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "C8_GetCaseBasicInfo", _DBPath, tm_uname);
                        record.Wait();
                        pkId = record.Result.APP_TYPE_INFO_ID;
                    }
                    else
                        pkId = record.Result.APP_TYPE_INFO_ID;


                    SaveViewJsonSqlite(objview, "E2_GetCaseList", _DBPath, Convert.ToString(_caseTypeID), record?.Result?.TransactionType?.ToUpper() == "M" ? Convert.ToString(record?.Result?.ID) : Convert.ToString(insertedRecordid), pkId, _CaseTitle, 0, instanceName);
                }
            }
            catch (Exception)
            {


            }
        }
        */
        #endregion

        #region store Common Ribbon
        public static async void storeCommonRibbon<T>(bool _IsOnline, object objTakeOwnership, int _caseTypeID, string _caseID, string _UserName, string _DBPath, T objJson, string systemscreenName = "", string typescreninfo = "", string instanceName = "", string _CaseTitle = "", GetCaseTypesResponse.BasicCase viewobj = null, JToken temp = null, string sEventName = "", string ApiUrl = "", string Scrname = "")
        {
            int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, systemscreenName, _caseTypeID, _DBPath);

            var tm_uname = DBHelper.GetAppTypeInfo_tmname(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "E2_GetCaseList" + Scrname, _DBPath).Result?.TM_Username;

            try
            {
                if (_IsOnline)
                {
                    if (temp != null && temp.ToString() != "[]")
                    {
                        var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, objJson, ApiUrl, sEventName, Convert.ToInt32(_caseID), Convert.ToInt32(_caseID), _DBPath, instanceName, id, typescreninfo);
                    }
                }
                else
                {
                    var insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, objJson, ApiUrl, sEventName, 0, Convert.ToInt32(_caseID), _DBPath, instanceName, id);

                    int pkId = 0;
                    Task<AppTypeInfoList> record = null;

                    record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "E2_GetCaseList" + Scrname, _DBPath, tm_uname);
                    record.Wait();
                    if (record?.Result == null)
                    {
                        record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseID), _caseTypeID, "E2_GetCaseList" + Scrname, _DBPath, tm_uname);
                        record.Wait();

                        pkId = record.Result.APP_TYPE_INFO_ID;
                    }
                    else
                        pkId = record.Result.APP_TYPE_INFO_ID;

                    List<GetCaseTypesResponse.BasicCase> objlist = new List<GetCaseTypesResponse.BasicCase>();
                    objlist.Add(viewobj);
                    string teamName = string.Empty;

                    if (Scrname == "_AssignedToMe")
                    {
                        // L1_GetTeamMembers
                        var TeamMemberslist = DBHelper.GetAppTypeInfoList(UserProfileInstance, Convert.ToInt32(0), 0, "L1_GetTeamMembers", _DBPath, null);
                        TeamMemberslist.Wait();
                        if (TeamMemberslist?.Result != null)
                        {
                            var TMList = JsonConvert.DeserializeObject<List<GetUserInfoResponse.UserInfo>>(TeamMemberslist?.Result?.ASSOC_FIELD_INFO);

                            var Temamate = TMList.Where(v => v.SAMName == viewobj.CaseAssignedToSAM).Count();
                            if (Temamate > 0)
                            {
                                Scrname = "_AssignedToMyTeam";
                                teamName = viewobj.CaseAssignedToSAM;
                            }
                        }
                        //else
                        //{
                        //    Scrname = "_AssignedToMyTeam";
                        //    teamName = viewobj.CaseAssignedToSAM
                        //}
                    }
                    else if (viewobj.CaseAssignedToSAM?.ToLower() == _UserName?.ToLower())
                    {
                        Scrname = "_AssignedToMe";
                        teamName = _UserName;
                    }


                    SaveViewJsonSqlite(objlist, "E2_GetCaseList" + Scrname, _DBPath, Convert.ToString(_caseTypeID), record?.Result?.TransactionType?.ToUpper() == "M" ? Convert.ToString(record?.Result?.ID) : Convert.ToString(_caseID), pkId, record?.Result.TYPE_NAME, 0, instanceName, Convert.ToBoolean(record?.Result.IS_ONLINE), "T", teamName);

                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        public static async Task<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>> GetTypeValuesByAssocCaseTypeExternalDS(bool _IsOnline, object _Body_value, string _DBPath)
        {
            List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> lstResult = new List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>();

            try
            {
                if (_IsOnline)
                {
                    var Result = CasesAPIMethods.GetTypeValuesByAssocCaseTypeExternalDS(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>>(temp.ToString());
                    }
                }
                return lstResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Store Create Case
        public static async Task<int> StoreAndcreateCase(bool _IsOnline, int _caseTypeID, object _Body_value, string _CaseNotes, string _UserName, string _DBPath, object AssignCase = null, string UserFullName = "", string Notetypeid = "19", string Screemname = "")
        {
            int insertedRecordid = 0;
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, "C1_C2_CASES_CASETYPELIST", _caseTypeID, _DBPath);
                string _CaseTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath).Result?.TYPE_NAME;
                if (_IsOnline)
                {
                    var Result = CasesAPIMethods.CreateCaseOptimized(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_caseTypeID, _Body_value, Constants.CreateCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[0], insertedRecordid, insertedRecordid, _DBPath, CasesInstance, id);



                    if (!string.IsNullOrEmpty(_CaseNotes))
                        AddCasNotes(_IsOnline, _caseTypeID, _IsOnline ? Convert.ToString(insertedRecordid) : Convert.ToString(iteminfoid), _CaseNotes, _UserName, Notetypeid, _DBPath, UserFullName);
                    if (AssignCase != null)
                    {
                        storeAssignCase(_IsOnline, AssignCase, _caseTypeID, Convert.ToString(insertedRecordid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_AssignCase", CasesInstance, null, _CaseTitle, UserFullName);
                    }
                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, _Body_value, Constants.CreateCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[0], 0, insertedRecordid, _DBPath, ConstantsSync.CasesInstance, id);

                    #region Offline View case
                    Task<AppTypeInfoList> GetAppTypeInfo = DBHelper.GetAppTypeInfoListByTypeID_SystemName(_caseTypeID, CasesInstance, "C1_C2_CASES_CASETYPELIST", _DBPath);
                    GetAppTypeInfo.Wait();
                    GetCaseTypesResponse.BasicCase viewCase = new GetCaseTypesResponse.BasicCase();
                    List<GetCaseTypesResponse.BasicCase> viewCaselist = new List<GetCaseTypesResponse.BasicCase>();

                    //if (!string.IsNullOrEmpty(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO))
                    {
                        var temp = (CreateCaseOptimizedRequest.CreateCaseModelOptimized)_Body_value;
                        foreach (var item in temp.textValues)
                        {
                            viewCase.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                            {
                                AssociatedTypeID = item.Key,
                                TextValue = item.Value,
                            });
                        }

                        if (temp?.metaDataValues != null)
                        {
                            foreach (var item in temp?.metaDataValues)
                            {
                                viewCase.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                {
                                    AssociatedTypeID = item.Key,
                                    TextValue = item.Value,
                                });
                            }
                        }

                        viewCase.CaseCreatedDateTime = DateTime.Now.ToString();
                        viewCase.CaseOwnerSAM = _UserName;
                        viewCase.CaseAssignDateTime = DateTime.Now.ToString();
                        viewCase.CaseCreatedSAM = _UserName;
                        viewCase.CaseCreatedDisplayName = UserFullName;
                        viewCase.CaseModifiedDateTime = DateTime.Now.ToString();
                        viewCase.CaseCreatedDateTime = DateTime.Now.ToString();

                        viewCase.CaseOwnerDisplayName = UserFullName;
                        viewCase.CaseOwnerDateTime = DateTime.Now.ToString();
                        viewCase.CaseModifiedByDisplayName = UserFullName;
                        viewCase.CaseTypeID = _caseTypeID;
                        viewCase.CaseTitle = _CaseTitle;
                        viewCase.CaseCreatedSAM = temp.currentUser;
                        viewCase.CaseID = insertedRecordid;
                        viewCaselist.Add(viewCase);


                        var tm_uname = DBHelper.GetAppTypeInfo_tmname(CasesInstance, viewCase.CaseID, viewCase.CaseTypeID, "E2_GetCaseList" + Screemname, _DBPath).Result?.TM_Username;

                        SaveViewJsonSqlite(viewCaselist, "E2_GetCaseList" + Screemname, _DBPath, Convert.ToString(_caseTypeID), Convert.ToString(insertedRecordid), 0, "", 0, CasesInstance, _IsOnline, "M", tm_uname);

                        List<GetCaseTypesResponse.CaseData> lstResultBasicInfo = new List<GetCaseTypesResponse.CaseData>();


                        GetCaseTypesResponse.CaseData ls = new GetCaseTypesResponse.CaseData()
                        {
                            CaseTypeID = viewCaselist.FirstOrDefault().CaseTypeID,
                            CreateBySam = _UserName,
                            CaseOwner = _UserName,
                            CaseOwnerDateTime = DateTime.Now,
                            MetaDataCollection = viewCase.MetaDataCollection,
                            CaseOwnerDisplayName = UserFullName,
                            CaseTypeName = viewCase.CaseTypeName,
                            CreateDateTime = DateTime.Now,
                            CreateByDisplayName = UserFullName,
                            CaseID = viewCase.CaseID
                        };
                        lstResultBasicInfo.Add(ls);

                        if (lstResultBasicInfo.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResultBasicInfo), CasesInstance, "C8_GetCaseBasicInfo", GetAppTypeInfo.Result.INSTANCE_USER_ASSOC_ID, _DBPath, 0, Convert.ToString(_caseTypeID), "T", Convert.ToString(insertedRecordid), 0, "", "", true);
                        }
                    }

                    #endregion

                    if (!string.IsNullOrEmpty(_CaseNotes))
                    {
                        AddCasNotes(_IsOnline, _caseTypeID, Convert.ToString(insertedRecordid), _CaseNotes, _UserName, Notetypeid, _DBPath, UserFullName);
                    }
                    if (AssignCase != null)
                    {
                        storeAssignCase(_IsOnline, AssignCase, _caseTypeID, Convert.ToString(insertedRecordid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_AssignCase", CasesInstance, viewCase, _CaseTitle, UserFullName);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Store Update Case
        public static async Task<int> StoreAndUpdateCase(bool _IsOnline, int _caseTypeID, object _Body_value, string _CaseNotes, string _UserName, string _DBPath, object _createCaseBodyvalue = null, object objAssignCase = null, object objTakeOwershipCase = null, string _Notestypeid = "19", bool isApproveCase = false, bool isDeclineCase = false, string FullName = "", string Screenname = "")
        {
            int insertedRecordid = 0;
            string sMode = string.Empty;
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(CasesInstance, "C1_C2_CASES_CASETYPELIST", _caseTypeID, _DBPath);
                string _CaseTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath).Result?.TYPE_NAME;

                var tempRecord = _Body_value as SaveCaseTypeRequest;

                var tm_uname = DBHelper.GetAppTypeInfo_tmname(CasesInstance, tempRecord.caseId, _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath).Result?.TM_Username;

                if (_IsOnline)
                {
                    var Result = CasesAPIMethods.SaveCaseOptimized(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Convert.ToInt32(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_caseTypeID, _Body_value, Constants.SaveCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[8], insertedRecordid, insertedRecordid, _DBPath, CasesInstance, id);


                    if (!string.IsNullOrEmpty(_CaseNotes) && !isApproveCase && !isDeclineCase)
                        AddCasNotes(_IsOnline, _caseTypeID, _IsOnline ? Convert.ToString(insertedRecordid) : Convert.ToString(iteminfoid), _CaseNotes, _UserName, _Notestypeid, _DBPath, FullName);

                    GetUserInfoResponse.UserInfo Userobj = new GetUserInfoResponse.UserInfo();
                    if (objAssignCase != null)
                    {
                        Userobj = objAssignCase as GetUserInfoResponse.UserInfo;

                        if (isApproveCase)
                        {
                            ApproveCaseRequest ac = new ApproveCaseRequest();
                            ac.CaseId = Convert.ToInt32(insertedRecordid);
                            ac.newCaseOwner = Userobj.SAMName;
                            ac.username = _UserName;
                            ac.caseNote = _CaseNotes;

                            var resultAssign = CasesAPIMethods.ApproveandAssign(ac);
                            var tempAssign = resultAssign.GetValue("ResponseContent");
                            if (tempAssign != null && tempAssign.ToString() != "[]")
                            {

                            }
                        }
                        else if (isDeclineCase)
                        {
                            DeclineAndAssignToRequest ac = new DeclineAndAssignToRequest();
                            ac.caseId = Convert.ToInt32(insertedRecordid);
                            ac.userName = _UserName;
                            ac.notes = _CaseNotes;
                            ac.newUserName = Userobj.SAMName;

                            var resultAssign = CasesAPIMethods.DeclienAndAssign(ac);
                            var tempAssign = resultAssign.GetValue("ResponseContent");
                            if (tempAssign != null && tempAssign.ToString() != "[]")
                            {

                            }
                        }
                        else
                        {
                            AssignCaseRequest ac = new AssignCaseRequest();
                            ac.CaseId = Convert.ToInt32(insertedRecordid);
                            ac.newCaseOwner = Userobj.SAMName;
                            ac.username = _UserName;

                            var resultAssign = CasesAPIMethods.AssignCase(ac);
                            var tempAssign = resultAssign.GetValue("ResponseContent");
                            if (tempAssign != null && tempAssign.ToString() != "[]")
                            {

                            }
                        }
                    }
                    if (objTakeOwershipCase != null)
                    {
                        AcceptCaseRequest obja = objTakeOwershipCase as AcceptCaseRequest;

                        var resultTakeOwership = CasesAPIMethods.AcceptCase(obja);
                        var tempTakeOwership = resultTakeOwership.GetValue("ResponseContent");
                    }

                    SaveCaseTypeRequest objSaveCase = _Body_value as SaveCaseTypeRequest;
                    Task<AppTypeInfoList> GetAppTypeInfo = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(objSaveCase.caseId), _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath, tm_uname);
                    GetAppTypeInfo.Wait();

                    if (GetAppTypeInfo.Result == null)
                    {
                        GetAppTypeInfo = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(objSaveCase.caseId), _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath, null);
                        GetAppTypeInfo.Wait();
                    }

                    List<GetCaseTypesResponse.BasicCase> viewCaseList = new List<GetCaseTypesResponse.BasicCase>();
                    GetCaseTypesResponse.BasicCase viewCase = new GetCaseTypesResponse.BasicCase();
                    if (!string.IsNullOrEmpty(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO))
                    {
                        List<GetCaseTypesResponse.BasicCase> jsonRecordOfln = new List<GetCaseTypesResponse.BasicCase>();
                        try
                        {
                            jsonRecordOfln = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO);
                        }
                        catch
                        {
                            var jsonRecordOflntemp = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO);
                            jsonRecordOfln.Add(jsonRecordOflntemp);
                        }

                        foreach (var item in objSaveCase?.textValues)
                        {
                            viewCase.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                            {
                                AssociatedTypeID = item.Key,
                                TextValue = item.Value,
                            });
                        }

                        if (objSaveCase?.dropDownValues != null)
                        {
                            foreach (var item in objSaveCase?.dropDownValues)
                            {
                                viewCase.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                {
                                    AssociatedTypeID = item.Key,
                                    TextValue = item.Value.ToString(),
                                });
                            }
                        }

                        List<GetCaseTypesResponse.BasicCase> lstResult = new List<GetCaseTypesResponse.BasicCase>();


                        Task<List<AppTypeInfoList>> OnlineList = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), insertedRecordid, _DBPath, "E2_GetCaseList" + Screenname, "M", tm_uname);
                        OnlineList.Wait();
                        Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), insertedRecordid, _DBPath, "E2_GetCaseList" + Screenname, "T", tm_uname);
                        OfflineList.Wait();

                        /*vishalPr*/
                        //Task<List<AppTypeInfoList>> OnlineList1 = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode(CasesInstance, Convert.ToInt32(_caseTypeID), insertedRecordid, _DBPath, "E2_GetCaseList" + Screenname, "M", tm_uname);
                        // OnlineList1.Wait();

                        //Task<List<AppTypeInfoList>> OfflineList1 = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode(CasesInstance, Convert.ToInt32(_caseTypeID), insertedRecordid, _DBPath, "E2_GetCaseList" + Screenname, "T", tm_uname);
                        //OfflineList1.Wait();


                        if (OnlineList.Result.Count() == 0 && OfflineList.Result.Count() == 0)
                        {
                            List<GetCaseTypesResponse.BasicCase> jsonlist = new List<GetCaseTypesResponse.BasicCase>();
                            if (GetAppTypeInfo.Result.ASSOC_FIELD_INFO?.ToString().Trim() != "[]")
                            {
                                List<GetCaseTypesResponse.CaseData> casedata = new List<GetCaseTypesResponse.CaseData>();
                                casedata = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(GetAppTypeInfo.Result.ASSOC_FIELD_INFO);
                                GetCaseTypesResponse.BasicCase json = new GetCaseTypesResponse.BasicCase()
                                {
                                    CaseTitle = casedata.FirstOrDefault().CaseTitle,
                                    CaseCreatedDisplayName = casedata.FirstOrDefault().CreateByDisplayName,
                                    CaseCreatedSAM = casedata.FirstOrDefault().CreateBy == "" ? casedata.FirstOrDefault().CaseOwner : casedata.FirstOrDefault().CreateBy,
                                    ListID = Convert.ToInt32(casedata.FirstOrDefault().ListID),
                                    CaseTypeID = casedata.FirstOrDefault().CaseTypeID,
                                    CaseTypeName = casedata.FirstOrDefault().CaseTypeName,
                                    CaseAssignedToSAM = casedata.FirstOrDefault().CaseAssignedTo,
                                    CaseID = casedata.FirstOrDefault().CaseID,
                                    CaseOwnerSAM = casedata.FirstOrDefault().CaseOwner,
                                    CaseCreatedDateTime = Convert.ToString(casedata.FirstOrDefault().CreateDateTime),
                                    CaseAssignedToDisplayName = casedata.FirstOrDefault().CaseAssignedToDisplayName,
                                    MetaDataCollection = casedata.FirstOrDefault().MetaDataCollection,
                                    CaseModifiedBySAM = casedata.FirstOrDefault().ModifiedBy,
                                    CaseModifiedByDisplayName = casedata.FirstOrDefault().ModifiedByDisplayName,
                                    SecurityType = casedata.FirstOrDefault().SceurityType,
                                };
                                jsonlist.Add(json);

                                var lst = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OnlineList.Result.FirstOrDefault().ASSOC_FIELD_INFO);
                                lst.AddRange(jsonlist);

                                bool flg = false;
                                if (jsonRecordOfln.FindAll(v => v.CaseID == Convert.ToInt32(insertedRecordid))?.Count > 0)
                                {
                                    var vv = lst.Where(v => v.CaseID == insertedRecordid);
                                    var Results = lst.Where(p => !vv.Any(p2 => p2.CaseID == p.CaseID));
                                    if (!flg)
                                    {
                                        foreach (var itm in Results)
                                        {
                                            lstResult.Add(itm);
                                            flg = true;
                                        }
                                    }
                                    var res = vv.Select(v =>
                                    {
                                        v.CaseCreatedSAM = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedSAM == "" ? json.CaseCreatedSAM : jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedSAM;
                                        v.CaseCreatedDisplayName = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedDisplayName == "" ? json.CaseCreatedDisplayName : jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedDisplayName;
                                        v.MetaDataCollection = viewCase.MetaDataCollection;
                                        v.CaseTitle = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseTitle;
                                        v.CaseID = insertedRecordid;
                                        return v;
                                    });
                                    if (lstResult.Where(v => v.CaseID == insertedRecordid)?.Count() == 0)
                                    {
                                        lstResult.AddRange(res);
                                    }
                                    else
                                    {
                                        lstResult.Where(v => v.CaseID == insertedRecordid).Select(av =>
                                        {
                                            av.CaseCreatedSAM = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedSAM;
                                            av.CaseCreatedDisplayName = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedDisplayName;
                                            av.MetaDataCollection = viewCase.MetaDataCollection;
                                            av.CaseTitle = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseTitle;
                                            av.CaseID = insertedRecordid;
                                            return av;
                                        }).OrderByDescending(v => v.CaseID).ToList();
                                    }
                                }

                                SaveViewJsonSqlite(lstResult, "E2_GetCaseList" + Screenname, _DBPath, Convert.ToString(_caseTypeID), Convert.ToString(insertedRecordid), Convert.ToInt32(GetAppTypeInfo.Result.APP_TYPE_INFO_ID), GetAppTypeInfo.Result.TYPE_NAME, 0, CasesInstance, true, "M", Userobj.SAMName);
                            }
                        }
                        else
                        {
                            GetCaseTypesResponse.BasicCase records = new GetCaseTypesResponse.BasicCase();
                            List<GetCaseTypesResponse.BasicCase> json = new List<GetCaseTypesResponse.BasicCase>();
                            GetCaseTypesResponse.BasicCase temps = new GetCaseTypesResponse.BasicCase();
                            if (OnlineList.Result.Count > 0)
                            {
                                json = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OnlineList.Result.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                            }
                            else
                            {
                                List<GetCaseTypesResponse.BasicCase> jsonttt = new List<GetCaseTypesResponse.BasicCase>();
                                try
                                {
                                    var jsontt = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OfflineList.Result.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                                    jsonttt.AddRange(jsontt);
                                }
                                catch
                                {
                                    var jsont = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(OfflineList.Result.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                                    jsonttt.Add(jsont);
                                }
                                json.AddRange(jsonttt);
                            }
                            bool flg = false;
                            if (jsonRecordOfln.FindAll(v => v.CaseID == Convert.ToInt32(insertedRecordid))?.Count > 0)
                            {
                                var vv = json.Where(v => v.CaseID == insertedRecordid);
                                var Results = json.Where(p => !vv.Any(p2 => p2.CaseID == p.CaseID));
                                if (!flg)
                                {
                                    foreach (var itm in Results)
                                    {
                                        lstResult.Add(itm);
                                        flg = true;
                                    }
                                }
                                var res = vv.Select(v =>
                                {
                                    v.CaseCreatedSAM = string.IsNullOrEmpty(json.Where(av => av.CaseID == insertedRecordid).FirstOrDefault().CaseCreatedSAM) ? jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedSAM : json.Where(av => av.CaseID == insertedRecordid).FirstOrDefault().CaseCreatedSAM;
                                    v.CaseCreatedDisplayName = string.IsNullOrEmpty(json.Where(av => av.CaseID == insertedRecordid).FirstOrDefault().CaseCreatedDisplayName) ? jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedDisplayName : json.Where(av => av.CaseID == insertedRecordid).FirstOrDefault().CaseCreatedDisplayName;
                                    v.MetaDataCollection = viewCase.MetaDataCollection;
                                    v.CaseTitle = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseTitle;
                                    v.CaseID = insertedRecordid;
                                    return v;
                                });
                                if (lstResult.Where(v => v.CaseID == insertedRecordid)?.Count() == 0)
                                {
                                    lstResult.AddRange(res);
                                }
                                else
                                {
                                    lstResult.Where(v => v.CaseID == insertedRecordid).Select(av =>
                                    {
                                        av.CaseCreatedSAM = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedSAM;
                                        av.CaseCreatedDisplayName = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseCreatedDisplayName;
                                        av.MetaDataCollection = viewCase.MetaDataCollection;
                                        av.CaseTitle = jsonRecordOfln.Where(vvv => vvv.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault().CaseTitle;
                                        av.CaseID = insertedRecordid;
                                        return av;
                                    }).OrderByDescending(v => v.CaseID).ToList();
                                }
                            }

                            /*vishalPr*/
                            ////int pkId = 0;
                            //var record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(objSaveCase.caseId), _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath, tm_uname);
                            //record.Wait();
                            //if (record?.Result == null)
                            //{
                            //    record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(objSaveCase.caseId), _caseTypeID, "C8_GetCaseBasicInfo", _DBPath, null);
                            //    record.Wait();
                            //    pkId = record.Result.APP_TYPE_INFO_ID;
                            //}
                            //else
                            //    pkId = record.Result.APP_TYPE_INFO_ID;
                            int APP_TYPE_INFO_ID = 0;
                            String typename = string.Empty;
                            if (OnlineList.Result.Count > 0)
                            {
                                APP_TYPE_INFO_ID = OnlineList.Result.FirstOrDefault().APP_TYPE_INFO_ID;
                                typename = OnlineList.Result.FirstOrDefault().TYPE_NAME;
                            }
                            else
                            {
                                APP_TYPE_INFO_ID = OfflineList.Result.FirstOrDefault().APP_TYPE_INFO_ID;
                                typename = OfflineList.Result.FirstOrDefault().TYPE_NAME;
                                // SaveViewJsonSqlite(lstResult, "E2_GetCaseList" + Screenname, _DBPath, Convert.ToString(_caseTypeID), Convert.ToString(insertedRecordid), APP_TYPE_INFO_ID, _CaseTitle, 0, CasesInstance, false);
                            }
                            SaveViewJsonSqlite(lstResult, "E2_GetCaseList" + Screenname, _DBPath, Convert.ToString(_caseTypeID), Convert.ToString(insertedRecordid), APP_TYPE_INFO_ID, typename, 0, CasesInstance, OnlineList.Result.Count > 0 ? true : false, "M", tm_uname);
                        }

                        List<GetCaseTypesResponse.CaseData> lstResultBasicInfo = new List<GetCaseTypesResponse.CaseData>();

                        Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), Convert.ToInt32(insertedRecordid), _DBPath, "C8_GetCaseBasicInfo", "M", null);
                        onlineRecord.Wait();
                        if (onlineRecord?.Result?.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                            {
                                lstResultBasicInfo = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                            }
                        }
                        var ob = objAssignCase as GetUserInfoResponse.UserInfo;
                        lstResultBasicInfo = lstResultBasicInfo.Select(av =>
                        {
                            av.CaseAssignedDateTime = (objAssignCase != null && isApproveCase == false && isDeclineCase == false) ? DateTime.Now : Convert.ToDateTime(lstResultBasicInfo.FirstOrDefault().CaseAssignedDateTime);
                            av.CaseAssignedTo = (objAssignCase != null && isApproveCase == false && isDeclineCase == false) ? ob.DisplayName : lstResultBasicInfo.FirstOrDefault().CaseAssignedToDisplayName;
                            av.CaseAssignedToDisplayName = objAssignCase != null && isApproveCase == false && isDeclineCase == false ? ob.DisplayName : lstResultBasicInfo.FirstOrDefault().CaseAssignedToDisplayName;
                            av.CaseTypeID = lstResultBasicInfo.FirstOrDefault().CaseTypeID;
                            av.CreateBySam = lstResultBasicInfo.FirstOrDefault().CreateBy;
                            av.ListID = lstResultBasicInfo.FirstOrDefault().ListID;
                            av.CaseOwner = lstResultBasicInfo.FirstOrDefault().CaseOwner;
                            av.CaseOwnerDateTime = Convert.ToDateTime(lstResultBasicInfo.FirstOrDefault().CaseOwnerDateTime);
                            av.MetaDataCollection = viewCase.MetaDataCollection;
                            av.ModifiedBySam = FullName;
                            av.ModifiedByDisplayName = FullName;
                            av.ModifiedDateTime = DateTime.Now;
                            av.CaseClosedBy = lstResultBasicInfo.FirstOrDefault().CaseClosedByDisplayName;
                            av.CaseClosedDateTime = lstResultBasicInfo.FirstOrDefault().CaseClosedDateTime == default(DateTime) ? DateTime.Now : Convert.ToDateTime(lstResultBasicInfo.FirstOrDefault().CaseClosedDateTime);
                            av.CaseOwnerDisplayName = lstResultBasicInfo.FirstOrDefault().CaseOwnerDisplayName;
                            av.CaseTypeName = lstResultBasicInfo.FirstOrDefault().CaseTypeName;
                            av.CreateDateTime = Convert.ToDateTime(lstResultBasicInfo.FirstOrDefault().CreateDateTime);
                            av.CreateByDisplayName = lstResultBasicInfo.FirstOrDefault().CreateByDisplayName;
                            return av;
                        }).ToList();

                        if (lstResultBasicInfo.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResultBasicInfo), CasesInstance, "C8_GetCaseBasicInfo", GetAppTypeInfo.Result.INSTANCE_USER_ASSOC_ID, _DBPath, Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID), Convert.ToString(_caseTypeID), "M", Convert.ToString(insertedRecordid), 3, "", "", true);
                        }
                    }
                }
                else
                {
                    sMode = "U";
                    var _caseId = tempRecord.caseId;

                    Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(CasesInstance, _caseTypeID, _caseId, _DBPath, "C8_GetCaseBasicInfo", "E2_GetCaseList" + Screenname, true, tm_uname);
                    IscheckOnlineRecordApppType.Wait();
                    if (IscheckOnlineRecordApppType.Result.Count > 0)
                    {
                        insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, tempRecord, Constants.SaveCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[8], 0, insertedRecordid, _DBPath, ConstantsSync.CasesInstance, _caseId, sMode, IscheckOnlineRecordApppType.Result.FirstOrDefault().TYPE_SCREEN_INFO);
                    }
                    else
                    {
                        var Records = _createCaseBodyvalue as CreateCaseOptimizedRequest.CreateCaseModelOptimized;

                        insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, Records, Constants.CreateCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[8], 0, insertedRecordid, _DBPath, ConstantsSync.CasesInstance, _caseId, sMode, "C8_GetCaseBasicInfo");
                    }

                    #region Offline View case

                    Task<AppTypeInfoList> GetAppTypeInfo = DBHelper.GetAppTypeInfoList(CasesInstance, _caseId, _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath, tm_uname);
                    GetAppTypeInfo.Wait();

                    if (GetAppTypeInfo.Result == null)
                    {
                        GetAppTypeInfo = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(CasesInstance, "E2_GetCaseList" + Screenname, _caseTypeID, _DBPath, tm_uname);
                        GetAppTypeInfo.Wait();
                    }

                    List<GetCaseTypesResponse.BasicCase> viewCaseList = new List<GetCaseTypesResponse.BasicCase>();
                    GetCaseTypesResponse.BasicCase viewCase_Json = new GetCaseTypesResponse.BasicCase();
                    //viewCase_Json -- Updated JSON Need to Store in SQLite
                    if (!string.IsNullOrEmpty(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO))
                    {
                        try
                        {
                            // Fetch Older View Json
                            viewCase_Json = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO).FirstOrDefault();
                        }
                        catch
                        {
                            var jsonRecordOflntemp = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO);
                            if (IscheckOnlineRecordApppType.Result.Count > 0)
                            {
                                viewCase_Json = jsonRecordOflntemp.Where(v => v.CaseID == Convert.ToInt32(_caseId))?.FirstOrDefault();
                                if (viewCase_Json == null)
                                {
                                    Task<AppTypeInfoList> lst = DBHelper.GetAppTypeInfoListByIDS(CasesInstance, Convert.ToInt32(_caseTypeID), Convert.ToInt32(_caseId), _DBPath);
                                    lst.Wait();
                                    viewCase_Json = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(lst.Result?.ASSOC_FIELD_INFO);
                                }
                            }
                            else
                            {
                                var offlinerecor = jsonRecordOflntemp.Where(v => v.CaseID == Convert.ToInt32(insertedRecordid))?.FirstOrDefault();
                                if (offlinerecor?.CaseID > 0)
                                    viewCase_Json = offlinerecor;
                                else
                                    viewCase_Json = jsonRecordOflntemp.FirstOrDefault();

                            }
                        }

                        #region Covert Create Json to the View case Object With Updated values

                        dynamic temp;
                        if (sMode?.ToUpper() != "U")
                        {
                            temp = (CreateCaseOptimizedRequest.CreateCaseModelOptimized)_Body_value;
                            foreach (var item in temp?.textValues)
                            {
                                viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                {
                                    AssociatedTypeID = item.Key,
                                    TextValue = item.Value,
                                });
                            }
                            if (temp?.metaDataValues != null)
                            {
                                foreach (var item in temp?.metaDataValues)
                                {
                                    viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                    {
                                        AssociatedTypeID = item.Key,
                                        TextValue = item.Value,
                                    });
                                }
                            }
                        }
                        else if (IscheckOnlineRecordApppType?.Result?.Count == 0)
                        {
                            temp = (CreateCaseOptimizedRequest.CreateCaseModelOptimized)_createCaseBodyvalue;
                            foreach (var item in temp?.textValues)
                            {
                                viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                {
                                    AssociatedTypeID = item.Key,
                                    TextValue = item.Value,
                                });
                            }
                            if (temp?.metaDataValues != null)
                            {
                                foreach (var item in temp?.metaDataValues)
                                {
                                    viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                    {
                                        AssociatedTypeID = item.Key,
                                        TextValue = item.Value,
                                    });
                                }
                            }
                        }
                        else
                        {
                            temp = (SaveCaseTypeRequest)_Body_value;
                            if (IscheckOnlineRecordApppType.Result.Count == 0)
                            {
                                foreach (var item in temp?.textValues)
                                {
                                    viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                    {
                                        AssociatedTypeID = item.Key,
                                        TextValue = item.Value,
                                    });
                                }

                                if (temp?.dropDownValues != null)
                                {
                                    foreach (var item in temp?.dropDownValues)
                                    {
                                        viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                        {
                                            AssociatedTypeID = item.Key,
                                            TextValue = item.Value,
                                        });
                                    }
                                }
                            }
                            else
                            {
                                foreach (var item in temp?.textValues)
                                {
                                    viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                    {
                                        AssociatedTypeID = item.Key,
                                        TextValue = item.Value,
                                    });
                                }

                                if (temp?.dropDownValues != null)
                                {
                                    foreach (var item in temp?.dropDownValues)
                                    {
                                        var rec = item.GetType()?.GetProperty("Value")?.GetValue(item, null) as GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue;
                                        viewCase_Json.MetaDataCollection.Add(new GetCaseTypesResponse.MetaData
                                        {
                                            AssociatedTypeID = item.Key,
                                            TextValue = rec.Name,
                                        });
                                    }
                                }
                            }
                            viewCase_Json.CaseID = temp.caseId;
                        }

                        #endregion

                        if (viewCase_Json.CaseID == 0)
                        {
                            //viewCase.CaseAssignedToSAM = _UserName;
                            viewCase_Json.CaseCreatedDateTime = DateTime.Now.ToString();
                            viewCase_Json.CaseOwnerSAM = _UserName;
                            //viewCase.CaseAssignDateTime = DateTime.Now.ToString();
                            viewCase_Json.CaseCreatedSAM = _UserName;
                            viewCase_Json.CaseCreatedDisplayName = FullName;
                            viewCase_Json.CaseModifiedDateTime = DateTime.Now.ToString();
                            //viewCase.CaseAssignedToDisplayName = FullName;
                            viewCase_Json.CaseOwnerDisplayName = FullName;
                            viewCase_Json.CaseOwnerDateTime = DateTime.Now.ToString();
                            viewCase_Json.CaseModifiedByDisplayName = FullName;
                            viewCase_Json.CaseTypeID = _caseTypeID;
                            viewCase_Json.CaseTitle = _CaseTitle;
                            viewCase_Json.CaseCreatedSAM = temp.currentUser; //temp?.GetType()?.GetProperty("currentUser")?.GetValue(temp, null);
                            viewCase_Json.CaseID = insertedRecordid;

                        }
                        else
                        {
                            //viewCase_Json.CaseAssignedToSAM = viewCase_Json.CaseAssignedToSAM;//== "" ? _UserName : viewCase_Json.CaseAssignedToSAM;
                            //viewCase_Json.CaseCreatedDateTime = viewCase_Json.CaseCreatedDateTime;
                            ////viewCase.CaseOwnerSAM = jsonRecordOfln.CaseOwnerSAM;
                            //viewCase_Json.CaseOwnerSAM = _UserName;
                            //viewCase_Json.CaseAssignDateTime = viewCase_Json.CaseAssignDateTime;

                            ////viewCase.CaseCreatedSAM = jsonRecordOfln.CaseCreatedSAM;
                            //viewCase_Json.CaseCreatedSAM = viewCase_Json.CaseCreatedSAM == "" ? _UserName : viewCase_Json.CaseCreatedSAM;

                            //viewCase_Json.CaseCreatedDisplayName = viewCase_Json.CaseCreatedDisplayName == "" ? FullName : viewCase_Json.CaseCreatedDisplayName;
                            viewCase_Json.CaseModifiedDateTime = viewCase_Json.CaseModifiedDateTime;
                            viewCase_Json.CaseModifiedByDisplayName = FullName;
                            //viewCase_Json.CaseAssignedToDisplayName = viewCase_Json.CaseAssignedToDisplayName;
                            //viewCase_Json.CaseOwnerDisplayName = viewCase_Json.CaseOwnerDisplayName == "" ? FullName : viewCase_Json.CaseOwnerDisplayName;
                            //viewCase_Json.CaseOwnerDateTime = viewCase_Json.CaseOwnerDateTime;
                            //viewCase_Json.CaseTypeID = _caseTypeID;
                            //viewCase_Json.CaseTitle = _CaseTitle;
                            //viewCase_Json.CaseCreatedSAM = temp.currentUser;
                        }

                        List<GetCaseTypesResponse.BasicCase> lstResult = new List<GetCaseTypesResponse.BasicCase>();

                        /*vishalpr*/
                        /*For Update Single List JSOn ID WISE */

                        //Task<List<AppTypeInfoList>> OnlineList = DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(CasesInstance, Convert.ToInt32(_caseTypeID), _DBPath, "E2_GetCaseList" + Screenname, "M");
                        //OnlineList.Wait();
                        //Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(CasesInstance, Convert.ToInt32(_caseTypeID), _DBPath, "E2_GetCaseList" + Screenname, "T");
                        //OfflineList.Wait();

                        Task<List<AppTypeInfoList>> OnlineList = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), _caseId, _DBPath, "E2_GetCaseList" + Screenname, "M", tm_uname);
                        OnlineList.Wait();
                        Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), _caseId, _DBPath, "E2_GetCaseList" + Screenname, "T", tm_uname);
                        OfflineList.Wait();


                        //GetCaseTypesResponse.BasicCase records = new GetCaseTypesResponse.BasicCase();
                        List<GetCaseTypesResponse.BasicCase> Tempjson = new List<GetCaseTypesResponse.BasicCase>();
                        // GetCaseTypesResponse.BasicCase temps = new GetCaseTypesResponse.BasicCase();
                        //Task<List<AppTypeInfoList>> OnlineList1 = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), insertedRecordid, _DBPath, "E2_GetCaseList" + Screenname, "M", tm_uname);
                        // OnlineList1.Wait();

                        //Task<List<AppTypeInfoList>> OfflineList1 = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), insertedRecordid, _DBPath, "E2_GetCaseList" + Screenname, "T", tm_uname);
                        // OfflineList1.Wait();


                        if (OnlineList.Result.Count > 0)
                        {
                            try
                            {
                                Tempjson = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OnlineList.Result.Select(v => v.ASSOC_FIELD_INFO).ToList().FirstOrDefault());
                            }
                            catch (Exception)
                            {
                                /*vishalpr*/
                                var tp = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OnlineList.Result.Select(v => v.ASSOC_FIELD_INFO).ToList().FirstOrDefault());
                                Tempjson.AddRange(tp);
                            }
                        }
                        if (OfflineList.Result.Count > 0)
                        {
                            try
                            {
                                var jsonj = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OfflineList.Result.Where(v => v.ID == insertedRecordid).FirstOrDefault().ASSOC_FIELD_INFO);
                                if (jsonj.Count == 0)
                                {
                                    jsonj = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(Convert.ToString(OfflineList.Result.Where(v => v.ID == tempRecord.caseId)?.FirstOrDefault()?.ASSOC_FIELD_INFO));
                                }
                                Tempjson.AddRange(jsonj);
                            }
                            catch
                            {
                                try
                                {
                                    var st = Convert.ToString(OfflineList.Result.Where(v => v.ID == tempRecord.caseId)?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                    if (!string.IsNullOrEmpty(st))
                                        Tempjson = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(st);
                                }
                                catch (Exception)
                                {
                                    var jsontt = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(Convert.ToString(OfflineList.Result.Where(v => v.ID == tempRecord.caseId)?.FirstOrDefault()?.ASSOC_FIELD_INFO));
                                    if (jsontt == null)
                                    {
                                        jsontt = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(OfflineList.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                    }
                                    Tempjson.Add(jsontt);
                                }
                            }
                        }

                        int searchById = 0;
                        if (OnlineList.Result.Count > 0)
                        {
                            searchById = _caseId;
                        }
                        else
                        {
                            if (OfflineList.Result.Count > 0)
                            {
                                searchById = _caseId;
                            }
                            else
                                searchById = Convert.ToInt32(insertedRecordid);
                        }

                        bool flg = false;
                        if (Tempjson.FindAll(v => v.CaseID == searchById)?.Count > 0)
                        {
                            var vv = Tempjson.Where(v => v.CaseID == searchById);
                            var Results = Tempjson.Where(p => !vv.Any(p2 => p2.CaseID == p.CaseID));
                            if (!flg)
                            {
                                foreach (var itm in Results)
                                {
                                    lstResult.Add(itm);
                                    flg = true;
                                }
                            }


                            var res = vv.Select(v =>
                            {
                                v.CaseCreatedSAM = Tempjson.Where(av => av.CaseID == searchById).FirstOrDefault().CaseCreatedSAM == "" ? viewCase_Json.CaseCreatedSAM : Tempjson.Where(av => v.CaseID == searchById).FirstOrDefault().CaseCreatedSAM;
                                v.CaseCreatedDisplayName = Tempjson.Where(av => av.CaseID == searchById).FirstOrDefault().CaseCreatedDisplayName == "" ? viewCase_Json.CaseCreatedDisplayName : Tempjson.Where(av => av.CaseID == searchById).FirstOrDefault().CaseCreatedDisplayName;
                                v.MetaDataCollection = viewCase_Json.MetaDataCollection;
                                v.CaseTitle = viewCase_Json.CaseTitle;
                                v.CaseID = viewCase_Json.CaseID;
                                return v;
                            });
                            /**/

                            if (lstResult.Where(v => v.CaseID == searchById)?.Count() == 0)
                            {
                                lstResult.AddRange(res);
                            }
                            else
                            {
                                lstResult.Where(v => v.CaseID == searchById).Select(av =>
                                {
                                    av.CaseCreatedSAM = viewCase_Json.CaseCreatedSAM;
                                    av.CaseCreatedDisplayName = viewCase_Json.CaseCreatedDisplayName;
                                    av.MetaDataCollection = viewCase_Json.MetaDataCollection;
                                    av.CaseTitle = viewCase_Json.CaseTitle;
                                    av.CaseID = viewCase_Json.CaseID;
                                    return av;
                                }).OrderByDescending(v => v.CaseID).ToList();
                            }
                        }
                        else
                        {
                            lstResult.AddRange(Tempjson);
                        }
                        //lstResult -- updated View case Json

                        int pkId = 0;
                        Task<AppTypeInfoList> record = null;

                        if (sMode?.ToUpper() == "U")
                        {
                            record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseId), _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath, tm_uname);
                            record.Wait();
                            if (record.Result != null)
                                pkId = record.Result.APP_TYPE_INFO_ID;

                            /*VishaL pR*/
                            //if (record?.Result == null)
                            //{
                            //    record = DBHelper.GetAppTypeInfoList(CasesInstance, Convert.ToInt32(_caseId), _caseTypeID, "E2_GetCaseList" + Screenname, _DBPath, tm_uname);

                            //    record.Wait();
                            //    pkId = record.Result.APP_TYPE_INFO_ID;
                            //}
                            //else
                            //    pkId = record.Result.APP_TYPE_INFO_ID;
                        }
                        string _cid = "0";
                        if (record?.Result.IS_ONLINE == true)
                        {
                            _cid = Convert.ToString(record?.Result?.ID);
                        }
                        else
                        {
                            _cid = record?.Result?.TransactionType?.ToUpper() == "M" ? Convert.ToString(record?.Result?.ID) : Convert.ToString(insertedRecordid);
                        }

                        SaveViewJsonSqlite(lstResult, "E2_GetCaseList" + Screenname, _DBPath, Convert.ToString(_caseTypeID), _cid, pkId, record.Result.TYPE_NAME, 0, CasesInstance, IscheckOnlineRecordApppType?.Result?.Count == 0 ? false : true, "T", tm_uname);


                        List<GetCaseTypesResponse.CaseData> lstResultBasicInfo = new List<GetCaseTypesResponse.CaseData>();


                        Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_caseTypeID), Convert.ToInt32(_cid), _DBPath, "C8_GetCaseBasicInfo", "M", null);

                        onlineRecord.Wait();
                        if (onlineRecord?.Result?.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                            {
                                /*vishalpr*/
                                try
                                {
                                    lstResultBasicInfo = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                }
                                catch (Exception)
                                {
                                    var tp = JsonConvert.DeserializeObject<GetCaseTypesResponse.CaseData>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                    lstResultBasicInfo.Add(tp);
                                }
                            }
                        }

                        //lstResultBasicInfo -- till here this Has a OLD Data


                        #region Converting View case Json to CASEDATA Class
                        var x = lstResultBasicInfo;
                        lstResultBasicInfo = lstResultBasicInfo.Select(av =>
                                                {

                                                    //av.CaseAssignedDateTime = Convert.ToDateTime(lstResult.FirstOrDefault().CaseAssignedToDisplayName);
                                                    av.CaseAssignedTo = lstResult.FirstOrDefault().CaseAssignedToDisplayName;
                                                    av.CaseAssignedToDisplayName = lstResult.FirstOrDefault().CaseAssignedToDisplayName;
                                                    av.CaseTypeID = lstResult.FirstOrDefault().CaseTypeID;
                                                    av.CreateBySam = _UserName;
                                                    av.ListID = lstResult.FirstOrDefault().ListID;
                                                    av.CaseOwner = lstResult.FirstOrDefault().CaseOwnerDisplayName;
                                                    av.CaseOwnerDateTime = Convert.ToDateTime(lstResult.FirstOrDefault().CaseOwnerDateTime);
                                                    av.MetaDataCollection = lstResult.FirstOrDefault().MetaDataCollection;
                                                    av.ModifiedBySam = lstResult.FirstOrDefault().CaseModifiedBySAM;
                                                    av.ModifiedByDisplayName = lstResult.FirstOrDefault().CaseModifiedByDisplayName;
                                                    av.ModifiedDateTime = Convert.ToDateTime(lstResult.FirstOrDefault().CaseModifiedDateTime);
                                                    av.CaseClosedBy = lstResult.FirstOrDefault().CaseClosedByDisplayName;
                                                    av.CaseClosedDateTime = string.IsNullOrEmpty(lstResult.FirstOrDefault().CaseClosedDateTime) ? DateTime.Now : Convert.ToDateTime(lstResult.FirstOrDefault().CaseClosedDateTime);
                                                    av.CaseOwnerDisplayName = lstResult.FirstOrDefault().CaseOwnerDisplayName;
                                                    av.CaseTypeName = lstResult.FirstOrDefault().CaseTypeName;
                                                    av.CreateDateTime = Convert.ToDateTime(lstResult.FirstOrDefault().CaseCreatedDateTime);
                                                    av.CreateByDisplayName = lstResult.FirstOrDefault().CaseCreatedDisplayName;
                                                    return av;
                                                }).ToList();

                        if (lstResultBasicInfo.Count > 0)
                        {
                            int i = Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);


                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResultBasicInfo), CasesInstance, "C8_GetCaseBasicInfo", GetAppTypeInfo.Result.INSTANCE_USER_ASSOC_ID, _DBPath, Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID), Convert.ToString(_caseTypeID), "T", record?.Result.IS_ONLINE == true ? Convert.ToString(record?.Result.ID) : Convert.ToString(insertedRecordid), 0, "", "", true);
                        }
                        #endregion
                    }

                    #endregion

                    if (!string.IsNullOrEmpty(_CaseNotes))// && !isApproveCase)
                    {
                        /*vishalpr*/
                        string insertID = "0";
                        if (IscheckOnlineRecordApppType.Result.Count > 0)
                        {
                            insertID = Convert.ToString(_caseId);
                            //AddCasNotes(_IsOnline, _caseTypeID, Convert.ToString(_caseId), _CaseNotes, _UserName, _Notestypeid, _DBPath, FullName);
                        }
                        else
                        {
                            insertID = Convert.ToString(insertedRecordid);
                            //AddCasNotes(_IsOnline, _caseTypeID, Convert.ToString(insertedRecordid), _CaseNotes, _UserName, _Notestypeid, _DBPath, FullName);
                        }

                        AddCasNotes(_IsOnline, _caseTypeID, Convert.ToString(insertID), _CaseNotes, _UserName, _Notestypeid, _DBPath, FullName);
                    }

                    // Till Here New record has been updated In SQLite


                    if (objAssignCase != null)
                    {
                        if (isApproveCase)
                        {
                            string Tcaseid = "0";

                            if (IscheckOnlineRecordApppType.Result.Count > 0)
                            {
                                Tcaseid = Convert.ToString(tempRecord.caseId);
                                //storeApprovedAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(tempRecord.caseId), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_ApprovedAssignCase", CasesInstance, viewCase_Json, _CaseTitle, _CaseNotes, FullName, Screenname);
                            }
                            else
                            {
                                Tcaseid = Convert.ToString(insertedRecordid);
                                //storeApprovedAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(insertedRecordid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_ApprovedAssignCase", CasesInstance, viewCase_Json, _CaseTitle, _CaseNotes, FullName, Screenname);
                            }

                            storeApprovedAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(Tcaseid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_ApprovedAssignCase", CasesInstance, viewCase_Json, _CaseTitle, _CaseNotes, FullName, Screenname);

                        }
                        else if (isDeclineCase)
                        {
                            string Tcaseid = "0";

                            if (IscheckOnlineRecordApppType.Result.Count > 0)
                            {
                                Tcaseid = Convert.ToString(tempRecord.caseId);
                                //storeDeclineAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(tempRecord.caseId), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_ApprovedAssignCase", CasesInstance, viewCase_Json, _CaseTitle, _CaseNotes, FullName, Screenname);
                            }
                            else
                            {
                                Tcaseid = Convert.ToString(insertedRecordid);
                                // storeDeclineAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(insertedRecordid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_ApprovedAssignCase", CasesInstance, viewCase_Json, _CaseTitle, _CaseNotes, FullName, Screenname);
                            }
                            storeDeclineAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(Tcaseid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_ApprovedAssignCase", CasesInstance, viewCase_Json, _CaseTitle, _CaseNotes, FullName, Screenname);
                        }
                        else
                        {
                            string Tcaseid = "0";
                            if (IscheckOnlineRecordApppType.Result.Count > 0)
                            {
                                Tcaseid = Convert.ToString(tempRecord.caseId);
                                //storeAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(tempRecord.caseId), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_AssignCase", CasesInstance, viewCase_Json, _CaseTitle, FullName, Screenname);
                            }
                            else
                            {
                                Tcaseid = Convert.ToString(insertedRecordid);
                                //storeAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(insertedRecordid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_AssignCase", CasesInstance, viewCase_Json, _CaseTitle, FullName, Screenname);
                            }
                            storeAssignCase(_IsOnline, objAssignCase, _caseTypeID, Convert.ToString(Tcaseid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_AssignCase", CasesInstance, viewCase_Json, _CaseTitle, FullName, Screenname);
                        }
                    }
                    if (objTakeOwershipCase != null)
                    {
                        string Tcaseid = "0";
                        if (IscheckOnlineRecordApppType.Result.Count > 0)
                        {
                            Tcaseid = Convert.ToString(tempRecord.caseId);
                            //storeTakeOwershipCase(_IsOnline, objTakeOwershipCase, _caseTypeID, Convert.ToString(tempRecord.caseId), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_TakeOwerShip", CasesInstance, viewCase_Json, _CaseTitle, FullName, Screenname);
                        }
                        else
                        {
                            Tcaseid = Convert.ToString(insertedRecordid);
                            //storeTakeOwershipCase(_IsOnline, objTakeOwershipCase, _caseTypeID, Convert.ToString(insertedRecordid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_TakeOwerShip", CasesInstance, viewCase_Json, _CaseTitle, FullName, Screenname);
                        }
                        storeTakeOwershipCase(_IsOnline, objTakeOwershipCase, _caseTypeID, Convert.ToString(Tcaseid), _UserName, _DBPath, "C1_C2_CASES_CASETYPELIST", "C4_TakeOwerShip", CasesInstance, viewCase_Json, _CaseTitle, FullName, Screenname);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return insertedRecordid;
        }
        #endregion

        #region Save View Json Sqlite
        public static void SaveViewJsonSqlite(object _BodyValue, string _ScreenName, string _DBPath, string _CaseTypeId, string Id, int pkId, string _caseType, int? _CategoryId = null, string InstanceName = "", bool isonlineRecord = false, string _transtype = "T", string tm_uname = null)
        {
            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(_BodyValue), InstanceName, _ScreenName, INSTANCE_USER_ASSOC_ID, _DBPath, pkId, _CaseTypeId, _transtype, Id, _CategoryId, _caseType, "", isonlineRecord, tm_uname);
        }
        #endregion

        #region Create Case
        public static async Task<int> CreateCaseFromsDB(string _DBPath)
        {
            int insertedRecordid = 0;
            try
            {
                var Records = await DBHelper.GetItemTranInfoListList(_DBPath);
                var RecordList = Records.Where(v => v.PROCESS_ID == 0 && v.METHOD == Constants.CreateCaseOptimized);
                foreach (var item in RecordList)
                {
                    try
                    {
                        var _Body_value = JsonConvert.DeserializeObject<CreateCaseOptimizedRequest.CreateCaseModelOptimized>(item.ITEM_TRAN_INFO);
                        var Result = CasesAPIMethods.CreateCaseOptimized(_Body_value);
                        if (Result != null)
                        {
                            string Res = Convert.ToString(Result.GetValue("ResponseContent"));
                            item.PROCESS_ID = Int32.Parse(Res);
                            insertedRecordid = await DBHelper.SaveItemTranInfoList(item, _DBPath);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Store Case
        public static async Task<int> StoreAndReturnCaseToLastAssignee(bool _IsOnline, int _caseTypeID, object _Body_value, string _CaseNotes, string _UserName, string _DBPath)
        {
            int insertedRecordid = 0;
            try
            {
                if (_IsOnline)
                {
                    //ItemTranInfoList ItemTranInfo = new ItemTranInfoList();
                    //var GetAppTypeInfo = await DBHelper.GetAppTypeInfoListByID(_caseTypeID, _DBPath);
                    //ItemTranInfo.APP_TYPE_INFO_ID = GetAppTypeInfo.APP_TYPE_INFO_ID;
                    //ItemTranInfo.ITEM_TRAN_INFO = JsonConvert.SerializeObject(_Body_value);
                    //ItemTranInfo.METHOD = Constants.CreateCaseOptimized;
                    //ItemTranInfo.LAST_SYNC_DATETIME = DateTime.Now;

                    //insertedRecordid = await DBHelper.SaveItemTranInfoList(ItemTranInfo, _DBPath);

                    var Result = CasesAPIMethods.CreateCaseOptimized(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_caseTypeID, _Body_value, Constants.CreateCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[0], insertedRecordid, insertedRecordid, _DBPath);

                    if (!string.IsNullOrEmpty(_CaseNotes))
                        AddCasNotes(_IsOnline, _caseTypeID, Convert.ToString(iteminfoid), _CaseNotes, _UserName, "_Notestypeid", _DBPath, "");
                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(_caseTypeID, _Body_value, Constants.CreateCaseOptimized, (Enum.GetNames(typeof(ActionTypes)))[0], 0, insertedRecordid, _DBPath);

                    if (!string.IsNullOrEmpty(_CaseNotes))
                        AddCasNotes(_IsOnline, _caseTypeID, Convert.ToString(insertedRecordid), _CaseNotes, _UserName, "_Notestypeid", _DBPath, "");

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Call Calculations
        public static async Task<string> CallCalculations(bool _IsOnline, string _DBPath, object _Bodyvalue)
        {
            string ReturnValue = string.Empty;
            if (_IsOnline)
            {
                var Result = CasesAPIMethods.RefreshCalculationFields(_Bodyvalue);
                var temp = Result.GetValue("ResponseContent");

                if (temp != null && temp.ToString() != "[]")
                {
                    ReturnValue = temp.ToString();
                }
            }
            else
            {
                var request = _Bodyvalue as RefreshCalculationFieldsRequest;
                ReturnValue = CommonConstants.RefreshCalculationFields(request.assocFieldCollection, request.calculatedAssocId, request.assocFieldTexts, request.assocFieldValues, request.sdateFormats, _DBPath);
            }
            return ReturnValue;
        }
        #endregion

        #region Add Favorite
        public static Task<int> AddFavorite(bool _IsOnline, string _FavoriteName, string _FieldValues, string _IsActive, string _CreatedBy, string _CreatedByDt, string _ModifiedByDt, string _LastSyncDt, string _DBPath, string _CaseTypeId, string _InstanceUserAssocId, string _ApplicationId)
        {
            int res = 0;
            int insertedRecordid = 0;
            try
            {
                //    Task<AppTypeInfoList> AppTypeInfoID = DBHelper.GetAppTypeInfoListByID(Convert.ToInt32(_CaseTypeId), _DBPath);
                //    AppTypeInfoID.Wait();

                var inserted = CommonConstants.FavoriteOfflineStore(0, _CaseTypeId, null, _FavoriteName, _FieldValues, _IsActive, _CreatedBy, _CreatedByDt, _ModifiedByDt, _ApplicationId, _LastSyncDt, _InstanceUserAssocId, _DBPath);
                AddFavoriteRequest addfav = new AddFavoriteRequest()
                {
                    AppID = Convert.ToInt32(_ApplicationId),
                    FavoriteId = Convert.ToInt32(inserted.Result),
                    CreatedBy = _CreatedBy,
                    CreatedDateTime = Convert.ToDateTime(_CreatedByDt),
                    ModifiedDateTime = Convert.ToDateTime(_ModifiedByDt),
                    FavoriteName = _FavoriteName,
                    FieldValues = _FieldValues,
                    IsActive = _IsActive,
                    LastSyncDateTime = Convert.ToDateTime(_LastSyncDt),
                    pTypeId = Convert.ToInt32(_CaseTypeId),
                };
                // res = inserted.Result;
                if (_IsOnline)
                {


                    var result = CasesAPIMethods.AddFavorite(addfav);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        res = Convert.ToInt32(temp.ToString());
                    }
                }
                else
                {
                    Task<FavoriteList> GetFavoriteInfo = DBHelper.GetFavoriteListByID(int.Parse(_ApplicationId), 0, int.Parse(_CaseTypeId), _FavoriteName, _DBPath);
                    GetFavoriteInfo.Wait();
                    if (!string.IsNullOrEmpty(_CaseTypeId))
                    {
                        //Task<int> rs = CommonConstants.FavoriteOfflineStore(0, _CaseTypeId, "0", _FavoriteName, _FieldValues, _IsActive, _CreatedBy, _CreatedByDt, _ModifiedByDt, _ApplicationId, _LastSyncDt, _InstanceUserAssocId, _DBPath);
                        // res = rs.Result;

                        GetFavoriteResponse.GetFavorite viewFave = new GetFavoriteResponse.GetFavorite()
                        {
                            ApplicationID = Convert.ToInt32(_ApplicationId),
                            CreatedBy = _CreatedBy,
                            CreatedDateTime = Convert.ToDateTime(_CreatedByDt),
                            FavoriteId = inserted.Result,
                            FavoriteName = _FavoriteName,
                            FieldValues = _FieldValues,
                            IsActive = _IsActive,
                            LastSyncDateTime = Convert.ToDateTime(_LastSyncDt),
                            ModifiedDateTime = Convert.ToDateTime(_ModifiedByDt),
                            QuestAreaID = 0,
                            TypeID = Convert.ToInt32(_CaseTypeId)
                        };

                        SaveViewJsonSqlite(viewFave, "C1_GetFavorite", _DBPath, Convert.ToString(_CaseTypeId), Convert.ToString(inserted.Result), 0, "", 0, CasesInstance);

                        var GetFavoriteList = CommonConstants.GetResultBySytemcodeList(CasesInstance, "C1_GetFavorite", _DBPath);
                        ItemTranInfoList ItemTranInfo = new ItemTranInfoList();
                        ItemTranInfo.APP_TYPE_INFO_ID = GetFavoriteList.Result.APP_TYPE_INFO_ID; //Convert.ToInt32(_CaseTypeId);
                        ItemTranInfo.ITEM_TRAN_INFO = JsonConvert.SerializeObject(addfav);
                        ItemTranInfo.METHOD = Constants.AddFavorite;
                        ItemTranInfo.ACTION_TYPE = (Enum.GetNames(typeof(ActionTypes)))[9];
                        ItemTranInfo.PROCESS_ID = 0;
                        ItemTranInfo.LAST_SYNC_DATETIME = DateTime.Now;
                        ItemTranInfo.REF_ITEM_TRAN_INFO_ID = 0;
                        ItemTranInfo.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                        ItemTranInfo.ITEM_TRAN_INFO_ID = 0;

                        var insertedRecordid1 = DBHelper.SaveItemTranInfoList(ItemTranInfo, _DBPath);



                        //insertedRecordid = CommonConstants.AddofflineTranInfo(Convert.ToInt32(_CaseTypeId), addfav,Constants.AddFavorite, (Enum.GetNames(typeof(ActionTypes)))[9],0, insertedRecordid, _DBPath,CasesInstance,0,"C").Result;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(res);
        }
        #endregion

        #region Remove Favorite
        public static Task<int> RemoveFavorite(bool _IsOnline, string _FavoriteID, string _DBPath, string INSTANCE_USER_ASSOC_ID, String Username)
        {
            int res = 0;
            try
            {
                var GetFavoriteInfo = DBHelper.GetFavoriteList(Convert.ToInt32(_FavoriteID), _DBPath);
                GetFavoriteInfo.Wait();
                //int FavId = GetFavoriteInfo.Result.Where(v => v.FAVORITE_ID == Convert.ToInt32(_FavoriteID)).Select(a => a.FAVORITE_ID).FirstOrDefault();
                if (_IsOnline)
                {

                    //var inserted = CommonConstants.FavoriteOfflineStore(GetFavoriteInfo.Result.FAVORITE_ID, Convert.ToString(AppTypeInfoID.Result.APP_TYPE_INFO_ID), _FavoriteName, _FieldValues, _IsActive, _CreatedBy, _CreatedByDt, _ModifiedByDt, _LastSyncDt, _InstanceUserAssocId, _DBPath);
                    var result = CasesAPIMethods.RemoveFavorite(_FavoriteID, Username);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        res = Convert.ToInt32(temp.ToString() == "True" ? 1 : 0);
                    }
                }
                else
                {

                    if (GetFavoriteInfo.Result != null)
                    {
                        Task<int> rs = CommonConstants.RemoveFavoriteOfflineStore(CasesInstance, GetFavoriteInfo.Result.FAVORITE_ID, "C1_GetFavorite", _DBPath);
                        res = rs.Result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Task.FromResult(res);
        }
        #endregion

        #region Get Favorite
        public static async Task<List<GetFavoriteResponse.GetFavorite>> GetFavorite(bool _IsOnline, string _CreatedBy, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetFavoriteResponse.GetFavorite> lstResult = new List<GetFavoriteResponse.GetFavorite>();
            var GetFavoriteList = CommonConstants.GetResultBySytemcodeList(CasesInstance, "C1_GetFavorite", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetFavorite(Convert.ToString(_CreatedBy));
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetFavoriteResponse.GetFavorite>>(temp.ToString());
                        DBHelper.RemoveFavorites(_CreatedBy, _DBPath);
                        foreach (var item in lstResult)
                        {
                            var inserted = CommonConstants.FavoriteOfflineStore(0, item.TypeID.ToString(), null, item.FavoriteName, item.FieldValues, item.IsActive, _CreatedBy, item.CreatedDateTime.ToString(), item.ModifiedDateTime.ToString(), item.ApplicationID.ToString(), item.LastSyncDateTime.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID.ToString(), _DBPath);
                        }
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C1_GetFavorite", _InstanceUserAssocId, _DBPath, GetFavoriteList.Result == null ? 0 : GetFavoriteList.Result.APP_TYPE_INFO_ID, "", "M");
                        }
                    }
                }
                else
                {
                    //lstResult = GetFavoriteList.Result?.ASSOC_FIELD_INFO.ToString() == null ? null : JsonConvert.DeserializeObject<List<GetFavoriteResponse.GetFavorite>>(GetFavoriteList.Result?.ASSOC_FIELD_INFO.ToString());
                    //lstResult = CommonConstants.ReturnListResult<GetFavoriteResponse.GetFavorite>(CasesInstance, "C1_GetFavorite", _DBPath);
                    Task<List<AppTypeInfoList>> OnlineList = DBHelper.GetAppTypeInfoListByTransTypeSyscode_tm_username(CasesInstance, _DBPath, "C1_GetFavorite", "M");
                    OnlineList.Wait();
                    Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByTransTypeSyscode_tm_username(CasesInstance, _DBPath, "C1_GetFavorite", "T");
                    OfflineList.Wait();
                    GetFavoriteResponse.GetFavorite records = new GetFavoriteResponse.GetFavorite();
                    List<GetFavoriteResponse.GetFavorite> json = new List<GetFavoriteResponse.GetFavorite>();
                    if (OnlineList.Result.Count > 0)
                    {
                        json = JsonConvert.DeserializeObject<List<GetFavoriteResponse.GetFavorite>>(OnlineList.Result.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                    }

                    int cnt = 0;
                    foreach (var item in OfflineList.Result)
                    {
                        var temp = JsonConvert.DeserializeObject<GetFavoriteResponse.GetFavorite>(item.ASSOC_FIELD_INFO);
                        lstResult.Add(temp);
                    }
                    lstResult.AddRange(json);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion


        #region Get CaseList
        public static async Task<List<GetCaseTypesResponse.BasicCase>> GetCaseList(bool _IsOnline, string _user, string _CaseTypeID, string _CaseOwnerSAM, string _AssignedToSAM, string _ClosedBySAM, string _CreatedBySAM, string _PropertyID, string _TenantCode, string _TenantID, string _showOpenClosedCasesType, string _showPastDueDate, string _SearchQuery, int _InstanceUserAssocId, string _DBPath, string _FullName, string sTitle, bool SaveSql, string screenName, int? _pageindex, int? _pagenumber)
        {
            List<GetCaseTypesResponse.BasicCase> OnlineCaselist = new List<GetCaseTypesResponse.BasicCase>();
            //int id = CommonConstants.GetResultBySytemcode(CasesInstance, "E2_GetCaseList" + screenName, _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetCaseList(_user, _CaseTypeID, _CaseOwnerSAM, _AssignedToSAM, _ClosedBySAM, _CreatedBySAM,
                                                    _PropertyID, _TenantCode, _TenantID, _showOpenClosedCasesType, _showPastDueDate, _SearchQuery, Convert.ToString(_pagenumber), Convert.ToString(_pageindex));
                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        OnlineCaselist = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(temp.ToString());
                        if (OnlineCaselist.Count > 0)
                        {
                            if (SaveSql)// && string.IsNullOrEmpty(screenName))
                            {
                                string Ntm_uname = string.Empty;


                                if (screenName == "_OwnedByMe")
                                {
                                    Ntm_uname = _CaseOwnerSAM;
                                }
                                else if (screenName == "_AssignedToMe")
                                {
                                    Ntm_uname = _AssignedToSAM;
                                }
                                else if (screenName == "_CreatedByMe")
                                {
                                    Ntm_uname = _CreatedBySAM;
                                }
                                else if (screenName == "_AssignedToMyTeam")
                                {
                                    Ntm_uname = _AssignedToSAM;
                                }
                                else
                                {
                                    Ntm_uname = _user;
                                }

                                var infolist = DBHelper.GetAppTypeInfoListByTransTypeSyscode_tm_username(CasesInstance, _DBPath, "E2_GetCaseList" + screenName, "M", Ntm_uname);
                                infolist.Wait();

                                /*vishalpr*/
                                foreach (var item in OnlineCaselist)
                                {
                                    var tm_uname = DBHelper.GetAppTypeInfo_tmname(CasesInstance, Convert.ToInt32(item.CaseID), item.CaseTypeID, "E2_GetCaseList" + screenName, _DBPath).Result?.TM_Username;

                                    #region For Duplicate Record Check

                                    var OfflineResultset = DBHelper.GetAppTypeInfoList(CasesInstance, item.CaseID, item.CaseTypeID, "E2_GetCaseList" + screenName, _DBPath, tm_uname);
                                    OfflineResultset.Wait();

                                    if (OfflineResultset?.Result == null)
                                    {
                                        /*For New Entry*/
                                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(new List<GetCaseTypesResponse.BasicCase>() { item }), CasesInstance, "E2_GetCaseList" + screenName, _InstanceUserAssocId, _DBPath, 0, Convert.ToString(item.CaseTypeID), "M", Convert.ToString(item.CaseID), 0, item.CaseTypeName, "", true, Ntm_uname);
                                    }
                                    else
                                    {
                                        /*For Update Any json*/
                                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(new List<GetCaseTypesResponse.BasicCase>() { item }), CasesInstance, "E2_GetCaseList" + screenName, _InstanceUserAssocId, _DBPath, OfflineResultset.Result.APP_TYPE_INFO_ID, Convert.ToString(item.CaseTypeID), OfflineResultset.Result.TransactionType, Convert.ToString(item.CaseID), 0, item.CaseTypeName, "", true, Ntm_uname);
                                    }
                                    #endregion

                                }

                                if (screenName != "")
                                {
                                    List<GetCaseTypesResponse.BasicCase> offlineCaselist = new List<GetCaseTypesResponse.BasicCase>();

                                    foreach (var ite in infolist.Result)
                                    {
                                        offlineCaselist.AddRange(JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(ite?.ASSOC_FIELD_INFO.ToString()));
                                    }

                                    var N_caseid = OnlineCaselist.Where(x => !offlineCaselist.Any(y => y.CaseID == x.CaseID)).ToList();//.Select(v => v.CaseID).ToList();

                                    //if (offlineCaselist.Count > 0)
                                    {
                                        /*Return Case not exist in Offline List*/
                                        //var Necaseid = OnlineCaselist.Where(x => !offlineCaselist.Any(y => y.CaseID == x.CaseID)).ToList();//.Select(v => v.CaseID).ToList();

                                        List<GetCaseTypesResponse.BasicCase> Necaseid = new List<GetCaseTypesResponse.BasicCase>();
                                        Necaseid.AddRange(OnlineCaselist);
                                        Necaseid.AddRange(offlineCaselist);

                                        //var Necasetypeid = OnlineCaselist.Where(x => !offlineCaselist.Any(y => y.CaseID == x.CaseID)).ToList();//.Select(v => v.CaseTypeID).ToList();
                                        for (int i = 0; i < Necaseid.Count; i++)
                                        {
                                            int id = Necaseid[i].CaseID;
                                            int typeid = Necaseid[i].CaseTypeID;

                                            var Repeatlist = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_Object(CasesInstance, typeid, id, _DBPath, "E2_GetCaseList_AssignedToMyTeam", "M", null);

                                            if (Repeatlist.Result == null)
                                            {
                                                Repeatlist = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_Object(CasesInstance, typeid, id, _DBPath, "E2_GetCaseList_AssignedToMyTeam", "T", null);
                                            }

                                            Repeatlist.Wait();
                                            string rTM_Name = string.Empty;
                                            if (Repeatlist?.Result != null)
                                            {
                                                rTM_Name = Repeatlist?.Result?.TM_Username;
                                                if (screenName != "_AssignedToMyTeam")
                                                    /*Delete Record From List of Assigned to My TEAM */
                                                    DBHelper.DeleteAppTypeInfoListById(Repeatlist?.Result, _DBPath).Wait();

                                                #region Delete C4_AddNotes
                                                //    //C4_AddNotes
                                                var On_Addnote = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Repeatlist?.Result.TYPE_ID, Convert.ToInt32(Repeatlist?.Result.ID), _DBPath, "C4_AddNotes", "M", Ntm_uname);
                                                On_Addnote.Wait();

                                                foreach (var on_note in On_Addnote.Result)
                                                {
                                                    DBHelper.DeleteAppTypeInfoListById(on_note, _DBPath).Wait();
                                                }

                                                var Off_Addnote = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Repeatlist?.Result.TYPE_ID, Convert.ToInt32(Repeatlist?.Result.ID), _DBPath, "C4_AddNotes", "T", Ntm_uname);
                                                Off_Addnote.Wait();
                                                foreach (var off_note in Off_Addnote.Result)
                                                {
                                                    DBHelper.DeleteAppTypeInfoListById(off_note, _DBPath).Wait();
                                                }
                                                #endregion
                                            }

                                            CommonConstants.InsertnewCase_C8_C4_Json(_DBPath, _user, Convert.ToString(typeid), Convert.ToString(id), INSTANCE_USER_ASSOC_ID, rTM_Name == "" ? _user : rTM_Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    _CaseTypeID = _CaseTypeID == "" ? "0" : _CaseTypeID;
                    List<AppTypeInfoList> OnlineList = new List<AppTypeInfoList>();
                    List<AppTypeInfoList> OfflineList = new List<AppTypeInfoList>();

                    if (string.IsNullOrEmpty(screenName))
                    {
                        // No need tm_user wise Record its whole list as per Screen anme
                        var tempol = DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(CasesInstance, Convert.ToInt32(_CaseTypeID), _DBPath, "E2_GetCaseList" + screenName, "M");
                        tempol.Wait();
                        OnlineList = tempol.Result;

                        var tempoff = DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(CasesInstance, Convert.ToInt32(_CaseTypeID), _DBPath, "E2_GetCaseList" + screenName, "T");
                        tempoff.Wait();
                        OfflineList = tempoff.Result;
                    }
                    else
                    {
                        string SearchByTMuser = string.Empty;
                        if (screenName == "_OwnedByMe")
                        {
                            SearchByTMuser = _CaseOwnerSAM;
                        }
                        else if (screenName == "_AssignedToMe")
                        {
                            SearchByTMuser = _AssignedToSAM;
                        }
                        else if (screenName == "_CreatedByMe")
                        {
                            SearchByTMuser = _CreatedBySAM;
                        }
                        else if (screenName == "_AssignedToMyTeam")
                        {
                            SearchByTMuser = _AssignedToSAM;

                        }

                        var tempol = DBHelper.GetAppTypeInfoListByTransTypeSyscode_tm_username(CasesInstance, _DBPath, "E2_GetCaseList" + screenName, "M", SearchByTMuser);
                        tempol.Wait();
                        OnlineList = tempol.Result;

                        var tempoff = DBHelper.GetAppTypeInfoListByTransTypeSyscode_tm_username(CasesInstance, _DBPath, "E2_GetCaseList" + screenName, "T", SearchByTMuser);
                        tempoff.Wait();
                        OfflineList = tempoff.Result;
                    }

                    GetCaseTypesResponse.BasicCase records = new GetCaseTypesResponse.BasicCase();
                    List<GetCaseTypesResponse.BasicCase> json = new List<GetCaseTypesResponse.BasicCase>();
                    if (OnlineList.Count > 0)
                    {
                        /*vishalpr*/
                        try
                        {
                            foreach (var item in OnlineList)
                            {
                                var it = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(item.ASSOC_FIELD_INFO);
                                json.AddRange(it);
                            }
                        }
                        catch (Exception)
                        {
                            json = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(OnlineList.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                        }
                    }

                    int cnt = 0;
                    bool flg = false;
                    foreach (var item in OfflineList)
                    {
                        if (item.ASSOC_FIELD_INFO.ToString().Trim() == "[]" || string.IsNullOrEmpty(item.ASSOC_FIELD_INFO.ToString()))
                            continue;
                        GetCaseTypesResponse.BasicCase temp = new GetCaseTypesResponse.BasicCase();
                        try
                        {
                            temp = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(item.ASSOC_FIELD_INFO);
                        }
                        catch
                        {
                            try
                            {
                                var tempt = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(item.ASSOC_FIELD_INFO);
                                temp = tempt.Where(v => v.CaseID == item.ID).FirstOrDefault();
                                if (temp == null)
                                    temp = tempt.FirstOrDefault();
                            }
                            catch (Exception)
                            {
                                var tempt1 = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(item.ASSOC_FIELD_INFO);
                                temp = tempt1.FirstOrDefault();
                            }
                        }

                        temp.CaseTitle = temp.MetaDataCollection?.FirstOrDefault()?.TextValue;
                        temp.CaseCreatedSAM = _user;
                        temp.CaseCreatedDateTime = Convert.ToString(DateTime.Now);
                        temp.CaseCreatedDisplayName = _FullName;
                        temp.ListID = json.Count == 0 ? OnlineList.Count + (++cnt) : Convert.ToInt32(temp.DisplayListID.Split('-')[1]);
                        temp.CaseAssignedToSAM = json.Count == 0 ? "" : json.FirstOrDefault().CaseAssignedToSAM;
                        temp.CaseID = Convert.ToInt32(item.ID);

                        if (json.FindAll(v => v.CaseID == Convert.ToInt32(temp.CaseID))?.Count > 0)
                        {
                            var vv = json.Where(v => v.CaseID == temp.CaseID);
                            var Result = json.Where(p => !vv.Any(p2 => p2.CaseID == p.CaseID));
                            if (!flg)
                            {
                                foreach (var itm in Result)
                                {
                                    OnlineCaselist.Add(itm);
                                    flg = true;
                                }
                            }
                            var res = vv.Select(v =>
                            {
                                v.CaseCreatedSAM = temp.CaseCreatedSAM;
                                v.CaseCreatedDisplayName = temp.CaseCreatedDisplayName;
                                v.MetaDataCollection = temp.MetaDataCollection;
                                v.CaseTitle = temp.CaseTitle;
                                v.CaseID = temp.CaseID;
                                return v;
                            });
                            if (OnlineCaselist.Where(v => v.CaseID == temp.CaseID)?.Count() == 0)
                            {
                                OnlineCaselist.AddRange(res);
                            }
                            else
                            {
                                OnlineCaselist.Where(v => v.CaseID == temp.CaseID).Select(av =>
                                {
                                    av.CaseCreatedSAM = temp.CaseCreatedSAM;
                                    av.CaseCreatedDisplayName = temp.CaseCreatedDisplayName;
                                    av.MetaDataCollection = temp.MetaDataCollection;
                                    av.CaseTitle = temp.CaseTitle;
                                    av.CaseID = temp.CaseID;
                                    return av;
                                }).OrderByDescending(v => v.CaseID).ToList();
                            }
                        }
                        else
                            OnlineCaselist.Add(temp);
                    }
                    if (!flg && json.Count > 0)
                        OnlineCaselist.AddRange(json);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return OnlineCaselist.OrderByDescending(lst => lst.CaseID).ToList();
        }
        #endregion

        #region Get CaseList To Me
        //public static async Task<List<GetCaseTypesResponse.BasicCase>> GetCaseListToMe(bool _Isonline, string screenName, string _DBPath, string _user, string _CaseTypeID, string _CaseOwnerSAM, string _AssignedToSAM, string _ClosedBySAM, string _CreatedBySAM, string _PropertyID, string _TenantCode, string _TenantID, string _showOpenClosedCasesType, string _showPastDueDate, string _SearchQuery, int _InstanceUserAssocId)
        //{
        //    List<GetCaseTypesResponse.BasicCase> lstResult = new List<GetCaseTypesResponse.BasicCase>();
        //    screenName = "E2_GetCaseList" + screenName;

        //    try
        //    {
        //        if (_Isonline)
        //        {
        //            var results = CasesAPIMethods.GetCaseList(_user, _CaseTypeID, _CaseOwnerSAM, _AssignedToSAM, _ClosedBySAM, _CreatedBySAM, _PropertyID, _TenantCode, _TenantID, _showOpenClosedCasesType, _showPastDueDate, _SearchQuery);
        //            var temp = results.GetValue("ResponseContent");

        //            if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
        //            {
        //                lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(temp.ToString());
        //                if (lstResult.Count > 0)
        //                {
        //                    Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoListByTypeID_SystemName(0, CasesInstance, screenName, _DBPath);
        //                    result.Wait();

        //                    if (!string.IsNullOrEmpty(result.Result?.ToString()) && result.Result.ToString() != "[]")
        //                    {
        //                        string lst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[0];
        //                        var lstResults = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(lst);

        //                        string lstt = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[1];
        //                        var templstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(lstt);

        //                        string lstnote = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[2];
        //                        var lstResultsnotes = JsonConvert.DeserializeObject<List<List<GetCaseNotesResponse.NoteData>>>(lstnote);

        //                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResults) + "|||" + JsonConvert.SerializeObject(templstResult) + "|||" + JsonConvert.SerializeObject(lstResultsnotes), CasesInstance, screenName, _InstanceUserAssocId, _DBPath, result.Result.APP_TYPE_INFO_ID, "", "M", "", 0, "", "", true);
        //                    }

        //                }
        //            }
        //        }

        //        else
        //        {
        //            Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoListByTypeID_SystemName(0, CasesInstance, screenName, _DBPath);
        //            result.Wait();

        //            if (!string.IsNullOrEmpty(result.Result?.ToString()) && result.Result.ToString() != "[]")
        //            {
        //                string lst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[0];
        //                lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(lst);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return lstResult.OrderByDescending(lst => lst.CaseID).ToList();
        //}
        #endregion

        #region Get CaseList To Me Metadata
        public static async Task<List<GetCaseTypesResponse.CaseData>> GetCaseListToMeMetadata(bool _Isonline, string _CaseID, string _screenName, string _DBPath, string _userName, string _caseTypeId, int _InstanceUserAssocId)
        {
            List<GetCaseTypesResponse.CaseData> lstResult = new List<GetCaseTypesResponse.CaseData>();
            _screenName = "E2_GetCaseList" + _screenName;

            try
            {
                if (_Isonline)
                {
                    var results = CasesAPIMethods.GetCaseBasicInfo(_userName, _CaseID);
                    var temp = results.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        var lstResulttemp = JsonConvert.DeserializeObject<GetCaseTypesResponse.CaseData>(temp.ToString());
                        lstResult.Add(lstResulttemp);
                        if (lstResult != null)
                        {
                            Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoListByTypeID_SystemName(0, CasesInstance, _screenName, _DBPath);

                            if (!string.IsNullOrEmpty(result.Result?.ToString()) && result.Result.ToString() != "[]")
                            {
                                string lstcaselst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[0];
                                var templstcaselst = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(lstcaselst);


                                string lst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[1];
                                var templstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(lst);

                                var lstResults = templstResult.Where(v => v.CaseID == Convert.ToInt32(_CaseID)).ToList();

                                List<GetCaseTypesResponse.CaseData> ls = new List<GetCaseTypesResponse.CaseData>();
                                int i = templstResult.IndexOf(templstResult.Single(v => v.CaseID == Convert.ToInt32(_CaseID)));
                                templstResult.RemoveAt(i);
                                ls.Add(lstResulttemp);
                                templstResult.AddRange(ls);

                                //templstResult = templstResult.Except(lstResults).ToList();
                                //templstResult.AddRange(lstResult);

                                string lstnote = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[2];
                                var lstResultsnotes = JsonConvert.DeserializeObject<List<List<GetCaseNotesResponse.NoteData>>>(lstnote);


                                var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(templstcaselst) + "|||" + JsonConvert.SerializeObject(templstResult) + "|||" + JsonConvert.SerializeObject(lstResultsnotes), CasesInstance, _screenName, _InstanceUserAssocId, _DBPath, result.Result.APP_TYPE_INFO_ID, "", "M", "", 0, "", "", true);
                            }
                        }
                    }


                }
                else
                {
                    Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoListByTypeID_SystemName(0, CasesInstance, _screenName, _DBPath);
                    result.Wait();
                    if (!string.IsNullOrEmpty(result.Result?.ToString()) && result.Result.ToString() != "[]")
                    {
                        string lst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[1];
                        var templstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(lst);
                        lstResult = templstResult.Where(v => v.CaseID == Convert.ToInt32(_CaseID)).ToList();
                        //lstResult = lstResult == null ? new List<GetCaseTypesResponse.CaseData>() : lstResult;

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return lstResult.OrderByDescending(lst => lst.CaseID).ToList();
        }
        #endregion

        #region Get CaseList To Me Metadata Notes
        public static async Task<List<GetCaseNotesResponse.NoteData>> GetCaseListToMeMetadataNotes(bool _Isonline, string _CaseID, string _screenName, string _DBPath, int _InstanceUserAssocId)
        {
            List<GetCaseNotesResponse.NoteData> lstResult = new List<GetCaseNotesResponse.NoteData>();
            _screenName = "E2_GetCaseList" + _screenName;

            try
            {
                if (_Isonline)
                {
                    var results = CasesAPIMethods.GetCaseNotes(_CaseID, DateTime.Now.ToString("yyyy/MM/dd"));
                    var temp = results.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseNotesResponse.NoteData>>(temp.ToString());
                        if (lstResult != null)
                        {
                            Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoListByTypeID_SystemName(0, CasesInstance, _screenName, _DBPath);

                            if (!string.IsNullOrEmpty(result.Result?.ToString()) && result.Result.ToString() != "[]")
                            {
                                string lstcaselst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[0];
                                var templstcaselst = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(lstcaselst);


                                string lst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[1];
                                var templstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(lst);

                                string lstnote = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[2];

                                var templstResultnote = JsonConvert.DeserializeObject<List<List<GetCaseNotesResponse.NoteData>>>(lstnote);

                                List<List<GetCaseNotesResponse.NoteData>> ls = new List<List<GetCaseNotesResponse.NoteData>>();

                                List<GetCaseNotesResponse.NoteData> temps = templstResultnote.SelectMany(v => v).ToList();

                                List<GetCaseNotesResponse.NoteData> lstResultss = temps.Where(v => v.CaseID == Convert.ToInt32(_CaseID)).ToList();

                                int i = templstResult.IndexOf(templstResult.Where(v => v.CaseID == Convert.ToInt32(_CaseID)).FirstOrDefault());

                                templstResultnote.RemoveAt(i);
                                ls.Add(lstResult);
                                templstResultnote.AddRange(ls);


                                var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(templstcaselst) + "|||" + JsonConvert.SerializeObject(templstResult)
                                    + "|||" + JsonConvert.SerializeObject(templstResultnote), CasesInstance, _screenName, _InstanceUserAssocId, _DBPath, result.Result.APP_TYPE_INFO_ID, "", "M", "", 0, "", "", true);
                            }
                        }
                    }


                }
                else
                {
                    Task<AppTypeInfoList> result = DBHelper.GetAppTypeInfoListByTypeID_SystemName(0, CasesInstance, _screenName, _DBPath);
                    result.Wait();
                    if (!string.IsNullOrEmpty(result.Result?.ToString()) && result.Result.ToString() != "[]")
                    {
                        string lst = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[2];
                        var templstResult = JsonConvert.DeserializeObject<List<List<GetCaseNotesResponse.NoteData>>>(lst);

                        string lstt = Convert.ToString(result.Result.ASSOC_FIELD_INFO).Split(new string[] { "|||" }, StringSplitOptions.None)[1];
                        var templstResults = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(lstt);

                        int i = templstResults.IndexOf(templstResults.Where(v => v.CaseID == Convert.ToInt32(_CaseID)).FirstOrDefault());
                        lstResult = templstResult[i];

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return lstResult.OrderByDescending(lst => lst.CaseID).ToList();
        }
        #endregion



        #region Get AssocCascade Info By CaseType
        public static async Task<List<AssocCascadeInfo>> GetAssocCascadeInfo(bool _IsOnline, string _caseTypeID, int _InstanceUserAssocId, string _DBPath)
        {
            List<AssocCascadeInfo> lstResult = new List<AssocCascadeInfo>();
            int id = CommonConstants.GetResultBySytemcode(CasesInstance, "C4_GetAssocCascadeInfoByCaseType", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetAssocCascadeInfoByCaseType(_caseTypeID);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(temp.ToString());
                        //if (lstResult.Count > 0)
                        //{
                        //    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetAssocCascadeInfoByCaseType", _InstanceUserAssocId, _DBPath, id, _caseTypeID, "M");
                        //}
                    }
                }
                else
                {
                    // var GetOriginationCenterForUser = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(CasesInstance, "C4_GetAssocCascadeInfoByCaseType", _DBPath);
                    // GetOriginationCenterForUser.Wait();
                    //lstResult = GetOriginationCenterForUser.Result?.ASSOC_FIELD_INFO.ToString() == null ? null : JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(GetOriginationCenterForUser.Result?.ASSOC_FIELD_INFO.ToString());
                    //lstResult = CommonConstants.ReturnListResult<AssocCascadeInfo>(CasesInstance, "C4_GetAssocCascadeInfoByCaseType", _DBPath);
                }
            }
            catch (Exception ex)
            {

                //throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get All External Datasource List
        public static async Task<List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>> GetAllExternalDatasourceList(bool _IsOnline, string _CaseTypeID, string _AssocTypeID, string _user, int _InstanceUserAssocId, string _DBPath)
        {
            List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST> lstResult = new List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>();
            int id = CommonConstants.GetResultBySytemcode(CasesInstance, "C7_GetAllExternalDatasourceList", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetAllExternalDatasourceList(_CaseTypeID, _AssocTypeID, _user);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C7_GetAllExternalDatasourceList", _InstanceUserAssocId, _DBPath, id, _CaseTypeID, "M");
                        }
                    }
                }
                else
                {
                    var GetOriginationCenterForUser = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(CasesInstance, "C7_GetAllExternalDatasourceList", _DBPath);
                    GetOriginationCenterForUser.Wait();
                    //lstResult = GetOriginationCenterForUser.Result?.ASSOC_FIELD_INFO.ToString() == null ? null : JsonConvert.DeserializeObject<List<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>>(GetOriginationCenterForUser.Result?.ASSOC_FIELD_INFO.ToString());
                    lstResult = CommonConstants.ReturnListResult<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>(CasesInstance, "C7_GetAllExternalDatasourceList", _DBPath);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Case Activity
        public static async Task<List<GetCaseActivityResponse.Activity>> GetCaseActivity(bool _IsOnline, string _caseID, int _CaseTypeId, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetCaseActivityResponse.Activity> lstResult = new List<GetCaseActivityResponse.Activity>();
            /// int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C10_GetCaseActivity", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetCaseActivity(_caseID);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseActivityResponse.Activity>>(temp.ToString());

                        if (lstResult.Count > 0)
                        {
                            /*vishalpr*/
                            Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_CaseTypeId), Convert.ToInt32(_caseID), _DBPath, "C10_GetCaseActivity", "M", null);
                            onlineRecord.Wait();
                            int id = 0;
                            if (onlineRecord?.Result?.Count > 0)
                                id = Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);

                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C10_GetCaseActivity", _InstanceUserAssocId, _DBPath, id, Convert.ToString(_CaseTypeId), "M", _caseID);

                        }
                    }
                }
                else
                {
                    Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_CaseTypeId), Convert.ToInt32(_caseID), _DBPath, "C10_GetCaseActivity", "M", null);
                    onlineRecord.Wait();
                    if (onlineRecord?.Result?.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                        {
                            lstResult = JsonConvert.DeserializeObject<List<GetCaseActivityResponse.Activity>>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                        }
                    }

                    // lstResult = CommonConstants.ReturnListResult<GetCaseActivityResponse.Activity>(CasesInstance, "C10_GetCaseActivity", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        //#region Get Case Activity
        //public static async Task<List<GetCaseActivityResponse.Activity>> GetCaseActivity(bool _IsOnline, string _caseID, int _CaseTypeId, int _InstanceUserAssocId, string _DBPath)
        //{
        //    List<GetCaseActivityResponse.Activity> lstResult = new List<GetCaseActivityResponse.Activity>();
        //    /// int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C10_GetCaseActivity", _DBPath);
        //    try
        //    {
        //        if (_IsOnline)
        //        {
        //            var result = CasesAPIMethods.GetCaseActivity(_caseID);
        //            var temp = result.GetValue("ResponseContent");

        //            if (temp != null && temp.ToString() != "[]")
        //            {
        //                lstResult = JsonConvert.DeserializeObject<List<GetCaseActivityResponse.Activity>>(temp.ToString());

        //                if (lstResult.Count > 0)
        //                {

        //                    ActivityDetails log = new ActivityDetails
        //                    {
        //                        ActivityId = 0,
        //                        ActivityJson = JsonConvert.SerializeObject(temp),
        //                        ID = Convert.ToInt32(_caseID),
        //                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
        //                        SYSTEM = ConstantsSync.CasesInstance,
        //                        TYPE_ID = Convert.ToInt32(_CaseTypeId),
        //                        ActivityType = "C10_GetCaseActivity",
        //                        LAST_MODIFIED_DATETIME = DateTime.Now
        //                    };

        //                    var Rec = DBHelper.GetActivityDetails(CasesInstance, Convert.ToInt32(_caseID), Convert.ToInt32(_CaseTypeId), _DBPath);
        //                    Rec.Wait();
        //                    if (Rec.Result != null)
        //                    {
        //                        log.ActivityId = Rec.Result.ActivityId;
        //                    }
        //                    DBHelper.SaveActivityDetails(log, _DBPath).Wait();

        //                    ///*vishalpr*/
        //                    //Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_CaseTypeId), Convert.ToInt32(_caseID), _DBPath, "C10_GetCaseActivity", "M", null);
        //                    //onlineRecord.Wait();
        //                    //int id = 0;
        //                    //if (onlineRecord?.Result?.Count > 0)
        //                    //    id = Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);

        //                    //var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C10_GetCaseActivity", _InstanceUserAssocId, _DBPath, id, Convert.ToString(_CaseTypeId), "M", _caseID);

        //                }
        //            }
        //        }
        //        else
        //        {
        //            Task<ActivityDetails> onlineRecord = DBHelper.GetActivityDetails(CasesInstance, Convert.ToInt32(_caseID), Convert.ToInt32(_CaseTypeId), _DBPath);
        //            onlineRecord.Wait();
        //            if (onlineRecord?.Result != null)
        //            {
        //                if (!string.IsNullOrEmpty(onlineRecord.Result.ActivityJson))
        //                {
        //                    lstResult = JsonConvert.DeserializeObject<List<GetCaseActivityResponse.Activity>>(onlineRecord.Result.ActivityJson);
        //                }
        //            }

        //            // lstResult = CommonConstants.ReturnListResult<GetCaseActivityResponse.Activity>(CasesInstance, "C10_GetCaseActivity", _DBPath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // throw ex;
        //    }
        //    return lstResult;
        //}
        //#endregion

        #region Get Types For List By CaseTypeID
        public static async Task<List<GetCaseTypesResponse.ItemType>> GetTypesForListByCaseTypeID(bool _IsOnline, string _caseTypeId, string _userName, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetCaseTypesResponse.ItemType> lstResult = new List<GetCaseTypesResponse.ItemType>();
            int id = CommonConstants.GetResultBySytemcodeId(ConstantsSync.CasesInstance, "C4_GetTypesForListByCaseTypeID", Convert.ToInt32(_caseTypeId), _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetTypesForListByCaseTypeID(_caseTypeId, _userName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.ItemType>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetTypesForListByCaseTypeID", _InstanceUserAssocId, _DBPath, id, _caseTypeId, "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<GetCaseTypesResponse.ItemType>(CasesInstance, "C4_GetTypesForListByCaseTypeID", _DBPath);
                    lstResult = lstResult == null ? new List<GetCaseTypesResponse.ItemType>() : lstResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get ExternalDataSource By Id
        public static async Task<List<GetExternalDataSourceByIdResponse.ExternalDatasource>> GetExternalDataSourceById(bool _IsOnline, string _ExternalDatasourceID, string _SystemCode, int _InstanceUserAssocId, string _DBPath, int _caseTypeID = 0, int assoctypeId = 0)
        {
            List<GetExternalDataSourceByIdResponse.ExternalDatasource> lstResult = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetExternalDataSourceById(_ExternalDatasourceID, _SystemCode);
                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(Convert.ToString(temp)) && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetExternalDataSourceByIdResponse.ExternalDatasource>>(temp.ToString());
                    }
                }
                else
                {
                    #region From EDS_RESULT Table
                    //var GetAppTypeInfo = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(CasesInstance, "C1_C2_CASES_CASETYPELIST", _caseTypeID, _DBPath, null);
                    //GetAppTypeInfo.Wait();

                    //Task<EDSResultList> Result = DBHelper.GetEDSResultListwithId(Convert.ToInt32(assoctypeId), Convert.ToInt32(GetAppTypeInfo?.Result?.APP_TYPE_INFO_ID), _DBPath);
                    //Result.Wait();
                    //if (Result?.Result?.ASSOC_FIELD_ID > 0)
                    //{
                    //    string jsonvalue = Result.Result.EDS_RESULT;
                    //    lstResult = JsonConvert.DeserializeObject<List<GetExternalDataSourceByIdResponse.ExternalDatasource>>(jsonvalue);
                    //} 
                    #endregion

                    #region From EDS Cache table
                    var ResultXDS = await DBHelper.GetXDSDetails("CASES", Convert.ToInt32(_ExternalDatasourceID), _DBPath);
                    if (ResultXDS != null)
                    {
                        string jsonvalue = ResultXDS.EDS_VALUES;
                        lstResult = JsonConvert.DeserializeObject<List<GetExternalDataSourceByIdResponse.ExternalDatasource>>(jsonvalue);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Type Values By AssocCaseType
        public static async Task<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>> GetTypeValuesByAssocCaseType(bool _IsOnline, object _Body_value, int _InstanceUserAssocId, string _DBPath, string _CaseTypeId)
        {
            List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> lstResult = new List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>();
            //int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetTypeValuesByAssocCaseType", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetTypeValuesByAssocCaseType(_Body_value);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            //  var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetTypeValuesByAssocCaseType", _InstanceUserAssocId, _DBPath, id, _CaseTypeId, "M");
                        }
                    }
                }
                else
                {
                    GetTypeValuesByAssocCaseTypeExternalDSRequest.GetTypeValuesByAssocCaseTypeExternalDS Request = (GetTypeValuesByAssocCaseTypeExternalDSRequest.GetTypeValuesByAssocCaseTypeExternalDS)_Body_value;

                    List<EDSResultList> Result = DBHelper.EDSResultByAssocFieldId(Request.caseTypeID.ToString(), "C1_C2_CASES_CASETYPELIST", Request.assocCaseTypeID, ConstantsSync.CasesInstance, _DBPath).Result;

                    lstResult = JsonConvert.DeserializeObject<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>>(Result.FirstOrDefault().EDS_RESULT);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Connection String
        public static async Task<List<ConnectionStringCls>> GetConnectionString(bool _IsOnline, string _ExternalDatasourceID, int _InstanceUserAssocId, string _DBPath, string _CasetypeId)
        {
            List<ConnectionStringCls> lstResult = new List<ConnectionStringCls>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetConnectionString", _DBPath);

            try
            {
                //if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetConnectionString(_ExternalDatasourceID);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<ConnectionStringCls>>(temp.ToString());

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get ExternalDataSource Items By Id
        public static async Task<List<ExternalDatasourceInfo>> GetExternalDataSourceItemsById(bool _IsOnline, string _ExternalDatasourceID, int _InstanceUserAssocId, string _DBPath, string _CasetypeId)
        {
            List<ExternalDatasourceInfo> lstResult = new List<ExternalDatasourceInfo>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetExternalDataSourceItemsById", _DBPath);

            try
            {
                //if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetExternalDataSourceItemsById(_ExternalDatasourceID);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<ExternalDatasourceInfo>>(temp.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Item Value From Query Exd
        public static async Task<List<GetCaseTypesResponse.ExternalObjectDataItem>> GetItemValueFromQueryExd(bool _IsOnline, string _connectionString, string _query, string _DBPath)
        {
            List<GetCaseTypesResponse.ExternalObjectDataItem> lstResult = new List<GetCaseTypesResponse.ExternalObjectDataItem>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetItemValueFromQueryExd", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetItemValueFromQueryExd(_connectionString, _query);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.ExternalObjectDataItem>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            //var inserted = CommonConstants.AddRecordOfflineStore(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetItemValueFromQueryExd", _InstanceUserAssocId, _DBPath, id, _CasetypeId, "M");
                        }
                    }
                }
                else
                {
                    //lstResult = CommonConstants.ReturnListResult<ExternalDatasourceInfo>(CasesInstance, "C4_GetItemValueFromQueryExd", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Filter Query
        public static async Task<List<ConnectionStringCls>> GetFilterQueryCases(bool _IsOnline, string _ExternalDatasourceID, int _InstanceUserAssocId, string _DBPath, string _CasetypeId)
        {
            List<ConnectionStringCls> lstResult = new List<ConnectionStringCls>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetFilterQueryCases", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetFilterQueryCases(_ExternalDatasourceID);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<ConnectionStringCls>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetFilterQueryCases", _InstanceUserAssocId, _DBPath, id, _CasetypeId, "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<ConnectionStringCls>(CasesInstance, "C4_GetFilterQueryCases", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Values Query And Connection
        public static async Task<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>> GetValuesQueryAndConnection(bool _IsOnline, string _ExternalDatasourceID, int _InstanceUserAssocId, string _DBPath, string _CasetypeId, string _assocCaseTypeID, string _caseTypeDesc, string _connectionString, string _query)
        {
            List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> lstResult = new List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetValuesQueryAndConnection", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetValuesQueryAndConnection(_CasetypeId, _assocCaseTypeID, _caseTypeDesc, "Y", _connectionString, _query);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            //var inserted = CommonConstants.AddRecordOfflineStore(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetValuesQueryAndConnection", _InstanceUserAssocId, _DBPath, id, _CasetypeId, "M");
                        }
                    }
                }
                else
                {
                    //lstResult = CommonConstants.ReturnListResult<GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>(CasesInstance, "C4_GetValuesQueryAndConnection", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Case Basic Information
        //view case
        public static async Task<List<GetCaseTypesResponse.CaseData>> GetCaseBasicInfo(bool _IsOnline, string _UserName, string _CaseID, int _InstanceUserAssocId, string _DBPath, string _CasetypeId, string screenname = "")
        {
            List<GetCaseTypesResponse.CaseData> lstResult = new List<GetCaseTypesResponse.CaseData>();
            var tempid = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(ConstantsSync.CasesInstance, Convert.ToInt32(_CasetypeId), 0, _DBPath, "C8_GetCaseBasicInfo", Convert.ToInt32(_CaseID));
            tempid.Wait();
            int? id = tempid.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID;
            //int Masterid = CommonConstants.GetResultBySytemcodeId(CasesInstance, "C1_C2_CASES_CASETYPELIST", Convert.ToInt32(_CasetypeId), _DBPath);
            try
            {

                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetCaseBasicInfo(_UserName, _CaseID);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        var lstResulttemp = JsonConvert.DeserializeObject<GetCaseTypesResponse.CaseData>(temp.ToString());
                        lstResult.Add(lstResulttemp);
                        if (lstResult.Count > 0)
                        {
                            /*vishalpr*/
                            /*To avoid Duplcate Record for C8 */

                            var Record = DBHelper.GetAppTypeInfoList(CasesInstance, lstResulttemp.CaseID, lstResulttemp.CaseTypeID, "C8_GetCaseBasicInfo", _DBPath, null);
                            Record.Wait();

                            int InserID = 0;
                            if (Record.Result == null)
                            {
                                InserID = 0;
                                //var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C8_GetCaseBasicInfo", _InstanceUserAssocId, _DBPath, 0, _CasetypeId, "M", Convert.ToString(lstResult?.FirstOrDefault()?.CaseID), 4, "", "", true);
                            }
                            else
                            {
                                InserID = Record.Result.APP_TYPE_INFO_ID;
                                //var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C8_GetCaseBasicInfo", _InstanceUserAssocId, _DBPath, Record.Result.APP_TYPE_INFO_ID, _CasetypeId, Record.Result.TransactionType, Convert.ToString(Record.Result.ID), 5, "", "", true);
                            }
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C8_GetCaseBasicInfo", _InstanceUserAssocId, _DBPath, InserID, _CasetypeId, "M", Convert.ToString(lstResult?.FirstOrDefault()?.CaseID), 4, "", "", true);
                        }
                    }
                }
                else
                {
                    Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, Convert.ToInt32(_CasetypeId), Convert.ToInt32(_CaseID), _DBPath, "C8_GetCaseBasicInfo", "M", null);
                    onlineRecord.Wait();
                    if (onlineRecord?.Result?.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                        {
                            try
                            {
                                lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                            }
                            catch (Exception ex)
                            {

                                var obj = JsonConvert.DeserializeObject<GetCaseTypesResponse.CaseData>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                lstResult.Add(obj);
                            }
                        }
                    }

                    Task<List<AppTypeInfoList>> offlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, int.Parse(_CasetypeId), int.Parse(_CaseID), _DBPath, "C8_GetCaseBasicInfo", "T", null);

                    offlineRecord.Wait();
                    if (offlineRecord?.Result?.Count > 0)
                    {
                        foreach (var item in offlineRecord.Result)
                        {
                            if (item.ASSOC_FIELD_INFO.ToString() == "[]")
                            {
                                continue;
                            }
                            else
                            {
                                try
                                {
                                    lstResult.AddRange(JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseData>>(item.ASSOC_FIELD_INFO));
                                }
                                catch (Exception)
                                {
                                    var obj = JsonConvert.DeserializeObject<GetCaseTypesResponse.CaseData>(item.ASSOC_FIELD_INFO);
                                    lstResult.Add(obj);
                                }
                            }

                            #region MyRegion
                            //GetCaseTypesResponse.BasicCase tempBasicCase = new GetCaseTypesResponse.BasicCase();
                            //try
                            //{
                            //    tempBasicCase = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(offlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                            //}
                            //catch
                            //{
                            //    try
                            //    {
                            //        var tempresult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(offlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                            //        tempBasicCase = tempresult.Where(v => v.CaseID == offlineRecord?.Result.FirstOrDefault().ID).FirstOrDefault();
                            //        if (tempBasicCase == null)
                            //            tempBasicCase = tempresult.Where(v => v.CaseID == offlineRecord?.Result.FirstOrDefault().ID).LastOrDefault();
                            //    }
                            //    catch
                            //    {
                            //        var tempresult1 = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(offlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                            //        tempBasicCase = tempresult1.FirstOrDefault();
                            //    }
                            //}

                            //GetCaseTypesResponse.CaseData temp = new GetCaseTypesResponse.CaseData()
                            //{
                            //    CaseID = tempBasicCase.CaseID,
                            //    CaseAssignedDateTime = Convert.ToDateTime(tempBasicCase.CaseAssignDateTime),
                            //    CaseAssignedTo = tempBasicCase.CaseAssignedToDisplayName,
                            //    CaseAssignedToDisplayName = tempBasicCase.CaseAssignedToDisplayName,
                            //    CaseTypeID = tempBasicCase.CaseTypeID,
                            //    CreateBy = _UserName,
                            //    ListID = tempBasicCase.ListID,
                            //    CaseOwner = tempBasicCase.CaseOwnerSAM,
                            //    CaseOwnerDateTime = Convert.ToDateTime(tempBasicCase.CaseAssignDateTime),
                            //    MetaDataCollection = tempBasicCase.MetaDataCollection,
                            //    ModifiedBy = tempBasicCase.CaseModifiedByDisplayName,
                            //    ModifiedByDisplayName = tempBasicCase.CaseModifiedByDisplayName,
                            //    ModifiedDateTime = Convert.ToDateTime(tempBasicCase.CaseModifiedDateTime),
                            //    CaseClosedBy = tempBasicCase.CaseClosedByDisplayName,
                            //    CaseClosedDateTime = tempBasicCase.CaseClosedDateTime == "" ? DateTime.Now : Convert.ToDateTime(tempBasicCase.CaseClosedDateTime),
                            //    CaseOwnerDisplayName = tempBasicCase.CaseOwnerDisplayName,
                            //    CaseTypeName = tempBasicCase.CaseTypeName,
                            //    CreateDateTime = Convert.ToDateTime(tempBasicCase.CaseCreatedDateTime),
                            //    CreateByDisplayName = tempBasicCase.CaseCreatedDisplayName,
                            //};
                            ////lstResult.Add(temp);
                            //lstResult = lstResult.Where(v => v.CaseID == temp.CaseID).Select(av =>
                            //{
                            //    av.CreateBy = av.CreateBy == "" ? temp.CreateBy : av.CreateBy;
                            //    av.CreateByDisplayName = av.CreateByDisplayName == "" ? temp.CreateByDisplayName : av.CreateByDisplayName;
                            //    av.CaseAssignedTo = temp.CaseAssignedTo;
                            //    av.CaseAssignedToDisplayName = temp.CaseAssignedToDisplayName;
                            //    return av;
                            //}).ToList();
                            #endregion

                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            return lstResult;
        }
        #endregion

        #region Get Types By CaseTypeID Raw
        public static async Task<List<spi_MobileApp_GetTypesByCaseTypeResult>> GetTypesByCaseTypeIDRaw(bool _IsOnline, string _CaseTypeId, int _InstanceUserAssocId, string _DBPath, string username)
        {
            List<spi_MobileApp_GetTypesByCaseTypeResult> lstResult = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, C8_GetTypesByCaseTypeIDRaw, _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetTypesByCaseTypeIDRaw(_CaseTypeId, username);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<spi_MobileApp_GetTypesByCaseTypeResult>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, C8_GetTypesByCaseTypeIDRaw, _InstanceUserAssocId, _DBPath, id, _CaseTypeId, "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<spi_MobileApp_GetTypesByCaseTypeResult>(CasesInstance, C8_GetTypesByCaseTypeIDRaw, _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Case Type Data By User
        public static async Task<List<spi_MobileApp_GetCaseTypeDataByUserResult>> GetCaseTypeDataByUser(bool _IsOnline, string _CaseTypeId, int _InstanceUserAssocId, string _DBPath, string username)
        {
            List<spi_MobileApp_GetCaseTypeDataByUserResult> lstResult = new List<spi_MobileApp_GetCaseTypeDataByUserResult>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C3_GetCaseTypeDataByUser", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetCaseTypeDataByUser(_CaseTypeId, username);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<spi_MobileApp_GetCaseTypeDataByUserResult>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C3_GetCaseTypeDataByUser", _InstanceUserAssocId, _DBPath, id, _CaseTypeId, "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<spi_MobileApp_GetCaseTypeDataByUserResult>(CasesInstance, "C3_GetCaseTypeDataByUser", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region GetUserInfo
        public static GetUserInfoResponse.UserInfo GetUserInfo(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath, string Currentuser)
        {
            GetUserInfoResponse.UserInfo lstResult = null;
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.UserProfileInstance, "B2_GetUserInfo", _DBPath);
            int idd = CommonConstants.GetResultBySytemcode(ConstantsSync.UserDetailsInstance, "U1_GetAllEmployeeUser", _DBPath);

            try
            {

                if (_IsOnline)
                {

                    var result = CasesAPIMethods.GetUserInfo(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = new GetUserInfoResponse.UserInfo();
                        lstResult = JsonConvert.DeserializeObject<GetUserInfoResponse.UserInfo>(temp.ToString());
                        if (lstResult != null)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), UserProfileInstance, "B2_GetUserInfo", _InstanceUserAssocId, _DBPath, id, "", "M");

                            var GetResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(UserDetailsInstance, "U1_GetAllEmployeeUser", _DBPath);
                            GetResult.Wait();
                            var list = JsonConvert.DeserializeObject<List<GetUserInfoResponse.UserInfo>>(GetResult.Result.ASSOC_FIELD_INFO.ToString());

                            var lstResultTemp = list.Where(v => v.SAMName.ToLower().Contains(_UserName.ToLower())).FirstOrDefault();

                            var commonRecord = list.Where(v => v.ID == lstResultTemp.ID);
                            commonRecord = commonRecord.Select(v =>
                            {
                                v.ASSIGNED_COUNT = lstResult.ASSIGNED_COUNT;
                                v.ButtonName = lstResult.ButtonName;
                                v.CellPhone = lstResult.CellPhone;
                                v.City = lstResult.City;
                                v.CREATED_COUNT = lstResult.CREATED_COUNT;
                                v.Department = lstResult.Department;
                                v.DisplayName = lstResult.DisplayName;
                                v.Email = lstResult.Email;
                                v.Entity_Count = lstResult.Entity_Count;
                                v.FirstName = lstResult.FirstName;
                                v.ID = lstResult.ID;
                                v.LastName = lstResult.LastName;
                                v.MiddleName = lstResult.MiddleName;
                                v.OfficePhone = lstResult.OfficePhone;
                                v.OWNED_COUNT = lstResult.OWNED_COUNT;
                                v.PrimaryJobTitle = lstResult.PrimaryJobTitle;
                                v.SAMName = _UserName;
                                v.State = lstResult.State;
                                v.Supervisor = lstResult.Supervisor;
                                v.Team_count = lstResult.Team_count;
                                v.UserID = lstResult.UserID;
                                v.SupervisorSAM = lstResult.SupervisorSAM;

                                return v;
                            });


                            list = list.Except(commonRecord).ToList();
                            lstResult = commonRecord.FirstOrDefault();
                            //list.Add(lstResult);
                            list.AddRange(commonRecord);

                            var inserted2 = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(list), UserDetailsInstance, "U1_GetAllEmployeeUser", _InstanceUserAssocId, _DBPath, idd, "", "M");
                        }
                    }

                }
                else
                {
                    if (_UserName == Currentuser)
                    {
                        var GetResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(UserProfileInstance, "B2_GetUserInfo", _DBPath);
                        GetResult.Wait();
                        lstResult = JsonConvert.DeserializeObject<GetUserInfoResponse.UserInfo>(GetResult.Result.ASSOC_FIELD_INFO.ToString());
                    }
                    else
                    {
                        Task<AppTypeInfoList> GetResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(UserDetailsInstance, "U1_GetAllEmployeeUser", _DBPath);
                        GetResult.Wait();
                        var list = JsonConvert.DeserializeObject<List<GetUserInfoResponse.UserInfo>>(GetResult.Result.ASSOC_FIELD_INFO.ToString());
                        lstResult = list.Where(v => v.SAMName.ToLower().Contains(_UserName.ToLower())).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                // throw ex;
            }
            return lstResult;
        }
        #endregion

        #region GetTeamMembers
        public static async Task<List<GetUserInfoResponse.UserInfo>> GetTeamMembers(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetUserInfoResponse.UserInfo> lstResult = new List<GetUserInfoResponse.UserInfo>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.UserProfileInstance, "L1_GetTeamMembers", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetTeamMembers(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetUserInfoResponse.UserInfo>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), UserProfileInstance, "L1_GetTeamMembers", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<GetUserInfoResponse.UserInfo>(UserProfileInstance, "L1_GetTeamMembers", _DBPath);

                    if (lstResult?.Count <= 0)
                    {
                        var GetResult = DBHelper.GetAppTypeInfoListBySystemName(CasesInstance, "E2_GetCaseList_AssignedToMyTeam", _DBPath);
                        GetResult.Wait();

                        var user2 = GetResult.Result.Select(m => m.TM_Username.ToLower()).ToList().Distinct();

                        //var user = GetResult.Result.Select(v => v.TM_Username).ToList().Distinct();
                        foreach (var item in user2)
                        {
                            lstResult.Add(new GetUserInfoResponse.UserInfo() { UserID = item, SAMName = item });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Case Notes
        public static async Task<List<GetCaseNotesResponse.NoteData>> GetCaseNotes(bool _IsOnline, string _CaseId, string _CaseTypeId, int _InstanceUserAssocId, string _DBPath, char ShowOnTop)
        {
            List<GetCaseNotesResponse.NoteData> lstResult = new List<GetCaseNotesResponse.NoteData>();
            //int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetCaseNotes", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetCaseNotes(_CaseId, DateTime.Now.ToString("yyyy/MM/dd"));
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseNotesResponse.NoteData>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            //var tm_uname = DBHelper.GetAppTypeInfo_tmname(CasesInstance, Convert.ToInt32(_CaseId), Convert.ToInt32(_CaseTypeId), "C4_GetCaseNotes", _DBPath).Result?.TM_Username;

                            /*vishalpr*/
                            Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, int.Parse(_CaseTypeId), Convert.ToInt32(_CaseId), _DBPath, "C4_GetCaseNotes", "M", null);
                            onlineRecord.Wait();
                            int id = 0;
                            if (onlineRecord?.Result?.Count > 0)
                                id = Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);

                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetCaseNotes", _InstanceUserAssocId, _DBPath, id, _CaseTypeId, "M", _CaseId);

                        }
                    }
                }
                else
                {
                    Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, int.Parse(_CaseTypeId), Convert.ToInt32(_CaseId), _DBPath, "C4_GetCaseNotes", "M", null);
                    onlineRecord.Wait();
                    if (onlineRecord?.Result?.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                        {
                            lstResult = JsonConvert.DeserializeObject<List<GetCaseNotesResponse.NoteData>>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                        }
                    }

                    if (ShowOnTop.ToString().ToLower() == "y")
                    {
                        lstResult = lstResult.OrderBy(v => v.CaseNoteID).ToList();
                    }


                    Task<List<AppTypeInfoList>> offlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(CasesInstance, int.Parse(_CaseTypeId), int.Parse(_CaseId), _DBPath, "C4_AddNotes", "T", null);
                    offlineRecord.Wait();
                    if (offlineRecord?.Result?.Count > 0)
                    {

                        var OrdrList = offlineRecord.Result.OrderBy(x => x.APP_TYPE_INFO_ID);

                        if (!string.IsNullOrEmpty(OrdrList?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                        {
                            foreach (var itm in OrdrList)
                            {
                                var AddCaseNotes = JsonConvert.DeserializeObject<AddCaseNotesRequest>(itm.ASSOC_FIELD_INFO);
                                GetCaseNotesResponse.UserInfo UserInfo = new GetCaseNotesResponse.UserInfo();
                                UserInfo.DisplayName = AddCaseNotes.currentUserFullName;
                                GetCaseNotesResponse.NoteData nt = new GetCaseNotesResponse.NoteData();
                                nt.CaseID = AddCaseNotes.caseID;
                                nt.CaseNoteID = AddCaseNotes.noteTypeId;
                                nt.CreatedBy = AddCaseNotes.currentUser;
                                nt.CreatedDateTime = AddCaseNotes.createDateTime;
                                nt.Note = AddCaseNotes.note;
                                nt.CreatedByUser = UserInfo;

                                if (ShowOnTop.ToString().ToLower() == "y")
                                {
                                    lstResult.Insert(0, nt);
                                }
                                else
                                {
                                    lstResult.Add(nt);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Hopper With Owner By Username
        //public async static Task<List<GetCaseTypesResponse.BasicCase>> CasesHomeGetCaseListUser(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        //{
        //    List<GetCaseTypesResponse.BasicCase> lstResult = new List<GetCaseTypesResponse.BasicCase>();
        //    int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "B2_CasesHomeGetCaseListUser", _DBPath);

        //    try
        //    {

        //        if (_IsOnline)
        //        {

        //            var result = CasesAPIMethods.CasesHomeGetCaseListUser(_UserName);
        //            var temp = result.GetValue("ResponseContent");

        //            if (temp != null && temp.ToString() != "[]")
        //            {
        //                lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.BasicCase>>(temp.ToString());
        //                if (lstResult.Count > 0)
        //                {

        //                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "B2_CasesHomeGetCaseListUser", _InstanceUserAssocId, _DBPath, id, "", "M");
        //                }
        //            }

        //        }
        //        else
        //        {
        //            lstResult = CommonConstants.ReturnListResult<GetCaseTypesResponse.BasicCase>(CasesInstance, "B2_CasesHomeGetCaseListUser", _DBPath);
        //            lstResult = lstResult == null ? new List<GetCaseTypesResponse.BasicCase>() : lstResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return lstResult;
        //}
        #endregion

        #region Get Case Note Types
        public async static Task<List<Dictionary<string, string>>> GetCaseNoteTypes(bool _IsOnline, string _UserName, string _CasetypeId, int _InstanceUserAssocId, string _DBPath)
        {
            List<Dictionary<string, string>> lstResult = new List<Dictionary<string, string>>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "C4_GetCaseNoteTypes", _DBPath);

            try
            {

                if (_IsOnline)
                {

                    var result = CasesAPIMethods.GetCaseNoteTypes();
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "C4_GetCaseNoteTypes", _InstanceUserAssocId, _DBPath, id, _CasetypeId, "M");
                        }
                    }

                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<Dictionary<string, string>>(CasesInstance, "C4_GetCaseNoteTypes", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Hopper With Owner By Username
        public async static Task<List<GetCaseTypesResponse.HopperInfo>> GetHopperWithOwnerByUsername(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetCaseTypesResponse.HopperInfo> lstResult = new List<GetCaseTypesResponse.HopperInfo>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "K3_GetHopperWithOwnerByUsername", _DBPath);

            try
            {

                if (_IsOnline)
                {

                    var result = CasesAPIMethods.GetHopperWithOwnerByUsername(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.HopperInfo>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "K3_GetHopperWithOwnerByUsername", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }

                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<GetCaseTypesResponse.HopperInfo>(CasesInstance, "K3_GetHopperWithOwnerByUsername", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Hopper Center By User
        public async static Task<List<GetCaseListByHopperResponse.HopperCenterData>> GetHopperCenterByUser(bool _IsOnline, string _UserName, string _showall, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetCaseListByHopperResponse.HopperCenterData> lstResult = new List<GetCaseListByHopperResponse.HopperCenterData>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "E1_GetHopperCenterByUser", _DBPath);

            try
            {

                if (_IsOnline)
                {

                    var result = CasesAPIMethods.GetHopperCenterByUser(_UserName, _showall);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseListByHopperResponse.HopperCenterData>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "E1_GetHopperCenterByUser", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }

                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<GetCaseListByHopperResponse.HopperCenterData>(CasesInstance, "E1_GetHopperCenterByUser", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Hopper Center By User
        public async static Task<List<GetUserInfoResponse.UserInfo>> GetEmployeesBySearch(string _UserName, string _DBPath, string currentuser)
        {
            List<GetUserInfoResponse.UserInfo> lstResult = new List<GetUserInfoResponse.UserInfo>();

            try
            {
                var list = CommonConstants.ReturnListResult<GetUserInfoResponse.UserInfo>(UserDetailsInstance, "U1_GetAllEmployeeUser", _DBPath);

                if (list != null && list?.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(_UserName))
                        return lstResult = list.Where(v => !string.IsNullOrEmpty(v.DisplayName) && v.DisplayName.ToLower().Trim().Contains(_UserName.ToLower())).ToList();
                    else
                        return lstResult;
                }
                else
                {
                    Task.Run(() =>
                    {
                        string str = GetAllEmployeeUser(_DBPath, currentuser);
                    }).Wait();

                    list = CommonConstants.ReturnListResult<GetUserInfoResponse.UserInfo>(UserDetailsInstance, "U1_GetAllEmployeeUser", _DBPath);

                    if (list != null && list?.Count() > 0)
                    {
                        if (!string.IsNullOrEmpty(_UserName))
                            return lstResult = list.Where(v => !string.IsNullOrEmpty(v.DisplayName) && v.DisplayName.ToLower().Trim().Contains(_UserName.ToLower())).ToList();
                        else
                            return lstResult;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Case Types
        public async static Task<List<GetCaseTypesResponse.CaseType>> GetCaseTypes(bool _IsOnline, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetCaseTypesResponse.CaseType> lstResult = new List<GetCaseTypesResponse.CaseType>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.CasesInstance, "E1_GetCaseTypes", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetCaseTypes();
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.CaseType>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), CasesInstance, "E1_GetCaseTypes", _InstanceUserAssocId, _DBPath, id, "", "M");
                        }
                    }

                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<GetCaseTypesResponse.CaseType>(CasesInstance, "E1_GetCaseTypes", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get Search Result
        public static List<SearchResult> GetSearchResult(bool _IsOnline, string _UserName, int? SystemId, int? _TypeId, int? _FieldId, int? _ItemInfoFieldsId, string _SearchText, int? _FromPageIndex, int? _ToPageIndex, int _InstanceUserAssocId, string _DBPath)
        {
            List<SearchResult> SearchResult = new List<SearchResult>();
            try
            {
                SearchRequest sr = new SearchRequest();
                sr.SystemId = SystemId;
                sr.SearchText = _SearchText;
                sr.FromPageIndex = _FromPageIndex;
                sr.ToPageIndex = _ToPageIndex;
                sr.username = _UserName;
                if (_IsOnline)
                {
                    var result = CasesAPIMethods.GetSearchData(sr);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        SearchResult = JsonConvert.DeserializeObject<List<SearchResult>>(temp.ToString());

                        if (SearchResult.Count > 0)
                        {

                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
            return SearchResult;
        }
        #endregion
        public static void AddCasesLogsas(string _dbpath, int ActivityTypeID, string NOTE, string CASE_ID, string CaseTypeID, string CREATED_BY_FULLNAME, string DESCRIPTION, string NAME, string MODIFIED_BY, string SystemCode)
        {
            //try
            //{
            //    GetCaseActivityResponse.Activity responseItem = new GetCaseActivityResponse.Activity();
            //    responseItem.ActivityTypeID = ActivityTypeID;
            //    responseItem.Note = NOTE;
            //    responseItem.CaseID = CASE_ID;
            //    responseItem.SystemCode = SystemCode;
            //    responseItem.CaseTypeID = CaseTypeID;
            //    responseItem.CreatedByFullName = CREATED_BY_FULLNAME;
            //    responseItem.CreatedDateTime = DateTime.Now;
            //    responseItem.Description = DESCRIPTION;
            //    responseItem.IsActive = "Y";
            //    responseItem.Name = NAME;
            //    responseItem.ModifiedBy = MODIFIED_BY;
            //    responseItem.ModifiedDateTime = DateTime.Now;

            //    ActivityDetails log = new ActivityDetails
            //    {
            //        ID = Convert.ToInt32(CASE_ID),
            //        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
            //        SYSTEM = ConstantsSync.CasesInstance,
            //        TYPE_ID = Convert.ToInt32(CaseTypeID),
            //        ActivityType = "C10_GetCaseActivity"
            //    };
            //    List<GetCaseActivityResponse.Activity> lstResult = new List<GetCaseActivityResponse.Activity>();
            //    var Rec = DBHelper.GetActivityDetails(CasesInstance, Convert.ToInt32(CASE_ID), Convert.ToInt32(CaseTypeID), _dbpath);
            //    Rec.Wait();
            //    if (Rec.Result != null)
            //    {
            //        log.ActivityId = Rec.Result.ActivityId;
            //        lstResult = JsonConvert.DeserializeObject<List<GetCaseActivityResponse.Activity>>(Rec.Result.ActivityJson.ToString());
            //    }

            //    lstResult.Add(responseItem);

            //    log.ActivityJson = JsonConvert.SerializeObject(lstResult);

            //    DBHelper.SaveActivityDetails(log, _dbpath).Wait();
            //}
            //catch (Exception)
            //{
            //}
        }

        public static List<AssocCascadeInfo> GetAssocCascadeInfo(bool _IsOnline, string _user, string Casetypeid)
        {
            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
            var AssocType = json.GetValue("ResponseContent");
            return JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());

            return null;
        }
    }
}
