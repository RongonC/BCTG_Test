using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntityRelatedApplicationsRequest
    {
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string User { get; set; }
    }



    public class EntitiesRelationForEntityType
    {
        public int EntityId { get; set; }
        public int EntityTypeId { get; set; }
        public string EntityTypeName { get; set; }
        public int CountofEntity { get; set; }
    }
}