using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class DeclineAndAssignToRequest
    {
        public int caseId { get; set; }
        public string userName { get; set; }
        public string notes { get; set; }
        public string newUserName { get; set; }

    }
}