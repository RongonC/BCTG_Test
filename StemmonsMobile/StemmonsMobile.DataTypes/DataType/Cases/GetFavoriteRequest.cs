using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetFavoriteRequest
    {
        public string CreatedBy { get; set; }
        public int ApplicationId { get; set; }
        public int AreaId { get; set; }
        public int TypeId { get; set; }

    }
}
