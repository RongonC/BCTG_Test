using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using SQLite;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Common
{
    public class CommonConstants
    {
        public static int cnt = 0;
        #region Master Offline Store
        public static string MasterOfflineStore(string Res, string _DBPath)
        {
            Stopwatch sp = new Stopwatch();

            sp.Start();
            string serror = string.Empty;
            try
            {
                GetAllCaseType casedata = JsonConvert.DeserializeObject<GetAllCaseType>(Res);
                AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();
                EDSResultList EDSResultList = new EDSResultList();
                List<KeyValuePair<int, int?>> dc = new List<KeyValuePair<int, int?>>();
                if (casedata != null)
                {

                    try
                    {
                        casedata.AppTypeInfo = casedata.AppTypeInfo.Select(i =>
                                    {
                                        i.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                                        return i;
                                    }).ToList();
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        casedata.EDSResult = casedata.EDSResult.Select(i =>
                               {
                                   i.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                                   return i;
                               }).ToList();

                    }
                    catch (Exception)
                    {
                    }

                    List<AppTypeInfoList> _apptypeinfolist = casedata.AppTypeInfo.Select(i => new AppTypeInfoList()
                    {
                        ASSOC_FIELD_INFO = i.ASSOC_FIELD_INFO,
                        LAST_SYNC_DATETIME = DateTime.Now,
                        SYSTEM = i.SYSTEM,
                        TYPE_ID = i.TYPE_ID,
                        ID = i.ID,
                        TYPE_NAME = i.TYPE_NAME,
                        CategoryId = i.CategoryId,
                        CategoryName = i.CategoryName,
                        TransactionType = "M",
                        TYPE_SCREEN_INFO = i.TYPE_SCREEN_INFO,
                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                        IS_ONLINE = true,
                        TM_Username = i.TM_Username,
                    }).ToList();

                    if (_apptypeinfolist.Count() > 0)
                    {
                        SQLiteAsyncConnection database = new SQLiteAsyncConnection(_DBPath);
                        database.InsertAllAsync(_apptypeinfolist).Wait();
                    }
                    //117----- 335 ms

                    dc = _apptypeinfolist.AsEnumerable().Select(item => new KeyValuePair<int, int?>(item.APP_TYPE_INFO_ID, item.TYPE_ID)).ToList();

                    List<EDSResultList> Edslst = casedata.EDSResult.Select(i => new EDSResultList()
                    {
                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                        APP_TYPE_INFO_ID = dc.Where(v => v.Value == Int32.Parse(i.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[1])).FirstOrDefault().Key,
                        ASSOC_FIELD_ID = Convert.ToInt32(i.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[0]),
                        EDS_RESULT = i.EDS_RESULT,
                        LAST_SYNC_DATETIME = DateTime.Now
                    }).ToList();

                    if (Edslst.Count() > 0)
                    {
                        SQLiteAsyncConnection data = new SQLiteAsyncConnection(_DBPath);
                        data.InsertAllAsync(Edslst).Wait();
                    }

                    //foreach (var item in casedata.EDSResult)
                    //{
                    //    try
                    //    {
                    //        var kvp = dc.Where(v => v.Value == Int32.Parse(item.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[1])).FirstOrDefault();

                    //        var assoc_field_id = Int32.Parse(item.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[0]);

                    //        EDSResultList.EDS_RESULT_ID = 0;
                    //        EDSResultList.APP_TYPE_INFO_ID = kvp.Key;
                    //        EDSResultList.LAST_SYNC_DATETIME = DateTime.Now;
                    //        EDSResultList.ASSOC_FIELD_ID = assoc_field_id;
                    //        EDSResultList.EDS_RESULT = item.EDS_RESULT;

                    //        EDSResultList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    //        Task<int> inserted = DBHelper.SaveEDSResult(EDSResultList, _DBPath);
                    //        inserted.Wait();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        serror = ex.Message;
                    //    }
                    //}

                    #region Old Logic
                    //foreach (var item in casedata.AppTypeInfo)
                    //{
                    //    try
                    //    {
                    //        Task<List<AppTypeInfoList>> record = DBHelper.GetAllAppTypeInfoList(_DBPath);
                    //        record.Wait();

                    //        var ischeck = record.Result.Where(v => v.SYSTEM == item.SYSTEM && v.TYPE_ID == item.TYPE_ID && v.ID == item.ID && /*v.TYPE_NAME == item.TYPE_NAME &&*/ v.INSTANCE_USER_ASSOC_ID == item.INSTANCE_USER_ASSOC_ID && v.TYPE_SCREEN_INFO == item.TYPE_SCREEN_INFO).FirstOrDefault();

                    //        if (ischeck == null)
                    //        {
                    //            _AppTypeInfoList.APP_TYPE_INFO_ID = 0;
                    //            _AppTypeInfoList.ASSOC_FIELD_INFO = item.ASSOC_FIELD_INFO;
                    //            _AppTypeInfoList.LAST_SYNC_DATETIME = DateTime.Now;
                    //            _AppTypeInfoList.SYSTEM = item.SYSTEM;
                    //            _AppTypeInfoList.TYPE_ID = item.TYPE_ID;
                    //            _AppTypeInfoList.ID = item.ID;
                    //            _AppTypeInfoList.TYPE_NAME = item.TYPE_NAME;
                    //            _AppTypeInfoList.CategoryId = item.CategoryId;
                    //            _AppTypeInfoList.CategoryName = item.CategoryName;
                    //            _AppTypeInfoList.TransactionType = "M";
                    //            _AppTypeInfoList.TYPE_SCREEN_INFO = item.TYPE_SCREEN_INFO;
                    //            _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    //            _AppTypeInfoList.IS_ONLINE = true;
                    //            _AppTypeInfoList.TM_Username = item.TM_Username;

                    //            Task<int> inserted = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, _DBPath);
                    //            inserted.Wait();
                    //            dc.Add(new KeyValuePair<int, int>(inserted.Result, item.TYPE_ID));
                    //        }
                    //        else
                    //        {
                    //            _AppTypeInfoList.LAST_SYNC_DATETIME = ischeck.LAST_SYNC_DATETIME;
                    //            _AppTypeInfoList.SYSTEM = ischeck.SYSTEM;
                    //            _AppTypeInfoList.APP_TYPE_INFO_ID = ischeck.APP_TYPE_INFO_ID;
                    //            _AppTypeInfoList.TYPE_ID = ischeck.TYPE_ID;
                    //            _AppTypeInfoList.ID = ischeck.ID;
                    //            _AppTypeInfoList.TYPE_NAME = ischeck.TYPE_NAME;
                    //            _AppTypeInfoList.CategoryId = ischeck.CategoryId;
                    //            _AppTypeInfoList.CategoryName = ischeck.CategoryName;
                    //            _AppTypeInfoList.TransactionType = "M";
                    //            _AppTypeInfoList.TYPE_SCREEN_INFO = ischeck.TYPE_SCREEN_INFO;
                    //            _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    //            _AppTypeInfoList.IS_ONLINE = true;
                    //            _AppTypeInfoList.TM_Username = item.TM_Username;
                    //            _AppTypeInfoList.ASSOC_FIELD_INFO = item.ASSOC_FIELD_INFO;
                    //            Task<int> inserted = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, _DBPath);
                    //            inserted.Wait();
                    //            dc.Add(new KeyValuePair<int, int>(ischeck.APP_TYPE_INFO_ID, item.TYPE_ID));
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        serror = ex.Message;
                    //    }
                    //}

                    //foreach (var item in casedata.EDSResult)
                    //{
                    //    try
                    //    {
                    //        if (dc.Count > 0)
                    //        {
                    //            var kvp = dc.Where(v => v.Value == Int32.Parse(item.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[1])).FirstOrDefault();
                    //            Task<List<EDSResultList>> record = DBHelper.GetEDSResultList(_DBPath);
                    //            record.Wait();

                    //            var ischeck = record.Result.Where(v => v.APP_TYPE_INFO_ID == kvp.Key && v.ASSOC_FIELD_ID == Int32.Parse(item.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[0]) && v.INSTANCE_USER_ASSOC_ID == item.INSTANCE_USER_ASSOC_ID).FirstOrDefault();

                    //            if (ischeck == null)
                    //            {
                    //                EDSResultList.EDS_RESULT_ID = 0;
                    //                EDSResultList.APP_TYPE_INFO_ID = kvp.Key;
                    //                EDSResultList.LAST_SYNC_DATETIME = DateTime.Now;
                    //                EDSResultList.ASSOC_FIELD_ID = Int32.Parse(item.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[0]);
                    //                EDSResultList.EDS_RESULT = item.EDS_RESULT;
                    //                EDSResultList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    //                Task<int> inserted = DBHelper.SaveEDSResult(EDSResultList, _DBPath);
                    //                inserted.Wait();
                    //            }
                    //            else
                    //            {
                    //                EDSResultList = ischeck;
                    //                EDSResultList.EDS_RESULT = item.EDS_RESULT;
                    //                Task<int> inserted = DBHelper.SaveEDSResult(EDSResultList, _DBPath);
                    //                inserted.Wait();
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        serror = ex.Message;
                    //    }
                    //} 
                    #endregion
                }
                sp.Stop();
                Debug.WriteLine("Sync Count => " + (cnt++) + Environment.NewLine + "Time taken => " + sp.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                serror = ex.Message;
            }
            return serror;
        }

        public static string MasterOfflineStore_withEDSTable(string Res, string _DBPath)
        {
            Stopwatch sp = new Stopwatch();

            sp.Start();
            string serror = string.Empty;
            try
            {
                GetAllEntityType Jobj = JsonConvert.DeserializeObject<GetAllEntityType>(Res);
                AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();
                EDSResultList EDSResultList = new EDSResultList();
                External_DSCache X_DSCacheList = new External_DSCache();
                List<KeyValuePair<int, int?>> dc = new List<KeyValuePair<int, int?>>();
                if (Jobj != null)
                {
                    try
                    {
                        Jobj.AppTypeInfo = Jobj.AppTypeInfo.Select(i =>
                        {
                            i.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                            return i;
                        }).ToList();
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        Jobj.EDSResult = Jobj.EDSResult.Select(i =>
                        {
                            i.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                            return i;
                        }).ToList();

                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        Jobj.XDSCache = Jobj.XDSCache.Select(i =>
                        {
                            i.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                            return i;
                        }).ToList();

                    }
                    catch (Exception)
                    {
                    }

                    List<AppTypeInfoList> _apptypeinfolist = Jobj.AppTypeInfo.Select(i => new AppTypeInfoList()
                    {
                        ASSOC_FIELD_INFO = i.ASSOC_FIELD_INFO,
                        LAST_SYNC_DATETIME = DateTime.Now,
                        SYSTEM = i.SYSTEM,
                        TYPE_ID = i.TYPE_ID,
                        ID = i.ID,
                        TYPE_NAME = i.TYPE_NAME,
                        CategoryId = i.CategoryId,
                        CategoryName = i.CategoryName,
                        TransactionType = "M",
                        TYPE_SCREEN_INFO = i.TYPE_SCREEN_INFO,
                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                        IS_ONLINE = true,
                        TM_Username = i.TM_Username,
                    }).ToList();

                    SQLiteAsyncConnection database = new SQLiteAsyncConnection(_DBPath);
                    database.InsertAllAsync(_apptypeinfolist).Wait();
                    //117----- 335 ms

                    dc = _apptypeinfolist.AsEnumerable().Select(item => new KeyValuePair<int, int?>(item.APP_TYPE_INFO_ID, item.TYPE_ID)).ToList();

                    List<EDSResultList> Edslst = Jobj.EDSResult.Select(i => new EDSResultList()
                    {
                        APP_TYPE_INFO_ID = dc.Where(v => v.Value == Int32.Parse(i.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[1])).FirstOrDefault().Key,
                        EDS_RESULT_ID = 0,
                        LAST_SYNC_DATETIME = DateTime.Now,
                        ASSOC_FIELD_ID = Convert.ToInt32(i.ASSOC_FIELD_ID?.Split(new string[] { "|||" }, StringSplitOptions.None)[0]),
                        EDS_RESULT = i.EDS_RESULT,
                        INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID,
                    }).ToList();

                    SQLiteAsyncConnection data = new SQLiteAsyncConnection(_DBPath);
                    data.InsertAllAsync(Edslst).Wait();

                    // 10960ms - 715

                    foreach (var item in Jobj.XDSCache)
                    {
                        var ischeck = DBHelper.GetXDSDetails(item.SYSTEM, item.EXT_DATASOURCE_ID, _DBPath);
                        ischeck.Wait();
                        //var ischeck = record.Result.Where(v => v.SYSTEM == item.SYSTEM && v.EXT_DATASOURCE_ID == item.EXT_DATASOURCE_ID && v.INSTANCE_USER_ASSOC_ID == item.INSTANCE_USER_ASSOC_ID).FirstOrDefault();

                        if (ischeck.Result == null)
                        {
                            X_DSCacheList.EDS_CACHE_ID = 0;
                            X_DSCacheList.EDS_VALUES = item.EDS_VALUES;
                            X_DSCacheList.LAST_MODIFIED_DATETIME = DateTime.Now;
                            X_DSCacheList.SYSTEM = item.SYSTEM;
                            X_DSCacheList.EXT_DATASOURCE_ID = item.EXT_DATASOURCE_ID;
                            X_DSCacheList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;

                            Task<int> inserted = DBHelper.SaveXDSDetails(X_DSCacheList, _DBPath);
                            inserted.Wait();
                        }
                        else
                        {
                            X_DSCacheList.EDS_CACHE_ID = ischeck.Result.EDS_CACHE_ID;
                            X_DSCacheList.EDS_VALUES = item.EDS_VALUES;
                            X_DSCacheList.LAST_MODIFIED_DATETIME = DateTime.Now;
                            X_DSCacheList.SYSTEM = item.SYSTEM;
                            X_DSCacheList.EXT_DATASOURCE_ID = item.EXT_DATASOURCE_ID;
                            X_DSCacheList.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;

                            Task<int> inserted = DBHelper.SaveXDSDetails(X_DSCacheList, _DBPath);
                            inserted.Wait();
                        }
                    }
                    //5581
                    //4837
                }
                sp.Stop();
                Debug.WriteLine("Sync Count => " + (cnt++) + Environment.NewLine + "Time taken => " + sp.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                serror = ex.Message;
            }
            return serror;
        }



        #endregion

        #region Add Favorite
        public static async Task<int> FavoriteOfflineStore(int FavoriteID, string _TypeId, string _QuestAreaId, string _FavoriteName, string _FieldValues, string _IsActive, string _CreatedBy, string _CreatedByDt, string _ModifiedByDt, string _ApplicationId, string _LastSyncDt, string _InstanceUserAssocId, string _DBPath)
        {
            FavoriteList favorite = new FavoriteList();
            favorite.FAVORITE_ID = FavoriteID;
            favorite.FAVORITE_NAME = _FavoriteName;
            favorite.FIELD_VALUES = _FieldValues;
            //favorite.APP_TYPE_INFO_ID = Convert.ToInt32(_AppTypeInfoId);
            favorite.CREATED_BY = _CreatedBy;
            favorite.CREATED_DATETIME = Convert.ToDateTime(_CreatedByDt);
            favorite.INSTANCE_USER_ASSOC_ID = Convert.ToInt32(_InstanceUserAssocId);
            favorite.IS_ACTIVE = _IsActive;
            favorite.MODIFY_DATETIME = Convert.ToDateTime(_ModifiedByDt);
            favorite.APPLICATION_ID = int.Parse(_ApplicationId);
            favorite.TYPE_ID = int.Parse(_TypeId);
            favorite.QUEST_AREA_ID = string.IsNullOrEmpty(_QuestAreaId) ? 0 : int.Parse(_QuestAreaId);
            Task<int> inserted = DBHelper.SaveFavorite(favorite, _DBPath);
            inserted.Wait();
            return inserted.Result;
        }
        #endregion

        #region Add Favorite
        public static async Task<int> RemoveFavoriteOfflineStore(string _systemName, int _favoriteID, string _screenName, string _DBPath)
        {
            FavoriteList favorite = new FavoriteList();
            favorite.FAVORITE_ID = _favoriteID;
            Task<int> insertedFav = DBHelper.RemoveFavorite(favorite, _DBPath);
            insertedFav.Wait();
            var GetappinfoList = DBHelper.GetAppTypeInfoListScreenIDSystem(_systemName, _favoriteID, _screenName, _DBPath);
            GetappinfoList.Wait();
            Task<int> insertedAppinfo = DBHelper.DeleteAppTypeInfoListById(GetappinfoList.Result, _DBPath);
            insertedAppinfo.Wait();
            return insertedFav.Result;
        }
        #endregion

        #region Add Record Offline Store
        public static async Task<int> AddRecordOfflineStore_AppTypeInfo(string _JsonSerealized, string _InstanceName, string TypeScreenInfo, int _InstanceUserAssocId, string _DBPath, int _AppTypeInfoID, string _TypeId, string _TransactionType, string _CaseId = "", int? _CategoryId = 0, string _TypeName = "", string _CategoryName = "", bool IsonlineRecord = false, string tm_uname = null)
        {
            Task<int> inserted = null;
            AppTypeInfoList _AppTypeInfoList = new AppTypeInfoList();
            _AppTypeInfoList.APP_TYPE_INFO_ID = _AppTypeInfoID;
            _AppTypeInfoList.ASSOC_FIELD_INFO = _JsonSerealized;
            _AppTypeInfoList.LAST_SYNC_DATETIME = DateTime.Now;
            _AppTypeInfoList.SYSTEM = _InstanceName;
            _AppTypeInfoList.TYPE_ID = string.IsNullOrEmpty(_TypeId) ? 0 : int.Parse(_TypeId);
            _AppTypeInfoList.TYPE_NAME = _TypeName;
            _AppTypeInfoList.CategoryId = _CategoryId;
            _AppTypeInfoList.CategoryName = _CategoryName;
            _AppTypeInfoList.TYPE_SCREEN_INFO = TypeScreenInfo;
            _AppTypeInfoList.INSTANCE_USER_ASSOC_ID = _InstanceUserAssocId;
            _AppTypeInfoList.TransactionType = _TransactionType;
            _AppTypeInfoList.ID = string.IsNullOrEmpty(_CaseId) ? 0 : int.Parse(_CaseId);
            _AppTypeInfoList.IS_ONLINE = IsonlineRecord;

            try
            {
                _AppTypeInfoList.TM_Username = tm_uname;//?? DBHelper.GetAppTypeInfo_tmname(ConstantsSync.CasesInstance, Convert.ToInt32(_AppTypeInfoList.ID), Convert.ToInt32(_AppTypeInfoList.TYPE_ID), TypeScreenInfo, _DBPath).Result?.TM_Username;
            }
            catch (Exception)
            {

            }

            inserted = DBHelper.SaveAppTypeInfo(_AppTypeInfoList, _DBPath);
            inserted.Wait();

            return inserted.Result;
        }
        #endregion

        #region Return Result
        public static List<T> ReturnListResult<T>(string ApplicationeName, string TypeScreenInfo, string _DBPath)
        {
            List<T> result = default(List<T>);
            var GetResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(ApplicationeName, TypeScreenInfo, _DBPath);
            GetResult.Wait();
            result = GetResult.Result?.ASSOC_FIELD_INFO == null ? new List<T>() : JsonConvert.DeserializeObject<List<T>>(GetResult.Result.ASSOC_FIELD_INFO.ToString());
            return result;
        }

        public static List<T> ReturnListResult<T>(string ApplicationeName, string TypeScreenInfo, string _DBPath, string typeID)
        {
            List<T> result = default(List<T>);
            var GetResult = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(ApplicationeName, TypeScreenInfo, Convert.ToInt32(typeID), _DBPath, null);
            GetResult.Wait();
            result = GetResult.Result?.ASSOC_FIELD_INFO == null ? null : JsonConvert.DeserializeObject<List<T>>(GetResult.Result.ASSOC_FIELD_INFO.ToString());
            return result;
        }

        public static T ReturnResult<T>(string ApplicationeName, string TypeScreenInfo, string _DBPath)
        {
            T result = default(T);
            var GetResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(ApplicationeName, TypeScreenInfo, _DBPath);
            GetResult.Wait();
            result = JsonConvert.DeserializeObject<T>(GetResult.Result.ASSOC_FIELD_INFO.ToString());
            return result;
        }
        #endregion

        #region Get Result By Sytemcode
        public static int GetResultBySytemcode(string _Instance, string _Systemcode, string _DBPath)
        {
            int iResult = 0;
            var Result = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(_Instance, _Systemcode, _DBPath);
            Result.Wait();
            iResult = Result.Result == null ? 0 : Result.Result.APP_TYPE_INFO_ID;
            return iResult;
        }
        #endregion

        #region Get Result By Sytemcode ID
        public static int GetResultBySytemcodeId(string _Instance, string _Systemcode, int _CasetypeId, string _DBPath)
        {
            int iResult = 0;
            var Result = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(_Instance, _Systemcode, _CasetypeId, _DBPath, null);
            Result.Wait();
            iResult = Result.Result == null ? 0 : Result.Result.APP_TYPE_INFO_ID;
            return iResult;
        }
        #endregion

        #region Get Result By Sytemcode ID
        public static int GetResultBySytemcodetypeidId(string _Instance, string _Systemcode, int _CasetypeId, int sCatId, int id, string _DBPath)
        {
            int iResult = 0;
            var Result = DBHelper.GetAppTypeInfoListByCatIdTransTypeSyscodeID(_Instance, _CasetypeId, sCatId, _DBPath, _Systemcode, "M", id);
            Result.Wait();
            iResult = Result.Result == null ? 0 : Convert.ToInt32(Result.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);
            return iResult;
        }
        #endregion

        #region Get Result By Sytemcode List
        public static Task<AppTypeInfoList> GetResultBySytemcodeList(string _Instance, string _Systemcode, string _DBPath)
        {
            Task<AppTypeInfoList> iResult = DBHelper.GetAppTypeInfoListByNameTypeScreenInfo(_Instance, _Systemcode, _DBPath);
            iResult.Wait();
            return iResult;
        }
        #endregion

        #region Refresh CalculationFields
        public static string RefreshCalculationFields(string assocFieldCollection, string calculatedAssocId, Dictionary<string, string> assocFieldTexts, Dictionary<string, string> assocFieldValues, string sdateFormats, string _DBPath)
        {
            string returnVal = string.Empty;
            if (string.IsNullOrEmpty(assocFieldCollection))
                throw new Exception("assocFieldCollection is empty");
            List<GetCaseTypesResponse.ItemType> casesAssocFieldCollection = null;
            try
            {
                casesAssocFieldCollection = JsonConvert.DeserializeObject<List<GetCaseTypesResponse.ItemType>>(assocFieldCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (casesAssocFieldCollection == null)
                throw new Exception("casesAssocFieldCollection is NULL");
            string FieldType = calculatedAssocId.Split('_')[1];
            string AssocTypeID = calculatedAssocId.Split('_')[2];
            try
            {
                var calculationItem = casesAssocFieldCollection.Where(x => x.AssocFieldType == Convert.ToChar(FieldType) && x.AssocTypeID == int.Parse(AssocTypeID)).FirstOrDefault();

                var formula = calculationItem?.CalculationFormula;
                string originalFormula = formula;
                if (formula != null)
                {
                    if (formula.IndexOf("{") > -1)
                    {
                        if (casesAssocFieldCollection != null)
                        {

                            //DataTypes.CommonMethods controller = new DataTypes.CommonMethods();
                            List<string> results = ExtractFromString(formula, "{", "}");

                            var systemCodes = casesAssocFieldCollection.Where(i => i.SystemCode != null).Select(x => x.SystemCode.ToLower()).ToArray();
                            var assocItemsWithExternalDatasource = casesAssocFieldCollection.Where(x => x.ExternalDataSourceID != null);

                            List<ExternalDatasourceInfo> lstexddatasource = new List<ExternalDatasourceInfo>();
                            List<string> lstexddatasourceName = new List<string>();
                            foreach (var item in assocItemsWithExternalDatasource)
                            {
                                var GetAppTypeInfo = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(ConstantsSync.CasesInstance, "C1_C2_CASES_CASETYPELIST", item.CaseTypeID, _DBPath, null);
                                GetAppTypeInfo.Wait();

                                Task<EDSResultList> exd = DBHelper.GetEDSResultListwithId(item.AssocTypeID, GetAppTypeInfo.Result.APP_TYPE_INFO_ID, _DBPath);
                                exd.Wait();
                                if (!string.IsNullOrEmpty(exd?.Result?.EDS_RESULT))
                                {
                                    List<GetExternalDataSourceByIdResponse.ExternalDatasource> lstexd = JsonConvert.DeserializeObject<List<GetExternalDataSourceByIdResponse.ExternalDatasource>>(exd?.Result?.EDS_RESULT.ToString());

                                    ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST Exdinfo = JsonConvert.DeserializeObject<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>(exd.Result.EDS_RESULT);
                                    ExternalDatasourceList.EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPE exdassoc = Exdinfo?.FREQUENT?.CASE_LIST?.CASE?.ASSOC_TYPE as ExternalDatasourceList.EXTERNAL_DATASOURCE_LISTFREQUENTCASE_LISTCASEASSOC_TYPE;
                                    foreach (var it in exdassoc?.EXTERNAL_DATASOURCE)
                                    {
                                        ExternalDatasourceInfo exdatasource = new ExternalDatasourceInfo();
                                        exdatasource.ConnectionString = it.CONNECTION_STRING;
                                        exdatasource.Description = it.EXTERNAL_DATASOURCE_DESCRIPTION;
                                        exdatasource.Entity_Type_ID = Convert.ToInt32(it.Entity_type_ID);
                                        exdatasource.Name = it.NAME;
                                        exdatasource.ObjectDisplay = it.OBJECT_DISPLAY;
                                        exdatasource.ObjectId = it.OBJECT_ID;
                                        exdatasource.Query = it.QUERY;
                                        exdatasource.SystemCode = Convert.ToString(it.SYSTEM_CODE);
                                        if (!string.IsNullOrEmpty(it.NAME))
                                            lstexddatasourceName.Add(it.NAME);
                                        lstexddatasource.Add(exdatasource);
                                    }

                                }
                            }
                            lstexddatasourceName = lstexddatasourceName.Distinct().ToList();
                            lstexddatasource = lstexddatasource.Distinct().ToList();

                            foreach (string result in results)
                            {
                                int position = result.IndexOf(".");
                                bool formulaHasExDs = true; // assume first part of formula (i.e. part before ".") is "EXDS_NAME" or "SYSTEM_CODE" of field with exds


                                string part1 = result;
                                if (position > -1) // look for exds
                                {
                                    part1 = result.Substring(0, position).Trim().ToLower();
                                    if (!lstexddatasourceName.Contains(part1)) // part1 is not a direct exds 
                                    {
                                        if (systemCodes.Contains(part1) && (assocItemsWithExternalDatasource == null || !assocItemsWithExternalDatasource.Where(i => i.SystemCode != null).Select(x => x.SystemCode.ToLower()).ToArray().Contains(part1)))
                                        {
                                            formulaHasExDs = false; // part1 may be system code of decode value
                                        }
                                    }
                                }
                                if (position > -1 && formulaHasExDs && lstexddatasourceName != null && lstexddatasource != null)
                                {
                                    string externalDataSourceName = "";
                                    string externalDataField = "";
                                    string externalDataObjectId = "";

                                    string part2 = result.Substring(position + 1).Trim().ToLower();

                                    string query = "";
                                    string conectionString = "";

                                    bool valueSet = false;

                                    GetCaseTypesResponse.ItemType exFiled = null;
                                    if (systemCodes.Contains(part1))
                                    {
                                        exFiled = assocItemsWithExternalDatasource.Where(x => x.SystemCode != null && x.SystemCode.ToLower() == part1).FirstOrDefault();
                                        if (exFiled != null)
                                        {
                                            var exd = lstexddatasource.Where(x => x.Entity_Type_ID == exFiled.ExternalDataSourceEntityTypeID).Select(x => x.Name);
                                            if (exd?.FirstOrDefault() != null)
                                            {
                                                externalDataSourceName = (exFiled.ExternalDataSourceID != null) ? exd.FirstOrDefault().ToString() : string.Empty;
                                                externalDataField = part2;
                                            }
                                        }
                                    }

                                    else if (lstexddatasourceName.Contains(part1))
                                    {
                                        var tempexFiled = assocItemsWithExternalDatasource.Where(x => x.SystemCode.ToLower() == part1).FirstOrDefault();
                                        var exd = lstexddatasource.Where(x => x.Entity_Type_ID == exFiled.ExternalDataSourceEntityTypeID).Select(x => x.Name);
                                        externalDataSourceName = part1; //or exFiled.ExternalDataSource?.DataSourceName;
                                        externalDataField = part2;
                                    }

                                    if (exFiled != null && lstexddatasource != null)
                                    {
                                        query = lstexddatasource.Where(x => x.Entity_Type_ID == exFiled.ExternalDataSourceEntityTypeID).Select(x => x.Query).FirstOrDefault();
                                        conectionString = lstexddatasource.Where(x => x.Entity_Type_ID == exFiled.ExternalDataSourceEntityTypeID).Select(x => x.ConnectionString).FirstOrDefault();
                                        externalDataObjectId = lstexddatasource.Where(x => x.Entity_Type_ID == exFiled.ExternalDataSourceEntityTypeID).Select(x => x.ObjectId).FirstOrDefault();
                                        externalDataField = lstexddatasource.Where(x => x.Entity_Type_ID == exFiled.ExternalDataSourceEntityTypeID).Select(x => x.ObjectDisplay).FirstOrDefault();
                                    }
                                    else
                                    {
                                        externalDataSourceName = part1; //or exFiled.ExternalDataSource?.DataSourceName;
                                        externalDataField = part2;
                                        var extExDs = lstexddatasource.Where(x => x.Name?.ToUpper() == externalDataSourceName?.ToUpper()).FirstOrDefault();//entitiesCommon.GetExternalDataSourceInfoByName(externalDataSourceName); //extended external datasource is part1
                                        if (extExDs != null)
                                        {
                                            query = extExDs.Query;
                                            conectionString = extExDs.ConnectionString;
                                            externalDataObjectId = extExDs.ObjectId;
                                        }
                                    }
                                    foreach (var item in assocItemsWithExternalDatasource)
                                    {

                                        string selectedValue = "";

                                        string evaluatedValue = string.Empty;
                                        int controlAssocTypeId = item.AssocTypeID;
                                        string selectedText = string.Empty;
                                        if (item.AssocFieldType == 'E' || item.AssocFieldType == 'O')
                                        {
                                            assocFieldValues.TryGetValue("assoc_" + item.AssocFieldType + "_" + controlAssocTypeId, out selectedValue);
                                            selectedText = assocFieldTexts.Where(a => a.Key == (assocFieldValues.Where(v => v.Value == selectedValue).FirstOrDefault().Key)).FirstOrDefault().Value;
                                        }
                                        if (item.SystemCode != null)
                                        {
                                            if (selectedValue != null && selectedValue != "-1" && selectedValue.Trim().Length > 0 &&
                                                        (part1 == item.SystemCode.ToLower() || (item.SystemCode != null && part1 == item.SystemCode.ToLower())
                                                        || (!lstexddatasourceName.Contains(part1) && !systemCodes.Contains(part1))))
                                            {
                                                try
                                                {
                                                    bool success;
                                                    if ((!lstexddatasourceName.Contains(part1) && !systemCodes.Contains(part1)))
                                                    {
                                                        query = GetQueryStringWithParamaters(query, externalDataSourceName, selectedValue.Split('|')[1], out success, item.Name, true);
                                                        if (success)
                                                            valueSet = true;
                                                    }
                                                    else
                                                    {
                                                        query = GetQueryStringWithParamaters(query, externalDataSourceName, selectedValue.Split('|')[1], out success, item.Name);
                                                        if (success)
                                                            valueSet = true;
                                                    }


                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }

                                        if (!valueSet && !string.IsNullOrEmpty(query))
                                        {

                                            var lstExd = lstexddatasource.Where(x => x.Name.Trim().ToLower() == item.Name.ToLower()).FirstOrDefault();
                                            if (lstExd == null)
                                            {
                                                lstExd = lstexddatasource.Where(x => x.Name.Trim().ToLower() == part1.ToLower()).FirstOrDefault();
                                            }

                                            if (item.SystemCode != null)
                                            {
                                                if (selectedValue != null && selectedValue != "-1" && selectedValue.Trim().Length > 0 &&
                                                        (part1 == item.SystemCode.ToLower() || (item.SystemCode != null && part1 == item.SystemCode.ToLower())
                                                        || (lstexddatasourceName.FindAll(s => s.IndexOf(part1, StringComparison.OrdinalIgnoreCase) >= 0).Count == 0 && !systemCodes.Contains(part1))))
                                                {
                                                    //System.Data.DataTable dt = controller.GetExternalDataSet(conectionString, query);

                                                    //if (dt != null && dt.Rows.Count > 0)
                                                    //{
                                                    //    DataRow[] dr = dt.Select(Convert.ToString(externalDataObjectId) + " =" + selectedValue.Split('|')[1].ToString());
                                                    //    if (dr.Length > 0)
                                                    //    {
                                                    //        if (part2.ToUpper() == externalDataObjectId)
                                                    //            formula = formula.Replace("{" + result + "}", @"""" + dr[0][Convert.ToString(externalDataObjectId)] + @"""");
                                                    //        else
                                                    //            formula = formula.Replace("{" + result + "}", @"""" + dr[0][Convert.ToString(externalDataField)] + @"""");
                                                    //        //formula = formula.Replace("{" + result + "}", @"""" + dt.Rows[0][externalDataField].ToString() + @"""");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if ((item.Description != null && item.Description.Trim().ToLower() == part1.Trim()) || (item.SystemCode != null && item.SystemCode.Trim().ToLower() == part1.Trim()))
                                                    //        {
                                                    //            formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                                                    //        }
                                                    //    }
                                                    //}

                                                    formula = ReturnFormula(formula, selectedValue, result, item.Description, part1, part2, item.SystemCode, selectedText, externalDataObjectId);
                                                }
                                                else if (lstExd != null)
                                                {
                                                    if (selectedValue != null && selectedValue != "-1" && selectedValue.Trim().Length > 0
                                                                 && (!string.IsNullOrEmpty(lstExd?.Name) && lstExd?.Name?.ToLower() == part1.Trim().ToLower()) || (lstExd?.SystemCode != null && lstExd?.SystemCode.Trim().ToLower() == part1.Trim()))
                                                    {
                                                        formula = ReturnFormula(formula, selectedValue, result, item.Description, part1, part2, item.SystemCode, selectedText, externalDataObjectId);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (selectedValue != null && selectedValue != "-1" && selectedValue.Trim().Length > 0
                                                    && (!lstexddatasourceName.Contains(part1) && !systemCodes.Contains(part1)))
                                                {
                                                    //System.Data.DataTable dt = controller.GetExternalDataSet(conectionString, query);

                                                    //if (dt != null && dt.Rows.Count > 0)
                                                    //{
                                                    //    DataRow[] dr = dt.Select(Convert.ToString(externalDataObjectId) + " =" + selectedValue.Split('|')[1].ToString());
                                                    //    if (dr.Length > 0)
                                                    //    {
                                                    //        if (part2.ToUpper() == externalDataObjectId)
                                                    //            formula = formula.Replace("{" + result + "}", @"""" + dr[0][Convert.ToString(externalDataObjectId)] + @"""");
                                                    //        else
                                                    //            formula = formula.Replace("{" + result + "}", @"""" + dr[0][Convert.ToString(externalDataField)] + @"""");
                                                    //        //formula = formula.Replace("{" + result + "}", @"""" + dt.Rows[0][externalDataField].ToString() + @"""");
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        if ((item.Description != null && item.Description.Trim().ToLower() == part1.Trim()) || (item.SystemCode != null && item.SystemCode.Trim().ToLower() == part1.Trim()))
                                                    //        {
                                                    //            formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                                                    //        }
                                                    //    }
                                                    //}

                                                    formula = ReturnFormula(formula, selectedValue, result, item.Description, part1, part2, item.SystemCode, selectedText, externalDataObjectId);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if ((item.Description != null && item.Description.Trim().ToLower() == part1.Trim()) || (item.SystemCode != null && item.SystemCode.Trim().ToLower() == part1.Trim()))
                                            {
                                                // formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                                                if (part2.ToUpper() == externalDataObjectId)
                                                    formula = formula.Replace("{" + result + "}", @"""" + selectedValue + @"""");
                                                else
                                                    formula = formula.Replace("{" + result + "}", @"""" + selectedValue + @"""");
                                            }
                                        }

                                    }

                                }
                                else
                                {
                                    string filedName = result;
                                    if (position < 0)
                                    {
                                        if (systemCodes.Contains(result.ToLower()))
                                        {
                                            var field = casesAssocFieldCollection.Where(i => i.SystemCode != null && i.SystemCode.ToLower() == result.ToLower()).FirstOrDefault();
                                            if (field != null)
                                            {
                                                filedName = field.Name;
                                            }
                                        }
                                    }
                                    else if (formulaHasExDs == false) // formula contains "." and part1 may be system code of decode value
                                    {
                                        filedName = result.Substring(position + 1).Trim(); // it's part2
                                    }
                                    foreach (var item in casesAssocFieldCollection)
                                    {
                                        //Extract field value and replace with the ASSOC name.
                                        var controlId = "assoc_" + item.AssocFieldType + "_" + item.AssocTypeID;

                                        if (filedName != item.Name.ToString())
                                            continue;
                                        if (item.AssocFieldType == 'D' || item.AssocFieldType == 'X' || item.AssocFieldType == 'L' || item.AssocFieldType == 'A' || item.AssocFieldType == 'N')
                                        {
                                            string selectedTextboxValue = "";

                                            assocFieldTexts.TryGetValue(controlId, out selectedTextboxValue);
                                            if (selectedTextboxValue == "" || selectedTextboxValue == string.Empty || selectedTextboxValue == "Select Value")
                                                selectedTextboxValue = "";
                                            formula = formula.Replace("{" + result + "}", @"""" + selectedTextboxValue + @"""");
                                        }
                                        else if (item.AssocFieldType == 'E')
                                        {
                                            string selectedText1 = "";
                                            string selectedValue1 = "";
                                            assocFieldTexts.TryGetValue(controlId, out selectedText1);
                                            assocFieldValues.TryGetValue(controlId, out selectedValue1);
                                            if (!string.IsNullOrEmpty(selectedValue1))
                                            {
                                                if (selectedValue1?.Split('|')[1] == "-1")
                                                    selectedText1 = "";
                                                formula = formula.Replace("{" + result + "}", @"""" + selectedText1 + @"""");
                                            }
                                        }
                                        else if (item.AssocFieldType == 'O')
                                        {
                                            string selectedValue = "";
                                            assocFieldValues.TryGetValue(controlId, out selectedValue);

                                            var extExDs = lstexddatasource.Where(x => x.Entity_Type_ID == item?.ExternalDataSourceEntityTypeID).FirstOrDefault();//entitiesCommon.GetExternalDataSourceInfoByName(externalDataSourceName); //extended external datasource is part1
                                            if (extExDs != null)
                                            {
                                                var query = extExDs.Query;
                                                var conectionString = extExDs.ConnectionString;
                                                var externalDataObjectId = extExDs.ObjectId;
                                                var externalDataField = extExDs.ObjectDisplay;


                                                //System.Data.DataTable dt = controller.GetExternalDataSet(conectionString, query);

                                                //if (dt != null && dt.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(selectedValue)))
                                                //{
                                                //    DataRow[] dr = dt.Select(Convert.ToString(externalDataObjectId) + " =" + selectedValue?.Split('|')[1].ToString());
                                                //    if (dr.Length > 0)
                                                //    {
                                                //        formula = formula.Replace("{" + result + "}", @"""" + dr[0][Convert.ToString(externalDataField)] + @"""");
                                                //    }
                                                //    else
                                                //    {
                                                //        if ((item.Description != null && item.Description.Trim().ToLower() == part1.Trim()) || (item.SystemCode != null && item.SystemCode.Trim().ToLower() == part1.Trim()))
                                                //        {
                                                //            formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                                                //        }
                                                //    }
                                                //}
                                                //else
                                                //    formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                                            }
                                            else
                                                formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                                        }
                                        else if (item.AssocFieldType == 'C')
                                        {
                                            string selectedTextboxValue = "";
                                            assocFieldTexts.TryGetValue(controlId, out selectedTextboxValue);
                                            formula = formula.Replace("{" + result + "}", selectedTextboxValue);
                                            if (!string.IsNullOrEmpty(item.SeparatorCharactor))
                                                formula = formula.Replace(item.SeparatorCharactor, string.Empty);
                                        }
                                        else
                                        {
                                            string selectedTextboxValue = "";
                                            assocFieldTexts.TryGetValue(controlId, out selectedTextboxValue);
                                            formula = formula.Replace("{" + result + "}", @"""" + selectedTextboxValue + @"""");
                                        }

                                    }
                                }

                                if (formula.IndexOf("\"") > -1)
                                    formula = formula.Replace("\n", " ").Replace("\"", @"""");
                                if (formula.Contains(result))
                                    formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
                            }

                            //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["HttpsCertificate"]))
                            //{
                            //    System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                            //}

                            //WSVBA.WSVBASoapClient obj = new WSVBA.WSVBASoapClient();


                            if (formula.IndexOf("{") > -1)
                            {
                                formula = formula.Replace("{", @"""{");
                                formula = formula.Replace("}", @"}""");
                            }
                            //DataTypes.CommonMethods.WriteToLog("formula :" + formula, DataTypes.CommonMethods.ErrorLevel.ERROR);
                            //returnVal = obj.ExecuteVBAScript(formula);
                            //returnVal = HelperCalculation.ExecuteVBAScript(formula);


                            if (sdateFormats.ToLower() != "mm/dd/yyyy")
                            {
                                if (!string.IsNullOrEmpty(returnVal))
                                {
                                    DateTime dDate;
                                    if (DateTime.TryParse(returnVal, out dDate))
                                    {
                                        string[] strspit = returnVal.Split('/');
                                        returnVal = strspit[1] + "/" + strspit[0] + "/" + strspit[2];
                                    }
                                    else if (returnVal.ToLower().Contains("am") || returnVal.ToLower().Contains("pm") || returnVal.Contains("/"))
                                    {
                                        string[] strspit = returnVal.Split('/');
                                        returnVal = strspit[1] + "/" + strspit[0] + "/" + strspit[2];
                                    }
                                }
                            }
                            //DataTypes.CommonMethods.WriteToLog("returnVal" + returnVal, DataTypes.CommonMethods.ErrorLevel.ERROR);
                        }
                    }
                    else
                    {
                        if (formula.IndexOf("\"") > -1)
                        {
                            formula = formula.Replace("\"", @"""");
                        }

                        //if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["HttpsCertificate"]))
                        //{
                        //    System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                        //}
                        //WSVBA.WSVBASoapClient obj = new WSVBA.WSVBASoapClient();
                        //string calculationResult = obj.ExecuteVBAScript(formula);
                        //returnVal = obj.ExecuteVBAScript(formula);

                        // returnVal = HelperCalculation.ExecuteVBAScript(formula);

                        //returnVal = formula;
                    }
                }
                return (returnVal == originalFormula) ? string.Empty : returnVal;  //.Replace("\"", "")
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Extract From String
        public static List<string> ExtractFromString(string text, string startString, string endString)
        {
            List<string> matched = new List<string>();
            int indexStart = 0, indexEnd = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart = text.IndexOf(startString);
                indexEnd = text.IndexOf(endString);
                if (indexStart != -1 && indexEnd != -1)
                {
                    matched.Add(text.Substring(indexStart + startString.Length,
                        indexEnd - indexStart - startString.Length));
                    text = text.Substring(indexEnd + endString.Length);
                }
                else
                    exit = true;
            }
            return matched;
        }
        #endregion

        #region Get QueryString With Paramaters
        public static string GetQueryStringWithParamaters(string query, string externalDsName, string value, out bool success, string field = "", bool filedNameHasPriority = false, bool checkIn = false, bool checkLike = false)
        {
            string results = query;
            success = false;

            ICollection<string> matches = Regex.Matches(
                    results.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                    .Cast<Match>()
                    .Select(x => x.Groups[1].Value)
                    .ToList();

            foreach (string match in matches)
            {
                if (match.Contains("|"))
                {
                    var split = match.Split('|');

                    if (!filedNameHasPriority)
                    {
                        if (!string.IsNullOrEmpty(externalDsName))
                        {
                            if (split[1].ToUpper().Trim() == externalDsName.ToUpper().Trim())
                            {
                                if (!checkLike)
                                {
                                    success = true;
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " IN (" + value.ToString().Trim() + ")");
                                }
                                else
                                {
                                    success = true;
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " LIKE '%" + value.ToString().Trim() + "%'");
                                }
                            }

                            else
                            if (!string.IsNullOrEmpty(field))
                            {
                                if (split[1].ToUpper().Trim() == field.ToUpper().Trim())
                                {
                                    if (!checkLike)
                                    {
                                        success = true;
                                        results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " IN (" + value.ToString().Trim() + ")");
                                    }
                                    else
                                    {
                                        success = true;
                                        results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " LIKE '%" + value.ToString().Trim() + "%'");
                                    }
                                }
                            }
                        }

                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(field))
                        {
                            if (split[1].ToUpper().Trim() == field.ToUpper().Trim())
                            {
                                if (!checkLike)
                                {
                                    success = true;
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " IN (" + value.ToString().Trim() + ")");
                                }
                                else
                                {
                                    success = true;
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " LIKE '%" + value.ToString().Trim() + "%'");
                                }
                            }

                            else if (split[1].ToUpper().Trim() == externalDsName.ToUpper().Trim())
                            {
                                if (!checkLike)
                                {
                                    success = true;
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " IN (" + value.ToString().Trim() + ")");
                                }
                                else
                                {
                                    success = true;
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " LIKE '%" + value.ToString().Trim() + "%'");
                                }
                            }
                        }

                    }
                }
                else
                {
                }
            }

            return results.ToString();
        }
        #endregion

        #region Return Formula
        public static string ReturnFormula(string formula, string selectedValue, string result, string Description, string part1, string part2, string SystemCode, string selectedText, string externalDataObjectId)
        {
            if (selectedValue != "" && (selectedValue != "Select Value" || selectedValue != "--Select Item--") && !string.IsNullOrEmpty(selectedText))
            {
                if (part2.ToUpper() == externalDataObjectId)
                    formula = formula.Replace("{" + result + "}", @"""" + selectedValue.Split('|')[1] + @"""");
                else
                    formula = formula.Replace("{" + result + "}", @"""" + selectedText + @"""");
            }
            else if (selectedValue == "" || selectedValue == string.Empty || selectedValue == "Select Value" || selectedValue == "--Select Item--")
            {
                formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
            }
            else if ((Description != null && Description.Trim().ToLower() == part1.Trim()) || (SystemCode != null && SystemCode.Trim().ToLower() == part1.Trim()))
            {
                formula = formula.Replace("{" + result + "}", @"""" + string.Empty + @"""");
            }
            return formula;
        }
        #endregion

        public static async Task<int> AddofflineTranInfo(int _TypeID, object _Body_value, string _MethodName, string _ActionType, int _ProcessId, int _RefItemTranInfoID, string _DBPath, string ApplicationName = "", int AppTypeinfoId = 0, string _Mode = "", string sscreenName = "", string scondaryScreenName = "")
        {
            try
            {
                bool flg = false;
                int insertedRecordid = -1;
                ItemTranInfoList ItemTranInfo = new ItemTranInfoList();
                dynamic GetAppTypeInfo;
                if (_Mode?.ToUpper() != "U")
                {
                    GetAppTypeInfo = DBHelper.GetAppTypeInfoListByPk(AppTypeinfoId, _DBPath);
                    GetAppTypeInfo.Wait();
                }
                else
                {
                    GetAppTypeInfo = DBHelper.GetItemTranInfoListByID(AppTypeinfoId, _DBPath);
                    GetAppTypeInfo.Wait();
                    if (GetAppTypeInfo?.Result == null)
                    {
                        // Find for Record With M type
                        /*VishalPr*/
                        GetAppTypeInfo = DBHelper.GetAppTypeInfoIdTransTypeSyscode(ApplicationName, _TypeID, AppTypeinfoId, _DBPath, sscreenName, "M");
                        GetAppTypeInfo.Wait();
                        if (GetAppTypeInfo?.Result != null)
                        {
                            var temp = GetAppTypeInfo.Result as AppTypeInfoList;

                            if (temp.IS_ONLINE == true)
                            {
                                GetAppTypeInfo = DBHelper.GetItemTranInfoListByAppinfoId(temp.APP_TYPE_INFO_ID, _DBPath);
                                GetAppTypeInfo.Wait();
                                if (GetAppTypeInfo?.Result == null)
                                {
                                    GetAppTypeInfo = DBHelper.GetAppTypeInfoIdTransTypeSyscode(ApplicationName, _TypeID, AppTypeinfoId, _DBPath, sscreenName, "M");
                                    GetAppTypeInfo.Wait();
                                    flg = true;
                                }

                                _Mode = "U";

                            }
                            else
                                _Mode = "C";
                        }
                        else if (GetAppTypeInfo?.Result == null)
                        {

                            _Mode = "UC";
                            GetAppTypeInfo = DBHelper.GetAppTypeInfoIdTransTypeSyscode(ApplicationName, _TypeID, AppTypeinfoId, _DBPath, sscreenName, "T");
                            GetAppTypeInfo.Wait();
                            if (GetAppTypeInfo?.Result == null)
                            {
                                GetAppTypeInfo = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ApplicationName, AppTypeinfoId, _DBPath, sscreenName, "T");
                                GetAppTypeInfo.Wait();
                                if (GetAppTypeInfo?.Result == null)
                                {
                                    GetAppTypeInfo = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ApplicationName, AppTypeinfoId, _DBPath, scondaryScreenName, "M");
                                    GetAppTypeInfo.Wait();
                                }
                            }

                            var temp = GetAppTypeInfo.Result as AppTypeInfoList;

                            if (temp.IS_ONLINE == true)
                            {
                                GetAppTypeInfo = DBHelper.GetItemTranInfoListByAppinfoId(temp.APP_TYPE_INFO_ID, _DBPath);
                                GetAppTypeInfo.Wait();
                                if (GetAppTypeInfo?.Result == null)
                                {
                                    string strsystemcode = string.Empty;
                                    if (temp.SYSTEM.ToUpper() == "CASES")
                                    {
                                        strsystemcode = "C1_C2_CASES_CASETYPELIST";
                                    }
                                    else if (temp.SYSTEM.ToUpper() == "QUEST")
                                    {
                                        strsystemcode = "H1_H2_H3_QUEST_AREA_FORM";
                                    }
                                    else if (temp.SYSTEM.ToUpper() == "ENTITY")
                                    {
                                        strsystemcode = "G1_G2_Entity_Cate_TypeDetails";
                                    }

                                    int id = CommonConstants.GetResultBySytemcodeId(temp.SYSTEM, strsystemcode, Convert.ToInt32(temp.TYPE_ID), _DBPath);
                                    GetAppTypeInfo = DBHelper.GetItemTranInfoListByProcessIdappid(Convert.ToInt32(temp.ID), Convert.ToInt32(id), _DBPath);
                                    GetAppTypeInfo.Wait();
                                    if (GetAppTypeInfo?.Result == null)
                                    {
                                        GetAppTypeInfo = DBHelper.GetItemTranInfoListByProcessIdappid(0, Convert.ToInt32(id), _DBPath);
                                        GetAppTypeInfo.Wait();
                                    }
                                }

                                _Mode = "U";

                            }
                        }
                    }
                }
                if (GetAppTypeInfo.Result != null)
                {
                    ItemTranInfo.APP_TYPE_INFO_ID = GetAppTypeInfo.Result.APP_TYPE_INFO_ID;
                    ItemTranInfo.ITEM_TRAN_INFO = JsonConvert.SerializeObject(_Body_value);
                    ItemTranInfo.METHOD = _MethodName;
                    ItemTranInfo.ACTION_TYPE = _ActionType;
                    ItemTranInfo.PROCESS_ID = _ProcessId;
                    ItemTranInfo.LAST_SYNC_DATETIME = DateTime.Now;
                    ItemTranInfo.REF_ITEM_TRAN_INFO_ID = _RefItemTranInfoID;
                    ItemTranInfo.INSTANCE_USER_ASSOC_ID = ConstantsSync.INSTANCE_USER_ASSOC_ID;
                    ItemTranInfo.ITEM_TRAN_INFO_ID = _Mode?.ToUpper() == "UC" ? GetAppTypeInfo.Result.ID : (_Mode?.ToUpper() != "U" ? 0 : (flg == true ? 0 : GetAppTypeInfo.Result.ITEM_TRAN_INFO_ID));
                    return insertedRecordid = await DBHelper.SaveItemTranInfoList(ItemTranInfo, _DBPath);
                }
                else
                    insertedRecordid = 0;

                return insertedRecordid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateEDSInfoList_Entity(bool _Isonline, object _JsonSerialise, int _TypeId, string SystemName, string _ScreenName, int UserAsssocID, string _DbPath)
        {
            try
            {
                var AppinfoFullList = DBHelper.GetAllAppTypeInfoList(_DbPath);
                AppinfoFullList.Wait();
                var AppinfoId = AppinfoFullList.Result.Where(v => v.SYSTEM.ToLower() == SystemName.ToLower() && v.TYPE_SCREEN_INFO.ToLower() == _ScreenName.ToLower() && v.TYPE_ID == _TypeId && v.INSTANCE_USER_ASSOC_ID == UserAsssocID)?.FirstOrDefault().APP_TYPE_INFO_ID;

                EntityClass entityClass = _JsonSerialise as EntityClass;

                foreach (var item in entityClass.AssociationFieldCollection)
                {
                    int AssocID = item.AssocTypeID;
                    var ExtDS = item.EXTERNAL_DATASOURCE;
                    string _field_type = item.FieldType;

                    switch (_field_type)
                    {
                        case "SE":
                        case "EL":
                        case "ME":
                        case "MS":
                        case "SS":
                            var EDSInfoList = DBHelper.GetEDSResultList(_DbPath);
                            EDSInfoList.Wait();
                            var EDSInfoID = EDSInfoList.Result.Where(v => v.APP_TYPE_INFO_ID == AppinfoId && v.ASSOC_FIELD_ID == AssocID && v.INSTANCE_USER_ASSOC_ID == UserAsssocID)?.FirstOrDefault()?.EDS_RESULT_ID;
                            if (EDSInfoID != null)
                            {
                                EDSResultList EDSitem = new EDSResultList
                                {
                                    EDS_RESULT_ID = (int)EDSInfoID,
                                    EDS_RESULT = JsonConvert.SerializeObject(ExtDS),
                                    ASSOC_FIELD_ID = AssocID,
                                    APP_TYPE_INFO_ID = AppinfoId.Value,
                                    INSTANCE_USER_ASSOC_ID = UserAsssocID,
                                    LAST_SYNC_DATETIME = DateTime.Now
                                };
                                DBHelper.SaveEDSResult(EDSitem, _DbPath);
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }




        public static void InsertnewCase_C8_C4_Json(string dbPath, string _username, string _CasetypeId, string caseId, int INSTANCE_USER_ASSOC_ID, string rTM_Name)
        {
            //SaveCase_Body_value.caseId

            //var tm_uname = DBHelper.GetAppTypeInfo_tmname("CASES", Convert.ToInt32(caseId), Convert.ToInt32(_CasetypeId), "C8_GetCaseBasicInfo", dbPath).Result?.TM_Username;

            #region Update C8_GetCaseBasicInfo Record

            var CaseBasic = CasesAPIMethods.GetCaseBasicInfo(_username, Convert.ToString(caseId));
            var CaseBasicData = CaseBasic.GetValue("ResponseContent");
            GetCaseTypesResponse.BasicCase lstResulttemp = new GetCaseTypesResponse.BasicCase();
            if (!string.IsNullOrEmpty(Convert.ToString(CaseBasicData)))
            {
                List<GetCaseTypesResponse.BasicCase> lstResult = new List<GetCaseTypesResponse.BasicCase>();
                lstResulttemp = JsonConvert.DeserializeObject<GetCaseTypesResponse.BasicCase>(CaseBasicData.ToString());
                lstResult.Add(lstResulttemp);
                if (lstResult.Count > 0)
                {
                    /*vishalpr*/
                    /*To avoid Duplcate Record for C8 */
                    var Record = DBHelper.GetAppTypeInfoList("CASES", Convert.ToInt32(caseId), Convert.ToInt32(lstResulttemp.CaseTypeID), "C8_GetCaseBasicInfo", dbPath, null);
                    Record.Wait();

                    if (Record.Result == null)
                    {
                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), "CASES", "C8_GetCaseBasicInfo", INSTANCE_USER_ASSOC_ID, dbPath, 0, Convert.ToString(lstResulttemp.CaseTypeID), "M", caseId, 0, "", "", true, null);
                    }
                    else
                    {
                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), "CASES", "C8_GetCaseBasicInfo", INSTANCE_USER_ASSOC_ID, dbPath, Record.Result.APP_TYPE_INFO_ID, Convert.ToString(lstResulttemp.CaseTypeID), "M", Convert.ToString(Record.Result.ID), 0, "", "", true, null);
                    }
                }
            }

            #endregion

            #region Update C4_GetCaseNotes Record
            var result = CasesAPIMethods.GetCaseNotes(Convert.ToString(caseId), DateTime.Now.ToString("yyyy/MM/dd"));
            var ResponseContent = result.GetValue("ResponseContent");

            if (!string.IsNullOrEmpty(Convert.ToString(ResponseContent)) && ResponseContent.ToString() != "[]")
            {
                List<GetCaseNotesResponse.NoteData> lstResult = JsonConvert.DeserializeObject<List<GetCaseNotesResponse.NoteData>>(ResponseContent.ToString());
                if (lstResult.Count > 0)
                {
                    //tm_uname = DBHelper.GetAppTypeInfo_tmname("CASES", Convert.ToInt32(caseId), Convert.ToInt32(lstResulttemp.CaseTypeID), "C4_GetCaseNotes", dbPath).Result?.TM_Username;
                    /*vishalpr*/
                    Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list("CASES", Convert.ToInt32(lstResulttemp.CaseTypeID), Convert.ToInt32(caseId), dbPath, "C4_GetCaseNotes", "M", null);
                    onlineRecord.Wait();
                    int id = 0;
                    if (onlineRecord?.Result?.Count > 0)
                        id = Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);

                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), "CASES", "C4_GetCaseNotes", INSTANCE_USER_ASSOC_ID, dbPath, id, Convert.ToString(lstResulttemp.CaseTypeID), "M", Convert.ToString(caseId), 0, "", "", true, null);


                    /*it will delete 'T' Entry Opertaion of Notes bcz this recors synced in online - on - Line no:1135*/
                    var collection = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list("CASES", Convert.ToInt32(_CasetypeId), Convert.ToInt32(caseId), dbPath, "C4_AddNotes", "T", null)?.Result;

                    if (collection.Count <= 0)
                    {
                        collection = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list("CASES", Convert.ToInt32(_CasetypeId), Convert.ToInt32(caseId), dbPath, "C4_AddNotes", "M", null)?.Result;
                    }

                    if (collection != null)
                    {
                        foreach (var item in collection)
                        {
                            DBHelper.DeleteAppTypeInfoListById(item, dbPath);
                        }
                    }
                }
            }
            #endregion
        }

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public static string DateFormatStringToString(string inputDate, string inputDateFormate = "MM/dd/yyyy", string outputDateFormate = "yyyy/MM/dd")
        {
            string sInput = inputDate;
            string sOutPut = string.Empty;
            try
            {
                //if (inputDate.IndexOf('-') > -1)
                //{
                //    try
                //    {
                //        inputDate = inputDate.Replace('-', '/');
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //}

                //CultureInfo ci = new CultureInfo("en-US");
                //var tdt = Convert.ToDateTime(inputDate);
                //sOutPut = tdt.ToString("MM/dd/yyyy", ci);
                DateTime Dout = new DateTime();
                DateTime.TryParse(inputDate, CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out Dout);

                sOutPut = Dout.Date.ToString();

                // DateTime dateTime16 = DateTime.ParseExact(inputDate, new string[] { "MM.dd.yyyy", "MM-dd-yyyy", "MM/dd/yyyy" }, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                // string json = null;

                // DateTime dt2 = DateTime.ParseExact(inputDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);


                //inputDate = SeperatorValueReplace(inputDate);
                //sOutPut = DateTime.ParseExact(inputDate, inputDateFormate, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString(outputDateFormate);
            }
            catch (Exception ex)
            {
                return sInput;
            }
            return sOutPut;
        }

        public static string SeperatorValueReplace(string sInput)
        {
            try
            {
                char cSeperator = '/';
                if (sInput.IndexOf('/') > -1)
                {
                    cSeperator = '/';
                }
                else if (sInput.IndexOf("-") > -1)
                {
                    cSeperator = '-';
                }
                else if (sInput.IndexOf(".") > -1)
                {
                    cSeperator = '.';
                }
                string[] arrinput = sInput.Split(cSeperator);
                string[] arrOutPut = new string[arrinput.Length];


                for (int i = 0; i <= arrinput.Length - 1; i++)
                {
                    if (arrinput[i].Length == 1)
                    {
                        arrOutPut[i] = "0" + arrinput[i] + cSeperator;
                    }
                    else
                    {
                        if (i != arrinput.Length - 1)
                        {
                            arrOutPut[i] = arrinput[i] + cSeperator;
                        }
                        else
                        {
                            arrOutPut[i] = arrinput[i];
                        }
                    }
                }
                return String.Concat(arrOutPut);
            }
            catch (Exception)
            {
                return sInput;
            }

        }
    }
}
