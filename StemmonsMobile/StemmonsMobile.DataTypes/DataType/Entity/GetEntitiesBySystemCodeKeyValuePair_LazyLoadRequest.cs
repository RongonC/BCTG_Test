
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest

    {
        public string user { get; set; }
        public FILTER_VALUE entityTypeSchema { get; set; }

            public string SHOW_ENTITIES_ACTIVE_INACTIVE { get; set; }
        public int EntityTypeID{ get; set; }
        public int? pageIndex { get; set; }
        public int? pageSize { get; set; }
        public string sortBy { get; set; }
        public string sortDirection { get; set; }
        public char? isActive { get; set; }
        public char? assignedToMe { get; set; }
        public char? createdByMe { get; set; }
        public char? ownedByMe { get; set; }
        public char? associatedToMe { get; set; }
        public char? inActivatedByMe { get; set; }
        public string SystemCode { get; set; }

    }
}