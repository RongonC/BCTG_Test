using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemCategoriesByItemIDResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemCategoryByItemId
        {
            public int intItemCategoryID { get; set; }

            public int intItemID { get; set; }

            public string strItemCategoryName { get; set; }

            public string strItemCategoryDesc { get; set; }
            public string strExtraField1 { get; set; }
            public int? intDisplayOrder { get; set; }
            public string strSupervisorEmail { get; set; }
            public bool? blnSuppressAlert { get; set; }
            public bool? blnHidePoints { get; set; }
            public string strIsProcessCMECase { get; set; }
            public string strisActive { get; set; }
            public string strHeader1 { get; set; }
            public string strHeader2 { get; set; }
            public string strHeader3 { get; set; }
            public string strIsShowCMECheckBox { get; set; }

            public string strAvaliablePoints { get; set; }
            public string strEarned { get; set; }
            public string strScore { get; set; }

            public string PointsAvail
            {
                get
                {
                    return $"{"Points Availables:"} {strAvaliablePoints}";
                }
            }


            public string PointsEarn
            {
                get
                {
                    string val = string.IsNullOrEmpty(strEarned) ? "0" : strEarned;
                    return $"{"Points Earned:"} {val}";
                }
            }

            public string FinalScore
            {
                get
                {
                    string val = string.IsNullOrEmpty(strScore) ? "0" : strScore;
                    return $"{"Section Score:"} {val + "%"}";
                }
            }
        }
    }
}