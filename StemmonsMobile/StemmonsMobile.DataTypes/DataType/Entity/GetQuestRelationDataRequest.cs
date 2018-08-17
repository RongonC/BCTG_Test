using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetQuestRelationDataRequest 
    {
        public int EntityId { get; set; }
        public int AreaId { get; set; }
        public int ItemId { get; set; }

        public string User { get; set; }
    }
}