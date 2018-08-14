using StemmonsMobile.iOS;
using System;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Datalayer))]

namespace StemmonsMobile.iOS
{
    class Datalayer : IDatalayer
    {
        //test
        public string GetLocalFilePath(string filename)
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder, filename);
        }

         public string GetLocalfolderpath()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

            if (!Directory.Exists(libFolder))
            {
                Directory.CreateDirectory(libFolder);
            }

            return Path.Combine(libFolder);
        }
    }
}
