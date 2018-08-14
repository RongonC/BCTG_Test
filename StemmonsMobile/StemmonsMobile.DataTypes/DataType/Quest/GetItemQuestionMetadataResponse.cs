
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetItemQuestionMetadataResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ItemQuestionMetaData
        {
            public int intItemInstanceTranID { get; set; }

            public int intItemID { get; set; }

            public string strItemName { get; set; }

            public string strOtherComments { get; set; }

            public int intItemQuestionMetadataID { get; set; }

            public int intItemQuestionFieldID { get; set; }

            public string strQuestion { get; set; }

            public string strMeetsStandards { get; set; }

            public decimal? strPointsAvailable { get; set; }

            public decimal? strPointsEarned { get; set; }

            public decimal? strWeight { get; set; }

            public string strNotes { get; set; }

            public int? intCaseID { get; set; }

            public int? intTrainingID { get; set; }

            public int intItemCategoryID { get; set; }

            public string strItemCategoryName { get; set; }

            public string strCaseTypeId { get; set; }

            public ItemQuestionMetaData()
            { }


            public int intListID { get; set; }

            public string blnIsEdit { get; set; }

            public int intAreaID { get; set; }

            public string blnIsAutoCase { get; set; }

            public string strIsCaseRequested { get; set; }

            public int? blnIsHighlightRow { get; set; }

            public string strAreaName { get; set; }

            public string strSupervisorEmail { get; set; }

            public string blnSuppressAlert { get; set; }

            public string blnHidePoints { get; set; }

            public string PointsAvail
            {
                get
                {
                    return $"{"Points Available:"} {strPointsAvailable}";
                }
            }

            public string PointsEarned
            {
                get
                {
                    return $"{"Earned:"}{strPointsEarned}";
                }
            }

            public string Score
            {
                get
                {
                    Double points1 = (Convert.ToDouble(strPointsAvailable == null ? 0 : strPointsAvailable));
                    Double points2 = (Convert.ToDouble(strPointsEarned == null ? 0 : strPointsEarned));
                    Double totalpercentage = (points2 / points1) * 100;
                    totalpercentage = Math.Truncate(Double.IsNaN(totalpercentage) || Double.IsInfinity(totalpercentage) ? 0 : totalpercentage);
                    return Convert.ToString(totalpercentage);
                }
            }
        }
    }
}