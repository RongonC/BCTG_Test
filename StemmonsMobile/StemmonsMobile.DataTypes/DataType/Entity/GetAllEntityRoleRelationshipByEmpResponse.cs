
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetAllEntityRoleRelationshipByEmpResponse : Response
    {
        public object ResponseContent { get; set; }

    }

    public partial class AllEntityRoleRelationshipByEmp
    {

        public int ENTITYID { get; set; }

        public string ENTITYTITLE { get; set; }

        public int ROLEID { get; set; }

        public string ROLENAME { get; set; }

        public int DEPARTMENTENTITYROLEASSIGNID { get; set; }

        public string EMPLOYEE_GUID { get; set; }

        public int DEPARTMENTENTITYTYPEID { get; set; }

        public string ENTITYTYPENAME { get; set; }


    }
}