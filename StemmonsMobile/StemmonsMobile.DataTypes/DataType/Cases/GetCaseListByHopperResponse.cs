using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseListByHopperResponse : Response
    {
        public object ResponseContent { get; set; }

        public class HopperCenterData
        {
            public string HopperName { get; set; }
            public string HopperUsername { get; set; }
            public int? HopperID { get; set; }
            public int? OpenCaseCount { get; set; }
            public int? CreatedByMeCount { get; set; }
            public int? OwnedByMeCount { get; set; }
            public string SPORGField { get; set; }
            public string CaseTypeOwnerEmail { get; set; }
            public string CaseTypeOwnerName { get; set; }
            public string CaseTypeOwnerHelpUrl { get; set; }
            public bool HasSecurity { get; set; }
            public string OpenCaseCountLink { get; set; }
            public string CreatedByMeCountLink { get; set; }
            public string OwnedByMeCountLink { get; set; }
            public List<string> SecurityGroups { get; set; }
            public string SecurityType { get; set; }
        }
    }
}