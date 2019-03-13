using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Behavior;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.CustomControls;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StemmonsMobile.ViewModels.EntityViewModel
{
    public class EntityListViewModel : INotifyPropertyChanged
    {
        public string ScreenCode { get; set; }
        // Blank Means Fetch All Entity List Based on Type ID
        //"MYPROPLIST" - To Fetch Properties Related to User
        //"PROPLIST" - To Fetch Properties Based on Entity Type System Code

        public EntityListViewModel()
        {
            ListEntityitem = new ObservableCollection<EntityListMBView>();
            refreshCommand = RefreshCommand;

            this.LoadDataCommand = new Command(async () =>
            {

                PageIndex += 1;

                IsShow = true;
                //await GetEntityListwithCall();
                IsShow = false;

            });

            this.ItemTappedCommand = new Command<ItemTappedEventArgs>(onItemTap);
        }


        public ICommand LoadDataCommand
        {
            get;
            set;
        }

        public ICommand ItemTappedCommand
        {
            get;
            set;
        }

        public ObservableCollection<EntityListMBView> ListEntityitem { get; set; }

        public string _Viewtype { get; set; }

        public int _entityTypeID { get; set; }
        public int EntityID { get; set; }

        public string SystemCodeEntityType { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; } = 50;//Number Of records

        public async void onItemTap(object item)
        {
            if (item != null)
            {
                var tap = item as EntityListMBView;
                if (string.IsNullOrEmpty(ScreenCode) || ScreenCode == "UNITS")
                {
                    // For Regualr Entity List Page
                    await Application.Current.MainPage.Navigation.PushAsync(new Entity_View(tap));
                }
                else if (ScreenCode == "CAMPS")
                {
                    // For Campus Page
                    await Application.Current.MainPage.Navigation.PushAsync(new CampusPage(tap.EntityDetails));
                }
                else if (ScreenCode == "TNTLIST")
                {
                    // For Campus Page
                    await Application.Current.MainPage.Navigation.PushAsync(new TenantViewPage(tap.EntityDetails));
                }
                else
                {
                    // For Property List and My Property 
                    await Application.Current.MainPage.Navigation.PushAsync(new PropertyViewPage(tap.EntityDetails));
                }
            }
        }

        EntityListMBView _elist;
        public EntityListMBView _elist_property
        {
            get => _elist;
            set
            {
                if (value != null)
                {
                    _elist = value;
                    onItemTap(_elist);
                }
            }
        }


        public async Task GetEntityListwithCall()
        {
            try
            {
                #region Parameter
                GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest Lazyload_request = new GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest
                {
                    user = StemmonsMobile.Commonfiles.Functions.UserName,
                };

                if (_Viewtype?.ToLower() == "assigned to me")
                    Lazyload_request.assignedToMe = 'Y';
                if (_Viewtype?.ToLower() == "active")
                    Lazyload_request.isActive = 'Y';
                if (_Viewtype?.ToLower() == "inactive")
                    Lazyload_request.isActive = 'N';
                if (_Viewtype?.ToLower() == "created by me")
                    Lazyload_request.createdByMe = 'Y';
                if (_Viewtype?.ToLower() == "owned by me")
                    Lazyload_request.ownedByMe = 'Y';
                if (_Viewtype?.ToLower() == "associated by me")
                    Lazyload_request.associatedToMe = 'Y';
                if (_Viewtype?.ToLower() == "inactivated by me")
                    Lazyload_request.inActivatedByMe = 'Y';

                FILTER_VALUE fv = new FILTER_VALUE
                {
                    SHOW_ENTITIES_ACTIVE_INACTIVE = "ALL",
                    EXTERNAL_DATASOURCE_OBJECT_ID_ENTITY = EntityID,
                    ENTITY_TYPE = new List<int>
                    {
                        (int) _entityTypeID
                    }
                };
                //if (ScreenCode == "PROPLIST")
                {
                    fv.SYSTEM_CODE_ENTITY_TYPE = SystemCodeEntityType;
                }
                Lazyload_request.EntityTypeID = _entityTypeID;
                Lazyload_request.entityTypeSchema = fv;

                Lazyload_request.pageIndex = PageIndex;
                Lazyload_request.pageSize = PageSize;
                #endregion

                List<EntityClass> ListEntity = new List<EntityClass>();

                // EntityListVM.ScreenCode = "MYPROPLIST";
                // EntityListVM.ScreenCode = "PROPLIST";
                // EntityListVM.ScreenCode = "CAMPS";

                if (ScreenCode == "CAMPSRELATED")
                {
                    var Result = EntityAPIMethods.GetEntityTypeRelationDatabyentityid(EntityID, Functions.UserName, "");
                    string jsonValue = Convert.ToString(Result.GetValue("ResponseContent"));
                    ListEntity = JsonConvert.DeserializeObject<List<EntityClass>>(jsonValue);
                }
                else if (ScreenCode == "PROPLIST" || ScreenCode == "CAMPS" || ScreenCode == "TNTLIST" || ScreenCode == "UNITS")
                {
                    Lazyload_request.pageIndex = 0;
                    Lazyload_request.pageSize = 0;

                    await Task.Run(async () =>
                    {
                        ListEntity = await EntitySyncAPIMethods.GetEntitiesBySystemCodeKeyValuePair_LazyLoadCommon(App.Isonline, Functions.UserName, Lazyload_request, App.DBPath, (int)_entityTypeID, Functions.UserFullName, _Viewtype);
                    });
                }
                else
                {
                    await Task.Run(() =>
                    {
                        ListEntity = EntitySyncAPIMethods.GetEntityRoleAssignByEmp(App.Isonline, Functions.UserName, "PROPY", App.DBPath);
                    });
                }

                var List_Entityitem = new ObservableCollection<EntityListMBView>();

                var _item = new ObservableCollection<EntityClass>(ListEntity);

                if (_item?.Count > 0)
                {
                    int count = _item.Count();
                    EntityListMBView mb = new EntityListMBView();
                    for (int i = 0; i < _item.Count; i++)
                    {
                        var te1 = _item[i].AssociationFieldCollection;
                        for (int j = 0; j < te1.Count; j++)
                        {
                            switch (te1[j]?.FieldType?.ToLower())
                            {
                                case "se":
                                case "el":
                                case "me":
                                    if (te1[j]?.AssocSystemCode?.ToLower() == "title")
                                        if (te1[j].AssocMetaData.Count != 0)
                                            mb.Title = te1[j].AssocMetaData[0].FieldValue;
                                        else
                                            mb.Title = "";
                                    break;
                                default:
                                    if (te1[j]?.AssocSystemCode?.ToLower() == "title")
                                        if (te1[j].AssocMetaDataText.Count != 0)
                                        {
                                            if (te1[j].AssocMetaDataText.Count != 0)
                                                mb.Title = te1[j].AssocMetaDataText[0].TextValue;
                                            else
                                                mb.Title = "";
                                        }
                                        else if (te1[j].AssocDecode.Count != 0)
                                        {
                                            if (te1[j].AssocDecode.Count != 0)
                                                mb.Title = te1[j].AssocDecode[0].AssocDecodeName;
                                            else
                                                mb.Title = "";
                                        }
                                    break;
                            }
                        }
                        mb.Field2 = "Created By: " + _item[i].EntityCreatedByFullName;

                        mb.Field4 = Convert.ToDateTime(DataServiceBus.OfflineHelper.DataTypes.Common.CommonConstants.DateFormatStringToString(_item[i].EntityCreatedDateTime)).Date.ToString("d");

                        string a = Convert.ToString(_item[i].EntityTypeID) + " - " + Convert.ToString(_item[i].ListID);

                        string b = Convert.ToString(_item[i].EntityTypeID) + " - " + Convert.ToString(count++);

                        mb.ListId = _item[i].ListID.ToString() != "0" ? a : b;

                        mb.EntityDetails = _item[i];
                        List_Entityitem.Add(mb);
                        mb = new EntityListMBView();
                    }
                }

                ListEntityitem.Clear();
                foreach (var item in List_Entityitem)
                {
                    ListEntityitem.Add(item);
                }
            }
            catch (Exception)
            {
            }
        }

        private bool isShow;
        public bool IsShow
        {
            get { return isShow; }
            set
            {
                if (isShow == value)
                    return;

                isShow = value;
                OnPropertyChanged(nameof(IsShow));
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;

                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private Command refreshCommand;

        public Command RefreshCommand
        {
            get
            {
                return new Command(() =>
                {

                    RefreshList();
                });
                ;
            }
        }

        async Task RefreshList()
        {
            IsBusy = true;
            PageIndex = 0;
            PageSize = 0;
            await GetEntityListwithCall();
            IsBusy = false;
        }

        //private async Task ExecuteRefreshCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;
        //    ListEntityitem.Clear();

        //    await Task.Run(() => { Task.Delay(5000); });

        //    for (int i = 0; i < 10; i++)
        //        ListEntityitem.Add(new EntityListMBView() { Title = "Test " + i });

        //    IsBusy = false;
        //}

        #region INotifyPropertyChanged implementation

        //To let the UI know that something changed on the View Model
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}