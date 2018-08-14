using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseListRequest
    {
        public string caseTypeId { get; set; }
        public string userName { get; set; }
        public string caseOwnerSAM { get; set; }
        public string caseAssgnSAM { get; set; }
        public string caseCloseBySAM { get; set; }
        public string caseCreateBySAM { get; set; }
        public string propertyId { get; set; }
        public int? tenant_Id { get; set; }
        public string tenant_Code { get; set; }
        public char? showOpenClosedCasesType { get; set; }
        public string searchQuery { get; set; }
        public string ScreenName { get; set; }
        public char? showPastDueCases { get; set; }
        public List<KeyValuePair<string, string>> keyValuePairs { get; set; }

    }
}