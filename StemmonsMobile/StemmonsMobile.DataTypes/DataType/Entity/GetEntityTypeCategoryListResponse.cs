
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetEntityTypeCategoryListResponse : Response
    {


        public object ResponseContent { get; set; }
        public List<EntityTypeCategory> Result { get; set; }
     

      

    }
}