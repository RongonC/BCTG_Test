using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class AddFiletoFormRequest
    {
        //public int itemInstanceTranID { get; set; }
        //public string description { get; set; }
        //public string createdBy { get; set; }


        public int intItemInstanceTranID { get; set; }
        public string strDescription { get; set; }
        public string strFileName { get; set; }
        public int intFileSizeBytes { get; set; }
        public byte[] FileBinary { get; set; }
        public string strCreatedBy { get; set; }
    }
}