using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntityTypeRelationRequest
    {
        public int EntityId { get; set; }
        public string User { get; set; }
        public string System_Code { get; set; }
    }
}