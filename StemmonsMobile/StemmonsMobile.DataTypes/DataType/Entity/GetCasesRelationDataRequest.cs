using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetCasesRelationDataRequest
    {
        public int EntityId { get; set; }
        public int CaseTypeId { get; set; }
        public string User { get; set; }
        public char GetCasesStatus { get; set; }
        public int pageIndex { get; set; }  
        public int pageSize { get; set; }
        public string sortBy { get; set; }
        public string sortOrder { get; set; }
        public string searchXml { get; set; }
        public string getListFor { get; set; }
        
    }
}