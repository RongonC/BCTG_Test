
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class AddTypeRequest

    {


        public int TypeID { get; set; }
        public string Type { get; set; }
        public char IsActive { get; set; }
        public string CreatedBy { get; set; }


    }
}