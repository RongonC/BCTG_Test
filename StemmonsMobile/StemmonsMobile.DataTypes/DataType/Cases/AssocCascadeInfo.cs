using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class AssocCascadeInfo
    {

        public  int _CASE_ASSOC_TYPE_CASCADE_ID { get; set; }

        public  int _CASE_ASSOC_TYPE_ID_PARENT { get; set; }

        public int _CASE_ASSOC_TYPE_ID_CHILD { get; set; }

        public string _PARENT_EXTERNAL_DATASOURCE_IS_ACTIVE { get; set; }

        public string _CHILD_EXTERNAL_DATASOURCE_IS_ACTIVE { get; set; }
    }
}