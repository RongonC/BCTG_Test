
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemWiseSummaryForScoresResponse:Response
    {
        public object ResponseContent { get; set; }

        public class ItemWiseSummaryForScores
        {
            public ItemWiseSummaryForScores()
            {
                // statements
            }


            public int? intItemID { get; set; }

            public string strItemName { get; set; }

            public string strItemCategoryName1 { get; set; }
            public string strItemCategoryName2 { get; set; }
            public string strItemCategoryName3 { get; set; }
            public string strItemCategoryName4 { get; set; }
            public string strItemCategoryName5 { get; set; }
            public string strItemCategoryName6 { get; set; }
            public string strItemCategoryName7 { get; set; }
            public string strItemCategoryName8 { get; set; }
            public string strItemCategoryName9 { get; set; }
            public string strItemCategoryName10 { get; set; }

            public decimal? dcOverAllScoreCategory1 { get; set; }
            public decimal? dcOverAllScoreCategory2 { get; set; }
            public decimal? dcOverAllScoreCategory3 { get; set; }
            public decimal? dcOverAllScoreCategory4 { get; set; }
            public decimal? dcOverAllScoreCategory5 { get; set; }
            public decimal? dcOverAllScoreCategory6 { get; set; }
            public decimal? dcOverAllScoreCategory7 { get; set; }
            public decimal? dcOverAllScoreCategory8 { get; set; }
            public decimal? dcOverAllScoreCategory9 { get; set; }
            public decimal? dcOverAllScoreCategory10 { get; set; }

            public double? dcOverAllScore { get; set; }
        }
    }
}