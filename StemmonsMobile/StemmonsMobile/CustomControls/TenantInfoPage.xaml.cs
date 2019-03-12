using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels;
using StemmonsMobile.ViewModels.EntityViewModel;
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
    public partial class TenantInfoPage : ContentPage
    {
        string fileStr = string.Empty;
        private EntityListViewModel _EntityListviewmodel = new EntityListViewModel();

        public EntityListViewModel EntityListVM
        {
            get
            {
                if (_EntityListviewmodel == null)
                    _EntityListviewmodel = new EntityListViewModel();
                return _EntityListviewmodel;
            }

            set
            {
                if (_EntityListviewmodel == value)
                    return;
                _EntityListviewmodel = value;
            }
        }



        public TenantInfoPage(EntityClass entityDetails, string _pagecode)
        {
            InitializeComponent();

            Titlelbl.Text = entityDetails.EntityTitle;
            EntityListVM.ScreenCode = _pagecode;
            if (_pagecode == "TNTLIST")
            {
                Title = "Tenant List";
                EntityListVM.SystemCodeEntityType = "CSTMR";
                EntityListVM._Viewtype = "";
            }
            else
            {
                Title = "Available Units";
                //EntityListVM.ScreenCode = "TNTLIST";
                EntityListVM.SystemCodeEntityType = "No code";
                EntityListVM._Viewtype = "";
            }


            try
            {
                var entFileID = entityDetails.AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityFileID;

                var entEntityID = entityDetails.AssociationFieldCollection.Where(x => x.AssocSystemCode == "PHGAL").FirstOrDefault().AssocMetaDataText.FirstOrDefault().EntityID;

                GetEntityImage(entEntityID.ToString(), entFileID.ToString());
            }

            catch (Exception ex)
            {
                ProfileImg.Source = "Assets/Property_Default.png";
            }


        }

        int? _pageindex = 1;
        int? pageSize = 50;

        protected async override void OnAppearing()
        {
            base.OnAppearing();


            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                for (int i = 1; i <= _pageindex; i++)
                {
                    EntityListVM.PageIndex = i;
                    await EntityListVM.GetEntityListwithCall();
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

        }

        async public void GetEntityImage(string EntityID, string FileID)
        {
            await Task.Run(() =>
            {
                var d = EntityAPIMethods.GetFileFromEntity(EntityID, FileID, Functions.UserName);
                fileStr = d.GetValue("ResponseContent").ToString();
            });

            FileItem fileResp = JsonConvert.DeserializeObject<FileItem>(fileStr);
            //byte[] fileData = fileResp.BLOB; //File byte array

            OpenImage(fileResp.BLOB);
        }
        private void OpenImage(byte[] imageBytes)
        {
            ProfileImg.BorderColor = Color.Transparent;
            ProfileImg.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
        }
    }


    //class TenantInformationList
    //{
    //    public string TenantFieldName { get; set; }
    //    public string TenantFieldValue { get; set; }
    //    public string TenantExpDate { get; set; }

    //    public List<TenantInformationList> lst_entity = new List<TenantInformationList>();
    //}
}