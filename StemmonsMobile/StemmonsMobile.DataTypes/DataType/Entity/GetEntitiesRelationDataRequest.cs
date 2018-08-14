using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntitiesRelationDataRequest 
    {
        public string User { get; set; }
        public FILTER_VALUE EntityTypeSchema { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        public char? IsActive { get; set; }
        public char? AssignedToMe { get; set; }
        public char? CreatedByMe { get; set; }
        public char? OwnedByMe { get; set; }
        public char? AssociatedToMe { get; set; }
        public char? InActivatedByMe { get; set; }
        public string SystemCode { get; set; }


    }
}