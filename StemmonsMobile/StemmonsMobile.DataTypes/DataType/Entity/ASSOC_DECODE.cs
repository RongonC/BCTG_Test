using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Xml.Serialization;

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class ASSOC_DECODE
    {
        [XmlElement(ElementName = "ASSOC_DECODE_SYSTEM_CODE")]
        public string ASSOC_DECODE_SYSTEM_CODE { get; set; }

        [XmlElement(ElementName = "ASSOC_DECODE_NAME")]
        public string ASSOC_DECODE_NAME { get; set; }

        [XmlElement(ElementName = "ASSOC_DECODE_DESCRIPTION")]
        public string ASSOC_DECODE_DESCRIPTION { get; set; }
    }
}
