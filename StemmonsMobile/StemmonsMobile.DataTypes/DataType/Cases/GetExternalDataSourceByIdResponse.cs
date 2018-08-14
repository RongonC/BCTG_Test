using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetExternalDataSourceByIdResponse : Response
    {
        public object ResponseContent { get; set; }
        public class ExternalDatasource
        {
            public int? ID { get; set; }
            public string NAME { get; set; }
            public string DESCRIPTION { get; set; }

        }
    }
}
