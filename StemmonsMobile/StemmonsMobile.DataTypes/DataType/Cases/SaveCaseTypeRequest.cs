using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class SaveCaseTypeRequest
    {
        public string TransactionType { get; set; }

        /// <summary>
        /// Case ref ItemTranInfoID
        /// </summary>
        public string RefItemTranInfoID { get; set; }
        /// <summary>
        /// Case id to update data
        /// </summary>
        public int caseId { get; set; }
        /// <summary>
        /// Case title for new case
        /// </summary>
        public string caseTitle { get; set; }
        /// <summary>
        /// Dropdown values of case
        /// </summary>
        public Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> dropDownValues { get; set; }
        /// <summary>
        /// Current user who created this case
        /// </summary>
        public string currentUser { get; set; }
        /// <summary>
        /// Note type id of case
        /// </summary>
        public int noteTypeID { get; set; }
        /// <summary>
        /// Case notes for new case
        /// </summary>
        public string caseNote { get; set; }
        /// <summary>
        /// Text values of new case
        /// </summary>
        public Dictionary<int, string> textValues { get; set; }
        /// <summary>
        /// Weather to send alerts
        /// </summary>
        public bool sendAlerts { get; set; }
        /// <summary>
        /// Weather to update case list cache
        /// </summary>
        public bool updateCaseListCache { get; set; }
    }
}