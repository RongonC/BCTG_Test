
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class GetAppMetadataByParentAppForUser_BookView_SyncRequest

    {

        public string pUserSAM { get; set; }
        public int pAPP_ASSOC_META_DATA_ID { get; set; }
        public string pIncludeInactiveNodes { get; set; }

        
    }
}