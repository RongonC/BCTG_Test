using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetPrevousCaseAssignmentResponse : Response
    {
        //public List<CaseType> ListCaseTypes { get; set; }
        public object ResponseContent { get; set; }
        public class UserInfo
        {
            public int ID { get; set; }

            public string MiddleName { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string PrimaryJobTitle { get; set; }

            public string CellPhone { get; set; }

            public string Department { get; set; }

            public string OfficePhone { get; set; }

            public string Email { get; set; }

            public string City { get; set; }

            public string State { get; set; }

            public string DisplayName { get; set; }

            public string UserID { get; set; }

            public string JobTitle { get; set; }
            public int Entity_Type_ID { get; set; }
            public string Entity_Type_Name { get; set; }
            public int Entity_ID { get; set; }
            public string Entity_Title { get; set; }
            public int RoleId { get; set; }
            public string RoleName { get; set; }
            public string ShortName { get; set; }
            public char? IsExternalUser { get; set; }

        }
    }
}