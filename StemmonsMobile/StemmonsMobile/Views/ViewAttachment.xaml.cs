using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAttachment : ContentPage
    {
        public static string currImgURL = "";
        string fileExt = string.Empty;
        string AppType = string.Empty;
        string EntityID = string.Empty;
        //Stopwatch stopwatch = new Stopwatch();

        public ViewAttachment(string imgurl, string appType)
        {
            InitializeComponent();
            currImgURL = imgurl;
            BackgroundColor = Color.LightGray;
            AppType = appType;

        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                string fileStr = string.Empty;

                int fileIDMarker = currImgURL.IndexOf('=');
                string FileID = currImgURL.Substring(fileIDMarker + 1, 4);

                if (AppType.Equals("Entities"))
                {
                    int entityIDMarker = currImgURL.LastIndexOf('=');
                    EntityID = currImgURL.Substring(entityIDMarker + 1);
                }

                string userName = Functions.UserName;

                if (AppType.Equals("Cases"))
                {
                    await Task.Run(() =>
                    {
                        var d = CasesAPIMethods.GetFileFromCase(FileID, userName);
                        fileStr = d.GetValue("ResponseContent").ToString();
                    });
                }
                else if (AppType.Equals("Entities"))
                {
                    await Task.Run(() =>
                    {
                        var d = EntityAPIMethods.GetFileFromEntity(EntityID, FileID, userName);
                        fileStr = d.GetValue("ResponseContent").ToString();
                    });
                }

                FileItem fileResp = JsonConvert.DeserializeObject<FileItem>(fileStr);

                byte[] fileData = fileResp.BLOB; //File byte array

                try
                {
                    fileExt = fileResp.FileName.Substring(fileResp.FileName.LastIndexOf('.'));
                }
                catch (Exception)
                {

                }

                switch (fileExt)
                {
                    case ".pdf":
                        /*PdfView should be Visible and other Control will be False*/
                        PdfView.IsVisible = true;
                        imgStackLayout.IsVisible = false;
                        txtScrollView.IsVisible = false;

                        OpenPDFViewer(fileData);
                        break;

                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                        /*imgStackLayout should be Visible and other Control will be False*/
                        PdfView.IsVisible = false;
                        imgStackLayout.IsVisible = true;
                        txtScrollView.IsVisible = false;
                        OpenImage(fileData);
                        break;

                    case ".txt":
                        /*txtScrollView should be Visible and other Control will be False*/
                        PdfView.IsVisible = false;
                        imgStackLayout.IsVisible = false;
                        txtScrollView.IsVisible = true;
                        OpenTxt(fileData);
                        break;

                    //case ".mp4":
                    //case ".mkv":
                    //case ".vod":
                    //case ".avi":
                    //case ".flv":
                    //case ".mpeg":
                    //case ".wmv":
                    //VideoPlayer.IsVisible = true;
                    //OpenVideo(fileData);
                    //break;

                    default:
                        //Preview not available
                        //DisplayAlert("File cannot be viewed","File format not supported","OK");
                        Device.OpenUri(new Uri(currImgURL));
                        break;

                }

                //string fileName = fileResp.FileName.Substring(0, fileResp.FileName.LastIndexOf('.'));
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

            }
            catch (Exception ex)
            {

            }
        }



        private void OpenPDFViewer(byte[] fileBytes)
        {
            var localPath = string.Empty;

            var dependency = DependencyService.Get<ILocalFileProvider>();

            if (dependency == null)
            {
                DisplayAlert("Error loading PDF", "Computer says no", "OK");

                return;
            }

            var fileName = Guid.NewGuid().ToString();

            //var pdfStream = Task.Run(() => httpClient.GetByteArrayAsync(url)).Result;

            localPath =
                Task.Run(() => dependency.SaveFileToDisk(fileBytes, $"{fileName}.pdf")).Result;

            if (string.IsNullOrWhiteSpace(localPath))
            {
                DisplayAlert("Error loading PDF", "Computer says no", "OK");

                return;
            }

            if (Device.RuntimePlatform == Device.Android)
                PdfView.Source = $"file:///android_asset/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(localPath)}";
            else
            {
                PdfView.Source = localPath;
            }
        }

        private void OpenImage(byte[] imageBytes)
        {
            img.VerticalOptions = LayoutOptions.Center;
            img.HorizontalOptions = LayoutOptions.Center;
            img.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }

        private void OpenTxt(byte[] fileBytes)
        {
            var fileContents = string.Empty;

            var dependency = DependencyService.Get<ILocalFileProvider>();

            if (dependency == null)
            {
                DisplayAlert("Error loading file", "Computer says no", "OK");
                return;
            }

            var fileName = Guid.NewGuid().ToString();

            fileContents =
                Task.Run(() => dependency.ReadWriteTxtFile(fileBytes, $"{fileName}.txt")).Result;

            if (string.IsNullOrWhiteSpace(fileContents))
            {
                DisplayAlert("No data found", "Error", "OK");

                return;
            }

            txtDataLabel.Text = fileContents;
        }

        #region Video
        //async private void OpenVideo(byte[] fileBytes)
        //{
        //    try
        //    {
        //        var localPath = string.Empty;

        //        var dependency = DependencyService.Get<ILocalFileProvider>();

        //        if (dependency == null)
        //        {
        //            DisplayAlert("Error loading file", "Computer says no", "OK");

        //            return;
        //        }

        //        var fileName = Guid.NewGuid().ToString();

        //        //var pdfStream = Task.Run(() => httpClient.GetByteArrayAsync(url)).Result;

        //        localPath =
        //            Task.Run(() => dependency.SaveFileToDisk(fileBytes, $"{fileName}.mp4")).Result;

        //        if (string.IsNullOrWhiteSpace(localPath))
        //        {
        //            DisplayAlert("No data found", "Error", "OK");

        //            return;
        //        }

        //        await Navigation.PushAsync(new PlayVideoResourcePage(localPath));
        //        //VideoPlayer.Source = VideoSource.FromFile(localPath);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }


        //} 
        #endregion
    }
}