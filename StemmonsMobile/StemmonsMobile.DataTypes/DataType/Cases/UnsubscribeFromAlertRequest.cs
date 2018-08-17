using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class UnsubscribeFromAlertRequest
    {
        public int caseTypeId { get; set; }
        public string emailAddress { get; set; }
        public string username { get; set; }
    }
}