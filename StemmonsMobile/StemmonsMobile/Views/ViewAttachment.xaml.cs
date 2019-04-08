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
        string FileID = string.Empty;
        byte[] fileData;
        bool isFirstAppearing = true;

        //Stopwatch stopwatch = new Stopwatch();

        public ViewAttachment(string imgurl, string appType)
        {
            InitializeComponent();
            currImgURL = imgurl.ToLower();
            BackgroundColor = Color.LightGray;
            AppType = appType;

        }

        async protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                if (isFirstAppearing)
                {
                    isFirstAppearing = false;
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    string FileID = "0";
                    string EntityID = "0";

                    if (AppType.Equals("Entities"))
                    {
                        //http://home-s-21.boxerproperty.com/ENTITIES/Download.aspx?FileID=1129&EntityId=2326
                        var eqsplit = currImgURL.Split(new string[] { "?fileid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                FileID += ite;
                            }
                            else
                            {
                                break;
                            }
                        }

                        eqsplit = currImgURL.Split(new string[] { "&entityid=" }, StringSplitOptions.None);
                        foreach (var ite in eqsplit[1].ToCharArray())
                        {
                            if (char.IsDigit(ite))
                            {
                                EntityID += ite;
                            }
                            else
                            {
                                break;
                            }
                        }


                        await Task.Run(() =>
                        {
                            var d = EntityAPIMethods.GetFileFromEntity(EntityID, this.FileID, Functions.UserName);
                            FileID = d.GetValue("ResponseContent").ToString();
                        });
                    }
                    else if (AppType.Equals("Cases"))
                    {
                        //http://cases.boxerproperty.com/DownloadFile.aspx?CaseFileID=1453439
                        var eqsplit = currImgURL.Split(new string[] { "?casefileid=" }, StringSplitOptions.None);
                        this.FileID = eqsplit[1];

                        await Task.Run(() =>
                        {
                            var d = CasesAPIMethods.GetFileFromCase(this.FileID, Functions.UserName);
                            FileID = d.GetValue("ResponseContent").ToString();
                        });
                    }

                    if (FileID != null && FileID != "")
                    {
                        FileItem fileResp = JsonConvert.DeserializeObject<FileItem>(FileID);
                        byte[] fileData = fileResp.BLOB; //File byte array
                        try
                        {
                            fileExt = fileResp.FileName.Substring(fileResp.FileName.LastIndexOf('.'));
                        }
                        catch (Exception)
                        {

                        }

                    }
                    else
                    {
                        await DisplayAlert("Alert", "Unable to view attachment", "OK");

                    }
                    switch (fileExt)
                    {
                        case ".pdf":
                            txtScrollView.IsVisible = false;
                            imgStackLayout.IsVisible = false;
                            //VideoPlayer.IsVisible = false;
                            OpenPDFViewer(fileData);
                            break;

                        case ".jpg":
                        case ".jpeg":
                        case ".png":
                        case ".gif":

                            PdfView.IsVisible = false;
                            txtScrollView.IsVisible = false;
                            imgStackLayout.IsVisible = true;
                            OpenImage(fileData);
                            break;

                        case ".txt":
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
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
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



            //img.Source = new UriImageSource
            //{
            //    Uri = new Uri(currImgURL),
            //    CachingEnabled = true,
            //    CacheValidity = new TimeSpan(5, 0, 0, 0)

            //};
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

            //var pdfStream = Task.Run(() => httpClient.GetByteArrayAsync(url)).Result;

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