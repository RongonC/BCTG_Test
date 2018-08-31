
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class CaseTypeList
    {
        public int caseTypeId { get; set; }
        public string Name { get; set; }
        public string instanceName { get; set; }
        public string instanceNamePlural { get; set; }
        public string Description { get; set; }
        public string sporgOverride { get; set; }
        public int? defaultHooperId { get; set; }
        public char? newestNotesOnTop { get; set; }
        public char? LockWhenUnowned { get; set; }
        public string allowedSecurityGroups { get; set; }
        public string CaseOwner { get; set; }
        //public string CaseOwnerDisplayName { get { return DataAccessCME.GetUserInfo(CaseOwner).DisplayName; } }
        public string helpUrl { get; set; }
        public char? isAllowApproval { get; set; }
        public char? isForceSyncToFacts { get; set; }
        public char? isExportToFacts { get; set; }
        public char? isExportClosedToFactsOption { get; set; }
        public char? isShowPermissionIconToCaseOwner { get; set; }
        public char? isAllowOutsideEmailToCases { get; set; }
        public char? isCopyNotesToChild { get; set; }
        public char? isCopyStatusToChild { get; set; }
    }

    public class Case_List
    {
        private string _caseTitle;
        public int CaseTypeId { get; set; }
        public int CaseID { get; set; }
        public int ListID { get; set; }
        public string CaseTypeName { get; set; }
      
        public override string ToString()
        {
            string format = "{0}, CaseID = {1},CaseTypeId = {2}";
            return string.Format(format, CaseTypeName, this.CaseID, CaseTypeId);
        }
    }
}