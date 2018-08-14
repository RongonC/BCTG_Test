using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceBus
{
    public interface IDataHelper
    {
        string GetLocalFilePath(string filename);

        string GetLocalfolderpath();


    }
}
