
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetExternalDatasourceResultResponse:Response
    {
        public object ResponseContent { get; set; }

        public class ExternalObjectDataItem
        {
            public int? ID { get; set; }

            public string NAME { get; set; }

            public string DESCRIPTION { get; set; }

            public ExternalObjectDataItem(int? intQuestObjectID, string strQuestObjectDisp, string strQuestObjectDesc)
            {
                ID = intQuestObjectID;
                NAME = strQuestObjectDisp;
                DESCRIPTION = strQuestObjectDesc;
            }

            public ExternalObjectDataItem()
            {
                // statements
            }
        }
    }
}