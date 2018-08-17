using System;
using System.Collections.Generic;
using System.Linq;
 
using static StemmonsMobile.DataTypes.DataType.Quest.AddFormRequest;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class UpdateFormRequest
    {
        public int itemInstanceTranId { get; set; }
        public bool isEdit { get; set; }
        public string otherComments { get; set; }
        public string itemInfoFieldIds { get; set; }
        public string itemInfoFieldValues { get; set; }
        public string externalDatasourceObjectIDs { get; set; }
        public string itemQuestionFieldIDs { get; set; }
        public string meetsStandards { get; set; }
        public string pointsAvailable { get; set; }
        public string pointsEarned { get; set; }
        public string isCaseRequested { get; set; }
        public string notes { get; set; }
        public string modifiedBy { get; set; }
    }
}