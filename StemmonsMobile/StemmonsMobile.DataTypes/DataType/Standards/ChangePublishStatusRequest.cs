
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class ChangePublishStatusRequest

    {
      
        public int iAppAssocMetadataVersionID { get; set; }
        public char cStatus { get; set; }
        public string sChangedBy { get; set; }
      
        
    }
}