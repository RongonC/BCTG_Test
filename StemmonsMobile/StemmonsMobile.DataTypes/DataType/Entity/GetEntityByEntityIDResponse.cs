

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntityByEntityIDResponse : Response
    {
        public object ResponseContent { get; set; }
        public EntityClass result { get; set; }
    }
}