using StemmonsMobile.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(Datalayer))]

namespace StemmonsMobile.UWP
{
   public class Datalayer :IDatalayer
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path, filename);
        }

        public string GetLocalfolderpath()
        {
            return Path.Combine(ApplicationData.Current.LocalFolder.Path);
        }


    }
}
