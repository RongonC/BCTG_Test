using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetExternalDatasourceByQueryRequest
    {
        
        public string Query { get; set; }
        public string Connectionstring { get; set; }
    }
}