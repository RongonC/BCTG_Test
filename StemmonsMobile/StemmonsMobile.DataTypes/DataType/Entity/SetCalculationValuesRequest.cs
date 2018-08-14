
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class SetCalculationValuesRequest

    {

        public int ENTITY_ASSOC_TYPE_ID { get; set; }
        public int ENTITY_ID { get; set; }
        public string TEXT { get; set; }
        public string CALCULATION_ERROR { get; set; }
        public string CALCULATION_EQUATION { get; set; }

    }
}