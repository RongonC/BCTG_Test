using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class DeleteItemTranCaseRequest
    {
        public int itemInstanceTranID { get; set; }
        public int itemInstanceTranCaseID { get; set; }
    }
}