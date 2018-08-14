using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemInfoDependencyRequest
    {
        public int itemID { get; set; }
        public int itemInfoFieldIDChild { get; set; }
    }
}