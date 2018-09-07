using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class EntityBasicDetailsRequest
    {
        public int Entity_id { get; set; }
        public string User_Name { get; set; }
    }


    public class EntityBasicDetails
    {
        public string ENTITY_TYPE_ID { get; set; }
        public string ENTITY_TYPE_NAME { get; set; }
    }
}
