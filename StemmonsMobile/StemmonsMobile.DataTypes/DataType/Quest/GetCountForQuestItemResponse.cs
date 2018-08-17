
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetCountForQuestItemResponse : Response
    {
        public object ResponseContent { get; set; }

        public class CountForQuestItem
        {
            public int? Pending { get; set; }
            public int? Finalized { get; set; }
            public int? Total { get; set; }
            public int? MyPending { get; set; }
            public int? MyFinalized { get; set; }
            public string Owner { get; set; }
            public string Newest { get; set; }
            public string securityType{ get; set; }
        }
    }
}