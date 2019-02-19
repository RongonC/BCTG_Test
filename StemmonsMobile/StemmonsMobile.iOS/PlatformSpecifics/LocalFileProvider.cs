using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using StemmonsMobile.Interfaces;
using StemmonsMobile.iOS.PlatformSpecifics;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalFileProvider))]
namespace StemmonsMobile.iOS.PlatformSpecifics
{
    public class LocalFileProvider : ILocalFileProvider
    {
        private readonly string _rootDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pdfjs");

        //private readonly string _rootDir = Path.Combine(NSBundle.MainBundle.BundlePath);

        string ss = "";

        public async Task<string> SaveFileToDisk(byte[] stream, string fileName)
        {
            try
            {
                if (!Directory.Exists(_rootDir))
                    Directory.CreateDirectory(_rootDir);

                var filePath = Path.Combine(_rootDir, fileName);

                File.WriteAllBytes(filePath, stream);

                return filePath;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        public async Task<string> ReadWriteTxtFile(byte[] txtBytes, string fileName)
        {
            try
            {
                if (!Directory.Exists(_rootDir))
                    Directory.CreateDirectory(_rootDir);

                var filePath = Path.Combine(_rootDir, fileName);

                File.WriteAllBytes(filePath, txtBytes);

                string fileText = File.ReadAllText(filePath);

                return fileText;

            }
            catch (Exception ex)
            {
                string exStr = ex.ToString();
                return null;
            }
        }
    }
}