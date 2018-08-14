
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetFilterQuery_EntityResponse : Response
    {
        public object ResponseContent { get; set; }
      
    }
    public class ConnectionStringCls
    {
        public string _QUERY { get; set; }

        public  int _INTERNAL_ENTITY_TYPE_ID { get; set; }

        public string _CONNECTION_STRING { get; set; }

        public string _FILTER_QUERY { get; set; }
    }
   
}