

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntityTypeConfigurationResponse : Response
    {
        public object ResponseContent { get; set; }
        public EntityTypes result { get; set; }
    }
}