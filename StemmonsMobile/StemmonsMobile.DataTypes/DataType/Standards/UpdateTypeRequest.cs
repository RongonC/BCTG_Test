
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class UpdateTypeRequest

    {

        
        public int TypeID { get; set; }
        public string Type { get; set; }
        public char IsActive { get; set; }
        public string ModifiedBy { get; set; }
        


    }
}