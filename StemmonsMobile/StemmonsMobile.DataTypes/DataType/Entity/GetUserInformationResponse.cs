using StemmonsMobile.DataTypes.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetUserInformationResponse : Response
    {
        //public List<CaseType> ListCaseTypes { get; set; }
        public object ResponseContent { get; set; }
        public class UserInfo
        {
            public int ID { get; set; }

            public string MiddleName { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string PrimaryJobTitle { get; set; }

            public string CellPhone { get; set; }

            public string Department { get; set; }

            public string OfficePhone { get; set; }

            public string Email { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string DisplayName { get; set; }

            public string UserID { get; set; }

            public string JobTitle { get; set; }
            public int Entity_Type_ID { get; set; }
            public string Entity_Type_Name { get; set; }
            public int Entity_ID { get; set; }
            public string Entity_Title { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public string ShortName { get; set; }
            public char? IsExternalUser { get; set; }

        }

        public class EntityType
        {

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
            public DateTime EntityTypeCreatedDateTime { get; set; }
            public string EntityTypeCreatedBySAM { get; set; }
            public DateTime EntityTypeModifiedDateTime { get; set; }
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

            public int? ListDesktopPriorityValue { get; set; }

            public int? ListMobilePriorityValue { get; set; }

            public int? ItemDesktopPriorityValue { get; set; }

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

            public bool IsExternalEntityAssocType { get; set; }

            public override string ToString()
            {
                string format = "{0}, AssocTypeID = {1}";
                return string.Format(format, this.AssocName, this.AssocTypeID);
            }
        }

        public class EntityWithNote : Entity
        {
            public string EntityNote { get; set; }
        }

        public class Entity : EntityType
        {
            public int? EntityID { get; set; }

            public int? ListID { get; set; }


            public string EntitySecurityType { get; set; }

            public int? MasterEntityID { get; set; }

            public string MasterEntityRelationDescription { get; set; }
            public string EntityCreatedByUserName { get; set; }
            public string EntityCreatedByFullName { get; set; }
            public string EntityCreatedByPhone { get; set; }
            public string EntityCreatedByEmail { get; set; }
            public Guid? EntityCreatedByGUID { get; set; }
            public DateTime? EntityCreatedDateTime { get; set; }

            public string EntityModifiedByUserName { get; set; }
            public string EntityModifiedByFullName { get; set; }
            public string EntityModifiedByPhone { get; set; }
            public string EntityModifiedByEmail { get; set; }
            public Guid? EntityModifiedByGUID { get; set; }
            public DateTime? EntityModifiedDateTime { get; set; }

            public string EntitiyAssignedToUserName { get; set; }
            public string EntityAssignedToFullName { get; set; }
            public string EntityAssignedToEmail { get; set; }
            public string EntityAssignedToPhone { get; set; }
            public Guid? EntityAssignedToGUID { get; set; }
            public DateTime? EntityAssignedToDateTime { get; set; }

            public string EntitiyOwnedByUserName { get; set; }
            public string EntityOwnedByFullName { get; set; }
            public string EntityOwnedByEmail { get; set; }
            public string EntityOwnedByPhone { get; set; }
            public Guid? EntityOwnedByGUID { get; set; }
            public DateTime? EntityOwnedByDateTime { get; set; }

            public string SHOW_ENTITIES_ACTIVE_INACTIVE { get; set; }
            public string SHOW_ENTITIES_ASSIGNED_TO_USER { get; set; }
            public string SHOW_ENTITIES_CREATED_BY_USER { get; set; }
            public string SHOW_ENTITIES_OWNED_BY_USER { get; set; }
            public string SHOW_ENTITIES_INACTIVE_BY_USER { get; set; }

            public bool HasRightsToConfigSecurity { get; set; }

            public List<EntityType> EntityTypeValue { get; set; }

            public List<EntityTypeScreenConfiguration> ScreenConfigurationSettings { get; set; }
        }

        public class EntityTypeTemplate
        {
            public int EntityTemplateID { get; set; }

            public string TemplateName { get; set; }

            public string TemplateDescription { get; set; }

            public string TemplateServerFilePath { get; set; }

            public string TemplateType { get; set; }
        }

        public class EntityTypeCustomList
        {
            public int ENTITY_CUSTOM_LIST_ID { get; set; }

            public string ENTITY_CUSTOM_LIST_NAME { get; set; }

            public string ENTITY_CUSTOM_LIST_Description { get; set; }

            public string ENTITY_CUSTOM_LIST_ServerFilePath { get; set; }

            public string ENTITY_CUSTOM_LIST_TemplateType { get; set; }


            public string ENTITY_ADDITIONAL_FILTER_QUERY { get; set; }

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
            public int AssocMetaDataTextID { get; set; }

            public int AssocTypeID { get; set; }

            public int? EntityID { get; set; }

            public string TextValue { get; set; }

            public int? EntityFileID { get; set; }

            public double? TryDecimalValue { get; set; }

            public int? TryIntegerValue { get; set; }

            public DateTime? TryDateValue { get; set; }

            public DateTime? TryDateTimeValue { get; set; }

            public double? TryCurrencyValue { get; set; }

            public string CalculationEquation { get; set; }

            public string CalculationError { get; set; }

            public int? CalculationFrequencyMin { get; set; }

            public DateTime? CalculationLastRanDateTime { get; set; }

        }

        public class EntityTypeRelationship
        {
            public int EntityTypeID { get; set; }
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
    }
}