using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Default
{
    public class HomeScreenCountRequest
    {
        public string username { get; set; }
    }


    public class GetImageURLRequest
    {
        public string System_Code { get; set; }
    }
    public class  MobileBranding
    {
        public string SYSTEM_CODE { get; set; }
        public string KEY { get; set; }
        public string VALUE { get; set; }
        public string IS_ACTIVE { get; set; }
        public string CREATED_DATETIME { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_DATETIME { get; set; }
        public string MODIFIED_BY { get; set; }
    }
}
