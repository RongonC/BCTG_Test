using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Cases
{
    public class ConstantsSync
    {
        public static string Baseurl = ConnectionAPI.APIURL.ToString();

        #region Cases Offline API Lists

        public static string GetAllCaseTypeWithID = "/api/v1/Synchronize/GetAllCaseTypeWithID";
        #endregion
    }
}
