using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class UpdateFileByQuestionMetadataIDNewFormRequest
    {
        public int? fileID { get; set; }
        public int? itemQuestionFieldID { get; set; }
        public int? itemInstanceTransID { get; set; }
        public string modifiedBy { get; set; }
    }
}