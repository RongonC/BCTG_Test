using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StemmonsMobile.DataTypes.DataType
{
    public class FileItem
    {
        public int FileID { get; set; }
        public int AppID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime ModifiedDateTime { get; set; }
        public string Description { get; set; }
        public string ExternalURI { get; set; }
        public byte[] BLOB { get; set; }
        public DateTime FileDateTime { get; set; }
        public string FileName { get; set; }
        public int FileSizebytes { get; set; }
        public int FileVersion { get; set; }
        public char IsActive { get; set; }
        public string ModifiedBy { get; set; }
        public char ShowInlineNotes { get; set; }
        public string SystemCode { get; set; }
        public string MigratedPath { get; set; }
    }
}

