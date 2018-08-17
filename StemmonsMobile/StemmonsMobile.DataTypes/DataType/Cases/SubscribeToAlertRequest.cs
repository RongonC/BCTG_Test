using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class SubscribeToAlertRequest
    {
        #region SubscribeToAlertModel
        public class SubscribeToAlert
        {
            /// <summary>
            /// Case ID for subscribe
            /// </summary>
            public int? caseId { get; set; }
            /// <summary>
            /// Case type ID for subscribe
            /// </summary>
            public int caseTypeId { get; set; }
            /// <summary>
            /// Email Address for subscribe
            /// </summary>
            public string emailAddress { get; set; }
            /// <summary>
            /// Assoc system code Id
            /// </summary>
            public int assocSystemCodeId { get; set; }
            /// <summary>
            /// Alert type to subscribe
            /// </summary>
            public char alertType { get; set; }
            /// <summary>
            /// Alert frequency to subscribe
            /// </summary>
            public char alertFrequency { get; set; }
            /// <summary>
            /// Alert hour to subscribe
            /// </summary>
            public byte? alertHour { get; set; }
            /// <summary>
            /// Current user
            /// </summary>
            public string currentUser { get; set; }
        }
        #endregion
    }
}