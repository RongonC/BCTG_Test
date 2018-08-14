
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemCategoriesByItemID_ViewScoresResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemCategory_ViewScores
        {
            public int intItemCategoryID { get; set; }

            public int intItemID { get; set; }

            public string strItemCategoryName { get; set; }

            public string strItemCategoryDesc { get; set; }

            public ItemCategory_ViewScores()
            { }

            public ItemCategory_ViewScores(int ItemCategoryID, string ItemCategoryName)
            {
                intItemCategoryID = ItemCategoryID;
                strItemCategoryName = ItemCategoryName;
            }

            public string strExtraField1 { get; set; }


            public string strItemName { get; set; }

            public string strItemDesc { get; set; }

            public int intAreaID { get; set; }

            public string strAreaName { get; set; }

            public string strAreaDesc { get; set; }


            public int? intDisplayOrder { get; set; }

            public string strSupervisorEmail { get; set; }

            public bool? blnSuppressAlert { get; set; }

            public bool? blnHidePoints { get; set; }

            public string strIsProcessCMECase { get; set; }

            public string strisActive { get; set; }

            public decimal dcOverallScoreCategory { get; set; }

            public decimal? dcOverallScoreForm { get; set; }

            public int? intTotalQuestionCount { get; set; }

            public int? intTotalQuestionCount_NA { get; set; }

            public bool? blnIsEdit { get; set; }
        }
    }
}