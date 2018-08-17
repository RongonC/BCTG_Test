
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class AddAppTypeRequest

    {

        public  int AppID { get; set; }
        public int TypeID { get; set; }
        public  int Level { get; set; }
        public string sCreatedBY { get; set; }

        
    }
}