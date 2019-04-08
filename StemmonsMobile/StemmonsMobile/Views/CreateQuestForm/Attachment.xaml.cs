using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using Plugin.Media;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using StemmonsMobile.Views.LoginProcess;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Attachment : ContentPage
    {
        string Iteminstancetranid, questionfieldId, metadtaid;
        string FileName = string.Empty;
        public Attachment(string ItemQuestionFieldId, string ItemInstanceTranID, string itemQuestionMetadataId)
        {
            InitializeComponent();
			try{
            metadtaid = itemQuestionMetadataId;
            Iteminstancetranid = ItemInstanceTranID;
            questionfieldId = ItemQuestionFieldId;
            List<AttachmentDetail> lst = new List<AttachmentDetail>();

			}
			catch(Exception ex){
				
			}

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            try
            {
                AttachmentdataCall(metadtaid);
            }
            catch (Exception ex)
            {

            }
        }

		void AttachmentdataCall(string metadataid)
		{
			try
			{
				if (metadataid == "0")
				{
					metadtaid = "-1";
				}
				Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
				var AttachmentCall = QuestSyncAPIMethods.GetFilesByQuestionID(App.Isonline, metadtaid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
				listdata.ItemsSource = null;
				listdata.ItemsSource = AttachmentCall.Result;
				Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
			}
			catch(Exception ex){
				
			}
		}

        async void AddAttachmentClicked(object sender, System.EventArgs e)
        {
            try
            {
                var action = await this.DisplayActionSheet(null, "Cancel", null, "From Photo Gallery", "Take Photo");
                switch (action)
                {
                    case "From Photo Gallery":
                        OpenGallery();
                        break;

                    case "Take Photo":
                        TakePhoto();
                        break;

                }
            }
            catch (Exception ex)
            {
                
            }
        }
        static string ReturnFileType(string extensionName)
        {
			
            string FileType = string.Empty;
            if (extensionName == "pdf")
                FileType = "application/" + extensionName;
            else if (extensionName == "vsdx")
                FileType = "application/" + extensionName;
            else if (extensionName.ToLower().Trim() == "jpeg" || extensionName.ToLower().Trim() == "jpg" || extensionName.ToLower().Trim() == "gif" || extensionName.ToLower().Trim() == "png" || extensionName.ToLower().Trim() == "bpm" || extensionName.ToLower().Trim() == "tiff")
                FileType = "image/" + extensionName;
            else
                FileType = "application/" + extensionName;
            return FileType;
        }
        async void OpenGallery()
        {
            try
            {
                int response = 0;
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("No Gallery", ":( No Gallery available.", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions());

                if (file == null)
                {
                    return;
                }

                //  await DisplayAlert("File Location", file.Path, "OK");



                long size = file.Path.Length;
                byte[] fileBytes = null;
                var bytesStream = file.GetStream();
                var filetype = file.GetType().ToString();

                string name = file.Path.ToLower();
                int endIndex = name.LastIndexOf('.');
                string extensionName = name.Remove(0, endIndex + 1);
                FileName = System.IO.Path.GetFileName(file.Path);

                using (var memoryStream = new MemoryStream())
                {
                    bytesStream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                //byte[] fileBinary = null;
                AddFiletoQuestionRequest addfile = new AddFiletoQuestionRequest();

                addfile.itemInstanceTranID = Convert.ToInt32(Iteminstancetranid);
                addfile.itemQuestionFieldId = Convert.ToInt32(questionfieldId);

                addfile.fileType = ReturnFileType(extensionName);
                addfile.fileName = FileName;
                addfile.fileSizeBytes = Convert.ToInt32(fileBytes.Count());
                addfile.fileBinary = fileBytes;
                addfile.createdBy = Functions.UserName;
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(async () =>
                {
                    //var AttachmentAPi = QuestAPIMethods.AddFiletoQuestion(Convert.ToInt32(Iteminstancetranid), Convert.ToInt32(questionfieldId), ".JPG", "test1", Convert.ToString(size), fileBytes, Functions.UserName);
                    //var response = AttachmentAPi.GetValue("ResponseContent");
                    var AttachmentAPi = QuestSyncAPIMethods.StoreAndCreateFiletoQuestion(App.Isonline, int.Parse(Iteminstancetranid), addfile, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    response = AttachmentAPi.Result;

                });
                Functions.ShowOverlayView_Grid(overlay, false,   masterGrid);
                if (response > 0)
                {
                    await DisplayAlert("Attachment", "Photo attached Successfully.", "Ok");
                }
                else
                {
                    await DisplayAlert("Attachment", "Photo not attached.", "Ok");
                }


                AttachmentdataCall(metadtaid);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
			try{
            ListView l = (ListView)sender;
            var sd = (GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel)l.SelectedItem;

            this.Navigation.PushAsync(new ViewAttachment(Convert.ToString(sd.intFileID)));
			}
			catch(Exception ex){
				
			}
        }

        private void btn_home_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private async void btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);
            }
            catch (Exception)
            {
            }
        }

        async void TakePhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = Iteminstancetranid + "_" + DateTime.Now.ToString()
                });

                var fname = Iteminstancetranid + "_" + DateTime.Now.ToString();
                if (file == null)
                    return;

               // await DisplayAlert("File Location", file.Path, "OK");

                long size = file.Path.Length;
                byte[] fileBytes = null;
                var bytesStream = file.GetStream();
                using (var memoryStream = new MemoryStream())
                {
                    bytesStream.CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                byte[] fileBinary = null;

                string name = file.Path.ToLower();
                int endIndex = name.LastIndexOf('.');
                string extensionName = name.Remove(0, endIndex + 1);
                FileName = System.IO.Path.GetFileName(file.Path);

                AddFiletoQuestionRequest addfile = new AddFiletoQuestionRequest();

                addfile.itemInstanceTranID = Convert.ToInt32(Iteminstancetranid);
                addfile.itemQuestionFieldId = Convert.ToInt32(questionfieldId);
                addfile.fileType = ReturnFileType(extensionName);
                addfile.fileName = FileName;
                addfile.fileSizeBytes = Convert.ToInt32(size);
                addfile.fileBinary = fileBinary;
                addfile.createdBy = Functions.UserName;
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                int response = 0;
                await Task.Run(async () =>
                {
                    //var AttachmentAPi = QuestAPIMethods.AddFiletoQuestion(Convert.ToInt32(Iteminstancetranid), Convert.ToInt32(questionfieldId), ".JPG", fname, Convert.ToString(size), fileBytes, Functions.UserName);
                    //var response = AttachmentAPi.GetValue("ResponseContent");
                    var AttachmentAPi = QuestSyncAPIMethods.StoreAndCreateFiletoQuestion(App.Isonline, int.Parse(Iteminstancetranid), addfile, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    response = AttachmentAPi.Result;
                });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                if (response > 0)
                {
                    await DisplayAlert("Attachment", "Photo attached Successfully.", "Ok");
                }
                else
                {
                    await DisplayAlert("Attachment", "Photo not attached.", "Ok");
                }


                AttachmentdataCall(metadtaid);
            }
            catch (Exception ex)
            {

                
            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var mi = ((Button)sender);
                var value = mi.CommandParameter as GetFilesByQuestionIDResponse.GetFilesByQuestionIDModel;

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(async () =>
                {
                    var deleteapicall = QuestAPIMethods.DeleteFileToQuestion(Convert.ToString(value.intFileID));
                });
				 AttachmentdataCall(Convert.ToString(value.intItemQuestionMetaDataID));
            }
            catch (Exception ex)
            {
            }
			Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
    }
    public class AttachmentData
    {
        public string AttachmentName { get; set; }
        public string AttachmentId { get; set; }
        public AttachmentData(String name, string nameId)
        {
            AttachmentName = name;
            AttachmentId = nameId;
        }
    }

    public class AttachmentDetail
    {
        public string intFileID { get; set; }
        public string intItemQuestionMetaDataID { get; set; }
        public string strDescription { get; set; }
        public string strFileName { get; set; }
        public string intFileSizeBytes { get; set; }
        public string attchmentSrc
        {
            get
            {
                return $"{"http://quest-s-15.boxerproperty.com/Download.aspx?FileID="} {intFileID}";
            }
        }
    }
}
