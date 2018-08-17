
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class CasesHomeGetCaseListUserResponse : Response
    {
        public object ResponseContent { get; set; }
    }
	
    public class CasesHome
    {
        public int CaseID { get; set; }

        public string ListID { get; set; }

        public string CaseTypeName { get; set; }

        public string CaseTitle { get; set; }

        public string CaseStatusValue { get; set; }

        public string CasePriorityValue { get; set; }

        public string CaseLifeDHM { get; set; }

        public string CaseDue { get; set; }

        public string CaseCreatedByDisplayName { get; set; }
        public string CaseOwnerDisplayName { get; set; }

        public string CaseAssgnToDisplayName { get; set; }
    }

}