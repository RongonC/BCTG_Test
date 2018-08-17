
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class CreateNewEntityItemRequest

    {
        public string  User{ get; set; }
        public EntityClass entityTypeSchema { get; set; }


    }
}