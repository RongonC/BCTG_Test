using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class AddFormRequest
    {
        public int itemId { get; set; }
        public bool isEdit { get; set; }
        public string otherComments { get; set; }
        public List<ItemInfoFieldInfo> ItemInfoFieldValues { get; set; }

        public string itemQuestionFieldIDs { get; set; }
        public string meetsStandards { get; set; }
        public string pointsAvailable { get; set; }
        public string pointsEarned { get; set; }
        public string isCaseRequested { get; set; }
        public string notes { get; set; }
        public string createdBy { get; set; }

        public class ItemInfoFieldInfo
        {
            public int ItemInfoFieldId { get; set; }
            public InfoFieldValues ItemInfoFieldData { get; set; }
        }
        public class InfoFieldValues
        {
            public string ItemInfoFieldText { get; set; }
            public string externalDatasourceObjectIDs { get; set; }

        }
    }
}