using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseModifiedDateRequest
    {
        public int caseID { get; set; }
        public DateTime? LastSyncModifiedTime { get; set; }

        public int? CaseTypeID { get; set; }

    }
}
