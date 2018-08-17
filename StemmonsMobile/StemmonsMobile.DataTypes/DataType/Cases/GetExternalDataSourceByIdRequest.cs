using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetExternalDataSourceByIdRequest
    {
        public int ExternalDatasourceID { get; set; }
        public string SystemCode { get; set; }
    }
}
