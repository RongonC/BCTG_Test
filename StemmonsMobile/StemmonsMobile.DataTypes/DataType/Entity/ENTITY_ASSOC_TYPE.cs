using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Xml.Serialization;

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class ENTITY_ASSOC_TYPE
    {
        [XmlElement(ElementName = "ENTITY_ASSOC_TYPE_ID")]
        public int ENTITY_ASSOC_TYPE_ID { get; set; }

        [XmlElement(ElementName = "ASSOC_TYPE_SYSTEM_CODE")]
        public string ASSOC_TYPE_SYSTEM_CODE { get; set; }

        [XmlElement(ElementName = "ASSOC_TYPE_DESCRIPTION")]
        public string ASSOC_TYPE_DESCRIPTION { get; set; }

        [XmlElement(ElementName = "ASSOC_TYPE_NAME")]
        public string ASSOC_TYPE_NAME { get; set; }

        [XmlElement(ElementName = "ASSOC_DECODE")]
        public List<ASSOC_DECODE> Assocdecode { get; set; }

        [XmlElement(ElementName = "EXTERNAL_DATASOURCE")]
        public List<EXTERNAL_DATASOURCE> DataSource { get; set; }

        [XmlElement(ElementName = "FIELD_TYPE")]
        public string FIELD_TYPE { get; set; }



    }
}