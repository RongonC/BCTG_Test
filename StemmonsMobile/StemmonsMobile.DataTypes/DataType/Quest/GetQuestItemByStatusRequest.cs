using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class GetQuestItemByStatusRequest
    {
        public int ItemID { get; set; }
        public string User { get; set; }
        public string StatusType { get; set; }
    }
}
