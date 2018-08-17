using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Xml.Serialization;

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class EXTERNAL_DATASOURCE
    {
        
        public int ID { get; set; }

        public string EXTERNAL_DATASOURCE_NAME { get; set; }

        public string EXTERNAL_DATASOURCE_DESCRIPTION { get; set; }

        public int Count { get; set; }
    }
}