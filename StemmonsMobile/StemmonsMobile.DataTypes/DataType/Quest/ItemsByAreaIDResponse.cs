
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class ItemsByAreaIDResponse:Response
    {
        public object ResponseContent { get; set; }
        public class ItemsByAreaID
        {
            public int intItemID { get; set; }

            public string strItemName { get; set; }

            public string strItemDesc { get; set; }

            public int intAreaID { get; set; }
            public string securityType { get; set; }

            public ItemsByAreaID(int ItemID, string ItemName)
            {
                intItemID = ItemID;
                strItemName = ItemName;
                strisActive = "Y";
            }

            public ItemsByAreaID()
            { }


            public string strSupervisorEmail { get; set; }

            public int? blnSuppressAlert { get; set; }

            public bool? blnHidePoints { get; set; }

            public string strisActive { get; set; }

            public string strHeader1 { get; set; }

            public string strHeader2 { get; set; }

            public string strHeader3 { get; set; }

            public string strEXTRA_FIELD1 { get; set; }
        }
    }
}