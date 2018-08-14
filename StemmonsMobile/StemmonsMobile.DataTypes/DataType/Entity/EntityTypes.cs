using System;
using System.Collections.Generic;
using System.Linq;
 
using System.Xml.Serialization;

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    [XmlRoot(ElementName = "ENTITY_TYPE")]
    public partial class EntityTypes
    {
        [XmlElement(ElementName = "ENTITY_TYPE_ID")]
        public int ENTITY_TYPE_ID { get; set; }

        [XmlElement(ElementName = "ENTITY_ASSOC_TYPE")]
        public List<ENTITY_ASSOC_TYPE> AssocType { get; set; }

        public List<EntityTypes> ListEntityTypes { get; set; }

    }
}