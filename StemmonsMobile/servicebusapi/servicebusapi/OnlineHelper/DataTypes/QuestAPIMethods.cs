using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Quest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class QuestAPIMethods
    {

        public static JObject GetFilesByQuestionID(string itemQuestionMetadataID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl + Constants.GetFilesByQuestionID),
            };

            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemQuestionMetaDataID",itemQuestionMetadataID)
            };
            #endregion

            var val = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (val != null)
            {
                return val;
            }
            else
                return null;

        }

        #region Get Area FormList
        public static JObject GetAreaFormList(string _userName, string _areaId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetAreaFormList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user", _userName),
                new KeyValuePair<string, string>("areaId", _areaId),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ItemsBy AreaID FormList
        public static JObject GetItemsByAreaIDFormList(string _intAreaID, string _userName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemsByAreaIDFormList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intAreaID", _intAreaID),
                new KeyValuePair<string, string>("user", _userName),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region getConnectionString
        public static JObject GetFilterQuery_Quest(string ExternalDataSourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetFilterQuery_Quest)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ExternalDataSourceID", ExternalDataSourceID.ToString())

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Item Instance UpdateLock
        public static JObject ItemInstance_UpdateLock(string _itemInstanceTranID, string _modifiedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.ItemInstance_UpdateLock)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("modifiedBy", _modifiedBy),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region ItemInstance UpdateUnLock
        public static JObject ItemInstance_UpdateUnLock(string _itemInstanceTranID, string _modifiedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.ItemInstance_UpdateUnLock)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("modifiedBy", _modifiedBy),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion 

        #region Get Item Instance Tran
        public static JObject GetItemInstanceTran(string _itemInstanceTranID, string _itemId, string _isIncludeClosedItems, string _fromDate, string _toDate, string _itemInstanceTransIDs, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemInstanceTran)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("itemId", _itemId),
                new KeyValuePair<string, string>("isIncludeClosedItems", _isIncludeClosedItems),
                new KeyValuePair<string, string>("fromDate", _fromDate),
                new KeyValuePair<string, string>("toDate", _toDate),
                new KeyValuePair<string, string>("itemInstanceTransIDs", _itemInstanceTransIDs),
                new KeyValuePair<string, string>("username", username)

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion 

        #region GetQuestItemByStatus
        public static JObject GetQuestItemByStatus(string _ItemID, string _User, string _StatusType)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetQuestItemByStatus)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ItemID", _ItemID),
                new KeyValuePair<string, string>("User", _User),
                new KeyValuePair<string, string>("StatusType", _StatusType)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Generate Case Queue
        public static JObject GenerateCaseQueue(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GenerateCaseQueue);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


        #region Get ItemInfoFields By ItemInfoField ID
        public static JObject GetItemInfoFieldsByItemInfoFieldID(string _intItemInfoFieldID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemInfoFieldsByItemInfoFieldID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intItemInfoFieldID", _intItemInfoFieldID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion 

        #region Get External Datasource By ID
        public static JObject GetExternalDatasourceByID(string _externalDataSourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetExternalDatasourceByID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("externalDataSourceID", _externalDataSourceID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion


        #region GetExternalDatasourceInfoByID
        public static JObject GetExternalDatasourceInfoByID(string _externalDataSourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetExternalDatasourceInfoByID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("externalDataSourceID", _externalDataSourceID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ExternalDatasource Result
        public static JObject GetExternalDatasourceResult(string _strConnString_External, string _strQuery)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetExternalDatasourceResult)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("strConnString_External", _strConnString_External),
                new KeyValuePair<string, string>("strQuery", _strQuery)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Delete Item Instance
        public static JObject DeleteItemInstance(string _ItemInstanceTranID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.DeleteItemInstance)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ItemInstanceTranID", _ItemInstanceTranID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Add File to Question
        //public static JObject AddFiletoQuestion(int itemInstanceTranID, int itemQuestionFieldId, string fileType, string fileName, string fileSizeBytes, byte[] fileBinary, string createdBy)
        public static JObject AddFiletoQuestion(object _BodyValue)
        {

            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.AddFiletoQuestion)
            };
            #endregion

            //AddFiletoQuestionRequest addfile = new AddFiletoQuestionRequest();

            //addfile.itemInstanceTranID = Convert.ToInt32(itemInstanceTranID);
            //addfile.itemQuestionFieldId = itemQuestionFieldId;
            //addfile.fileType = fileType;
            //addfile.fileName = fileName;
            //addfile.fileSizeBytes = Convert.ToInt32(fileSizeBytes);
            //addfile.fileBinary = fileBinary;
            //addfile.createdBy = createdBy;

            //addfile. = Convert.ToInt32(ENTITY_ID);
            //addfile.DESCRIPTION = DESCRIPTION.ToString();
            //addfile.FILE_DATE_TIME = DateTime.Now;
            //addfile.FILE_NAME = FILE_NAME;
            //addfile.FILE_SIZE_BYTES = Convert.ToInt32(FILE_SIZE_BYTES);
            //addfile.FILE_BLOB = FILE_BLOB;
            //addfile.EXTERNAL_URI = EXTERNAL_URI;
            //addfile.SHOW_INLINE_NOTES = SHOW_INLINE_NOTES;
            //addfile.SYSTEM_CODE = SYSTEM_CODE;
            //addfile.IS_ACTIVE = IS_ACTIVE;
            //addfile.CREATED_BY = CREATED_BY;

            var Result = Constants.ApiCommon(_BodyValue, Constants.AddFiletoQuestion);


            //#region API Body Details
            //var Body_value = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
            //    new KeyValuePair<string, string>("itemQuestionFieldId", _itemQuestionFieldId),
            //    new KeyValuePair<string, string>("fileType", _fileType),
            //    new KeyValuePair<string, string>("createdBy", _createdBy)

            //};
            //#endregion

            //var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Update File By QuestionMetadataID NewForm
        public static JObject UpdateFileByQuestionMetadataID_NewForm(string _fileID, string _itemQuestionFieldID, string _itemInstanceTransID, string _createdBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.UpdateFileByQuestionMetadataID_NewForm)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("fileID", _fileID),
                new KeyValuePair<string, string>("itemQuestionFieldID", _itemQuestionFieldID),
                new KeyValuePair<string, string>("itemInstanceTransID", _itemInstanceTransID),
                new KeyValuePair<string, string>("createdBy", _createdBy)

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get List Of Items For New Form
        public static JObject GetListOfItemsForNewForm(string _intAreaID, string _user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetListOfItemsForNewForm)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intAreaID", _intAreaID),
                new KeyValuePair<string, string>("user", _user)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Categories By ItemID
        public static JObject GetItemCategoriesByItemID(string _itemID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemCategoriesByItemID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemID", _itemID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ItemQuestionFields By ItemCategoryID
        public static JObject GetItemQuestionFieldsByItemCategoryID(string _itemCategoryID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemQuestionFieldsByItemCategoryID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemCategoryID", _itemCategoryID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ItemInfo Dependency
        public static JObject GetItemInfoDependency(string _itemInfoFieldIDChild, string _itemid)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemInfoDependency)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemID", _itemid),
                new KeyValuePair<string, string>("itemInfoFieldIDChild", _itemInfoFieldIDChild)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ItemInfo Dependency
        public static JObject GetExternalDatasourceByQuery(string Query, string Connectionstring)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetExternalDatasourceByQuery)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("Query", Query),
                 new KeyValuePair<string, string>("Connectionstring", Connectionstring)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Question Decode By FieldID
        public static JObject GetItemQuestionDecodeByFieldID(string _itemQuestionFieldID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemQuestionDecodeByFieldID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemQuestionFieldID", _itemQuestionFieldID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Tran To Check Duplicate
        public static JObject GetItemTranToCheckDuplicate(string _itemInfoFieldID, string _value)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemTranToCheckDuplicate)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInfoFieldID", _itemInfoFieldID),
                new KeyValuePair<string, string>("value", _value)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region AddForm
        public static JObject AddForm(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddForm);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region AddCaseToQuestion
        public static JObject AddCaseToQuestion(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddCaseToQuestion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Add Case to Form
        public static JObject AddCasetoForm(string _itemInstanceTranID, string _notes, string _createdBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.AddCasetoForm)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("notes", _notes),
                new KeyValuePair<string, string>("createdBy", _createdBy),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Areas
        public static JObject GetAreas(string _areaId, string _user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetAreas)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("areaId", _areaId),
                new KeyValuePair<string, string>("user", _user)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Items By AreaID
        public static JObject GetItemsByAreaID(string _intAreaID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemsByAreaID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intAreaID", _intAreaID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Security Data
        public static JObject GetSecurityData(string _itemInstanceTranId, string _user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetSecurityData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranId", _itemInstanceTranId),
                new KeyValuePair<string, string>("user", _user)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region UpdateForm
        public static JObject UpdateForm(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.UpdateForm);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Update Case Notes To Question
        public static JObject UpdateCaseNotesToQuestion(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.UpdateCaseNotesToQuestion);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Delete Item Question Metadata Case
        public static JObject DeleteItemQuestionMetadataCase(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.DeleteItemQuestionMetadataCase);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Update Case Notes To Form
        public static JObject UpdateCaseNotesToForm(string _itemInstanceTranCaseId, string _notes, string _modifiedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.UpdateCaseNotesToForm)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranCaseId", _itemInstanceTranCaseId),
                new KeyValuePair<string, string>("notes", _notes),
                new KeyValuePair<string, string>("modifiedBy", _modifiedBy),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Update Case Info To Question
        public static JObject UpdateCaseInfoToQuestion(string _itemQuestionMetadataCaseId, string _caseId, string _modifiedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.UpdateCaseInfoToQuestion)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemQuestionMetadataCaseId", _itemQuestionMetadataCaseId),
                new KeyValuePair<string, string>("caseId", _caseId),
                new KeyValuePair<string, string>("modifiedBy", _modifiedBy),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Delete Item TranCase
        public static JObject DeleteItemTranCase(string _itemInstanceTranID, string _itemInstanceTranCaseID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.DeleteItemTranCase)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("itemInstanceTranCaseID", _itemInstanceTranCaseID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Instance Tran Data
        public static JObject GetItemInstanceTranData(string _intItemInstanceTranID, string _intItemID, string _intListID, string _blnIsEdit, string _strItemName,
            string _strOtherComments, string _intAreaID, string _strAreaName, string _strSupervisorEmail, string _blnSuppressAlert, string _blnHidePoints,
            string _strExtraField1, string _strCreatedBy, string _dtCreatedDateTime, string _strModifiedby, string _dtModifiedDateTime)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemInstanceTranData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intItemInstanceTranID", _intItemInstanceTranID),
                new KeyValuePair<string, string>("intItemID", _intItemID),
                new KeyValuePair<string, string>("intListID", _intListID),
                new KeyValuePair<string, string>("blnIsEdit", _blnIsEdit),
                new KeyValuePair<string, string>("strItemName", _strItemName),
                new KeyValuePair<string, string>("strOtherComments", _strOtherComments),
                new KeyValuePair<string, string>("intAreaID", _intAreaID),
                new KeyValuePair<string, string>("strAreaName", _strAreaName),
                new KeyValuePair<string, string>("strSupervisorEmail", _strSupervisorEmail),
                new KeyValuePair<string, string>("blnSuppressAlert", _blnSuppressAlert),
                new KeyValuePair<string, string>("blnHidePoints", _blnHidePoints),
                new KeyValuePair<string, string>("strExtraField1", _strExtraField1),
                new KeyValuePair<string, string>("strCreatedBy", _strCreatedBy),
                new KeyValuePair<string, string>("dtCreatedDateTime", _dtCreatedDateTime),
                new KeyValuePair<string, string>("strModifiedby", _strModifiedby),
                new KeyValuePair<string, string>("dtModifiedDateTime", _dtModifiedDateTime)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Info Field MetaData
        public static JObject GetItemInfoFieldMetaData(string _itemInstanceTranID, string _itemInfoFieldIDs, string _showOnPage, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemInfoFieldMetaData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("itemInfoFieldIDs", _itemInfoFieldIDs),
                new KeyValuePair<string, string>("showOnPage", _showOnPage),
                new KeyValuePair<string, string>("username", username)

            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ItemInfoFields By ItemID
        public static JObject GetItemInfoFieldsByItemID(string _itemID, string _showOnPage)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemInfoFieldsByItemID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemID", _itemID),
                new KeyValuePair<string, string>("showOnPage", _showOnPage)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Question Metadata
        public static JObject GetItemQuestionMetadata(string _intItemInstanceTranID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemQuestionMetadata)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intItemInstanceTranID", _intItemInstanceTranID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item Question Metadata
        public static JObject GetItemQuestionMetadataCase(string _itemQuestionMetadataID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemQuestionMetadataCase)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemQuestionMetadataID", _itemQuestionMetadataID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region GetItemCategoriesByItemID_ViewScores
        public static JObject GetItemCategoriesByItemID_ViewScores(string _itemInstanceTranID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemCategoriesByItemID_ViewScores)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get ItemTran Case
        public static JObject GetItemTranCase(string _intItemInstanceTranID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemTranCase)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("intItemInstanceTranID", _intItemInstanceTranID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Delete File
        public static JObject DeleteFile(string _itemInstanceTranFileID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.DeleteFile)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranFileID", _itemInstanceTranFileID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region AddFiletoForm
        public static JObject AddFiletoForm(string intItemInstanceTranID, string strDescription, string _intItemInstanceTranID, string strFileName, string intFileSizeBytes, byte[] FileBinary, string strCreatedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.AddFiletoForm)
            };
            #endregion

            //           #region API Body Details
            //           var Body_value = new List<KeyValuePair<string, string>>
            //           {
            //               new KeyValuePair<string, string>("intItemInstanceTranID", intItemInstanceTranID),
            //new KeyValuePair<string, string>("strDescription", strDescription),
            //new KeyValuePair<string, string>("strFileName", strFileName),
            //new KeyValuePair<string, string>("intFileSizeBytes", intFileSizeBytes),
            //new KeyValuePair<string, string>("FileBinary", FileBinary),
            //new KeyValuePair<string, string>("strCreatedBy", strCreatedBy),



            //   };
            //           #endregion


            AddFiletoFormRequest addfile = new AddFiletoFormRequest();

            addfile.intItemInstanceTranID = Convert.ToInt32(intItemInstanceTranID);
            addfile.strDescription = strDescription;
            addfile.strFileName = strFileName;
            addfile.intFileSizeBytes = Convert.ToInt32(intFileSizeBytes);
            addfile.FileBinary = FileBinary;
            addfile.strCreatedBy = strCreatedBy;

            //addfile. = Convert.ToInt32(ENTITY_ID);
            //addfile.DESCRIPTION = DESCRIPTION.ToString();
            //addfile.FILE_DATE_TIME = DateTime.Now;
            //addfile.FILE_NAME = FILE_NAME;
            //addfile.FILE_SIZE_BYTES = Convert.ToInt32(FILE_SIZE_BYTES);
            //addfile.FILE_BLOB = FILE_BLOB;
            //addfile.EXTERNAL_URI = EXTERNAL_URI;
            //addfile.SHOW_INLINE_NOTES = SHOW_INLINE_NOTES;
            //addfile.SYSTEM_CODE = SYSTEM_CODE;
            //addfile.IS_ACTIVE = IS_ACTIVE;
            //addfile.CREATED_BY = CREATED_BY;

            var Result = Constants.ApiCommon(addfile, Constants.AddFiletoForm);
            // var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        //#region Add File to Form
        //public static JObject AddFiletoForm(object Body_value)
        //{

        //    try
        //    {
        //        return Constants.ApiCommon(Body_value, Constants.AddFiletoForm);
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        //#endregion

        #region Get Item Wis eSummary For Scores
        public static JObject GetItemWiseSummaryForScores(string _areaID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemWiseSummaryForScores)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("areaID", _areaID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item QuestionFields By ItemCategoryID ViewScores
        public static JObject GetItemQuestionFieldsByItemCategoryID_ViewScores(string _itemInstanceTranID, string _itemCategoryID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemQuestionFieldsByItemCategoryID_ViewScores)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranID", _itemInstanceTranID),
                new KeyValuePair<string, string>("itemCategoryID", _itemCategoryID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Item QuestionFields By ItemCategoryID
        public static JObject GetItemQuestionFieldsByItemCategoryIDviewscores(string _itemCategoryID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetItemQuestionFieldsByItemCategoryIDviewScores)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemCategoryID", _itemCategoryID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Update ItemInstance Tran Category
        public static JObject UpdateItemInstanceTranCategory(string _itemInstanceTranId, string _itemCategoryIds, string _scoreValues, string _overallScore, string _modifiedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.UpdateItemInstanceTranCategory)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("itemInstanceTranId", _itemInstanceTranId),
                new KeyValuePair<string, string>("itemCategoryIds", _itemCategoryIds),
                new KeyValuePair<string, string>("scoreValues", _scoreValues),
                new KeyValuePair<string, string>("overallScore", _overallScore),
                new KeyValuePair<string, string>("modifiedBy", _modifiedBy),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion



        #region Add_Questions_Metadata
        public static JObject AddQuestionsMetadata(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddQuestionsMetadata);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get Item QuestionFields By ItemCategoryID ViewScores
        public static JObject GetCountForQuestItem(string _ItemId, string _User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetCountForQuestItem)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ItemId", _ItemId),
                new KeyValuePair<string, string>("User", _User)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Delete File To Question
        public static JObject DeleteFileToQuestion(string _FileID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.DeleteFileToQuestion),
            };

            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FileID",_FileID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;

        }
        #endregion


        #region BoxerCentralHome_GetQuestFormsForUser
        public static JObject BoxerCentralHome_GetQuestFormsForUser(string USER_SAM, string RecordCount, string Count_Option)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                 new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.BoxerCentralHome_GetQuestFormsForUser),
            };

            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("USER_SAM",USER_SAM),
                new KeyValuePair<string, string>("RecordCount",RecordCount),
                new KeyValuePair<string, string>("Count_Option",Count_Option)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;

        }
        #endregion


        #region Get Favorite
        public static JObject GetFavorite(string _CreatedBy, string _ApplicationId, string _AreaId, string _TypeId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.GetFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("CreatedBy", _CreatedBy),
                new KeyValuePair<string, string>("ApplicationId", _ApplicationId),
                new KeyValuePair<string, string>("AreaId", _AreaId),
                new KeyValuePair<string, string>("TypeId", _TypeId),
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Edit Favorite
        public static JObject EditFavorite(string _FavoriteID, string AppID, string _FavoriteName, string _FieldValues, string _IsActive, string _CreatedBy, string _CreatedByDt, string _ModifiedByDt, string _LastSyncDt, string QuestAreaID, string pTypeId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.EditFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FavoriteID", _FavoriteID),
                new KeyValuePair<string, string>("AppTypeInfoId", AppID),
                new KeyValuePair<string, string>("FavoriteName", _FavoriteName),
                new KeyValuePair<string, string>("FieldValues", _FieldValues),
                new KeyValuePair<string, string>("IsActive", _IsActive),
                new KeyValuePair<string, string>("CreatedBy", _CreatedBy),
                new KeyValuePair<string, string>("CreatedByDt", _CreatedByDt),
                new KeyValuePair<string, string>("ModifiedByDt", _ModifiedByDt),
                new KeyValuePair<string, string>("LastSyncDt", _LastSyncDt),
                   new KeyValuePair<string, string>("QuestAreaID", QuestAreaID),
                new KeyValuePair<string, string>("pTypeId", pTypeId)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Remove Favorite
        public static JObject RemoveFavorite(string _FavoriteID, string username)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.RemoveFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FavoriteID", _FavoriteID),
                 new KeyValuePair<string, string>("username", username)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region DeleteFavorite
        public static JObject RemoveFavorite(string _FavoriteID, string _AppTypeInfoId, string _FavoriteName, string _FieldValues, string _IsActive, string _CreatedBy, string _CreatedByDt, string _ModifiedByDt, string _LastSyncDt)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name",Constants.Baseurl + Constants.RemoveFavorite)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FavoriteID", _FavoriteID)
            };
            #endregion

            var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Add Favorite
        public static JObject AddFavorite(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddFavorite);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion


    }
}
