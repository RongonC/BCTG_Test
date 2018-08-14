using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class GetCaseActivityResponse : Response
    {
        public object ResponseContent { get; set; }

        public class Activity
        {
            public int ActivityTypeID { get; set; }

            public string BackgroundColor { get; set; }

            public int CaseActivityID { get; set; }

            public string CaseID { get; set; }

            public string CaseTypeID { get; set; }

            public string CreatedByUID { get; set; }

            public DateTime CreatedDateTime { get; set; }

            public string Description { get; set; }

            public string ForgroundColor { get; set; }

            public string IsActive { get; set; }

            public string ModifiedBy { get; set; }

            public DateTime ModifiedDateTime { get; set; }

            public string Name { get; set; }

            public string Note { get; set; }

            public string SystemCode { get; set; }

            public string CreatedByFullName { get; set; }

        }
    }
}
