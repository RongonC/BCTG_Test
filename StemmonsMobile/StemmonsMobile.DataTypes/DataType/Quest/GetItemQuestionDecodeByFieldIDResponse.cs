
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemQuestionDecodeByFieldIDResponse:Response
    {
        public object ResponseContent { get; set; }
        public class ItemQuestionDecode
        {
            public ItemQuestionDecode()
            { }
            public int intItemID { get; set; }
            public ItemQuestionDecode(int ITEM_QUESTION_DECODE_ID, string MEETS_STANDARDS)
            {
                intItemQuestionDecodeID = ITEM_QUESTION_DECODE_ID;
                strMeetsStandards = MEETS_STANDARDS;
            }
            public int intItemQuestionDecodeID { get; set; }

            public int intItemQuestionFieldID { get; set; }

            public string strMeetsStandards { get; set; }

            public string strHeader1 { get; set; }

            public string strHeader2 { get; set; }

            public string strHeader3 { get; set; }

            public decimal? dcPointsAvailable { get; set; }

            public decimal? dcPointsEarned { get; set; }

            public int? intDisplayOrder { get; set; }


            public string strDrpValue { get; set; }

            public string strIsActive { get; set; }

            public bool? blnIsHightlightRow { get; set; }


            public string strIsDefault { get; set; }
        }
    }
}