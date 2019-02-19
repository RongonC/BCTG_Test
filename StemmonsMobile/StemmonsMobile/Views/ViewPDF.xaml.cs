using StemmonsMobile.Controls;
using StemmonsMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views
{    
    public partial class ViewPDF : ContentPage
    {
        public ViewPDF(string url)
        {
            InitializeComponent();
            this.Title = "PDF Viewer";            
            var localPath = string.Empty;
            if (true)
            {
                var dependency = DependencyService.Get<ILocalFileProvider>();

                if (dependency == null)
                {
                    DisplayAlert("Error loading PDF", "Computer says no", "OK");

                    return;
                }

                var fileName = Guid.NewGuid().ToString();

                // Download PDF locally for viewing
                using (var httpClient = new HttpClient())
                {
                    var pdfStream = Task.Run(() => httpClient.GetByteArrayAsync(url)).Result;

                    localPath =
                        Task.Run(() => dependency.SaveFileToDisk(pdfStream, $"{fileName}.pdf")).Result;
                }

                if (string.IsNullOrWhiteSpace(localPath))
                {
                    DisplayAlert("Error loading PDF", "Computer says no", "OK");

                    return;
                }
            }

            if (Device.RuntimePlatform == Device.Android)
                PdfView.Source = $"file:///android_asset/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(localPath)}";
            else
            {
                PdfView.Source = localPath;
            }
        }
    }
}