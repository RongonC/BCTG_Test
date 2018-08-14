using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OfflineHelper.DataTypes.Entity
{
    public class ConstantsSync
    {
        public static string Baseurl = ConnectionAPI.APIURL.ToString();

        #region Entity Offline API Lists

        public static string GetEntityTypeList = "/api/v1/Synchronize/FillEntityOfflineData";
        #endregion
    }
}
