using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class ItemInstanceTranData
    {
        public int intItemInstanceTranID { get; set; }

        public int intItemID { get; set; }

        public int intListID { get; set; }

        public bool? blnIsEdit { get; set; }

        public string strItemName { get; set; }

        public string strOtherComments { get; set; }

        public int intAreaID { get; set; }

        public string strAreaName { get; set; }

        public string strSupervisorEmail { get; set; }

        public bool? blnSuppressAlert { get; set; }

        public bool? blnHidePoints { get; set; }

        public string strExtraField1 { get; set; }

        public string strCreatedBy { get; set; }

        public DateTime dtCreatedDateTime { get; set; }

        public string strModifiedby { get; set; }

        public DateTime? dtModifiedDateTime { get; set; }

        public ItemInstanceTranData()
        {
            // to do statements
        }
    }
}