using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class EntityAPIMethods
    {
        #region Add Entity Notes
        public static JObject AddEntityNote(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AddEntityNote);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Add Entity file 
        public static JObject AddFileToEntity(string ENTITY_ID, string DESCRIPTION, string FILE_DATE_TIME, string FILE_NAME, string FILE_SIZE_BYTES, byte[] FILE_BLOB, string EXTERNAL_URI, Char SHOW_INLINE_NOTES, string SYSTEM_CODE, Char IS_ACTIVE, string CREATED_BY)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.AddFileToEntity)
            };
            #endregion

            #region API Body Details

            AddFileToEntityRequest addfile = new AddFileToEntityRequest();
            addfile.ENTITY_ID = Convert.ToInt32(ENTITY_ID);
            addfile.DESCRIPTION = DESCRIPTION.ToString();
            addfile.FILE_DATE_TIME = DateTime.Now;
            addfile.FILE_NAME = FILE_NAME;
            addfile.FILE_SIZE_BYTES = Convert.ToInt32(FILE_SIZE_BYTES);
            addfile.FILE_BLOB = FILE_BLOB;
            addfile.EXTERNAL_URI = EXTERNAL_URI;
            addfile.SHOW_INLINE_NOTES = SHOW_INLINE_NOTES;
            addfile.SYSTEM_CODE = SYSTEM_CODE;
            addfile.IS_ACTIVE = IS_ACTIVE;
            addfile.CREATED_BY = CREATED_BY;
            #endregion

            var Result = Constants.ApiCommon(addfile, Constants.AddFileToEntity);
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Assign Entity
        public static JObject AssignEntity(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.AssignEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Forward Entity
        public static JObject ForwardEntity(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.ForwardEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetEntityByAssingToSAM
        public static JObject GetEntityByAssingToSAM(string user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityByAssingToSAM)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user",user),

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

        #region Create New Entity Item
        public static JObject CreateNewEntityItem(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.CreateNewEntityItem);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Assign Entity
        public static JObject DeleteFile(string FileID, string CurrentUser)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.EntityDeleteFile)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("FileID", FileID.ToString()),
                 new KeyValuePair<string, string>("CurrentUser", CurrentUser),

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

        #region Get Activity
        public static JObject GetActivity(string ENTITY_ID, string USER)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetActivity)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ENTITY_ID", ENTITY_ID.ToString()),
                 new KeyValuePair<string, string>("USER", USER),

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

        #region Get Entities By System Code Key Value Pair
        public static JObject GetEntitiesBySystemCodeKeyValuePair(string entityTypeId, string count)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntitiesBySystemCodeKeyValuePair)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("entityTypeId", entityTypeId.ToString()),
                 new KeyValuePair<string, string>("count", count.ToString()),

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

        #region Get Entity Type Category List
        public static JObject GetEntityTypeCategoryList(string user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeCategoryList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user", user.ToString())
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

        #region Get Entity Type List
        public static JObject GetEntityTypeList(string CategoryId, string user)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("user", user.ToString()),
                  new KeyValuePair<string, string>("CategoryId", CategoryId)
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

        #region Get Entity Type Schema By EntityTypeID
        public static JObject GetEntityTypeSchemaByEntityTypeID(string ENTITY_TYPE_ID, string USER)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeSchemaByEntityTypeID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ENTITY_TYPE_ID", ENTITY_TYPE_ID.ToString()),
                  new KeyValuePair<string, string>("USER", USER.ToString())
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

        #region Get External Data Source By Name
        public static JObject GetExternalDataSourceByName(string externalDatasourceName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetExternalDataSourceByName)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("externalDatasourceName", externalDatasourceName.ToString())
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

        #region Get External Data Source By ID
        public static JObject GetExternalDataSourceByID(string externalDatasourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetExternalDataSourceByID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("externalDatasourceID", externalDatasourceID.ToString())
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

        #region Get Files By FileID
        public static JObject GetFilesByFileID(string externalDatasourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetFilesByFileID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("externalDatasourceID", externalDatasourceID.ToString())
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

        #region Get Notes
        public static JObject GetNotes(string UserName, string EntityId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetNotes)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                 new KeyValuePair<string, string>("UserName", UserName.ToString())
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

        #region Save EntityItem

        public static JObject SaveEntityItem(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.SaveEntityItem);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get Note Types
        public static JObject GetNoteTypes(string UserName, string EntityId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetNoteTypes)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
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

        #region TakeOwnership
        public static JObject TakeOwnership(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.TakeOwnership);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #region API Details
            //var API_value = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.TakeOwnership)
            //};


            //#region API Body Details
            //var Body_value = new List<KeyValuePair<string, string>>
            //{
            //    new KeyValuePair<string, string>("ENTITY_ID", ENTITY_ID.ToString()),
            //     new KeyValuePair<string, string>("USER", USER.ToString())
            //};
            //#endregion

            //var Result = MobileAPIMethods.CallAPIGetPost(API_value, Body_value, "POST");
            //if (Result != null)
            //{
            //    return Result;
            //}
            //else
            //    return null;
            #endregion
        }
        #endregion

        #region Update EntityID For File
        public static JObject UpdateEntityIDForFile(string EntityFileId, string EntityId, string ModifiedBy)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.UpdateEntityIDForFile)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityFileId", EntityFileId.ToString()),
                 new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                 new KeyValuePair<string, string>("ModifiedBy", ModifiedBy.ToString())
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

        #region UserName Lookup
        public static JObject UserNameLookup(string SEARCH_STRING)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.UserNameLookup)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityFileId", SEARCH_STRING.ToString())
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

        #region Get Entities To Calculate
        public static JObject GetEntitiesToCalculate(string entityTypeID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntitiesToCalculate)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityFileId", entityTypeID.ToString())
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

        #region Set Calculation Values
        public static JObject SetCalculationValues(string ENTITY_ASSOC_TYPE_ID, string ENTITY_ID, string TEXT, string CALCULATION_ERROR, string CALCULATION_EQUATION)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.SetCalculationValues)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ENTITY_ASSOC_TYPE_ID", ENTITY_ASSOC_TYPE_ID.ToString()),
                                new KeyValuePair<string, string>("ENTITY_ID", ENTITY_ID.ToString()),
                new KeyValuePair<string, string>("TEXT", TEXT.ToString()),
                new KeyValuePair<string, string>("CALCULATION_ERROR", CALCULATION_ERROR.ToString()),
                new KeyValuePair<string, string>("CALCULATION_EQUATION", CALCULATION_EQUATION.ToString()),

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

        #region Delete EntityItem
        public static JObject DeleteEntityItem(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.DeleteEntityItem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Entity By EntityID
        public static JObject GetEntityByEntityID(string ENTITY_ID, string user, string activityIndicator)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityByEntityID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ENTITY_ID", ENTITY_ID.ToString()),
                new KeyValuePair<string, string>("user", user.ToString()),
                new KeyValuePair<string, string>("activityIndicator", activityIndicator.ToString())
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

        #region WS_GetEntityTypeList
        public static JObject WS_GetEntityTypeList(string ENTITY_ID, string user, string activityIndicator)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.WS_GetEntityTypeList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {

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

        #region WS_ Get Entity Type Schema By EntityTypeID
        public static JObject WS_GetEntityTypeSchemaByEntityTypeID(string entityTypeID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.WS_GetEntityTypeSchemaByEntityTypeID)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("entityTypeID", entityTypeID.ToString())
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

        #region Add File To EntityAssoc Type
        public static JObject AddFileToEntityAssocType(object addfile)
        {
            var Result = Constants.ApiCommon(addfile, Constants.AddFileToEntityAssocType);

            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Entity Type Configuration
        public static JObject GetEntityTypeConfiguration(string pEntityTypeID, string pUser)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeConfiguration)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pEntityTypeID", pEntityTypeID.ToString()),
                new KeyValuePair<string, string>("pUser", pUser.ToString())

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

        #region Get Entities By System Code Key Value Pair_LazyLoad
        public static JObject GetEntitiesBySystemCodeKeyValuePair_LazyLoad(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GetEntitiesBySystemCodeKeyValuePair_LazyLoad);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get Entity Type Detail
        public static JObject GetEntityTypeDetail(string EntityTypeId, string User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeDetail)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityTypeId", EntityTypeId.ToString()),
                new KeyValuePair<string, string>("User", User.ToString()),
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

        #region Custom Template Methods

        #region Get Custom Template Tags
        public static JObject GetCustomTemplateTags(string EntityTypeId, string Tags, string Path, string GetDefaultTemplate)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetCustomTemplateTags)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityTypeId", EntityTypeId.ToString()),
               new KeyValuePair<string, string>("Tags", Tags.ToString()),
                new KeyValuePair<string, string>("Path", Path.ToString()),
                    new KeyValuePair<string, string>("GetDefaultTemplate", GetDefaultTemplate.ToString())
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

        #region Get Custom Template Tags By Attribute
        public static JObject GetCustomTemplateTagsByAttribute(string EntityTypeId, string Attribute, string Values, string GetDefaultTemplate)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetCustomTemplateTagsByAttribute)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityTypeId", EntityTypeId.ToString()),
               new KeyValuePair<string, string>("Attribute", Attribute.ToString()),
                new KeyValuePair<string, string>("Values", Values.ToString()),
                    new KeyValuePair<string, string>("GetDefaultTemplate", GetDefaultTemplate.ToString())
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

        #region Get Entity Related Applications
        public static JObject GetEntityRelatedApplications(string EntityTypeId, string EntityId, string User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityRelatedApplications)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityTypeId", EntityTypeId.ToString()),
               new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                new KeyValuePair<string, string>("User", User.ToString())
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

        #region Get Entity Related Types
        public static JObject GetEntityRelatedTypes(string EntityTypeId, string EntityId, string User, string Application)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityRelatedTypes)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("EntityTypeId", EntityTypeId.ToString()),
               new KeyValuePair<string, string>("EntityId", EntityId?.ToString()),
                new KeyValuePair<string, string>("User", User?.ToString()),
                  new KeyValuePair<string, string>("Application", Application?.ToString())
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

        #region Get Entity Role Relation Data
        public static JObject GetEntityRoleRelationData(string EntityId, string User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityRoleRelationData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                new KeyValuePair<string, string>("User", User.ToString()),
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

        #region GetEntityTypeRelationData
        public static JObject GetEntityTypeRelationData(string EntityId, string User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeRelationData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                new KeyValuePair<string, string>("User", User.ToString()),
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

        #region Get Cases Relation Data
        public static JObject GetCasesRelationData(string EntityId, string CaseTypeId, string User, string GetCasesStatus, string pageIndex, string pageSize, string sortBy, string sortOrder, string searchXml, string getListFor)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetCasesRelationData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                 new KeyValuePair<string, string>("CaseTypeId", CaseTypeId.ToString()),
                new KeyValuePair<string, string>("User", User.ToString()),
                 new KeyValuePair<string, string>("GetCasesStatus", GetCasesStatus.ToString()),
                  new KeyValuePair<string, string>("pageIndex", pageIndex.ToString()),
                   new KeyValuePair<string, string>("pageSize", pageSize.ToString()),
                    new KeyValuePair<string, string>("sortBy", sortBy.ToString()),
                     new KeyValuePair<string, string>("sortOrder", sortOrder.ToString()),
                      new KeyValuePair<string, string>("searchXml", searchXml.ToString()),
                       new KeyValuePair<string, string>("getListFor", getListFor.ToString())
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

        #region Get Entities Relation Data


        public static JObject GetEntitiesRelationData(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.GetEntitiesRelationData);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region  Get Quest Relation Data
        public static JObject GetQuestRelationData(string EntityId, string AreaId, string ItemId, string User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetQuestRelationData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
                 new KeyValuePair<string, string>("AreaId", AreaId.ToString()),
                new KeyValuePair<string, string>("ItemId", ItemId.ToString()),
                 new KeyValuePair<string, string>("User", User.ToString())
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

        #region  Get Trigger Grid Data
        public static JObject GetTriggerGridData(string EntityId)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetTriggerGridData)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("EntityId", EntityId.ToString())

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


        #region Get EntityType Relation Data by entityid
        public static JObject GetEntityTypeRelationDatabyentityid(int EntityId, string User, string System_Code)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityTypeRelationDatabyentityid)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("EntityId", EntityId.ToString()),
               new KeyValuePair<string, string>("User", User),
               new KeyValuePair<string, string>("System_Code", System_Code)
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


        #endregion

        #region Get CalculationFields
        public static JObject RefreshCalculationFields(object Body_value)
        {
            try
            {
                return Constants.ApiCommon(Body_value, Constants.RefreshCalculationFieldsEntity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ExternalDatasourceByQuery
        public static JObject ExternalDatasourceByQuery(string ConnectionString, string Query)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.ExternalDatasourceByQuery)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("ConnectionString", ConnectionString.ToString()),
                new KeyValuePair<string, string>("Query", Query.ToString()),
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
        public static JObject getConnectionString(string ExternalDataSourceID)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetFilterQuery_Entity)
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

        #region Entities_GetEntityList
        public static JObject BoxerCentralHome_Entities_GetEntityList(string User_SAM, string RecordCount)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.BoxerCentralHome_Entities_GetEntityList)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("User_SAM", User_SAM),
                 new KeyValuePair<string, string>("RecordCount", RecordCount)

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

        #region GetAllEntityRoleRelationshipByEmp
        public static JObject GetAllEntityRoleRelationshipByEmp(string pEMPLOYEE_SAM, string UserName)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetAllEntityRoleRelationshipByEmp)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("pEMPLOYEE_SAM", pEMPLOYEE_SAM),
                 new KeyValuePair<string, string>("UserName", UserName)

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

        #region Get Entities By System Code Key Value Pair_LazyLoad
        public static JObject GetEntityBasicDetails(int EntityId, string user_name)
        {
            try
            {
                EntityBasicDetailsRequest Body_value = new EntityBasicDetailsRequest();
                Body_value.Entity_id = EntityId;
                Body_value.User_Name = user_name;

                return Constants.ApiCommon(Body_value, Constants.EntityBasicDetails);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region GetEntityFileUsingModel
        public static JObject GetFileFromEntity(string EntityID, string fileID, string userName)
        {
            //RequestModel
            EntityFileRequest getEntityFileModel = new EntityFileRequest();
            getEntityFileModel.EntityID = EntityID;
            getEntityFileModel.FileID = fileID;
            getEntityFileModel.UserName = userName;

            return Constants.ApiCommon(getEntityFileModel, Constants.GetEntityFile);
        }
        #endregion

        #region Add Entity file 
        public static JObject AddFileToEntity(string ENTITY_ID, string DESCRIPTION, string FILE_NAME, string FILE_SIZE_BYTES, byte[] FILE_BLOB, string EXTERNAL_URI, Char SHOW_INLINE_NOTES, string SYSTEM_CODE, Char IS_ACTIVE, string CREATED_BY)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.AddFileToEntity)
            };
            #endregion

            #region API Body Details

            AddFileToEntityRequest addfile = new AddFileToEntityRequest();
            addfile.ENTITY_ID = Convert.ToInt32(ENTITY_ID);
            addfile.DESCRIPTION = DESCRIPTION.ToString();
            addfile.FILE_DATE_TIME = DateTime.Now;
            addfile.FILE_NAME = FILE_NAME;
            addfile.FILE_SIZE_BYTES = Convert.ToInt32(FILE_SIZE_BYTES);
            addfile.FILE_BLOB = FILE_BLOB;
            addfile.EXTERNAL_URI = EXTERNAL_URI;
            addfile.SHOW_INLINE_NOTES = SHOW_INLINE_NOTES;
            addfile.SYSTEM_CODE = SYSTEM_CODE;
            addfile.IS_ACTIVE = IS_ACTIVE;
            addfile.CREATED_BY = CREATED_BY;
            #endregion

            var Result = Constants.ApiCommon(addfile, Constants.AddFileToEntity);
            if (Result != null)
            {
                return Result;
            }
            else
                return null;
        }
        #endregion

        #region Get Entity Role Relation Data
        public static JObject GetEntityRoleAssignByEmp(string System_Code, string User)
        {
            #region API Details
            var API_value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("API_Name", Constants.Baseurl + Constants.GetEntityRoleAssignByEmp)
            };
            #endregion

            #region API Body Details
            var Body_value = new List<KeyValuePair<string, string>>
            {
               new KeyValuePair<string, string>("System_Code", System_Code),
                new KeyValuePair<string, string>("Username", User.ToString()),
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
    }
}
