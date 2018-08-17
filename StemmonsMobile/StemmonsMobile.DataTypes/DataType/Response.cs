using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.DataTypes.DataType
{

    public class Response
    {
        public bool Success = true;                         // Success or Falure, default is success
        public string Message = "Success!";                 // Sucess or Falure Response Message, this can be cusotmized to display a friendly message to the user
        public string StackTrace = "";                      // Display the StackTrace
        public const string Version = "1.0";                // This is the static version number, for future versions instead of modifying this class for enhancments, please add a new version.
        public DateTime RequestDateTime = DateTime.Now;     // Used to store on the device that the last timestamp was this time for this type of request
        public Guid CorrelationID = Guid.NewGuid();         // Used for writing to the database log all of the requests.
        public double TotalResponseTimeMS
        {
            get
            {
                ResponseStopWatch.Stop();
                TimeSpan ts = ResponseStopWatch.Elapsed;
                return ts.TotalMilliseconds;
            }
        }

        private Stopwatch ResponseStopWatch = new Stopwatch();

        public Response()
        {
            this.ResponseStopWatch.Start();
        }

        public void Fail()
        {
            Success = false;
            Message = "Falure!";
            StackTrace = "";
        }

        public void Fail(string message)
        {
            Success = false;
            Message = message;
            StackTrace = "";
        }

        public void Fail(string message, string stackTrace)
        {
            Success = false;
            Message = message;
            StackTrace = stackTrace;
        }
    }

}
