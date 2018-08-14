using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class LogRequest
    {
        public string txt { get; set; }
        public string isSuccess { get; set; }
        public string appName { get; set; }
        public string CreatedBy { get; set; }
        public string createdDateTime { get; set; }
    }
}
