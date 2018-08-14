using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetRelatedTypesRequest
    {
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string User { get; set; }
        public string Application { get; set; }
    }
}