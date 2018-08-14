
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class UpdateAppMetadataRequest

    {

        public int AsscMetadataID { get; set; }
        public int TypeID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string DescPlainText { get; set; }
        public string Content { get; set; }
        public string ContentPlaintText { get; set; }
        public string Summary { get; set; }
        public string SummaryPlainText { get; set; }
        public int ParentMetaDataID { get; set; }
        public int AppID { get; set; }
        public int DisplayOrder { get; set; }
        public string AppLogo { get; set; }
        public string AppHeader { get; set; }
        public string Footer { get; set; }
        public char IsActive { get; set; }
        public string CreatedBy { get; set; }


    }
}