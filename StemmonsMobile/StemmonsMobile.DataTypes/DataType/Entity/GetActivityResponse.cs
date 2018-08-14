
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetActivityResponse : Response
    {
        public object ResponseContent { get; set; }
        public int ENTITY_ACTIVITY_ID { get; set; }
        public int ENTITY_ID { get; set; }
        public int ENTITY_ACTIVITY_TYPE_ID { get; set; }
        public string NOTE { get; set; }
        public Char IS_ACTIVE { get; set; }
        public DateTime CREATED_DATETIME { get; set; }
        public String CREATED_BY { get; set; }
        public DateTime MODIFIED_DATETIME { get; set; }
        public String MODIFIED_BY { get; set; }
        public String NAME { get; set; }
        public String DESCRIPTION { get; set; }
        public String BCOLOR { get; set; }
        public String FCOLOR { get; set; }
        public String SYSTEM_CODE { get; set; }

    }
}