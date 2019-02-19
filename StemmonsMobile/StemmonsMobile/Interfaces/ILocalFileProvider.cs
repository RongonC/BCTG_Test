using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StemmonsMobile.Interfaces
{
    public interface ILocalFileProvider
    {
        Task<string> SaveFileToDisk(byte[] bytes, string fileName);

        Task<string> ReadWriteTxtFile(byte[] bytes, string fileName);


    }
}
