using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class Constants
    {

        public static string _UserName = "";
        public static string Platformtype = "";
        public static string Baseurl = "";// ConnectionAPI.APIURL.ToString();

        public static string access_token = "";
        public static int Selected_Instance = 0;

        public static string Get_Token = "/token";
        public static string User_authenticate = "/api/v1/default/Authenticate";
        public static string GetProfilePicture = "/api/v1/default/GetProfilePicture";
        public static string GetLogo = "/api/v1/default/GetLogo";
        public static string GetImageList = "/api/v1/default/GetImageList";
        public static string Decrypt = "/api/v1/default/Decrypt";
        public static string Encrypt = "/api/v1/default/Encrypt";
        public static string HomeScreenCount = "/api/v1/default/HomeScreenCount";
        public static string AddLog = "/api/v1/Cases/AddLog";



        #region Cases API Lists

        public static string AddCaseNotes = "/api/v1/Cases/AddCaseNotes";
        public static string GetUserInfo = "/api/v1/Cases/GetUserInfo";
        public static string GetCaseNoteTypes = "/api/v1/Cases/GetCaseNoteTypes";
        public static string GetCaseNotes = "/api/v1/Cases/GetCaseNotes";
        public static string GetCaseLastAssignee = "/api/v1/Cases/GetCaseLastAssignee";
        public static string CloseCase = "/api/v1/Cases/CloseCase";
        public static string ReOpenCase = "/api/v1/Cases/ReOpenCase";
        public static string GetCaseListByHopper = "/api/v1/Cases/GetCaseListByHopper";
        public static string GetHopperCenterByUser = "/api/v1/Cases/GetHopperCenterByUser";
        public static string GetAllEmployeeUser = "/api/v1/Cases/GetAllEmployeeUser";
        public static string getCaseTypes = "/api/v1/Cases/getCaseTypes";
        public static string GetOriginationCenterForUser = "/api/v1/Cases/GetOriginationCenterForUser";
        public static string getCaseTypesUserCaseTypeID = "/api/v1/Cases/getCaseTypesUserCaseTypeID";
        public static string GetCaseList = "/api/v1/Cases/GetCaseList";
        public static string GetCaseListSync = "/api/v1/Cases/GetCaseListSync";
        public static string CreateCase = "/api/v1/Cases/CreateCase";
        public static string AssignCase = "/api/v1/Cases/AssignCase";
        public static string GetCaseTypesToCreateByUser = "/api/v1/Cases/GetCaseTypesToCreateByUser";
        public static string GetSystemConfigurationValues = "/api/v1/Cases/GetSystemConfigurationValues";
        public static string UnsubscribeFromAlert = "/api/v1/Cases/UnsubscribeFromAlert";
        public static string SubscribeToAlert = "/api/v1/Cases/SubscribeToAlert";
        public static string GetCaseTypeName = "/api/v1/Cases/GetCaseTypeName";
        public static string GetTypesByCaseTypeID = "/api/v1/Cases/GetTypesByCaseTypeID";
        public static string GetTypeValuesByAssocCaseTypeExternalDS = "/api/v1/Cases/GetTypeValuesByAssocCaseTypeExternalDS";
        public static string GetExdCascadedValue = "/api/v1/Cases/GetExdCascadedValue";
        public static string GetTypeValuesByAssocCaseType = "/api/v1/Cases/GetTypeValuesByAssocCaseType";
        public static string GetTypesByCaseTypeIDRaw = "/api/v1/Cases/GetTypesByCaseTypeIDRaw";

        public static string GetCaseTypeDataByUser = "/api/v1/Cases/GetCaseTypeDataByUser";
        public static string GetTypeValuesByParentID = "/api/v1/Cases/GetTypeValuesByParentID";
        public static string UploadFileToCase = "/api/v1/Cases/UploadFileToCase";
        public static string GetEmailField = "/api/v1/Cases/GetEmailField";
        public static string GetCaseBasicInfo = "/api/v1/Cases/GetCaseBasicInfo";
        public static string CreateCaseOptimized = "/api/v1/Cases/CreateCaseOptimized";
        public static string GetCaseIDByCaseTypeIDAndListID = "/api/v1/Cases/GetCaseIDByCaseTypeIDAndListID";
        public static string GetSearchData = "/api/v1/Cases/GetSearchData";
        public static string EDSCache = "/api/v1/Cases/EDSCache";
        public static string GetCasesForUser = "/api/v1/Cases/GetCasesForUser";
        public static string FormatPhone = "/api/v1/Cases/FormatPhone";
        public static string SubscribeToCaseAlerts = "/api/v1/Cases/SubscribeToCaseAlerts";
        public static string GetPrevousCaseAssignment = "/api/v1/Cases/GetPrevousCaseAssignment";
        public static string SendEmailAlert = "/api/v1/Cases/SendEmailAlert";
        public static string GetSubscriptions = "/api/v1/Cases/GetSubscriptions";
        public static string GetTypesByCaseType = "/api/v1/Cases/GetTypesByCaseType";
        public static string SaveCase = "/api/v1/Cases/SaveCase";
        public static string SaveCaseOptimized = "/api/v1/Cases/SaveCaseOptimized";
        public static string AcceptCase = "/api/v1/Cases/AcceptCase";
        public static string GetEmployeesBySearch = "/api/v1/Cases/GetEmployeesBySearch";
        public static string Index = "/api/v1/Cases/Index";
        public static string GetAllExternalDatasourceList = "/api/v1/Cases/GetAllExternalDatasourceList";
        public static string GetTeamMembers = "/api/v1/Cases/GetTeamMembers";
        public static string GetFavorite = "/api/v1/Cases/GetFavorites";
        public static string EditFavorite = "/api/v1/Cases/EditFavorite";
        public static string RemoveFavorite = "/api/v1/Cases/RemoveFavorite";
        public static string AddFavorite = "/api/v1/Cases/AddFavorite";
        public static string GetExternalDataSourceById = "/api/v1/Cases/GetExternalDataSourceById";
        public static string GetCaseActivity = "/api/v1/Cases/GetCaseActivity";
        public static string RefreshCalculationFields = "/api/v1/Cases/RefreshCalculationFields";
        public static string GetTypesForListByCaseTypeID = "/api/v1/Cases/GetTypesForListByCaseTypeID";
        public static string SubscribeToHopper = "/api/v1/Cases/SubscribeToHopper";
        public static string GetExternalDataSourceItemsById = "/api/v1/Cases/GetExternalDataSourceItemsById";
        public static string ReturnCaseToLastAssignee = "/api/v1/Cases/ReturnCaseToLastAssignee";
        public static string ReturnCaseToLastAssigner = "/api/v1/Cases/ReturnCaseToLastAssigner";
        public static string GetAssocCascadeInfoByCaseType = "/api/v1/Cases/GetAssocCascadeInfoByCaseType";
        public static string GetFilterQuery_Cases = "/api/v1/Cases/GetFilterQuery_Cases";
        public static string GetValuesQueryAndConnection = "/api/v1/Cases/GetValuesQueryAndConnection";
        public static string CasesHomeGetCaseListUser = "/api/v1/Cases/CasesHomeGetCaseListUser";
        public static string DeclineAndReturn = "/api/v1/Cases/DeclineAndReturn";
        public static string DeclienAndAssign = "/api/v1/Cases/DeclienAndAssign";
        public static string ApproveandAssign = "/api/v1/Cases/ApproveCase";
        public static string ApproveandReturn = "/api/v1/Cases/ApproveandReturn";
        public static string GetHopperWithOwnerByUsername = "/api/v1/Cases/GetHopperWithOwnerByUsername";
        public static string GetItemValueFromQueryExd = "/api/v1/Cases/GetItemValueFromQueryExd";
        public static string GetCaseModifiedDate = "/api/v1/Cases/GetCaseModifiedDate";
        public static string GetCaseInfo = "/api/v1/Cases/GetCaseInfo";
        #endregion

        #region Quest API Lists

        public static string GetAreaFormList = "api/v1/Quest/GetAreaFormList";
        public static string GetItemsByAreaIDFormList = "/api/v1/Quest/GetItemsByAreaIDFormList";
        public static string ItemInstance_UpdateLock = "/api/v1/Quest/ItemInstance_UpdateLock";
        public static string ItemInstance_UpdateUnLock = "/api/v1/Quest/ItemInstance_UpdateUnLock";
        public static string GetItemInstanceTran = "/api/v1/Quest/GetItemInstanceTran";
        public static string GetItemInfoFieldsByItemInfoFieldID = "/api/v1/Quest/GetItemInfoFieldsByItemInfoFieldID";
        public static string GetExternalDatasourceByID = "/api/v1/Quest/GetExternalDatasourceByID";
        public static string GetExternalDatasourceResult = "/api/v1/Quest/GetExternalDatasourceResult";
        public static string GetFilesByQuestionID = "/api/v1/Quest/GetFilesByQuestionID";
        public static string DeleteItemInstance = "/api/v1/Quest/DeleteItemInstance";
        public static string AddFiletoQuestion = "/api/v1/Quest/AddFiletoQuestion";
        public static string UpdateFileByQuestionMetadataID_NewForm = "/api/v1/Quest/UpdateFileByQuestionMetadataID_NewForm";
        public static string GetListOfItemsForNewForm = "/api/v1/Quest/GetListOfItemsForNewForm";
        public static string GetItemCategoriesByItemID = "/api/v1/Quest/GetItemCategoriesByItemID";
        public static string GetItemQuestionFieldsByItemCategoryID = "/api/v1/Quest/GetItemQuestionFieldsByItemCategoryID";
        public static string GetItemInfoDependency = "/api/v1/Quest/GetItemInfoDependency";
        public static string GetItemQuestionDecodeByFieldID = "/api/v1/Quest/GetItemQuestionDecodeByFieldID";
        public static string GetItemTranToCheckDuplicate = "/api/v1/Quest/GetItemTranToCheckDuplicate";
        public static string AddForm = "/api/v1/Quest/AddForm";
        public static string AddCaseToQuestion = "/api/v1/Quest/AddCaseToQuestion";
        public static string AddCasetoForm = "/api/v1/Quest/AddCasetoForm";
        public static string GetAreas = "/api/v1/Quest/GetAreas";
        public static string GetItemsByAreaID = "/api/v1/Quest/GetItemsByAreaID";
        public static string GetSecurityData = "/api/v1/Quest/GetSecurityData";
        public static string UpdateForm = "/api/v1/Quest/UpdateForm";
        public static string UpdateCaseNotesToQuestion = "/api/v1/Quest/UpdateCaseNotesToQuestion";
        public static string DeleteItemQuestionMetadataCase = "/api/v1/Quest/DeleteItemQuestionMetadataCase";
        public static string UpdateCaseNotesToForm = "/api/v1/Quest/UpdateCaseNotesToForm";
        public static string UpdateCaseInfoToQuestion = "/api/v1/Quest/UpdateCaseInfoToQuestion";
        public static string DeleteItemTranCase = "/api/v1/Quest/DeleteItemTranCase";
        public static string GetItemInstanceTranData = "/api/v1/Quest/GetItemInstanceTranData";
        public static string GetItemInfoFieldMetaData = "/api/v1/Quest/GetItemInfoFieldMetaData";
        public static string AddQuestionsMetadata = "/api/v1/Quest/AddQuestionsMetadata";
        public static string GetItemInfoFieldsByItemID = "/api/v1/Quest/GetItemInfoFieldsByItemID";
        public static string GetItemQuestionMetadata = "/api/v1/Quest/GetItemQuestionMetadata";
        public static string GetItemCategoriesByItemID_ViewScores = "/api/v1/Quest/GetItemCategoriesByItemID_ViewScores";
        public static string GetItemTranCase = "/api/v1/Quest/GetItemTranCase";
        public static string DeleteFile = "/api/v1/Quest/DeleteFile";
        public static string AddFiletoForm = "/api/v1/Quest/AddFiletoForm";
        public static string GetItemWiseSummaryForScores = "/api/v1/Quest/GetItemWiseSummaryForScores";
        public static string GetItemQuestionFieldsByItemCategoryID_ViewScores = "/api/v1/Quest/GetItemQuestionFieldsByItemCategoryID_ViewScores";
        public static string GetItemQuestionFieldsByItemCategoryIDviewScores = "/api/v1/Quest/GetItemQuestionFieldsByItemCategoryIDviewScores";
        public static string UpdateItemInstanceTranCategory = "/api/v1/Quest/UpdateItemInstanceTranCategory";
        public static string GetCountForQuestItem = "/api/v1/Quest/GetCountForQuestItem";
        public static string GetItemQuestionMetadataCase = "/api/v1/Quest/GetItemQuestionMetadataCase";
        public static string DeleteFileToQuestion = "/api/v1/Quest/DeleteFileToQuestion";
        public static string BoxerCentralHome_GetQuestFormsForUser = "/api/v1/Quest/BoxerCentralHome_GetQuestFormsForUser";
        public static string GetFilterQuery_Quest = "/api/v1/Quest/GetFilterQuery_Quest";
        public static string GetExternalDatasourceInfoByID = "/api/v1/Quest/GetExternalDatasourceInfoByID";
        public static string GetExternalDatasourceByQuery = "/api/v1/Quest/GetExternalDatasourceByQuery";
        public static string GetQuestItemByStatus = "/api/v1/Quest/GetQuestItemByStatus";
        public static string GenerateCaseQueue = "/api/v1/Quest/GenerateCaseQueue";


        #endregion

        #region Entity API Lists

        #region Entity Custom API List
        public static string GetCustomTemplateTags = "/api/v1/Entity/GetCustomTemplateTags";
        public static string GetCustomTemplateTagsByAttribute = "/api/v1/Entity/GetCustomTemplateTagsByAttribute";
        public static string GetEntityRelatedApplications = "/api/v1/Entity/GetEntityRelatedApplications";
        public static string GetEntityRelatedTypes = "/api/v1/Entity/GetEntityRelatedTypes";
        public static string GetEntityRoleRelationData = "/api/v1/Entity/GetEntityRoleRelationData";
        public static string GetEntityTypeRelationData = "/api/v1/Entity/GetEntityTypeRelationData";
        public static string GetCasesRelationData = "/api/v1/Entity/GetCasesRelationData";
        public static string GetEntitiesRelationData = "/api/v1/Entity/GetEntitiesRelationData";
        public static string GetQuestRelationData = "/api/v1/Entity/GetQuestRelationData";
        public static string GetTriggerGridData = "/api/v1/Entity/GetTriggerGridData";
        #endregion

        public static string AddEntityNote = "/api/v1/Entity/AddEntityNote";
        public static string AddFileToEntity = "/api/v1/Entity/AddFileToEntity";
        public static string GetAssociatedEntityList = "/api/v1/Entity/GetAssociatedEntityList";
        public static string AssignEntity = "/api/v1/Entity/AssignEntity";
        public static string GetEntityByAssingToSAM = "/api/v1/Entity/GetEntityByAssingToSAM";
        public static string CreateNewEntityItem = "/api/v1/Entity/CreateNewEntityItem";
        public static string EntityDeleteFile = "/api/v1/Entity/DeleteFile";
        public static string ForwardEntity = "/api/v1/Entity/ForwardEntity";
        public static string GetActivity = "/api/v1/Entity/GetActivity";
        public static string GetEntitiesBySystemCodeKeyValuePair = "/api/v1/Entity/GetEntitiesBySystemCodeKeyValuePair";
        public static string GetEntityTypeCategoryList = "/api/v1/Entity/GetEntityTypeCategoryList";
        public static string GetEntityTypeList = "/api/v1/Entity/GetEntityTypeList";
        public static string GetEntityTypeSchemaByEntityTypeID = "/api/v1/Entity/GetEntityTypeSchemaByEntityTypeID";
        public static string GetExternalDataSourceByName = "/api/v1/Entity/GetExternalDataSourceByName";
        public static string GetExternalDataSourceByID = "/api/v1/Entity/GetExternalDataSourceByID";
        public static string GetFilesByFileID = "/api/v1/Entity/GetFilesByFileID";
        public static string GetNotes = "/api/v1/Entity/GetNotes";
        public static string GetNoteTypes = "/api/v1/Entity/GetNoteTypes";
        public static string SaveEntityItem = "/api/v1/Entity/SaveEntityItem";
        public static string TakeOwnership = "/api/v1/Entity/TakeOwnership";
        public static string UpdateEntityIDForFile = "/api/v1/Entity/UpdateEntityIDForFile";
        public static string UserNameLookup = "/api/v1/Entity/UserNameLookup";
        public static string GetEntitiesToCalculate = "/api/v1/Entity/GetEntitiesToCalculate";
        public static string SetCalculationValues = "/api/v1/Entity/SetCalculationValues";
        public static string DeleteEntityItem = "/api/v1/Entity/DeleteEntityItem";
        public static string GetEntityByEntityID = "/api/v1/Entity/GetEntityByEntityID";
        public static string WS_GetEntityTypeList = "/api/v1/Entity/WS_GetEntityTypeList";
        public static string WS_GetEntityTypeSchemaByEntityTypeID = "/api/v1/Entity/WS_GetEntityTypeSchemaByEntityTypeID";
        public static string AddFileToEntityAssocType = "/api/v1/Entity/AddFileToEntityAssocType";
        public static string GetEntityTypeConfiguration = "/api/v1/Entity/GetEntityTypeConfiguration";
        public static string GetEntityTypeDetail = "/api/v1/Entity/GetEntityTypeDetail";
        public static string RefreshCalculationFieldsEntity = "/api/v1/Entity/RefreshCalculationFields";
        public static string GetEntitiesBySystemCodeKeyValuePair_LazyLoad = "/api/v1/Entity/GetEntitiesBySystemCodeKeyValuePair_LazyLoad";
        public static string ExternalDatasourceByQuery = "/api/v1/Entity/ExternalDatasourceByQuery";
        public static string GetFilterQuery_Entity = "/api/v1/Entity/GetFilterQuery_Entity";
        public static string BoxerCentralHome_Entities_GetEntityList = "/api/v1/Entity/BoxerCentralHome_Entities_GetEntityList";
        public static string GetAllEntityRoleRelationshipByEmp = "/api/v1/Entity/GetAllEntityRoleRelationshipByEmp";
        public static string EntityBasicDetails = "/api/v1/Entity/EntityBasicDetails";
        #endregion

        #region  Standarad APIs List       

        public static string GetBookList = "/api/v1/Standards/GetBookDetailToUser";
        public static string WebView = "/api/v1/Standards/GetAppMetadataByParentAppForUser_Sync";
        public static string BookView = "/api/v1/Standards/GetAppMetadataByParentAppForUser_BookView_Sync";
        public static string security_GetAllAppForUser = "/api/v1/Standards/security_GetAllAppForUser";
        public static string GetBookCreatedByUser = "/api/v1/Standards/security_GetAppCreatedByUserBasedOnSAM";
        public static string GetPublishedBookByUser = "/api/v1/Standards/security_GetPublishedAppByUserBasedOnSAM";
        public static string security_SearchMetadata = "/api/v1/Standards/security_SearchMetadata";
        public static string GetBookRelateToUser = "/api/v1/Standards/security_GetAppRelateToUserBasedOnSAM";

        #endregion

        #region  Login API      

        public static string LoginAuthenticate = "/api/v1/Default/Authenticate";


        #endregion

        #region Common Method For Class ParaMeter
        public static JObject ApiCommon(object Body_value, string ApiName)
        {
            try
            {
                #region API Details
                var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Baseurl  + ApiName)
            };
                #endregion

                var Result = MobileAPIMethods.CallAPIGetPostList(API_value, Body_value);
                if (Result != null)
                {
                    return JObject.Parse(Result.ToString());
                }
                else
                    return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
