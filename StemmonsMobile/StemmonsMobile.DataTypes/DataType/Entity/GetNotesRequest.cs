using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetNotesRequest
    {
        public string UserName { get; set; }
        public int EntityId { get; set; }
    }
}