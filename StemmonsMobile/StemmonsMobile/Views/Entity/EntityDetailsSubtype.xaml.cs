using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Datatemplates;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.ViewModels.EntityViewModel;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityDetailsSubtype : ContentPage
    {
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

        ObservableCollection<EntityClass> List_Entityitem = new ObservableCollection<EntityClass>();
        EntityOrgCenterList _selectedlist;
        List<EntityClass> EntityLists = new List<EntityClass>();
        int? _pageindex = 1;
        int? pageSize = 50;
        static string _Viewtype = "";

        public EntityDetailsSubtype(EntityOrgCenterList selectedlist, string Viewtype)
        {

            InitializeComponent();

            _selectedlist = selectedlist;
            Title = _selectedlist.EntityTypeName;
            EntityListVM._Viewtype = _Viewtype = Viewtype;
            EntityListVM._entityTypeID = _selectedlist.EntityTypeID;

            // List_entity_subtypes.ItemAppearing += List_entity_subtypes_ItemAppearing;

            #region Lazy Loding Logic
            //List_entity_subtypes.ItemAppearing += async (object sender, ItemVisibilityEventArgs e) =>
            //{
            //    if (App.Isonline)
            //    {
            //        var item = e.Item as EntityListMBView;
            //        int index = 0;
            //        try
            //        {
            //            index = List_Entityitem.IndexOf(item);
            //        }
            //        catch (Exception eqs)
            //        {

            //        }
            //        if (List_Entityitem.Count - 2 <= index)
            //        {
            //            if (List_Entityitem.Count == (pageSize * _pageindex))
            //            {
            //                Device.BeginInvokeOnMainThread(() =>
            //                {
            //                    //lstfooter_indicator.IsVisible = true;
            //                });

            //                _pageindex++;
            //                await LoadEntityList(_pageindex, pageSize);

            //                Device.BeginInvokeOnMainThread(() =>
            //                {
            //                    //lstfooter_indicator.IsVisible = false;
            //                });
            //            }

            //        }
            //    }
            //}; 
            #endregion
        }


        //private void List_entity_subtypes_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    MessagingCenter.Send<EntityDetailsSubtype, ItemVisibilityEventArgs>(this, "LoadItems", e.Item as ItemVisibilityEventArgs);
        //}

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (_selectedlist.SecurityType.ToLower() == "r")
                btn_addentity.IsVisible = false;

            App.SetConnectionFlag();
            txt_seacrhbar.Text = "";
            Functions.IsEditEntity = false;
            Title = _selectedlist.EntityTypeName;
            List_Entityitem.Clear();
            EntityListVM.ListEntityitem.Clear();
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                for (int i = 1; i <= _pageindex; i++)
                {
                    EntityListVM.PageIndex = i;
                    await EntityListVM.GetEntityListwithCall();
                    //await LoadEntityList();
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        //private async Task LoadEntityList()
        //{

        //    await EntityListVM.GetEntityListwithCall();

        //    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

        //    // convert EntityLists To EntityListMBView 
        //    // find Title fields ID From entityList Collection
        //    //if (EntityListVM.ListEntityitem?.Count > 0)
        //    //{
        //    //    int count = EntityListVM.ListEntityitem.Count();
        //    //    EntityListMBView mb = new EntityListMBView();
        //    //    var AssocCollection = EntityListVM.ListEntityitem.Select(t => t.AssociationFieldCollection).ToList();
        //    //    for (int i = 0; i < EntityListVM.ListEntityitem.Count; i++)
        //    //    {
        //    //        var te1 = EntityListVM.ListEntityitem[i].AssociationFieldCollection;
        //    //        for (int j = 0; j < te1.Count; j++)
        //    //        {
        //    //            switch (te1[j]?.FieldType?.ToLower())
        //    //            {
        //    //                case "se":
        //    //                case "el":
        //    //                case "me":
        //    //                    //if (te1[j]?.AssocSystemCode?.ToLower() == "title")
        //    //                    if (te1[j]?.AssocMetaDataText?.Count>0)
        //    //                    {
        //    //                        if (te1[j].AssocMetaData.Count != 0)
        //    //                            mb.Title = te1[j].AssocMetaData[0].FieldValue;
        //    //                        else
        //    //                            mb.Title = "";
        //    //                    }
        //    //                    break;
        //    //                default:
        //    //                    //if (te1[j]?.AssocSystemCode?.ToLower() == "title")
        //    //                    if (te1[j]?.AssocMetaDataText?.Count > 0)
        //    //                    {
        //    //                        if (te1[j].AssocMetaDataText.Count != 0)
        //    //                        {
        //    //                            if (te1[j].AssocMetaDataText.Count != 0)
        //    //                                mb.Title = te1[j].AssocMetaDataText[0].TextValue;
        //    //                            else
        //    //                                mb.Title = "";
        //    //                        }
        //    //                        else if (te1[j].AssocDecode.Count != 0)
        //    //                        {
        //    //                            if (te1[j].AssocDecode.Count != 0)
        //    //                                mb.Title = te1[j].AssocDecode[0].AssocDecodeName;
        //    //                            else
        //    //                                mb.Title = "";
        //    //                        }
        //    //                    }
        //    //                    break;
        //    //            }
        //    //        }
        //    //        mb.Field2 = "Created By: " + EntityLists[i].EntityCreatedByFullName;
        //    //        mb.Field4 = Convert.ToDateTime(CommonConstants.DateFormatStringToString(EntityLists[i].EntityCreatedDateTime)).Date.ToString("d");

        //    //        string a = Convert.ToString(EntityLists[i].EntityTypeID) + " - " + Convert.ToString(EntityLists[i].ListID);

        //    //        string b = Convert.ToString(EntityLists[i].EntityTypeID) + " - " + Convert.ToString(count++);

        //    //        mb.ListId = EntityLists[i].ListID.ToString() != "0" ? a : b;

        //    //        mb.EntityDetails = EntityLists[i];
        //    //        List_Entityitem.Add(mb);
        //    //        mb = new EntityListMBView();
        //    //    }

        //    //    Device.BeginInvokeOnMainThread(() =>
        //    //    {
        //    //        //List_entity_subtypes.ListSource = null;
        //    //        //List_entity_subtypes.ListSource = EntityListVM.ListEntityitem;
        //    //    });
        //    //}
        //    //else
        //    //{
        //    //    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
        //    //}
        //}

        private async void List_entity_subtypes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var obj = e.Item as EntityClass;
            await Navigation.PushAsync(new Entity_View(obj));
        }

        private async void Menu_Del_entity_subtypes_Clicked(object sender, EventArgs e)
        {
            MenuItem ls = (MenuItem)sender;
            var bind = ls.BindingContext as EntityClass;

            if (bind.EntityTypeSecurityType != null)
            {

                bool hasDeleteRightOnEntityType = bind.EntityTypeSecurityType != null && (bind.EntityTypeSecurityType.ToLowerInvariant().Equals("open") || bind.EntityTypeSecurityType.ToLowerInvariant().Contains("d"));

                bool fields = bind.EntityTypeSecurityType.ToLowerInvariant().Contains("u") || bind.EntityTypeSecurityType.ToLowerInvariant().Equals("open");
                if (fields)
                {
                    if (hasDeleteRightOnEntityType)
                    {
                        var action = await DisplayActionSheet("Are you sure to delete this record?", "Cancel", null, "Yes", "No");
                        switch (action)
                        {
                            case "Yes":

                                var temp = EntitySyncAPIMethods.DeleteEntityItem(App.Isonline, bind.EntityID, bind.EntityTypeID, Functions.UserName, App.DBPath);
                                temp.Wait();
                                if (temp.Result)
                                {
                                    var Remove = List_Entityitem.Remove((List_Entityitem.Where(t => t.ListID == bind.ListID).ToList())[0]);
                                    if (Remove)
                                    { }
                                    //List_entity_subtypes.ListSource = List_Entityitem;
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
        private void Txt_seacrhbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    //List_entity_subtypes.ItemsSource = List_Entityitem;
                }

                else
                {
                    var itemlist = List_Entityitem.Where(x => x.EntityTitle != null && x.EntityTitle.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                    //List_entity_subtypes.ItemsSource = itemlist;

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
                EntityClass mb = new EntityClass();
                mb.EntityTypeID = _selectedlist.EntityTypeID;
                mb.EntityID = _selectedlist.EntityID;
                Functions.IsEditEntity = false;
                await Navigation.PushAsync(new CreateEntityPage(_selectedlist.EntityTypeID, _selectedlist.EntityID, _selectedlist.NewEntityText, mb));
            }
            catch (Exception)
            {
            }
        }
        int cmnt = 0;
        private void Btn_home_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private void Btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);
            }
            catch (Exception)
            {
            }
        }

        //private bool _isRefreshing = false;
        //public bool IsRefreshing
        //{
        //    get { return _isRefreshing; }
        //    set
        //    {
        //        _isRefreshing = value;
        //        OnPropertyChanged(nameof(IsRefreshing));
        //    }
        //}

        //public ICommand RefreshCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            IsRefreshing = true;

        //            try
        //            {
        //                //await LoadEntityList(0, 0);
        //            }
        //            catch (Exception)
        //            {
        //            }
        //        });
        //    }
        //}
    }
}
