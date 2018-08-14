using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseTypeDataByUserRequest
    {
        public int caseTypeId { get; set; }
        public string username { get; set; } 
    }
}