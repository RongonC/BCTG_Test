using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    #region ExternalDatasource
    public class ExternalDatasourceInfo
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string Query { get; set; }
        public string ObjectId { get; set; }
        public string ObjectDisplay { get; set; }
        public string Description { get; set; }
        public string SystemCode { get; set; }
        public bool IsActive { get; set; }
        public int Entity_Type_ID { get; set; }
    }
    #endregion
}
