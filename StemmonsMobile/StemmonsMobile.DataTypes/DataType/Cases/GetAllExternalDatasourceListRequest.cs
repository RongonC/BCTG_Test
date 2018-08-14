using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetAllExternalDatasourceListRequest
    {
        public int CaseTypeID { get; set; }
        public int AssocTypeID { get; set; }
        public string User { get; set; }
        public string ListType { get; set; }
    }
}