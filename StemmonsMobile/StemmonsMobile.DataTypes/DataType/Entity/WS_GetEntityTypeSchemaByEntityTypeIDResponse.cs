

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class WS_GetEntityTypeSchemaByEntityTypeIDResponse : Response
    {
        public object ResponseContent { get; set; }
        public EntityType result { get; set; }
    }
}