using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Standards
{
    public class ConstantsSync
    {
        public static string Baseurl = ConnectionAPI.APIURL.ToString();

        #region Standards Offline API Lists

        public static string GetAllStandards = "/api/v1/Synchronize/GetAllStandards";
        #endregion
    }
}
