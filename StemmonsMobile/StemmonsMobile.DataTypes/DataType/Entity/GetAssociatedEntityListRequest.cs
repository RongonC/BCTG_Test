
using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class GetAssociatedEntityListRequest

    {

        public String USER_SAM { get; set; }
        public List<KeyValuePair<string , string>> list { get; set; }



    }
}