using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class AssignCaseRequest 
    {
        public int CaseId { get; set; }
        public string newCaseOwner { get; set; }
        public string username { get; set; }
    }
}