using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetFilesByFileIDRequest
    {
        public string UserName { get; set; }

        public int FileId { get; set; }

        public int? EntityId { get; set; }
    }
}