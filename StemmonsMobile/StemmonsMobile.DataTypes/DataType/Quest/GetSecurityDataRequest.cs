using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetSecurityDataRequest
    {
        public int? itemInstanceTranId { get; set; }
        public string user { get; set; }
    }
}