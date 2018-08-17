using StemmonsMobile.DataTypes.DataType.Standards;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 




namespace StemmonsMobile.DataTypes.DataType.Standards.DataTypes
{
    public class DataTypes
    {
    }
    public class SecurityDetail
    {
        public int? StrdUserCacheId { get; set; }
        public string SamName { get; set; }
        public string DisplayName { get; set; }
        public int? AppAssocMetaDataId { get; set; }
        public int? AppId { get; set; }
        public string SecurityType { get; set; }
        public string SecurityLevelDescription { get; set; }

    }
    public class SecurityUser
    {
        public string FullName { get; set; }
        public int? EmployeeID { get; set; }
        public string EmployeeGUID { get; set; }
        public string ManagerPath { get; set; }
        public string UserPath { get; set; }
        public string Manager { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public string SamName { get; set; }
    }

    public class ResourceSecurityUser
    {
        public int? StandardsResourceSecurityUserID { get; set; }
        public int? UserCacheID { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? AppID { get; set; }
        public int? AppMetadataID { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsRead { get; set; }

        public bool IsDelete { get; set; }
        public string SecurityType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public char? IsActive { get; set; }
    }

    public class Entities
    {
        public int? EntityID { get; set; }
        public string Entity { get; set; }
    }
    public class EntityRole
    {
        public int? RoleID { get; set; }
        public string Role { get; set; }
    }
    public class EntityTypes
    {
        public int? EntityTypeID { get; set; }
        public string EntityType { get; set; }
    }
    public class StandardsEntityRoleSecurity
    {
        public int? StandardsResourceSecurityRoleID { get; set; }
        public string EntityType { get; set; }
        public string Role { get; set; }
        public string Entity { get; set; }
        public int? AppID { get; set; }
        public int? AppMetadataID { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsRead { get; set; }

        public bool IsDelete { get; set; }
        public string SecurityType { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public char? IsActive { get; set; }

        public int? EntityTypeId { get; set; }
        public int? RoleId { get; set; }
        public int? EntityId { get; set; }

    }

    public class JobTitle
    {
        public int? JobTitleID { get; set; }
        public string JobTitleName { get; set; }
    }


    public class JobFunction
    {
        public int? JobFunctionID { get; set; }
        public string JobFunctionName { get; set; }
    }
    public class SubDepartment
    {
        public int? SubDepartmentID { get; set; }
        public string SubDepartmentName { get; set; }
    }
    public class BasicNames
    {
        public int? BasicNameID { get; set; }
        public string BasicName { get; set; }
    }
    public class TopLevel
    {
        public int? TopLevelID { get; set; }
        public string TopLevelName { get; set; }
    }
    public class StandardsDepartmentSecurity
    {
        public int? StandardResourceSecurityDepartmentID { get; set; }

        public int? TopLevelID { get; set; }
        public string TopLevelName { get; set; }
        public int? BasicNameID { get; set; }
        public string BasicName { get; set; }
        public int? SubDepartmentID { get; set; }
        public string SubDepartmentName { get; set; }
        public int? JobFunctionID { get; set; }
        public string JobFunctionName { get; set; }
        public int? JobTitleID { get; set; }
        public string JobTitleName { get; set; }

        public int? AppID { get; set; }

        public int? AppMetadataID { get; set; }

        public string Department { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsRead { get; set; }

        public bool IsDelete { get; set; }
        public string SecurityType { get; set; }
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public char? IsActive { get; set; }


    }

    public class StandardsGroupSecurity
    {
        public int? StandardsResourceSecurityID { get; set; }
        public string Name { get; set; }
        public int? AppID { get; set; }

        public int? AppAssocMetadataID { get; set; }
        public bool IsAdd { get; set; }
        public bool IsEdit { get; set; }
        public bool IsRead { get; set; }

        public bool IsDelete { get; set; }
        public string SecurityType { get; set; }
        public string Description { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public char? IsActive { get; set; }

    }
    public class Published
    {
        public int? AppAssocMetadataVersionID { get; set; }
        public int? AppAssocMetadataID { get; set; }
        public string Name { get; set; }
        public int? AppID { get; set; }
        public string AppName { get; set; }
        public DateTime? PublishDate { get; set; }
        public string PublishRemark { get; set; }
        public int? NoOfDays { get; set; }
        public char? Status { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public int SrNo { get; set; }
        public string sPublishDate { get; set; }
    }

    public class Types
    {
        public int? SrNo { get; set; }
        public int? AppTypeID { get; set; }
        public int? TypeID { get; set; }
        public string TypeName { get; set; }
        public int? AppID { get; set; }
        public int? Level { get; set; }
        public char? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string sCreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string sModifyDate { get; set; }


    }
    public class MailAddress
    {
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
    }
    public class GetType
    {
        public int? TypeId { get; set; }
        public string Type { get; set; }
        public char? IsActive { get; set; }
    }
    public class DeptRole
    {
        public int? AppId { get; set; }
        public string ApplicationName { get; set; }
        public int? AppAssocMetaDataDeptId { get; set; }
        public string RefType { get; set; }
        public char? IsActive { get; set; }
        public int? RefID { get; set; }
        public string AssgnTo { get; set; }
    }
    public class DropDownViewDepartment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public char? IS_CREATE { get; set; }
        public char? IS_DELETE { get; set; }
    }

    public class DropDown
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Files
    {
        public int? AppAssocFileID { get; set; }
        public int? AppAssocMetaDataID { get; set; }
        public string MetadataName { get; set; }
        public string FileName { get; set; }
        public string FileDesc { get; set; }
        public int? FileSizeBytes { get; set; }
        public string FileBlob { get; set; }
        public char IsActive { get; set; }
        public DateTime? Createddate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string FileDownLoadLink { get; set; }
        public int SrNo { get; set; }
    }
    public class ForMe
    {
        public int? AppID { get; set; }
        public string AppName { get; set; }
        public string Description { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string sModifyDate { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string sLastVisitDate { get; set; }
    }

    public class Grp_StandardDetails
    {
        public string APP_ASSOC_META_DATA_ID { get; set; }
        public string TYPE_ID { get; set; }
        public string NO_OF_DAYS { get; set; }
        public string TYPE_NAME { get; set; }
        public string NAME { get; set; }
        public string AUTHOR { get; set; }
        public string DESCRIPTIONS { get; set; }
        public string PARENT_META_DATA_ID { get; set; }
        public string DISPLAY_ORDER { get; set; }
        public string APP_LOGO { get; set; }
        public string PUBLISH_REMARK { get; set; }
        public string APP_HEADER { get; set; }
        public string APP_FOOTER { get; set; }
        public string IS_ACTIVE { get; set; }
        public string CREATED_DATETIME { get; set; }
        public string LAST_VISIT_Date { get; set; }
        public string PUBLISH_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string PARENT_LEVEL { get; set; }
        public string APP_ID { get; set; }
        public string APP_NAME { get; set; }
        public string TOTAL_POSOTIVE_FEEDBACK { get; set; }
        public string TOTAL_NEGITIVE_FEEDBACK { get; set; }
        public string TOTAL_REVIEW_RANK { get; set; }
        public string TOTAL_VISIT { get; set; }
        public string MODIFIED_DATETIME { get; set; }
        public string MODIFIED_BY { get; set; }
    }
    public class MetaData
    {
        public int? AppAssocMetaDataID { get; set; }
        public int? TypeID { get; set; }
        public string TypeName { get; set; }
        public string ApplicationName { get; set; }
        public string Name { get; set; }
        public int? ParentMetaDataID { get; set; }
        public string ParentName { get; set; }
        public int? DisplayOrder { get; set; }
        public string AppLogo { get; set; }
        public string AppHeader { get; set; }
        public string AppFooter { get; set; }
        public char? IsActive { get; set; }
        public int? ParentLevel { get; set; }
        public int? AppID { get; set; }
        public int? AppAssocMetaDetaContentID { get; set; }
        public string MetaDataDesc { get; set; }
        public string MetaDataSummary { get; set; }
        public string MetaDataContent { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string sCreateDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
        public string sModifyDate { get; set; }
        public int? TotalPositiveFeedback { get; set; }
        public int? TotalNagativeFeedback { get; set; }
        public int? TotalReviewRank { get; set; }
        public int? TotolVisit { get; set; }
        public DateTime? LastVisitDate { get; set; }
        public string sLastVisitDate { get; set; }
        public DateTime? PublishDate { get; set; }
        public string sPublishDate { get; set; }
        public string Author { get; set; }
        public string PublishRemark { get; set; }
        public string Display_Name { get; set; }
        public int? publishVersion { get; set; }
        public string level_key { get; set; }
        public char? isFixedStruct { get; set; }
        public char? isFixedStructApp { get; set; }
        public string isHasDetail { get; set; }
        public int? MetadataContentID { get; set; }

        public string IsCreate { get; set; }
        public string IsUpdate { get; set; }
        public string IsDelete { get; set; }
        public string IsRead { get; set; }

    }
    public class DownLoadImage
    {
        public int? AppAssocFileID { get; set; }
        public int? AppAssocMetaDataID { get; set; }
        public string MetadataName { get; set; }
        public string FileName { get; set; }
        public string FileDesc { get; set; }
        public int? FileSizeBytes { get; set; }
        public string  File { get; set; }
        public int? SrNo { get; set; }
        public char IsActive { get; set; }
        public string IsImage
        {
            get
            {
                if (this.FileName.Contains(".jpg")) return "true";
                if (this.FileName.Contains(".tif")) return "true";
                if (this.FileName.Contains(".png")) return "true";
                if (this.FileName.Contains(".gif")) return "true";
                else
                {
                    return "false";
                }
            }
        }
    }
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public int AppId { get; set; }
        public string Comments { get; set; }

        public string Name { get; set; }
        public string UserResponse { get; set; }
        public DateTime CreatedDatetime { get; set; }
        public string CreatedBy { get; set; }

    }

    public class standards
    {

        public List<STANDARDSALL> All { get; set; }
        public List<STANDARDSMOST_POPULAR> MostPopular { get; set; }
        public List<STANDARDSWHATS_NEW> WhatsNew { get; set; }
        
        public List<STANDARDSFOR_ME> ForMe { get; set; }
        
    }

    public class standardsWebView
    {

        public List<STANDARDSWEB_VIEW> Web_View { get; set; }
        
    }
    
    public class standardsBookView
    {

        public List<STANDARDSBOOK_VIEW> BookView { get; set; }

    }
    
}
