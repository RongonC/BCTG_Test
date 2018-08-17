using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.Models
{
    public class Case_Type_Data
    {
        public string CaseTypeName { get; set; }
        public string CaseTypeID { get; set; }
        public string OpenCaseCount { get; set; }
        public string TotalCaseCount { get; set; }
        public string AssignedToMeCount { get; set; }
        public string TotalPastDueCaseCount { get; set; }
        public string PastDueAssignedToMeCount { get; set; }
        public string CreatedByMeCount { get; set; }
        public string ClosedCaseCount { get; set; }
        public string OwnedByMeCount { get; set; }
        public string ClosedByMeCount { get; set; }
        public string CreateNewCaseURL { get; set; }
        public string SPORGField { get; set; }
        public string DefaultHopperName { get; set; }
        public string CaseTypeOwnerEmail { get; set; }
        public string CaseTypeOwnerName { get; set; }
        public string CaseTypeOwnerHelpUrl { get; set; }
        public string HasSecurity { get; set; }
        public string OpenCaseCountLink { get; set; }
        public string ClosedCaseCountLink { get; set; }
        public string TotalCaseCountLink { get; set; }
        public string AssignedToMeCountLink { get; set; }
        public string PastDueAssignedToMeCountLink { get; set; }
        public string CreatedByMeCountLink { get; set; }
        public string OwnedByMeCountLink { get; set; }
        public string ClosedByMeCountLink { get; set; }
        public string TotalPastDueCaseCountLink { get; set; }
        public string SecurityGroups { get; set; }
        public string SecurityType { get; set; }
    }
}
