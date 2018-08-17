using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Entity;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;

namespace DataServiceBus.OfflineHelper.DataTypes.Entity
{
    public class EntitySyncAPIMethods
    {
        #region Get All Entity Type With ID
        public static string GetAllEntityTypeWithID(string CategoryId, string User, string _DBPath)
        {
            string sError = string.Empty;
            JObject Result = null;
            try
            {
                MobileAPIMethods Mapi = new MobileAPIMethods();
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Common.ConstantsSync.GetEntityTypeList)
                };
                #endregion

                #region API Body Details
                var Body_value = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("CategoryId", CategoryId),
                    new KeyValuePair<string, string>("User", User)
                };
                #endregion

                Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");

                Debug.WriteLine("GetAllEntityTypeWithID ==> " + Convert.ToString(Result));

                if (Result != null)
                {
                    string ResponseContent = Convert.ToString(Result.GetValue("ResponseContent"));

                    sError = ResponseContent;

                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(Result), "Y", "GetAllEntityTypeWithID", User, DateTime.Now.ToString());

                    if (!string.IsNullOrEmpty(ResponseContent?.ToString()) && Convert.ToString(ResponseContent) != "[]" && Convert.ToString(ResponseContent) != "{}" && Convert.ToString(ResponseContent) != "[ ]" && Convert.ToString(ResponseContent) != "{ }" && Convert.ToString(ResponseContent) != "[{ }]" && Convert.ToString(ResponseContent) != "[{}]")
                    {
                        CommonConstants.MasterOfflineStore(ResponseContent, _DBPath);
                        GetMasterEntityCount(User, _DBPath);
                    }
                }
                else
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(Result), "N", "GetAllEntityTypeWithID", User, DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAllEntityTypeWithID", User, DateTime.Now.ToString());

                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(Result), "N", "GetAllEntityTypeWithID", User, DateTime.Now.ToString());
            }
            return sError;
        }
        #endregion

        #region Get Associated Entity List

        public static string GetAssociatedEntityList(string User, string _DBPath)
        {
            string sError = string.Empty;
            JObject rResult = null;
            try
            {
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("API_Name", Constants.Baseurl  + Common.ConstantsSync.GetEntityTypeList)
                };
                #endregion

                List<BoxerCentralHomePage_EntityList_Mob> Result = new List<BoxerCentralHomePage_EntityList_Mob>();

                List<KeyValuePair<string, string>> idAndDateTime = new List<KeyValuePair<string, string>>();
                try
                {
                    Task<List<AppTypeInfoList>> AppTypeInforesult = DBHelper.GetAppTypeInfoListBySystemName(ConstantsSync.EntityInstance, EntityItemView, _DBPath);
                    AppTypeInforesult.Wait();
                    if (AppTypeInforesult.Result != null)
                    {
                        foreach (var item in AppTypeInforesult.Result)
                        {
                            var it = JsonConvert.DeserializeObject<EntityClass>(item.ASSOC_FIELD_INFO);

                            if (it.EntitiyAssignedToUserName.ToLower() == User.ToLower())
                                idAndDateTime.Add(new KeyValuePair<string, string>(Convert.ToString(it.EntityID), Convert.ToString(Convert.ToDateTime(it.EntityModifiedDateTime))));
                        }
                    }
                }
                catch (Exception)
                {

                }

                GetAssociatedEntityListRequest getAssociatedEntityList = new GetAssociatedEntityListRequest() { USER_SAM = User, list = idAndDateTime };


                rResult = Constants.ApiCommon(getAssociatedEntityList, Constants.GetAssociatedEntityList);

                Debug.WriteLine("GetAssociatedEntityList ==> " + Convert.ToString(rResult));

                if (rResult != null)
                {
                    string ResponseContent = Convert.ToString(rResult.GetValue("ResponseContent"));

                    MasterSyncGetAllEntityType Output = JsonConvert.DeserializeObject<MasterSyncGetAllEntityType>(ResponseContent);
                    var DeleteItem = Output.RemoveItemList;

                    ResponseContent = JsonConvert.SerializeObject(Output.GetAllEntityType);

                    sError = ResponseContent;

                    DefaultAPIMethod.AddLog("Result Success Log => " + Convert.ToString(rResult), "Y", "GetAssociatedEntityList", User, DateTime.Now.ToString());

                    if (!string.IsNullOrEmpty(ResponseContent?.ToString()) && Convert.ToString(ResponseContent) != "[]" && Convert.ToString(ResponseContent) != "{}" && Convert.ToString(ResponseContent) != "[ ]" && Convert.ToString(ResponseContent) != "{ }" && Convert.ToString(ResponseContent) != "[{ }]" && Convert.ToString(ResponseContent) != "[{}]")
                    {
                        CommonConstants.MasterOfflineStore(ResponseContent, _DBPath);
                    }
                }
                else
                {
                    DefaultAPIMethod.AddLog("Result Fail Log => " + Convert.ToString(rResult), "N", "GetAssociatedEntityList", User, DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                DefaultAPIMethod.AddLog("Exceptions Log => " + ex.Message.ToString(), "N", "GetAssociatedEntityList", User, DateTime.Now.ToString());
                DefaultAPIMethod.AddLog("Result Exceptions Log => " + Convert.ToString(rResult), "N", "GetAssociatedEntityList", User, DateTime.Now.ToString());
            }
            return sError;
        }
        #endregion

        #region #1 Get Entity Catogory List
        public async static Task<List<Entity_categoryList>> GetEntitycatogoryList(bool _IsOnline, string Sys_Name, int _caseTypeID, object _Body_value, string _DBPath, string Username)
        {
            List<Entity_categoryList> EntityList = new List<Entity_categoryList>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            try
            {
                if (_IsOnline)
                {
                    var result = EntityAPIMethods.GetEntityTypeCategoryList(Username);

                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        EntityList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Entity_categoryList>>(temp.ToString());
                    }
                }
                else
                {
                    lstResult = await DBHelper.GetAppTypeInfoListBySystemName(ConstantsSync.EntityInstance, Entity_Category_TypeDetails, _DBPath);

                    var sd = lstResult.Select(o => new { o.CategoryId, o.CategoryName }).Distinct().ToList();

                    foreach (var item in sd)
                    {
                        if (item.CategoryId > 0)
                            EntityList.Add(new Entity_categoryList(item.CategoryId, item.CategoryName, "", "", ""));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            EntityList.Insert(0, new Entity_categoryList(0, "All", "0", "All", "y"));
            return EntityList;
        }
        #endregion

        #region #2 Get Entity Type List
        public async static Task<List<EntityOrgCenterList>> GetEntityTypeList(bool _IsOnline, int? CatId, string username, object _Body_value, string _DBPath)
        {
            List<EntityOrgCenterList> EntityList = new List<EntityOrgCenterList>();
            List<AppTypeInfoList> lstResult = new List<AppTypeInfoList>();
            try
            {
                if (_IsOnline)
                {
                    var result = EntityAPIMethods.GetEntityTypeList(CatId.ToString() ?? null, username);
                    var temp = result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                        EntityList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntityOrgCenterList>>(temp.ToString());
                }
                else
                {
                    if (CatId != null)
                        lstResult = await DBHelper.GetAppTypeInfoListByCategoryId(EntityInstance, CatId, EntityType_CountDetails, _DBPath);
                    else
                        lstResult = await DBHelper.GetAppTypeInfoListBySystemName(EntityInstance, EntityType_CountDetails, _DBPath);

                    foreach (var item in lstResult)
                    {
                        var temp = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntityOrgCenterList>>(item.ASSOC_FIELD_INFO.ToString());

                        foreach (var it in temp)
                        {
                            EntityList.Add(it);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EntityList;
        }
        #endregion

        #region #3 GetEntitiesBySystemCodeKeyValuePair_LazyLoad
        public async static Task<List<EntityClass>> GetEntitiesBySystemCodeKeyValuePair_LazyLoadCommon(bool _IsOnline, string username, GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest _Body_value, string _DBPath, int EntityTypeId, string Fullname, string _Viewtype)
        {
            List<EntityClass> json = null;
            List<EntityClass> ListEntity = new List<EntityClass>();
            string ScreenName = EntityItemList_Lazyload;
            if (_Viewtype?.ToLower() == "assigned to me")
                ScreenName = EntityItemList_Lazyload + " ||| AssignedToMe";
            if (_Viewtype?.ToLower() == "active")
                ScreenName = EntityItemList_Lazyload + " ||| Active";
            if (_Viewtype?.ToLower() == "inactive")
                ScreenName = EntityItemList_Lazyload + " ||| Inactive";
            if (_Viewtype?.ToLower() == "created by me")
                ScreenName = EntityItemList_Lazyload + " ||| CreatedByMe";
            if (_Viewtype?.ToLower() == "owned by me")
                ScreenName = EntityItemList_Lazyload + " ||| OwnedByMe";
            if (_Viewtype?.ToLower() == "associated by me")
                ScreenName = EntityItemList_Lazyload + " ||| AssociatedByMe";
            if (_Viewtype?.ToLower() == "inactivated by me")
                ScreenName = EntityItemList_Lazyload + " ||| InactivatedByMe";


            try
            {
                if (_IsOnline)
                {
                    var Result = EntityAPIMethods.GetEntitiesBySystemCodeKeyValuePair_LazyLoad(_Body_value);

                    string jsonValue = Convert.ToString(Result.GetValue("ResponseContent"));

                    ListEntity = JsonConvert.DeserializeObject<List<EntityClass>>(jsonValue);
                    if (ListEntity.Count > 0)
                    {
                        AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList
                        {
                            APP_TYPE_INFO_ID = 0,
                            ASSOC_FIELD_INFO = jsonValue,
                            LAST_SYNC_DATETIME = DateTime.Now,
                            SYSTEM = ConstantsSync.EntityInstance,
                            TYPE_ID = EntityTypeId,
                            TYPE_NAME = ListEntity[0].EntityTypeName,
                            CategoryId = 0,
                            CategoryName = "",
                            TransactionType = "M",
                            TYPE_SCREEN_INFO = ScreenName,
                            INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                            IS_ONLINE = _IsOnline

                        };
                        ListEntity = ListEntity.Select(i =>
                        {
                            i.TransactionType = "M";
                            return i;
                        }).ToList();
                        Task<int> inserted = DBHelper.UpdateAppTypeInfoList(_AppTypeInfoList, _DBPath);
                        inserted.Wait();
                    }
                }
                else
                {
                    json = new List<EntityClass>();
                    List<AppTypeInfoList> OnlineresultG4 = await DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(ConstantsSync.EntityInstance, EntityTypeId, _DBPath, ScreenName, "M");
                    List<AppTypeInfoList> OfflineresultG8 = await DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(ConstantsSync.EntityInstance, EntityTypeId, _DBPath, EntityItemView, "T");
                    bool flg = false;
                    if (OnlineresultG4.Count > 0)
                    {
                        json = JsonConvert.DeserializeObject<List<EntityClass>>(OnlineresultG4.Select(v => v.ASSOC_FIELD_INFO).FirstOrDefault());
                        json = json.Select(i =>
                        {
                            i.TransactionType = "M";
                            return i;
                        }).ToList();
                    }
                    if (_Viewtype?.ToLower() == "created by me" || string.IsNullOrEmpty(_Viewtype))
                    {
                        if (OfflineresultG8.Count > 0)
                        {
                            foreach (var item in OfflineresultG8)
                            {
                                var temp = JsonConvert.DeserializeObject<EntityClass>(item.ASSOC_FIELD_INFO);
                                temp.EntityCreatedDateTime = Convert.ToDateTime(item.LAST_SYNC_DATETIME).ToString();
                                temp.EntityCreatedByFullName = Fullname;
                                temp.ItemtransinfoID = item.ID;
                                temp.TransactionType = "T";
                                if (json.FindAll(v => v.EntityID == Convert.ToInt32(temp.EntityID))?.Count > 0)
                                {
                                    var vv = json.Where(v => v.EntityID == temp.EntityID);
                                    var Result = json.Where(p => !vv.Any(p2 => p2.EntityID == p.EntityID));
                                    if (!flg)
                                    {
                                        foreach (var itm in Result)
                                        {
                                            ListEntity.Add(itm);
                                            flg = true;
                                        }
                                    }
                                    var res = vv.Select(v =>
                                    {
                                        v.EntityTypeModifiedBySAM = temp.EntityTypeModifiedBySAM;
                                        v.EntityModifiedByFullName = temp.EntityModifiedByFullName;
                                        v.AssociationFieldCollection = temp.AssociationFieldCollection;
                                        v.EntityTypeName = temp.EntityTypeName;
                                        v.EntityID = temp.EntityID;
                                        return v;
                                    });
                                    ListEntity.AddRange(res);
                                    //return ListEntity;
                                }
                                else
                                    ListEntity.Add(temp);
                            }
                            if (!flg && json.Count > 0)
                                ListEntity.AddRange(json);
                        }
                        else
                            ListEntity = json;
                    }
                    return ListEntity.OrderByDescending(lst => lst.ListID).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ListEntity.OrderByDescending(lst => lst.ListID).ToList(); ;
        }
        #endregion

        #region #4 Get Entity Type Schema For Design Page
        public async static Task<EntityClass> GetEntityTypeSchema(bool _IsOnline, string Sys_Name, int Entity_TypeID, string username, object _Body_value, string _DBPath, string _Mode)
        {
            EntityClass EntityList = new EntityClass();
            try
            {
                if (_IsOnline)
                {
                    var Result = EntityAPIMethods.GetEntityTypeSchemaByEntityTypeID(Entity_TypeID.ToString(), username);

                    var ResponseValue = Result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(ResponseValue?.ToString()) && ResponseValue.ToString() != "[]")
                    {
                        EntityList = JsonConvert.DeserializeObject<EntityClass>(ResponseValue.ToString());
                    }
                }
                else
                {
                    dynamic lstResult = null;
                    lstResult = DBHelper.GetAppTypeInfoListByNameTypeIdScreenInfo(Sys_Name, Entity_Category_TypeDetails, Entity_TypeID, _DBPath, null);
                    lstResult = lstResult.Result as AppTypeInfoList;

                    if (lstResult != null)
                        EntityList = JsonConvert.DeserializeObject<EntityClass>(lstResult.ASSOC_FIELD_INFO.ToString());

                    for (int i = 0; i < EntityList.AssociationFieldCollection.Count(); i++)
                    {
                        if (EntityList.AssociationFieldCollection[i].FieldType.ToLower() == "se" || EntityList.AssociationFieldCollection[i].FieldType.ToLower() == "el" || EntityList.AssociationFieldCollection[i].FieldType.ToLower() == "ms" || EntityList.AssociationFieldCollection[i].FieldType.ToLower() == "me" || EntityList.AssociationFieldCollection[i].FieldType.ToLower() == "ss")
                        {
                            //if (EntityList.AssociationFieldCollection[i].ExternalDataSourceID != null)
                            {
                                var Result = await DBHelper.GetEDSResultListwithId(EntityList.AssociationFieldCollection[i].AssocTypeID, lstResult.APP_TYPE_INFO_ID, _DBPath);
                                if (Result != null)
                                {
                                    string jsonvalue = Result.EDS_RESULT;
                                    List<EXTERNAL_DATASOURCE1> exdDetail = JsonConvert.DeserializeObject<List<EXTERNAL_DATASOURCE1>>(jsonvalue);
                                    EntityList.AssociationFieldCollection[i].EXTERNAL_DATASOURCE = exdDetail;
                                }
                                else
                                    EntityList.AssociationFieldCollection[i].EXTERNAL_DATASOURCE = new List<EXTERNAL_DATASOURCE1>();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
            return EntityList;
        }
        #endregion

        #region #5 Add Entity Notes

        public static async Task<string> StoreEntityNotes(bool _IsOnline, int _EntityTypeID, string _EntityID, string _Entitynotes, string _UserName, string _Notestypeid, string _Actiontype, string _DBPath, string Fullname = "")
        {
            string insertedRecordid = "0";
            _Notestypeid = "1";

            AddEntityNoteRequest AddEntityNote = new AddEntityNoteRequest();
            AddEntityNote.ENTITY_ID = Convert.ToInt32(_EntityID);
            AddEntityNote.NOTE = _Entitynotes;
            AddEntityNote.ENTITY_NOTE_TYPE_ID = Convert.ToInt32(_Notestypeid);
            AddEntityNote.CREATED_BY = _UserName;
            int id = CommonConstants.GetResultBySytemcodeId(EntityInstance, "G1_G2_Entity_Cate_TypeDetails", Convert.ToInt32(_EntityTypeID), _DBPath);
            string _EntityTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath).Result.TYPE_NAME;


            if (_IsOnline)
            {
                if (!string.IsNullOrEmpty(_Entitynotes))
                {
                    var result = EntityAPIMethods.AddEntityNote(AddEntityNote);

                    insertedRecordid = (string)result.GetValue("ResponseContent");

                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(EntityInstance, _EntityTypeID, Convert.ToInt32(_EntityID), _DBPath, EntityItemView, "", true);
                        IscheckOnlineRecordApppType.Wait();
                        if (IscheckOnlineRecordApppType.Result.Count > 0)
                        {

                            var t = await CommonConstants.AddofflineTranInfo(_EntityTypeID, AddEntityNote, Constants.AddEntityNote, _Actiontype, Convert.ToInt32(_EntityID), Convert.ToInt32(_EntityID), _DBPath, EntityInstance, id, "", IscheckOnlineRecordApppType?.Result?.FirstOrDefault()?.TYPE_SCREEN_INFO);
                            insertedRecordid = t.ToString();
                        }
                        else
                        {
                            var t = await CommonConstants.AddofflineTranInfo(_EntityTypeID, AddEntityNote, Constants.AddEntityNote, _Actiontype, Convert.ToInt32(_EntityID), Convert.ToInt32(_EntityID), _DBPath, EntityInstance, id, "", "", IscheckOnlineRecordApppType?.Result?.FirstOrDefault()?.TYPE_SCREEN_INFO);
                            insertedRecordid = t.ToString();
                        }

                        SaveEntityViewJsonLocal(_EntityID, _EntityTypeID.ToString(), _DBPath, "", JsonConvert.SerializeObject(AddEntityNote), "M", EntityItemNotes, _IsOnline);

                    }
                    else
                    {
                        insertedRecordid = result.GetValue("Message").ToString();
                    }
                }
            }
            else
            {
                var t = await CommonConstants.AddofflineTranInfo(_EntityTypeID, AddEntityNote, Constants.AddEntityNote, _Actiontype, 0, Convert.ToInt32(_EntityID), _DBPath, EntityInstance, id, "C", EntityItemNotes);
                insertedRecordid = t.ToString();

                SaveEntityViewJsonLocal(_EntityID, _EntityTypeID.ToString(), _DBPath, "", JsonConvert.SerializeObject(AddEntityNote), "T", EntityItemNotes, _IsOnline);
            }

            return insertedRecordid;
        }


        #endregion

        #region #6 View Entity Service Bus
        #region #6 Get Entity By EntityID
        public async static Task<EntityClass> GetEntityByEntityID(bool _IsOnline, string EntityID, string username, string EntityTypeID, string _DBPath)
        {
            EntityClass EntityLists = null;
            try
            {
                if (_IsOnline)
                {
                    var EntityResult = EntityAPIMethods.GetEntityByEntityID(EntityID, username, "Y");
                    var EntityResponse = EntityResult.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(EntityResponse?.ToString()) && EntityResponse.ToString() != "[]")
                    {
                        EntityLists = Newtonsoft.Json.JsonConvert.DeserializeObject<EntityClass>(EntityResponse.ToString());

                        SaveEntityViewJsonLocal(EntityID, EntityTypeID, _DBPath, EntityLists.EntityTypeName, EntityResponse.ToString(), "M", EntityItemView, _IsOnline);
                    }
                }
                else
                {
                    AppTypeInfoList result = await DBHelper.GetAppTypeInfoList(EntityInstance, Convert.ToInt32(EntityID), Convert.ToInt32(EntityTypeID), EntityItemView, _DBPath, null);
                    if (result != null)
                    {
                        EntityLists = JsonConvert.DeserializeObject<EntityClass>(result.ASSOC_FIELD_INFO);
                        EntityLists.TransactionType = result.TransactionType;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return EntityLists;
        }


        #endregion

        private static void SaveEntityViewJsonLocal(string EntityID, string EntityTypeID, string _DBPath, string EntityTypeName, string EntityResponse, string _transtype, string _TYPE_SCREEN_INFO, bool onlineRecord)
        {
            Task<List<AppTypeInfoList>> record = DBHelper.GetAllAppTypeInfoList(_DBPath);
            record.Wait();

            var ischeck = record.Result.Where(v => v.SYSTEM == EntityInstance && v.TYPE_ID == Convert.ToInt32(EntityTypeID) && v.TYPE_NAME == EntityTypeName && v.TYPE_SCREEN_INFO == _TYPE_SCREEN_INFO && v.ID == Convert.ToInt32(EntityID) && v.IS_ONLINE == onlineRecord).FirstOrDefault();
            //var ischeck = record.Result.Where(v => v.SYSTEM == EntityInstance && v.TYPE_ID == Convert.ToInt32(EntityTypeID) && v.TYPE_NAME == EntityTypeName && v.TYPE_SCREEN_INFO == _TYPE_SCREEN_INFO && v.ID == Convert.ToInt32(EntityID) && v.IS_ONLINE == onlineRecord).FirstOrDefault();
            if (ischeck == null)
            {
                AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList
                {
                    APP_TYPE_INFO_ID = 0,
                    ASSOC_FIELD_INFO = EntityResponse.ToString(),
                    LAST_SYNC_DATETIME = DateTime.Now,
                    SYSTEM = EntityInstance,
                    TYPE_ID = Convert.ToInt32(EntityTypeID),
                    ID = Convert.ToInt32(EntityID),
                    TYPE_NAME = EntityTypeName,
                    CategoryId = 0,
                    CategoryName = "",
                    TransactionType = _transtype,
                    TYPE_SCREEN_INFO = _TYPE_SCREEN_INFO,
                    INSTANCE_USER_ASSOC_ID = INSTANCE_USER_ASSOC_ID,
                    IS_ONLINE = onlineRecord
                };

                Task<int> inserted = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, _DBPath);
                inserted.Wait();
            }
            else
            {
                ischeck.ASSOC_FIELD_INFO = EntityResponse.ToString();
                Task<int> inserted = DBHelper.SaveAppTypeInfo(ischeck, _DBPath);
                inserted.Wait();
            }
        }

        #region #6 Get Entity Notes By EntityID
        public async static Task<List<Entity_Notes>> GetNotes(bool _IsOnline, string EntityID, string username, string EntityTypeID, string EntityTypeName, string _DBPath)
        {
            List<Entity_Notes> EntityNotesList = null;
            try
            {
                if (_IsOnline)
                {
                    var EntityNotesResult = EntityAPIMethods.GetNotes(username, EntityID);
                    var EntityNotesResponse = EntityNotesResult.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(EntityNotesResponse?.ToString()) && EntityNotesResponse.ToString() != "[]")
                    {
                        EntityNotesList = JsonConvert.DeserializeObject<List<Entity_Notes>>(EntityNotesResponse.ToString());

                        Task<List<AppTypeInfoList>> record = DBHelper.GetAllAppTypeInfoList(_DBPath);
                        record.Wait();

                        var ischeck = record.Result.Where(v => v.SYSTEM == EntityInstance && v.TYPE_ID == Convert.ToInt32(EntityTypeID) && v.TYPE_NAME == EntityTypeName && v.TYPE_SCREEN_INFO == EntityItemNotes && v.TransactionType?.ToUpper() == "M").FirstOrDefault();

                        //var ischeck = record.Result.Where(v => v.SYSTEM == EntityInstance && v.TYPE_ID == Convert.ToInt32(EntityTypeID) && v.TYPE_NAME == EntityTypeName && v.TYPE_SCREEN_INFO == EntityItemNotes && v.TransactionType?.ToUpper() == "M").FirstOrDefault();
                        if (ischeck == null)
                        {
                            AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList
                            {
                                APP_TYPE_INFO_ID = 0,
                                ASSOC_FIELD_INFO = EntityNotesResponse.ToString(),
                                LAST_SYNC_DATETIME = DateTime.Now,
                                SYSTEM = ConstantsSync.EntityInstance,
                                TYPE_ID = Convert.ToInt32(EntityTypeID),
                                ID = Convert.ToInt32(EntityID),
                                TYPE_NAME = EntityTypeName,
                                CategoryId = 0,
                                CategoryName = "",
                                TransactionType = "M",
                                TYPE_SCREEN_INFO = EntityItemNotes,
                                INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                                IS_ONLINE = _IsOnline
                            };

                            Task<int> inserted = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, _DBPath);
                            inserted.Wait();
                        }
                        else
                        {
                            ischeck.ASSOC_FIELD_INFO = Convert.ToString(EntityNotesResponse);
                            Task<int> inserted = DBHelper.SaveAppTypeInfo(ischeck, _DBPath);
                            inserted.Wait();
                        }
                    }
                }
                else
                {
                    EntityNotesList = new List<Entity_Notes>();
                    List<AppTypeInfoList> Off_resultG8 = await DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(EntityInstance, Convert.ToInt32(EntityTypeID), Convert.ToInt32(EntityID), _DBPath, EntityItemNotes, "T", null);
                    List<AppTypeInfoList> On_resultG8 = await DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(EntityInstance, Convert.ToInt32(EntityTypeID), Convert.ToInt32(EntityID), _DBPath, EntityItemNotes, "M", null);
                    if (On_resultG8.Count > 0)
                    {
                        EntityNotesList = JsonConvert.DeserializeObject<List<Entity_Notes>>(On_resultG8.Where(v => v.ASSOC_FIELD_INFO != null && v.ASSOC_FIELD_INFO != "").Select(a => a.ASSOC_FIELD_INFO).FirstOrDefault());
                    }
                    if (Off_resultG8.Count > 0)
                    {
                        foreach (var item in Off_resultG8)
                        {
                            Entity_Notes EntityNOtes = new Entity_Notes();
                            var tempEntityNOtesList = JsonConvert.DeserializeObject<AddEntityNoteRequest>(item.ASSOC_FIELD_INFO);
                            EntityNOtes.ID = Convert.ToString(tempEntityNOtesList.ENTITY_ID);
                            EntityNOtes.Note = Convert.ToString(tempEntityNOtesList.NOTE);
                            EntityNOtes.EntityNoteType = Convert.ToString(tempEntityNOtesList.ENTITY_NOTE_TYPE_ID);
                            EntityNOtes.CreatedBy = tempEntityNOtesList.CREATED_BY;
                            EntityNOtes.ModifiedByFullName = Convert.ToString(tempEntityNOtesList.CREATED_BY);
                            EntityNOtes.CreatedDatetime = Convert.ToDateTime(item.LAST_SYNC_DATETIME).ToString();
                            EntityNotesList.Add(EntityNOtes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                EntityNotesList = null;
                throw ex;
            }
            return EntityNotesList;
        }
        #endregion

        #region #6 Get Entity Related Applications
        public async static Task<string> GetEntityRelatedApplications(bool _IsOnline, string EntityTypeName, string EntityID, string EntityTypeID, string username, string _DBPath)
        {
            string RelatedAppData = "";
            try
            {
                if (_IsOnline)
                {
                    var EntityRealatedResult = EntityAPIMethods.GetEntityRelatedApplications(EntityTypeID, EntityID, username);
                    RelatedAppData = EntityRealatedResult.GetValue("ResponseContent").ToString();
                    if (!string.IsNullOrEmpty(RelatedAppData?.ToString()) && RelatedAppData.ToString() != "[]")
                    {
                        SaveEntityViewJsonLocal(EntityID, EntityTypeID.ToString(), _DBPath, "", RelatedAppData.ToString(), "M", EntityRelatedApplications, _IsOnline);
                    }
                }
                else
                {
                    AppTypeInfoList result = await DBHelper.GetAppTypeInfoList(EntityInstance, Convert.ToInt32(EntityID), Convert.ToInt32(EntityTypeID), EntityRelatedApplications, _DBPath, null);
                    if (result != null)
                        RelatedAppData = result.ASSOC_FIELD_INFO;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RelatedAppData;
        }
        #endregion
        #endregion

        #region #7 Get Entity Related Types
        public async static Task<List<EntityAppRelatedTypes>> GetEntityRelatedTypes(bool _IsOnline, string EntityTypeName, string EntityID, string EntityTypeID, string username, string _DBPath)
        {
            List<EntityAppRelatedTypes> RelatedAppData = null;
            List<string> templist = new List<string>();
            try
            {
                if (_IsOnline)
                {
                    var Result = EntityAPIMethods.GetEntityRelatedTypes(EntityTypeID, EntityID, username, EntityTypeName?.ToString());//"10", "5572"
                    var _temp = Result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(_temp?.ToString()) && _temp.ToString() != "[]")
                    {
                        foreach (var item in _temp)
                        {
                            var app = item.ToString();
                            templist.Add(app);
                        }
                        List<EntityAppRelatedTypes> TempRelatedAppData = new List<EntityAppRelatedTypes>();
                        for (int i = 0; i < templist.Count(); i++)
                        {
                            EntityAppRelatedTypes rItem = new EntityAppRelatedTypes(templist[i].Split(':')[1].Trim().TrimStart('"').TrimEnd('"'), templist[i].Split(':')[0].Trim().TrimStart('"').TrimEnd('"'));

                            if (templist[i].Contains("|||"))
                            {
                                rItem.AreaId = templist[i].Split(new string[] { "|||" }, StringSplitOptions.None)[1];
                                rItem.Name = templist[i].Split(new string[] { "," }, StringSplitOptions.None)[1].Trim('\"');
                                rItem.ITEM_INSTANCE_TRAN_ID = templist[i].Split(new string[] { "," }, StringSplitOptions.None)[2].Trim('\"');
                            }
                            else
                                rItem.AreaId = "0";

                            TempRelatedAppData.Add(rItem);
                        }
                        RelatedAppData = TempRelatedAppData;

                        SaveEntityViewJsonLocal(EntityID, EntityTypeID.ToString(), _DBPath, "", _temp.ToString(), "M", EntityRelatedTypesList + "|||||" + EntityTypeName, _IsOnline);
                    }
                }
                else
                {
                    var result = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(EntityInstance, Convert.ToInt32(EntityTypeID), Convert.ToInt32(EntityID), _DBPath, EntityRelatedTypesList + "|||||" + EntityTypeName, "M", null);
                    result.Wait();
                    if (result.Result != null)
                    {
                        var tk = result.Result[0].ASSOC_FIELD_INFO;
                        var pk = JToken.Parse(tk);
                        foreach (var item in pk)
                        {
                            var app = item.ToString();
                            templist.Add(app);
                        }
                        List<EntityAppRelatedTypes> TempRelatedAppData = new List<EntityAppRelatedTypes>();
                        for (int i = 0; i < templist.Count(); i++)
                        {
                            EntityAppRelatedTypes rItem = new EntityAppRelatedTypes(templist[i].Split(':')[1].Trim().TrimStart('"').TrimEnd('"'), templist[i].Split(':')[0].Trim().TrimStart('"').TrimEnd('"'));

                            if (templist[i].Contains("|||"))
                            {
                                rItem.AreaId = templist[i].Split(new string[] { "|||" }, StringSplitOptions.None)[1];
                                rItem.Name = templist[i].Split(new string[] { "," }, StringSplitOptions.None)[1].Trim('\"');
                                rItem.ITEM_INSTANCE_TRAN_ID = templist[i].Split(new string[] { "," }, StringSplitOptions.None)[2].Trim('\"');
                            }
                            else
                                rItem.AreaId = "0";

                            TempRelatedAppData.Add(rItem);
                        }
                        RelatedAppData = TempRelatedAppData;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RelatedAppData;
        }
        #endregion

        #region #8 Get Entity Related Data
        public async static Task<List<EntityRelationData>> GetEntitiesRelationData(bool _IsOnline, string EntityID, string EntityTypeID, string username, string _DBPath, object req)
        {
            List<EntityRelationData> RelatedAppData = null;
            try
            {
                if (_IsOnline)
                {

                    var Result = EntityAPIMethods.GetEntitiesRelationData(req);
                    var _temp = Result.GetValue("ResponseContent");
                    if (!string.IsNullOrEmpty(_temp?.ToString()) && _temp.ToString() != "[]")
                    {
                        RelatedAppData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntityRelationData>>(_temp.ToString());

                        SaveEntityViewJsonLocal(EntityID, EntityTypeID.ToString(), _DBPath, "", _temp.ToString(), "M", EntityRelatedData, _IsOnline);
                    }
                }
                else
                {
                    var result = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(EntityInstance, Convert.ToInt32(EntityTypeID), Convert.ToInt32(EntityID), _DBPath, EntityRelatedData, "M", null);
                    result.Wait();
                    if (result.Result.Count > 0)
                    {
                        RelatedAppData = JsonConvert.DeserializeObject<List<EntityRelationData>>(result.Result[0].ASSOC_FIELD_INFO.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RelatedAppData;
        }
        #endregion

        #region #8 Get Cases Related Data
        public async static Task<List<EntityRelationData>> GetCasesRelationData(bool _IsOnline, string EntityId, string CaseTypeId, string User, string GetCasesStatus, string pageIndex, string pageSize, string sortBy, string sortOrder, string searchXml, string getListFor, string _DBPath)
        {
            List<EntityRelationData> RelatedAppData = null;
            try
            {
                if (_IsOnline)
                {
                    var Result = EntityAPIMethods.GetCasesRelationData(EntityId, CaseTypeId, User, GetCasesStatus, pageIndex, pageSize, sortBy, sortOrder, searchXml, getListFor);
                    var _temp = Result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(_temp?.ToString()) && _temp.ToString() != "[]")
                    {
                        RelatedAppData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntityRelationData>>(_temp.ToString());

                        SaveEntityViewJsonLocal(EntityId, CaseTypeId.ToString(), _DBPath, "", _temp.ToString(), "M", CasesRelatedData, _IsOnline);
                    }
                }
                else
                {
                    var result = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(EntityInstance, Convert.ToInt32(CaseTypeId), Convert.ToInt32(EntityId), _DBPath, CasesRelatedData, "M", null);
                    result.Wait();
                    if (result.Result.Count > 0)
                    {
                        RelatedAppData = JsonConvert.DeserializeObject<List<EntityRelationData>>(result.Result[0].ASSOC_FIELD_INFO.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RelatedAppData;
        }
        #endregion

        #region #8 Get Quest Related Data
        public async static Task<List<EntityRelationData>> GetQuestRelationData(bool _IsOnline, string EntityId, string AreaId, string ItemId, string User, string _DBPath)
        {
            List<EntityRelationData> RelatedAppData = null;
            try
            {
                if (_IsOnline)
                {
                    var Result = EntityAPIMethods.GetQuestRelationData(EntityId, AreaId, ItemId, User);
                    var _temp = Result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(_temp?.ToString()) && _temp.ToString() != "[]")
                    {
                        RelatedAppData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EntityRelationData>>(_temp.ToString());

                        SaveEntityViewJsonLocal(EntityId, AreaId.ToString(), _DBPath, "", _temp.ToString(), "M", QuestRelatedData, _IsOnline);
                    }
                }
                else
                {
                    var result = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list(EntityInstance, Convert.ToInt32(AreaId), Convert.ToInt32(EntityId), _DBPath, QuestRelatedData, "M", null);
                    result.Wait();
                    if (result.Result.Count > 0)
                    {
                        RelatedAppData = JsonConvert.DeserializeObject<List<EntityRelationData>>(result.Result[0].ASSOC_FIELD_INFO.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RelatedAppData;
        }
        #endregion

        #region Store and Create Entity
        public static async Task<int> StoreAndcreateEntity(bool _IsOnline, int _TypeID, bool isEdit, object _Body_value, string _DBPath)
        {
            int insertedRecordid = -1;
            string method = "";
            string _Action = "";

            int id = CommonConstants.GetResultBySytemcodeId(EntityInstance, "G1_G2_Entity_Cate_TypeDetails", _TypeID, _DBPath);
            string _EntityTitle = DBHelper.GetAppTypeInfoListByPk(id, _DBPath)?.Result?.TYPE_NAME;
            string Mode = "";

            try
            {
                if (_IsOnline)
                {
                    if (!isEdit)
                    {
                        var Result = EntityAPIMethods.CreateNewEntityItem(_Body_value);
                        string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                        insertedRecordid = Convert.ToInt32(temp == "" ? "0" : temp);
                        method = Constants.CreateNewEntityItem;
                        Mode = "C";
                        _Action = (Enum.GetNames(typeof(ActionTypes)))[4];
                    }
                    else
                    {
                        var Result = EntityAPIMethods.SaveEntityItem(_Body_value);
                        string temp = Convert.ToString(Result.GetValue("ResponseContent"));
                        insertedRecordid = Convert.ToInt32(temp == "" ? "0" : temp);
                        method = Constants.SaveEntityItem;
                        Mode = "U";
                        _Action = (Enum.GetNames(typeof(ActionTypes)))[5];
                    }

                    if (insertedRecordid > 0)
                    {
                        Task<List<AppTypeInfoList>> IscheckOnlineRecordApppType = DBHelper.GetAppTypeInfoListByIdTransTypeSyscodeIsonline(EntityInstance, _TypeID, insertedRecordid, _DBPath, EntityItemView, "", true);
                        IscheckOnlineRecordApppType.Wait();
                        if (IscheckOnlineRecordApppType.Result.Count > 0)
                        {

                            await CommonConstants.AddofflineTranInfo(_TypeID, _Body_value, method, _Action, 0, insertedRecordid, _DBPath, EntityInstance, insertedRecordid, Mode, IscheckOnlineRecordApppType?.Result?.FirstOrDefault()?.TYPE_SCREEN_INFO);
                        }
                        else
                        {
                            await CommonConstants.AddofflineTranInfo(_TypeID, _Body_value, method, _Action, insertedRecordid, insertedRecordid, _DBPath, EntityInstance, id, Mode);
                        }

                        CreateNewEntityItemRequest tmp = new CreateNewEntityItemRequest();
                        var t = JsonConvert.SerializeObject(_Body_value);
                        tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateNewEntityItemRequest>(t.ToString());
                        var enttype = (JsonConvert.DeserializeObject<EntityClass>(JsonConvert.SerializeObject(tmp.entityTypeSchema).ToString()));
                        enttype.EntityID = insertedRecordid;
                        enttype.EntityTypeName = _EntityTitle;

                        SaveEntityViewJsonLocal(insertedRecordid.ToString(), _TypeID.ToString(), _DBPath, enttype.EntityTypeName.ToString(), JsonConvert.SerializeObject(enttype), "M", EntityItemView, _IsOnline);
                    }
                }
                else
                {
                    if (!isEdit)
                    {
                        method = Constants.CreateNewEntityItem;
                        Mode = "C";
                        _Action = (Enum.GetNames(typeof(ActionTypes)))[4];
                    }
                    else
                    {
                        method = Constants.CreateNewEntityItem;
                        _Action = (Enum.GetNames(typeof(ActionTypes)))[4];
                        Mode = "U";
                        var tempRecord = _Body_value as CreateNewEntityItemRequest;
                        id = tempRecord.entityTypeSchema.EntityID;
                    }


                    insertedRecordid = await CommonConstants.AddofflineTranInfo(_TypeID, _Body_value, method, _Action, 0, insertedRecordid, _DBPath, EntityInstance, id, Mode, EntityItemView);

                    int? pkId = 0;
                    Task<AppTypeInfoList> record = null;
                    if (Mode?.ToUpper() == "U")
                    {
                        record = DBHelper.GetAppTypeInfoList(EntityInstance, Convert.ToInt32(id), _TypeID, EntityItemView, _DBPath, null);
                        record.Wait();
                        pkId = record.Result?.APP_TYPE_INFO_ID;
                    }

                    CreateNewEntityItemRequest tmp = new CreateNewEntityItemRequest();
                    var t = JsonConvert.SerializeObject(_Body_value);
                    tmp = Newtonsoft.Json.JsonConvert.DeserializeObject<CreateNewEntityItemRequest>(t.ToString());
                    var EntityBody = (JsonConvert.DeserializeObject<EntityClass>(JsonConvert.SerializeObject(tmp.entityTypeSchema).ToString()));
                    EntityBody.EntityID = record?.Result?.TransactionType == "M" ? Convert.ToInt32(record?.Result?.ID) : insertedRecordid;

                    insertedRecordid = record?.Result?.TransactionType == "M" ? Convert.ToInt32(record?.Result?.ID) : insertedRecordid;

                    CasesSyncAPIMethods.SaveViewJsonSqlite(EntityBody, EntityItemView, _DBPath, Convert.ToString(_TypeID), Convert.ToString(EntityBody.EntityID), Convert.ToInt32(pkId), EntityBody.EntityTypeName, null, (EntityInstance));

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return insertedRecordid;
        }
        #endregion

        #region Create Entity From DB
        public static async Task<int> CreateEntityFromDB(string _DBPath)
        {
            int insertedRecordid = 0;
            try
            {
                var Records = await DBHelper.GetItemTranInfoListList(_DBPath);
                var RecordList = Records.Where(v => v.PROCESS_ID == 0 && v.METHOD == Constants.CreateNewEntityItem);
                foreach (var item in RecordList)
                {
                    try
                    {
                        var _Body_value = JsonConvert.DeserializeObject<CreateNewEntityItemRequest>(item.ITEM_TRAN_INFO);
                        var Result = EntityAPIMethods.CreateNewEntityItem(_Body_value);
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

        //it get and store Details of Entity Type Details for Entity Details page
        public static async void GetMasterEntityCount(string UserName, string _DBPath)
        {
            List<Entity_categoryList> EntityList = new List<Entity_categoryList>();
            var lstResult = await DBHelper.GetAppTypeInfoListBySystemName(EntityInstance, ConstantsSync.Entity_Category_TypeDetails, _DBPath);

            foreach (var item in lstResult.Select(o => new { o.CategoryId, o.CategoryName }).Distinct().ToList())
            {
                try
                {
                    if (item.CategoryId > 0)
                    {
                        var temp = EntityAPIMethods.GetEntityTypeList(item.CategoryId.ToString() ?? null, UserName).GetValue("ResponseContent");
                        if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                        {
                            var EntitysList = JsonConvert.DeserializeObject<List<EntityOrgCenterList>>(temp.ToString());

                            Task<List<AppTypeInfoList>> record = DBHelper.GetAppTypeInfoListBySystemName(ConstantsSync.EntityInstance, EntityType_CountDetails, _DBPath);
                            record.Wait();

                            var ischeck = record.Result.Where(v => v.SYSTEM == EntityInstance && v.TYPE_SCREEN_INFO == EntityType_CountDetails && v.TYPE_ID == Convert.ToInt32(EntitysList[0].EntityTypeID) && v.CategoryId == Convert.ToInt32(item.CategoryId ?? default(int)) && v.TYPE_NAME == EntitysList[0].EntityTypeName).FirstOrDefault();

                            if (ischeck == null)
                            {
                                if (EntitysList.Count > 0)
                                {
                                    AppTypeInfoList AppTypeInfo = new AppTypeInfoList()
                                    {
                                        APP_TYPE_INFO_ID = 0,
                                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                                        LAST_SYNC_DATETIME = DateTime.Now,
                                        SYSTEM = "ENTITY",
                                        TYPE_ID = Convert.ToInt32(EntitysList[0].EntityTypeID),
                                        TYPE_NAME = EntitysList[0].EntityTypeName,
                                        ASSOC_FIELD_INFO = JsonConvert.SerializeObject(EntitysList),
                                        CategoryId = Convert.ToInt32(item.CategoryId),
                                        CategoryName = item.CategoryName,
                                        TYPE_SCREEN_INFO = EntityType_CountDetails,
                                        TransactionType = "M",
                                        IS_ONLINE = true
                                    };
                                    Task<int> inserted = DBHelper.SaveAppTypeInfo(AppTypeInfo, _DBPath);
                                    inserted.Wait();
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        #region GetEntityListSync
        public async static Task<List<BoxerCentralHomePage_EntityList_Mob>> GetEntityAssociationList(bool _IsOnline, string username, string _DBPath)
        {
            List<BoxerCentralHomePage_EntityList_Mob> Result = new List<BoxerCentralHomePage_EntityList_Mob>();
            try
            {
                //if (!_IsOnline)
                //{
                //    var JResult = EntityAPIMethods.BoxerCentralHome_Entities_GetEntityList(username, null);
                //    string jsonValue = Convert.ToString(JResult.GetValue("ResponseContent"));
                //    Result = new List<BoxerCentralHomePage_EntityList_Mob>();
                //    Result = JsonConvert.DeserializeObject<List<BoxerCentralHomePage_EntityList_Mob>>(jsonValue);
                //    int TypeID = 0;

                //    AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();

                //    _AppTypeInfoList.APP_TYPE_INFO_ID = 0;
                //    _AppTypeInfoList.ASSOC_FIELD_INFO = jsonValue;
                //    _AppTypeInfoList.LAST_SYNC_DATETIME = DateTime.Now;
                //    _AppTypeInfoList.SYSTEM = ConstantsSync.EntityInstance;
                //    _AppTypeInfoList.TYPE_ID = TypeID;
                //    _AppTypeInfoList.TYPE_NAME = "";
                //    _AppTypeInfoList.CategoryId = 0;
                //    _AppTypeInfoList.CategoryName = "";
                //    _AppTypeInfoList.TransactionType = "M";
                //    _AppTypeInfoList.TYPE_SCREEN_INFO = MyEntityAssociation;
                //    _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                //    _AppTypeInfoList.IS_ONLINE = _IsOnline;

                //    Task<int> i = DBHelper.UpdateAppTypeInfoList(_AppTypeInfoList, _DBPath);
                //    i.Wait();
                //}
                //else
                {
                    Task<List<AppTypeInfoList>> AppTypeInforesult = DBHelper.GetAppTypeInfoListBySystemName(ConstantsSync.EntityInstance, EntityItemView, _DBPath);
                    AppTypeInforesult.Wait();
                    if (AppTypeInforesult.Result != null)
                    {

                        foreach (var item in AppTypeInforesult.Result)
                        {
                            var it = JsonConvert.DeserializeObject<EntityClass>(item.ASSOC_FIELD_INFO);
                            var tit = it.AssociationFieldCollection.Where(v => v.AssocSystemCode?.ToLower() == "title").ToList()?.Select(d => d.AssocMetaDataText.Count > 0 ? d.AssocMetaDataText[0]?.TextValue : "NO TITLE PROVIDED")?.FirstOrDefault();

                            Result.Add(
                                new BoxerCentralHomePage_EntityList_Mob
                                {
                                    ENTITY_ID = Convert.ToString(it.EntityID),
                                    ENTITY_TYPE_ID = Convert.ToString(it.EntityTypeID),
                                    ENTITY_TITLE = tit,
                                    ENTITY_TYPE_NAME = it.EntityTypeName,
                                    LIST_ID = "List Id: " + Convert.ToString(it.ListID),
                                    MODIFIED_BY = "Modified By: " + it.EntityModifiedByFullName,
                                    MODIFIED_DATETIME = "Modified On: " + Convert.ToDateTime(it.EntityModifiedDateTime).ToString("MM/dd/yyyy"),
                                    CREATED_BY = "Created By: " + it.EntityCreatedByFullName,
                                    CREATED_DATETIME = "Created On: " + Convert.ToDateTime(it.EntityCreatedDateTime).ToString("MM/dd/yyyy"),
                                });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Result;
        }
        #endregion

        #region Get All Entity Role Relationship By Emp
        public async static Task<List<AllEntityRoleRelationshipByEmp>> GetAllEntityRoleRelationshipByEmp(bool _IsOnline, string _pEMPLOYEE_SAM, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<AllEntityRoleRelationshipByEmp> lstResult = new List<AllEntityRoleRelationshipByEmp>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.EntityInstance, "K3_AllEntityRoleRelationshipByEmp", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = EntityAPIMethods.GetAllEntityRoleRelationshipByEmp(_pEMPLOYEE_SAM, _UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<AllEntityRoleRelationshipByEmp>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), EntityInstance, "K3_AllEntityRoleRelationshipByEmp", _InstanceUserAssocId, _DBPath, id, "", "M", "", 0, "", "", _IsOnline);
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<AllEntityRoleRelationshipByEmp>(CasesInstance, "K3_AllEntityRoleRelationshipByEmp", _DBPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lstResult;
        }
        #endregion

        #region Ribbon Operation Service Bus

        #region Take Owner Ship
        public static bool TakeOwnership(bool _IsOnline, string _UserName, int _EntityID, int _TypeID, string _DBPath)
        {
            try
            {
                TakeOwnershipRequest TO = new TakeOwnershipRequest();
                TO.ENTITY_ID = _EntityID;
                TO.USER = _UserName;
                if (_IsOnline)
                {
                    var result = EntityAPIMethods.TakeOwnership(TO);
                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        return true;
                    }
                }
                else
                {
                    TakeOwnershipRequest TOwner = new TakeOwnershipRequest();
                    TOwner.ENTITY_ID = _EntityID;
                    TOwner.USER = _UserName;
                    var list = DBHelper.GetAppTypeInfoList(EntityInstance, _EntityID, _TypeID, EntityItemView, _DBPath, null);
                    list.Wait();

                    var insertedRecordid = CommonConstants.AddofflineTranInfo(_TypeID, TOwner, Constants.TakeOwnership, Enum.GetNames(typeof(ActionTypes))[10], 0, _EntityID, _DBPath, EntityInstance, list.Result.APP_TYPE_INFO_ID, "C", TakeOwnership_G11);
                    insertedRecordid.Wait();
                    if (insertedRecordid.Result == 0)
                        return false;
                    else
                        return true;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region Forward Item
        public static bool ForwardEntityItem(bool _IsOnline, string _UserName, string AssignedTouser, int _EntityID, int _TypeID, string _DBPath)
        {
            try
            {
                ForwardEntityRequest Body_value = new ForwardEntityRequest();
                Body_value.EntityID = _EntityID;
                Body_value.FORWARD_TO_USER = AssignedTouser;
                Body_value.FORWARD_BY_USER = _UserName;

                if (_IsOnline)
                {
                    var result = EntityAPIMethods.ForwardEntity(Body_value);
                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        return true;
                    }
                }
                else
                {
                    var list = DBHelper.GetAppTypeInfoList(EntityInstance, _EntityID, _TypeID, EntityItemView, _DBPath, null);
                    list.Wait();

                    var insertedRecordid = CommonConstants.AddofflineTranInfo(_TypeID, Body_value, Constants.ForwardEntity, Enum.GetNames(typeof(ActionTypes))[21], 0, _EntityID, _DBPath, EntityInstance, list.Result.APP_TYPE_INFO_ID, "C", EntityItemForward_G11);
                    insertedRecordid.Wait();
                    if (insertedRecordid.Result == 0)
                        return false;
                    else
                        return true;
                }

                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

        #region Assign Entity Item
        public static bool AssignEntityItem(bool _IsOnline, string _UserName, string AssignedTouser, int _EntityID, int _TypeID, string _DBPath)
        {
            try
            {
                AssignEntityRequest Body_value = new AssignEntityRequest();
                Body_value.EntityID = _EntityID;
                Body_value.AssignTo = AssignedTouser;
                Body_value.AssignBy = _UserName;

                if (_IsOnline)
                {
                    var result = EntityAPIMethods.AssignEntity(Body_value);
                    var temp = result.GetValue("ResponseContent");

                    if (!string.IsNullOrEmpty(temp?.ToString()) && temp.ToString() != "[]")
                    {
                        return true;
                    }
                }
                else
                {
                    var list = DBHelper.GetAppTypeInfoList(EntityInstance, _EntityID, _TypeID, EntityItemView, _DBPath, null);
                    list?.Wait();

                    var insertedRecordid = CommonConstants.AddofflineTranInfo(_TypeID, Body_value, Constants.AssignEntity, Enum.GetNames(typeof(ActionTypes))[20], 0, _EntityID, _DBPath, EntityInstance, list.Result.APP_TYPE_INFO_ID, "C", EntityItemAssign_G11);
                    insertedRecordid.Wait();
                    if (insertedRecordid.Result == 0)
                        return false;
                    else
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        #endregion

        #endregion

        #region #8 Entity Delete Data
        public async static Task<bool> DeleteEntityItem(bool _IsOnline, int EntityID, int EntityTypeID, string username, string _DBPath)
        {
            DeleteEntityItemRequest DelReq = new DeleteEntityItemRequest
            {
                ENTITY_ID = EntityID,
                DELETED_BY = username
            };
            try
            {
                if (_IsOnline)
                {
                    var result = EntityAPIMethods.DeleteEntityItem(DelReq);

                    if (!string.IsNullOrEmpty(result?.ToString()) && result.ToString() != "[]")
                    {// 22
                        return true;
                    }
                }
                else
                {
                    var Listt = DBHelper.GetItemTranInfoListList(_DBPath);
                    Listt.Wait();
                    var t = Listt.Result.Where(v => v.REF_ITEM_TRAN_INFO_ID == EntityID).ToList();
                    foreach (var item in t)
                    {
                        DBHelper.DeleteItemTranInfoList(item, _DBPath).Wait();
                    }

                    var SingleEntityItem = DBHelper.GetAllAppTypeInfoList(_DBPath);
                    SingleEntityItem?.Wait();

                    List<AppTypeInfoList> tmp = SingleEntityItem.Result.Where(v => v.SYSTEM == EntityInstance && v.ID == EntityID && v.TYPE_ID == EntityTypeID && v.TYPE_SCREEN_INFO == EntityItemView).ToList();

                    foreach (var item in tmp)
                    {
                        DBHelper.DeleteAppTypeInfoListById(item, _DBPath).Wait();
                    }
                    List<AppTypeInfoList> SymList = new List<AppTypeInfoList>();
                    var FullJson = SingleEntityItem.Result.Where(v => v.TYPE_SCREEN_INFO != null && v.TYPE_ID != null && v.TYPE_SCREEN_INFO.Contains(EntityItemList_Lazyload) && v.TYPE_ID == EntityTypeID).ToList();

                    List<EntityClass> ListEntity = new List<EntityClass>();
                    foreach (var item in FullJson)
                    {
                        ListEntity = JsonConvert.DeserializeObject<List<EntityClass>>(item.ASSOC_FIELD_INFO);
                        ListEntity.Remove(ListEntity.Where(v => v.EntityID == EntityID).FirstOrDefault());
                        item.ASSOC_FIELD_INFO = ListEntity.Count > 0 ? JsonConvert.SerializeObject(ListEntity) : "";
                        SymList.Add(item);
                    }

                    foreach (var item in SymList)
                    {
                        if (!string.IsNullOrEmpty(item.ASSOC_FIELD_INFO))
                        {
                            Task<int> inserted = DBHelper.UpdateAppTypeInfoList(item, _DBPath);
                            inserted.Wait();
                        }
                        else
                        {
                            Task<int> inserted = DBHelper.DeleteAppTypeInfoListById(item, _DBPath);
                            inserted.Wait();
                        }
                    }

                    int id = CommonConstants.GetResultBySytemcodeId(EntityInstance, "G1_G2_Entity_Cate_TypeDetails", EntityTypeID, _DBPath);

                    await CommonConstants.AddofflineTranInfo(EntityTypeID, DelReq, Constants.DeleteEntityItem, (Enum.GetNames(typeof(ActionTypes)))[22], 0, EntityID, _DBPath, EntityInstance, id, "C");
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        #endregion

        #region B1_Screen GetEntityByAssingToSAM
        public static async Task<List<AssignToUser>> GetEntityByAssingToSAM(bool _IsOnline, string _UserName, int _InstanceUserAssocId, string _DBPath)
        {
            List<AssignToUser> lstResult = new List<AssignToUser>();
            int id = CommonConstants.GetResultBySytemcode(ConstantsSync.EntityInstance, "B1_GetEntityByAssingToSAM", _DBPath);

            try
            {
                if (_IsOnline)
                {
                    var result = EntityAPIMethods.GetEntityByAssingToSAM(_UserName);
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null && temp.ToString() != "[]")
                    {
                        lstResult = JsonConvert.DeserializeObject<List<AssignToUser>>(temp.ToString());
                        if (lstResult.Count > 0)
                        {
                            var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), ConstantsSync.EntityInstance, "B1_GetEntityByAssingToSAM", _InstanceUserAssocId, _DBPath, id, "", "M", "", 0, "", "", _IsOnline);
                        }
                    }
                }
                else
                {
                    lstResult = CommonConstants.ReturnListResult<AssignToUser>(ConstantsSync.EntityInstance, "B1_GetEntityByAssingToSAM", _DBPath);
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