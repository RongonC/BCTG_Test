

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntitiesBySystemCodeKeyValuePair_LazyLoadResponse : Response
    {
        public object ResponseContent { get; set; }
        public List<EntityClass> result { get; set; }
    }
}