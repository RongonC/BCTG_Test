using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class RefreshCalculationFieldsRequest
    {
        public string assocFieldCollection { get; set; }
        public string calculatedAssocId { get; set; }
        public Dictionary<string, string> assocFieldTexts { get; set; }
        public Dictionary<string, string> assocFieldValues { get; set; }
        public string sdateFormats { get; set; }

    }
}
