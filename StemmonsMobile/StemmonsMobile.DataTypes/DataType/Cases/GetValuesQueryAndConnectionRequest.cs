using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetValuesQueryAndConnectionRequest
    {
        
        public int caseTypeID { get; set; }
        public int assocCaseTypeID { get; set; }
        public string caseTypeDesc { get; set; }
        public bool isRequired { get; set; }
        public string connectionString { get; set; }
        public string query { get; set; }
       
    }
}
