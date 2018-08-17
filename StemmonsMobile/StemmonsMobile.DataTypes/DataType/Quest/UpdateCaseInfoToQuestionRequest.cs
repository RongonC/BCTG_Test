using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class UpdateCaseInfoToQuestionRequest
    {
        public int itemQuestionMetadataCaseId { get; set; }
        public int caseId { get; set; }
        public string modifiedBy { get; set; }
    }
}