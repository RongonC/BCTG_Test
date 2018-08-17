using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class UpdateEntityIDForFileRequest
    {
        public string EntityFileId { get; set; }
        public int EntityId { get; set; }
        public string ModifiedBy { get; set; }
    }
}