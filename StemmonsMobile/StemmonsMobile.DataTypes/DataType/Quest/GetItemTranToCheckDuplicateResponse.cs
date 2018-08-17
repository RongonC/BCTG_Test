
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemTranToCheckDuplicateResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemTranToCheckDuplicate
        {
            public ItemTranToCheckDuplicate()
            {
            }
            public int intItemInfoMetadataID { get; set; }

            public int intItemInstanceTranID { get; set; }
        }
    }
}