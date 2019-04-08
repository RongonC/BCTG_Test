using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DataServiceBus.OfflineHelper.DataTypes
{
    public class SyncStatus
    {
        public string APIName { get; set; }
        public string FailDesc { get; set; }
        public ImageSource ImageName
        {
            get
            {
                return ApiCallSuccess ? ImageSource.FromFile("Assets/tick_2.png") : ImageSource.FromFile("Assets/cross_2.png");
                //Assets/tick_2.png
                //Assets/cross_2.png
            }
        }

        private bool _apicallsuccess;

        public bool ApiCallSuccess
        {
            get { return _apicallsuccess; }
            set { _apicallsuccess = value; }
        }
    }
}
