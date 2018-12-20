using DataServiceBus.OfflineHelper.DataTypes.Common;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes
{
    public class DBHelper
    {
        public DBHelper(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<InstanceList>().Wait();
            database.CreateTableAsync<AppTypeInfoList>().Wait();
            database.CreateTableAsync<EDSResultList>().Wait();
            database.CreateTableAsync<ItemTranInfoList>().Wait();
            database.CreateTableAsync<FavoriteList>().Wait();
            database.CreateTableAsync<INSTANCE_USER_ASSOC>().Wait();
            // database.CreateTableAsync<ActivityDetails>().Wait();
            database.CreateTableAsync<External_DSCache>().Wait();
        }

        #region InstanceList Table Queries

        #region Get Instance List
        public static Task<List<InstanceList>> GetInstanceList(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<InstanceList>().ToListAsync();
        }
        #endregion

        #region Get Instance List By ID
        public static Task<InstanceList> GetInstanceListByID(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<InstanceList>().Where(i => i.InstanceID == id).FirstOrDefaultAsync();
        }
        #endregion

        #region Save Instance
        public static Task<int> SaveInstance(InstanceList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            if (item.InstanceID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        #endregion

        #endregion

        #region # INSTANCE_USER_ASSOC Tables Queries

        #region Get INSTANCE USER ASSOC List
        public static Task<List<INSTANCE_USER_ASSOC>> GetinstanceuserassocList(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<INSTANCE_USER_ASSOC>().ToListAsync();
        }
        #endregion

        #region Get INSTANCE USER ASSOC List By ID
        public static Task<INSTANCE_USER_ASSOC> GetinstanceuserassocListByID(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<INSTANCE_USER_ASSOC>().Where(i => i.INSTANCE_USER_ASSOC_ID == id).FirstOrDefaultAsync();
        }
        #endregion

        #region Update or Insert INSTANCE_USER_ASSOC For Home Screen Count
        //HomeUpdateScreen
        public static Task<int> Save_InstanceUserAssoc(INSTANCE_USER_ASSOC INSTANCE_USER_ASSOC, string dbPath)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            Task<INSTANCE_USER_ASSOC> result = conn.Table<INSTANCE_USER_ASSOC>().Where(i => i.INSTANCE_ID == INSTANCE_USER_ASSOC.INSTANCE_ID && i.USER == INSTANCE_USER_ASSOC.USER).FirstOrDefaultAsync();

            if (result.Result != null)
            {
                result.Result.HOME_SCREEN_INFO = INSTANCE_USER_ASSOC.HOME_SCREEN_INFO;
                conn.UpdateAsync(result.Result).Wait();
                return Task.FromResult(result.Result.INSTANCE_USER_ASSOC_ID);
            }
            else
            {
                INSTANCE_USER_ASSOC.INSTANCE_USER_ASSOC_ID = 0;
                conn.InsertAsync(INSTANCE_USER_ASSOC).Wait();
                return Task.FromResult(INSTANCE_USER_ASSOC.INSTANCE_USER_ASSOC_ID);
            }
        }
        #endregion

        #region Get INSTANCE USER ASSOC List By User Name == Instance_id
        public static Task<INSTANCE_USER_ASSOC> GetinstanceuserassocListByUsername_Id(string Username, int Instance_id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<INSTANCE_USER_ASSOC>().Where(i => i.USER.ToLower() == Username.ToLower() && i.INSTANCE_ID == Instance_id).FirstOrDefaultAsync();
        }
        #endregion

        #region Save Instance User Assoc
        public static Task<int> SaveInstanceUserAssoc(INSTANCE_USER_ASSOC item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            if (item.INSTANCE_USER_ASSOC_ID != 0)
            {
                database.UpdateAsync(item).Wait();
                return Task.FromResult(item.INSTANCE_USER_ASSOC_ID);
            }
            else
            {
                database.InsertAsync(item).Wait();
                return Task.FromResult(item.INSTANCE_USER_ASSOC_ID);
            }
        }
        #endregion

        #endregion

        #region # AppTypeInfoList Tables Queries

        #region Get all AppTypeInfo List
        public static Task<List<AppTypeInfoList>> GetAllAppTypeInfoList(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(v => v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Get all AppTypeInfo List
        public static Task<List<AppTypeInfoList>> GetAllAppTypeInfoList_Query(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.QueryAsync<AppTypeInfoList>("select * from AppTypeInfoList where ID=2976602 ");
            //Table<AppTypeInfoList>().Where(v => v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion


        #region Get AppTypeInfoList By SYSTEM == TYPE_ID == ID == INSTANCE_USER_ASSOC_ID == TYPE_SCREEN_INFO
        public static Task<AppTypeInfoList> GetAppTypeInfoListBy_systemtypeidScreen(string SystemName, int? TypeId, int? ID, string dbPath, string ScreenName, int INSTANCE_USER_ASSOC_ID)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.TYPE_SCREEN_INFO == ScreenName && i.INSTANCE_USER_ASSOC_ID == INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion



        #region Get AppTypeInfoList By ID and System Name
        public static Task<AppTypeInfoList> GetAppTypeInfoListByID(int id, string System, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.ID == id && i.SYSTEM.ToLower() == System.ToLower() && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfoList By TYPE_ID and System Name screenname
        public static Task<AppTypeInfoList> GetAppTypeInfoListByTypeID_SystemName(int id, string System, string screenname, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.TYPE_ID == id && i.SYSTEM.ToLower() == System.ToLower() && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TYPE_SCREEN_INFO == screenname).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfoList By APP_TYPE_INFO_ID
        public static Task<AppTypeInfoList> GetAppTypeInfoListByPk(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.APP_TYPE_INFO_ID == id && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfoList By TYPE_ID == TYPE_SCREEN_INFO
        public static Task<AppTypeInfoList> GetAppTypeInfoListByTypeID(int id, string ScreenName, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.TYPE_ID == id && i.TYPE_SCREEN_INFO == ScreenName && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SYSTEM & TYPE_SCREEN_INFO
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListBySystemName(string SystemName, string ScreenName, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.TYPE_SCREEN_INFO == ScreenName && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();

        }
        #endregion

        #region Get AppTypeInfo Object By SYSTEM & TYPE_SCREEN_INFO
        public static Task<AppTypeInfoList> GetAppTypeInfoListByNameTypeScreenInfo(string Name, string TypeScreenInfo, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == Name.ToLower() && i.TYPE_SCREEN_INFO.ToUpper() == TypeScreenInfo.ToUpper() && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

        }
        #endregion

        #region Get Single AppTypeInfoList By SYSTEM == TypeScreenInfo == TYPE_ID == TM_Username
        public static Task<AppTypeInfoList> GetAppTypeInfoByNameTypeIdScreenInfo(string SystemName, string TypeScreenInfo, int TypeId, string dbPath, string tm_username)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);

            try
            {
                if (tm_username != null)
                    return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.TYPE_SCREEN_INFO.ToUpper() == TypeScreenInfo.ToUpper() && i.TYPE_ID == TypeId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username == tm_username)?.FirstOrDefaultAsync();
                else
                    return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.TYPE_SCREEN_INFO.ToUpper() == TypeScreenInfo.ToUpper() && i.TYPE_ID == TypeId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID)?.FirstOrDefaultAsync();
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion

        #region Get List OF AppTypeInfoList By SYSTEM == TypeScreenInfo == ID == TM_Username
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByNameIdScreenInfo(string SystemName, string TypeScreenInfo, int ID, string dbPath, string tm_username)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);

            try
            {
                if (tm_username != null)
                    return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.TYPE_SCREEN_INFO.ToUpper() == TypeScreenInfo.ToUpper() && i.ID == ID && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username == tm_username)?.ToListAsync();
                else
                    return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.TYPE_SCREEN_INFO.ToUpper() == TypeScreenInfo.ToUpper() && i.ID == ID && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID)?.ToListAsync();
            }
            catch (Exception)
            {
            }
            return null;
        }
        #endregion

        #region Get App Type Info List By Category Name For Entity only SYSTEM =  CategoryId = TYPE_SCREEN_INFO
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByCategoryId(string SystemName, int? CatId, string Screen_Name, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.CategoryId == CatId && i.TYPE_SCREEN_INFO == Screen_Name && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();

        }
        #endregion

        #region Get App Type Info List By SystemName == ID == TYPE_ID == Screen_Name
        public static Task<AppTypeInfoList> GetAppTypeInfo_tmname(string SystemName, int Id, int TypeID, string Screen_Name, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.TYPE_ID == TypeID && i.TYPE_SCREEN_INFO == Screen_Name && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

        }
        #endregion

        #region Get App Type Info List By SystemName == ID == TYPE_ID == Screen_Name
        public static Task<AppTypeInfoList> GetAppTypeInfoList(string SystemName, int Id, int TypeID, string Screen_Name, string dbPath, string tm_uname)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);

            if (tm_uname != null)
                return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.TYPE_ID == TypeID && i.TYPE_SCREEN_INFO == Screen_Name && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username.ToLower() == tm_uname.ToLower()).FirstOrDefaultAsync();
            else
                return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.TYPE_ID == TypeID && i.TYPE_SCREEN_INFO == Screen_Name && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get App Type Info List By SystemName, & ID & TYPE_SCREEN_INFO & Screen_Name
        public static Task<AppTypeInfoList> GetAppTypeInfoListScreenIDSystem(string SystemName, int? Id, string Screen_Name, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.TYPE_SCREEN_INFO == Screen_Name && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

        }
        #endregion

        #region Get App Type Info List By SystemName, & ID & TYPE_SCREEN_INFO & Screen_Name
        public static Task<AppTypeInfoList> GetAppTypeInfoListContains_scrname(string SystemName, int? Id, string Screen_Name, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.TYPE_SCREEN_INFO.ToLower().Contains(Screen_Name.ToLower()) && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfo List By ID Sync SystemName == Id
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByIDSync(string SystemName, int Id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();

        }
        #endregion

        #region Save AppTypeInfo
        public static Task<int> SaveAppTypeInfo(AppTypeInfoList item, string dbPath)
        {
            try
            {
                SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
                if (item.APP_TYPE_INFO_ID != 0)
                {
                    database.UpdateAsync(item).Wait();
                    return Task.FromResult(item.APP_TYPE_INFO_ID);
                }
                else
                {
                    database.InsertAsync(item).Wait();
                    return Task.FromResult(item.APP_TYPE_INFO_ID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Save ALL data AppTypeInfo
        public static Task<int> SaveAllAppTypeInfo(List<AppTypeInfoList> item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            {
                return database.InsertAllAsync(item);

            }
        }
        #endregion

        #region Update AppTypeInfoList with avoid Duplicate 
        public static Task<int> UpdateAppTypeInfoList(AppTypeInfoList AppTypeInfoList, string dbPath)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            Task<AppTypeInfoList> result = conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == AppTypeInfoList.SYSTEM.ToLower() && i.TYPE_ID == AppTypeInfoList.TYPE_ID && i.TYPE_SCREEN_INFO == AppTypeInfoList.TYPE_SCREEN_INFO && i.TransactionType == AppTypeInfoList.TransactionType && i.INSTANCE_USER_ASSOC_ID == AppTypeInfoList.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

            if (result.Result != null)
            {
                result.Result.ASSOC_FIELD_INFO = AppTypeInfoList.ASSOC_FIELD_INFO;
                result.Result.APP_TYPE_INFO_ID = result.Result.APP_TYPE_INFO_ID;
                result.Result.CategoryId = AppTypeInfoList.CategoryId;
                result.Result.CategoryName = AppTypeInfoList.CategoryName;
                result.Result.ID = AppTypeInfoList.ID;
                result.Result.INSTANCE_USER_ASSOC_ID = AppTypeInfoList.INSTANCE_USER_ASSOC_ID;
                result.Result.LAST_SYNC_DATETIME = AppTypeInfoList.LAST_SYNC_DATETIME;
                result.Result.SYSTEM = AppTypeInfoList.SYSTEM;
                result.Result.TransactionType = AppTypeInfoList.TransactionType;
                result.Result.TYPE_ID = AppTypeInfoList.TYPE_ID;
                result.Result.TYPE_NAME = AppTypeInfoList.TYPE_NAME;
                result.Result.TYPE_SCREEN_INFO = AppTypeInfoList.TYPE_SCREEN_INFO;
                return conn.UpdateAsync(result.Result);

            }
            else
            {
                Task<int> inserted = DBHelper.SaveAppTypeInfo(AppTypeInfoList, dbPath);
                inserted.Wait();
                return inserted;
            }
        }
        #endregion

        #region Delete AppTypeInfoList By APP_TYPE_INFO_ID
        public static Task<int> DeleteAppTypeInfoListById(AppTypeInfoList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            database.DeleteAsync(item).Wait();
            return Task.FromResult(item.APP_TYPE_INFO_ID);
        }
        #endregion

        #region Get AppTypeInfo List By ID Sync SystemName == TypeId == Id 
        public static Task<AppTypeInfoList> GetAppTypeInfoListByIDS(string SystemName, int TypeId, int Id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.TYPE_ID == TypeId && i.ID == Id && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

        }
        #endregion

        #region GetAppTypeInfoListBy SYSTEM == TYPE_ID == TYPE_SCREEN_INFO == TransactionType

        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByTypeIdTransTypeSyscode(string SystemName, int? TypeId, string dbPath, string ScreenName, string TransType = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }

        #endregion

        #region GetApp Type InfoList By SystemName == TYPE_SCREEN_INFO ==TransactionType == tm_username

        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByTransTypeSyscode_tm_username(string SystemName, string dbPath, string ScreenName, string TransType = null, string tm_username = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            if (tm_username != null)
                return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username.ToLower() == tm_username.ToLower()).ToListAsync();
            else
                return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }

        #endregion

        #region GetApp Type InfoList By SystemName == TYPE_SCREEN_INFO ==TransactionType 

        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByTransTypeSyscodes(string SystemName, string dbPath, string ScreenName, string TransType = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }

        #endregion

        #region Get AppTypeInfo List By SystemName == TypeId == ID == dbPath == ScreenName == TransType == tm_uname
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByIdTransTypeSyscode_list(string SystemName, int? TypeId, int ID, string dbPath, string ScreenName, string TransType, string tm_uname)
        {

            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
                if (!string.IsNullOrEmpty(tm_uname))
                    return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.TYPE_SCREEN_INFO.ToLower() == ScreenName.ToLower() && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username.ToLower() == tm_uname.ToLower()).OrderByDescending(y => y.APP_TYPE_INFO_ID).ToListAsync();
                else
                    return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.TYPE_SCREEN_INFO.ToLower() == ScreenName.ToLower() && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).OrderByDescending(y => y.APP_TYPE_INFO_ID).ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Get AppTypeInfo Object By SystemName == TypeId == ID == dbPath == ScreenName == TransType == tm_uname
        public static Task<AppTypeInfoList> GetAppTypeInfoListByIdTransTypeSyscode_Object(string SystemName, int? TypeId, int ID, string dbPath, string ScreenName, string TransType, string tm_uname)
        {

            try
            {
                SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
                if (!string.IsNullOrEmpty(tm_uname))
                    return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.TYPE_SCREEN_INFO.ToLower() == ScreenName.ToLower() && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username.ToLower() == tm_uname.ToLower()).FirstOrDefaultAsync();
                else
                    return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.TYPE_SCREEN_INFO.ToLower() == ScreenName.ToLower() && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == ID == dbPath == ScreenName == secondaryScreenName == TransType 
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByIdTransTypeSyscodeIsonline(string SystemName, int? TypeId, int ID, string dbPath, string ScreenName, string secondaryScreenName, bool Isonline = false, string tm_username = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            if (tm_username != null)
                return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && (i.TYPE_SCREEN_INFO == ScreenName || i.TYPE_SCREEN_INFO == secondaryScreenName) && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.IS_ONLINE == Isonline && i.TM_Username.ToLower() == tm_username.ToLower()).ToListAsync();
            else
                return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && (i.TYPE_SCREEN_INFO == ScreenName || i.TYPE_SCREEN_INFO == secondaryScreenName) && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.IS_ONLINE == Isonline).ToListAsync();

        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == ID == dbPath 
        public static Task<AppTypeInfoList> GetAppTypeInfoListByIsonline(string SystemName, int? TypeId, int ID, int Catid, int AppTypeId, string dbPath)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.CategoryId == Catid && i.APP_TYPE_INFO_ID == AppTypeId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == ID == ScreenName == TransType
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByCatIdTransTypeSyscode(string SystemName, int? TypeId, int ID, string dbPath, string ScreenName, string TransType = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.CategoryId == ID && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == ID
        public static Task<AppTypeInfoList> GetAppTypeInfoListByCatIdol(string SystemName, int? TypeId, int ID, string dbPath)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.CategoryId == ID && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SystemName == ID == ScreenName == TransType
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByTransTypeSyscodewithouttypeid(string SystemName, int ID, string dbPath, string ScreenName, string TransType = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.ID == ID && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SystemName == ID == ScreenName == TransType
        public static Task<AppTypeInfoList> GetAppTypeInfoByTransTypeSyscodewithouttypeid(string SystemName, int ID, string dbPath, string ScreenName, string TransType = null, string tm_uname = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            if (tm_uname != null)
                return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.ID == ID && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && i.TM_Username.ToLower() == tm_uname.ToLower()).FirstOrDefaultAsync();
            else
                return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.ID == ID && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == ID == ScreenName == TransType
        public static Task<AppTypeInfoList> GetAppTypeInfoIdTransTypeSyscode(string SystemName, int? TypeId, int ID, string dbPath, string ScreenName, string TransType = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.ID == ID && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Search SYSTEMName  == TYPE_SCREEN_INFO single Record
        public static AppTypeInfoList UserScreenRetrive(string SYSTEMName, string dbPath, string ScreenName)
        {
            SQLiteConnection conn = new SQLiteConnection(dbPath);
            AppTypeInfoList result = conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToLower() == SYSTEMName.ToLower() && i.TYPE_SCREEN_INFO == ScreenName && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefault();

            return result;
        }

        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == Cat ID == ScreenName == TransType == ID
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByCatIdTransTypeSyscodeID(string SystemName, int? TypeId, int catID, string dbPath, string ScreenName, string TransType = null, int? ID = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.CategoryId == catID && i.TYPE_SCREEN_INFO == ScreenName && i.TransactionType == TransType && i.ID == ID && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == Cat ID == ScreenName ==  ID
        public static Task<List<AppTypeInfoList>> GetAppTypeInfoListByCatIdSyscodeID(string SystemName, int? TypeId, int catID, string dbPath, string ScreenName, int? ID = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<AppTypeInfoList>().Where(i => i.SYSTEM.ToUpper() == SystemName.ToUpper() && i.TYPE_ID == TypeId && i.CategoryId == catID && i.TYPE_SCREEN_INFO == ScreenName && i.ID == ID && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Get AppTypeInfoList By SystemName == TypeId == Cat ID == ScreenName == TransType == ID
        public static Task<List<ItemTranInfoList>> GetItemTranInfoListByCatIdTransTypeSyscodeID(int AppTypeInfoId, string dbPath, int? ID = null)
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection(dbPath);
            return conn.Table<ItemTranInfoList>().Where(i => i.REF_ITEM_TRAN_INFO_ID == ID && i.APP_TYPE_INFO_ID == AppTypeInfoId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #endregion

        #region # EDSResultList Tables Queries

        #region Get EDSResult List
        public static Task<List<EDSResultList>> GetEDSResultList(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<EDSResultList>().Where(v => v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Get EDSResultList By ID
        public static Task<EDSResultList> GetEDSResultListwithId(int id, int appTypeInfoId, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<EDSResultList>().Where(i => i.ASSOC_FIELD_ID == id && i.APP_TYPE_INFO_ID == appTypeInfoId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get EDSResultList By ID
        public static Task<List<EDSResultList>> GetEDSResultListwithAPP_TYPE_INFO_ID(int appTypeInfoId, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<EDSResultList>().Where(i => i.APP_TYPE_INFO_ID == appTypeInfoId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Save EDSResult
        public static Task<int> SaveEDSResult(EDSResultList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            if (item.EDS_RESULT_ID != 0)
            {
                database.UpdateAsync(item).Wait();
                return Task.FromResult(item.APP_TYPE_INFO_ID);
            }
            else
            {
                database.InsertAsync(item).Wait();
                return Task.FromResult(item.APP_TYPE_INFO_ID);
            }
        }
        #endregion

        #region Save ALL data EDSResult
        public static Task<int> SaveAllEDSResult(List<EDSResult> item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            {
                return database.InsertAllAsync(item);

            }
        }
        #endregion

        #region Delete AppTypeInfoList By APP_TYPE_INFO_ID
        public static Task<int> DeleteEDSResultListById(EDSResultList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            database.DeleteAsync(item).Wait();
            return Task.FromResult(item.EDS_RESULT_ID);
        }
        #endregion



        #region Delete EDSResult List Table
        public static async Task<int> DropEDSResultList(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return await database.DropTableAsync<EDSResultList>();
        }
        #endregion


        #endregion

        #region# ItemTranInfoList Tables Queries

        #region Get ItemTranInfo List
        public static Task<List<ItemTranInfoList>> GetItemTranInfoListList(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<ItemTranInfoList>().Where(v => v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID)?.ToListAsync();
        }
        #endregion

        #region Get ItemTranInfoList By ID
        public static Task<ItemTranInfoList> GetItemTranInfoListByID(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<ItemTranInfoList>().Where(i => i.ITEM_TRAN_INFO_ID == id && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID)?.FirstOrDefaultAsync();
        }
        #endregion

        #region Get ItemTranInfoList By PROCCESSID
        public static Task<List<ItemTranInfoList>> GetItemTranInfoListByProcessId(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<ItemTranInfoList>().Where(i => i.PROCESS_ID == id && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID)?.ToListAsync();
        }
        #endregion

        #region Get ItemTranInfoList By PROCCESSID and CaseTypeId
        public static Task<ItemTranInfoList> GetItemTranInfoListByProcessIdappid(int id, int apptypeinfoid, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<ItemTranInfoList>().Where(i => i.PROCESS_ID == id && i.APP_TYPE_INFO_ID == apptypeinfoid && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID)?.FirstOrDefaultAsync();
        }
        #endregion

        #region Get ItemTranInfoList By APP_TYPE_INFO_ID
        public static Task<ItemTranInfoList> GetItemTranInfoListByAppinfoId(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<ItemTranInfoList>().Where(i => i.APP_TYPE_INFO_ID == id && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Save ItemTranInfoList
        public static Task<int> SaveItemTranInfoList(ItemTranInfoList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            if (item.ITEM_TRAN_INFO_ID != 0)
            {
                database.UpdateAsync(item).Wait();
                return Task.FromResult(item.ITEM_TRAN_INFO_ID);
            }
            else
            {
                database.InsertAsync(item).Wait();
                return Task.FromResult(item.ITEM_TRAN_INFO_ID);
            }
        }
        #endregion

        #region Delete ItemTranInfoList
        public static Task<int> DeleteItemTranInfoList(ItemTranInfoList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            database.DeleteAsync(item).Wait();
            return Task.FromResult(item.ITEM_TRAN_INFO_ID);
        }
        #endregion

        #endregion

        #region# Favorite Table Queries
        #region Get Favorite List
        public static Task<FavoriteList> GetFavoriteList(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<FavoriteList>().Where(v => v.IS_ACTIVE == "Y" && v.FAVORITE_ID == id && v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get FavoriteList By ID
        public static Task<FavoriteList> GetFavoriteListByID(int applicatoinId, int? QuestAreaId, int? TypeId, string FavoriteName, string dbPath)
        {
            //SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            //return database.Table<FavoriteList>().Where(i => i.TYPE_ID == TypeId && i.QUEST_AREA_ID == QuestAreaId && i.APPLICATION_ID == applicatoinId && i.FAVORITE_NAME.ToLower() == FavoriteName.ToLower()).FirstOrDefaultAsync();
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<FavoriteList>().Where(i => i.TYPE_ID == TypeId && i.QUEST_AREA_ID == QuestAreaId && i.APPLICATION_ID == applicatoinId && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion

        #region Get FavoriteList By Name
        public static Task<List<FavoriteList>> GetFavoriteListByName(string Name, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<FavoriteList>().Where(i => i.CREATED_BY.ToUpper() == Name.ToUpper() && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
        }
        #endregion

        #region Save Favorite
        public static Task<int> SaveFavorite(FavoriteList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            if (item.FAVORITE_ID != 0)
            {
                database.UpdateAsync(item).Wait();
                return Task.FromResult(item.FAVORITE_ID);
            }
            else
            {
                database.InsertAsync(item).Wait();
                return Task.FromResult(item.FAVORITE_ID);
            }
        }
        #endregion

        #region Remove Favorite
        public static Task<int> RemoveFavorite(FavoriteList item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            database.DeleteAsync(item).Wait();
            return Task.FromResult(item.FAVORITE_ID);
        }
        #endregion

        #region Remove Favorites
        public static void RemoveFavorites(string Created_BY, string dbPath)
        {
            try
            {
                SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
                var List = database.Table<FavoriteList>().Where(i => i.CREATED_BY.ToUpper() == Created_BY.ToUpper()).ToListAsync();
                foreach (var item in List.Result)
                {
                    database.DeleteAsync(item).Wait();
                }
            }
            catch
            {
            }


            //return Task.FromResult(item.FAVORITE_ID);
        }
        #endregion

        #endregion

        #region Get EDSList BY TypeID and AssocFieldID
        public static Task<List<EDSResultList>> EDSResultByAssocFieldId(string TypeID, string TYPE_SCREEN_INFO, int AssocFieldID, string systemName, string dbPath)
        {
            try
            {
                int AppTypeInfoId = AppTypeInfoID(dbPath, TypeID, systemName, TYPE_SCREEN_INFO).Result;
                SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
                return database.Table<EDSResultList>().Where(v => v.APP_TYPE_INFO_ID == AppTypeInfoId && v.ASSOC_FIELD_ID == AssocFieldID && v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).ToListAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region AppTypeInfoID By TypeID
        public static async Task<int> AppTypeInfoID(string dbPath, string TypeID, string SystemName, string TYPE_SCREEN_INFO)
        {
            try
            {
                SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
                int typeID = Convert.ToInt32(TypeID);
                return database.Table<AppTypeInfoList>().Where(v => v.TYPE_ID == typeID && v.SYSTEM.ToLower() == SystemName.ToLower() && v.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID && v.TYPE_SCREEN_INFO == TYPE_SCREEN_INFO).FirstOrDefaultAsync().Result.APP_TYPE_INFO_ID;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region # ActivityDetails Tables Queries

        //#region Get Instance List
        //public static Task<List<ActivityDetails>> GetActivityDetails(string dbPath)
        //{
        //    SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
        //    return database.Table<ActivityDetails>().ToListAsync();
        //}
        //#endregion

        //#region Get Instance List By ID
        //public static Task<ActivityDetails> GetActivityDetailsByID(int id, string dbPath)
        //{
        //    SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
        //    return database.Table<ActivityDetails>().Where(i => i.ActivityId == id).FirstOrDefaultAsync();
        //}
        //#endregion

        //#region Save Instance
        //public static Task<int> SaveActivityDetails(ActivityDetails item, string dbPath)
        //{
        //    SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
        //    if (item.ActivityId != 0)
        //    {
        //        return database.UpdateAsync(item);
        //    }
        //    else
        //    {
        //        return database.InsertAsync(item);
        //    }
        //}
        //#endregion

        //#region Get App Type Info List By SystemName == ID == TYPE_ID == Screen_Name
        //public static Task<ActivityDetails> GetActivityDetails(string SystemName, int Id, int TypeID, string dbPath)
        //{
        //    SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
        //    return database.Table<ActivityDetails>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.ID == Id && i.TYPE_ID == TypeID && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();

        //}
        //#endregion






        #endregion


        #region # External_DS Cache Tables Queries

        #region Get All External_DS Cache
        public static Task<List<External_DSCache>> GetAllXDSDetails(string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<External_DSCache>().ToListAsync();
        }
        #endregion

        #region Get External_DS Cache List By ID
        public static Task<External_DSCache> GetXDSDetailsByID(int id, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<External_DSCache>().Where(i => i.EDS_CACHE_ID == id).FirstOrDefaultAsync();
        }
        #endregion

        #region Save External_DS Cache
        public static Task<int> SaveXDSDetails(External_DSCache item, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            if (item.EDS_CACHE_ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }
        #endregion

        #region Get External_DS Cache List By SystemName == EDS_ID == TYPE_ID == Screen_Name
        public static Task<External_DSCache> GetXDSDetails(string SystemName, int XDSid, string dbPath)
        {
            SQLiteAsyncConnection database = new SQLiteAsyncConnection(dbPath);
            return database.Table<External_DSCache>().Where(i => i.SYSTEM.ToLower() == SystemName.ToLower() && i.EXT_DATASOURCE_ID == XDSid && i.INSTANCE_USER_ASSOC_ID == ConstantsSync.INSTANCE_USER_ASSOC_ID).FirstOrDefaultAsync();
        }
        #endregion






        #endregion
    }
}
