using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseNotesRequest
    { 
        public int CaseID { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}