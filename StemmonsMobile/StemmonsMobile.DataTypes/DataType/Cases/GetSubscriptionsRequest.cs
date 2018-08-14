using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetSubscriptionsRequest
    {
        public int caseTypeId { get; set; }
        public string emailAddress { get; set; }
        public int CaseId { get; set; }

    }
}