


using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class Security_Role_GetRolesByEntityTypeResponse : Response
    {
        public object ResponseContent { get; set; }

        public List<EntityRole> result { get; set; }


    }
}