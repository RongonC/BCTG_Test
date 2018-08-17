
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetCustomTemplateTagsRequest 
    {
        public string EntityTypeId { get; set; }
        public string Tags { get; set; }

        public string Path { get; set; }
        public bool GetDefaultTemplate { get; set; }
    }
}