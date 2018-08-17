

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class UserNameLookupResponse : Response
    {
        public object ResponseContent { get; set; }
        public List<EmpList> result { get; set; }
       
        public string ENTITY_USER_CACHE_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
        public string DISPLAY_NAME { get; set; }
        public string USERNAME { get; set; }
        public string   EMPLOYEE_GUID { get; set; }
    }
}