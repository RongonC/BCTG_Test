
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class OriginationCenterDataResponse : Response
    {
        public object ResponseContent { get; set; }
        public class OriginationCenterData
        {
            public string CaseTypeName { get; set; }
            public int? CaseTypeID { get; set; }
            public int? OpenCaseCount { get; set; }
            public int? TotalCaseCount { get; set; }
            public int? AssignedToMeCount { get; set; }
            public int? TotalPastDueCaseCount { get; set; }
            public int? PastDueAssignedToMeCount { get; set; }
            public int? CreatedByMeCount { get; set; }
            public int? ClosedCaseCount { get; set; }
            public int? OwnedByMeCount { get; set; }
            public int? ClosedByMeCount { get; set; }
            public string CreateNewCaseURL { get; set; }
            public string SPORGField { get; set; }
            public string DefaultHopperName { get; set; }
            public string CaseTypeOwnerEmail { get; set; }
            public string CaseTypeOwnerName { get; set; }
            public string CaseTypeOwnerHelpUrl { get; set; }
            public bool HasSecurity { get; set; }
            public string OpenCaseCountLink { get; set; }
            public string ClosedCaseCountLink { get; set; }
            public string TotalCaseCountLink { get; set; }
            public string AssignedToMeCountLink { get; set; }
            public string PastDueAssignedToMeCountLink { get; set; }
            public string CreatedByMeCountLink { get; set; }
            public string OwnedByMeCountLink { get; set; }
            public string ClosedByMeCountLink { get; set; }
            public string TotalPastDueCaseCountLink { get; set; }

            public List<string> SecurityGroups { get; set; }

            public string SecurityType { get; set; }
        }
    }
}