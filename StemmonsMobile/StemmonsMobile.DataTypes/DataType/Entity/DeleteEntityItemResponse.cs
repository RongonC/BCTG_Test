﻿
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class DeleteEntityItemResponse : Response
    {
        public object ResponseContent { get; set; }
        public int result { get; set; }
    }
}