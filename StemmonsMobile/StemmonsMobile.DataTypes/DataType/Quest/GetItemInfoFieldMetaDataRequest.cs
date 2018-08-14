using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemInfoFieldMetaDataRequest
    {
        public int itemInstanceTranID { get; set; }
        public string itemInfoFieldIDs { get; set; }
        public string showOnPage { get; set; }
        public string username { get; set; }
    }
}