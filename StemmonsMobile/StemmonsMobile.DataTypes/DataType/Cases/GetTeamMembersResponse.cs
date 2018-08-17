using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetTeamMembersResponse : Response
    {
        public object ResponseContent { get; set; }

        public class TeamMember
        {
            public int EmployeeId { get; set; }
            public Guid? EmployeeGuid { get; set; }
            public string DisplayName { get; set; }
            public string EmailId { get; set; }
            public string SAMName { get; set; }

            public string JobTitle { get; set; }
            public string DepartmentName  { get; set; }
        }
    }
}
