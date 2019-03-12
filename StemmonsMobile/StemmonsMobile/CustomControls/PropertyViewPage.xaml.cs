using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyViewPage : ContentPage
    {
        string fileStr = string.Empty;

        private PropertyViewPageViewModel _propertyviewvm;

        public PropertyViewPageViewModel PropertyViewVM
        {
            get
            {
                if (_propertyviewvm == null)
                    _propertyviewvm = new PropertyViewPageViewModel();
                return _propertyviewvm;
            }

            set
            {
                if (_propertyviewvm == value)
                    return;
                _propertyviewvm = value;
            }
        }

        public PropertyViewPage(EntityClass _entdetail)
        {
            InitializeComponent();
            //PropertyPicturecodebtn.Source = "https://atxre.com/wp-content/uploads/2019/01/Image-of-Properties-2.png";
            PropertyViewVM.EntityDetails = _entdetail;

            this.BindingContext = PropertyViewVM;
            getData();
        }

        public void getData()
        {
            try
            {
                var entFileID = PropertyViewVM.EntityDetails.AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityFileID;

                var entEntityID = PropertyViewVM.EntityDetails.AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityID;
                //PropertyPicturecodebtn.Source = "https://atxre.com/wp-content/uploads/2019/01/Image-of-Properties-2.png";

                GetEntityImage(entEntityID.ToString(), entFileID.ToString());
            }
            catch (Exception ec)
            {
                PropertyPicturecodebtn.Source = "Assets/PropertyImage.png";
            }
        }

        async public void GetEntityImage(string EntityID, string FileID)
        {
            await Task.Run(() =>
            {
                var d = EntityAPIMethods.GetFileFromEntity(EntityID, FileID, Functions.UserName);
                fileStr = d.GetValue("ResponseContent").ToString();
            });

            FileItem fileResp = JsonConvert.DeserializeObject<FileItem>(fileStr);

            OpenImage(fileResp.BLOB);
        }
        private void OpenImage(byte[] imageBytes)
        {
            //img.VerticalOptions = LayoutOptions.Center;
            //img.HorizontalOptions = LayoutOptions.Center;
            PropertyPicturecodebtn.BorderColor = Color.Transparent;
            PropertyPicturecodebtn.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }
}