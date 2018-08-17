
using System;
using System.Collections.Generic;
using System.Linq;



namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseLastAssigneeResponse:Response
    {
        public object ResponseContent { get; set; }

        //[Serializable]
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
        }
    }
}