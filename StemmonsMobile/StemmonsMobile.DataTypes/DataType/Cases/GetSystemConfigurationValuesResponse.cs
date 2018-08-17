
using System;
using System.Collections.Generic;
using System.Linq;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetSystemConfigurationValuesResponse:Response
    {
        public object ResponseContent { get; set; }

        #region SystemConfig
        public class SystemConfig
        {
            public string SystemCode { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
            public DateTime CreatedDateTime { get; set; }
            public string CreatedBy { get; set; }
            public DateTime ModifiedDateTime { get; set; }
            public string ModifiedBy { get; set; }
        }
        #endregion
    }
}