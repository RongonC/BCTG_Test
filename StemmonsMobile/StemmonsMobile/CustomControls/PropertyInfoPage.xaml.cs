using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Controls;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PropertyInfoPage : ContentPage
    {
        public EntityClass EntityLists = new EntityClass();
        public string fileStr = string.Empty;
        public PropertyInfoPage(EntityClass _entityClass)
        {
            InitializeComponent();
            //. create obj  from cnt and pass to select entity

            //add cntrl
            EntityLists = _entityClass;
            EntityFieldListView control = new EntityFieldListView(_entityClass, new List<string>() { "TITLE", "EPILR" }, new List<string>() { "1470" });
            controlFrame.Content = control;

            this.BindingContext = EntityLists;// new PropInfoPageViewModel(_entityClass);

            //if (App.Isonline)
            //    PropertyInfoImage.Source = new UriImageSource
            //    {
            //        Uri = new Uri("https://atxre.com/wp-content/uploads/2019/01/Image-of-Properties-2.png"),
            //        CachingEnabled = true,
            //    };
            //else
            //    PropertyInfoImage.Source = ImageSource.FromFile("Assets/userIcon.png");

            try
            {
                var entFileID = EntityLists.AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityFileID;

                var entEntityID = EntityLists.AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityID;

                GetEntityImage(entEntityID.ToString(), entFileID.ToString());
            }
            catch (Exception ex)
            {
                ProfileImg.Source = "Assets/PropertyImage.png";
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
        public void OpenImage(byte[] imageBytes)
        {
            ProfileImg.BorderColor = Color.Transparent;
            ProfileImg.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }
}

class PropertyInformationList
{
    public string PropertyFieldName { get; set; }
    public string PropertyFieldValue { get; set; }

    public List<PropertyInformationList> lst_entity = new List<PropertyInformationList>();
}


