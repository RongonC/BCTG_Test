using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Departments
{
    public class EntityRoleAssignment
    {
        public int EntityRoleAssignID { get; set; }
        public string EmployeeGUID { get; set; }
        public int EntityTypeID { get; set; }
        public int EntityID { get; set; }
        public string EntityTitle { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string Operation { get; set; }
        public string IsPrimary { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Comments { get; set; }
        public char? ControlType { get; set; }
        public string Primary { get; set; }
        public string Remove { get; set; }
    }
}
