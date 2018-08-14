
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemQuestionFieldsByItemCategoryIDResponse:Response
    {
        public object ResponseContent { get; set; }

        public class ItemQuestionField
        {
            public ItemQuestionField()
            { }
            public ItemQuestionField(int ITEM_QUESTION_FIELD_ID, string NAME)
            {
                intItemQuestionFieldID = ITEM_QUESTION_FIELD_ID;
                strItemQuestionFieldName = NAME;
            }
            public int intItemQuestionFieldID { get; set; }

            /******************Added By Vaibhav shah************************/

            public int intItemQuestionMetaDataID { get; set; }

            /******************Added By Vaibhav shah************************/

            public int intItemCategoryID { get; set; }

            public string strItemQuestionFieldName { get; set; }

            public string strItemQuestionFieldDesc { get; set; }

            public string strItemQuestionHeader1 { get; set; }

            public string strItemQuestionHeader2 { get; set; }

            public string strItemQuestionHeader3 { get; set; }

            public string strExtraField1 { get; set; }


            public string strAreaName { get; set; }

            public int intAreaID { get; set; }

            public int intItemID { get; set; }

            public string strItemName { get; set; }

            public string strCategoryName { get; set; }

            public int? intDisplayOrder { get; set; }

            public string strIsActive { get; set; }

            public string strIsShowCMECheckBox { get; set; }

            public string strEXTRA_FIELD2 { get; set; }
        }
    }
}