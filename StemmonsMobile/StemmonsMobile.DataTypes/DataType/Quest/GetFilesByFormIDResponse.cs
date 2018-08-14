
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetFilesByFormIDResponse:Response
    {
        public object ResponseContent { get; set; }
        public class FilesByFormID
        {
            public int intItemInstanceTranFileID { get; set; }

            public int intItemInstanceTranID { get; set; }

            public string strDescription { get; set; }

            public string strFileName { get; set; }

            public int intFileSizeBytes { get; set; }

            public string bnFileBinary { get; set; }

            public char strIsActive { get; set; }

            public DateTime dtCreatedDateTime { get; set; }

            public string strCreatedBy { get; set; }

            public DateTime? dtModifiedDateTime { get; set; }

            public string strModifiedBy { get; set; }

            public FilesByFormID()
            {
                // statements
            }
        }
    }
}