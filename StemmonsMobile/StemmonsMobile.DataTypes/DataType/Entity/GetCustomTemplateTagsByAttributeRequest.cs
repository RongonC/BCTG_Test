using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetCustomTemplateTagsByAttributeRequest
    {
        public string EntityTypeId { get; set; }
        public string Attributes { get; set; }
        public string Values { get; set; }
        public bool GetDefaultTemplate { get; set; }
    }
}