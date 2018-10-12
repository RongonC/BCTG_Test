using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Quest;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;


namespace DataServiceBus.OfflineHelper.DataTypes.Quest
{
    public class QuestSyncAPIMethods
    {
        #region Get All Quest 
        public static string GetAllQuest(string _AreaId, string _UserName, string _DBPath)
        {
            string sError = string.Empty;
            JObject Result = null;
            try
            {
                MobileAPIMethods Mapi = new MobileAPIMethods();
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl  + Common.ConstantsSync.GetAllQuestTypeWithID)
            };
                #endregion

                #region API Body Details
                var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user", _UserName),
            };
                #endregion

                Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
                Debug.WriteLine("GetAllQuest ==> " + Result.ToString());

                if (Result != null)
                {
                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(Result), "Y", "GetAllQuest", _UserName, DateTime.Now.ToString());

                    string ResponseContent = Convert.ToString(Result.GetValue("ResponseContent"));
                    sError = ResponseContent;

                    if (!string.IsNullOrEmpty(ResponseContent) && Convert.ToString(ResponseContent) != "[]" && Convert.ToString(ResponseContent) != "{}" && Convert.ToString(ResponseContent) != "[ ]" && Convert.ToString(ResponseContent) != "{ }" && Convert.ToString(ResponseContent) != "[{ }]" && Convert.ToString(ResponseContent) != "[{}]")
                    {
                        CommonConstants.MasterOfflineStore(ResponseContent, _DBPath);
                    }
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(Result), "N", "GetAllQuest", _UserName, DateTime.Now.ToString());

            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAllEntityTypeWithID", _UserName, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(Result), "N", "GetAllQuest", _UserName, DateTime.Now.ToString());
            }

            return sError;
        }
        #endregion

        #region #1 Get Area List
        public async static Task<List<AreaResponse.Area>> GetAreaList(bool _IsOnline, string user, string AreaId, object _Body_value, int _InstanceUserAssocId, string _DBPath)
        {
            List<AreaResponse.Area> areaList = new List<AreaResponse.Area>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            AreaResponse.Area area = new AreaResponse.Area();
            var GetAreaList = CommonConstants.GetResultBySytemcodeList(QuestInstance, "H1_GetAreaList", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetAreas(AreaId, user);

                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        areaList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AreaResponse.Area>>(temp.ToString());

                    }
                }
                else
                {
                    Task<List<AppTypeInfoList>> Result = DBHelper.GetAppTypeInfoListBySystemName(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    Result.Wait();
                    lstResult = Result?.Result;
                    var sd = lstResult.Select(o => new { o.CategoryId, o.CategoryName }).Distinct().ToList();
                    int cnt = 0;
                    foreach (var item in sd)
                    {
                        var temp = JsonConvert.DeserializeObject<List<ItemInfoField>>(lstResult[cnt].ASSOC_FIELD_INFO).FirstOrDefault();
                        if (item.CategoryId > 0)
                            areaList.Add(new AreaResponse.Area(Convert.ToInt32(item.CategoryId), item.CategoryName, temp.FIELD_SECURITY));
                        cnt++;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return areaList;
        }
        #endregion

        public static void GenerateCaseQueue(bool _IsOnline, object _Body_value)
        {
            if (_IsOnline)
            {
                GenerateCaseQueueRequest obj = (GenerateCaseQueueRequest)_Body_value;
                var Result = QuestAPIMethods.GenerateCaseQueue(obj);
                string temp = Convert.ToString(Result.GetValue("ResponseContent"));
            }
        }

        #region Get Areas FormList
        public async static Task<List<AreaResponse.Area>> GetAreasFormList(bool _IsOnline, string user, string AreaId, object _Body_value, int _InstanceUserAssocId, string _DBPath)
        {
            List<AreaResponse.Area> areaList = new List<AreaResponse.Area>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            AreaResponse.Area area = new AreaResponse.Area();
            var GetAreaList = CommonConstants.GetResultBySytemcodeList(QuestInstance, "H1_GetAreasFormList", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetAreaFormList(user, AreaId);

                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        areaList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AreaResponse.Area>>(temp.ToString());
                        if (areaList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(areaList), QuestInstance, "H1_GetAreasFormList", _InstanceUserAssocId, _DBPath, GetAreaList.Result == null ? 0 : GetAreaList.Result.APP_TYPE_INFO_ID, AreaId, "M");
                        }
                    }
                }
                else
                {
                    areaList = JsonConvert.DeserializeObject<List<AreaResponse.Area>>(GetAreaList.Result.ASSOC_FIELD_INFO.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return areaList;
        }
        #endregion

        #region #2 Get Item List
        //public async static Task<List<ItemsByAreaIDResponse.ItemsByAreaID>> GetItemList(bool _IsOnline, string Sys_Name, int AreaId, string username, object _Body_value, int _InstanceUserAssocId, string _DBPath)
        //{
        //    List<ItemsByAreaIDResponse.ItemsByAreaID> ItemList = new List<ItemsByAreaIDResponse.ItemsByAreaID>();
        //    List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
        //    try
        //    {
        //        if (_IsOnline)
        //        {
        //            var result = QuestAPIMethods.GetItemsByAreaIDFormList(AreaId.ToString(), username);
        //            var temp = result.GetValue("ResponseContent");
        //            if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
        //            {
        //                ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemsByAreaIDResponse.ItemsByAreaID>>(temp.ToString());
        //            }
        //        }
        //        else
        //        {
        //            lstResult = await DBHelper.GetAppTypeInfoListByCategoryId(Sys_Name, AreaId, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);

        //            foreach (var item in lstResult)
        //            {
        //                var ls = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemsByAreaIDResponse.ItemsByAreaID>(item.ASSOC_FIELD_INFO.ToString());
        //                ItemList.Add(ls);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return ItemList;
        //}
        #endregion

        #region Assign Controls
        public static async Task<List<ItemInfoField>> AssignControlsAsync(bool _IsOnline, string _intItemID, string _strShowOnPage, int _InstanceUserAssocId, string _DBPath)
        {
            List<ItemInfoField> lstResult = new List<ItemInfoField>();
            ItemInfoField getypeResult = new ItemInfoField();

            try
            {
                var GetAppTypeInfo = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(_intItemID), _DBPath, null);
                GetAppTypeInfo.Wait();
                if (!string.IsNullOrEmpty(GetAppTypeInfo?.Result?.ASSOC_FIELD_INFO))
                {
                    lstResult = JsonConvert.DeserializeObject<List<ItemInfoField>>(GetAppTypeInfo.Result.ASSOC_FIELD_INFO);
                }
                else
                {
                    if (_IsOnline)
                    {
                        var result = QuestAPIMethods.GetItemInfoFieldsByItemID(_intItemID, _strShowOnPage);
                        var temp = result.GetValue("ResponseContent");

                        if (temp != null && temp.ToString() != "[]")
                        {
                            lstResult = JsonConvert.DeserializeObject<List<ItemInfoField>>(temp.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

            try
            {
                //if (_IsOnline)
                //{
                //    var result = QuestAPIMethods.GetItemInfoFieldsByItemID(_intItemID, _strShowOnPage);
                //    var temp = result.GetValue("ResponseContent");

                //    if (temp != null && temp.ToString() != "[]")
                //    {
                //        lstResult = JsonConvert.DeserializeObject<List<ItemInfoField>>(temp.ToString());
                //        if (lstResult.Count > 0)
                //        {
                //            //var inserted = CommonConstants.AddRecordOfflineStore(JsonConvert.SerializeObject(lstResult), QuestInstance, "H15_GetItemQuestionMetaData", _InstanceUserAssocId, _DBPath, 0, _intItemID, "M");
                //        }
                //    }
                //}
                //else
                //{

                //    if (GetAppTypeInfo.Result != null)
                //    {
                //    }
                //    else
                //    {
                //        GetAppTypeInfo = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(_intItemID), _DBPath, null);
                //        GetAppTypeInfo.Wait();
                //    }
                //    if (!string.IsNullOrEmpty(Convert.ToString(GetAppTypeInfo?.Result)))
                //    {
                //        if (!string.IsNullOrEmpty(GetAppTypeInfo?.Result?.ASSOC_FIELD_INFO))
                //        {
                //            lstResult = JsonConvert.DeserializeObject<List<ItemInfoField>>(GetAppTypeInfo.Result.ASSOC_FIELD_INFO);
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Store And create
        public static async Task<int> StoreAndcreate(bool _IsOnline, int _QuestTypeID, object _Body_value, string _QuestNotes, string _UserName, int _InstanceUserAssocId, string _DBPath, object oQuestionMetaData)
        {
            int insertedRecordid = 0;
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _QuestTypeID, _DBPath);
            string _formTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.TYPE_NAME;
            int? _CatId = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.CategoryId;
            try
            {
                if (_IsOnline)
                {

                    var Result = QuestAPIMethods.AddForm(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], insertedRecordid, insertedRecordid, _DBPath, ConstantsSync.QuestInstance, id);

                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], 0, insertedRecordid, _DBPath, QuestInstance, id, "", "H15_GetItemQuestionMetaData");

                    #region Offline View Form
                    //Task<AppTypeInfoList> GetAppTypeInfo = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(QuestInstance, "H15_GetItemQuestionMetaData", _QuestTypeID, _DBPath);
                    //GetAppTypeInfo.Wait();
                    //List<GetItemInstanceTranResponse.ItemInstanceTran> viewCaseList = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
                    //if (!string.IsNullOrEmpty(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO))
                    {
                        //sviewCaseList = JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO); ;



                        GetItemInstanceTranResponse.ItemInstanceTran viewForm = new GetItemInstanceTranResponse.ItemInstanceTran();
                        var temp = (AddFormRequest)_Body_value;

                        List<string> lsValues = new List<string>();
                        int counter = 0;

                        foreach (var item in temp.ItemInfoFieldValues)
                        {
                            ++counter;

                            viewForm["intCol" + (counter) + "ItemInfoFieldID"] = item.ItemInfoFieldId;
                            viewForm["strCol" + (counter) + "ItemInfoFieldName"] = null;
                            viewForm["strCol" + (counter) + "ItemInfoFieldValue"] = item.ItemInfoFieldData.ItemInfoFieldText;
                        }


                        viewForm.intItemID = _QuestTypeID;
                        viewForm.blnIsEdit = temp.isEdit;
                        viewForm.strItemNAme = _formTitle;
                        viewForm.strFormCretedBy = _UserName;
                        viewForm.FormCredtedDate = DateTime.Now;
                        viewForm.intItemInstanceTranID = insertedRecordid;



                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(viewForm, "H15_GetItemQuestionMetaData", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(insertedRecordid), 0, _formTitle, _CatId, QuestInstance);

                        List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> obj = oQuestionMetaData as List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>;

                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(obj, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(insertedRecordid), 0, _formTitle, _CatId, QuestInstance);
                    }


                    #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Store And Update
        public static async Task<int> StoreAndUpdate(bool _IsOnline, int _QuestTypeID, object _Body_value, string _QuestNotes, string _UserName, int _InstanceUserAssocId, string _DBPath, object _createFormBodyvalue = null, object oQuestionMetaData = null)
        {
            int insertedRecordid = 0;
            string sMode = string.Empty;
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _QuestTypeID, _DBPath);
            int? _CatId = null;
            string _FormTitle = string.Empty;
            if (id == 0)
            {
                Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByTransTypeSyscodewithouttypeid(QuestInstance, Convert.ToInt32(_QuestTypeID), _DBPath, "H15_GetItemQuestionMetaData", "T");
                OfflineList.Wait();
                _FormTitle = OfflineList.Result.FirstOrDefault()?.TYPE_NAME;
                _CatId = OfflineList.Result.FirstOrDefault()?.CategoryId;
                //id = OfflineList.Result.FirstOrDefault().APP_TYPE_INFO_ID;
                //_QuestTypeID = Convert.ToInt32(OfflineList.Result.FirstOrDefault().TYPE_ID);
            }
            else
            {
                _FormTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.TYPE_NAME;
                _CatId = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.CategoryId;
            }
            try
            {
                if (_IsOnline)
                {

                    var Result = QuestAPIMethods.UpdateForm(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[7], insertedRecordid, insertedRecordid, _DBPath, ConstantsSync.QuestInstance, id);

                    var tempRecord = _Body_value as UpdateFormRequest;
                    id = tempRecord.itemInstanceTranId;

                    Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(QuestInstance, _QuestTypeID, id, _DBPath, "H5_GetItemInstanceTran ", "H15_GetItemQuestionMetaData", true);
                    IscheckOnlineRecordApppType.Wait();
                    if (IscheckOnlineRecordApppType.Result.Count > 0)
                    {

                        insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, tempRecord, Constants.UpdateForm, (Enum.GetNames(typeof(ActionTypes)))[7], 0, insertedRecordid, _DBPath, QuestInstance, id, sMode, IscheckOnlineRecordApppType.Result.FirstOrDefault().TYPE_SCREEN_INFO);


                        #region Offline View Form

                        List<GetItemInstanceTranResponse.ItemInstanceTran> viewCaseList = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
                        GetItemInstanceTranResponse.ItemInstanceTran viewForm = new GetItemInstanceTranResponse.ItemInstanceTran();




                        dynamic tempres = (UpdateFormRequest)_Body_value;
                        int counter = 0;
                        int cnt = 0;
                        string stritemInfoFieldValues = tempres.GetType().GetProperty("itemInfoFieldValues").GetValue(tempres, null);
                        string strexternalDatasourceObjectIDs = tempres.GetType().GetProperty("externalDatasourceObjectIDs").GetValue(tempres, null);
                        string stritemInfoFieldIds = tempres.GetType().GetProperty("itemInfoFieldIds").GetValue(tempres, null);

                        foreach (var item in stritemInfoFieldValues.Split(','))
                        {
                            counter++;
                            viewForm["intCol" + (counter) + "ItemInfoFieldID"] = Convert.ToInt32(stritemInfoFieldIds.Split(',')[cnt]);
                            viewForm["strCol" + (counter) + "ItemInfoFieldName"] = strexternalDatasourceObjectIDs.Split(',')[cnt];
                            viewForm["strCol" + (counter) + "ItemInfoFieldValue"] = item;
                            cnt++;
                        }


                        viewForm.intItemID = _QuestTypeID;
                        viewForm.blnIsEdit = tempres.isEdit;
                        viewForm.strItemNAme = _FormTitle;
                        viewForm.strFormCretedBy = _UserName;
                        viewForm.FormCredtedDate = DateTime.Now;
                        viewForm.intItemInstanceTranID = tempres.GetType().GetProperty("itemInstanceTranId").GetValue(tempres, null);
                        viewForm.ItemtransinfoID = insertedRecordid;
                        viewForm.TransactionType = "T";




                        int pkId = 0;
                        Task<List<AppTypeInfoList>> record = null;
                        string screenname = string.Empty;
                        //if (sMode?.ToUpper() == "U")
                        {
                            screenname = "H15_GetItemQuestionMetaData";
                            record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, screenname, "T", id);
                            record.Wait();
                            if (record?.Result.Count == 0)
                            {
                                record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, screenname, "M", id);
                                record.Wait();
                                pkId = Convert.ToInt32(record.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);
                            }
                            else
                                pkId = Convert.ToInt32(record.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);
                        }

                        if (record?.Result.FirstOrDefault()?.IS_ONLINE == true)
                            Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(viewForm, screenname, _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(id), pkId, _FormTitle, _CatId, QuestInstance, IscheckOnlineRecordApppType?.Result?.Count == 0 ? false : true);

                        List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> obj = oQuestionMetaData as List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>;
                        Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, _QuestTypeID, Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", insertedRecordid);
                        Questionlist.Wait();
                        int? iQuestionMetadatId = Questionlist.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID;

                        if (record?.Result.FirstOrDefault()?.IS_ONLINE == true)
                        {
                            var tempr = record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", id);
                            tempr.Wait();
                            int rid = 0;
                            if (tempr?.Result?.Count > 0)
                                rid = tempr.Result.FirstOrDefault().APP_TYPE_INFO_ID;
                            Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(obj, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(id), Convert.ToInt32(rid), _FormTitle, _CatId, QuestInstance, IscheckOnlineRecordApppType?.Result?.Count == 0 ? false : true);
                        }
                        else
                            Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(obj, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(insertedRecordid), Convert.ToInt32(iQuestionMetadatId), _FormTitle, _CatId, QuestInstance);
                    }

                    #endregion
                }
                else
                {
                    sMode = "U";
                    var tempRecord = _Body_value as UpdateFormRequest;
                    id = tempRecord.itemInstanceTranId;

                    //var Records = _createFormBodyvalue as AddFormRequest;

                    Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(QuestInstance, _QuestTypeID, id, _DBPath, "H5_GetItemInstanceTran ", "H15_GetItemQuestionMetaData", true);
                    IscheckOnlineRecordApppType.Wait();
                    if (IscheckOnlineRecordApppType.Result.Count > 0)
                    {

                        insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, tempRecord, Constants.UpdateForm, (Enum.GetNames(typeof(ActionTypes)))[7], 0, insertedRecordid, _DBPath, QuestInstance, id, sMode, IscheckOnlineRecordApppType.Result.FirstOrDefault().TYPE_SCREEN_INFO);
                    }
                    else
                    {
                        var Records = _createFormBodyvalue as AddFormRequest;

                        insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, Records, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[7], 0, insertedRecordid, _DBPath, QuestInstance, id, sMode, "H15_GetItemQuestionMetaData");
                    }

                    //insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[7], 0, insertedRecordid, _DBPath, QuestInstance, id, sMode, "H15_GetItemQuestionMetaData");

                    #region Offline View Form
                    //Task<AppTypeInfoList> GetAppTypeInfo = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(QuestInstance, "H15_GetItemQuestionMetaData", _QuestTypeID, _DBPath);
                    //GetAppTypeInfo.Wait();
                    //if (GetAppTypeInfo.Result == null)
                    //{
                    //    GetAppTypeInfo = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(QuestInstance, Convert.ToInt32(_QuestTypeID), _DBPath, "H15_GetItemQuestionMetaData", "T");
                    //    GetAppTypeInfo.Wait();
                    //}
                    List<GetItemInstanceTranResponse.ItemInstanceTran> viewCaseList = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
                    GetItemInstanceTranResponse.ItemInstanceTran viewForm = new GetItemInstanceTranResponse.ItemInstanceTran();
                    //if (!string.IsNullOrEmpty(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO))
                    {
                        //viewCaseList = JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(GetAppTypeInfo.Result?.ASSOC_FIELD_INFO);

                        dynamic temp;
                        int counter = 0;
                        if (IscheckOnlineRecordApppType?.Result?.Count == 0)
                        {
                            if (sMode?.ToUpper() != "U")
                                temp = (AddFormRequest)_Body_value;
                            else
                            {
                                temp = (AddFormRequest)_createFormBodyvalue;
                            }
                            foreach (var item in temp.ItemInfoFieldValues)
                            {
                                ++counter;

                                viewForm["intCol" + (counter) + "ItemInfoFieldID"] = item.ItemInfoFieldId;
                                viewForm["strCol" + (counter) + "ItemInfoFieldName"] = item.ItemInfoFieldData.externalDatasourceObjectIDs;
                                viewForm["strCol" + (counter) + "ItemInfoFieldValue"] = item.ItemInfoFieldData.ItemInfoFieldText;
                            }


                            viewForm.intItemID = _QuestTypeID;
                            viewForm.blnIsEdit = temp.isEdit;
                            viewForm.strItemNAme = _FormTitle;
                            viewForm.strFormCretedBy = _UserName;
                            viewForm.FormCredtedDate = DateTime.Now;
                            viewForm.intItemInstanceTranID = tempRecord.itemInstanceTranId;
                            viewForm.ItemtransinfoID = insertedRecordid;
                            viewForm.TransactionType = "T";
                        }
                        else
                        {
                            temp = (UpdateFormRequest)_Body_value;
                            counter = 0;
                            int cnt = 0;
                            string stritemInfoFieldValues = temp.GetType().GetProperty("itemInfoFieldValues").GetValue(temp, null);
                            string strexternalDatasourceObjectIDs = temp.GetType().GetProperty("externalDatasourceObjectIDs").GetValue(temp, null);
                            string stritemInfoFieldIds = temp.GetType().GetProperty("itemInfoFieldIds").GetValue(temp, null);

                            foreach (var item in stritemInfoFieldValues.Split(','))
                            {
                                counter++;
                                viewForm["intCol" + (counter) + "ItemInfoFieldID"] = Convert.ToInt32(stritemInfoFieldIds.Split(',')[cnt]);
                                viewForm["strCol" + (counter) + "ItemInfoFieldName"] = strexternalDatasourceObjectIDs.Split(',')[cnt];
                                viewForm["strCol" + (counter) + "ItemInfoFieldValue"] = item;
                                cnt++;
                            }


                            viewForm.intItemID = _QuestTypeID;
                            viewForm.blnIsEdit = temp.isEdit;
                            viewForm.strItemNAme = _FormTitle;
                            viewForm.strFormCretedBy = _UserName;
                            viewForm.FormCredtedDate = DateTime.Now;
                            viewForm.intItemInstanceTranID = temp.GetType().GetProperty("itemInstanceTranId").GetValue(temp, null);
                            viewForm.ItemtransinfoID = insertedRecordid;
                            viewForm.TransactionType = "T";

                        }

                        //List<string> lsValues = new List<string>();
                        //int counter = 0;
                        //foreach (var item in temp.ItemInfoFieldValues)
                        //{
                        //    ++counter;

                        //    viewForm["intCol" + (counter) + "ItemInfoFieldID"] = item.ItemInfoFieldId;
                        //    viewForm["strCol" + (counter) + "ItemInfoFieldName"] = item.ItemInfoFieldData.externalDatasourceObjectIDs;
                        //    viewForm["strCol" + (counter) + "ItemInfoFieldValue"] = item.ItemInfoFieldData.ItemInfoFieldText;
                        //}


                        //viewForm.intItemID = _QuestTypeID;
                        //viewForm.blnIsEdit = temp.isEdit;
                        //viewForm.strItemNAme = _FormTitle;
                        //viewForm.strFormCretedBy = _UserName;
                        //viewForm.FormCredtedDate = DateTime.Now;
                        //viewForm.intItemInstanceTranID = tempRecord.itemInstanceTranId;
                        //viewForm.ItemtransinfoID = insertedRecordid;
                        //viewForm.TransactionType = "T";

                        int pkId = 0;
                        Task<List<AppTypeInfoList>> record = null;
                        string screenname = string.Empty;
                        if (sMode?.ToUpper() == "U")
                        {
                            screenname = "H15_GetItemQuestionMetaData";
                            record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, screenname, "T", id);
                            record.Wait();
                            if (record?.Result.Count == 0)
                            {
                                record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, screenname, "M", id);
                                record.Wait();
                                pkId = Convert.ToInt32(record.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);
                                //screenname = "H15_GetItemQuestionMetaData";
                                //record = DBHelper.GetAppTypeInfoList(QuestInstance, Convert.ToInt32(id), _QuestTypeID, screenname, _DBPath);
                                //record.Wait();
                                //if (record.Result == null)
                                //{
                                //pkId = GetAppTypeInfo.Result.APP_TYPE_INFO_ID;
                                //_QuestTypeID = Convert.ToInt32(GetAppTypeInfo.Result.TYPE_ID);
                                //}
                                //else
                                //    pkId = record.Result.APP_TYPE_INFO_ID;
                            }
                            else
                                pkId = Convert.ToInt32(record.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);
                        }

                        if (record?.Result.FirstOrDefault()?.IS_ONLINE == true)
                            Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(viewForm, screenname, _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(id), pkId, _FormTitle, _CatId, QuestInstance, IscheckOnlineRecordApppType?.Result?.Count == 0 ? false : true);
                        else
                            Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(viewForm, screenname, _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(insertedRecordid), pkId, _FormTitle, _CatId, QuestInstance);

                        List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> obj = oQuestionMetaData as List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>;
                        Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, _QuestTypeID, Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", insertedRecordid);
                        Questionlist.Wait();
                        int? iQuestionMetadatId = Questionlist.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID;

                        if (record?.Result.FirstOrDefault()?.IS_ONLINE == true)
                        {
                            var tempr = record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", id);
                            tempr.Wait();
                            int rid = 0;
                            if (tempr?.Result?.Count > 0)
                            {
                                rid = tempr.Result.FirstOrDefault().APP_TYPE_INFO_ID;
                                Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(obj, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(id), Convert.ToInt32(rid), _FormTitle, _CatId, QuestInstance, IscheckOnlineRecordApppType?.Result?.Count == 0 ? false : true);
                            }
                            else
                            {
                                var temprl = record = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_QuestTypeID), Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "M", id);
                                temprl.Wait();
                                if (temprl?.Result?.Count > 0)
                                {
                                    rid = temprl.Result.FirstOrDefault().APP_TYPE_INFO_ID;
                                    Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(obj, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(id), Convert.ToInt32(rid), _FormTitle, _CatId, QuestInstance, IscheckOnlineRecordApppType?.Result?.Count == 0 ? false : true);
                                }
                            }
                        }
                        else
                            Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(obj, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_QuestTypeID), Convert.ToString(insertedRecordid), Convert.ToInt32(iQuestionMetadatId), _FormTitle, _CatId, QuestInstance);
                    }


                    #endregion

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Store And create File to Question
        public static async Task<int> StoreAndCreateFiletoQuestion(bool _IsOnline, int _QuestTypeID, object _Body_value, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            int insertedRecordid = 0;
            try
            {

                if (_IsOnline)
                {
                    var Result = QuestAPIMethods.AddFiletoQuestion(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], insertedRecordid, insertedRecordid, _DBPath, ConstantsSync.QuestInstance);

                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], 0, insertedRecordid, _DBPath);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Store And create File to Question
        public static async Task<int> StoreAndDeleteFileToQuestion(bool _IsOnline, int _QuestTypeID, object _Body_value, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            int insertedRecordid = 0;
            try
            {

                if (_IsOnline)
                {
                    var Result = QuestAPIMethods.AddFiletoQuestion(_Body_value);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], insertedRecordid, insertedRecordid, _DBPath, ConstantsSync.QuestInstance);

                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], 0, insertedRecordid, _DBPath);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        //#region Store And Case To Question
        //public static async Task<int> StoreAndAddCaseToQuestion(bool _IsOnline, int _QuestTypeID, object _Body_value, string _intItemInstanceTranID, string _intItemQuestionFieldID, string _strNotes, string _strCreatedBy, string _UserName, int _InstanceUserAssocId, string _DBPath)
        //{
        //    int insertedRecordid = 0;
        //    try
        //    {

        //        if (_IsOnline)
        //        {
        //            var Result = QuestAPIMethods.AddCaseToQuestion(_intItemInstanceTranID, _intItemQuestionFieldID, _strNotes, _strCreatedBy);
        //            string temp = Convert.ToString(Result.GetValue("ResponseContent"));
        //            insertedRecordid = Int32.Parse(temp);

        //            int iteminfoid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], insertedRecordid, insertedRecordid, _DBPath, ConstantsSync.QuestInstance);

        //        }
        //        else
        //        {
        //            insertedRecordid = await CommonConstants.AddofflineTranInfo(_QuestTypeID, _Body_value, Constants.AddForm, (Enum.GetNames(typeof(ActionTypes)))[6], 0, insertedRecordid, _DBPath);

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return insertedRecordid;
        //}
        //#endregion

        #region Add Questions Metadata
        public static async Task<int> AddQuestionsMetadata(bool _IsOnline, string _pITEM_INSTANCE_TRAN_ID, string _pITEM_ID, string _pITEM_QUESTION_FIELD_IDs, string _pMEETS_STANDARDS, string _pPOINTS_AVAILABLE, string _pPOINTS_EARNED, string _pIS_CASE_REQUESTED, string _pWEIGHT, string _pNOTES, string _pCREATED_BY, string _UserName, int _InstanceUserAssocId, string _DBPath, object _Body_value, string sMode = "")
        {
            int insertedRecordid = 0;
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(_pITEM_ID), _DBPath);
            string _formTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.TYPE_NAME;
            int? _CatId = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.CategoryId;
            try
            {
                if (_IsOnline)
                {
                    Add_Questions_MetadataRequest obj = JsonConvert.DeserializeObject<Add_Questions_MetadataRequest>(_Body_value.ToString());
                    var Result = QuestAPIMethods.AddQuestionsMetadata(obj);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.AddQuestionsMetadata, ((Enum.GetNames(typeof(ActionTypes)))[6] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), insertedRecordid, insertedRecordid, _DBPath, ConstantsSync.QuestInstance, id
                        );
                }
                else
                {
                    sMode = "U";
                    var tempRecord = JsonConvert.DeserializeObject<Add_Questions_MetadataRequest>(_Body_value.ToString());
                    //sMode = "C";
                    //id = Convert.ToInt32(tempRecord.pITEM_INSTANCE_TRAN_ID);

                    Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID), _DBPath, "H15_GetItemQuestionMetaData", "H15_GetItemQuestionMetaData", true);
                    IscheckOnlineRecordApppType.Wait();

                    //Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddQuestionsMetadata", Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID));
                    //IscheckOnlineRecordApppType.Wait();

                    if (IscheckOnlineRecordApppType.Result.Count > 0)
                    {
                        insertedRecordid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.AddQuestionsMetadata, ((Enum.GetNames(typeof(ActionTypes)))[7] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), 0, Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID), _DBPath, ConstantsSync.QuestInstance, Convert.ToInt32(IscheckOnlineRecordApppType.Result.FirstOrDefault().ID), sMode, IscheckOnlineRecordApppType.Result.FirstOrDefault().TYPE_SCREEN_INFO);
                    }
                    else
                    {
                        IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID), _DBPath, "H15_GetItemQuestionMetaData", "H15_GetItemQuestionMetaData", false);
                        IscheckOnlineRecordApppType.Wait();
                        if (IscheckOnlineRecordApppType.Result.Count > 0)
                        {
                            insertedRecordid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.AddQuestionsMetadata, ((Enum.GetNames(typeof(ActionTypes)))[7] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), 0, Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID), _DBPath, ConstantsSync.QuestInstance, Convert.ToInt32(IscheckOnlineRecordApppType.Result.FirstOrDefault().ID), sMode, IscheckOnlineRecordApppType.Result.FirstOrDefault().TYPE_SCREEN_INFO);
                        }
                    }

                    Task<List<AppTypeInfoList>> temprecordid = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddQuestionsMetadata", Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID));
                    temprecordid.Wait();
                    int pkid = 0;
                    if (temprecordid?.Result?.Count > 0)
                        pkid = temprecordid.Result.FirstOrDefault().APP_TYPE_INFO_ID;



                    //Task<List<AppTypeInfoList>> tempQuestionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_UpdateCaseNotesToQuestion", "T");
                    //tempQuestionlist.Wait();


                    //Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddQuestionsMetadata", "T", tempQuestionlist?.Result?.LastOrDefault()?.ID);
                    //Questionlist.Wait();

                    //Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddQuestionsMetadata", "T", Math.Abs(Convert.ToInt32(insertedRecordid) - Convert.ToInt32(tempRecord.pITEM_INSTANCE_TRAN_ID)));

                    //Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddQuestionsMetadata", "T",pkid);
                    //Questionlist.Wait();
                    //int? iQuestionMetadatId = Questionlist.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID;

                    int iQuestionMetadatId = Convert.ToInt32(temprecordid.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);

                    //Task<AppTypeInfoList> Questionlistol = DBHelper.GetAppTypeInfoListByCatIdol(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath);
                    //Questionlistol.Wait();

                    if (IscheckOnlineRecordApppType.Result?.FirstOrDefault()?.IS_ONLINE == true)
                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(_Body_value, "H15_AddQuestionsMetadata", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(_pITEM_INSTANCE_TRAN_ID), Convert.ToInt32(iQuestionMetadatId), _formTitle, _CatId, QuestInstance, true);
                    else
                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(_Body_value, "H15_AddQuestionsMetadata", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(insertedRecordid), Convert.ToInt32(iQuestionMetadatId), _formTitle, _CatId, QuestInstance, false);

                    //Task<List<AppTypeInfoList>> Questionlistviewscore = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", tempRecord.pITEM_INSTANCE_TRAN_ID);
                    //Questionlistviewscore.Wait();

                    Task<List<AppTypeInfoList>> Questionlistviewscore = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaDataviewscore", Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID));
                    Questionlistviewscore.Wait();

                    //if (Questionlistviewscore?.Result?.Count > 0)
                    //{
                    //    string[] p = tempRecord.pPOINTS_AVAILABLE.Split(',');
                    //    int cnt = 0;
                    //    foreach (var i in p)
                    //    {
                    //        cnt += Convert.ToInt32(i);
                    //    }
                    //    List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> lst = new List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>();
                    //    GetItemCategoriesByItemIDResponse.ItemCategoryByItemId ls = new GetItemCategoriesByItemIDResponse.ItemCategoryByItemId();
                    //    var it = JsonConvert.DeserializeObject<List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>>(Questionlistviewscore?.Result.FirstOrDefault()?.ASSOC_FIELD_INFO);
                    //    var tempit = it;
                    //    it = it.Select(v => new GetItemCategoriesByItemIDResponse.ItemCategoryByItemId
                    //    {
                    //        strAvaliablePoints = Convert.ToString(cnt),
                    //        blnHidePoints = tempit.FirstOrDefault().blnHidePoints,
                    //        blnSuppressAlert = tempit.FirstOrDefault().blnSuppressAlert,
                    //        intDisplayOrder = tempit.FirstOrDefault().intDisplayOrder,
                    //        intItemCategoryID = tempit.FirstOrDefault().intItemCategoryID,
                    //        intItemID = tempit.FirstOrDefault().intItemID,
                    //        strEarned = tempit.FirstOrDefault().strEarned,
                    //        strExtraField1 = tempit.FirstOrDefault().strExtraField1,
                    //        strHeader1 = tempit.FirstOrDefault().strHeader1,
                    //        strHeader2 = tempit.FirstOrDefault().strHeader2,
                    //        strHeader3 = tempit.FirstOrDefault().strHeader3,
                    //        strisActive = tempit.FirstOrDefault().strisActive,
                    //        strIsProcessCMECase = tempit.FirstOrDefault().strIsProcessCMECase,
                    //        strIsShowCMECheckBox = tempit.FirstOrDefault().strIsShowCMECheckBox,
                    //        strItemCategoryDesc = tempit.FirstOrDefault().strItemCategoryDesc,
                    //        strItemCategoryName = tempit.FirstOrDefault().strItemCategoryName,
                    //        strScore = tempit.FirstOrDefault().strScore,
                    //        strSupervisorEmail = tempit.FirstOrDefault().strSupervisorEmail,

                    //    }).ToList();
                    //    Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(it, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(Questionlistviewscore?.Result.FirstOrDefault()?.ID), Convert.ToInt32(Questionlistviewscore?.Result.FirstOrDefault()?.APP_TYPE_INFO_ID), _formTitle, _CatId, QuestInstance);
                    //}

                    if (Questionlistviewscore?.Result?.Count > 0)
                    {
                        //string[] p = tempRecord.pPOINTS_AVAILABLE.Split(',');
                        //int cnt = 0;
                        //foreach (var i in p)
                        //{
                        //    cnt += Convert.ToInt32(i);
                        //}
                        List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> lst = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();
                        GetItemQuestionMetadataResponse.ItemQuestionMetaData ls = new GetItemQuestionMetadataResponse.ItemQuestionMetaData();
                        var it = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Questionlistviewscore?.Result.FirstOrDefault()?.ASSOC_FIELD_INFO);

                        for (int i = 0; i < it.Count; i++)
                        {
                            ls = new GetItemQuestionMetadataResponse.ItemQuestionMetaData()
                            {
                                blnHidePoints = it[i].blnHidePoints,
                                blnIsAutoCase = it[i].blnIsAutoCase,
                                blnSuppressAlert = it[i].blnSuppressAlert,
                                blnIsEdit = it[i].blnIsEdit,
                                blnIsHighlightRow = it[i].blnIsHighlightRow,
                                intAreaID = it[i].intAreaID,
                                intCaseID = it[i].intCaseID,
                                intItemInstanceTranID = it[i].intItemInstanceTranID,
                                intItemQuestionFieldID = it[i].intItemQuestionFieldID,
                                intItemQuestionMetadataID = it[i].intItemQuestionMetadataID,
                                intListID = it[i].intListID,
                                intTrainingID = it[i].intTrainingID,
                                strAreaName = it[i].strAreaName,
                                strIsCaseRequested = tempRecord.pIS_CASE_REQUESTED.Split(',')[i],
                                strItemName = it[i].strItemName,
                                strMeetsStandards = tempRecord.pMEETS_STANDARDS.Split(',')[i],
                                strNotes = tempRecord.pNOTES.Split(',')[i],
                                strOtherComments = it[i].strOtherComments,
                                strPointsAvailable = Convert.ToDecimal(tempRecord.pPOINTS_AVAILABLE.Split(',')[i]),
                                strPointsEarned = Convert.ToDecimal(tempRecord.pPOINTS_EARNED.Split(',')[i]),
                                strQuestion = it[i].strQuestion,
                                strWeight = it[i].strWeight,
                                strItemCategoryName = _formTitle,
                                intItemCategoryID = it[i].intItemCategoryID
                            };
                            lst.Add(ls);
                        }

                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(lst, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(Questionlistviewscore?.Result.FirstOrDefault()?.ID), Convert.ToInt32(Questionlistviewscore?.Result.FirstOrDefault()?.APP_TYPE_INFO_ID), _formTitle, _CatId, QuestInstance);
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

        #region Update Case Notes To Question
        public static async Task<int> UpdateCaseNotesToQuestion(bool _IsOnline, string _pITEM_INSTANCE_TRAN_ID, string _pITEM_ID, string _pContentData, string userName, int _InstanceUserAssocId, string _DBPath, object _Body_value, object objupdatecase)
        {
            int insertedRecordid = 0;
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(_pITEM_ID), _DBPath);
            string _formTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.TYPE_NAME;
            int? _CatId = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.CategoryId;
            try
            {
                if (_IsOnline)
                {
                    UpdateCaseNotesToQuestionRequest obj = _Body_value as UpdateCaseNotesToQuestionRequest;

                    var Result = QuestAPIMethods.UpdateCaseNotesToQuestion(obj);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.UpdateCaseNotesToQuestion, ((Enum.GetNames(typeof(ActionTypes)))[7] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), insertedRecordid, insertedRecordid, _DBPath, QuestInstance, id
                        );
                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.UpdateCaseNotesToQuestion, ((Enum.GetNames(typeof(ActionTypes)))[7] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), 0, Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID), _DBPath, QuestInstance, id);


                    //Task<List<AppTypeInfoList>> tempQuestionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_UpdateCaseNotesToQuestion", "T");
                    //tempQuestionlist.Wait();
                    //Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_UpdateCaseNotesToQuestion", "T", tempQuestionlist.Result.LastOrDefault().ID);
                    //Questionlist.Wait();
                    //int? iQuestionMetadatId = Questionlist.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID;
                    Task<List<AppTypeInfoList>> temprecordid = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_UpdateCaseNotesToQuestion", Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID));
                    temprecordid.Wait();
                    int pkid = 0;
                    if (temprecordid?.Result?.Count > 0)
                        pkid = temprecordid.Result.FirstOrDefault().APP_TYPE_INFO_ID;

                    Task<List<AppTypeInfoList>> recordIscheckol = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_GetItemQuestionMetaData", Convert.ToInt32(_pITEM_INSTANCE_TRAN_ID));
                    recordIscheckol.Wait();
                    if (recordIscheckol.Result?.FirstOrDefault()?.IS_ONLINE == true)
                    {
                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(_Body_value, "H15_UpdateCaseNotesToQuestion", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(_pITEM_INSTANCE_TRAN_ID), pkid, _formTitle, _CatId, QuestInstance, true);
                    }
                    else
                        Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(_Body_value, "H15_UpdateCaseNotesToQuestion", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(insertedRecordid), pkid, _formTitle, _CatId, QuestInstance, false);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Delete Item Question Metadata Case
        public static async Task<int> DeleteItemQuestionMetadataCase(bool _IsOnline, string _pITEM_INSTANCE_TRAN_ID, string _pITEM_ID, string intItemQuestionMetadataCaseID, int _InstanceUserAssocId, string _DBPath, object _Body_value, object obj)
        {
            int insertedRecordid = 0;
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(_pITEM_ID), _DBPath);
            try
            {
                if (_IsOnline)
                {
                    DeleteItemQuestionMetadataCaseRequest _obj = obj as DeleteItemQuestionMetadataCaseRequest;
                    var Result = QuestAPIMethods.DeleteItemQuestionMetadataCase(_obj);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.UpdateCaseNotesToQuestion, ((Enum.GetNames(typeof(ActionTypes)))[6] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), insertedRecordid, insertedRecordid, _DBPath, QuestInstance, id
                        );
                }
                else
                {
                    Task<AppTypeInfoList> Getapplist = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(QuestInstance, "H15_GetItemQuestionMetadataCase", Convert.ToInt32(_pITEM_ID), _DBPath, null);
                    Getapplist.Wait();
                    var tempItemList = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>>(Getapplist.Result.ASSOC_FIELD_INFO);
                    var ItemList = tempItemList.Where(v => v.intItemQuestionMetadataCaseID == Convert.ToInt32(intItemQuestionMetadataCaseID)).ToList();
                    var ItemListResult = tempItemList.Where(v => !ItemList.Any(vv => vv.intItemQuestionMetadataCaseID == v.intItemQuestionMetadataCaseID));
                    AppTypeInfoList app = new AppTypeInfoList()
                    {
                        APP_TYPE_INFO_ID = Getapplist.Result.APP_TYPE_INFO_ID,
                        ASSOC_FIELD_INFO = JsonConvert.SerializeObject(ItemListResult),
                        INSTANCE_USER_ASSOC_ID = Getapplist.Result.INSTANCE_USER_ASSOC_ID,
                        LAST_SYNC_DATETIME = Getapplist.Result.LAST_SYNC_DATETIME,
                        SYSTEM = Getapplist.Result.SYSTEM,
                        TransactionType = Getapplist.Result.TransactionType,
                        ID = Getapplist.Result.ID,
                        TYPE_ID = Getapplist.Result.TYPE_ID,
                        CategoryId = Getapplist.Result.CategoryId,
                        CategoryName = Getapplist.Result.CategoryName,
                        TYPE_NAME = Getapplist.Result.TYPE_NAME,
                        TYPE_SCREEN_INFO = Getapplist.Result.TYPE_SCREEN_INFO,
                        IS_ONLINE = false
                    };
                    insertedRecordid = await DBHelper.SaveAppTypeInfo(app, _DBPath);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Add Case To Question
        public static async Task<int> AddCaseToQuestion(bool _IsOnline, string _pITEM_INSTANCE_TRAN_ID, string _pITEM_ID, string intItemQuestionFieldID, string _pContentData, string userName, int _InstanceUserAssocId, string _DBPath, object _Body_value, object objAddCaseToQuestion)
        {
            int insertedRecordid = 0;
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(_pITEM_ID), _DBPath);
            string _formTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.TYPE_NAME;
            int? _CatId = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.CategoryId;
            try
            {
                if (_IsOnline)
                {

                    var Result = QuestAPIMethods.AddCaseToQuestion(objAddCaseToQuestion);
                    string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                    insertedRecordid = Int32.Parse(temp);

                    int iteminfoid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.AddCaseToQuestion, ((Enum.GetNames(typeof(ActionTypes)))[6] + "," + (Enum.GetNames(typeof(ActionTypes)))[1] + "," + (Enum.GetNames(typeof(ActionTypes)))[23]), insertedRecordid, insertedRecordid, _DBPath, QuestInstance, id
                        );
                }
                else
                {
                    insertedRecordid = await CommonConstants.AddofflineTranInfo(int.Parse(_pITEM_ID), _Body_value, Constants.AddCaseToQuestion, ((Enum.GetNames(typeof(ActionTypes)))[6] + "," + (Enum.GetNames(typeof(ActionTypes)))[1] + "," + (Enum.GetNames(typeof(ActionTypes)))[23]), 0, insertedRecordid, _DBPath, QuestInstance, id);

                    Task<List<AppTypeInfoList>> tempQuestionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddCaseToQuestion", "T");
                    tempQuestionlist.Wait();
                    Task<List<AppTypeInfoList>> Questionlist = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_pITEM_ID), Convert.ToInt32(_CatId), _DBPath, "H15_AddCaseToQuestion", "T", tempQuestionlist?.Result?.LastOrDefault()?.ID);
                    Questionlist.Wait();
                    int? iQuestionMetadatId = Questionlist.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID;

                    Cases.CasesSyncAPIMethods.SaveViewJsonSqlite(_Body_value, "H15_AddCaseToQuestion", _DBPath, Convert.ToString(_pITEM_ID), Convert.ToString(insertedRecordid), Convert.ToInt32(iQuestionMetadatId), _formTitle, _CatId, QuestInstance);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Add Form From DB
        public static async Task<int> AddFormFromDB(string _DBPath)
        {
            int insertedRecordid = 0;
            try
            {
                var Records = await DBHelper.GetItemTranInfoListList(_DBPath);
                var RecordList = Records.Where(v => v.PROCESS_ID == 0 && v.METHOD == Constants.AddForm);
                foreach (var item in RecordList)
                {
                    try
                    {
                        var _Body_value = JsonConvert.DeserializeObject<AddFormRequest>(item.ITEM_TRAN_INFO);
                        var Result = QuestAPIMethods.AddForm(_Body_value);
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

        #region Get Items By AreaID FormList
        public async static Task<List<ItemsByAreaIDResponse.ItemsByAreaID>> GetItemsByAreaIDFormList(bool _IsOnline, string user, string AreaId, int _InstanceUserAssocId, string _DBPath)
        {
            List<ItemsByAreaIDResponse.ItemsByAreaID> ItemsByAreaIDFormList = new List<ItemsByAreaIDResponse.ItemsByAreaID>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            ItemsByAreaIDResponse.ItemsByAreaID ItemsByAreaIDForm = new ItemsByAreaIDResponse.ItemsByAreaID();
            var GetAreaList = CommonConstants.GetResultBySytemcodeList(QuestInstance, "H2_GetItemsByAreaIDFormList", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemsByAreaIDFormList(AreaId, user);

                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemsByAreaIDFormList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemsByAreaIDResponse.ItemsByAreaID>>(temp.ToString());
                    }
                }
                else
                {
                    if (AreaId != null)
                    {
                        Task<List<AppTypeInfoList>> Result = DBHelper.GetAppTypeInfoListByCategoryId(QuestInstance, Convert.ToInt32(AreaId), "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                        Result.Wait();

                        var lst = Result.Result?.Select(v => new { v.TYPE_ID, v.TYPE_NAME })?.Distinct();
                        int cnt = 0;
                        foreach (var item in lst)
                        {
                            var temp = JsonConvert.DeserializeObject<List<ItemInfoField>>(Result.Result[cnt].ASSOC_FIELD_INFO).FirstOrDefault();
                            if (item.TYPE_ID > 0)
                                ItemsByAreaIDFormList.Add(new ItemsByAreaIDResponse.ItemsByAreaID
                                {
                                    intItemID = Convert.ToInt32(item.TYPE_ID),
                                    strItemName = item.TYPE_NAME,
                                    securityType = temp.FIELD_SECURITY
                                });
                            cnt++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemsByAreaIDFormList;
        }
        #endregion

        #region Get Count For Quest Item
        public async static Task<GetCountForQuestItemResponse.CountForQuestItem> GetCountForQuestItem(bool _IsOnline, string _ItemId, string _User, int _InstanceUserAssocId, string _DBPath)
        {
            GetCountForQuestItemResponse.CountForQuestItem ItemList = new GetCountForQuestItemResponse.CountForQuestItem();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H4_GetCountForQuestItem", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetCountForQuestItem(_ItemId, _User);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetCountForQuestItemResponse.CountForQuestItem>>(temp.ToString())[0];
                        if (ItemList != null)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H4_GetCountForQuestItem", _InstanceUserAssocId, _DBPath, id, _ItemId, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnResult<GetCountForQuestItemResponse.CountForQuestItem>(QuestInstance, "H4_GetCountForQuestItem", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ItemInfoFields By ItemID
        public async static Task<List<ItemInfoField>> GetItemInfoFieldsByItemID(bool _IsOnline, string _intItemID, string _strShowOnPage, int _InstanceUserAssocId, string _DBPath)
        {
            List<ItemInfoField> ItemList = new List<ItemInfoField>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H6_GetItemInfoFieldsByItemID", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemInfoFieldsByItemID(_intItemID, _strShowOnPage);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemInfoField>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetItemInfoFieldsByItemID", _InstanceUserAssocId, _DBPath, id, _intItemID, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<ItemInfoField>(QuestInstance, "H6_GetItemInfoFieldsByItemID", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get Item Question Fields By Item CategoryID viewscores
        public async static Task<List<GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse.ItemQuestionField_ViewScoresModel>> GetItemQuestionFieldsByItemCategoryIDviewscores(bool _IsOnline, string _intItemID, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse.ItemQuestionField_ViewScoresModel> ItemList = new List<GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse.ItemQuestionField_ViewScoresModel>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H6_GetItemQuestionFieldsByItemCategoryIDviewscores", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemQuestionFieldsByItemCategoryIDviewscores(_intItemID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse.ItemQuestionField_ViewScoresModel>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetItemQuestionFieldsByItemCategoryIDviewscores", _InstanceUserAssocId, _DBPath, id, _intItemID, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse.ItemQuestionField_ViewScoresModel>(QuestInstance, "H6_GetItemQuestionFieldsByItemCategoryIDviewscores", _DBPath, _intItemID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ItemInstanceTran
        // List of Quest form of specific item.
        public async static Task<List<GetItemInstanceTranResponse.ItemInstanceTran>> GetItemInstanceTran(bool _IsOnline, string _itemInstanceTranID, string _itemId, string _isIncludeClosedItems, string _fromDate, string _toDate, string _itemInstanceTransIDs, int _InstanceUserAssocId, string _DBPath, string _sCatId, string username)
        {
            List<GetItemInstanceTranResponse.ItemInstanceTran> ItemList = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H5_GetItemInstanceTran", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemInstanceTran(_itemInstanceTranID, _itemId, _isIncludeClosedItems, _fromDate, _toDate, _itemInstanceTransIDs, username);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H5_GetItemInstanceTran", _InstanceUserAssocId, _DBPath, id, _itemId, "M", "", Convert.ToInt32(_sCatId), "", "", true);
                        }
                    }
                }
                else
                {
                    //view Offline records
                    //ItemList = CommonConstants.ReturnListResult<GetItemInstanceTranResponse.ItemInstanceTran>(QuestInstance, "H5_GetItemInstanceTran", _DBPath);
                    Task<List<AppTypeInfoList>> OnlineList = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode
                    (QuestInstance, Convert.ToInt32(_itemId), Convert.ToInt32(_sCatId), _DBPath, "H5_GetItemInstanceTran", "M");
                    OnlineList.Wait();
                    //if (onlineRecord?.Result?.Count > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                    //    {
                    //        ItemList = JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(onlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                    //    }
                    //}
                    Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_itemId), Convert.ToInt32(_sCatId), _DBPath, "H15_GetItemQuestionMetaData", "T");
                    OfflineList.Wait();
                    //if (offlineRecord?.Result?.Count > 0)
                    //{
                    //    if (!string.IsNullOrEmpty(offlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO))
                    //    {
                    //        GetItemInstanceTranResponse.ItemInstanceTran temp = JsonConvert.DeserializeObject<GetItemInstanceTranResponse.ItemInstanceTran>(offlineRecord.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                    //        ItemList.Add(temp);
                    //    }
                    //}
                    GetItemInstanceTranResponse.ItemInstanceTran records = new GetItemInstanceTranResponse.ItemInstanceTran();

                    GetItemInstanceTranResponse.ItemInstanceTran viewForm = new GetItemInstanceTranResponse.ItemInstanceTran();
                    List<GetItemInstanceTranResponse.ItemInstanceTran> json = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
                    if (OnlineList.Result.Count > 0)
                    {
                        json = JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(OnlineList.Result.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                    }

                    int cnt = 0;
                    bool flg = false;
                    foreach (var item in OfflineList.Result)
                    {
                        dynamic temp = null;
                        try
                        {
                            temp = JsonConvert.DeserializeObject<GetItemInstanceTranResponse.ItemInstanceTran>(item.ASSOC_FIELD_INFO);
                            temp = temp as GetItemInstanceTranResponse.ItemInstanceTran;
                        }
                        catch (Exception)
                        {
                            temp = JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(item.ASSOC_FIELD_INFO).FirstOrDefault();
                            temp = temp as List<GetItemInstanceTranResponse.ItemInstanceTran>;
                        }

                        //var temp = JsonConvert.DeserializeObject<GetItemInstanceTranResponse.ItemInstanceTran>(item.ASSOC_FIELD_INFO);
                        //var temp = JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(item.ASSOC_FIELD_INFO).FirstOrDefault();
                        temp.SECURITY_TYPE_AREA = json?.ToList()?.FirstOrDefault()?.SECURITY_TYPE_AREA == null ? "OPEN" : json?.ToList()?.FirstOrDefault()?.SECURITY_TYPE_AREA;
                        temp.SECURITY_TYPE_ITEM = json?.ToList()?.FirstOrDefault()?.SECURITY_TYPE_ITEM == null ? "OPEN" : json?.ToList()?.FirstOrDefault()?.SECURITY_TYPE_ITEM;
                        temp.SECURITY_TYPE_TRAN = json?.ToList()?.FirstOrDefault()?.SECURITY_TYPE_TRAN == null ? "OPEN" : json?.ToList()?.FirstOrDefault()?.SECURITY_TYPE_TRAN;
                        temp.strItemNAme = item.TYPE_NAME;
                        //temp.blnIsEdit = item.ed;
                        //temp.CaseCreatedDateTime = Convert.ToString(DateTime.Now);
                        //temp.CaseCreatedDisplayName = _FullName;
                        temp.intListId = OnlineList.Result.Count + (++cnt);
                        //temp.CaseTypeName = json.Count == 0 ? sTitle : json.FirstOrDefault().CaseTypeName;
                        //temp.CaseAssignedToDisplayName = json.Count == 0 ? "" : json.FirstOrDefault().CaseAssignedToDisplayName;
                        //temp.CaseAssignedToSAM = json.Count == 0 ? "" : json.FirstOrDefault().CaseAssignedToSAM;
                        //temp.intItemID = Convert.ToInt32(item.ID);
                        temp.intItemInstanceTranID = Convert.ToInt32(item.ID);
                        temp.ItemtransinfoID = Convert.ToInt32(item.ID);
                        temp.TransactionType = "T";
                        if (json.FindAll(v => v.intItemInstanceTranID == Convert.ToInt32(temp.intItemInstanceTranID))?.Count > 0)
                        {
                            var vv = json.Where(v => v.intItemInstanceTranID == temp.intItemInstanceTranID);
                            var Result = json.Where(p => !vv.Any(p2 => p2.intItemInstanceTranID == p.intItemInstanceTranID));
                            foreach (var itm in Result)
                            {
                                if (!flg)
                                    ItemList.Add(itm);
                            }
                            flg = true;

                            var res = vv.Select(v =>
                            {
                                v.blnIsEdit = temp.blnIsEdit;
                                v.FormCredtedDate = temp.FormCredtedDate;
                                v.strItemNAme = temp.strItemNAme;
                                v.intItemID = temp.intItemID;
                                v.strOtherComments = temp.strOtherComments;
                                v.strFormCretedBy = temp.strFormCretedBy;
                                v.FormCredtedDate = temp.FormCredtedDate;
                                v.intCol1ItemInfoFieldID = temp.intCol1ItemInfoFieldID;
                                v.intCol2ItemInfoFieldID = temp.intCol2ItemInfoFieldID;
                                v.intCol3ItemInfoFieldID = temp.intCol3ItemInfoFieldID;
                                v.intCol4ItemInfoFieldID = temp.intCol4ItemInfoFieldID;
                                v.intCol5ItemInfoFieldID = temp.intCol5ItemInfoFieldID;
                                v.intCol6ItemInfoFieldID = temp.intCol6ItemInfoFieldID;
                                v.intCol7ItemInfoFieldID = temp.intCol7ItemInfoFieldID;
                                v.intCol8ItemInfoFieldID = temp.intCol8ItemInfoFieldID;
                                v.intCol9ItemInfoFieldID = temp.intCol9ItemInfoFieldID;
                                v.intCol10ItemInfoFieldID = temp.intCol10ItemInfoFieldID;
                                v.intCol11ItemInfoFieldID = temp.intCol11ItemInfoFieldID;
                                v.intCol12ItemInfoFieldID = temp.intCol12ItemInfoFieldID;
                                v.intCol13ItemInfoFieldID = temp.intCol13ItemInfoFieldID;
                                v.intCol14ItemInfoFieldID = temp.intCol14ItemInfoFieldID;
                                v.intCol15ItemInfoFieldID = temp.intCol15ItemInfoFieldID;
                                v.intCol16ItemInfoFieldID = temp.intCol16ItemInfoFieldID;
                                v.intCol17ItemInfoFieldID = temp.intCol17ItemInfoFieldID;
                                v.intCol18ItemInfoFieldID = temp.intCol18ItemInfoFieldID;
                                v.intCol19ItemInfoFieldID = temp.intCol19ItemInfoFieldID;
                                v.intCol20ItemInfoFieldID = temp.intCol20ItemInfoFieldID;

                                v.strCol1ItemInfoFieldName = temp.strCol1ItemInfoFieldName;
                                v.strCol2ItemInfoFieldName = temp.strCol2ItemInfoFieldName;
                                v.strCol3ItemInfoFieldName = temp.strCol3ItemInfoFieldName;
                                v.strCol4ItemInfoFieldName = temp.strCol4ItemInfoFieldName;
                                v.strCol5ItemInfoFieldName = temp.strCol5ItemInfoFieldName;
                                v.strCol6ItemInfoFieldName = temp.strCol6ItemInfoFieldName;
                                v.strCol7ItemInfoFieldName = temp.strCol7ItemInfoFieldName;
                                v.strCol8ItemInfoFieldName = temp.strCol8ItemInfoFieldName;
                                v.strCol9ItemInfoFieldName = temp.strCol9ItemInfoFieldName;
                                v.strCol10ItemInfoFieldName = temp.strCol10ItemInfoFieldName;
                                v.strCol11ItemInfoFieldName = temp.strCol11ItemInfoFieldName;
                                v.strCol12ItemInfoFieldName = temp.strCol12ItemInfoFieldName;
                                v.strCol13ItemInfoFieldName = temp.strCol13ItemInfoFieldName;
                                v.strCol14ItemInfoFieldName = temp.strCol14ItemInfoFieldName;
                                v.strCol15ItemInfoFieldName = temp.strCol15ItemInfoFieldName;
                                v.strCol16ItemInfoFieldName = temp.strCol16ItemInfoFieldName;
                                v.strCol17ItemInfoFieldName = temp.strCol17ItemInfoFieldName;
                                v.strCol18ItemInfoFieldName = temp.strCol18ItemInfoFieldName;
                                v.strCol19ItemInfoFieldName = temp.strCol19ItemInfoFieldName;
                                v.strCol20ItemInfoFieldName = temp.strCol20ItemInfoFieldName;

                                v.strCol1ItemInfoFieldValue = temp.strCol1ItemInfoFieldValue;
                                v.strCol2ItemInfoFieldValue = temp.strCol2ItemInfoFieldValue;
                                v.strCol3ItemInfoFieldValue = temp.strCol3ItemInfoFieldValue;
                                v.strCol4ItemInfoFieldValue = temp.strCol4ItemInfoFieldValue;
                                v.strCol5ItemInfoFieldValue = temp.strCol5ItemInfoFieldValue;
                                v.strCol6ItemInfoFieldValue = temp.strCol6ItemInfoFieldValue;
                                v.strCol7ItemInfoFieldValue = temp.strCol7ItemInfoFieldValue;
                                v.strCol8ItemInfoFieldValue = temp.strCol8ItemInfoFieldValue;
                                v.strCol9ItemInfoFieldValue = temp.strCol9ItemInfoFieldValue;
                                v.strCol10ItemInfoFieldValue = temp.strCol10ItemInfoFieldValue;
                                v.strCol11ItemInfoFieldValue = temp.strCol11ItemInfoFieldValue;
                                v.strCol12ItemInfoFieldValue = temp.strCol12ItemInfoFieldValue;
                                v.strCol13ItemInfoFieldValue = temp.strCol13ItemInfoFieldValue;
                                v.strCol14ItemInfoFieldValue = temp.strCol14ItemInfoFieldValue;
                                v.strCol15ItemInfoFieldValue = temp.strCol15ItemInfoFieldValue;
                                v.strCol16ItemInfoFieldValue = temp.strCol16ItemInfoFieldValue;
                                v.strCol17ItemInfoFieldValue = temp.strCol17ItemInfoFieldValue;
                                v.strCol18ItemInfoFieldValue = temp.strCol18ItemInfoFieldValue;
                                v.strCol19ItemInfoFieldValue = temp.strCol19ItemInfoFieldValue;
                                v.strCol20ItemInfoFieldValue = temp.strCol20ItemInfoFieldValue;
                                v.strIsLocked = temp.strIsLocked;
                                v.dcOverallScore = temp.dcOverallScore;
                                return v;
                            });
                            if (ItemList.Where(v => v.intItemInstanceTranID == res.FirstOrDefault().intInforTranId).Count() == 0)
                            {
                                ItemList.AddRange(res);
                                ItemList = ItemList.OrderByDescending(v => v.intItemInstanceTranID).ToList();
                            }
                            else
                            {
                                ItemList.Where(v => v.intItemInstanceTranID == res.FirstOrDefault().intInforTranId).Select(av =>
                                {
                                    av.blnIsEdit = res.FirstOrDefault().blnIsEdit;
                                    av.FormCredtedDate = res.FirstOrDefault().FormCredtedDate;
                                    av.strItemNAme = res.FirstOrDefault().strItemNAme;
                                    av.intItemID = res.FirstOrDefault().intItemID;
                                    av.strOtherComments = res.FirstOrDefault().strOtherComments;
                                    av.strFormCretedBy = res.FirstOrDefault().strFormCretedBy;
                                    av.FormCredtedDate = res.FirstOrDefault().FormCredtedDate;
                                    av.intCol1ItemInfoFieldID = res.FirstOrDefault().intCol1ItemInfoFieldID;
                                    av.intCol2ItemInfoFieldID = res.FirstOrDefault().intCol2ItemInfoFieldID;
                                    av.intCol3ItemInfoFieldID = res.FirstOrDefault().intCol3ItemInfoFieldID;
                                    av.intCol4ItemInfoFieldID = res.FirstOrDefault().intCol4ItemInfoFieldID;
                                    av.intCol5ItemInfoFieldID = res.FirstOrDefault().intCol5ItemInfoFieldID;
                                    av.intCol6ItemInfoFieldID = res.FirstOrDefault().intCol6ItemInfoFieldID;
                                    av.intCol7ItemInfoFieldID = res.FirstOrDefault().intCol7ItemInfoFieldID;
                                    av.intCol8ItemInfoFieldID = res.FirstOrDefault().intCol8ItemInfoFieldID;
                                    av.intCol9ItemInfoFieldID = res.FirstOrDefault().intCol9ItemInfoFieldID;
                                    av.intCol10ItemInfoFieldID = res.FirstOrDefault().intCol10ItemInfoFieldID;
                                    av.intCol11ItemInfoFieldID = res.FirstOrDefault().intCol11ItemInfoFieldID;
                                    av.intCol12ItemInfoFieldID = res.FirstOrDefault().intCol12ItemInfoFieldID;
                                    av.intCol13ItemInfoFieldID = res.FirstOrDefault().intCol13ItemInfoFieldID;
                                    av.intCol14ItemInfoFieldID = res.FirstOrDefault().intCol14ItemInfoFieldID;
                                    av.intCol15ItemInfoFieldID = res.FirstOrDefault().intCol15ItemInfoFieldID;
                                    av.intCol16ItemInfoFieldID = res.FirstOrDefault().intCol16ItemInfoFieldID;
                                    av.intCol17ItemInfoFieldID = res.FirstOrDefault().intCol17ItemInfoFieldID;
                                    av.intCol18ItemInfoFieldID = res.FirstOrDefault().intCol18ItemInfoFieldID;
                                    av.intCol19ItemInfoFieldID = res.FirstOrDefault().intCol19ItemInfoFieldID;
                                    av.intCol20ItemInfoFieldID = res.FirstOrDefault().intCol20ItemInfoFieldID;

                                    av.strCol1ItemInfoFieldName = res.FirstOrDefault().strCol1ItemInfoFieldName;
                                    av.strCol2ItemInfoFieldName = res.FirstOrDefault().strCol2ItemInfoFieldName;
                                    av.strCol3ItemInfoFieldName = res.FirstOrDefault().strCol3ItemInfoFieldName;
                                    av.strCol4ItemInfoFieldName = res.FirstOrDefault().strCol4ItemInfoFieldName;
                                    av.strCol5ItemInfoFieldName = res.FirstOrDefault().strCol5ItemInfoFieldName;
                                    av.strCol6ItemInfoFieldName = res.FirstOrDefault().strCol6ItemInfoFieldName;
                                    av.strCol7ItemInfoFieldName = res.FirstOrDefault().strCol7ItemInfoFieldName;
                                    av.strCol8ItemInfoFieldName = res.FirstOrDefault().strCol8ItemInfoFieldName;
                                    av.strCol9ItemInfoFieldName = res.FirstOrDefault().strCol9ItemInfoFieldName;
                                    av.strCol10ItemInfoFieldName = res.FirstOrDefault().strCol10ItemInfoFieldName;
                                    av.strCol11ItemInfoFieldName = res.FirstOrDefault().strCol11ItemInfoFieldName;
                                    av.strCol12ItemInfoFieldName = res.FirstOrDefault().strCol12ItemInfoFieldName;
                                    av.strCol13ItemInfoFieldName = res.FirstOrDefault().strCol13ItemInfoFieldName;
                                    av.strCol14ItemInfoFieldName = res.FirstOrDefault().strCol14ItemInfoFieldName;
                                    av.strCol15ItemInfoFieldName = res.FirstOrDefault().strCol15ItemInfoFieldName;
                                    av.strCol16ItemInfoFieldName = res.FirstOrDefault().strCol16ItemInfoFieldName;
                                    av.strCol17ItemInfoFieldName = res.FirstOrDefault().strCol17ItemInfoFieldName;
                                    av.strCol18ItemInfoFieldName = res.FirstOrDefault().strCol18ItemInfoFieldName;
                                    av.strCol19ItemInfoFieldName = res.FirstOrDefault().strCol19ItemInfoFieldName;
                                    av.strCol20ItemInfoFieldName = res.FirstOrDefault().strCol20ItemInfoFieldName;

                                    av.strCol1ItemInfoFieldValue = res.FirstOrDefault().strCol1ItemInfoFieldValue;
                                    av.strCol2ItemInfoFieldValue = res.FirstOrDefault().strCol2ItemInfoFieldValue;
                                    av.strCol3ItemInfoFieldValue = res.FirstOrDefault().strCol3ItemInfoFieldValue;
                                    av.strCol4ItemInfoFieldValue = res.FirstOrDefault().strCol4ItemInfoFieldValue;
                                    av.strCol5ItemInfoFieldValue = res.FirstOrDefault().strCol5ItemInfoFieldValue;
                                    av.strCol6ItemInfoFieldValue = res.FirstOrDefault().strCol6ItemInfoFieldValue;
                                    av.strCol7ItemInfoFieldValue = res.FirstOrDefault().strCol7ItemInfoFieldValue;
                                    av.strCol8ItemInfoFieldValue = res.FirstOrDefault().strCol8ItemInfoFieldValue;
                                    av.strCol9ItemInfoFieldValue = res.FirstOrDefault().strCol9ItemInfoFieldValue;
                                    av.strCol10ItemInfoFieldValue = res.FirstOrDefault().strCol10ItemInfoFieldValue;
                                    av.strCol11ItemInfoFieldValue = res.FirstOrDefault().strCol11ItemInfoFieldValue;
                                    av.strCol12ItemInfoFieldValue = res.FirstOrDefault().strCol12ItemInfoFieldValue;
                                    av.strCol13ItemInfoFieldValue = res.FirstOrDefault().strCol13ItemInfoFieldValue;
                                    av.strCol14ItemInfoFieldValue = res.FirstOrDefault().strCol14ItemInfoFieldValue;
                                    av.strCol15ItemInfoFieldValue = res.FirstOrDefault().strCol15ItemInfoFieldValue;
                                    av.strCol16ItemInfoFieldValue = res.FirstOrDefault().strCol16ItemInfoFieldValue;
                                    av.strCol17ItemInfoFieldValue = res.FirstOrDefault().strCol17ItemInfoFieldValue;
                                    av.strCol18ItemInfoFieldValue = res.FirstOrDefault().strCol18ItemInfoFieldValue;
                                    av.strCol19ItemInfoFieldValue = res.FirstOrDefault().strCol19ItemInfoFieldValue;
                                    av.strCol20ItemInfoFieldValue = res.FirstOrDefault().strCol20ItemInfoFieldValue;
                                    av.strIsLocked = res.FirstOrDefault().strIsLocked;
                                    av.dcOverallScore = res.FirstOrDefault().dcOverallScore;
                                    return av;
                                }).OrderByDescending(v => v.intItemInstanceTranID).ToList();
                                //ItemList.AddRange(lst);
                                ItemList = ItemList.OrderByDescending(v => v.intItemInstanceTranID).ToList();
                            }
                        }
                        else
                            ItemList.Add(temp);
                    }
                    if (!flg)
                        ItemList.AddRange(json);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;//.OrderByDescending(v => v.intItemInstanceTranID).ToList();
        }
        #endregion

        #region GetQuestItemByStatus
        public async static Task<List<GetItemInstanceTranResponse.ItemInstanceTran>> GetQuestItemByStatus(bool _IsOnline, string _ItemID, string _User, string _StatusType, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetItemInstanceTranResponse.ItemInstanceTran> ItemList = new List<GetItemInstanceTranResponse.ItemInstanceTran>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H5_GetQuestItemByStatus", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetQuestItemByStatus(_ItemID, _User, _StatusType);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H5_GetQuestItemByStatus", _InstanceUserAssocId, _DBPath, id, _ItemID, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<GetItemInstanceTranResponse.ItemInstanceTran>(QuestInstance, "H5_GetQuestItemByStatus", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ItemInfoField MetaData
        // Fill Control of Quest Form
        public async static Task<List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>> GetItemInfoFieldMetaData(bool _IsOnline, string _itemInstanceTranID, string _itemInfoFieldIDs, string _showOnPage, int _InstanceUserAssocId, string _DBPath, string itemid, string username, int? _sCatId)
        {
            List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData> ItemList = new List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            try
            {
                int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(itemid), _DBPath);
                int idd = CommonConstants.GetResultBySytemcodetypeidId(QuestInstance, "H15_GetItemQuestionMetaData", Convert.ToInt32(itemid), Convert.ToInt32(_sCatId), Convert.ToInt32(_itemInstanceTranID), _DBPath);
                //int? _sCatId = 0;
                int? _Typeid = 0;
                string _sTypeName = string.Empty;
                if (id == 0)
                {
                    Task<AppTypeInfoList> record = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Convert.ToInt32(itemid), QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    record.Wait();
                    //_sCatId = record.Result?.CategoryId;
                    _Typeid = Convert.ToInt32(itemid);
                    _sTypeName = record.Result?.TYPE_NAME;

                }
                else
                {
                    Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                    db.Wait();
                    if (db.Result != null)
                    {
                        //_sCatId = db.Result?.CategoryId;
                        _Typeid = db.Result?.TYPE_ID;
                        _sTypeName = db.Result?.TYPE_NAME;
                    }
                }
                try
                {
                    if (_IsOnline)
                    {
                        var result = QuestAPIMethods.GetItemInfoFieldMetaData(_itemInstanceTranID, _itemInfoFieldIDs, _showOnPage, username);
                        var temp = result.GetValue("ResponseContent");
                        if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                        {
                            var t = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemInstanceTranResponse.ItemInstanceTran>>(temp.ToString());

                            ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>>(temp.ToString());
                            if (ItemList.Count > 0)
                            {
                                var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H15_GetItemQuestionMetaData", _InstanceUserAssocId, _DBPath, idd, itemid, "M", _itemInstanceTranID, _sCatId, _sTypeName, "", true);
                            }
                        }
                    }
                    else
                    {
                        Task<List<AppTypeInfoList>> OnlineList = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID
            (QuestInstance, Convert.ToInt32(_Typeid), Convert.ToInt32(_sCatId), _DBPath, "H15_GetItemQuestionMetaData", "M", Convert.ToInt32(_itemInstanceTranID));
                        OnlineList.Wait();
                        if (OnlineList?.Result?.Count > 0)
                            ItemList = JsonConvert.DeserializeObject<List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>>(OnlineList?.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO.ToString());

                        Task<List<AppTypeInfoList>> OfflineList = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(itemid), Convert.ToInt32(_sCatId), _DBPath, "H15_GetItemQuestionMetaData", "T", Convert.ToInt32(_itemInstanceTranID));
                        OfflineList.Wait();

                        if (OfflineList?.Result?.Count > 0)
                        {
                            foreach (var item in OfflineList?.Result)
                            {
                                var temp = JsonConvert.DeserializeObject<GetItemInstanceTranResponse.ItemInstanceTran>(item.ASSOC_FIELD_INFO.ToString());
                                List<Tuple<int, string, string>> lst = new List<Tuple<int, string, string>>();

                                for (int i = 1; i < 21; i++)
                                {
                                    lst.Add(new Tuple<int, string, string>(Convert.ToInt32(temp["intCol" + i + "ItemInfoFieldID"]), Convert.ToString(temp["strCol" + i + "ItemInfoFieldValue"]), Convert.ToString(temp["strCol" + i + "ItemInfoFieldName"])));

                                    //GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData md = new GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData();
                                    //md.intItemInfoFieldID = Convert.ToInt32(temp.intCol1ItemInfoFieldID);
                                    //md.strItemInfoFieldName = Convert.ToString(temp.strCol1ItemInfoFieldName);
                                    //md.strItemInfoFieldValue = Convert.ToString(temp.strCol1ItemInfoFieldValue);
                                }

                                //ItemList.Clear();
                                for (int i = 0; i < 20; i++)
                                {
                                    GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData md = new GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData();
                                    if (lst[i].Item1 == 0)
                                        continue;
                                    md.strIsActive = temp.blnIsEdit == true ? "Y" : "N";
                                    md.strOtherComments = temp.strOtherComments;
                                    md.intItemInstanceTranID = temp.intItemInstanceTranID;
                                    md.intItemID = temp.intItemID;
                                    md.intListID = temp.intListId;
                                    md.strItemName = temp.strItemName;
                                    md.intItemInstanceTranID = temp.intItemInstanceTranID;
                                    md.FIELD_SECURITY = temp.SECURITY_TYPE_TRAN;
                                    md.intItemInfoFieldID = lst[i].Item1;
                                    md.strItemInfoFieldName = lst[i].Item3;
                                    md.strItemInfoFieldValue = lst[i].Item2;
                                    ItemList.Add(md);
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception)
            {
            }
            return ItemList;
        }
        #endregion
        #region Get ItemInfoDependency
        //Check Parent Value Of External DataSource
        public async static Task<List<GetItemInfoDependencyResponse.ItemInfoDependancy>> GetItemInfoDependency(bool _IsOnline, string _itemInfoFieldIDChild, string _itemid, int _InstanceUserAssocId, string _DBPath, string _Iteminstancetranid = "")
        {
            List<GetItemInfoDependencyResponse.ItemInfoDependancy> ItemList = new List<GetItemInfoDependencyResponse.ItemInfoDependancy>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H15_GetItemInfoDependency", _DBPath);
            try
            {
                int? _sCatId = 0;
                int? _Typeid = 0;
                string _sTypeName = string.Empty;
                if (id == 0)
                {
                    Task<AppTypeInfoList> record = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Convert.ToInt32(_itemid), QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    record.Wait();
                    _sCatId = record.Result?.CategoryId;
                    _Typeid = record.Result?.TYPE_ID;
                    _sTypeName = record.Result?.TYPE_NAME;

                }
                else
                {
                    Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                    db.Wait();
                    if (db.Result != null)
                    {
                        _sCatId = db.Result?.CategoryId;
                        _Typeid = db.Result?.TYPE_ID;
                        _sTypeName = db.Result?.TYPE_NAME;
                    }
                }
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemInfoDependency(_itemInfoFieldIDChild, _itemid);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemInfoDependencyResponse.ItemInfoDependancy>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H15_GetItemInfoDependency", _InstanceUserAssocId, _DBPath, id, _itemid, "M", _Iteminstancetranid, _sCatId, _sTypeName);
                        }
                    }
                }
                else
                {

                    ItemList = CommonConstants.ReturnListResult<GetItemInfoDependencyResponse.ItemInfoDependancy>(QuestInstance, "H15_GetItemInfoDependency", _DBPath);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ExternalDatasource By Query
        public async static Task<List<GetExternalDatasourceByIDResponse.ExternalDataSource>> GetExternalDatasourceByQuery(bool _IsOnline, string _itemInfoFieldIDChild, string _itemid, int _InstanceUserAssocId, string _DBPath)
        {
            List<GetExternalDatasourceByIDResponse.ExternalDataSource> ItemList = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H6_GetExternalDatasourceByQuery", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetExternalDatasourceByQuery(_itemInfoFieldIDChild, _itemid);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetExternalDatasourceByIDResponse.ExternalDataSource>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetExternalDatasourceByQuery", _InstanceUserAssocId, _DBPath, id, _itemid, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<GetExternalDatasourceByIDResponse.ExternalDataSource>(QuestInstance, "H6_GetExternalDatasourceByQuery", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ExternalDatasource By ID
        public async static Task<List<GetExternalDatasourceByIDResponse.ExternalDataSource>> GetExternalDatasourceByID(bool _IsOnline, string _ExternalDatasourceID, int _InstanceUserAssocId, string _DBPath, string itemid)
        {
            List<GetExternalDatasourceByIDResponse.ExternalDataSource> ItemList = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", Convert.ToInt32(itemid), _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetExternalDatasourceByID(_ExternalDatasourceID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetExternalDatasourceByIDResponse.ExternalDataSource>>(temp.ToString());
                        //if (ItemList.Count > 0)
                        //{
                        //    var inserted = CommonConstants.AddRecordOfflineStore(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetExternalDatasourceByID", _InstanceUserAssocId, _DBPath, id, itemid, "M");
                        //}
                    }
                }
                else
                {

                    Task<EDSResultList> Result = DBHelper.GetEDSResultListwithId(Convert.ToInt32(_ExternalDatasourceID), id, _DBPath);
                    Result.Wait();
                    if (Result?.Result?.ASSOC_FIELD_ID > 0)
                    {
                        string jsonvalue = Result.Result.EDS_RESULT;
                        ItemList = JsonConvert.DeserializeObject<List<GetExternalDatasourceByIDResponse.ExternalDataSource>>(jsonvalue);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ExternalDatasourceInfo By ID
        public async static Task<List<ExternalDatasourceInfo>> GetExternalDatasourceInfoByID(bool _IsOnline, string _ExternalDatasourceID, int _InstanceUserAssocId, string _DBPath, string itemid)
        {
            List<ExternalDatasourceInfo> ItemList = new List<ExternalDatasourceInfo>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H6_GetExternalDatasourceByID", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetExternalDatasourceInfoByID(_ExternalDatasourceID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ExternalDatasourceInfo>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetExternalDatasourceByID", _InstanceUserAssocId, _DBPath, id, itemid, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<ExternalDatasourceInfo>(QuestInstance, "H6_GetExternalDatasourceByID", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get Item Question MetaData
        // Questionary View Data For Specific TransId(Get First Note)
        public async static Task<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>> GetItemQuestionMetaData(bool _IsOnline, string _ItemInstanceTranID, int _InstanceUserAssocId, string _DBPath, string itemId, string view = "", string _scatid = "")
        {
            List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> ItemList = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            Task<List<AppTypeInfoList>> temprecordid = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H15_GetItemQuestionMetaDataviewscore", Convert.ToInt32(_ItemInstanceTranID));
            temprecordid.Wait();
            int id = 0;
            if (temprecordid?.Result?.Count > 0)
                id = temprecordid.Result.FirstOrDefault().APP_TYPE_INFO_ID;
            try
            {
                //int? _sCatId = 0;
                int? _Typeid = 0;
                string _sTypeName = string.Empty;
                if (id == 0)
                {
                    Task<AppTypeInfoList> record = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Convert.ToInt32(itemId), QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    record.Wait();
                    //_sCatId = record.Result?.CategoryId;
                    _Typeid = Convert.ToInt32(itemId);
                    _sTypeName = record.Result?.TYPE_NAME;

                }
                else
                {
                    Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                    db.Wait();
                    if (db.Result != null)
                    {
                        // _sCatId = db.Result?.CategoryId;
                        _Typeid = db.Result?.TYPE_ID;
                        _sTypeName = db.Result?.TYPE_NAME;
                    }
                }
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemQuestionMetadata(_ItemInstanceTranID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H15_GetItemQuestionMetaDataviewscore", _InstanceUserAssocId, _DBPath, id, Convert.ToString(_Typeid), "M", _ItemInstanceTranID, Convert.ToInt32(_scatid), _sTypeName, "", true);
                        }
                    }
                }
                else
                {
                    if (view == "")
                    {
                        //ItemList = CommonConstants.ReturnListResult<GetItemQuestionMetadataResponse.ItemQuestionMetaData>(QuestInstance, "H15_GetItemQuestionMetaDataviewscore", _DBPath, Convert.ToString(_Typeid));

                        Task<List<AppTypeInfoList>> Questionlistviewscoreonline = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "M", Convert.ToInt32(_ItemInstanceTranID));
                        Questionlistviewscoreonline.Wait();
                        if (Questionlistviewscoreonline?.Result?.Count > 0)
                        {
                            ItemList = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Questionlistviewscoreonline?.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                        }

                        Task<List<AppTypeInfoList>> Questionlistviewscore = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", Convert.ToInt32(_ItemInstanceTranID));
                        Questionlistviewscore.Wait();
                        if (Questionlistviewscore?.Result?.Count > 0)
                        {
                            var ItemListoffline = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Questionlistviewscore?.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                            ItemList.AddRange(ItemListoffline);
                            foreach (var item in ItemList)
                            {

                                ItemList.Where(v => v.strItemCategoryName == item.strItemCategoryName).Select(s => { s.strItemCategoryName = _sTypeName; return s; }).ToList();
                            }
                        }
                    }
                    else
                    {
                        var Appinfoisonline = DBHelper.GetAppTypeInfoListByIsonline(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_ItemInstanceTranID), Convert.ToInt32(_scatid), id, _DBPath);
                        Appinfoisonline.Wait();
                        if (Appinfoisonline?.Result?.IS_ONLINE == true)
                        {
                            ItemList = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Appinfoisonline?.Result.ASSOC_FIELD_INFO);
                        }
                        else
                        {
                            Add_Questions_MetadataRequest addQuestion = new Add_Questions_MetadataRequest();
                            var Appinfo = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H1_H2_H3_QUEST_AREA_FORM", "M");
                            Appinfo.Wait();

                            Task<List<ItemTranInfoList>> Questionlistviewscore = DBHelper.GetItemTranInfoListByCatIdTransTypeSyscodeID(Convert.ToInt32(Appinfo?.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID), _DBPath, Convert.ToInt32(_ItemInstanceTranID));
                            Questionlistviewscore.Wait();
                            if (Questionlistviewscore?.Result?.Count > 0)
                            {
                                //int vid = Convert.ToInt32(Questionlistviewscore?.Result.Where(v => v.METHOD.ToLower() == "/api/v1/quest/addquestionsmetadata").LastOrDefault().ITEM_TRAN_INFO_ID);
                                var Questionlistviewscores = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H15_AddQuestionsMetadata", "T", Convert.ToInt32(_ItemInstanceTranID));
                                Questionlistviewscores.Wait();
                                if (Questionlistviewscores?.Result?.Count > 0)
                                {
                                    var temp = JsonConvert.DeserializeObject<Add_Questions_MetadataRequest>(Questionlistviewscores?.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                    addQuestion = temp as Add_Questions_MetadataRequest;
                                }
                                //ItemList = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Questionlistviewscores?.Result?.FirstOrDefault().ASSOC_FIELD_INFO);
                            }
                            //else
                            {

                                var EdsInfo = DBHelper.GetEDSResultListwithId(Convert.ToInt32(itemId), Appinfo.Result[0].APP_TYPE_INFO_ID, _DBPath);
                                EdsInfo.Wait();
                                string str = EdsInfo.Result.EDS_RESULT.Split(new string[] { "|||||" }, StringSplitOptions.None)[1];
                                string sMainsplit = str.Split(new string[] { "*****" }, StringSplitOptions.None)[0];
                                List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField> ItemListsplit = JsonConvert.DeserializeObject<List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>>(sMainsplit);
                                int cnt = 0;
                                foreach (var itm in ItemListsplit)
                                {
                                    string[] arr = addQuestion?.pITEM_QUESTION_FIELD_IDs?.Split(',');
                                    GetItemQuestionMetadataResponse.ItemQuestionMetaData Itemlst = new GetItemQuestionMetadataResponse.ItemQuestionMetaData()
                                    {
                                        intItemQuestionFieldID = itm.intItemQuestionFieldID,
                                        intItemQuestionMetadataID = itm.intItemQuestionMetaDataID,
                                        intItemCategoryID = itm.intItemCategoryID,
                                        strItemName = itm.strItemName,
                                        strItemCategoryName = itm.strItemQuestionFieldName,
                                        strQuestion = itm.strItemQuestionFieldName,
                                        intAreaID = itm.intAreaID,
                                        intItemID = itm.intItemID,
                                        intItemInstanceTranID = Convert.ToInt32(_ItemInstanceTranID),
                                        strAreaName = itm.strAreaName,
                                        strMeetsStandards = Convert.ToString(addQuestion?.pMEETS_STANDARDS?.Split(',')[Array.IndexOf(arr, Convert.ToString(itm.intItemQuestionFieldID))]),
                                        strNotes = Convert.ToString(addQuestion?.pNOTES?.Split(',')[Array.IndexOf(arr, Convert.ToString(itm.intItemQuestionFieldID))]),
                                        strIsCaseRequested = addQuestion?.pIS_CASE_REQUESTED?.Split(',')[cnt],
                                        strPointsEarned = Convert.ToDecimal(addQuestion?.pPOINTS_EARNED?.Split(',')[cnt]),
                                        strPointsAvailable = Convert.ToDecimal(addQuestion?.pPOINTS_AVAILABLE?.Split(',')[cnt]),

                                    };
                                    ItemList.Add(Itemlst);
                                    cnt++;
                                }

                                Task<List<AppTypeInfoList>> Questionlistviewscoreonline = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "M", Convert.ToInt32(_ItemInstanceTranID));
                                Questionlistviewscoreonline.Wait();
                                if (Questionlistviewscoreonline?.Result?.Count > 0)
                                {
                                    var ItemListonline = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Questionlistviewscoreonline?.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                    List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> Itemstonline = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();
                                    foreach (var itm in ItemListonline)
                                    {
                                        foreach (var it in ItemList)
                                        {
                                            if (itm.intItemQuestionFieldID == it.intItemQuestionFieldID)
                                            {

                                                var temp = ItemList.Where(v => v.intItemQuestionFieldID == it.intItemQuestionFieldID).Select(a => new GetItemQuestionMetadataResponse.ItemQuestionMetaData
                                                {
                                                    strMeetsStandards = itm.strMeetsStandards,
                                                    strNotes = itm.strNotes,
                                                    intItemQuestionFieldID = itm.intItemQuestionFieldID,
                                                    intItemQuestionMetadataID = itm.intItemQuestionMetadataID,
                                                    intItemCategoryID = itm.intItemCategoryID,
                                                    strItemName = itm.strItemName,
                                                    strItemCategoryName = itm.strItemCategoryName,
                                                    strQuestion = itm.strQuestion,
                                                    intAreaID = itm.intAreaID,
                                                    intItemID = itm.intItemID,
                                                    intItemInstanceTranID = Convert.ToInt32(_ItemInstanceTranID),
                                                    strAreaName = itm.strAreaName,

                                                });
                                                Itemstonline.AddRange(temp);
                                            }
                                        }
                                    }
                                    if (Itemstonline.Count > 0)
                                        ItemList = Itemstonline;
                                    else
                                    {
                                        Task<List<AppTypeInfoList>> QuestionlistviewscoreonlineT = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(itemId), Convert.ToInt32(_scatid), _DBPath, "H15_GetItemQuestionMetaDataviewscore", "T", Convert.ToInt32(_ItemInstanceTranID));
                                        QuestionlistviewscoreonlineT.Wait();
                                        if (QuestionlistviewscoreonlineT?.Result?.Count > 0)
                                        {
                                            var ItemListonlineT = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>>(Questionlistviewscoreonline?.Result?.FirstOrDefault()?.ASSOC_FIELD_INFO);
                                            ItemList = ItemListonlineT;
                                        }
                                    }
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
            return ItemList;
        }
        #endregion

        #region Get Item Question Metadata Case
        //For extra notes
        public async static Task<List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>> GetItemQuestionMetadataCase(bool _IsOnline, string _intItemQuestionMetadataID, int _InstanceUserAssocId, string _DBPath, string _intItemID, string intItemInstanceTranID, string _intCatId, int _intItemQuestionFieldID, string _sCatid)
        {
            List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase> ItemList = new List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H15_GetItemQuestionMetadataCase", Convert.ToInt32(_intItemQuestionMetadataID), _DBPath);
            try
            {
                string _sTypeName = string.Empty;
                if (id == 0)
                {
                    Task<AppTypeInfoList> record = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Convert.ToInt32(_intItemID), QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    record.Wait();
                    _sTypeName = record.Result?.TYPE_NAME;

                }
                else
                {
                    Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                    db.Wait();
                    if (db.Result != null)
                    {
                        _sTypeName = db.Result?.TYPE_NAME;
                    }
                }
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemQuestionMetadataCase(_intItemQuestionMetadataID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            //var inserted = CommonConstants.AddRecordOfflineStore(JsonConvert.SerializeObject(ItemList), QuestInstance, "H15_GetItemQuestionMetadataCase", _InstanceUserAssocId, _DBPath, id, itemId, "M");

                            Task<List<AppTypeInfoList>> lst = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_intCatId), _DBPath, "H15_GetItemQuestionMetadataCase", "M", Convert.ToInt32(intItemInstanceTranID));
                            lst.Wait();
                            //Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                            //db.Wait();
                            string json = JsonConvert.SerializeObject(ItemList);
                            if (lst.Result != null)
                            {
                                if (lst.Result.Where(v => v.ASSOC_FIELD_INFO?.ToUpper()?.Trim() == json?.ToUpper()?.Trim())?.Count() > 0)
                                {
                                    id = lst.Result.Where(v => v.ASSOC_FIELD_INFO?.ToUpper()?.Trim() == json?.ToUpper()?.Trim()).FirstOrDefault().APP_TYPE_INFO_ID;
                                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(json, QuestInstance, "H15_GetItemQuestionMetadataCase", _InstanceUserAssocId, _DBPath, id, _intItemID, "M", intItemInstanceTranID, Convert.ToInt32(_intCatId), _sTypeName, "", true);
                                }
                                else
                                {
                                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(json, QuestInstance, "H15_GetItemQuestionMetadataCase", _InstanceUserAssocId, _DBPath, 0, _intItemID, "M", intItemInstanceTranID, Convert.ToInt32(_intCatId), _sTypeName, "", true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //ItemList = CommonConstants.ReturnListResult<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>(QuestInstance, "H15_GetItemQuestionMetadataCase", _DBPath);


                    Task<List<AppTypeInfoList>> lst = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_sCatid), _DBPath, "H15_UpdateCaseNotesToQuestion", "M", Convert.ToInt32(intItemInstanceTranID));
                    lst.Wait();

                    Task<List<AppTypeInfoList>> lstsOff = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_sCatid), _DBPath, "H15_UpdateCaseNotesToQuestion", "T");
                    lstsOff.Wait();

                    foreach (var ele in lst.Result)
                    {
                        var elem = JsonConvert.DeserializeObject<List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>>(ele.ASSOC_FIELD_INFO);
                        ItemList.AddRange(elem);
                    }


                    foreach (var ele in lstsOff.Result)
                    {
                        var elem = JsonConvert.DeserializeObject<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>(ele.ASSOC_FIELD_INFO);
                        if (_intItemQuestionFieldID == elem.intItemQuestionMetadataID)
                            ItemList.Add(elem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region GetFilterQuery_Quest
        public async static Task<List<ConnectionStringCls>> GetFilterQuery_Quest(bool _IsOnline, string _ExternalDatasourceIDChild, int _InstanceUserAssocId, string _DBPath, string itemid)
        {
            List<ConnectionStringCls> ItemList = new List<ConnectionStringCls>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H6_GetFilterQuery_Quest", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetFilterQuery_Quest(_ExternalDatasourceIDChild);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ConnectionStringCls>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetFilterQuery_Quest", _InstanceUserAssocId, _DBPath, id, itemid, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<ConnectionStringCls>(QuestInstance, "H6_GetFilterQuery_Quest", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region GetQuestFormsForUser
        public async static Task<List<ItemInstanceTranToProcessCase>> GetQuestFormsForUser(bool _IsOnline, string _UserName, string _DBPath)
        {
            List<ItemInstanceTranToProcessCase> lstResult = new List<ItemInstanceTranToProcessCase>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "B1_GetQuestFormsForUser", _DBPath);
            try
            {

                if (_IsOnline)
                {

                    var result = QuestAPIMethods.BoxerCentralHome_GetQuestFormsForUser(_UserName, "50", "F");
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<ItemInstanceTranToProcessCase>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), QuestInstance, "B1_GetQuestFormsForUser", INSTANCE_USER_ASSOC_ID, _DBPath, id, "", "M");
                        }
                    }

                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<ItemInstanceTranToProcessCase>(QuestInstance, "B1_GetQuestFormsForUser", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Get ItemCategories By ItemID #1
        public async static Task<List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>> GetItemCategoriesByItemID(bool _IsOnline, string _intItemID, int _InstanceUserAssocId, string _DBPath, string catid = "")
        {
            List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> ItemList = new List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H6_GetItemCategoriesByItemID", _DBPath);
            try
            {
                int? _sCatId = 0;
                int? _Typeid = 0;
                string _sTypeName = string.Empty;
                if (id == 0)
                {
                    Task<AppTypeInfoList> record = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Convert.ToInt32(_intItemID), QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    record.Wait();
                    _sCatId = record.Result?.CategoryId == null ? Convert.ToInt32(catid) : record.Result?.CategoryId;
                    _Typeid = record.Result?.TYPE_ID;
                    _sTypeName = record.Result?.TYPE_NAME;

                }
                else
                {
                    Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                    db.Wait();
                    if (db.Result != null)
                    {
                        _sCatId = db.Result?.CategoryId == null ? Convert.ToInt32(catid) : db.Result?.CategoryId;
                        _Typeid = db.Result?.TYPE_ID;
                        _sTypeName = db.Result?.TYPE_NAME;
                    }
                }
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemCategoriesByItemID(_intItemID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H6_GetItemCategoriesByItemID", _InstanceUserAssocId, _DBPath, id, _intItemID, "M", "0", _sCatId, _sTypeName);
                        }
                    }
                }
                else
                {
                    var Appinfo = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_sCatId), _DBPath, "H1_H2_H3_QUEST_AREA_FORM", "M");
                    Appinfo.Wait();

                    var EdsInfo = DBHelper.GetEDSResultListwithId(Convert.ToInt32(_intItemID), Appinfo.Result[0].APP_TYPE_INFO_ID, _DBPath);
                    EdsInfo.Wait();
                    ItemList = JsonConvert.DeserializeObject<List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>>(EdsInfo.Result.EDS_RESULT.Split(new string[] { "|||||" }, StringSplitOptions.None)[0]);


                    //ItemList = CommonConstants.ReturnListResult<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>(QuestInstance, "H6_GetItemCategoriesByItemID", _DBPath, _intItemID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ItemQuestionFields By Item CategoryID
        public async static Task<List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>> GetItemQuestionFieldsByItemCategoryID(bool _IsOnline, string _itemQuestionFieldID, int _InstanceUserAssocId, string _DBPath, string Itemid, string scatid)
        {
            List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField> ItemList = new List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            int id = CommonConstants.GetResultBySytemcode(QuestInstance, "H8_GetItemQuestionFieldsByItemCategoryID", _DBPath);
            try
            {
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemQuestionFieldsByItemCategoryID(_itemQuestionFieldID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(ItemList), QuestInstance, "H8_GetItemQuestionFieldsByItemCategoryID", _InstanceUserAssocId, _DBPath, id, Itemid, "M");
                        }
                    }
                }
                else
                {
                    ItemList = CommonConstants.ReturnListResult<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>(QuestInstance, "H8_GetItemQuestionFieldsByItemCategoryID", _DBPath);
                    if (ItemList == null)
                    {

                        var Appinfo = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(Itemid), Convert.ToInt32(scatid), _DBPath, "H1_H2_H3_QUEST_AREA_FORM", "M");
                        Appinfo.Wait();

                        var EdsInfo = DBHelper.GetEDSResultListwithId(Convert.ToInt32(Itemid), Appinfo.Result[0].APP_TYPE_INFO_ID, _DBPath);
                        EdsInfo.Wait();
                        var edsResult = EdsInfo.Result.EDS_RESULT.Contains("*****");
                        if (edsResult)
                        {
                            string str = EdsInfo.Result.EDS_RESULT.Split(new string[] { "|||||" }, StringSplitOptions.None)[1];
                            string sMainsplit = str.Split(new string[] { "*****" }, StringSplitOptions.None)[0];
                            ItemList = JsonConvert.DeserializeObject<List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>>(sMainsplit);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion

        #region Get ItemQuestion Decode By Field ID
        public async static Task<List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>> GetItemQuestionDecodeByFieldID(bool _IsOnline, string intItemQuestionFieldID, int _InstanceUserAssocId, string _DBPath, string _intItemID, string intItemInstanceTranID, string _intCatId, string _sCatId = "")
        {
            List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode> ItemList = new List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            //int id = CommonConstants.GetResultBySytemcodeId(QuestInstance, "H8_GetItemQuestionDecodeByFieldID", Convert.ToInt32(_intItemID), _DBPath);
            Task<List<AppTypeInfoList>> temprecordid = DBHelper.GetAppTypeInfoListByCatIdSyscodeID(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_sCatId), _DBPath, "H8_GetItemQuestionDecodeByFieldID", Convert.ToInt32(intItemInstanceTranID));
            temprecordid.Wait();
            int id = 0;
            if (temprecordid?.Result?.Count > 0)
                id = temprecordid.Result.FirstOrDefault().APP_TYPE_INFO_ID;
            try
            {
                string _sTypeName = string.Empty;
                //int? _sCatId = 0;
                if (id == 0)
                {
                    Task<AppTypeInfoList> record = DBHelper.GetAppTypeInfoListByTypeID_SystemName(Convert.ToInt32(_intItemID), QuestInstance, "H1_H2_H3_QUEST_AREA_FORM", _DBPath);
                    record.Wait();
                    _sTypeName = record.Result?.TYPE_NAME;
                    //_sCatId = record.Result?.CategoryId;

                }
                else
                {
                    Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                    db.Wait();
                    if (db.Result != null)
                    {
                        _sTypeName = db.Result?.TYPE_NAME;
                        // _sCatId = db.Result?.CategoryId;
                    }
                }
                if (_IsOnline)
                {
                    var result = QuestAPIMethods.GetItemQuestionDecodeByFieldID(intItemQuestionFieldID);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        ItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>>(temp.ToString());
                        if (ItemList.Count > 0)
                        {

                            Task<List<AppTypeInfoList>> lst = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_sCatId), _DBPath, "H8_GetItemQuestionDecodeByFieldID", "M", Convert.ToInt32(intItemInstanceTranID));
                            lst.Wait();
                            //Task<AppTypeInfoList> db = DBHelper.GetAppTypeInfoListByPk(id, _DBPath);
                            //db.Wait();
                            string json = JsonConvert.SerializeObject(ItemList);
                            if (lst.Result != null)
                            {
                                if (lst.Result.Where(v => v.ASSOC_FIELD_INFO?.ToUpper()?.Trim() == json?.ToUpper()?.Trim())?.Count() > 0)
                                {
                                    id = lst.Result.Where(v => v.ASSOC_FIELD_INFO?.ToUpper()?.Trim() == json?.ToUpper()?.Trim()).FirstOrDefault().APP_TYPE_INFO_ID;
                                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(json, QuestInstance, "H8_GetItemQuestionDecodeByFieldID", _InstanceUserAssocId, _DBPath, id, _intItemID, "M", intItemInstanceTranID, Convert.ToInt32(_sCatId), _sTypeName, "", true);
                                }
                                else
                                {
                                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(json, QuestInstance, "H8_GetItemQuestionDecodeByFieldID", _InstanceUserAssocId, _DBPath, 0, _intItemID, "M", intItemInstanceTranID, Convert.ToInt32(_sCatId), _sTypeName, "", true);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //ItemList = CommonConstants.ReturnListResult<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>(QuestInstance, "H8_GetItemQuestionDecodeByFieldID", _DBPath, _intItemID);

                    //Task<List<AppTypeInfoList>> lst = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_intCatId), _DBPath, "H8_GetItemQuestionDecodeByFieldID", "M", Convert.ToInt32(intItemInstanceTranID));
                    //lst.Wait();

                    //foreach (var ele in lst.Result)
                    //{
                    //    var elem = JsonConvert.DeserializeObject<List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>>(ele.ASSOC_FIELD_INFO);
                    //    ItemList.AddRange(elem);
                    //}

                    var Appinfoisonline = DBHelper.GetAppTypeInfoListByIsonline(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(intItemInstanceTranID), Convert.ToInt32(_sCatId), id, _DBPath);
                    Appinfoisonline.Wait();
                    if (Appinfoisonline?.Result?.IS_ONLINE == true)
                    {
                        ItemList = JsonConvert.DeserializeObject<List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>>(Appinfoisonline?.Result.ASSOC_FIELD_INFO);
                    }
                    else
                    {
                        var Appinfo = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscode(QuestInstance, Convert.ToInt32(_intItemID), Convert.ToInt32(_sCatId), _DBPath, "H1_H2_H3_QUEST_AREA_FORM", "M");
                        Appinfo.Wait();

                        var EdsInfo = DBHelper.GetEDSResultListwithId(Convert.ToInt32(_intItemID), Appinfo.Result[0].APP_TYPE_INFO_ID, _DBPath);
                        EdsInfo.Wait();
                        var edsResult = EdsInfo.Result.EDS_RESULT.Contains("*****");
                        if (edsResult)
                        {
                            ItemList = JsonConvert.DeserializeObject<List<GetItemQuestionDecodeByFieldIDResponse.ItemQuestionDecode>>(EdsInfo.Result?.EDS_RESULT.ToString()?.Split(new string[] { "*****" }, StringSplitOptions.None)[1]);
                            ItemList = ItemList.Where(v => v.intItemQuestionFieldID == Convert.ToInt32(intItemQuestionFieldID)).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ItemList;
        }
        #endregion


    }
}
