using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class AssignEntityRequest
    {
        public int EntityID { get; set; }
        public string AssignTo { get; set; }
        public string AssignBy { get; set; }
    }
}