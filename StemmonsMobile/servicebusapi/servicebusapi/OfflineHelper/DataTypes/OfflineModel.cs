using SQLite;

using System;
using System.Collections.Generic;
using System.Text;

namespace DataServiceBus.OfflineHelper.DataTypes
{
    #region Offline Table Structure
    public class InstanceList
    {
        [PrimaryKey, AutoIncrement]
        public int InstanceID { get; set; }
        public string InstanceName { get; set; }
        public string InstanceUrl { get; set; }
    }

    public class AppTypeInfoList
    {
        [PrimaryKey, AutoIncrement]
        public int APP_TYPE_INFO_ID { get; set; }
        public string SYSTEM { get; set; }
        public int? TYPE_ID { get; set; }
        public int? ID { get; set; }// it will save Entity Id, Case id and so on
        public string TYPE_NAME { get; set; }
        public string ASSOC_FIELD_INFO { get; set; }
        public string TYPE_SCREEN_INFO { get; set; }
        public string TransactionType { get; set; }//M- when view master Data, T- When perform operation for Transaction table offline mode
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime LAST_SYNC_DATETIME { get; set; }
        public int INSTANCE_USER_ASSOC_ID { get; set; }
        public bool IS_ONLINE { get; set; }
        public string TM_Username { get; set; }
    }

    public class EDSResultList
    {
        [PrimaryKey, AutoIncrement]
        public int EDS_RESULT_ID { get; set; }
        public int APP_TYPE_INFO_ID { get; set; }
        public int ASSOC_FIELD_ID { get; set; }
        public string EDS_RESULT { get; set; }
        public DateTime LAST_SYNC_DATETIME { get; set; }
        public int INSTANCE_USER_ASSOC_ID { get; set; }
    }

    public class ItemTranInfoList
    {
        [PrimaryKey, AutoIncrement]
        public int ITEM_TRAN_INFO_ID { get; set; }
        public int APP_TYPE_INFO_ID { get; set; }
        public string ITEM_TRAN_INFO { get; set; }
        public string METHOD { get; set; }
        public int PROCESS_ID { get; set; }
        public DateTime LAST_SYNC_DATETIME { get; set; }
        public int INSTANCE_USER_ASSOC_ID { get; set; }
        public string ACTION_TYPE { get; set; }
        public int REF_ITEM_TRAN_INFO_ID { get; set; }
        public int ACTION_SEQUENCE { get; set; }
    }

    public class FavoriteList
    {
        [PrimaryKey, AutoIncrement]
        public int FAVORITE_ID { get; set; }
        public string FAVORITE_NAME { get; set; }
        //public int APP_TYPE_INFO_ID { get; set; }
        public string FIELD_VALUES { get; set; }
        public string IS_ACTIVE { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public DateTime MODIFY_DATETIME { get; set; }
        public DateTime LAST_SYNC_DATETIME { get; set; }
        public int INSTANCE_USER_ASSOC_ID { get; set; }
        public int APPLICATION_ID { get; set; }
        public int QUEST_AREA_ID { get; set; }
        public int TYPE_ID { get; set; }
    }

    public class INSTANCE_USER_ASSOC
    {
        [PrimaryKey, AutoIncrement]
        public int INSTANCE_USER_ASSOC_ID { get; set; }
        public int INSTANCE_ID { get; set; }
        public string USER { get; set; }
        public string HOME_SCREEN_INFO { get; set; }
    }

    //public class ActivityDetails
    //{
    //    private DateTime _lAST_MODIFIED_DATETIME;

    //    [PrimaryKey, AutoIncrement]
    //    public int ActivityId { get; set; }
    //    public string SYSTEM { get; set; }
    //    public int? TYPE_ID { get; set; }
    //    public int? ID { get; set; }// it will save Entity Id, Case id and so on
    //    public string ActivityJson { get; set; }
    //    public string ActivityType { get; set; }
    //    public DateTime LAST_MODIFIED_DATETIME { get => _lAST_MODIFIED_DATETIME; set => _lAST_MODIFIED_DATETIME = DateTime.Now; }
    //    public int INSTANCE_USER_ASSOC_ID { get; set; }
    //}

    #endregion

    public class GetAllCaseType
    {
        public List<AppTypeInfo> AppTypeInfo { get; set; }
        public List<EDSResult> EDSResult { get; set; }
    }

    public class AppTypeInfo
    {
        public int APP_TYPE_INFO_ID { get; set; }
        public string SYSTEM { get; set; }
        public int TYPE_ID { get; set; }
        public int? ID { get; set; }
        public string TYPE_NAME { get; set; }
        public string TransactionType { get; set; }
        public string ASSOC_FIELD_INFO { get; set; }
        public string TYPE_SCREEN_INFO { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime LAST_SYNC_DATETIME { get; set; }
        //public DateTime MODIFIED_DATETIME { get; set; }
        public int INSTANCE_USER_ASSOC_ID { get; set; }
        public bool Is_Online { get; set; }
        public string TM_Username { get; set; }
    }

    public class Favorite
    {
        public int FAVORITE_ID { get; set; }
        public string APP_TYPE_INFO_ID { get; set; }
        public string FIELD_VALUES { get; set; }
        public string IS_ACTIVE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DATETIME { get; set; }
        public string MODIFY_DATETIME { get; set; }
        public string LAST_SYNC_DATETIME { get; set; }
        public string INSTANCE_USER_ASSOC_ID { get; set; }

    }

    public class EDSResult
    {
        public int EDS_RESULT_ID { get; set; }
        public int APP_TYPE_INFO_ID { get; set; }
        public string ASSOC_FIELD_ID { get; set; }
        public string EDS_RESULT { get; set; }
        public DateTime LAST_SYNC_DATETIME { get; set; }
        public int INSTANCE_USER_ASSOC_ID { get; set; }
    }

    #region EntityClasses

    public class GetAllEntityType
    {
        public List<AppTypeInfo> AppTypeInfo { get; set; }
        public List<EDSResult> EDSResult { get; set; }
    }

    #endregion

    #region EntityClasses
    public class MasterSyncGetAllEntityType
    {
        public List<Tuple<int, int, string, string>> RemoveItemList { get; set; }

        public GetAllEntityType GetAllEntityType { get; set; }
    }

    public class MasterSyncGetAllCaseType
    {
        //public List<Tuple<int, int, string, string>> RemoveItemList { get; set; }
        public List<KeyValuePair<int, int>> RemoveItemList { get; set; }
        public GetAllCaseType GetAllCaseType { get; set; }
    }

    #endregion

    #region Standards Classes

    public class GetAllStandards
    {
        public List<AppTypeInfo> AppTypeInfo { get; set; }
        public List<EDSResult> EDSResult { get; set; }
    }

    #endregion

    #region Quest Classes

    public class GetAllQuestType
    {
        public List<AppTypeInfo> AppTypeInfo { get; set; }
        public List<EDSResult> EDSResult { get; set; }
    }

    #endregion
}
