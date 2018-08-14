using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class AddCaseToQuestionRequest
    {
        public int itemInstanceTranID { get; set; }
        public int itemQuestionFieldID { get; set; }
        public string notes { get; set; }
        public string createdBy { get; set; }
        public string IsCaseRequested { get; set; }
    }
}