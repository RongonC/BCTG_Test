using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetExternalDatasourceResultRequest
    {
        public string strConnString_External { get; set; }
        public string strQuery { get; set; }
    }
}