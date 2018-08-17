

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetFilesByFileIDResponse : Response
    {
        public object ResponseContent { get; set; }
        public EntityFile result { get; set; }

        
    }
}