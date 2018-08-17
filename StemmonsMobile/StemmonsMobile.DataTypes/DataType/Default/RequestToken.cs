using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Default
{
    public class RequestToken
    {
        public string grant_type { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
}