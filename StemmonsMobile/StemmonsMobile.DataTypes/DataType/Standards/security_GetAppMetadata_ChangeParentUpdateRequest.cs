
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class security_GetAppMetadata_ChangeParentUpdateRequest

    {


        public int appId { get; set; }
        public int parentMetaDataId { get; set; }
        public string sUserName { get; set; }
    }
}