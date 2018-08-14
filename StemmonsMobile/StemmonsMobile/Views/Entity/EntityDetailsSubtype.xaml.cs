using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityDetailsSubtype : ContentPage
    {
        ObservableCollection<EntityListMBView> List_Entityitem = new ObservableCollection<EntityListMBView>();
        EntityOrgCenterList _selectedlist;
        List<EntityClass> EntityLists = new List<EntityClass>();

        string _Viewtype = "";
        public EntityDetailsSubtype(EntityOrgCenterList selectedlist, string Viewtype)
        {
            InitializeComponent();
            _selectedlist = selectedlist;
            Title = _selectedlist.EntityTypeName;
            _Viewtype = Viewtype;
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            txt_seacrhbar.Text = "";
            Functions.IsEditEntity = false;
            Title = _selectedlist.EntityTypeName;
            List_Entityitem.Clear();

            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {

                GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest Lazyload_request = new GetEntitiesBySystemCodeKeyValuePair_LazyLoadRequest
                {
                    user = Functions.UserName,
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
                    ENTITY_TYPE = new List<int>
                {
                    Convert.ToInt32(_selectedlist.EntityTypeID)
                }
                };
                Lazyload_request.entityTypeSchema = fv;

                await Task.Run(async () =>
                {
                    EntityLists = await EntitySyncAPIMethods.GetEntitiesBySystemCodeKeyValuePair_LazyLoadCommon(App.Isonline, Functions.UserName, Lazyload_request, App.DBPath, Convert.ToInt32(_selectedlist.EntityTypeID), Functions.UserFullName, _Viewtype);
                });
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                // convert EntityLists To EntityListMBView 
                // find Title fields ID From entityList Collection
                if (EntityLists?.Count > 0)
                {
                    int count = EntityLists.Count();
                    EntityListMBView mb = new EntityListMBView();
                    var AssocCollection = EntityLists.Select(t => t.AssociationFieldCollection).ToList();
                    for (int i = 0; i < EntityLists.Count; i++)
                    {
                        var te1 = EntityLists[i].AssociationFieldCollection;
                        for (int j = 0; j < te1.Count; j++)
                        {
                            switch (te1[j]?.FieldType?.ToLower())
                            {
                                case "se":
                                case "el":
                                case "me":
                                    if (te1[j]?.AssocSystemCode?.ToLower() == "title")
                                    {
                                        if (te1[j].AssocMetaData.Count != 0)
                                            mb.Title = te1[j].AssocMetaData[0].FieldValue;
                                        else
                                            mb.Title = "";
                                    }
                                    break;
                                default:
                                    if (te1[j]?.AssocSystemCode?.ToLower() == "title")
                                    {
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
                                    }
                                    break;
                            }
                        }
                        mb.Field2 = EntityLists[i].EntityCreatedByFullName;
                        mb.Field4 = Convert.ToDateTime(App.DateFormatStringToString(EntityLists[i].EntityCreatedDateTime)).ToString("MM/dd/yyyy");

                        string a = Convert.ToString(EntityLists[i].EntityTypeID) + " - " + Convert.ToString(EntityLists[i].ListID);

                        string b = Convert.ToString(EntityLists[i].EntityTypeID) + " - " + Convert.ToString(count++);

                        mb.ListId = EntityLists[i].ListID.ToString() != "0" ? a : b;

                        mb.EntityDetails = EntityLists[i];
                        List_Entityitem.Add(mb);
                        mb = new EntityListMBView();
                    }
                    //List<EntityListMBView> ls = List_Entityitem.ToList();

                    //ls = ls.Select(i =>
                    //{
                    //    i.Field4 = Convert.ToDateTime(i.Field4).ToString("MM/dd/yyyy");
                    //    return i;
                    //}).ToList();

                    //var orderByResult = from s in ls
                    //                    orderby s.Field4 
                    //                    select s;

                    List_entity_subtypes.ItemsSource = List_Entityitem;
                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception ex)
            {
                // 
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private async void List_entity_subtypes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Navigation.PushAsync(new Entity_View(e.Item as EntityListMBView));
        }

        private async void Menu_Del_entity_subtypes_Clicked(object sender, EventArgs e)
        {
            MenuItem ls = (MenuItem)sender;
            var bind = ls.BindingContext as EntityListMBView;

            if (bind.EntityDetails.EntityTypeSecurityType != null)
            {

                bool hasDeleteRightOnEntityType = bind.EntityDetails.EntityTypeSecurityType != null && (bind.EntityDetails.EntityTypeSecurityType.ToLowerInvariant().Equals("open") || bind.EntityDetails.EntityTypeSecurityType.ToLowerInvariant().Contains("d"));

                bool fields = bind.EntityDetails.EntityTypeSecurityType.ToLowerInvariant().Contains("u") || bind.EntityDetails.EntityTypeSecurityType.ToLowerInvariant().Equals("open");
                if (fields)
                {
                    if (hasDeleteRightOnEntityType)
                    {
                        var action = await DisplayActionSheet("Are you sure to delete this record?", "Cancel", null, "Yes", "No");
                        switch (action)
                        {
                            case "Yes":

                                var temp = EntitySyncAPIMethods.DeleteEntityItem(App.Isonline, bind.EntityDetails.EntityID, bind.EntityDetails.EntityTypeID, Functions.UserName, App.DBPath);
                                temp.Wait();
                                if (temp.Result)
                                {
                                    var Remove = List_Entityitem.Remove((List_Entityitem.Where(t => t.ListId == bind.ListId).ToList())[0]);
                                    if (Remove) List_entity_subtypes.ItemsSource = List_Entityitem;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        DisplayAlert("Oops", "You don't have rights for Delete this Entity Item.", "Ok");
                    }
                }


            }
        }
        private async void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                    List_entity_subtypes.ItemsSource = List_Entityitem;
                else
                {
                    var itemlist = List_Entityitem.Where(x => x.Title != null && x.Title.ToLower().Contains(e.NewTextValue.ToLower())).ToList(); ;
                    List_entity_subtypes.ItemsSource = itemlist;

                    if (itemlist.Count <= 0)
                    {
                        DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                        ((SearchBar)sender).Text = "";
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void Btn_addentity_Clicked(object sender, EventArgs e)
        {
            try
            {
                EntityListMBView mb = new EntityListMBView();
                mb.EntityDetails = new EntityClass();
                mb.EntityDetails.EntityTypeID = _selectedlist.EntityTypeID;
                mb.EntityDetails.EntityID = _selectedlist.EntityID;
                Functions.IsEditEntity = false;
                await Navigation.PushAsync(new CreateEntityPage(_selectedlist, mb));
            }
            catch (Exception)
            {
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
    }
}
