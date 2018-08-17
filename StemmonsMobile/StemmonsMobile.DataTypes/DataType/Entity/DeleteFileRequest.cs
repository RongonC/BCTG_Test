using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class DeleteFileRequest
    {
        public int FileID { get; set; }
        public string CurrentUser { get; set; }
    }
}