using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class UpdateItemInstanceTranCategoryRequest
    {
        public int itemInstanceTranId { get; set; }
        public string itemCategoryIds { get; set; }
        public string scoreValues { get; set; }
        public decimal overallScore { get; set; }
        public string modifiedBy { get; set; }
    }
}