using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCasesForUserRequest
    {
        public string user { get; set; }
        public string assignedTo { get; set; }
        public string caseownersam { get; set; }
        public string caseclosedbysam { get; set; }
        public string casecreatedbysam { get; set; }
        public char showopenclosedcasestype { get; set; }
        public string caseIds { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string searchXml { get; set; }
        public string SystemCodes { get; set; }

        public List<KeyValuePair<string, string>> keyValuePairs { get; set; }

    }
}
