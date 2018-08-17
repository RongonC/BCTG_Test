
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class BoxerCentralHome_GetQuestFormsForUserResponse : Response
    {
        public object ResponseContent { get; set; }


    }

    public class ItemInstanceTranToProcessCase
    {
       
        public int intItemInstanceTranID { get; set; }

        public int intItemID { get; set; }

        public int intListID { get; set; }

        public bool? blnIsEdit { get; set; }

        public string strItemName { get; set; }

        public string strOtherComments { get; set; }

        public int intItemInstanceTranCaseID { get; set; }

        public string strNotes { get; set; }

        public int? intCaseID { get; set; }

        public int intAreaID { get; set; }

        public string strAreaName { get; set; }

        public string strInstanceUser { get; set; }

        public string strInstanceUserMGR { get; set; }

        public decimal? dcOverAllScore { get; set; }

        public int intItemThresholdID { get; set; }

        public int intThresholdTypeID { get; set; }

        public decimal? dcPointsEarnedThreshold { get; set; }

        public string strAssignCaseTo { get; set; }

        public bool? blnIsUserCreatedCMECase { get; set; }

        public bool? blnIsContinueWithRules { get; set; }

        public int? intOrderPriority { get; set; }

        public string strType { get; set; }

        public string strManager { get; set; }

        public string strLead { get; set; }

        public string strPropID { get; set; }

        public string strProperty { get; set; }

        public string strPropCode { get; set; }

        public string strMarketID { get; set; }

        public string strMarket { get; set; }

        public string strInspectedBy { get; set; }

        public string strInspectedBySam { get; set; }

        public string strSuite { get; set; }

        public DateTime? Date { get; set; }

        public decimal? Overall { get; set; }

        public DateTime CreatedDatetime { get; set; }

        public string strCreatedBy { get; set; }

        public int? intQuarter { get; set; }

        public int? intYear { get; set; }

        public string intTenantId { get; set; }

        public string strTenantName { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string strIsLock { get; set; }

    }

}