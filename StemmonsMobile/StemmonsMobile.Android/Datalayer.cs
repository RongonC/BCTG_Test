using System;
using System.IO;
using Xamarin.Forms;
using StemmonsMobile.Droid;

[assembly: Dependency(typeof(Datalayer))]
namespace StemmonsMobile.Droid
{
    class Datalayer : IDatalayer
    {
        
        public string GetLocalFilePath(string filename)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }

        public string GetLocalfolderpath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            return Path.Combine(path);
        }
    }
}
