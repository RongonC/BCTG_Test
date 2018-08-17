
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class security_GetAppMetadataRequest

    {
        
        public int AppID { get; set; }
        public int? iParentID { get; set; }
        public string sCurrentUser { get; set; }
        

    }
}