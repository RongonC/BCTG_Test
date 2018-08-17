
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class GetSecurityForUserAccessToolRequest

    {

        public string user { get; set; }
        public int? AppAssocMetaDataId { get; set; }
        public int? AppId { get; set; }


    }
}