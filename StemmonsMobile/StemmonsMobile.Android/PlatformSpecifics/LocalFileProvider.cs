using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StemmonsMobile.Droid.PlatformSpecifics;
using StemmonsMobile.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalFileProvider))]
namespace StemmonsMobile.Droid.PlatformSpecifics
{
    public class LocalFileProvider : ILocalFileProvider
    {
        private readonly string _rootDir = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, "pdfjs");

        public async Task<string> SaveFileToDisk(byte[] pdfBytes, string fileName)
        {
            try
            {
                if (!Directory.Exists(_rootDir))
                    Directory.CreateDirectory(_rootDir);

                var filePath = Path.Combine(_rootDir, fileName);

                File.WriteAllBytes(filePath, pdfBytes);

                string contents = File.ReadAllText(filePath);

                return filePath;

                //using (var memoryStream = new MemoryStream())
                //{
                //    await pdfStream.CopyToAsync(memoryStream);
                //    File.WriteAllBytes(filePath, memoryStream.ToArray());
                //}
            }
            catch (Exception ex)
            {
                string exStr = ex.ToString();
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