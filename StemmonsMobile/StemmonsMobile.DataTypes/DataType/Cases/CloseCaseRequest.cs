using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class CloseCaseRequest
    {
        public int caseID { get; set; }
        public string user { get; set; }
        public string userFullName { get; set; }
    }
}