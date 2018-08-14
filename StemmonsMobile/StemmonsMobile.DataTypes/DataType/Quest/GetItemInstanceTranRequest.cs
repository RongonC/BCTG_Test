using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemInstanceTranRequest
    {
        public int? itemInstanceTranID { get; set; }
        public string itemId { get; set; }
        public bool? isIncludeClosedItems { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string itemInstanceTransIDs { get; set; }
        public string username { get; set; }
    }
}