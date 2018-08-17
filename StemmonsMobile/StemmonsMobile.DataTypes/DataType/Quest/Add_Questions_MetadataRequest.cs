using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class Add_Questions_MetadataRequest
    {
        public int? pITEM_INSTANCE_TRAN_ID { get; set; }
        public int pITEM_ID { get; set; }
        public string pITEM_QUESTION_FIELD_IDs { get; set; }
        public string pMEETS_STANDARDS { get; set; }
        public string pPOINTS_AVAILABLE { get; set; }
        public string pPOINTS_EARNED { get; set; }
        public string pIS_CASE_REQUESTED { get; set; }
        public double pWEIGHT { get; set; }
        public string pNOTES { get; set; }
        public string pCREATED_BY { get; set; }

    }
}
