using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus.OnlineHelper.DataTypes
{
    public class RequestToken
    {

        public string grant_type { get; set; } = "password";
        public string username { get; set; } = "api_admin";
        public string password { get; set; } = "Boxer@123";

    }
}
