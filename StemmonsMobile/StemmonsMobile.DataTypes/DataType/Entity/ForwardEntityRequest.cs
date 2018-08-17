using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class ForwardEntityRequest
    {
        public int EntityID { get; set; }
        public string FORWARD_TO_USER { get; set; }
        public string FORWARD_BY_USER { get; set; }

    }
}