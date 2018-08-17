using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetExdCascadedValueRequest
    {
        #region GetExdCascadedValue
        public class GetExdCascadedValue
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
            /// Case type description
            /// </summary>
            public string caseTypeDesc { get; set; }
            /// <summary>
            /// Parent selected values with assoctypeid and Externaldatasource object id
            /// </summary>
            public List<GetCaseTypesRequest.ParentSelectedValues> parentSelectedValues { get; set; }
            public string systemCode { get; set; }
        }
        #endregion
    }
}