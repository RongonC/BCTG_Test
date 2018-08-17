using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile
{
    public interface IDatalayer
    {
        string GetLocalFilePath(string filename);

        string GetLocalfolderpath();

    }
}
