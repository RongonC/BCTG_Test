﻿

using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class WS_GetEntityTypeListResponse : Response
    {
        public object ResponseContent { get; set; }
        public List<EntityTypeList> result { get; set; }
    }
}