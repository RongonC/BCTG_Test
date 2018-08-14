using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Quest
{
    public class UpdateCaseNotesToFormRequest
    {
        public int itemInstanceTranCaseId { get; set; }
        public string notes { get; set; }
        public string modifiedBy { get; set; }
    }
}