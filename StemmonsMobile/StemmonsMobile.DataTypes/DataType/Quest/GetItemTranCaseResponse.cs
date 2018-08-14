
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemTranCaseResponse:Response
    {
        public object ResponseContent { get; set; }
        public class ItemInstanceTranCase
        {
            public ItemInstanceTranCase()
            {

            }
            public int intItemInstanceTranCaseID { get; set; }
            public int intItemInstanceTranID { get; set; }
            public string strNotes { get; set; }
            public int? intCaseID { get; set; }
            public char IsActive { get; set; }
            public DateTime dtCreatedDateTime { get; set; }
            public string strCreatedBy { get; set; }
            public DateTime dtModifiedDateTime { get; set; }
            public string strModifiedBy { get; set; }
        }
    }
}