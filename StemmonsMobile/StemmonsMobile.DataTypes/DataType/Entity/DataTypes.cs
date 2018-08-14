using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
//using System.Security.Cryptography;


namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class AreaItem
    {
        public int ItemID { get; set; }

        public string ItemName { get; set; }

        public int AreaID { get; set; }

        public string AreaName { get; set; }
    }

    public class ItemInstanceTranForEntity
    {
        public ItemInstanceTranForEntity() { }
        public int intItemInstanceTranID { get; set; }
        public int intItemID { get; set; }
        public int intListId { get; set; }
        public bool? blnIsEdit { get; set; }
        public string strItemNAme { get; set; }
        public int intFormInstanceID { get; set; }
        public string strOtherComments { get; set; }
        public string strFormCretedBy { get; set; }
        public DateTime FormCredtedDate { get; set; }
        public int? intInforTranId { get; set; }
        public string strIsLocked { get; set; }
        public decimal? dcOverallScore { get; set; }
        public string strOverallScore { get; set; }
        public string strInspectedByName { get; set; }
        public string Url { get; set; }
        public string SECURITY_TYPE_AREA { get; set; }
        public string SECURITY_TYPE_ITEM { get; set; }
        public string SECURITY_TYPE_TRAN { get; set; }
    }

    public class EntityTypeRelationAssignment
    {
        public string Comments { get; set; }
        public char? ControlType { get; set; }
        public string EmployeeGUID { get; set; }
        public string EndDate { get; set; }
        public int EntityID { get; set; }
        public string EntityTitle { get; set; }
        public int EntityTypeAssignID { get; set; }
        public int EntityTypeID { get; set; }
        public string FullName { get; set; }
        public string IsPrimary { get; set; }
        public string Operation { get; set; }
        public string Primary { get; set; }
        public string Remove { get; set; }
        public string StartDate { get; set; }
    }

    public class EntityRoleRelationAssignment
    {
        public string Comments { get; set; }
        public char? ControlType { get; set; }
        public string EmployeeGUID { get; set; }
        public string EndDate { get; set; }
        public int EntityID { get; set; }
        public int EntityRoleAssignID { get; set; }
        public string EntityTitle { get; set; }
        public int EntityTypeID { get; set; }
        public string FullName { get; set; }
        public string IsPrimary { get; set; }
        public string Operation { get; set; }
        public string Primary { get; set; }
        public string Remove { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string StartDate { get; set; }
    }

    public class QuestRelationalGridItem
    {
        public int ItemId { get; set; }
        public int ListId { get; set; }
        public int FormInastanceId { get; set; }
        public string ItemName { get; set; }
        public int ItemInstanceId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDateTime { get; set; }
    }

    public class DomList
    {
        public string MobilePosition { get; set; }
        public string Dom { get; set; }
    }

    public class DataTypes
    {
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ENTITY_LIST_WITH_NOTES : ENTITY_LIST
    {

        private ENTITY_LISTENTITY_WITH_NOTES[] eNTITY_WITH_NOTESField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_WITH_NOTES")]
        public ENTITY_LISTENTITY_WITH_NOTES[] ENTITY_WITH_NOTES
        {
            get
            {
                return this.eNTITY_WITH_NOTESField;
            }
            set
            {
                this.eNTITY_WITH_NOTESField = value;
            }
        }


        public List<EntityWithNote> DeserializeEntityListWithNotes()
        {
            List<EntityWithNote> eList = new List<EntityWithNote>();
            foreach (ENTITY_LISTENTITY_WITH_NOTES eItem in this.eNTITY_WITH_NOTESField)
            {

                EntityWithNote entity = new EntityWithNote()
                {
                    EntityTypeName = eItem.ENTITY_TYPE_NAME,
                    EntityTypeID = eItem.ENTITY_TYPE_ID,
                    InstanceSingular = eItem.INSTANCE_NAME,
                    InstancePlural = eItem.INSTANCE_NAME_PLURAL,
                    EntityTypeOwener = eItem.OWNER,
                    EntityTypeHelpURL = eItem.HELP_URL,
                    AssociationFieldCollection = new List<AssociationField>(),
                    EntityID = eItem.ENTITY_ID,
                    ListID = eItem.LIST_ID,
                    EntityTypeRelationship = new List<EntityTypeRelationship>(),
                    EntitiyAssignedToUserName = eItem.ENTITY_ASSIGNED_TO_USERNAME,
                    EntitiyOwnedByUserName = eItem.ENTITY_OWNER_BY_USERNAME,
                    EntityAssignedToDateTime = eItem.ENTITY_ASSIGNED_TO_DATETIME,
                    EntityAssignedToEmail = eItem.ENTITY_ASSIGNED_TO_EMAIL,
                    EntityAssignedToFullName = eItem.ENTITY_ASSIGNED_TO_FULLNAME,
                    EntityAssignedToGUID = eItem.ENTITY_ASSIGNED_TO_GUID,
                    EntityAssignedToPhone = eItem.ENTITY_ASSIGNED_TO_PHONE,
                    EntityCreatedByEmail = eItem.ENTITY_CREATED_BY_EMAIL,
                    EntityCreatedByFullName = eItem.ENTITY_CREATED_BY_FULLNAME,
                    EntityCreatedByGUID = eItem.ENTITY_CREATED_BY_GUID,
                    EntityCreatedByPhone = eItem.ENTITY_CREATED_BY_PHONE,
                    EntityCreatedByUserName = eItem.ENTITY_CREATED_BY_USERNAME,
                    EntityCreatedDateTime = eItem.ENTITY_CREATED_DATETIME,
                    EntityModifiedByEmail = eItem.ENTITY_MODIFIED_BY_EMAIL,
                    EntityModifiedByFullName = eItem.ENTITY_MODIFIED_BY_FULLNAME,
                    EntityModifiedByGUID = eItem.ENTITY_MODIFIED_BY_GUID,
                    EntityModifiedByPhone = eItem.ENTITY_MODIFIED_BY_PHONE,
                    EntityModifiedByUserName = eItem.ENTITY_MODIFIED_BY_USERNAME,
                    EntityModifiedDateTime = eItem.ENTITY_MODIFIED_DATETIME,
                    EntityOwnedByDateTime = eItem.ENTITY_OWNER_DATETIME,
                    EntityOwnedByEmail = eItem.ENTITY_OWNER_BY_EMAIL,
                    EntityOwnedByFullName = eItem.ENTITY_OWNER_BY_FULLNAME,
                    EntityOwnedByGUID = eItem.ENTITY_OWNER_BY_GUID,
                    EntityOwnedByPhone = eItem.ENTITY_OWNER_BY_PHONE,
                    MasterEntityID = eItem.MASTER_ENTITY_ID,
                    MasterEntityRelationDescription = eItem.MASTER_RELATION_DESC,
                    //EntitySecurityType = eItem.ENTITY_SECURITY_TYPE,
                    NewestNotesOnTop = eItem.NEWEST_NOTE_ON_TOP,
                    EntityNote = eItem.ENTITY_NOTE
                };

                if (eItem.ASSOC_TYPE != null)
                {
                    foreach (var item in eItem.ASSOC_TYPE)
                    {
                        AssociationField eAssociationField = new AssociationField()
                        {
                            EntityTypeID = eItem.ENTITY_TYPE_ID,
                            AssocTypeID = item.ENTITY_ASSOC_TYPE_ID,
                            FieldType = item.FIELD_TYPE,
                            AssocName = item.ASSOC_TYPE_NAME,
                            CurrencyTypeID = item.ENTITY_CURRENCY_TYPE_ID,
                            CurrencyIndicator = item.CURRENCY_INDICATOR,
                            //DecimalPrecesion = item.DECIMAL_PRECESION,
                            //IsRequired = item.IS_REQUIRED,
                            ItemDesktopPriorityValue = item.ITEM_DESKTOP_PRIORITY_VALUE,
                            ItemMobilePriorityValue = item.ITEM_MOBILE_PRIORITY_VALUE,
                            ListDesktopPriorityValue = item.LIST_DESKTOP_PRIORITY_VALUE,
                            ListMobilePriorityValue = item.LIST_MOBILE_PRIORITY_VALUE,
                            //AssocShowOnList = item.SHOW_ON_LIST,
                            AssocSystemCode = item.ASSOC_TYPE_SYSTEM_CODE,
                            AssocSystemCodeName = item.ASSOC_TYPE_SYSTEM_CODE_NAME,
                            AssocDecode = new List<AssociationFieldValue>(),
                            AssocMetaData = new List<AssociationMetaData>(),
                            AssocMetaDataText = new List<AssociationMetaDataText>()
                        };

                        if (item.ASSOC_METADATA_TEXT != null)
                        {
                            //ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA_TEXT subItem = item.ASSOC_METADATA_TEXT;
                            foreach (var subItem in item.ASSOC_METADATA_TEXT)
                            {
                                eAssociationField.AssocMetaDataText.Add(new AssociationMetaDataText()
                                {
                                    AssocMetaDataTextID = subItem.ENTITY_ASSOC_METADATA_TEXT_ID,
                                    AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID,
                                    EntityID = eItem.ENTITY_ID,
                                    TextValue = subItem.TEXT,
                                    CalculationEquation = subItem.CALCULATION_EQUATION,
                                    CalculationError = subItem.CALCULATION_ERROR,
                                    CalculationFrequencyMin = subItem.CALCULATION_FREQUENCY_MIN,
                                    CalculationLastRanDateTime = subItem.CALCULATION_LAST_RAN_DATETIME,
                                    EntityFileID = subItem.ENTITY_FILE_ID,
                                    TryCurrencyValue = subItem.TRY_CURRENCY_VALUE,
                                    TryDateTimeValue = subItem.TRY_DATETIME_VALUE,
                                    TryDateValue = subItem.TRY_DATE_VALUE,
                                    TryDecimalValue = subItem.TRY_DECIMAL_VALUE,
                                    TryIntegerValue = subItem.TRY_INTEGER_VALUE,
                                });
                            }
                        }

                        if (item.ASSOC_METADATA != null)
                        {
                            foreach (var subItem in item.ASSOC_METADATA)
                            {
                                AssociationMetaData metaData = new AssociationMetaData();

                                metaData.AssocMetaDataID = subItem.ENTITY_ASSOC_METADATA_ID;
                                metaData.AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID;
                                metaData.AssocDecodeID = subItem.ENTITY_ASSOC_DECODE_ID;
                                metaData.EntityID = eItem.ENTITY_ID;
                                metaData.ExternalDatasourceObjectID = subItem.EXTERNAL_DATASOURCE_OBJECT_ID;
                                metaData.FieldName = subItem.FIELD_NAME;
                                metaData.FieldValue = subItem.FIELD_VALUE;

                                if (metaData.AssocMetaDataID != 0)
                                { eAssociationField.AssocMetaData.Add(metaData); }
                            }
                        }
                        entity.AssociationFieldCollection.Add(eAssociationField);
                    }
                }
                eList.Add(entity);
            }
            return eList;

        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ENTITY_LIST
    {

        private ENTITY_LISTENTITY[] eNTITYField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY")]
        public ENTITY_LISTENTITY[] ENTITY
        {
            get
            {
                return this.eNTITYField;
            }
            set
            {
                this.eNTITYField = value;
            }
        }

        public List<EntityClass> DeserializeEntityList()
        {
            List<EntityClass> eList = new List<EntityClass>();
            foreach (ENTITY_LISTENTITY eItem in this.eNTITYField)
            {

                EntityClass entity = new EntityClass()
                {
                    EntityTypeName = eItem.ENTITY_TYPE_NAME,
                    EntityTypeID = eItem.ENTITY_TYPE_ID,
                    InstanceSingular = eItem.INSTANCE_NAME,
                    InstancePlural = eItem.INSTANCE_NAME_PLURAL,
                    EntityTypeOwener = eItem.OWNER,
                    EntityTypeHelpURL = eItem.HELP_URL,
                    AssociationFieldCollection = new List<AssociationField>(),
                    EntityID = eItem.ENTITY_ID,
                    ListID = eItem.LIST_ID,
                    EntityTypeRelationship = new List<EntityTypeRelationship>(),
                    EntitiyAssignedToUserName = eItem.ENTITY_ASSIGNED_TO_USERNAME,
                    EntitiyOwnedByUserName = eItem.ENTITY_OWNER_BY_USERNAME,
                    EntityAssignedToDateTime = eItem.ENTITY_ASSIGNED_TO_DATETIME,
                    EntityAssignedToEmail = eItem.ENTITY_ASSIGNED_TO_EMAIL,
                    EntityAssignedToFullName = eItem.ENTITY_ASSIGNED_TO_FULLNAME,
                    EntityAssignedToGUID = eItem.ENTITY_ASSIGNED_TO_GUID,
                    EntityAssignedToPhone = eItem.ENTITY_ASSIGNED_TO_PHONE,
                    EntityCreatedByEmail = eItem.ENTITY_CREATED_BY_EMAIL,
                    EntityCreatedByFullName = eItem.ENTITY_CREATED_BY_FULLNAME,
                    EntityCreatedByGUID = eItem.ENTITY_CREATED_BY_GUID,
                    EntityCreatedByPhone = eItem.ENTITY_CREATED_BY_PHONE,
                    EntityCreatedByUserName = eItem.ENTITY_CREATED_BY_USERNAME,
                    EntityCreatedDateTime = eItem.ENTITY_CREATED_DATETIME,
                    EntityModifiedByEmail = eItem.ENTITY_MODIFIED_BY_EMAIL,
                    EntityModifiedByFullName = eItem.ENTITY_MODIFIED_BY_FULLNAME,
                    EntityModifiedByGUID = eItem.ENTITY_MODIFIED_BY_GUID,
                    EntityModifiedByPhone = eItem.ENTITY_MODIFIED_BY_PHONE,
                    EntityModifiedByUserName = eItem.ENTITY_MODIFIED_BY_USERNAME,
                    EntityModifiedDateTime = eItem.ENTITY_MODIFIED_DATETIME,
                    EntityOwnedByDateTime = eItem.ENTITY_OWNER_DATETIME,
                    EntityOwnedByEmail = eItem.ENTITY_OWNER_BY_EMAIL,
                    EntityOwnedByFullName = eItem.ENTITY_OWNER_BY_FULLNAME,
                    EntityOwnedByGUID = eItem.ENTITY_OWNER_BY_GUID,
                    EntityOwnedByPhone = eItem.ENTITY_OWNER_BY_PHONE,
                    MasterEntityID = eItem.MASTER_ENTITY_ID,
                    MasterEntityRelationDescription = eItem.MASTER_RELATION_DESC,
                    EntitySecurityType = eItem.ENTITY_SECURITY_TYPE,
                    NewestNotesOnTop = eItem.NEWEST_NOTE_ON_TOP,
                    ApplyAssocTypeCalculationFormula = eItem.IS_APPLY_ASSOC_TYPE_CALCULATION_FORMULA,
                    EntityTypeSecurityType = eItem.ENTITY_TYPE_SECURITY_TYPE,
                };

                if (eItem.ASSOC_TYPE != null)
                {
                    foreach (var item in eItem.ASSOC_TYPE)
                    {
                        AssociationField eAssociationField = new AssociationField()
                        {
                            EntityTypeID = eItem.ENTITY_TYPE_ID,
                            AssocTypeID = item.ENTITY_ASSOC_TYPE_ID,
                            FieldType = item.FIELD_TYPE,
                            AssocName = item.ASSOC_TYPE_NAME,
                            CurrencyTypeID = item.ENTITY_CURRENCY_TYPE_ID,
                            CurrencyIndicator = item.CURRENCY_INDICATOR,
                            //DecimalPrecesion = item.DECIMAL_PRECESION,
                            //IsRequired = item.IS_REQUIRED,
                            ItemDesktopPriorityValue = item.ITEM_DESKTOP_PRIORITY_VALUE,
                            ItemMobilePriorityValue = item.ITEM_MOBILE_PRIORITY_VALUE,
                            ListDesktopPriorityValue = item.LIST_DESKTOP_PRIORITY_VALUE,
                            ListMobilePriorityValue = item.LIST_MOBILE_PRIORITY_VALUE,
                            //AssocShowOnList = item.SHOW_ON_LIST,
                            AssocSystemCode = item.ASSOC_TYPE_SYSTEM_CODE,
                            AssocSystemCodeName = item.ASSOC_TYPE_SYSTEM_CODE_NAME,
                            AssocDecode = new List<AssociationFieldValue>(),
                            AssocMetaData = new List<AssociationMetaData>(),
                            AssocMetaDataText = new List<AssociationMetaDataText>(),
                            CalculationFormula = item.CALCULATION_FORMULA,
                            CalculationFrequencyMin = item.CALCULATION_FREQUENCY_MIN,
                            SecurityType = item.SECURITY_TYPE


                        };

                        if (item.ASSOC_METADATA_TEXT != null)
                        {
                            //ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA_TEXT subItem = item.ASSOC_METADATA_TEXT;
                            foreach (var subItem in item.ASSOC_METADATA_TEXT)
                            {
                                eAssociationField.AssocMetaDataText.Add(new AssociationMetaDataText()
                                {
                                    AssocMetaDataTextID = subItem.ENTITY_ASSOC_METADATA_TEXT_ID,
                                    AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID,
                                    EntityID = eItem.ENTITY_ID,
                                    TextValue = subItem.TEXT,
                                    CalculationEquation = subItem.CALCULATION_EQUATION,
                                    CalculationError = subItem.CALCULATION_ERROR,
                                    CalculationFrequencyMin = subItem.CALCULATION_FREQUENCY_MIN,
                                    CalculationLastRanDateTime = subItem.CALCULATION_LAST_RAN_DATETIME,
                                    EntityFileID = subItem.ENTITY_FILE_ID,
                                    TryCurrencyValue = subItem.TRY_CURRENCY_VALUE,
                                    TryDateTimeValue = subItem.TRY_DATETIME_VALUE,
                                    TryDateValue = subItem.TRY_DATE_VALUE,
                                    TryDecimalValue = subItem.TRY_DECIMAL_VALUE,
                                    TryIntegerValue = subItem.TRY_INTEGER_VALUE,
                                });
                            }
                        }

                        if (item.ASSOC_METADATA != null)
                        {
                            foreach (var subItem in item.ASSOC_METADATA)
                            {
                                AssociationMetaData metaData = new AssociationMetaData();

                                metaData.AssocMetaDataID = subItem.ENTITY_ASSOC_METADATA_ID;
                                metaData.AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID;
                                metaData.AssocDecodeID = subItem.ENTITY_ASSOC_DECODE_ID;
                                metaData.EntityID = eItem.ENTITY_ID;
                                metaData.ExternalDatasourceObjectID = subItem.EXTERNAL_DATASOURCE_OBJECT_ID;
                                metaData.FieldName = subItem.FIELD_NAME;
                                metaData.FieldValue = subItem.FIELD_VALUE;


                                if (metaData.AssocMetaDataID != 0)
                                { eAssociationField.AssocMetaData.Add(metaData); }

                            }
                        }






                        entity.AssociationFieldCollection.Add(eAssociationField);
                    }
                }
                eList.Add(entity);
            }

            return eList;

        }


        public List<EntityRelationData> DeserializeEntityMobileList()
        {
            List<EntityRelationData> eList = new List<EntityRelationData>();
            foreach (ENTITY_LISTENTITY eItem in this.eNTITYField)
            {

                EntityRelationData entity = new EntityRelationData()
                {
                    EntityID = eItem.ENTITY_ID,
                    ListID = eItem.LIST_ID,
                    EntityCreatedByUserName = eItem.ENTITY_CREATED_BY_USERNAME,
                    EntityCreatedDateTime = eItem.ENTITY_CREATED_DATETIME,
                    EntityModifiedByUserName = eItem.ENTITY_MODIFIED_BY_USERNAME,
                    EntityModifiedDateTime = eItem.ENTITY_MODIFIED_DATETIME,
                    EntityOwnedByDateTime = eItem.ENTITY_OWNER_DATETIME,
                    Title = eItem.ASSOC_TYPE.Select(x => x.ASSOC_METADATA_TEXT.Select(xx => xx.TEXT).FirstOrDefault()).FirstOrDefault()
                };
                eList.Add(entity);
            }

            return eList;

        }

    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_LISTENTITY_WITH_NOTES : ENTITY_LISTENTITY
    {
        private string eNTITY_NOTEField;

        public string ENTITY_NOTE
        {
            get
            {
                return this.eNTITY_NOTEField;
            }
            set
            {
                this.eNTITY_NOTEField = value;
            }
        }

    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_LISTENTITY
    {

        private int eNTITY_IDField;

        private int lIST_IDField;

        private int? mASTER_ENTITY_IDField;

        private string mASTER_RELATION_DESCField;

        private string eNTITY_CREATED_DATETIMEField;

        private string eNTITY_CREATED_BYField;

        private int eNTITY_TYPE_IDField;

        private string eNTITY_TYPE_NAMEField;

        private string iNSTANCE_NAMEField;

        private string iNSTANCE_NAME_PLURALField;

        private string oWNERField;

        private string nEWEST_NOTE_ON_TOPField;




        private string iS_APPLY_ASSOC_TYPE_CALCULATION_FORMULAField;

        private string hELP_URLField;

        private ENTITY_LISTENTITYASSOC_TYPE[] aSSOC_TYPEField;
        private string eNTITY_TYPE_SECURITY_TYPEField;

        private string eNTITY_SECURITY_TYPEField;

        private string eNTITY_CREATED_BY_USERNAMEField;

        private string eNTITY_CREATED_BY_FULLNAMEField;

        private string eNTITY_CREATED_BY_PHONEField;

        private string eNTITY_CREATED_BY_EMAILField;

        private Guid? eNTITY_CREATED_BY_GUIDField;

        private string eNTITY_MODIFIED_BY_USERNAMEField;

        private string eNTITY_MODIFIED_BY_FULLNAMEField;

        private string eNTITY_MODIFIED_BY_PHONEField;

        private string eNTITY_MODIFIED_BY_EMAILField;

        private Guid? eNTITY_MODIFIED_BY_GUIDField;

        private string eNTITY_MODIFIED_DATETIMEField;

        private string eNTITY_ASSIGNED_TO_USERNAMEField;

        private string eNTITY_ASSIGNED_TO_FULLNAMEField;

        private string eNTITY_ASSIGNED_TO_PHONEField;

        private string eNTITY_ASSIGNED_TO_EMAILField;

        private Guid? eNTITY_ASSIGNED_TO_GUIDField;

        private string eNTITY_ASSIGNED_TO_DATETIMEField;

        private string eNTITY_OWNER_BY_USERNAMEField;

        private string eNTITY_OWNER_BY_FULLNAMEField;

        private string eNTITY_OWNER_BY_PHONEField;

        private string eNTITY_OWNER_BY_EMAILField;

        private Guid? eNTITY_OWNER_BY_GUIDField;

        private string eNTITY_OWNER_DATETIMEField;



        /// <remarks/>
        //public string ENTITY_SECURITY_TYPE
        //{
        //    get
        //    {
        //        return this.eNTITY_SECURITY_TYPEField;
        //    }
        //    set
        //    {
        //        this.eNTITY_SECURITY_TYPEField = value;
        //    }
        //}

        /// <remarks/>


        /// <remarks/>
        public string ENTITY_CREATED_BY_USERNAME
        {
            get
            {
                return this.eNTITY_CREATED_BY_USERNAMEField;
            }
            set
            {
                this.eNTITY_CREATED_BY_USERNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_FULLNAME
        {
            get
            {
                return this.eNTITY_CREATED_BY_FULLNAMEField;
            }
            set
            {
                this.eNTITY_CREATED_BY_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_PHONE
        {
            get
            {
                return this.eNTITY_CREATED_BY_PHONEField;
            }
            set
            {
                this.eNTITY_CREATED_BY_PHONEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_EMAIL
        {
            get
            {
                return this.eNTITY_CREATED_BY_EMAILField;
            }
            set
            {
                this.eNTITY_CREATED_BY_EMAILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Guid? ENTITY_CREATED_BY_GUID
        {
            get
            {
                return this.eNTITY_CREATED_BY_GUIDField;
            }
            set
            {
                this.eNTITY_CREATED_BY_GUIDField = value;
            }
        }


        /// <remarks/>
        public string ENTITY_MODIFIED_BY_USERNAME
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_USERNAMEField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_USERNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_MODIFIED_BY_FULLNAME
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_FULLNAMEField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_MODIFIED_BY_PHONE
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_PHONEField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_PHONEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_MODIFIED_BY_EMAIL
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_EMAILField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_EMAILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Guid? ENTITY_MODIFIED_BY_GUID
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_GUIDField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_GUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_MODIFIED_DATETIME
        {
            get
            {
                return this.eNTITY_MODIFIED_DATETIMEField;
            }
            set
            {
                this.eNTITY_MODIFIED_DATETIMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_USERNAME
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_USERNAMEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_USERNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_FULLNAME
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_FULLNAMEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_PHONE
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_PHONEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_PHONEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_EMAIL
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_EMAILField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_EMAILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Guid? ENTITY_ASSIGNED_TO_GUID
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_GUIDField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_GUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_DATETIME
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_DATETIMEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_DATETIMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_USERNAME
        {
            get
            {
                return this.eNTITY_OWNER_BY_USERNAMEField;
            }
            set
            {
                this.eNTITY_OWNER_BY_USERNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_FULLNAME
        {
            get
            {
                return this.eNTITY_OWNER_BY_FULLNAMEField;
            }
            set
            {
                this.eNTITY_OWNER_BY_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_PHONE
        {
            get
            {
                return this.eNTITY_OWNER_BY_PHONEField;
            }
            set
            {
                this.eNTITY_OWNER_BY_PHONEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_EMAIL
        {
            get
            {
                return this.eNTITY_OWNER_BY_EMAILField;
            }
            set
            {
                this.eNTITY_OWNER_BY_EMAILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Guid? ENTITY_OWNER_BY_GUID
        {
            get
            {
                return this.eNTITY_OWNER_BY_GUIDField;
            }
            set
            {
                this.eNTITY_OWNER_BY_GUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_OWNER_DATETIME
        {
            get
            {
                return this.eNTITY_OWNER_DATETIMEField;
            }
            set
            {
                this.eNTITY_OWNER_DATETIMEField = value;
            }
        }
        /// <remarks/>
        public int ENTITY_ID
        {
            get
            {
                return this.eNTITY_IDField;
            }
            set
            {
                this.eNTITY_IDField = value;
            }
        }

        /// <remarks/>
        public int LIST_ID
        {
            get
            {
                return this.lIST_IDField;
            }
            set
            {
                this.lIST_IDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? MASTER_ENTITY_ID
        {
            get
            {
                return this.mASTER_ENTITY_IDField;
            }
            set
            {
                this.mASTER_ENTITY_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string MASTER_RELATION_DESC
        {
            get
            {
                return this.mASTER_RELATION_DESCField;
            }
            set
            {
                this.mASTER_RELATION_DESCField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_CREATED_DATETIME
        {
            get
            {
                return this.eNTITY_CREATED_DATETIMEField;
            }
            set
            {
                this.eNTITY_CREATED_DATETIMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY
        {
            get
            {
                return this.eNTITY_CREATED_BYField;
            }
            set
            {
                this.eNTITY_CREATED_BYField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_TYPE_ID
        {
            get
            {
                return this.eNTITY_TYPE_IDField;
            }
            set
            {
                this.eNTITY_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_TYPE_NAME
        {
            get
            {
                return this.eNTITY_TYPE_NAMEField;
            }
            set
            {
                this.eNTITY_TYPE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string INSTANCE_NAME
        {
            get
            {
                return this.iNSTANCE_NAMEField;
            }
            set
            {
                this.iNSTANCE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string INSTANCE_NAME_PLURAL
        {
            get
            {
                return this.iNSTANCE_NAME_PLURALField;
            }
            set
            {
                this.iNSTANCE_NAME_PLURALField = value;
            }
        }

        /// <remarks/>
        public string OWNER
        {
            get
            {
                return this.oWNERField;
            }
            set
            {
                this.oWNERField = value;
            }
        }

        /// <remarks/>
        public string NEWEST_NOTE_ON_TOP
        {
            get
            {
                return this.nEWEST_NOTE_ON_TOPField;
            }
            set
            {
                this.nEWEST_NOTE_ON_TOPField = value;
            }
        }

        public string ENTITY_SECURITY_TYPE
        {
            get
            {
                return this.eNTITY_SECURITY_TYPEField;
            }
            set
            {
                this.eNTITY_SECURITY_TYPEField = value;
            }
        }

        public string ENTITY_TYPE_SECURITY_TYPE
        {
            get
            {
                return this.eNTITY_TYPE_SECURITY_TYPEField;
            }
            set
            {
                this.eNTITY_TYPE_SECURITY_TYPEField = value;
            }
        }

        /// <remarks/>
        public string HELP_URL
        {
            get
            {
                return this.hELP_URLField;
            }
            set
            {
                this.hELP_URLField = value;
            }
        }

        public string IS_APPLY_ASSOC_TYPE_CALCULATION_FORMULA
        {
            get
            {
                return this.iS_APPLY_ASSOC_TYPE_CALCULATION_FORMULAField;
            }
            set
            {
                this.iS_APPLY_ASSOC_TYPE_CALCULATION_FORMULAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_TYPE")]
        public ENTITY_LISTENTITYASSOC_TYPE[] ASSOC_TYPE
        {
            get
            {
                return this.aSSOC_TYPEField;
            }
            set
            {
                this.aSSOC_TYPEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_LISTENTITYASSOC_TYPE
    {

        private int eNTITY_ASSOC_TYPE_IDField;

        private string aSSOC_TYPE_NAMEField;

        private string aSSOC_TYPE_SYSTEM_CODEField;

        private string field_TYPEField;

        private string sECURITY_TYPEField;

        private ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA[] aSSOC_METADATAField;

        private int? lIST_DESKTOP_PRIORITY_VALUEField;


        private int? lIST_MOBILE_PRIORITY_VALUEField;


        private int? iTEM_DESKTOP_PRIORITY_VALUEField;


        private int? iTEM_MOBILE_PRIORITY_VALUEField;

        private int? eNTITY_CURRENCY_TYPE_IDField;

        private string cURRENCY_NAMEField;

        private string cURRENCY_INDICATORField;

        private string aSSOC_TYPE_SYSTEM_CODE_NAMEField;

        private string cALCULATION_FORMULAField;

        private int? cALCULATION_FREQUENCY_MINField;

        public string CALCULATION_FORMULA
        {
            get
            {
                return this.cALCULATION_FORMULAField;
            }
            set
            {
                this.cALCULATION_FORMULAField = value;
            }
        }


        public int? CALCULATION_FREQUENCY_MIN
        {
            get
            {
                return this.cALCULATION_FREQUENCY_MINField;
            }
            set
            {
                this.cALCULATION_FREQUENCY_MINField = value;
            }
        }


        private ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA_TEXT[] aSSOC_METADATA_TEXTField;

        /// <remarks/>
        public string ASSOC_TYPE_SYSTEM_CODE_NAME
        {
            get
            {
                return this.aSSOC_TYPE_SYSTEM_CODE_NAMEField;
            }
            set
            {
                this.aSSOC_TYPE_SYSTEM_CODE_NAMEField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_TYPE_NAME
        {
            get
            {
                return this.aSSOC_TYPE_NAMEField;
            }
            set
            {
                this.aSSOC_TYPE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_TYPE_SYSTEM_CODE
        {
            get
            {
                return this.aSSOC_TYPE_SYSTEM_CODEField;
            }
            set
            {
                this.aSSOC_TYPE_SYSTEM_CODEField = value;
            }
        }

        /// <remarks/>
        public string FIELD_TYPE
        {
            get
            {
                return this.field_TYPEField;
            }
            set
            {
                this.field_TYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string SECURITY_TYPE
        {
            get
            {
                return this.sECURITY_TYPEField;
            }
            set
            {
                this.sECURITY_TYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_METADATA")]
        public ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA[] ASSOC_METADATA
        {
            get
            {
                return this.aSSOC_METADATAField;
            }
            set
            {
                this.aSSOC_METADATAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? LIST_DESKTOP_PRIORITY_VALUE
        {
            get
            {
                return this.lIST_DESKTOP_PRIORITY_VALUEField;
            }
            set
            {
                this.lIST_DESKTOP_PRIORITY_VALUEField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? LIST_MOBILE_PRIORITY_VALUE
        {
            get
            {
                return this.lIST_MOBILE_PRIORITY_VALUEField;
            }
            set
            {
                this.lIST_MOBILE_PRIORITY_VALUEField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ITEM_DESKTOP_PRIORITY_VALUE
        {
            get
            {
                return this.iTEM_DESKTOP_PRIORITY_VALUEField;
            }
            set
            {
                this.iTEM_DESKTOP_PRIORITY_VALUEField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ITEM_MOBILE_PRIORITY_VALUE
        {
            get
            {
                return this.iTEM_MOBILE_PRIORITY_VALUEField;
            }
            set
            {
                this.iTEM_MOBILE_PRIORITY_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ENTITY_CURRENCY_TYPE_ID
        {
            get
            {
                return this.eNTITY_CURRENCY_TYPE_IDField;
            }
            set
            {
                this.eNTITY_CURRENCY_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CURRENCY_NAME
        {
            get
            {
                return this.cURRENCY_NAMEField;
            }
            set
            {
                this.cURRENCY_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CURRENCY_INDICATOR
        {
            get
            {
                return this.cURRENCY_INDICATORField;
            }
            set
            {
                this.cURRENCY_INDICATORField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_METADATA_TEXT")]
        public ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA_TEXT[] ASSOC_METADATA_TEXT
        {
            get
            {
                return this.aSSOC_METADATA_TEXTField;
            }
            set
            {
                this.aSSOC_METADATA_TEXTField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA
    {

        private int eNTITY_ASSOC_METADATA_IDField;

        private int eNTITY_ASSOC_TYPE_IDField;

        private string fIELD_VALUEField;

        private string fIELD_NAMEField;

        private string eXTERNAL_DATASOURCE_OBJECT_IDField;

        private int? eNTITY_ASSOC_DECODE_IDField;


        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ENTITY_ASSOC_DECODE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_DECODE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_DECODE_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ASSOC_METADATA_ID
        {
            get
            {
                return this.eNTITY_ASSOC_METADATA_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_METADATA_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string FIELD_VALUE
        {
            get
            {
                return this.fIELD_VALUEField;
            }
            set
            {
                this.fIELD_VALUEField = value;
            }
        }

        /// <remarks/>
        public string FIELD_NAME
        {
            get
            {
                return this.fIELD_NAMEField;
            }
            set
            {
                this.fIELD_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string EXTERNAL_DATASOURCE_OBJECT_ID
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_OBJECT_IDField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_OBJECT_IDField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_LISTENTITYASSOC_TYPEASSOC_METADATA_TEXT
    {

        private int eNTITY_ASSOC_TYPE_IDField;

        private int eNTITY_ASSOC_METADATA_TEXT_IDField;

        private string tEXTField;

        private int? eNTITY_FILE_IDField;

        private double? tRY_DECIMAL_VALUEField;

        private int? tRY_INTEGER_VALUEField;

        private string tRY_DATE_VALUEField;

        private string tRY_DATETIME_VALUEField;

        private double? tRY_CURRENCY_VALUEField;

        private string cALCULATION_EQUATIONField;

        private string cALCULATION_ERRORField;

        private int? cALCULATION_FREQUENCY_MINField;

        private string cALCULATION_LAST_RAN_DATETIMEField;

        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ASSOC_METADATA_TEXT_ID
        {
            get
            {
                return this.eNTITY_ASSOC_METADATA_TEXT_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_METADATA_TEXT_IDField = value;
            }
        }

        /// <remarks/>
        public string TEXT
        {
            get
            {
                return this.tEXTField;
            }
            set
            {
                this.tEXTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ENTITY_FILE_ID
        {
            get
            {
                return this.eNTITY_FILE_IDField;
            }
            set
            {
                this.eNTITY_FILE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public double? TRY_DECIMAL_VALUE
        {
            get
            {
                return this.tRY_DECIMAL_VALUEField;
            }
            set
            {
                this.tRY_DECIMAL_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? TRY_INTEGER_VALUE
        {
            get
            {
                return this.tRY_INTEGER_VALUEField;
            }
            set
            {
                this.tRY_INTEGER_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string TRY_DATE_VALUE
        {
            get
            {
                return this.tRY_DATE_VALUEField;
            }
            set
            {
                this.tRY_DATE_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string TRY_DATETIME_VALUE
        {
            get
            {
                return this.tRY_DATETIME_VALUEField;
            }
            set
            {
                this.tRY_DATETIME_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public double? TRY_CURRENCY_VALUE
        {
            get
            {
                return this.tRY_CURRENCY_VALUEField;
            }
            set
            {
                this.tRY_CURRENCY_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_EQUATION
        {
            get
            {
                return this.cALCULATION_EQUATIONField;
            }
            set
            {
                this.cALCULATION_EQUATIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_ERROR
        {
            get
            {
                return this.cALCULATION_ERRORField;
            }
            set
            {
                this.cALCULATION_ERRORField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? CALCULATION_FREQUENCY_MIN
        {
            get
            {
                return this.cALCULATION_FREQUENCY_MINField;
            }
            set
            {
                this.cALCULATION_FREQUENCY_MINField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_LAST_RAN_DATETIME
        {
            get
            {
                return this.cALCULATION_LAST_RAN_DATETIMEField;
            }
            set
            {
                this.cALCULATION_LAST_RAN_DATETIMEField = value;
            }
        }
    }

    //Secound

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ENTITY_TYPE
    {
        public EntityClass DeserializeEntity()
        {
            EntityClass entity = new EntityClass()
            {
                EntityTypeName = this.ENTITY_TYPE_NAME,
                EntityTypeID = this.ENTITY_TYPE_ID,
                InstanceSingular = this.INSTANCE_NAME,
                InstancePlural = this.INSTANCE_NAME_PLURAL,
                EntityTypeOwener = this.OWNER,
                EntityTypeHelpURL = this.HELP_URL,
                AssociationFieldCollection = new List<AssociationField>(),
                EntityID = this.ENTITY_ID,
                ListID = this.LIST_ID,
                EntityTypeRelationship = new List<EntityTypeRelationship>(),
                EntitiyAssignedToUserName = this.ENTITY_ASSIGNED_TO_USERNAME,
                EntitiyOwnedByUserName = this.ENTITY_OWNER_BY_USERNAME,
                EntityAssignedToDateTime = this.ENTITY_ASSIGNED_TO_DATETIME,
                EntityAssignedToEmail = this.ENTITY_ASSIGNED_TO_EMAIL,
                EntityAssignedToFullName = this.ENTITY_ASSIGNED_TO_FULLNAME,
                EntityAssignedToGUID = this.ENTITY_ASSIGNED_TO_GUID,
                EntityAssignedToPhone = this.ENTITY_ASSIGNED_TO_PHONE,
                EntityCreatedByEmail = this.ENTITY_CREATED_BY_EMAIL,
                EntityCreatedByFullName = this.ENTITY_CREATED_BY_FULLNAME,
                EntityCreatedByGUID = this.ENTITY_CREATED_BY_GUID,
                EntityCreatedByPhone = this.ENTITY_CREATED_BY_PHONE,
                EntityCreatedByUserName = this.ENTITY_CREATED_BY_USERNAME,
                EntityCreatedDateTime = this.ENTITY_CREATED_DATETIME,
                EntityModifiedByEmail = this.ENTITY_MODIFIED_BY_EMAIL,
                EntityModifiedByFullName = this.ENTITY_MODIFIED_BY_FULLNAME,
                EntityModifiedByGUID = this.ENTITY_MODIFIED_BY_GUID,
                EntityModifiedByPhone = this.ENTITY_MODIFIED_BY_PHONE,
                EntityModifiedByUserName = this.ENTITY_MODIFIED_BY_USERNAME,
                EntityModifiedDateTime = this.ENTITY_MODIFIED_DATETIME,
                EntityOwnedByDateTime = this.ENTITY_OWNER_DATETIME,
                EntityOwnedByEmail = this.ENTITY_OWNER_BY_EMAIL,
                EntityOwnedByFullName = this.ENTITY_OWNER_BY_FULLNAME,
                EntityOwnedByGUID = this.ENTITY_OWNER_BY_GUID,
                EntityOwnedByPhone = this.ENTITY_OWNER_BY_PHONE,
                MasterEntityID = this.MASTER_ENTITY_ID,
                MasterEntityRelationDescription = this.MASTER_RELATION_DESC,
                EntitySecurityType = this.ENTITY_SECURITY_TYPE,
                EntityTypeSecurityType = this.ENTITY_TYPE_SECURITY_TYPE,
                NewestNotesOnTop = this.NEWEST_NOTE_ON_TOP,
                HasRightsToConfigSecurity = (this.IS_ALLOW_SECURITY_CONFIG == "Y") ? true : false,
                EntityTypeSystemCode = this.SYSTEM_CODE,
                EntityTypeTemplates = new List<EntityTypeTemplate>(),
                ScreenConfigurationSettings = new List<EntityTypeScreenConfiguration>()
            };

            if (this.ENTITY_TYPE_RELATIONSHIP != null)
            {
                foreach (var item in this.ENTITY_TYPE_RELATIONSHIP)
                {
                    entity.EntityTypeRelationship.Add(new EntityTypeRelationship()
                    {
                        EntityTypeID = item.ENTITY_TYPE_ID,
                        EntityTypeName = item.ENTITY_TYPE_NAME,
                        InstanceName = item.INSTANCE_NAME,
                        InstanceNamePlural = item.INSTANCE_NAME_PLURAL,
                        EntityTypeSecurityType = item.SECURITY_TYPE,
                        IsExternalEntityType = item.IS_EXTERNAL_ENTITY_TYPE == "Y" ? true : false
                    });
                }
            }

            if (this.ENTITY_TYPE_SCREEN_ITEM != null)
            {
                foreach (var item in this.ENTITY_TYPE_SCREEN_ITEM)
                {
                    entity.ScreenConfigurationSettings.Add(new EntityTypeScreenConfiguration()
                    {
                        EntityAssocTypeScreenItemID = item.ENTITY_ASSOC_TYPE_SCREEN_ITEM_ID,
                        EntityScreenItemID = item.ENTITY_SCREEN_ITEM_ID,
                        ScreenItemDescription = item.SCREEN_ITEM_DESCRIPTION,
                        ScreenItemName = item.SCREEN_ITEM_NAME,
                        ScreenItemType = item.SCREEN_ITEM_TYPE
                    });
                }
            }

            if (this.ENTITY_TYPE_TEMPLATE != null)
            {
                foreach (var item in this.ENTITY_TYPE_TEMPLATE)
                {
                    entity.EntityTypeTemplates.Add(new EntityTypeTemplate()
                    {
                        EntityTemplateID = item.ENTITY_TEMPLATE_ID,
                        TemplateName = item.TEMPLATE_NAME,
                        TemplateDescription = item.TEMPLATE_DESCRIPTION,
                        TemplateServerFilePath = item.TEMPLATE_SERVER_FILE_PATH,
                        TemplateType = item.TEMPLATE_TYPE
                    });
                }
            }
            if (this.ASSOC_TYPE != null)
            {
                foreach (var item in this.ASSOC_TYPE)
                {
                    AssociationField eAssociationField = new AssociationField()
                    {
                        EntityTypeID = item.ENTITY_TYPE_ID,
                        AssocTypeID = item.ENTITY_ASSOC_TYPE_ID,
                        FieldType = item.FIELD_TYPE,
                        AssocName = item.ASSOC_TYPE_NAME,
                        CurrencyTypeID = item.ENTITY_CURRENCY_TYPE_ID,
                        CurrencyIndicator = item.ENTITY_CURRENCY_TYPE_INDICATOR,
                        DecimalPrecesion = item.DECIMAL_PRECESION,
                        ExternalDataSourceID = item.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID,
                        IsRequired = item.IS_REQUIRED,
                        IsReadOnly = item.IS_READ_ONLY,
                        ItemDesktopPriorityValue = item.ITEM_DESKTOP_PRIORITY_VALUE,
                        ItemMobilePriorityValue = item.ITEM_MOBILE_PRIORITY_VALUE,
                        ListDesktopPriorityValue = item.LIST_DESKTOP_PRIORITY_VALUE,
                        ListMobilePriorityValue = item.LIST_MOBILE_PRIORITY_VALUE,
                        AssocShowOnList = item.SHOW_ON_LIST,
                        AssocSystemCode = item.ASSOC_TYPE_SYSTEM_CODE,
                        AllowedRegex = item.ALLOWED_REGEX,
                        FieldSubType = item.FIELD_SUB_TYPE,
                        MaxLength = item.MAX_LENGTH,
                        SecurityType = item.SECURITY_TYPE,
                        CalculationFormula = item.CALCULATION_FORMULA,
                        EnableForceCalculationOnFormula = item.IS_FORCE_RECALCULATION,
                        AssocDecode = new List<AssociationFieldValue>(),
                        AssocMetaData = new List<AssociationMetaData>(),
                        AssocMetaDataText = new List<AssociationMetaDataText>(),
                        EntityAssocTypeCascade = new List<EntityAssocTypeCascade>(),
                        IsExternalEntityAssocType = item.IS_EXTERNALENTITIES_ASSOC_TYPE == "Y" ? true : false
                    };

                    if (item.ASSOC_METADATA_TEXT != null)
                    {
                        foreach (var subItem in item.ASSOC_METADATA_TEXT)
                        {
                            eAssociationField.AssocMetaDataText.Add(new AssociationMetaDataText()
                            {
                                AssocMetaDataTextID = subItem.ENTITY_ASSOC_METADATA_TEXT_ID,
                                AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID,
                                EntityID = subItem.ENTITY_ID,
                                TextValue = subItem.TEXT,
                                CalculationEquation = subItem.CALCULATION_EQUATION,
                                CalculationError = subItem.CALCULATION_ERROR,
                                CalculationFrequencyMin = subItem.CALCULATION_FREQUENCY_MIN,
                                CalculationLastRanDateTime = subItem.CALCULATION_LAST_RAN_DATETIME,
                                EntityFileID = subItem.ENTITY_FILE_ID,
                                TryDateTimeValue = subItem.TRY_DATETIME_VALUE,
                                TryDateValue = subItem.TRY_DATE_VALUE,
                                TryDecimalValue = subItem.TRY_DECIMAL_VALUE,
                                TryIntegerValue = subItem.TRY_INTEGER_VALUE,
                                TryCurrencyValue = subItem.TRY_CURRENCY_VALUE
                            });
                        }
                    }

                    if (item.ASSOC_METADATA != null)
                    {
                        foreach (var subItem in item.ASSOC_METADATA)
                        {

                            AssociationMetaData metaData = new AssociationMetaData();

                            metaData.AssocMetaDataID = subItem.ENTITY_ASSOC_METADATA_ID;
                            metaData.AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID;
                            metaData.AssocDecodeID = subItem.ENTITY_ASSOC_DECODE_ID;
                            metaData.EntityID = this.ENTITY_ID;

                            //metaData.ExternalDatasourceObjectID = subItem.EXTERNAL_DATASOURCE_OBJECT_ID;

                            if (subItem.EXTERNAL_DATASOURCE_OBJECT_ID == null)
                            {
                                metaData.ExternalDatasourceObjectID = subItem.ENTITY_ASSOC_DECODE_ID.ToString();
                            }
                            else
                            {
                                metaData.ExternalDatasourceObjectID = subItem.EXTERNAL_DATASOURCE_OBJECT_ID;
                            }

                            subItem.EXTERNAL_DATASOURCE_OBJECT_ID = subItem.EXTERNAL_DATASOURCE_OBJECT_ID ?? Convert.ToString(subItem.ENTITY_ASSOC_DECODE_ID);

                            metaData.FieldName = subItem.FIELD_NAME;
                            metaData.FieldValue = subItem.FIELD_VALUE;
                            if (metaData.AssocMetaDataID != 0)
                            { eAssociationField.AssocMetaData.Add(metaData); }
                        }
                    }


                    if (item.ASSOC_DECODE != null)
                    {
                        foreach (var subItem in item.ASSOC_DECODE)
                        {
                            eAssociationField.AssocDecode.Add(new AssociationFieldValue()
                            {
                                AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID,
                                AssocDecodeID = subItem.ENTITY_ASSOC_DECODE_ID,
                                AssocDecodeName = subItem.ASSOC_DECODE_NAME,
                                AssocDecodeSystemCode = subItem.ASSOC_DECODE_SYSTEM_CODE,
                                DisplayPriority = subItem.DISPLAY_PRIORITY
                            });
                        }
                    }

                    if (item.EXTERNAL_DATASOURCE != null)
                    {
                        eAssociationField.ExternalDataSource = new ExternalDatasource()
                        {
                            ExternalDatasourceID = item.EXTERNAL_DATASOURCE.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID,
                            DataSourceDescription = item.EXTERNAL_DATASOURCE.EXTERNAL_DATASOURCE_DESCRIPTION,
                            DataSourceName = item.EXTERNAL_DATASOURCE.EXTERNAL_DATASOURCE_NAME,
                            FieldSystemCode = item.EXTERNAL_DATASOURCE.FIELD_SYSTEM_CODE,
                            ObjectDescription = item.EXTERNAL_DATASOURCE.OBJECT_DESCRIPTION,
                            ObjectDisplay = item.EXTERNAL_DATASOURCE.OBJECT_DISPLAY,
                            ObjectId = item.EXTERNAL_DATASOURCE.OBJECT_ID,
                            URLDrillInto = item.EXTERNAL_DATASOURCE.URL_DRILL_INTO,
                            ValueSystemCode = item.EXTERNAL_DATASOURCE.VALUE_SYSTEM_CODE,
                            Query = item.EXTERNAL_DATASOURCE.QUERY,
                            // ConnectionString = StringCipher.Decrypt(item.EXTERNAL_DATASOURCE.CONNECTION_STRING)
                        };
                    }

                    if (item.ENTITY_ASSOC_TYPE_CASCADE != null)
                    {
                        foreach (var subItem in item.ENTITY_ASSOC_TYPE_CASCADE)
                        {
                            eAssociationField.EntityAssocTypeCascade.Add(new EntityAssocTypeCascade()
                            {
                                EntityAssocTypeCascadeID = subItem.ENTITY_ASSOC_TYPE_CASCADE_ID,
                                CasCadeType = subItem.CASCADE_TYPE,
                                EntityAssocTypeIDParent = subItem.ENTITY_ASSOC_TYPE_ID_PARENT,
                                EntityAssocTypeNameParent = subItem.ENTITY_ASSOC_TYPE_NAME_PARENT,
                                EntityAssocExternalDatasourceIDParent = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_PARENT,
                                EntityAssocExternalDatasourceNameParent = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_PARENT,
                                FilterQueryParent = subItem.FILTER_QUERY_PARENT,
                                EntityAssocTypeIDChild = subItem.ENTITY_ASSOC_TYPE_ID_CHILD,
                                EntityAssocTypeNameChild = subItem.ENTITY_ASSOC_TYPE_NAME_CHILD,
                                EntityAssocExternalDatasourceIDChild = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_CHILD,
                                EntityAssocExternalDatasourceNameChild = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_CHILD,
                                FilterQueryChild = subItem.FILTER_QUERY_CHILD
                            });
                        }
                    }



                    entity.AssociationFieldCollection.Add(eAssociationField);
                }


            }
            return entity;
        }


        public EntityType DeserializeEntityType()
        {
            EntityType entityType = new EntityType()
            {
                EntityTypeName = this.ENTITY_TYPE_NAME,
                EntityTypeID = this.ENTITY_TYPE_ID,
                InstanceSingular = this.INSTANCE_NAME,
                InstancePlural = this.INSTANCE_NAME_PLURAL,
                EntityTypeOwener = this.OWNER,
                EntityTypeHelpURL = this.HELP_URL,
                EntityTypeSecurityType = this.ENTITY_TYPE_SECURITY_TYPE,
                AssociationFieldCollection = new List<AssociationField>(),
                EntityTypeTemplates = new List<EntityTypeTemplate>(),
                HasExternalEntityAssocType = this.HAS_EXTERNALENTITIES_ASSOC_TYPE == "Y" ? true : false,
                EntityTypeRelationship = new List<EntityTypeRelationship>()

            };
            if (this.ENTITY_TYPE_RELATIONSHIP != null)
            {
                foreach (var item in this.ENTITY_TYPE_RELATIONSHIP)
                {
                    entityType.EntityTypeRelationship.Add(new EntityTypeRelationship()
                    {

                        EntityTypeID = item.ENTITY_TYPE_ID,
                        EntityTypeName = item.ENTITY_TYPE_NAME,
                        InstanceName = item.INSTANCE_NAME,
                        InstanceNamePlural = item.INSTANCE_NAME_PLURAL,
                        EntityTypeSecurityType = item.SECURITY_TYPE,
                        IsExternalEntityType = Convert.ToBoolean(item.IS_EXTERNAL_ENTITY_TYPE)

                    });
                }
            }
            if (this.ENTITY_TYPE_TEMPLATE != null)
            {
                foreach (var item in this.ENTITY_TYPE_TEMPLATE)
                {
                    entityType.EntityTypeTemplates.Add(new EntityTypeTemplate()
                    {
                        EntityTemplateID = item.ENTITY_TEMPLATE_ID,
                        TemplateName = item.TEMPLATE_NAME,
                        TemplateDescription = item.TEMPLATE_DESCRIPTION,
                        TemplateServerFilePath = item.TEMPLATE_SERVER_FILE_PATH,
                        TemplateType = item.TEMPLATE_TYPE
                    });
                }
            }
            if (this.ASSOC_TYPE != null)
            {
                foreach (var item in this.ASSOC_TYPE)
                {
                    AssociationField eAssociationField = new AssociationField()
                    {
                        EntityTypeID = item.ENTITY_TYPE_ID,
                        AssocTypeID = item.ENTITY_ASSOC_TYPE_ID,
                        FieldType = item.FIELD_TYPE,
                        AssocName = item.ASSOC_TYPE_NAME,
                        CurrencyTypeID = item.ENTITY_CURRENCY_TYPE_ID,
                        CurrencyIndicator = item.ENTITY_CURRENCY_TYPE_INDICATOR,
                        DecimalPrecesion = item.DECIMAL_PRECESION,
                        ExternalDataSourceID = item.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID,
                        IsRequired = item.IS_REQUIRED,
                        IsReadOnly = item.IS_READ_ONLY,
                        ItemDesktopPriorityValue = item.ITEM_DESKTOP_PRIORITY_VALUE,
                        ItemMobilePriorityValue = item.ITEM_MOBILE_PRIORITY_VALUE,
                        ListDesktopPriorityValue = item.LIST_DESKTOP_PRIORITY_VALUE,
                        ListMobilePriorityValue = item.LIST_MOBILE_PRIORITY_VALUE,
                        AssocShowOnList = item.SHOW_ON_LIST,
                        AssocSystemCode = item.ASSOC_TYPE_SYSTEM_CODE,
                        AllowedRegex = item.ALLOWED_REGEX,
                        FieldSubType = item.FIELD_SUB_TYPE,
                        MaxLength = item.MAX_LENGTH,
                        SecurityType = item.SECURITY_TYPE,
                        AssocDecode = new List<AssociationFieldValue>(),
                        AssocMetaData = new List<AssociationMetaData>(),
                        AssocMetaDataText = new List<AssociationMetaDataText>(),
                        CalculationFormula = item.CALCULATION_FORMULA,
                        CalculationFrequencyMin = item.CALCULATION_FREQUENCY_MIN,
                        EntityAssocTypeCascade = new List<EntityAssocTypeCascade>(),
                        IsExternalEntityAssocType = item.IS_EXTERNALENTITIES_ASSOC_TYPE == "Y" ? true : false
                    };

                    if (item.ASSOC_DECODE != null)
                    {
                        foreach (var subItem in item.ASSOC_DECODE)
                        {
                            eAssociationField.AssocDecode.Add(new AssociationFieldValue()
                            {
                                AssocTypeID = subItem.ENTITY_ASSOC_TYPE_ID,
                                AssocDecodeID = subItem.ENTITY_ASSOC_DECODE_ID,
                                AssocDecodeName = subItem.ASSOC_DECODE_NAME,
                                AssocDecodeSystemCode = subItem.ASSOC_DECODE_SYSTEM_CODE,
                                DisplayPriority = subItem.DISPLAY_PRIORITY
                            });
                        }
                    }

                    if (item.EXTERNAL_DATASOURCE != null)
                    {
                        eAssociationField.ExternalDataSource = new ExternalDatasource()
                        {
                            ExternalDatasourceID = item.EXTERNAL_DATASOURCE.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID,
                            DataSourceDescription = item.EXTERNAL_DATASOURCE.EXTERNAL_DATASOURCE_DESCRIPTION,
                            DataSourceName = item.EXTERNAL_DATASOURCE.EXTERNAL_DATASOURCE_NAME,
                            FieldSystemCode = item.EXTERNAL_DATASOURCE.FIELD_SYSTEM_CODE,
                            ObjectDescription = item.EXTERNAL_DATASOURCE.OBJECT_DESCRIPTION,
                            ObjectDisplay = item.EXTERNAL_DATASOURCE.OBJECT_DISPLAY,
                            ObjectId = item.EXTERNAL_DATASOURCE.OBJECT_ID,
                            URLDrillInto = item.EXTERNAL_DATASOURCE.URL_DRILL_INTO,
                            ValueSystemCode = item.EXTERNAL_DATASOURCE.VALUE_SYSTEM_CODE,
                            Query = item.EXTERNAL_DATASOURCE.QUERY,
                            ConnectionString = item.EXTERNAL_DATASOURCE.CONNECTION_STRING,


                        };
                    }


                    if (item.ENTITY_ASSOC_TYPE_CASCADE != null)
                    {
                        foreach (var subItem in item.ENTITY_ASSOC_TYPE_CASCADE)
                        {
                            eAssociationField.EntityAssocTypeCascade.Add(new EntityAssocTypeCascade()
                            {
                                EntityAssocTypeCascadeID = subItem.ENTITY_ASSOC_TYPE_CASCADE_ID,
                                CasCadeType = subItem.CASCADE_TYPE,
                                EntityAssocTypeIDParent = subItem.ENTITY_ASSOC_TYPE_ID_PARENT,
                                EntityAssocTypeNameParent = subItem.ENTITY_ASSOC_TYPE_NAME_PARENT,
                                EntityAssocExternalDatasourceIDParent = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_PARENT,
                                EntityAssocExternalDatasourceNameParent = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_PARENT,
                                FilterQueryParent = subItem.FILTER_QUERY_PARENT,
                                EntityAssocTypeIDChild = subItem.ENTITY_ASSOC_TYPE_ID_CHILD,
                                EntityAssocTypeNameChild = subItem.ENTITY_ASSOC_TYPE_NAME_CHILD,
                                EntityAssocExternalDatasourceIDChild = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_CHILD,
                                EntityAssocExternalDatasourceNameChild = subItem.ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_CHILD,
                                FilterQueryChild = subItem.FILTER_QUERY_CHILD
                            });
                        }
                    }


                    entityType.AssociationFieldCollection.Add(eAssociationField);
                }
            }
            return entityType;
        }

        private int eNTITY_TYPE_IDField;

        private string eNTITY_TYPE_SYSTEM_CODEField;

        private string eNTITY_TYPE_NAMEField;

        private string iNSTANCE_NAMEField;

        private string iNSTANCE_NAME_PLURALField;

        private string oWNERField;

        private string nEWEST_NOTE_ON_TOPField;

        private string hELP_URLField;

        private int eNTITY_IDField;

        private int? lIST_IDField;

        private string eNTITY_TYPE_SECURITY_TYPEField;

        private string eNTITY_SECURITY_TYPEField;

        private int? mASTER_ENTITY_IDField;

        private string mASTER_RELATION_DESCField;

        private string eNTITY_CREATED_BY_USERNAMEField;

        private string eNTITY_CREATED_BY_FULLNAMEField;

        private string eNTITY_CREATED_BY_PHONEField;

        private string eNTITY_CREATED_BY_EMAILField;

        private Guid? eNTITY_CREATED_BY_GUIDField;

        private string eNTITY_CREATED_DATETIMEField;

        private string eNTITY_MODIFIED_BY_USERNAMEField;

        private string eNTITY_MODIFIED_BY_FULLNAMEField;

        private string eNTITY_MODIFIED_BY_PHONEField;

        private string eNTITY_MODIFIED_BY_EMAILField;

        private Guid? eNTITY_MODIFIED_BY_GUIDField;

        private string eNTITY_MODIFIED_DATETIMEField;

        private string eNTITY_ASSIGNED_TO_USERNAMEField;

        private string eNTITY_ASSIGNED_TO_FULLNAMEField;

        private string eNTITY_ASSIGNED_TO_PHONEField;

        private string eNTITY_ASSIGNED_TO_EMAILField;

        private Guid? eNTITY_ASSIGNED_TO_GUIDField;

        private string eNTITY_ASSIGNED_TO_DATETIMEField;

        private string eNTITY_OWNER_BY_USERNAMEField;

        private string eNTITY_OWNER_BY_FULLNAMEField;

        private string eNTITY_OWNER_BY_PHONEField;

        private string eNTITY_OWNER_BY_EMAILField;

        private Guid? eNTITY_OWNER_BY_GUIDField;

        private string eNTITY_OWNER_DATETIMEField;

        private string iS_ALLOW_SECURITY_CONFIGField;

        private string hAS_EXTERNALENTITIES_ASSOC_TYPEField;

        private ENTITY_TYPEENTITY_TYPE_RELATIONSHIP[] eNTITY_TYPE_RELATIONSHIPField;

        private ENTITY_TYPEASSOC_TYPE[] aSSOC_TYPEField;


        private ENTITY_TYPEENTITY_TYPE_TEMPLATE[] eNTITY_TYPE_TEMPLATEField;

        private ENTITY_TYPEENTITY_TYPE_SCREEN_ITEM[] eNTITY_TYPE_SCREEN_ITEMField;


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_TYPE_TEMPLATE", IsNullable = true)]
        public ENTITY_TYPEENTITY_TYPE_TEMPLATE[] ENTITY_TYPE_TEMPLATE
        {
            get
            {
                return this.eNTITY_TYPE_TEMPLATEField;
            }
            set
            {
                this.eNTITY_TYPE_TEMPLATEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_TYPE_SCREEN_ITEM", IsNullable = true)]
        public ENTITY_TYPEENTITY_TYPE_SCREEN_ITEM[] ENTITY_TYPE_SCREEN_ITEM
        {
            get
            {
                return this.eNTITY_TYPE_SCREEN_ITEMField;
            }
            set
            {
                this.eNTITY_TYPE_SCREEN_ITEMField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_TYPE_ID
        {
            get
            {
                return this.eNTITY_TYPE_IDField;
            }
            set
            {
                this.eNTITY_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ID
        {
            get
            {
                return this.eNTITY_IDField;
            }
            set
            {
                this.eNTITY_IDField = value;
            }
        }

        /// <remarks/>
        public int? LIST_ID
        {
            get
            {
                return this.lIST_IDField;
            }
            set
            {
                this.lIST_IDField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_TYPE_NAME
        {
            get
            {
                return this.eNTITY_TYPE_NAMEField;
            }
            set
            {
                this.eNTITY_TYPE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string SYSTEM_CODE
        {
            get
            {
                return this.eNTITY_TYPE_SYSTEM_CODEField;
            }
            set
            {
                this.eNTITY_TYPE_SYSTEM_CODEField = value;
            }
        }

        /// <remarks/>
        public string INSTANCE_NAME
        {
            get
            {
                return this.iNSTANCE_NAMEField;
            }
            set
            {
                this.iNSTANCE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string INSTANCE_NAME_PLURAL
        {
            get
            {
                return this.iNSTANCE_NAME_PLURALField;
            }
            set
            {
                this.iNSTANCE_NAME_PLURALField = value;
            }
        }

        /// <remarks/>
        public string OWNER
        {
            get
            {
                return this.oWNERField;
            }
            set
            {
                this.oWNERField = value;
            }
        }

        /// <remarks/>
        public string NEWEST_NOTE_ON_TOP
        {
            get
            {
                return this.nEWEST_NOTE_ON_TOPField;
            }
            set
            {
                this.nEWEST_NOTE_ON_TOPField = value;
            }
        }

        /// <remarks/>
        public string HELP_URL
        {
            get
            {
                return this.hELP_URLField;
            }
            set
            {
                this.hELP_URLField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_TYPE_SECURITY_TYPE
        {
            get
            {
                return this.eNTITY_TYPE_SECURITY_TYPEField;
            }
            set
            {
                this.eNTITY_TYPE_SECURITY_TYPEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_SECURITY_TYPE
        {
            get
            {
                return this.eNTITY_SECURITY_TYPEField;
            }
            set
            {
                this.eNTITY_SECURITY_TYPEField = value;
            }
        }



        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? MASTER_ENTITY_ID
        {
            get
            {
                return this.mASTER_ENTITY_IDField;
            }
            set
            {
                this.mASTER_ENTITY_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string MASTER_RELATION_DESC
        {
            get
            {
                return this.mASTER_RELATION_DESCField;
            }
            set
            {
                this.mASTER_RELATION_DESCField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_USERNAME
        {
            get
            {
                return this.eNTITY_CREATED_BY_USERNAMEField;
            }
            set
            {
                this.eNTITY_CREATED_BY_USERNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_FULLNAME
        {
            get
            {
                return this.eNTITY_CREATED_BY_FULLNAMEField;
            }
            set
            {
                this.eNTITY_CREATED_BY_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_PHONE
        {
            get
            {
                return this.eNTITY_CREATED_BY_PHONEField;
            }
            set
            {
                this.eNTITY_CREATED_BY_PHONEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_BY_EMAIL
        {
            get
            {
                return this.eNTITY_CREATED_BY_EMAILField;
            }
            set
            {
                this.eNTITY_CREATED_BY_EMAILField = value;
            }
        }

        /// <remarks/>
        public Guid? ENTITY_CREATED_BY_GUID
        {
            get
            {
                return this.eNTITY_CREATED_BY_GUIDField;
            }
            set
            {
                this.eNTITY_CREATED_BY_GUIDField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_CREATED_DATETIME
        {
            get
            {
                return this.eNTITY_CREATED_DATETIMEField;
            }
            set
            {
                this.eNTITY_CREATED_DATETIMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_MODIFIED_BY_USERNAME
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_USERNAMEField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_USERNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_MODIFIED_BY_FULLNAME
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_FULLNAMEField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_MODIFIED_BY_PHONE
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_PHONEField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_PHONEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_MODIFIED_BY_EMAIL
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_EMAILField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_EMAILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Guid? ENTITY_MODIFIED_BY_GUID
        {
            get
            {
                return this.eNTITY_MODIFIED_BY_GUIDField;
            }
            set
            {
                this.eNTITY_MODIFIED_BY_GUIDField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_MODIFIED_DATETIME
        {
            get
            {
                return this.eNTITY_MODIFIED_DATETIMEField;
            }
            set
            {
                this.eNTITY_MODIFIED_DATETIMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_USERNAME
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_USERNAMEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_USERNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_FULLNAME
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_FULLNAMEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_PHONE
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_PHONEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_PHONEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_EMAIL
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_EMAILField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_EMAILField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public Guid? ENTITY_ASSIGNED_TO_GUID
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_GUIDField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_GUIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_ASSIGNED_TO_DATETIME
        {
            get
            {
                return this.eNTITY_ASSIGNED_TO_DATETIMEField;
            }
            set
            {
                this.eNTITY_ASSIGNED_TO_DATETIMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_USERNAME
        {
            get
            {
                return this.eNTITY_OWNER_BY_USERNAMEField;
            }
            set
            {
                this.eNTITY_OWNER_BY_USERNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_FULLNAME
        {
            get
            {
                return this.eNTITY_OWNER_BY_FULLNAMEField;
            }
            set
            {
                this.eNTITY_OWNER_BY_FULLNAMEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_PHONE
        {
            get
            {
                return this.eNTITY_OWNER_BY_PHONEField;
            }
            set
            {
                this.eNTITY_OWNER_BY_PHONEField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_BY_EMAIL
        {
            get
            {
                return this.eNTITY_OWNER_BY_EMAILField;
            }
            set
            {
                this.eNTITY_OWNER_BY_EMAILField = value;
            }
        }

        /// <remarks/>
        public Guid? ENTITY_OWNER_BY_GUID
        {
            get
            {
                return this.eNTITY_OWNER_BY_GUIDField;
            }
            set
            {
                this.eNTITY_OWNER_BY_GUIDField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_OWNER_DATETIME
        {
            get
            {
                return this.eNTITY_OWNER_DATETIMEField;
            }
            set
            {
                this.eNTITY_OWNER_DATETIMEField = value;
            }
        }

        /// <remarks/>
        public string IS_ALLOW_SECURITY_CONFIG
        {
            get
            {
                return this.iS_ALLOW_SECURITY_CONFIGField;
            }
            set
            {
                this.iS_ALLOW_SECURITY_CONFIGField = value;
            }
        }

        /// <remarks/>
        public string HAS_EXTERNALENTITIES_ASSOC_TYPE
        {
            get
            {
                return this.hAS_EXTERNALENTITIES_ASSOC_TYPEField;
            }
            set
            {
                this.hAS_EXTERNALENTITIES_ASSOC_TYPEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_TYPE_RELATIONSHIP")]
        public ENTITY_TYPEENTITY_TYPE_RELATIONSHIP[] ENTITY_TYPE_RELATIONSHIP
        {
            get
            {
                return this.eNTITY_TYPE_RELATIONSHIPField;
            }
            set
            {
                this.eNTITY_TYPE_RELATIONSHIPField = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_TYPE")]
        public ENTITY_TYPEASSOC_TYPE[] ASSOC_TYPE
        {
            get
            {
                return this.aSSOC_TYPEField;
            }
            set
            {
                this.aSSOC_TYPEField = value;
            }
        }



    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEENTITY_TYPE_RELATIONSHIP
    {

        private string eNTITY_TYPE_IDField;

        private string eNTITY_TYPE_NAMEField;

        private string iNSTANCE_NAMEField;

        private string iNSTANCE_NAME_PLURALField;

        private string sECURITY_TYPEField;

        private string iS_EXTERNAL_ENTITY_TYPEField;

        /// <remarks/>
        public string ENTITY_TYPE_ID
        {
            get
            {
                return this.eNTITY_TYPE_IDField;
            }
            set
            {
                this.eNTITY_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string ENTITY_TYPE_NAME
        {
            get
            {
                return this.eNTITY_TYPE_NAMEField;
            }
            set
            {
                this.eNTITY_TYPE_NAMEField = value;
            }
        }

        public string INSTANCE_NAME
        {
            get
            {
                return this.iNSTANCE_NAMEField;
            }
            set
            {
                this.iNSTANCE_NAMEField = value;
            }
        }

        public string INSTANCE_NAME_PLURAL
        {
            get
            {
                return this.iNSTANCE_NAME_PLURALField;
            }
            set
            {
                this.iNSTANCE_NAME_PLURALField = value;
            }
        }

        public string SECURITY_TYPE
        {
            get
            {
                return this.sECURITY_TYPEField;
            }
            set
            {
                this.sECURITY_TYPEField = value;
            }
        }

        public string IS_EXTERNAL_ENTITY_TYPE
        {
            get
            {
                return this.iS_EXTERNAL_ENTITY_TYPEField;
            }
            set
            {
                this.iS_EXTERNAL_ENTITY_TYPEField = value;
            }
        }



    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEENTITY_TYPE_TEMPLATE
    {

        private int eNTITY_TEMPLATE_IDField;

        private string tEMPLATE_NAMEField;

        private string tEMPLATE_DESCRIPTIONField;

        private string tEMPLATE_SERVER_FILE_PATHField;

        private string tEMPLATE_TYPEField;

        /// <remarks/>
        public int ENTITY_TEMPLATE_ID
        {
            get
            {
                return this.eNTITY_TEMPLATE_IDField;
            }
            set
            {
                this.eNTITY_TEMPLATE_IDField = value;
            }
        }

        /// <remarks/>
        public string TEMPLATE_NAME
        {
            get
            {
                return this.tEMPLATE_NAMEField;
            }
            set
            {
                this.tEMPLATE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string TEMPLATE_DESCRIPTION
        {
            get
            {
                return this.tEMPLATE_DESCRIPTIONField;
            }
            set
            {
                this.tEMPLATE_DESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        public string TEMPLATE_SERVER_FILE_PATH
        {
            get
            {
                return this.tEMPLATE_SERVER_FILE_PATHField;
            }
            set
            {
                this.tEMPLATE_SERVER_FILE_PATHField = value;
            }
        }

        /// <remarks/>
        public string TEMPLATE_TYPE
        {
            get
            {
                return this.tEMPLATE_TYPEField;
            }
            set
            {
                this.tEMPLATE_TYPEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEENTITY_TYPE_SCREEN_ITEM
    {

        private int eNTITY_ASSOC_TYPE_SCREEN_ITEM_IDField;

        private int eNTITY_SCREEN_ITEM_IDField;

        private string sCREEN_ITEM_NAMEField;

        private string sCREEN_ITEM_DESCRIPTIONField;

        private string sCREEN_ITEM_TYPEField;

        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_SCREEN_ITEM_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_SCREEN_ITEM_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_SCREEN_ITEM_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_SCREEN_ITEM_ID
        {
            get
            {
                return this.eNTITY_SCREEN_ITEM_IDField;
            }
            set
            {
                this.eNTITY_SCREEN_ITEM_IDField = value;
            }
        }

        /// <remarks/>
        public string SCREEN_ITEM_NAME
        {
            get
            {
                return this.sCREEN_ITEM_NAMEField;
            }
            set
            {
                this.sCREEN_ITEM_NAMEField = value;
            }
        }

        /// <remarks/>
        public string SCREEN_ITEM_DESCRIPTION
        {
            get
            {
                return this.sCREEN_ITEM_DESCRIPTIONField;
            }
            set
            {
                this.sCREEN_ITEM_DESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        public string SCREEN_ITEM_TYPE
        {
            get
            {
                return this.sCREEN_ITEM_TYPEField;
            }
            set
            {
                this.sCREEN_ITEM_TYPEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEASSOC_TYPE
    {

        private int eNTITY_ASSOC_TYPE_IDField;

        private int eNTITY_TYPE_IDField;

        private string sECURITY_TYPEField;

        private string fIELD_TYPEField;

        private string aSSOC_TYPE_NAMEField;

        private int? eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField;

        private string aSSOC_TYPE_SYSTEM_CODEField;

        private int? lIST_DESKTOP_PRIORITY_VALUEField;

        private int? lIST_MOBILE_PRIORITY_VALUEField;

        private int? iTEM_DESKTOP_PRIORITY_VALUEField;

        private int? iTEM_MOBILE_PRIORITY_VALUEField;

        private string sHOW_ON_LISTField;

        private string iS_REQUIREDField;

        private string fIELD_SUB_TYPEField;

        private int? eNTITY_CURRENCY_TYPE_IDField;

        private string eNTITY_CURRENCY_TYPE_INDICATORField;

        private int? mAX_LENGTHField;

        private string aLLOWED_REGEXField;

        private int? dECIMAL_PRECESIONField;

        private string iS_READ_ONLYField;

        private string cALCULATION_FORMULAField;

        private int? cALCULATION_FREQUENCY_MINField;

        private string iS_FORCE_RECALCULATIONField;

        private string iS_EXTERNALENTITIES_ASSOC_TYPEField;

        private ENTITY_TYPEASSOC_TYPEASSOC_DECODE[] aSSOC_DECODEField;

        private ENTITY_TYPEASSOC_TYPEASSOC_METADATA[] aSSOC_METADATAField;

        private ENTITY_TYPEASSOC_TYPEASSOC_METADATA_TEXT[] aSSOC_METADATA_TEXTField;

        private ENTITY_TYPEASSOC_TYPEEXTERNAL_DATASOURCE eXTERNAL_DATASOURCEField;

        private ENTITY_ASSOC_TYPE_CASCADE[] eENTITY_ASSOC_TYPE_CASCADE;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_FORMULA
        {
            get
            {
                return this.cALCULATION_FORMULAField;
            }
            set
            {
                this.cALCULATION_FORMULAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string IS_FORCE_RECALCULATION
        {
            get
            {
                return this.iS_FORCE_RECALCULATIONField;
            }
            set
            {
                this.iS_FORCE_RECALCULATIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? CALCULATION_FREQUENCY_MIN
        {
            get
            {
                return this.cALCULATION_FREQUENCY_MINField;
            }
            set
            {
                this.cALCULATION_FREQUENCY_MINField = value;
            }
        }


        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_TYPE_ID
        {
            get
            {
                return this.eNTITY_TYPE_IDField;
            }
            set
            {
                this.eNTITY_TYPE_IDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string SECURITY_TYPE
        {
            get
            {
                return this.sECURITY_TYPEField;
            }
            set
            {
                this.sECURITY_TYPEField = value;
            }
        }

        /// <remarks/>
        public string FIELD_TYPE
        {
            get
            {
                return this.fIELD_TYPEField;
            }
            set
            {
                this.fIELD_TYPEField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_TYPE_NAME
        {
            get
            {
                return this.aSSOC_TYPE_NAMEField;
            }
            set
            {
                this.aSSOC_TYPE_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ASSOC_TYPE_SYSTEM_CODE
        {
            get
            {
                return this.aSSOC_TYPE_SYSTEM_CODEField;
            }
            set
            {
                this.aSSOC_TYPE_SYSTEM_CODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? LIST_DESKTOP_PRIORITY_VALUE
        {
            get
            {
                return this.lIST_DESKTOP_PRIORITY_VALUEField;
            }
            set
            {
                this.lIST_DESKTOP_PRIORITY_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? LIST_MOBILE_PRIORITY_VALUE
        {
            get
            {
                return this.lIST_MOBILE_PRIORITY_VALUEField;
            }
            set
            {
                this.lIST_MOBILE_PRIORITY_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ITEM_DESKTOP_PRIORITY_VALUE
        {
            get
            {
                return this.iTEM_DESKTOP_PRIORITY_VALUEField;
            }
            set
            {
                this.iTEM_DESKTOP_PRIORITY_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ITEM_MOBILE_PRIORITY_VALUE
        {
            get
            {
                return this.iTEM_MOBILE_PRIORITY_VALUEField;
            }
            set
            {
                this.iTEM_MOBILE_PRIORITY_VALUEField = value;
            }
        }

        /// <remarks/>
        public string SHOW_ON_LIST
        {
            get
            {
                return this.sHOW_ON_LISTField;
            }
            set
            {
                this.sHOW_ON_LISTField = value;
            }
        }

        /// <remarks/>
        public string IS_REQUIRED
        {
            get
            {
                return this.iS_REQUIREDField;
            }
            set
            {
                this.iS_REQUIREDField = value;
            }
        }

        public string IS_READ_ONLY
        {
            get
            {
                return this.iS_READ_ONLYField;
            }
            set
            {
                this.iS_READ_ONLYField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string FIELD_SUB_TYPE
        {
            get
            {
                return this.fIELD_SUB_TYPEField;
            }
            set
            {
                this.fIELD_SUB_TYPEField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ENTITY_CURRENCY_TYPE_ID
        {
            get
            {
                return this.eNTITY_CURRENCY_TYPE_IDField;
            }
            set
            {
                this.eNTITY_CURRENCY_TYPE_IDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ENTITY_CURRENCY_TYPE_INDICATOR
        {
            get
            {
                return this.eNTITY_CURRENCY_TYPE_INDICATORField;
            }
            set
            {
                this.eNTITY_CURRENCY_TYPE_INDICATORField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? MAX_LENGTH
        {
            get
            {
                return this.mAX_LENGTHField;
            }
            set
            {
                this.mAX_LENGTHField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ALLOWED_REGEX
        {
            get
            {
                return this.aLLOWED_REGEXField;
            }
            set
            {
                this.aLLOWED_REGEXField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? DECIMAL_PRECESION
        {
            get
            {
                return this.dECIMAL_PRECESIONField;
            }
            set
            {
                this.dECIMAL_PRECESIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_DECODE")]
        public ENTITY_TYPEASSOC_TYPEASSOC_DECODE[] ASSOC_DECODE
        {
            get
            {
                return this.aSSOC_DECODEField;
            }
            set
            {
                this.aSSOC_DECODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_METADATA")]
        public ENTITY_TYPEASSOC_TYPEASSOC_METADATA[] ASSOC_METADATA
        {
            get
            {
                return this.aSSOC_METADATAField;
            }
            set
            {
                this.aSSOC_METADATAField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ASSOC_METADATA_TEXT")]
        public ENTITY_TYPEASSOC_TYPEASSOC_METADATA_TEXT[] ASSOC_METADATA_TEXT
        {
            get
            {
                return this.aSSOC_METADATA_TEXTField;
            }
            set
            {
                this.aSSOC_METADATA_TEXTField = value;
            }
        }

        //[System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE")]
        public ENTITY_TYPEASSOC_TYPEEXTERNAL_DATASOURCE EXTERNAL_DATASOURCE
        {
            get
            {
                return this.eXTERNAL_DATASOURCEField;
            }
            set
            {
                this.eXTERNAL_DATASOURCEField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public ENTITY_ASSOC_TYPE_CASCADE[] ENTITY_ASSOC_TYPE_CASCADE
        {
            get
            {
                return this.eENTITY_ASSOC_TYPE_CASCADE;
            }
            set
            {
                this.eENTITY_ASSOC_TYPE_CASCADE = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string IS_EXTERNALENTITIES_ASSOC_TYPE
        {
            get
            {
                return this.iS_EXTERNALENTITIES_ASSOC_TYPEField;
            }
            set
            {
                this.iS_EXTERNALENTITIES_ASSOC_TYPEField = value;
            }
        }

    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEASSOC_TYPEASSOC_DECODE
    {

        private int eNTITY_ASSOC_DECODE_IDField;

        private int eNTITY_ASSOC_TYPE_IDField;

        private string aSSOC_DECODE_NAMEField;

        private string aSSOC_DECODE_SYSTEM_CODEField;

        private int? dISPLAY_PRIORITYField;

        /// <remarks/>
        public int ENTITY_ASSOC_DECODE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_DECODE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_DECODE_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public string ASSOC_DECODE_NAME
        {
            get
            {
                return this.aSSOC_DECODE_NAMEField;
            }
            set
            {
                this.aSSOC_DECODE_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string ASSOC_DECODE_SYSTEM_CODE
        {
            get
            {
                return this.aSSOC_DECODE_SYSTEM_CODEField;
            }
            set
            {
                this.aSSOC_DECODE_SYSTEM_CODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? DISPLAY_PRIORITY
        {
            get
            {
                return this.dISPLAY_PRIORITYField;
            }
            set
            {
                this.dISPLAY_PRIORITYField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEASSOC_TYPEASSOC_METADATA
    {
        private int eNTITY_ASSOC_METADATA_IDField;

        private int eNTITY_ASSOC_TYPE_IDField;

        private string fIELD_VALUEField;

        private string fIELD_NAMEField;

        private string eXTERNAL_DATASOURCE_OBJECT_IDField;

        private int? eNTITY_ASSOC_DECODE_IDField;

        private int? eNTITY_IDField;


        [System.Xml.Serialization.XmlElementAttribute("ENTITY_ASSOC_DECODE_ID", IsNullable = true)]
        public int? ENTITY_ASSOC_DECODE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_DECODE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_DECODE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_ASSOC_METADATA_ID")]
        public int ENTITY_ASSOC_METADATA_ID
        {
            get
            {
                return this.eNTITY_ASSOC_METADATA_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_METADATA_IDField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_ID", IsNullable = true)]
        public int? ENTITY_ID
        {
            get
            {
                return this.eNTITY_IDField;
            }
            set
            {
                this.eNTITY_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ENTITY_ASSOC_TYPE_ID")]
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FIELD_VALUE")]
        public string FIELD_VALUE
        {
            get
            {
                return this.fIELD_VALUEField;
            }
            set
            {
                this.fIELD_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("FIELD_NAME")]
        public string FIELD_NAME
        {
            get
            {
                return this.fIELD_NAMEField;
            }
            set
            {
                this.fIELD_NAMEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("EXTERNAL_DATASOURCE_OBJECT_ID", IsNullable = true)]
        public string EXTERNAL_DATASOURCE_OBJECT_ID
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_OBJECT_IDField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_OBJECT_IDField = value;
            }
        }

    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEASSOC_TYPEASSOC_METADATA_TEXT
    {

        private int eNTITY_ASSOC_METADATA_TEXT_IDField;

        private int eNTITY_ASSOC_TYPE_IDField;

        private int? eNTITY_IDField;

        private string tEXTField;

        private int? eNTITY_FILE_IDField;

        private double? tRY_DECIMAL_VALUEField;

        private int? tRY_INTEGER_VALUEField;

        private string tRY_DATE_VALUEField;

        private string tRY_DATETIME_VALUEField;

        private double? tRY_CURRENCY_VALUEField;

        private string cALCULATION_EQUATIONField;

        private string cALCULATION_ERRORField;

        private int? cALCULATION_FREQUENCY_MINField;

        private string cALCULATION_LAST_RAN_DATETIMEField;

        /// <remarks/>
        public int ENTITY_ASSOC_METADATA_TEXT_ID
        {
            get
            {
                return this.eNTITY_ASSOC_METADATA_TEXT_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_METADATA_TEXT_IDField = value;
            }
        }

        /// <remarks/>
        public int ENTITY_ASSOC_TYPE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_TYPE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_TYPE_IDField = value;
            }
        }

        /// <remarks/>
        public int? ENTITY_ID
        {
            get
            {
                return this.eNTITY_IDField;
            }
            set
            {
                this.eNTITY_IDField = value;
            }
        }

        /// <remarks/>
        public string TEXT
        {
            get
            {
                return this.tEXTField;
            }
            set
            {
                this.tEXTField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? ENTITY_FILE_ID
        {
            get
            {
                return this.eNTITY_FILE_IDField;
            }
            set
            {
                this.eNTITY_FILE_IDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public double? TRY_DECIMAL_VALUE
        {
            get
            {
                return this.tRY_DECIMAL_VALUEField;
            }
            set
            {
                this.tRY_DECIMAL_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? TRY_INTEGER_VALUE
        {
            get
            {
                return this.tRY_INTEGER_VALUEField;
            }
            set
            {
                this.tRY_INTEGER_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string TRY_DATE_VALUE
        {
            get
            {
                return this.tRY_DATE_VALUEField;
            }
            set
            {
                this.tRY_DATE_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string TRY_DATETIME_VALUE
        {
            get
            {
                return this.tRY_DATETIME_VALUEField;
            }
            set
            {
                this.tRY_DATETIME_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public double? TRY_CURRENCY_VALUE
        {
            get
            {
                return this.tRY_CURRENCY_VALUEField;
            }
            set
            {
                this.tRY_CURRENCY_VALUEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_EQUATION
        {
            get
            {
                return this.cALCULATION_EQUATIONField;
            }
            set
            {
                this.cALCULATION_EQUATIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_ERROR
        {
            get
            {
                return this.cALCULATION_ERRORField;
            }
            set
            {
                this.cALCULATION_ERRORField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public int? CALCULATION_FREQUENCY_MIN
        {
            get
            {
                return this.cALCULATION_FREQUENCY_MINField;
            }
            set
            {
                this.cALCULATION_FREQUENCY_MINField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string CALCULATION_LAST_RAN_DATETIME
        {
            get
            {
                return this.cALCULATION_LAST_RAN_DATETIMEField;
            }
            set
            {
                this.cALCULATION_LAST_RAN_DATETIMEField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_TYPEASSOC_TYPEEXTERNAL_DATASOURCE
    {

        private int eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField;

        private string eXTERNAL_DATASOURCE_NAMEField;

        private string eXTERNAL_DATASOURCE_DESCRIPTIONField;

        private string cONNECTION_STRINGField;

        private string qUERYField;

        private string oBJECT_IDField;

        private string oBJECT_DISPLAYField;

        private string oBJECT_DESCRIPTIONField;

        private string uRL_DRILL_INTOField;

        private string fIELD_SYSTEM_CODEField;

        private string vALUE_SYSTEM_CODEField;

        /// <remarks/>
        public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID
        {
            get
            {
                return this.eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField;
            }
            set
            {
                this.eNTITY_ASSOC_EXTERNAL_DATASOURCE_IDField = value;
            }
        }

        /// <remarks/>
        public string EXTERNAL_DATASOURCE_NAME
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_NAMEField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_NAMEField = value;
            }
        }

        /// <remarks/>
        public string EXTERNAL_DATASOURCE_DESCRIPTION
        {
            get
            {
                return this.eXTERNAL_DATASOURCE_DESCRIPTIONField;
            }
            set
            {
                this.eXTERNAL_DATASOURCE_DESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        public string CONNECTION_STRING
        {
            get
            {
                return this.cONNECTION_STRINGField;
            }
            set
            {
                this.cONNECTION_STRINGField = value;
            }
        }

        /// <remarks/>
        public string QUERY
        {
            get
            {
                return this.qUERYField;
            }
            set
            {
                this.qUERYField = value;
            }
        }

        /// <remarks/>
        public string OBJECT_ID
        {
            get
            {
                return this.oBJECT_IDField;
            }
            set
            {
                this.oBJECT_IDField = value;
            }
        }

        /// <remarks/>
        public string OBJECT_DISPLAY
        {
            get
            {
                return this.oBJECT_DISPLAYField;
            }
            set
            {
                this.oBJECT_DISPLAYField = value;
            }
        }

        /// <remarks/>
        public string OBJECT_DESCRIPTION
        {
            get
            {
                return this.oBJECT_DESCRIPTIONField;
            }
            set
            {
                this.oBJECT_DESCRIPTIONField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string URL_DRILL_INTO
        {
            get
            {
                return this.uRL_DRILL_INTOField;
            }
            set
            {
                this.uRL_DRILL_INTOField = value;
            }
        }

        /// <remarks/>
        public string FIELD_SYSTEM_CODE
        {
            get
            {
                return this.fIELD_SYSTEM_CODEField;
            }
            set
            {
                this.fIELD_SYSTEM_CODEField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string VALUE_SYSTEM_CODE
        {
            get
            {
                return this.vALUE_SYSTEM_CODEField;
            }
            set
            {
                this.vALUE_SYSTEM_CODEField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ENTITY_ASSOC_TYPE_CASCADE
    {

        private int eEntityAssocTypeCascadeID;
        private string eCasCadeType;
        private int eEntityAssocTypeIDParent;
        private string eEntityAssocTypeNameParent;
        private int eEntityAssocExternalDatasourceIDParent;
        private string eEntityAssocExternalDatasourceNameParent;
        private string eFilterQueryParent;

        private int eEntityAssocTypeIDChild;
        private string eEntityAssocTypeNameChild;

        private int eEntityAssocExternalDatasourceIDChild;
        private string eEntityAssocExternalDatasourceNameChild;

        private string eFilterQueryChild;



        [System.Xml.Serialization.XmlElementAttribute("ENTITY_ASSOC_TYPE_CASCADE_ID")]
        public int ENTITY_ASSOC_TYPE_CASCADE_ID
        {
            get
            {
                return this.eEntityAssocTypeCascadeID;
            }
            set
            {
                this.eEntityAssocTypeCascadeID = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute("CASCADE_TYPE")]
        public string CASCADE_TYPE
        {
            get
            {
                return this.eCasCadeType;
            }
            set
            {
                this.eCasCadeType = value;
            }
        }
        public int ENTITY_ASSOC_TYPE_ID_PARENT
        {
            get
            {
                return this.eEntityAssocTypeIDParent;
            }
            set
            {
                this.eEntityAssocTypeIDParent = value;
            }
        }
        public string ENTITY_ASSOC_TYPE_NAME_PARENT
        {
            get
            {
                return this.eEntityAssocTypeNameParent;
            }
            set
            {
                this.eEntityAssocTypeNameParent = value;
            }
        }
        public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_PARENT
        {
            get
            {
                return this.eEntityAssocExternalDatasourceIDParent;
            }
            set
            {
                this.eEntityAssocExternalDatasourceIDParent = value;
            }
        }
        public string ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_PARENT
        {
            get
            {
                return this.eEntityAssocExternalDatasourceNameParent;
            }
            set
            {
                this.eEntityAssocExternalDatasourceNameParent = value;
            }
        }
        public string FILTER_QUERY_PARENT
        {
            get
            {
                return this.eFilterQueryParent;
            }
            set
            {
                this.eFilterQueryParent = value;
            }
        }

        public int ENTITY_ASSOC_TYPE_ID_CHILD
        {
            get
            {
                return this.eEntityAssocTypeIDChild;
            }
            set
            {
                this.eEntityAssocTypeIDChild = value;
            }
        }
        public string ENTITY_ASSOC_TYPE_NAME_CHILD
        {
            get
            {
                return this.eEntityAssocTypeNameChild;
            }
            set
            {
                this.eEntityAssocTypeNameChild = value;
            }
        }

        public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID_CHILD
        {
            get
            {
                return this.eEntityAssocExternalDatasourceIDChild;
            }
            set
            {
                this.eEntityAssocExternalDatasourceIDChild = value;
            }
        }
        public string ENTITY_ASSOC_EXTERNAL_DATASOURCE_NAME_CHILD
        {
            get
            {
                return this.eEntityAssocExternalDatasourceNameChild;
            }
            set
            {
                this.eEntityAssocExternalDatasourceNameChild = value;
            }
        }

        public string FILTER_QUERY_CHILD
        {
            get
            {
                return this.eFilterQueryChild;
            }
            set
            {
                this.eFilterQueryChild = value;
            }
        }
    }

    //Third

    public class EntityOrgCenterList
    {

        public int EntityTypeID { get; set; }
        public int EntityID { get; set; }
        public string EntityTypeName { get; set; }

        public string InstanceNameSingular { get; set; }

        public string InstanceNamePlural { get; set; }
        public string SecurityType { get; set; }

        public bool HasRightsToConfigSecurity { get; set; }
        public bool ShowAsRssFeed { get; set; }

        public int? EntityTypeCategoryID { get; set; }
        public string EntityTypeCategoryName { get; set; }
        public string CategorySystemCode { get; set; }
        public int? ActiveCount { get; set; }
        public int? InactiveCount { get; set; }
        public int? TotalCount { get; set; }
        public int? AssignedToUser { get; set; }

        public int? CreatedByUser { get; set; }
        public int? OwnedByUser { get; set; }
        public int? InactivatedByUser { get; set; }
        public DateTime? LastCreatedDateTime { get; set; }
        public bool HasExternalEntityAssocType { get; set; }

        public int? ASSOCIATED_TO_USER { get; set; }

        public string Owner { get; set; }



        public string EntityListLink
        {
            get
            {
                return "/EntityList.aspx?EntityTypeID=" + this.EntityTypeID.ToString();
            }
        }

        public string NewEntityText
        {
            get
            {
                return "Create New " + InstanceNameSingular;
            }
            set { }
        }

        public string NewEntityLink
        {
            get
            {
                return "/EntityCreate.aspx?EntityTypeID=" + this.EntityTypeID.ToString();
            }
        }

        public string NewEntityLinkPopUp
        {
            get
            {
                return "<a class=\"aLink\" href=\"#" + "\" " + "onclick=\"OpenPopUporLink('/EntityCreate.aspx?EntityTypeID=" + this.EntityTypeID.ToString() + "', 'inner');return false;" + "\">" + NewEntityText + "</a>";
            }
        }

        public override string ToString()
        {
            string format = "{0}, EntityTypeID = {1}";
            return string.Format(format, this.EntityTypeID, this.EntityTypeName);
        }
    }

    public class EntityTypeCategory
    {
        public int EntityTypeCategoryID { get; set; }
        public string EntityTypeCategoryName { get; set; }
        public string CategorySystemCode { get; set; }
        public string CategoryDescription { get; set; }
        public char? IsDefault { get; set; }
    }

    public class EntityTypeList
    {
        public int EntityTypeId { get; set; }
        public string EntityTypeName { get; set; }
    }

    //Forth

    public class EntityType
    {
        public int? ItemtransinfoID { get; set; }// store Transaction type info table PK ID
        public string TransactionType { get; set; } // store Transation type of App Info ID
        public int _NEntityID { get; set; }
        public string EntityTypeName { get; set; }
        public string EntityTypeDescription { get; set; }
        public string InstanceSingular { get; set; }
        public string InstancePlural { get; set; }
        public string EntityTypeOwener { get; set; }
        public string NewestNotesOnTop { get; set; }
        public string ApplyAssocTypeCalculationFormula { get; set; }
        public string EntityTypeHelpURL { get; set; }
        public bool EntityTypeIsActive { get; set; }
        public bool HasExternalEntityAssocType { get; set; }

        public string EntityTypeSecurityType { get; set; }
        public string EntityTypeCreatedDateTime { get; set; }
        public string EntityTypeCreatedBySAM { get; set; }
        public string EntityTypeModifiedDateTime { get; set; }
        public string EntityTypeModifiedBySAM { get; set; }
        public int EntityTypeID { get; set; }
        public List<AssociationField> AssociationFieldCollection { get; set; }

        public List<EntityTypeRelationship> EntityTypeRelationship { get; set; }

        public List<EntityTypeTemplate> EntityTypeTemplates { get; set; }

        public string EntityTypeSystemCode { get; set; }
    }

    public class FILTER_VALUE
    {
        public string SHOW_ENTITIES_ACTIVE_INACTIVE { get; set; }
        public string SHOW_ENTITIES_ASSIGNED_TO_USER { get; set; }
        public string SHOW_ENTITIES_CREATED_BY_USER { get; set; }
        public string SHOW_ENTITIES_OWNED_BY_USER { get; set; }
        public string SHOW_ENTITIES_INACTIVE_BY_USER { get; set; }
        public int EXTERNAL_DATASOURCE_OBJECT_ID_ENTITY { get; set; }
        public List<int> ENTITY_TYPE { get; set; }


        public string SYSTEM_CODE_ENTITY_TYPE { get; set; }

        public List<EntityType> EntityTypeValue { get; set; }

        [System.Xml.Serialization.XmlArray("FILTER_COLUMNS", IsNullable = true)]
        [System.Xml.Serialization.XmlArrayItem("FILTER_COLUMN", IsNullable = true)]
        public List<FilterParameterInfo> FilterInformation { get; set; }


        [System.Xml.Serialization.XmlArray("FILTER_COLUMNS_STATIC", IsNullable = true)]
        [System.Xml.Serialization.XmlArrayItem("FILTER_COLUMN", IsNullable = true)]
        public List<FilterParameterInfo> FilterInformationForStaticFields { get; set; }


        public FILTER_VALUE()
        {
            ENTITY_TYPE = new List<int>();
            EntityTypeValue = new List<EntityType>();
            FilterInformation = new List<FilterParameterInfo>();
            FilterInformationForStaticFields = new List<FilterParameterInfo>();
        }


    }

    public class FilterParameterInfo
    {
        [System.Xml.Serialization.XmlElementAttribute("FIELD_NAME", IsNullable = true)]
        public string filtercolumnName { get; set; }

        [System.Xml.Serialization.XmlArray("FIELD_VALUES", IsNullable = true)]
        [System.Xml.Serialization.XmlArrayItem("VALUE", IsNullable = true)]
        public List<string> Values { get; set; }

        public FilterParameterInfo()
        {
            Values = new List<string>();
        }

        public FilterParameterInfo(string filtercolumnName, List<string> Values)
        {
            this.filtercolumnName = filtercolumnName;
            this.Values = Values;

        }
    }

    public class AssociationField
    {
        public int AssocTypeID { get; set; }

        public int EntityTypeID { get; set; }

        public string SecurityType { get; set; }

        public string FieldType { get; set; }

        public string AssocName { get; set; }

        public int? CurrencyTypeID { get; set; }

        public string CurrencyIndicator { get; set; }

        public int? DecimalPrecesion { get; set; }

        public int? ExternalDataSourceID { get; set; }

        public string AssocSystemCode { get; set; }

        public string AssocSystemCodeName { get; set; }

        private int? _ListDesktopPriorityValue = 5000;
        public int? ListDesktopPriorityValue
        {
            get { return _ListDesktopPriorityValue; }
            set { _ListDesktopPriorityValue = value ?? 5000; }
        }

        private int? _ListMobilePriorityValue = 5000;
        public int? ListMobilePriorityValue
        {
            get { return _ListMobilePriorityValue; }
            set { _ListMobilePriorityValue = value ?? 5000; }
        }

        private int? _itemDesktopPriorityValue = 5000;
        public int? ItemDesktopPriorityValue
        {
            get { return _itemDesktopPriorityValue; }
            set { _itemDesktopPriorityValue = value ?? 5000; }
        }

        public int? ItemMobilePriorityValue { get; set; }

        public string AssocShowOnList { get; set; }

        public string IsRequired { get; set; }

        public string IsReadOnly { get; set; }

        public string FieldSubType { get; set; }

        public int? MaxLength { get; set; }

        public string AllowedRegex { get; set; }

        public List<AssociationMetaData> AssocMetaData { get; set; }

        public List<AssociationMetaDataText> AssocMetaDataText { get; set; }

        public List<AssociationFieldValue> AssocDecode { get; set; }

        public ExternalDatasource ExternalDataSource { get; set; }

        public string CalculationFormula { get; set; }
        public string EnableForceCalculationOnFormula { get; set; }

        public int? CalculationFrequencyMin { get; set; }

        public List<EntityAssocTypeCascade> EntityAssocTypeCascade { get; set; }

        public List<EXTERNAL_DATASOURCE1> EXTERNAL_DATASOURCE { get; set; }

        public bool IsExternalEntityAssocType { get; set; }

        public string SeparatorCharactor { get; set; }


        public override string ToString()
        {
            string format = "{0}, AssocTypeID = {1},ItemDesktopPriorityValue = {2}";
            return string.Format(format, this.AssocName, this.AssocTypeID, this.ItemDesktopPriorityValue);
        }
    }

    public class EntityWithNote : EntityClass
    {
        public string EntityNote { get; set; }
    }

    public class EntityClass : EntityType
    {

        public int EntityID { get; set; }

        private int? _listid;
        public int? ListID
        {
            get
            {
                return _listid == null ? 0 : _listid;
            }
            set { _listid = value; }
        }


        public string EntitySecurityType { get; set; }

        public int? MasterEntityID { get; set; }

        public string MasterEntityRelationDescription { get; set; }
        public string EntityCreatedByUserName { get; set; }
        public string EntityCreatedByFullName { get; set; }
        public string EntityCreatedByPhone { get; set; }
        public string EntityCreatedByEmail { get; set; }
        public Guid? EntityCreatedByGUID { get; set; }
        public string EntityCreatedDateTime { get; set; }

        public string EntityModifiedByUserName { get; set; }
        public string EntityModifiedByFullName { get; set; }
        public string EntityModifiedByPhone { get; set; }
        public string EntityModifiedByEmail { get; set; }
        public Guid? EntityModifiedByGUID { get; set; }
        public string EntityModifiedDateTime { get; set; }

        public string EntitiyAssignedToUserName { get; set; }
        public string EntityAssignedToFullName { get; set; }
        public string EntityAssignedToEmail { get; set; }
        public string EntityAssignedToPhone { get; set; }
        public Guid? EntityAssignedToGUID { get; set; }
        public string EntityAssignedToDateTime { get; set; }

        public string EntitiyOwnedByUserName { get; set; }
        public string EntityOwnedByFullName { get; set; }
        public string EntityOwnedByEmail { get; set; }
        public string EntityOwnedByPhone { get; set; }
        public Guid? EntityOwnedByGUID { get; set; }
        public string EntityOwnedByDateTime { get; set; }

        public string SHOW_ENTITIES_ACTIVE_INACTIVE { get; set; }
        public string SHOW_ENTITIES_ASSIGNED_TO_USER { get; set; }
        public string SHOW_ENTITIES_CREATED_BY_USER { get; set; }
        public string SHOW_ENTITIES_OWNED_BY_USER { get; set; }
        public string SHOW_ENTITIES_INACTIVE_BY_USER { get; set; }

        public bool HasRightsToConfigSecurity { get; set; }

        public List<EntityType> EntityTypeValue { get; set; }

        public List<EntityTypeScreenConfiguration> ScreenConfigurationSettings { get; set; }

        public override string ToString()
        {
            string format = "{0}, EntityID = {1},EntityTypeID = {2}";
            return string.Format(format, EntityTypeName, this.EntityID, EntityTypeID);
        }
    }

    public class EntityTypeTemplate
    {
        public int EntityTemplateID { get; set; }

        public string TemplateName { get; set; }

        public string TemplateDescription { get; set; }

        public string TemplateServerFilePath { get; set; }

        public string TemplateType { get; set; }
    }

    public class EntityTypeScreenConfiguration
    {
        public int EntityAssocTypeScreenItemID { get; set; }

        public int EntityScreenItemID { get; set; }

        public string ScreenItemName { get; set; }

        public string ScreenItemDescription { get; set; }

        public string ScreenItemType { get; set; }
    }
    public class AssociationFieldValue
    {
        public int AssocDecodeID { get; set; }

        public string ExternalDatasourceObjectID { get; set; }

        public int AssocTypeID { get; set; }

        public string AssocDecodeName { get; set; }

        public string AssocDecodeSystemCode { get; set; }

        public int? DisplayPriority { get; set; }

        public override string ToString()
        {
            string format = "{0}, AssocDecodeID = {1}";
            return string.Format(format, this.AssocDecodeName, this.AssocDecodeID);
        }
    }

    public class AssociationMetaData
    {
        public int AssocMetaDataID { get; set; }

        public int AssocTypeID { get; set; }

        public int? EntityID { get; set; }

        public string FieldName { get; set; }

        public string FieldValue { get; set; }

        public string ExternalDatasourceObjectID { get; set; }

        public int? AssocDecodeID { get; set; }

    }

    public class AssociationMetaDataText
    {
        private string _textValue;

        public int AssocMetaDataTextID { get; set; }

        public int AssocTypeID { get; set; }

        public int? EntityID { get; set; }

        public string TextValue { get => _textValue ?? ""; set => _textValue = value ?? ""; }

        public int? EntityFileID { get; set; }

        public double? TryDecimalValue { get; set; }

        public int? TryIntegerValue { get; set; }

        public string TryDateValue { get; set; }

        public string TryDateTimeValue { get; set; }

        public double? TryCurrencyValue { get; set; }

        public string CalculationEquation { get; set; }

        public string CalculationError { get; set; }

        public int? CalculationFrequencyMin { get; set; }

        public string CalculationLastRanDateTime { get; set; }

    }

    public class EntityTypeRelationship
    {
        public string EntityTypeID { get; set; }
        public string EntityTypeName { get; set; }

        public string InstanceName { get; set; }

        public string InstanceNamePlural { get; set; }

        public string EntityTypeSecurityType { get; set; }

        public bool IsExternalEntityType { get; set; }
    }

    public class ExternalDatasource
    {
        public int ExternalDatasourceID { get; set; }
        public string DataSourceName { get; set; }
        public string DataSourceDescription { get; set; }

        public string ConnectionString { get; set; }
        public string Query { get; set; }

        public string ObjectId { get; set; }
        public string ObjectDisplay { get; set; }
        public string ObjectDescription { get; set; }
        public string URLDrillInto { get; set; }
        public string FieldSystemCode { get; set; }
        public string ValueSystemCode { get; set; }

        public List<EXTERNAL_DATASOURCE1> List { get; set; }


    }

    public class EXTERNAL_DATASOURCE1
    {
        public int ID { get; set; }
        public string EXTERNAL_DATASOURCE_NAME { get; set; }
        public string EXTERNAL_DATASOURCE_DESCRIPTION { get; set; }
        public int Count { get; set; }
    }

    public class EntityAssocTypeCascade
    {
        public int EntityAssocTypeCascadeID { get; set; }
        public string CasCadeType { get; set; }
        public int EntityAssocTypeIDParent { get; set; }
        public string EntityAssocTypeNameParent { get; set; }
        public int EntityAssocExternalDatasourceIDParent { get; set; }
        public string EntityAssocExternalDatasourceNameParent { get; set; }
        public string FilterQueryParent { get; set; }

        public int EntityAssocTypeIDChild { get; set; }
        public string EntityAssocTypeNameChild { get; set; }

        public int EntityAssocExternalDatasourceIDChild { get; set; }
        public string EntityAssocExternalDatasourceNameChild { get; set; }

        public string FilterQueryChild { get; set; }
    }

    public class EntityFile
    {
        public int EntityFileId { get; set; }
        public string Description { get; set; }
        public DateTime? FileDateTime { get; set; }
        public string FileName { get; set; }
        public long FileSizeBytes { get; set; }
        public byte[] FileBlob { get; set; }
        public string ExternalUri { get; set; }
        public int FileVersion { get; set; }
        public char ShowInlineNotes { get; set; }
        public string SystemCode { get; set; }
        public char IsActive { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
    }

    public class EmpList
    {
        public int EmpUserID { get; set; }
        public string EmpName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmployeeGUID { get; set; }
    }

    public class ActivityInstance
    {
        public int ActivityID { get; set; }
        public int EntityID { get; set; }
        public int? EntityActivityTypeID { get; set; }
        public string Note { get; set; }
        public char IsActive { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string BColor { get; set; }
        public string FColor { get; set; }
        public string SystemCode { get; set; }
    }


    public class EntityNoteItem
    {
        // From Column Name: ENTITY_NOTE_ID
        public int ID { get; set; }

        // From Column Name: ENTITY_ID
        public EntityClass Entity { get; set; }

        // From Column Name: ENTITY_NOTE_TYPE_ID
        public EntityNoteType EntityNoteType { get; set; }

        public string EntityNoteTypeByName { get; set; }
        // From Column Name: NOTE
        public string Note { get; set; }

        public string htmlNote { get; set; }

        public string BColor { get; set; }

        public string FColor { get; set; }

        // From Column Name: IS_ACTIVE
        public char IsActive { get; set; }

        // From Column Name: CREATED_DATETIME
        public DateTime CreatedDatetime { get; set; }

        // From Column Name: CREATED_BY
        public string CreatedBy { get; set; }

        // From Column Name: MODIFIED_DATETIME
        public DateTime? ModifiedDatetime { get; set; }

        // From Column Name: MODIFIED_BY
        public string ModifiedBy { get; set; }

        public string ModifiedByFullName { get; set; }

        public string SystemCode { get; set; }

        public char? AllowManualUse { get; set; }
    }
    public class EntityNoteType
    {
        // From Column Name: ENTITY_NOTE_TYPE_ID
        public int ID { get; set; }

        // From Column Name: NAME
        public string Name { get; set; }

        // From Column Name: DESCRIPTION
        public string Description { get; set; }

        // From Column Name: BCOLOR
        public string Bcolor { get; set; }

        // From Column Name: FCOLOR
        public string Fcolor { get; set; }

        // From Column Name: SYSTEM_CODE
        public string SystemCode { get; set; }

        // From Column Name: ALLOW_MANUAL_USE
        public char? AllowManualUse { get; set; }

        // From Column Name: IS_ACTIVE
        public char IsActive { get; set; }

        // From Column Name: CREATED_DATETIME
        public DateTime CreatedDatetime { get; set; }

        // From Column Name: CREATED_BY
        public string CreatedBy { get; set; }

        // From Column Name: MODIFIED_DATETIME
        public DateTime? ModifiedDatetime { get; set; }

        // From Column Name: MODIFIED_BY
        public string ModifiedBy { get; set; }

    }

    public class CasesRelationData
    {
        public string LIST_ID { get; set; }
        public string LIST_CASE_ID { get; set; }

        [JsonProperty(PropertyName = "Calculation Field7_Ø_TITLE")]
        public string Name_Ø_TITLE { get; set; }

        [JsonProperty(PropertyName = "Calculation Field1")]
        public string Calculated1 { get; set; }

        [JsonProperty(PropertyName = "Calculation Field2")]
        public string Calculated2 { get; set; }

        [JsonProperty(PropertyName = "Calculation Field3")]
        public string Calculated3 { get; set; }

        [JsonProperty(PropertyName = "Calculation Field4")]
        public string CalculationField4 { get; set; }

        [JsonProperty(PropertyName = "Calculation Field5")]
        public string CalculationField5 { get; set; }
        [JsonProperty(PropertyName = "Calculation Field6")]
        public string CalculationField6 { get; set; }
        [JsonProperty(PropertyName = "Calculation Field7")]
        public string CalculationField7 { get; set; }
        [JsonProperty(PropertyName = "Calculation Field8")]
        public string CalculationField8 { get; set; }
        [JsonProperty(PropertyName = "Calculation Field9")]
        public string CalculationField9 { get; set; }
        [JsonProperty(PropertyName = "Calculation Field10")]
        public string CalculationField10 { get; set; }

        [JsonProperty(PropertyName = "Calculation Field10")]
        public string Cost_Ø_COST { get; set; }
        public string Duedate_Ø_DUEDT { get; set; }
        public string City_Ø_SCITY { get; set; }
        public string Country_Ø_SCOUN { get; set; }
        public string Location_Ø_SLOCA { get; set; }
        public string State_Ø_SSTAT { get; set; }
        public string Status_Ø_STTUS { get; set; }
        public string LIST_CASE_LIFE_D_H_M { get; set; }
        public string LIST_CASE_OWNER_DISPLAY_NAME { get; set; }
        public string LIST_CASE_ASSGN_TO_DISPLAY_NAME { get; set; }
        public string LIST_CASE_CREATED_BY_DISPLAY_NAME { get; set; }
        public string Total_Case { get; set; }
        public string IsClosed { get; set; }
    }
    public class EntityRelationData
    {
        public int? EntityID { get; set; }
        public int? ListID { get; set; }
        public string EntityCreatedByUserName { get; set; }
        public string EntityCreatedDateTime { get; set; }

        public string EntityModifiedByUserName { get; set; }

        public string EntityModifiedDateTime { get; set; }

        public string EntitiyOwnedByUserName { get; set; }

        public string EntityOwnedByDateTime { get; set; }

        public string Title { get; set; }
        public string Ent_ListId { get; set; }

    }




}