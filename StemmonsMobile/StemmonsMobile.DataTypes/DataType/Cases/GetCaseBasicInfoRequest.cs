using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseBasicInfoRequest
    {
        public string username { get; set; }
        public int CaseId { get; set; }
    }
}