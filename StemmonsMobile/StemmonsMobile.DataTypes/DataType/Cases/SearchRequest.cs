using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class SearchRequest
    {
        public int? SystemId { get; set; }
        public int? TypeId { get; set; }
        public  int? FieldId { get; set; }
        public int? ItemInfoFieldsId { get; set; }
        public string SearchText { get; set; }
        public int? FromPageIndex { get; set; }
        public int? ToPageIndex { get; set; }
        public string username { get; set; }
    }
}
