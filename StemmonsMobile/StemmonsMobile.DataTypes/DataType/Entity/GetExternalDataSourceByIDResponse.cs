

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetExternalDataSourceByIDResponse : Response
    {
        public object ResponseContent { get; set; }

        public ExternalDatasource result { get; set; }
        //public int ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID { get; set; }
        //public String EXTERNAL_DATASOURCE_NAME { get; set; }
        //public String EXTERNAL_DATASOURCE_DESCRIPTION { get; set; }
        //public String CONNECTION_STRING { get; set; }
        //public String QUERY { get; set; }
        //public String OBJECT_ID { get; set; }
        //public String OBJECT_DISPLAY { get; set; }
        //public String OBJECT_DESCRIPTION { get; set; }
        //public String URL_DRILL_INTO { get; set; }
        //public String FIELD_SYSTEM_CODE { get; set; }
        //public String VALUE_SYSTEM_CODE { get; set; }
    }
}