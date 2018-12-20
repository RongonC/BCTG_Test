
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCasesForUserResponse : Response
    {
        public object ResponseContent { get; set; }

    }
    public class CaseListSP
    {
        public int LIST_CASE_ID { get; set; }
        public string List_ID { get; set; }
        public int LIST_CASE_TYPE_ID { get; set; }
        public string LIST_CASE_TYPE_NAME { get; set; }
        public string LIST_CASE_TITLE { get; set; }
        public string LIST_CASE_DUE { get; set; }
        public string LIST_CASE_STATUS_VALUE { get; set; }
        public string LIST_CASE_LIFE_D_H_M { get; set; }
        public System.DateTime LIST_CASE_OWNER_DATETIME { get; set; }
        public string LIST_CASE_OWNER_DISPLAY_NAME { get; set; }
        public System.DateTime LIST_CASE_ASSGN_DATETIME { get; set; }
        public string LIST_CASE_ASSGN_TO_DISPLAY_NAME { get; set; }
        public System.DateTime LIST_CASE_CREATED_DATETIME { get; set; }
        public string LIST_CASE_CREATED_BY_DISPLAY_NAME { get; set; }
        public System.DateTime LIST_CASE_MODIFIED_DATETIME { get; set; }
        public string LIST_CASE_MODIFIED_BY_DISPLAY_NAME { get; set; }
        public string LIST_CASE_PRIORITY_VALUE { get; set; }
        public int Total_Case { get; set; }

        public string LIST_CASE_OWNER_SAM { get; set; }
        public string LIST_CASE_ASSGN_TO_SAM { get; set; }
        public string LIST_CASE_CREATED_BY_SAM { get; set; }
        public string LIST_CASE_MODIFIED_BY_SAM { get; set; }
        public char NEWEST_NOTES_ON_TOP { get; set; }
    }

}