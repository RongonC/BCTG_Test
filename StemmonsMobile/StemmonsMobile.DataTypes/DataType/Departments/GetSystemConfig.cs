using System;
using System.Collections.Generic;
using System.Text;

namespace StemmonsMobile.DataTypes.DataType.Departments
{
    public class GetSystemConfig
    {
        public int CONFIG_SYSTEM_ID { get; set; }
        public string SYSTEM_CODE { get; set; }
        public string NAME { get; set; }
        public string VALUE { get; set; }
        public DateTime? CREATED_DATETIME { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? MODIFIED_DATETIME { get; set; }
        public string MODIFIED_BY { get; set; }
    }
}
