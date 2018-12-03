using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SQLite;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.DataTypes.DataType.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Common
{
    public class HelperProccessQueue
    {

        #region Cases API Lists

        public const string AddCaseNotes = "/api/v1/Cases/AddCaseNotes";
        public const string CloseCase = "/api/v1/Cases/CloseCase";
        public const string ReOpenCase = "/api/v1/Cases/ReOpenCase";
        public const string CreateCase = "/api/v1/Cases/CreateCase";
        public const string AssignCase = "/api/v1/Cases/AssignCase";
        public const string UnsubscribeFromAlert = "/api/v1/Cases/UnsubscribeFromAlert";
        public const string SubscribeToAlert = "/api/v1/Cases/SubscribeToAlert";
        public const string UploadFileToCase = "/api/v1/Cases/UploadFileToCase";
        public const string CreateCaseOptimized = "/api/v1/Cases/CreateCaseOptimized";
        public const string SubscribeToCaseAlerts = "/api/v1/Cases/SubscribeToCaseAlerts";
        public const string SendEmailAlert = "/api/v1/Cases/SendEmailAlert";
        public const string SaveCase = "/api/v1/Cases/SaveCase";
        public const string SaveCaseOptimized = "/api/v1/Cases/SaveCaseOptimized";
        public const string AcceptCase = "/api/v1/Cases/AcceptCase";
        public const string RemoveFavorite = "/api/v1/Cases/RemoveFavorite";
        public const string AddFavorite = "/api/v1/Cases/AddFavorite";
        public const string SubscribeToHopper = "/api/v1/Cases/SubscribeToHopper";
        public const string ReturnCaseToLastAssignee = "/api/v1/Cases/ReturnCaseToLastAssignee";
        public const string ReturnCaseToLastAssigner = "/api/v1/Cases/ReturnCaseToLastAssigner";
        public const string DeclineAndReturn = "/api/v1/Cases/DeclineAndReturn";
        public const string ApproveandAssign = "/api/v1/Cases/ApproveCase";
        public const string ApproveandReturn = "/api/v1/Cases/ApproveandReturn";
        public const string DeclienAndAssign = "/api/v1/Cases/DeclienAndAssign";

        #endregion

        #region Entity API Lists
        public const string CreateNewEntityItem = "/api/v1/Entity/CreateNewEntityItem";
        public const string AddEntityNote = "/api/v1/Entity/AddEntityNote";
        public const string AssignEntity = "/api/v1/Entity/AssignEntity";
        public const string SaveEntityItem = "/api/v1/Entity/SaveEntityItem";
        public const string TakeOwnership = "/api/v1/Entity/TakeOwnership";
        public const string DeleteEntityItem = "/api/v1/Entity/DeleteEntityItem";
        public const string AddFileToEntityAssocType = "/api/v1/Entity/AddFileToEntityAssocType";
        public const string ForwardEntity = "/api/v1/Entity/ForwardEntity";

        #endregion

        #region QUEST API Lists.

        public const string AddForm = "/api/v1/Quest/AddForm";
        public const string UpdateCaseNotesToQuestion = "/api/v1/Quest/UpdateCaseNotesToQuestion";
        public const string AddCaseToQuestion = "/api/v1/Quest/AddCaseToQuestion";
        public const string AddQuestionsMetadata = "/api/v1/Quest/AddQuestionsMetadata";
        public const string UpdateForm = "/api/v1/Quest/UpdateForm";
        public const string DeleteItemQuestionMetadataCase = "/api/v1/Quest/DeleteItemQuestionMetadataCase";

        #endregion

        public enum Applications
        {
            Quest = 1, Cases = 2, Entities = 3, Departments = 1002, Standards = 2002
        }

        // remove all the record after login which has processID >0 or not null
        // at the same time get the transaction ID of queue table and remove those records which have trans Type "T" with same transaction ID which is going to be delete.

        public void RemoveSyncedQueueTableRecord(string dbPath)
        {
            try
            {
                SQLite.SQLiteAsyncConnection db = new SQLiteAsyncConnection(dbPath);

                // these are the record which have processID >0
                Task<List<ItemTranInfoList>> rsItemTranInfoList = db.Table<ItemTranInfoList>().Where(t => !string.IsNullOrEmpty(Convert.ToString(t.PROCESS_ID)) && t.PROCESS_ID > 0).ToListAsync();

                //Get trans Date from AppTypeInfo table 
                Task<List<AppTypeInfo>> rsAppTypeInfo = db.Table<AppTypeInfo>().Where(t => t.TransactionType.ToLower().Trim() == "t").ToListAsync();

                foreach (var item in rsItemTranInfoList.Result) // these are the records which are synced with sql table
                {
                    //remove from here where you have stored json of this table appInfo table.
                    var itemAppTypeInfo = rsAppTypeInfo.Result.Where(t => t.ID == item.ITEM_TRAN_INFO_ID);
                    db.DeleteAsync(itemAppTypeInfo);
                }
                db.DeleteAsync(rsItemTranInfoList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SyncSqlLiteTableWithSQLDatabase(string dbPath, int iINSTANCE_USER_ASSOC_ID, string _username)
        {
            try
            {
                // get application Info ID by application and user wise.
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(dbPath);

                // read the records one by one with sequences FIFO.
                Task<List<ItemTranInfoList>> rsItemTranInfoList = db.Table<ItemTranInfoList>().Where(t => t.PROCESS_ID == 0 && t.INSTANCE_USER_ASSOC_ID == iINSTANCE_USER_ASSOC_ID).ToListAsync();
                rsItemTranInfoList.Wait();
                var orgListItemTranInfoList = rsItemTranInfoList.Result;

                foreach (var itemTransInfo in rsItemTranInfoList.Result)
                {
                    //itemTransInfo.ITEM_TRAN_INFO_ID
                    // itemTransInfo.REF_ITEM_TRAN_INFO_ID
                    List<ItemTranInfoList> lsItemTranInfoListDepedance = new List<ItemTranInfoList>();
                    lsItemTranInfoListDepedance.Add(itemTransInfo);
                    GetChildListInSequences(itemTransInfo.ITEM_TRAN_INFO_ID, orgListItemTranInfoList, ref lsItemTranInfoListDepedance);

                    foreach (var transTable in lsItemTranInfoListDepedance)
                    {
                        //call each trigger and method 
                        CallTriggerQueueTable(transTable, dbPath, _username);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// find the child or subsquence record and make it in one pair,for easy to process
        public static void GetChildListInSequences(int iITEM_TRAN_INFO_ID, List<ItemTranInfoList> lsItemTranInfoList, ref List<ItemTranInfoList> lsItemTranInfoListDepedance)
        {
            var reflsItemTranInfoList = lsItemTranInfoList.Where(t => t.REF_ITEM_TRAN_INFO_ID == iITEM_TRAN_INFO_ID);
            if (reflsItemTranInfoList?.Count() == 0)
            {
                return;
            }
            else
            {
                lsItemTranInfoListDepedance.AddRange(reflsItemTranInfoList);
                GetChildListInSequences(reflsItemTranInfoList.FirstOrDefault().ITEM_TRAN_INFO_ID, lsItemTranInfoList, ref lsItemTranInfoListDepedance);
            }
        }
        public static void CallTriggerQueueTable(ItemTranInfoList itemTranInfo, string dbPath, string _username)
        {
            try
            {

                int AppTypeInfoID = itemTranInfo.APP_TYPE_INFO_ID;
                // get the system name from the application Info ID like quest,cases, entity.
                SQLiteAsyncConnection db = new SQLiteAsyncConnection(dbPath);
                Task<List<AppTypeInfoList>> rsAppTypeInfoList = db.Table<AppTypeInfoList>().Where(t => t.APP_TYPE_INFO_ID == AppTypeInfoID).ToListAsync();
                rsAppTypeInfoList.Wait();

                // before sync again verify processID value, if null then only go for proceed 

                // read the records one by one with sequences FIFO.
                Task<List<ItemTranInfoList>> rsItemTranInfoList = db.Table<ItemTranInfoList>().Where(t => t.ITEM_TRAN_INFO_ID == itemTranInfo.ITEM_TRAN_INFO_ID).ToListAsync();
                rsItemTranInfoList.Wait();

                if (rsItemTranInfoList.Result.Count > 0)
                {
                    // if that record already synced then no need to go for sync.
                    int? precessID = rsItemTranInfoList.Result?.Select(t => t.PROCESS_ID)?.FirstOrDefault();

                    if (precessID != 0)
                    {
                        // this record is already sync.
                        return;
                    }
                }

                #region Update Process ID Which Shows that item has been Processed
                SQLiteAsyncConnection UP_db = new SQLiteAsyncConnection(dbPath);
                var temItem = itemTranInfo;
                temItem.PROCESS_ID = -1;
                UP_db.UpdateAsync(temItem);

                #endregion

                if (rsAppTypeInfoList.Result.Count > 0)
                {
                    string SystemName = rsAppTypeInfoList.Result.FirstOrDefault().SYSTEM;
                    switch (SystemName)
                    {
                        case "CASES":
                            // call api of cases
                            CallTriggerQueueTable_CASES(itemTranInfo, dbPath, _username);
                            break;
                        case "ENTITY":
                            // call api of Entity
                            CallTriggerQueueTable_CASES(itemTranInfo, dbPath, _username);
                            break;
                        case "QUEST":
                            // call api of QUEST                        
                            CallTriggerQueueTable_CASES(itemTranInfo, dbPath, _username);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int ReturnProcessId<T>(string Json, Func<T, JObject> MethodName, T Tobj)
        {
            try
            {
                int ProcessID = 0;
                var Result = MethodName(Tobj);
                string ResponseValue = Convert.ToString(Result.GetValue("ResponseContent"));
                if (!string.IsNullOrEmpty(ResponseValue?.ToString()) && ResponseValue.ToString() != "[]")
                    return ProcessID = Int32.Parse(ResponseValue);
                else
                    return 0;
            }
            catch
            {

            }
            return 0;
        }

        public static void CallTriggerQueueTable_CASES(ItemTranInfoList itemTranInfo, string dbPath, string _username)
        {
            try
            {
                string ApiUrl = itemTranInfo.METHOD;
                string JSON = itemTranInfo.ITEM_TRAN_INFO;
                int? ProcessID = 0; // this will be come from sql database, it may be entity_ID, case_ID or TransID(quest_form_ID)

                switch (ApiUrl)
                {

                    #region SaveCaseOptimized
                    case SaveCaseOptimized:
                        try
                        {
                            SaveCaseTypeRequest SaveCase_Body_value =
                                             JsonConvert.DeserializeObject<SaveCaseTypeRequest>(JSON);

                            ProcessID = ReturnProcessId<SaveCaseTypeRequest>(JSON, CasesAPIMethods.SaveCaseOptimized, SaveCase_Body_value);
                            if (ProcessID > 0)
                            {
                                UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                                UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                            }

                        }
                        catch (Exception)
                        {
                        }
                        break;
                    #endregion

                    #region CreateCaseOptimized
                    case CreateCaseOptimized:
                        try
                        {
                            // call method from service bus , and this would be in online mode because record should be sync in sql table.
                            // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                            // Pass ProcessID values in fuction which update sqlite object

                            CreateCaseOptimizedRequest.CreateCaseModelOptimized _Body_value =
                                JsonConvert.DeserializeObject<CreateCaseOptimizedRequest.CreateCaseModelOptimized>(JSON);

                            ProcessID = ReturnProcessId<CreateCaseOptimizedRequest.CreateCaseModelOptimized>(JSON, CasesAPIMethods.CreateCaseOptimized, _Body_value);
                            if (ProcessID > 0)
                            {
                                UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                                UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    #endregion

                    #region AddCaseNotes
                    case AddCaseNotes:
                        try
                        {
                            // call method from service bus , and this would be in online mode because record should be sync in sql table.
                            // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                            // Pass ProcessID values in fuction which update sqlite object
                            string NoteProceesid = string.Empty;
                            AddCaseNotesRequest Notes_Body_value =
                                JsonConvert.DeserializeObject<AddCaseNotesRequest>(JSON);

                            var y = DBHelper.GetItemTranInfoListByID(Notes_Body_value.caseID, dbPath);
                            y.Wait();
                            if (y.Result != null)
                            {
                                Notes_Body_value.caseID = y.Result.PROCESS_ID;
                            }

                            ProcessID = ReturnProcessId<AddCaseNotesRequest>(JSON, CasesAPIMethods.AddCaseNotes, Notes_Body_value);
                            // get here case_ID and itemTranInfo
                            //itemTranInfo.ITEM_TRAN_INFO_ID ==update this record with case ID
                            //

                            if (ProcessID > 0)
                            {
                                UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                                UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    #endregion

                    #region CloseCase
                    case CloseCase:
                        try
                        {
                            string CloseCaseProceesid = string.Empty;
                            CloseCaseRequest CloseCase_Body_value =
                                JsonConvert.DeserializeObject<CloseCaseRequest>(JSON);

                            var objCloseCase = DBHelper.GetItemTranInfoListByID(CloseCase_Body_value.caseID, dbPath);
                            objCloseCase.Wait();
                            if (objCloseCase.Result != null)
                            {
                                CloseCase_Body_value.caseID = objCloseCase.Result.PROCESS_ID;
                            }

                            ProcessID = ReturnProcessId<CloseCaseRequest>(JSON, CasesAPIMethods.CloseCase, CloseCase_Body_value);
                            if (ProcessID > 0)
                            {
                                UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                                UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    #endregion

                    #region ReOpenCase
                    case ReOpenCase:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region AssignCase
                    case AssignCase:
                        try
                        {
                            string AssignCaseProceesid = string.Empty;
                            AssignCaseRequest AssignCase_Body_value =
                                JsonConvert.DeserializeObject<AssignCaseRequest>(JSON);

                            var objAssignCase = DBHelper.GetItemTranInfoListByID(AssignCase_Body_value.CaseId, dbPath);
                            objAssignCase.Wait();
                            if (objAssignCase.Result != null)
                            {
                                AssignCase_Body_value.CaseId = objAssignCase.Result.PROCESS_ID;
                            }

                            ProcessID = ReturnProcessId<AssignCaseRequest>(JSON, CasesAPIMethods.AssignCase, AssignCase_Body_value);
                            if (ProcessID > 0)
                            {
                                UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                                UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        break;
                    #endregion

                    #region UnsubscribeFromAlert
                    case UnsubscribeFromAlert:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region SubscribeToAlert
                    case SubscribeToAlert:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;

                    #endregion

                    #region UploadFileToCase
                    case UploadFileToCase:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region SubscribeToCaseAlerts
                    case SubscribeToCaseAlerts:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region SendEmailAlert
                    case SendEmailAlert:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region SaveCase
                    case SaveCase:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region AcceptCase
                    case AcceptCase:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object

                        string AcceptCaseProceesid = string.Empty;
                        AcceptCaseRequest AcceptCase_Body_value =
                            JsonConvert.DeserializeObject<AcceptCaseRequest>(JSON);

                        var objAcceptCase = DBHelper.GetItemTranInfoListByID(AcceptCase_Body_value.CaseId, dbPath);
                        objAcceptCase.Wait();
                        if (objAcceptCase.Result != null)
                        {
                            AcceptCase_Body_value.CaseId = objAcceptCase.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<AcceptCaseRequest>(JSON, CasesAPIMethods.AcceptCase, AcceptCase_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region RemoveFavorite
                    case RemoveFavorite:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region AddFavorite
                    case AddFavorite:
                        AddFavoriteRequest AddFavoriteRequest_Body_value =
                          JsonConvert.DeserializeObject<AddFavoriteRequest>(JSON);

                        ProcessID = ReturnProcessId<AddFavoriteRequest>(JSON, CasesAPIMethods.AddFavorite, AddFavoriteRequest_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region SubscribeToHopper
                    case SubscribeToHopper:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                        break;
                    #endregion

                    #region ReturnCaseToLastAssignee
                    case ReturnCaseToLastAssignee:
                        // call method from service bus , and this would be in online mode because record should be sync in sql table.
                        // Get caseID or entityID, quest form ID from database which is sync and store in  processID variable
                        // Pass ProcessID values in fuction which update sqlite object
                        string ReturnCaseToLastAssigneeProceesid = string.Empty;
                        ReturnCaseToLastAssigneeRequest ReturnCaseToLastAssignee_Body_value =
                            JsonConvert.DeserializeObject<ReturnCaseToLastAssigneeRequest>(JSON);

                        var objReturnCaseToLastAssignee = DBHelper.GetItemTranInfoListByID(ReturnCaseToLastAssignee_Body_value.caseID, dbPath);
                        objReturnCaseToLastAssignee.Wait();
                        if (objReturnCaseToLastAssignee.Result != null)
                        {
                            ReturnCaseToLastAssignee_Body_value.caseID = objReturnCaseToLastAssignee.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<ReturnCaseToLastAssigneeRequest>(JSON, CasesAPIMethods.ReturnCaseToLastAssignee, ReturnCaseToLastAssignee_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region ReturnCaseToLastAssigner
                    case ReturnCaseToLastAssigner:

                        string ReturnCaseToLastAssignerProceesid = string.Empty;
                        ReturnCaseToLastAssignerRequest ReturnCaseToLastAssigner_Body_value =
                            JsonConvert.DeserializeObject<ReturnCaseToLastAssignerRequest>(JSON);

                        var objReturnCaseToLastAssigner = DBHelper.GetItemTranInfoListByID(ReturnCaseToLastAssigner_Body_value.caseid, dbPath);
                        objReturnCaseToLastAssigner.Wait();
                        if (objReturnCaseToLastAssigner.Result != null)
                        {
                            ReturnCaseToLastAssigner_Body_value.caseid = objReturnCaseToLastAssigner.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<ReturnCaseToLastAssignerRequest>(JSON, CasesAPIMethods.ReturnCaseToLastAssigner, ReturnCaseToLastAssigner_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region DeclineAndReturn
                    case DeclineAndReturn:
                        string DeclineAndReturnProceesid = string.Empty;
                        DeclineAndReturnRequest DeclineAndReturn_Body_value =
                            JsonConvert.DeserializeObject<DeclineAndReturnRequest>(JSON);

                        var objDeclineAndReturn = DBHelper.GetItemTranInfoListByID(DeclineAndReturn_Body_value.caseid, dbPath);
                        objDeclineAndReturn.Wait();
                        if (objDeclineAndReturn.Result != null)
                        {
                            DeclineAndReturn_Body_value.caseid = objDeclineAndReturn.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<DeclineAndReturnRequest>(JSON, CasesAPIMethods.DeclienAndReturn, DeclineAndReturn_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region DeclienAndAssign
                    case DeclienAndAssign:
                        string DeclienAndAssignProceesid = string.Empty;
                        DeclineAndAssignToRequest DeclienAndAssign_Body_value =
                            JsonConvert.DeserializeObject<DeclineAndAssignToRequest>(JSON);

                        var objDeclienAndAssign = DBHelper.GetItemTranInfoListByID(DeclienAndAssign_Body_value.caseId, dbPath);
                        objDeclienAndAssign.Wait();
                        if (objDeclienAndAssign.Result != null)
                        {
                            DeclienAndAssign_Body_value.caseId = objDeclienAndAssign.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<DeclineAndAssignToRequest>(JSON, CasesAPIMethods.DeclienAndAssign, DeclienAndAssign_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region ApproveandAssign
                    case ApproveandAssign:

                        string ApproveandAssignProceesid = string.Empty;
                        ApproveCaseRequest ApproveandAssign_Body_value =
                            JsonConvert.DeserializeObject<ApproveCaseRequest>(JSON);

                        var objApproveandAssign = DBHelper.GetItemTranInfoListByID(ApproveandAssign_Body_value.CaseId, dbPath);
                        objApproveandAssign.Wait();
                        if (objApproveandAssign.Result != null)
                        {
                            ApproveandAssign_Body_value.CaseId = objApproveandAssign.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<ApproveCaseRequest>(JSON, CasesAPIMethods.ApproveandAssign, ApproveandAssign_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    #region ApproveandReturn
                    case ApproveandReturn:

                        string ApproveandReturnProceesid = string.Empty;
                        ApproveAndReturnRequest ApproveandReturn_Body_value =
                            JsonConvert.DeserializeObject<ApproveAndReturnRequest>(JSON);

                        var objApproveandReturn = DBHelper.GetItemTranInfoListByID(ApproveandReturn_Body_value.caseid, dbPath);
                        objApproveandReturn.Wait();
                        if (objApproveandReturn.Result != null)
                        {
                            ApproveandReturn_Body_value.caseid = objApproveandReturn.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId<ApproveAndReturnRequest>(JSON, CasesAPIMethods.ApproveandReturn, ApproveandReturn_Body_value);
                        if (ProcessID > 0)
                        {
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.CasesInstance);
                            UpdateCase_C8_C4_Json(itemTranInfo.APP_TYPE_INFO_ID, dbPath, _username, Convert.ToString(ProcessID), itemTranInfo.INSTANCE_USER_ASSOC_ID);
                        }
                        break;
                    #endregion

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void UpdateCase_C8_C4_Json(int APP_TYPE_INFO_ID, string dbPath, string _username, string caseId, int INSTANCE_USER_ASSOC_ID)
        {
            //SaveCase_Body_value.caseId
            var Result1 = DBHelper.GetAppTypeInfoListByPk(APP_TYPE_INFO_ID, dbPath);
            Result1.Wait();
            string _CasetypeId = Convert.ToString(Result1?.Result?.TYPE_ID);

            //    var tm_uname = DBHelper.GetAppTypeInfo_tmname("CASES", Convert.ToInt32(caseId), Convert.ToInt32(_CasetypeId), "C8_GetCaseBasicInfo", dbPath).Result?.TM_Username;
            GetCaseTypesResponse.CaseData lstResulttemp = new GetCaseTypesResponse.CaseData();
            #region Update C8_GetCaseBasicInfo Record

            var CaseBasic = CasesAPIMethods.GetCaseBasicInfo(_username, Convert.ToString(caseId));
            var CaseBasicData = CaseBasic.GetValue("ResponseContent");

            if (!string.IsNullOrEmpty(Convert.ToString(CaseBasicData)))
            {
                List<GetCaseTypesResponse.CaseData> lstResult = new List<GetCaseTypesResponse.CaseData>();
                lstResulttemp = JsonConvert.DeserializeObject<GetCaseTypesResponse.CaseData>(CaseBasicData.ToString());
                lstResult.Add(lstResulttemp);
                if (lstResult.Count > 0)
                {
                    /*vishalpr*/
                    /*To avoid Duplcate Record for C8 */
                    var Record = DBHelper.GetAppTypeInfoList("CASES", lstResulttemp.CaseID, lstResulttemp.CaseTypeID, "C8_GetCaseBasicInfo", dbPath, null);
                    Record.Wait();

                    if (Record.Result == null)
                    {
                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), "CASES", "C8_GetCaseBasicInfo", INSTANCE_USER_ASSOC_ID, dbPath, 0, Convert.ToString(lstResulttemp.CaseTypeID), "M", Convert.ToString(lstResult?.FirstOrDefault()?.CaseID), 0, "", "", true);
                    }
                    else
                    {
                        var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), "CASES", "C8_GetCaseBasicInfo", INSTANCE_USER_ASSOC_ID, dbPath, Record.Result.APP_TYPE_INFO_ID, Convert.ToString(lstResulttemp.CaseTypeID), Record.Result.TransactionType, Convert.ToString(Record.Result.ID), 0, "", "", true);
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
                    // tm_uname = DBHelper.GetAppTypeInfo_tmname("CASES", Convert.ToInt32(caseId), Convert.ToInt32(_CasetypeId), "C4_GetCaseNotes", dbPath).Result?.TM_Username;
                    /*vishalpr*/
                    Task<List<AppTypeInfoList>> onlineRecord = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list("CASES", Convert.ToInt32(lstResulttemp.CaseTypeID), Convert.ToInt32(caseId), dbPath, "C4_GetCaseNotes", "M", null);
                    onlineRecord.Wait();
                    int id = 0;
                    if (onlineRecord?.Result?.Count > 0)
                        id = Convert.ToInt32(onlineRecord.Result?.FirstOrDefault()?.APP_TYPE_INFO_ID);

                    var inserted = CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(lstResult), "CASES", "C4_GetCaseNotes", INSTANCE_USER_ASSOC_ID, dbPath, id, Convert.ToString(lstResulttemp.CaseTypeID), "M", Convert.ToString(caseId));


                    /*it will delete 'T' Entry Opertaion of Notes bcz this recors synced in online - on - Line no:1135*/
                    var collection = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list("CASES", Convert.ToInt32(lstResulttemp.CaseTypeID), Convert.ToInt32(caseId), dbPath, "C4_AddNotes", "T", null)?.Result;

                    if (collection.Count <= 0)
                    {
                        collection = DBHelper.GetAppTypeInfoListByIdTransTypeSyscode_list("CASES", Convert.ToInt32(lstResulttemp.CaseTypeID), Convert.ToInt32(caseId), dbPath, "C4_AddNotes", "T", null)?.Result;
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

        public static void CallTriggerQueueTable_ENTITY(ItemTranInfoList itemTranInfo, string dbPath)
        {
            try
            {
                string ApiUrl = itemTranInfo.METHOD;
                string JSON = itemTranInfo.ITEM_TRAN_INFO;
                int? ProcessID = 0;

                switch (ApiUrl)
                {
                    #region CreateNewEntityItem
                    case CreateNewEntityItem:
                        CreateNewEntityItemRequest SaveEntity_Body_value =
                         JsonConvert.DeserializeObject<CreateNewEntityItemRequest>(JSON);

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.CreateNewEntityItem, SaveEntity_Body_value);
                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        else
                        { }
                        break;
                    #endregion

                    #region SaveEntityItem
                    case SaveEntityItem:
                        CreateNewEntityItemRequest SaveEntityItem_Body_value =
                          JsonConvert.DeserializeObject<CreateNewEntityItemRequest>(JSON);

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.SaveEntityItem, SaveEntityItem_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        break;
                    #endregion

                    #region AddEntityNote
                    case AddEntityNote:
                        AddEntityNoteRequest AddEntityNote_Body_value =
                         JsonConvert.DeserializeObject<AddEntityNoteRequest>(JSON);

                        var objAddEntityNote = DBHelper.GetItemTranInfoListByID(AddEntityNote_Body_value.ENTITY_ID, dbPath);
                        objAddEntityNote.Wait();
                        if (objAddEntityNote.Result != null)
                        {
                            AddEntityNote_Body_value.ENTITY_ID = objAddEntityNote.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.AddEntityNote, AddEntityNote_Body_value);

                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        else
                        { }

                        break;
                    #endregion

                    #region AssignEntity
                    case AssignEntity:
                        string AssignEntityProceesid = string.Empty;
                        AssignEntityRequest AssignEntity_Body_value =
                            JsonConvert.DeserializeObject<AssignEntityRequest>(JSON);

                        var objAssignEntity = DBHelper.GetItemTranInfoListByID(AssignEntity_Body_value.EntityID, dbPath);
                        objAssignEntity.Wait();
                        if (objAssignEntity.Result != null)
                        {
                            AssignEntity_Body_value.EntityID = objAssignEntity.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.AssignEntity, AssignEntity_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        break;
                    #endregion

                    #region TakeOwnership
                    case TakeOwnership:
                        string TakeOwnershipProceesid = string.Empty;
                        TakeOwnershipRequest TakeOwnership_Body_value =
                            JsonConvert.DeserializeObject<TakeOwnershipRequest>(JSON);

                        var objTakeOwnership = DBHelper.GetItemTranInfoListByID(TakeOwnership_Body_value.ENTITY_ID, dbPath);
                        objTakeOwnership.Wait();
                        if (objTakeOwnership.Result != null)
                        {
                            TakeOwnership_Body_value.ENTITY_ID = objTakeOwnership.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.AssignEntity, TakeOwnership_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        break;
                    #endregion

                    #region DeleteEntityItem
                    case DeleteEntityItem:
                        string DeleteEntityItemProceesid = string.Empty;
                        DeleteEntityItemRequest DeleteEntityItem_Body_value =
                            JsonConvert.DeserializeObject<DeleteEntityItemRequest>(JSON);

                        var objDeleteEntityItem = DBHelper.GetItemTranInfoListByID(DeleteEntityItem_Body_value.ENTITY_ID, dbPath);
                        objDeleteEntityItem.Wait();
                        if (objDeleteEntityItem.Result != null)
                        {
                            DeleteEntityItem_Body_value.ENTITY_ID = objDeleteEntityItem.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.DeleteEntityItem, DeleteEntityItem_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        break;
                    #endregion

                    #region AddFileToEntityAssocType
                    case AddFileToEntityAssocType:
                        string AddFileToEntityAssocTypeProceesid = string.Empty;
                        AssignEntityRequest AddFileToEntityAssocType_Body_value =
                            JsonConvert.DeserializeObject<AssignEntityRequest>(JSON);

                        var objAddFileToEntityAssocType = DBHelper.GetItemTranInfoListByID(AddFileToEntityAssocType_Body_value.EntityID, dbPath);
                        objAddFileToEntityAssocType.Wait();
                        if (objAddFileToEntityAssocType.Result != null)
                        {
                            AddFileToEntityAssocType_Body_value.EntityID = objAddFileToEntityAssocType.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.AddFileToEntityAssocType, AddFileToEntityAssocType_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        break;
                    #endregion

                    #region ForwardEntity
                    case ForwardEntity:
                        string ForwardEntityProceesid = string.Empty;
                        ForwardEntityRequest ForwardEntity_Body_value =
                            JsonConvert.DeserializeObject<ForwardEntityRequest>(JSON);

                        var objForwardEntity = DBHelper.GetItemTranInfoListByID(ForwardEntity_Body_value.EntityID, dbPath);
                        objForwardEntity.Wait();
                        if (objForwardEntity.Result != null)
                        {
                            ForwardEntity_Body_value.EntityID = objForwardEntity.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, EntityAPIMethods.AddFileToEntityAssocType, ForwardEntity_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.EntityInstance);
                        break;
                    #endregion


                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void CallTriggerQueueTable_QUEST(ItemTranInfoList itemTranInfo, string dbPath)
        {
            try
            {
                string ApiUrl = itemTranInfo.METHOD;
                string JSON = itemTranInfo.ITEM_TRAN_INFO;
                int? ProcessID = 0;

                switch (ApiUrl)
                {
                    #region Add Form 
                    case AddForm:
                        AddFormRequest AddForm_Body_value =
                         JsonConvert.DeserializeObject<AddFormRequest>(JSON);

                        ProcessID = ReturnProcessId(JSON, QuestAPIMethods.AddForm, AddForm_Body_value);
                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.QuestInstance);

                        break;
                    #endregion

                    #region Update Case Notes To Question 
                    case UpdateCaseNotesToQuestion:

                        GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase GetItemQuestionMetadataCase_Body_value =
                          JsonConvert.DeserializeObject<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>(JSON);

                        UpdateCaseNotesToQuestionRequest UpdateCaseNotesToQuestion_Body_value = new UpdateCaseNotesToQuestionRequest()
                        {
                            itemQuestionMetadataCaseID = GetItemQuestionMetadataCase_Body_value.intItemQuestionMetadataCaseID,
                            modifiedBy = GetItemQuestionMetadataCase_Body_value.usreName,
                            notes = GetItemQuestionMetadataCase_Body_value.strNotes
                        };

                        ProcessID = ReturnProcessId(JSON, QuestAPIMethods.UpdateCaseNotesToQuestion, UpdateCaseNotesToQuestion_Body_value);
                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.QuestInstance);
                        break;
                    #endregion

                    #region AddCaseToQuestion 
                    case AddCaseToQuestion:

                        GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase addGetItemQuestionMetadataCase_Body_value =
                          JsonConvert.DeserializeObject<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>(JSON);

                        AddCaseToQuestionRequest AddCaseToQuestion_Body_value = new AddCaseToQuestionRequest()
                        {
                            createdBy = addGetItemQuestionMetadataCase_Body_value.usreName,
                            itemQuestionFieldID = addGetItemQuestionMetadataCase_Body_value.intItemQuestionMetadataID,
                            notes = addGetItemQuestionMetadataCase_Body_value.strNotes,
                            itemInstanceTranID = Convert.ToInt32(addGetItemQuestionMetadataCase_Body_value.itemInstanceTranID)
                        };

                        var objAddEntityNote = DBHelper.GetItemTranInfoListByID(AddCaseToQuestion_Body_value.itemInstanceTranID, dbPath);
                        objAddEntityNote.Wait();
                        if (objAddEntityNote.Result != null)
                        {
                            AddCaseToQuestion_Body_value.itemInstanceTranID = objAddEntityNote.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, QuestAPIMethods.AddCaseToQuestion, AddCaseToQuestion_Body_value);

                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.QuestInstance);


                        break;
                    #endregion

                    #region AddQuestionsMetadata 
                    case AddQuestionsMetadata:
                        string AssignEntityProceesid = string.Empty;
                        Add_Questions_MetadataRequest AddQuestionsMetadata_Body_value =
                            JsonConvert.DeserializeObject<Add_Questions_MetadataRequest>(JSON);

                        var objAssignEntity = DBHelper.GetItemTranInfoListByID(Convert.ToInt32(AddQuestionsMetadata_Body_value.pITEM_INSTANCE_TRAN_ID), dbPath);
                        objAssignEntity.Wait();
                        if (objAssignEntity.Result != null)
                        {
                            AddQuestionsMetadata_Body_value.pITEM_INSTANCE_TRAN_ID = objAssignEntity.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, QuestAPIMethods.AddQuestionsMetadata, AddQuestionsMetadata_Body_value);
                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.QuestInstance);
                        break;
                    #endregion

                    #region UpdateForm 
                    case UpdateForm:
                        UpdateFormRequest UpdateFormRequest_Body_value =
                            JsonConvert.DeserializeObject<UpdateFormRequest>(JSON);

                        var objUpdateFormRequest = DBHelper.GetItemTranInfoListByID(UpdateFormRequest_Body_value.itemInstanceTranId, dbPath);
                        objUpdateFormRequest.Wait();
                        if (objUpdateFormRequest.Result != null)
                        {
                            UpdateFormRequest_Body_value.itemInstanceTranId = objUpdateFormRequest.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, QuestAPIMethods.UpdateForm, UpdateFormRequest_Body_value);
                        if (ProcessID > 0)
                            UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.QuestInstance);
                        break;
                    #endregion

                    #region DeleteItemQuestionMetadataCase 
                    case DeleteItemQuestionMetadataCase:

                        DeleteItemQuestionMetadataCaseRequest DeleteItemQuestionMetadataCase_Body_value =
                            JsonConvert.DeserializeObject<DeleteItemQuestionMetadataCaseRequest>(JSON);

                        var objDeleteEntityItem = DBHelper.GetItemTranInfoListByID(DeleteItemQuestionMetadataCase_Body_value.itemInstanceTranID, dbPath);
                        objDeleteEntityItem.Wait();
                        if (objDeleteEntityItem.Result != null)
                        {
                            DeleteItemQuestionMetadataCase_Body_value.itemInstanceTranID = objDeleteEntityItem.Result.PROCESS_ID;
                        }

                        ProcessID = ReturnProcessId(JSON, QuestAPIMethods.DeleteItemQuestionMetadataCase, DeleteItemQuestionMetadataCase_Body_value);

                        UpdateProcessIDInSqliteTable(itemTranInfo, dbPath, ProcessID, ConstantsSync.QuestInstance);
                        break;
                    #endregion



                    default:
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public static void UpdateProcessIDInSqliteTable(ItemTranInfoList itemTranInfo, string dbPath, int? processID, string InstanceName = "")
        {
            try
            {
                if (processID != null && processID > 0)
                {
                    SQLiteAsyncConnection db = new SQLiteAsyncConnection(dbPath);
                    Task<List<ItemTranInfoList>> rsAppTypeInfoList = db.Table<ItemTranInfoList>().Where(t => t.ITEM_TRAN_INFO_ID == itemTranInfo.ITEM_TRAN_INFO_ID).ToListAsync();

                    ItemTranInfoList ItemTranInfoListNew = new ItemTranInfoList();

                    #region Update ItemTranInfoList by ProccessId
                    foreach (var item in rsAppTypeInfoList.Result)
                    {
                        ItemTranInfoListNew.INSTANCE_USER_ASSOC_ID = item.INSTANCE_USER_ASSOC_ID;
                        ItemTranInfoListNew.ITEM_TRAN_INFO = item.ITEM_TRAN_INFO;
                        ItemTranInfoListNew.ITEM_TRAN_INFO_ID = item.ITEM_TRAN_INFO_ID;
                        ItemTranInfoListNew.LAST_SYNC_DATETIME = DateTime.Now; // this will be from sql server datatime
                        ItemTranInfoListNew.METHOD = item.METHOD;
                        ItemTranInfoListNew.PROCESS_ID = processID.Value; // assinged new processID here
                        ItemTranInfoListNew.REF_ITEM_TRAN_INFO_ID = item.REF_ITEM_TRAN_INFO_ID;
                        ItemTranInfoListNew.APP_TYPE_INFO_ID = item.APP_TYPE_INFO_ID;
                        ItemTranInfoListNew.ACTION_TYPE = item.ACTION_TYPE;
                        db.UpdateAsync(ItemTranInfoListNew).Wait();
                    }
                    #endregion

                    #region Delete AppInfoList by Id
                    Task<List<AppTypeInfoList>> GetappinfoList = DBHelper.GetAppTypeInfoListByIDSync(InstanceName, Convert.ToInt32(itemTranInfo.ITEM_TRAN_INFO_ID), dbPath);
                    GetappinfoList.Wait();
                    foreach (var ele in GetappinfoList.Result)
                    {
                        if (!string.IsNullOrEmpty(ele.ASSOC_FIELD_INFO))
                        {
                            Task<int> DeletedAppinfo = DBHelper.DeleteAppTypeInfoListById(ele, dbPath);
                            DeletedAppinfo.Wait();
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
