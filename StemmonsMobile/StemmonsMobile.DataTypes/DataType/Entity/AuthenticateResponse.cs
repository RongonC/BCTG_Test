
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Entity
{
    public class AuthenticateResponse : Response
    {
        public object ResponseContent { get; set; }
        public string result { get; set; }
    }
}