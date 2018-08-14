
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse:Response
    {
        public object ResponseContent { get; set; }

        public class ItemQuestionField_ViewScoresModel
        {
            public int intItemQuestionFieldID { get; set; }

            public int intItemCategoryID { get; set; }

            public string strItemQuestionFieldName { get; set; }

            public string strItemQuestionFieldDesc { get; set; }


            public string strExtraField1 { get; set; }

            public int? intDisplayOrder { get; set; }

            public string strIsActive { get; set; }

            public string strMeetsStandards { get; set; }

            public decimal? dcPointsAvailable { get; set; }

            public decimal? dcPointsEarned { get; set; }
        }
    }
}