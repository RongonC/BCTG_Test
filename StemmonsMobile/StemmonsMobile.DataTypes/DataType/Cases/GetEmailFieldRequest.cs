using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetEmailFieldRequest
    {
        public string caseTypeId { get; set; }
        public string emailAddress { get; set; }
    }
}