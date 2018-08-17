using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class AcceptCaseRequest
    {
        public int CaseId { get; set; }
        public string caseOwnerSAM { get; set; }
        public string username { get; set; }
    }
}
