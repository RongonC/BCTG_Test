using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseIDByCaseTypeIDAndListIDRequest
    {
        public int caseTypeId { get; set; }
        public int listID { get; set; }
    }
}