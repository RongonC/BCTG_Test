using System;
using System.Collections.Generic;
using System.Linq;


namespace StemmonsMobile.DataTypes.DataType.Cases
{
    public class SendEmailAlertRequest
    {
        #region SendEmailAlertModel
        public class SendEmailAlert
        {
            public string recipent { get; set; }
            public string subject { get; set; }
            public string body { get; set; }
        }
        #endregion
    }
}