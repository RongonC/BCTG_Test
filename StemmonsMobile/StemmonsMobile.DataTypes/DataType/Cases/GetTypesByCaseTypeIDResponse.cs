
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetTypesByCaseTypeIDResponse:Response
    {
        public object ResponseContent { get; set; }

        #region GetTypesByCaseType
        public class GetTypesByCaseType
        {
            public int AssocTypeId { get; set; }
            public char AssocFieldType { get; set; }
            public int CaseTypeId { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int ExternalDataSourceId { get; set; }
            public string SystemCode { get; set; }
            public int SystemPriority { get; set; }
            public string ShowOnList { get; set; }
            public char IsRequired { get; set; }
            public char IsActive { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            public DateTime ModifiedDateTime { get; set; }
            public string ModifiedBy { get; set; }
            public string SeparatorCharacter { get; set; }
            public int ExternalDataSourceEntityTypeId { get; set; }
            public string AssociatedTypeSecurity { get; set; }
        }
        #endregion
    }
}