using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class AddCasetoFormRequest
    {
        
        public int itemInstanceTranID { get; set; }
        public string notes { get; set; }
        public string createdBy { get; set; }
    }
}