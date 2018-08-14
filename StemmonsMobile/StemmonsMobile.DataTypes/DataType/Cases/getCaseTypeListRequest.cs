using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class getCaseTypeListRequest
    {
        public int? CaseTypeId { get; set; }
        public string UserName { get; set; }
        public string ListType { get; set; }
    }
}