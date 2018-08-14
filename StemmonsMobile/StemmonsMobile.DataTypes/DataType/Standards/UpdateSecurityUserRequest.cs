
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class UpdateSecurityUserRequest

    {
        public int iStandardsResourceSecurityUserID { get; set; }
        public char cIsAdd { get; set; }
        public char cIs_Edit { get; set; }
        public char cIs_Read { get; set; }
        public char cIs_Delete { get; set; }
        public string sModifyBy { get; set; }
        
    }
}