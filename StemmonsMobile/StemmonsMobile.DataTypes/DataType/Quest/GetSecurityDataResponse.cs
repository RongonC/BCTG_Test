
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetSecurityDataResponse:Response
    {
        public object ResponseContent { get; set; }
        public class ItemInstanceTranByIDViewForm
        {
            public int intItemInstanceTranID { get; set; }

            public int intItemID { get; set; }

            public int intListID { get; set; }

            public decimal? dcOverallScore { get; set; }

            public string strOtherComments { get; set; }

            public bool? blnIsEdit { get; set; }
            public bool blnIsSuperAdmin { get; set; }
            public string strCreatedBy { get; set; }
        }
    }
}