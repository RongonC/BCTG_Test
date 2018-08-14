
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class UpdateFileDetailAppMetaDataRequest

    {


        public int iAppFileID { get; set; }
        public int iAppAssocMetaDataID { get; set; }
        public string sFileName { get; set; }
        public string sFileDesc { get; set; }
        public int iFileSizeByte { get; set; }
        public byte[] FileBob { get; set; }
        public char isActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string FileDownLoadLink { get; set; }



    }
}