


using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace StemmonsMobile.DataTypes.DataType.Standards
{
    public class Security_Department_GetJobFunctionResponse : Response
    {
        public object ResponseContent { get; set; }

        public List<JobFunction> result { get; set; }


    }
}