using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class ApproveCaseRequest
    {
        public int CaseId { get; set; }
        public string username { get; set; }
        public string newCaseOwner { get; set; }
        public string caseNote { get; set; }
    }
}
