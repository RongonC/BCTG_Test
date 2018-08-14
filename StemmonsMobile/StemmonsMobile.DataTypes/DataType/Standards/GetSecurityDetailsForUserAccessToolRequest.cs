
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class GetSecurityDetailsForUserAccessToolRequest

    {
        public string user { get; set; }
        public int AppAssocMetaDataId { get; set; }
        public int AppId { get; set; }
        
    }
}