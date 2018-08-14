using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetTypeValuesByAssocCaseTypeExternalDSRequest
    {
        #region GetTypeValuesByAssocCaseTypeExternalDS
        public class GetTypeValuesByAssocCaseTypeExternalDS
        {
            /// <summary>
            /// Case type id
            /// </summary>
            public int caseTypeID { get; set; }
            /// <summary>
            /// Assoc case type id
            /// </summary>
            public int assocCaseTypeID { get; set; }
            /// <summary>
            /// Dropdown values of case
            /// </summary>
            public string caseTypeDesc { get; set; }
            /// <summary>
            /// Current user who created this case
            /// </summary>
            public string systemCode { get; set; }
            /// <summary>
            /// Note type id of case
            /// </summary>
            public bool isRequired { get; set; }
            /// <summary>
            /// Case notes for new case
            /// </summary>
            public Dictionary<string, string> currentValues { get; set; }
            /// <summary>
            /// Text values of new case
            /// </summary>
            public bool reLoad { get; set; }
            /// <summary>
            /// Weather to send alerts
            /// </summary>
            public int? externalDataSourceID { get; set; }
            /// <summary>
            /// Weather to send alerts
            /// </summary>
            public Dictionary<string, string> currentValuesAndExternalDatasorces { get; set; }
            public int parent { get; set; }
            public int child { get; set; }
        }
        #endregion
    }
}