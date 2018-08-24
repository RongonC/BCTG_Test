using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLStorage;
using Plugin.Media;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateEntityPage : ContentPage
    {
        BorderEditor txt_EntNotes = new BorderEditor();

        EntityOrgCenterList _selected;
        EntityListMBView mbView = new EntityListMBView();
        EntityClass EntitySchemaLists = null;
        EntityClass EntityListsValues = null;
        List<Entity_Notes> Entity_NotesLists = new List<Entity_Notes>();

        string ischeckcalControl = string.Empty;
        string ContolrLst = string.Empty;

        ObservableCollection<EntityNotesGroup> NotesGroups = new ObservableCollection<EntityNotesGroup>();
        string CalTxtbox = string.Empty;
        List<KeyValuePair<List<AssociationField>, string>> calculationsFieldlist = new List<KeyValuePair<List<AssociationField>, string>>();
        List<Tuple<List<AssociationField>, string, string>> lstcalculationsFieldlist = new List<Tuple<List<AssociationField>, string, string>>();
        public CreateEntityPage()
        { }
        public CreateEntityPage(EntityOrgCenterList value, EntityListMBView _mbView)
        {
            InitializeComponent();
            mbView = _mbView;
            _selected = value;
            if (!Functions.IsEditEntity)
            {
                Title = value.NewEntityText;
            }
            else
                Title = "Edit Entity";

            if (Functions.IsEditEntity)
            {
                var s = new FormattedString();

                if (mbView != null)
                {
                    if (mbView.EntityDetails.EntityCreatedByFullName != null)
                    {
                        s.Spans.Add(new Span { Text = mbView.EntityDetails.EntityCreatedByFullName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(mbView.EntityDetails.EntityCreatedDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_createname.FormattedText = s;

                    s = new FormattedString();
                    if (mbView.EntityDetails.EntityAssignedToFullName != null)
                    {
                        s.Spans.Add(new Span { Text = mbView.EntityDetails.EntityAssignedToFullName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(mbView.EntityDetails.EntityAssignedToDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_assignto.FormattedText = s;

                    s = new FormattedString();
                    if (mbView.EntityDetails.EntityOwnedByFullName != null)
                    {
                        s.Spans.Add(new Span { Text = mbView.EntityDetails.EntityOwnedByFullName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(mbView.EntityDetails.EntityOwnedByDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_ownername.FormattedText = s;

                    s = new FormattedString();
                    if (mbView.EntityDetails.EntityModifiedByFullName != null)
                    {
                        s.Spans.Add(new Span { Text = mbView.EntityDetails.EntityModifiedByFullName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(mbView.EntityDetails.EntityModifiedDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_modifiedname.FormattedText = s;
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            NotesGroups.Clear();
            App.SetConnectionFlag();
            DesignFormDynamic();
        }

        private bool CanCreate = false;


        public async void DesignFormDynamic()
        {
            try
            {
                TextFieldsLayout.Children.Clear();

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                await Task.Run(async () =>
                {
                    try
                    {
                        EntitySchemaLists = await EntitySyncAPIMethods.GetEntityTypeSchema(App.Isonline, ConstantsSync.EntityInstance, Convert.ToInt32(_selected.EntityTypeID), Functions.UserName, null, App.DBPath, Functions.IsEditEntity ? "U" : "C");
                    }
                    catch (Exception)
                    {
                    }
                    if (Functions.IsEditEntity)
                    {
                        try
                        {
                            EntityListsValues = await EntitySyncAPIMethods.GetEntityByEntityID(App.Isonline, _selected.EntityID.ToString(), Functions.UserName, Convert.ToString(_selected.EntityTypeID), App.DBPath);
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            Entity_NotesLists = await EntitySyncAPIMethods.GetNotes(App.Isonline, mbView.EntityDetails.EntityID.ToString(), Functions.UserName, mbView.EntityDetails.EntityTypeID.ToString(), mbView.EntityDetails.EntityTypeName, App.DBPath);

                        }
                        catch (Exception)
                        {
                        }
                    }
                });

                #region Get Entity Schema By EntityTypeID for Make Dynamic Design

                if (EntitySchemaLists == null)
                {
                    return;
                }

                EntitySchemaLists.AssociationFieldCollection = EntitySchemaLists.AssociationFieldCollection.OrderBy(x => x.ItemDesktopPriorityValue).ToList();

                if (EntitySchemaLists.AssociationFieldCollection != null)
                {
                    var assocFieldColl = EntitySchemaLists.AssociationFieldCollection.Where(field => field.IsExternalEntityAssocType == false)
                                                         .OrderBy(x => x.ItemDesktopPriorityValue == null)
                                                         .ThenBy(x => x.ItemDesktopPriorityValue);
                    if (!Functions.IsEditEntity)
                        CanCreate = assocFieldColl.Where(w => w.IsRequired == "Y").All(x => x.SecurityType != null && (x.SecurityType.ToLowerInvariant().Contains("c") || x.SecurityType.ToLowerInvariant().Equals("open")));
                    else
                        CanCreate = assocFieldColl.Where(w => w.IsRequired == "Y").All(x => x.SecurityType != null && (x.SecurityType.ToLowerInvariant().Contains("r") || x.SecurityType.ToLowerInvariant().Equals("open")));
                }

                if (CanCreate)
                {
                    if (!string.IsNullOrEmpty(EntitySchemaLists?.ToString()) && EntitySchemaLists.ToString() != "[]")
                    {
                        for (int i = 0; i < EntitySchemaLists.AssociationFieldCollection.Count; i++)
                        {
                            var Mainlayout = new StackLayout();
                            Mainlayout.Orientation = StackOrientation.Horizontal;
                            Mainlayout.Margin = new Thickness(10, 0, 0, 0);

                            var LeftLyout = new StackLayout();
                            LeftLyout.HorizontalOptions = LayoutOptions.Start;
                            LeftLyout.WidthRequest = 200;

                            var RightLyout = new StackLayout();
                            RightLyout.HorizontalOptions = LayoutOptions.Start;
                            RightLyout.VerticalOptions = LayoutOptions.Start;

                            var assocFieldColl = EntitySchemaLists.AssociationFieldCollection.Where(field => field.IsExternalEntityAssocType == false).ToList();

                            bool fieldLeveSecurity_Create = assocFieldColl.Where(w => w.IsRequired == "Y").All(x => x.SecurityType != null && (x.SecurityType.ToLowerInvariant().Contains("c") ||
                                                x.SecurityType.ToLowerInvariant().Equals("open")));

                            string _field_type = EntitySchemaLists.AssociationFieldCollection[i].FieldType;
                            bool allow = true;
                            if (_field_type == "MF" || _field_type == "SF" || _field_type == "PG")
                            {
                                allow = Functions.IsEditEntity;
                            }
                            if (allow)
                            {
                                var Label1 = new Label
                                { VerticalOptions = LayoutOptions.Start };
                                Label1.Text = EntitySchemaLists.AssociationFieldCollection[i].AssocName;
                                Label1.HorizontalOptions = LayoutOptions.Start;
                                Label1.FontSize = 16;
                                Label1.FontFamily = "Soin Sans Neue";
                                LeftLyout.Children.Add(Label1);
                                Mainlayout.Children.Add(LeftLyout);
                            }


                            switch (_field_type)
                            {
                                #region SE - EL - ME - MS - SS
                                case "SE":
                                case "EL":
                                case "ME":
                                case "MS":
                                case "SS":
                                    Picker pk = new Picker
                                    {
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID,
                                        WidthRequest = 200,
                                        TextColor = Color.Gray
                                    };
                                    if (Functions.IsEditEntity)
                                    {
                                        pk.SelectedIndexChanged += Pk_SelectedIndexChanged;
                                    }
                                    else
                                    {
                                        if (Device.RuntimePlatform == "iOS")
                                        {
                                            pk.Unfocused += Pk_Unfocused;
                                        }
                                        else
                                        {
                                            pk.SelectedIndexChanged += Pk_SelectedIndexChanged;
                                        }
                                    }

                                    try
                                    {
                                        List<EXTERNAL_DATASOURCE1> _list_ed1 = new List<EXTERNAL_DATASOURCE1>();
                                        EXTERNAL_DATASOURCE1 ed1 = new EXTERNAL_DATASOURCE1
                                        {
                                            Count = 0,
                                            EXTERNAL_DATASOURCE_DESCRIPTION = "-- Select Item --",
                                            EXTERNAL_DATASOURCE_NAME = "-- Select Item --",
                                            ID = 0
                                        };
                                        _list_ed1.Add(ed1);

                                        List<AssociationField> ls = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID).ToList();

                                        foreach (var item in ls)
                                        {
                                            if (item.EntityAssocTypeCascade?.Count >= 0)
                                            {
                                                List<int> CHildLst = item.EntityAssocTypeCascade?.Where(t => t.EntityAssocTypeIDChild == EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID).ToList().Select(x => x.EntityAssocTypeIDChild).ToList();
                                                if (CHildLst.Count < 1 && App.Isonline)
                                                {
                                                    if (EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE != null)
                                                    {
                                                        if (EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE.Count != 0)
                                                        {
                                                            for (int j = 0; j < EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE.Count(); j++)
                                                            {
                                                                _list_ed1.Add(EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE[j]);
                                                            }
                                                        }
                                                    }
                                                }
                                                else if (!App.Isonline)
                                                {
                                                    if (EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE == null)
                                                    {
                                                        List<EXTERNAL_DATASOURCE1> ld = new List<EXTERNAL_DATASOURCE1>();
                                                        EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE.Add(new EXTERNAL_DATASOURCE1());
                                                    }
                                                    if (EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE.Count != 0)
                                                    {
                                                        for (int j = 0; j < EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE.Count(); j++)
                                                        {
                                                            _list_ed1.Add(EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE[j]);
                                                        }
                                                    }

                                                    if (Functions.IsEditEntity)
                                                    {
                                                        var cnttype = EntityListsValues.AssociationFieldCollection.Where(v => v.AssocTypeID == EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID)?.ToList();
                                                        var Assoc = cnttype[0].AssocMetaData[0];

                                                        if (!string.IsNullOrEmpty(Assoc.FieldValue) && !string.IsNullOrEmpty(Assoc.ExternalDatasourceObjectID))
                                                        {
                                                            var p1 = _list_ed1.Find(x => x.ID == Convert.ToInt32(Assoc.ExternalDatasourceObjectID == "" ? "0" : Assoc.ExternalDatasourceObjectID));
                                                            if (p1 == null)
                                                            {
                                                                p1 = new EXTERNAL_DATASOURCE1();
                                                                p1.EXTERNAL_DATASOURCE_NAME = Assoc.FieldValue;
                                                                p1.ID = Convert.ToInt32(Assoc.ExternalDatasourceObjectID);
                                                                EntitySchemaLists.AssociationFieldCollection[i]?.EXTERNAL_DATASOURCE.Add(p1);
                                                                _list_ed1.Add(p1);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        pk.ItemsSource = _list_ed1;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (!fieldLeveSecurity_Create)
                                        pk.IsEnabled = false;
                                    pk.ItemDisplayBinding = new Binding("EXTERNAL_DATASOURCE_NAME");
                                    pk.SelectedIndex = 0;
                                    RightLyout.Children.Add(pk);
                                    break;
                                #endregion

                                #region DO - DT
                                case "DO":
                                case "DT":
                                    DatePicker DO = new DatePicker
                                    {
                                        Format = "MM/dd/yyyy",
                                        VerticalOptions = LayoutOptions.Start,
                                        WidthRequest = 200,
                                        Date = Convert.ToDateTime("01/01/1900"),
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID,
                                        TextColor = Color.Gray
                                    };
                                    RightLyout.Children.Add(DO);
                                    if (!fieldLeveSecurity_Create)
                                        DO.IsEnabled = false;
                                    try
                                    {
                                        ischeckcalControl = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(DO.StyleId?.Split('_')[1])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), DO.StyleId?.Split('_')[0]?.ToUpper() + "_" + DO.StyleId?.Split('_')[1]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                        DO.DateSelected += DO_DateSelected;
                                    break;
                                #endregion

                                #region ST - LN
                                case "ST":
                                case "LN":
                                    Entry ST = new Entry
                                    {
                                        FontSize = 16,
                                        Keyboard = Keyboard.Default,
                                        VerticalOptions = LayoutOptions.Start,
                                        WidthRequest = 200,
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID,
                                        FontFamily = "Soin Sans Neue"
                                    };
                                    RightLyout.Children.Add(ST);
                                    ST.Text = "";
                                    if (!fieldLeveSecurity_Create)
                                        ST.IsEnabled = false;
                                    try
                                    {
                                        ischeckcalControl = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(ST.StyleId?.Split('_')[1])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), ST.StyleId?.Split('_')[0]?.ToUpper() + "_" + ST.StyleId?.Split('_')[1]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                        ST.Unfocused += ST_Unfocused; ;
                                    break;
                                #endregion

                                #region MN - SN
                                case "MN":
                                case "SN":

                                    Entry txt_number = new Entry
                                    {
                                        FontSize = 16,
                                        WidthRequest = 200,
                                        Keyboard = Keyboard.Numeric,
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    txt_number.WidthRequest = 200;
                                    txt_number.Keyboard = Keyboard.Numeric;
                                    txt_number.StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;
                                    txt_number.FontFamily = "Soin Sans Neue";
                                    RightLyout.Children.Add(txt_number);
                                    txt_number.Text = "";
                                    if (!fieldLeveSecurity_Create)
                                        txt_number.IsEnabled = false;
                                    try
                                    {
                                        ischeckcalControl = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(txt_number.StyleId?.Split('_')[1])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), txt_number.StyleId?.Split('_')[0]?.ToUpper() + "_" + txt_number.StyleId?.Split('_')[1]);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                        txt_number.Unfocused += ST_Unfocused;
                                    break;
                                #endregion

                                #region TA - RT - ET - MT
                                case "TA":
                                case "RT":
                                case "ET":
                                case "MT":
                                    BorderEditor editor = new BorderEditor
                                    {
                                        FontSize = 16,
                                        HeightRequest = 100,
                                        BorderColor = Color.LightGray,
                                        BorderWidth = 1,
                                        CornerRadius = 5,
                                        WidthRequest = 200,
                                        Keyboard = Keyboard.Default,
                                        VerticalOptions = LayoutOptions.Start,
                                    };
                                    editor.WidthRequest = 200;
                                    editor.HeightRequest = 200;
                                    editor.StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;
                                    editor.FontFamily = "Soin Sans Neue";
                                    RightLyout.Children.Add(editor);
                                    editor.Text = "";
                                    if (!fieldLeveSecurity_Create)
                                        editor.IsEnabled = false;
                                    break;
                                #endregion

                                #region CL
                                case "CL":
                                    Entry CL = new Entry
                                    {
                                        FontSize = 16,
                                        VerticalOptions = LayoutOptions.Start,
                                        IsEnabled = false,
                                        Text = "",
                                        WidthRequest = 200,
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID,
                                        FontFamily = "Soin Sans Neue"
                                    };
                                    RightLyout.Children.Add(CL);
                                    if (!fieldLeveSecurity_Create)
                                        CL.IsEnabled = false;
                                    break;
                                #endregion

                                #region CB
                                case "CB":
                                    Switch cb = new Switch
                                    {
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID
                                    };
                                    RightLyout.Children.Add(cb);
                                    if (!fieldLeveSecurity_Create)
                                        cb.IsEnabled = false;
                                    break;
                                #endregion

                                #region MF - SF
                                case "MF":
                                case "SF":
                                    Label lbl = new Label
                                    {
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID,
                                        FontSize = 16
                                    };
                                    lbl.FontFamily = "Soin Sans Neue";
                                    if (Functions.IsEditEntity)
                                        RightLyout.Children.Add(lbl);
                                    if (!fieldLeveSecurity_Create)
                                        lbl.IsEnabled = false;
                                    break;
                                #endregion

                                #region PG
                                case "PG":
                                    Image img = new Image
                                    {
                                        StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID,
                                        HeightRequest = 0,
                                        WidthRequest = 0
                                    };
                                    Button btn_Photo = new Button();
                                    btn_Photo.StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;
                                    if (Functions.IsEditEntity)
                                    {
                                        RightLyout.Children.Add(img);
                                        btn_Photo.Text = "Add Photo(s)";
                                        btn_Photo.Clicked += Btn_Photo_Clicked;
                                        RightLyout.Children.Add(btn_Photo);
                                    }

                                    if (!fieldLeveSecurity_Create)
                                    {
                                        img.IsEnabled = false;
                                        btn_Photo.IsEnabled = false;
                                    }
                                    break;
                                #endregion

                                default:
                                    Label n = new Label();
                                    n.FontSize = 16;
                                    n.Text = "Not Added yet";
                                    n.FontFamily = "Soin Sans Neue";
                                    RightLyout.Children.Add(n);
                                    break;
                            }

                            Mainlayout.Children.Add(RightLyout);
                            TextFieldsLayout.Children.Add(Mainlayout);
                        }

                        var layout = new StackLayout();
                        layout.Orientation = StackOrientation.Horizontal;
                        layout.Margin = new Thickness(10, 0, 0, 0);

                        var layout1 = new StackLayout();
                        layout1.HorizontalOptions = LayoutOptions.Start;

                        layout1.WidthRequest = 200;

                        var Label11 = new Label
                        { VerticalOptions = LayoutOptions.Start };
                        Label11.Text = "Notes";
                        Label11.HorizontalOptions = LayoutOptions.Start;
                        Label11.FontSize = 16;
                        Label11.FontFamily = "Soin Sans Neue";

                        var layout14 = new StackLayout();
                        layout14.Orientation = StackOrientation.Vertical;
                        layout14.HorizontalOptions = LayoutOptions.Start;
                        layout14.WidthRequest = 200;

                        txt_EntNotes.VerticalOptions = LayoutOptions.Start;
                        txt_EntNotes.WidthRequest = 200;
                        txt_EntNotes.HeightRequest = 100;
                        txt_EntNotes.FontSize = 16;
                        txt_EntNotes.HorizontalOptions = LayoutOptions.Start;
                        txt_EntNotes.BorderColor = Color.LightGray;
                        txt_EntNotes.BorderWidth = 1;
                        txt_EntNotes.CornerRadius = 5;
                        txt_EntNotes.FontFamily = "Soin Sans Neue";
                        layout14.Children.Add(txt_EntNotes);

                        if (Functions.IsEditEntity)
                        {
                            var btn_addnotes = new Button
                            {
                                Text = "Add Notes",
                                FontSize = 16,
                                BackgroundColor = Color.Transparent
                            };
                            btn_addnotes.Clicked += Btn_addnotes_Clicked;
                            btn_addnotes.FontFamily = "Soin Sans Neue";
                            layout14.Children.Add(btn_addnotes);
                        }
                        layout1.Children.Add(Label11);
                        layout.Children.Add(layout1);
                        layout.Children.Add(layout14);

                        TextFieldsLayout.Children.Add(layout);
                    }
                    else
                    {
                        if (!Functions.IsEditEntity)
                            await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    }

                    if (Functions.IsEditEntity)
                    {
                        #region Get Entity By EntityID
                        if (EntityListsValues != null)
                        {
                            EntityListsValues.AssociationFieldCollection = EntityListsValues.AssociationFieldCollection.OrderBy(x => x.ItemDesktopPriorityValue)?.ToList();

                            for (int i = 0; i < EntityListsValues.AssociationFieldCollection.Count; i++)
                            {
                                string _field_type = EntityListsValues.AssociationFieldCollection[i].FieldType;

                                var cnt = FindEntityControl(_field_type + "_" + EntityListsValues.AssociationFieldCollection[i].AssocTypeID);
                                if (cnt != null)
                                {
                                    var cnt_type = cnt.GetType();
                                    var date_pick = new DatePicker();
                                    date_pick.TextColor = Color.Gray;
                                    var ent = new Entry();
                                    ent.FontFamily = "Soin Sans Neue";
                                    ent.FontSize = 16;
                                    var edit = new BorderEditor();
                                    edit.FontFamily = "Soin Sans Neue";
                                    edit.FontSize = 16;
                                    var _checkbox = new Switch();
                                    var pick_Ext_datasrc = new Picker();
                                    pick_Ext_datasrc.TextColor = Color.Gray;
                                    var lbl = new Label();
                                    lbl.FontSize = 16;
                                    lbl.FontFamily = "Soin Sans Neue";
                                    var img = new Image();

                                    if (cnt_type.Name.ToLower() == "datepicker")
                                    {
                                        date_pick = (DatePicker)cnt;
                                        var data = EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText;
                                        if (data.Count != 0)
                                            if (data[0].TextValue != "")
                                            {
                                                try
                                                {
                                                    string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                                    var Str = App.DateFormatStringToString(data[0].TextValue);
                                                    DateTime dt = Convert.ToDateTime(Str);
                                                    date_pick.Date = dt;
                                                }
                                                catch (Exception)
                                                {
                                                }
                                            }
                                            else
                                                date_pick.Date = DateTime.Now;
                                    }
                                    else if (cnt_type.Name.ToLower() == "entry")
                                    {
                                        ent = (Entry)cnt;
                                        var data = EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText;
                                        if (data.Count != 0)
                                            ent.Text = data[0].TextValue;
                                    }
                                    else if (cnt_type.Name.ToLower() == "label")
                                    {
                                        lbl = (Label)cnt;
                                        var data = EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText;
                                        if (data.Count != 0)
                                        {
                                            for (int j = 0; j < data.Count(); j++)
                                            {
                                                if (j < 1)
                                                    lbl.Text = data[j].TextValue;
                                                else
                                                    lbl.Text += Environment.NewLine + data[j].TextValue;
                                            }
                                        }
                                    }
                                    else if (cnt_type.Name.ToLower() == "bordereditor")
                                    {
                                        edit = (BorderEditor)cnt;
                                        var data = EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText;
                                        if (data.Count != 0)
                                            edit.Text = data[0].TextValue;
                                    }
                                    else if (cnt_type.Name.ToLower() == "switch")
                                    {
                                        _checkbox = (Switch)cnt;
                                        var data = EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText;
                                        if (data.Count != 0)
                                        {
                                            if (data[0].TextValue.ToLower() == "y")
                                                _checkbox.IsToggled = true;
                                            else
                                                _checkbox.IsToggled = false;
                                        }
                                    }
                                    else if (cnt_type.Name.ToLower() == "picker")
                                    {
                                        var data = EntityListsValues.AssociationFieldCollection[i].AssocMetaData;
                                        int j = 0;
                                        pick_Ext_datasrc = (Picker)cnt;
                                        List<EXTERNAL_DATASOURCE1> src = pick_Ext_datasrc.ItemsSource as List<EXTERNAL_DATASOURCE1>;
                                        if (data.Count != 0)
                                        {
                                            var records = src.Where(v => v.ID == (string.IsNullOrEmpty(Convert.ToString(data[0]?.ExternalDatasourceObjectID)) ? 0 : Convert.ToInt32(data[0]?.ExternalDatasourceObjectID))).FirstOrDefault();
                                            j = src.FindIndex(v => v.ID == records?.ID);
                                        }

                                        pick_Ext_datasrc.SelectedIndex = j;
                                    }
                                    else if (cnt_type.Name.ToLower() == "image")
                                    {
                                        img = (Image)cnt;
                                        string styleid = img.StyleId;
                                        var stack = (StackLayout)img.Parent;
                                        if (EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText.Count > 0)
                                            stack.Children.Remove(img);

                                        for (int k = 0; k < EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText.Count; k++)
                                        {
                                            if (EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText[k].EntityFileID != null)
                                            {
                                                Image im = new Image();
                                                im.HeightRequest = 150;
                                                im.WidthRequest = 150;
                                                im.StyleId = _field_type + "_" + EntityListsValues.AssociationFieldCollection[i].AssocTypeID + "|" + (k + 1) + "|" + EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText[k]?.EntityFileID ?? "0";

                                                im.Source = ImageSource.FromUri(new Uri("http://entities-s-15.boxerproperty.com/Download.aspx?FileID=" + EntityListsValues.AssociationFieldCollection[i].AssocMetaDataText[k]?.EntityFileID));
                                                stack.Children.Add(im);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        DisplayAlert(_field_type, "Not Found", "cancel");
                                        //http://entities-s-15.boxerproperty.com/EntityList.aspx?EntityTypeID=1050
                                    }
                                }
                            }
                        }
                        else
                        {
                            await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                        }
                        #endregion

                        #region Get Entity notes
                        if (Entity_NotesLists != null)
                        {
                            ObservableCollection<EntityNotesGroup> Temp = new ObservableCollection<EntityNotesGroup>();
                            if (Entity_NotesLists != null)
                            {
                                for (int i = 0; i < Entity_NotesLists.Count; i++)
                                {
                                    string st = App.DateFormatStringToString(Entity_NotesLists[i].CreatedDatetime);
                                    Temp.Add(new EntityNotesGroup("", st.ToString(), Entity_NotesLists[i].CreatedBy)
                                    {
                                        new Entity_Notes { Note = Entity_NotesLists[i].Note }
                                    });
                                }

                                foreach (var item in Temp)
                                {
                                    if (item.FirstOrDefault().Note.Contains("<img"))
                                    {
                                        item.FirstOrDefault().ImageVisible = true;
                                        item.FirstOrDefault().LabelVisible = true;
                                        item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                                        item.FirstOrDefault().ImageURL = App.EntityImgURL + "/" + Functions.HTMLToText(item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1]);
                                        item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                                    }
                                    else
                                    {
                                        item.FirstOrDefault().ImageVisible = false;
                                        item.FirstOrDefault().LabelVisible = true;
                                        item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                                        item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                                    }
                                    NotesGroups.Add(item);
                                }
                            }
                            gridEntitynotes.ItemsSource = NotesGroups;
                        }
                        #endregion
                    }
                    else
                    {
                        Grd_Entity_userDetails.IsVisible = false;
                        line_NotedHead.IsVisible = false;
                    }
                    gridEntitynotes.HeightRequest = NotesGroups.Count <= 0 ? 0 : 700;
                }
                else
                {
                    if (!Functions.IsEditEntity)
                        DisplayAlert("Alert!", "Insufficient rights to create entity items", "Ok");
                    else
                        DisplayAlert("Alert!", "You do not have permission to view this page.", "Ok");
                    await Navigation.PopAsync();
                }

                #endregion

                if (Functions.IsEditEntity)
                    CommonConstants.UpdateEDSInfoList_Entity(App.Isonline, EntitySchemaLists, Convert.ToInt32(_selected.EntityTypeID), EntityInstance, Entity_Category_TypeDetails, INSTANCE_USER_ASSOC_ID, App.DBPath);
            }
            catch (Exception ex)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void Pk_Unfocused(object sender, FocusEventArgs e)
        {
            Pk_SelectedIndexChanged(sender, e);
        }

        #region Add Photo Assoc Wise
        private async void Btn_Photo_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            var action = await this.DisplayActionSheet(null, "Cancel", null, "From Photo Gallery", "Take Photo");
            switch (action)
            {
                case "From Photo Gallery":
                    OpenGallery(btn.StyleId);
                    break;

                case "Take Photo":
                    TakePhoto(btn.StyleId);
                    break;

            }
        }

        async void OpenGallery(string CurrentStyleID)
        {
            //if (Device.RuntimePlatform != Device.Android)
            {
                try
                {
                    string Current_Assoc = CurrentStyleID.Split('_')[1];

                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        DisplayAlert("No Gallery", ":( No Gallery available.", "Ok");
                        return;
                    }

                    var file = await CrossMedia.Current.PickPhotoAsync();

                    if (file == null)
                    {
                        return;
                    }

                    //DisplayAlert("File Location", file.Path, "OK");

                    long size = file.Path.Length;
                    byte[] fileBytes = null;
                    var bytesStream = file.GetStream();
                    using (var memoryStream = new MemoryStream())
                    {
                        bytesStream.CopyTo(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }


                    AddPhotoGallaryOn_AssocField(Current_Assoc, file, fileBytes.Count(), fileBytes);


                    try
                    {
                        if (Device.RuntimePlatform == Device.UWP)
                        {
                            await (await FileSystem.Current.LocalStorage.GetFileAsync(file.Path.Substring(file.Path.LastIndexOf('\\') + 1))).DeleteAsync();
                        }
                        else
                        {
                            await (await FileSystem.Current.LocalStorage.GetFileAsync(file.Path.Substring(file.Path.LastIndexOf('/') + 1))).DeleteAsync();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        async void TakePhoto(string CurrentStyleID)
        {
            // if (Device.RuntimePlatform != Device.Android)
            {
                try
                {
                    string Current_Assoc = CurrentStyleID.Split('_')[1];

                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                    });

                    if (file == null)
                        return;

                    long size = file.Path.Length;
                    byte[] fileBytes = null;
                    var bytesStream = file.GetStream();
                    using (var memoryStream = new MemoryStream())
                    {
                        bytesStream.CopyTo(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }

                    AddPhotoGallaryOn_AssocField(Current_Assoc, file, fileBytes.Count(), fileBytes);
                }
                catch (Exception ex)
                {
                }

                try
                {
                    IFolder rootFolder = FileSystem.Current.LocalStorage;
                    await (await (rootFolder.GetFolderAsync("Sample"))).DeleteAsync();
                }
                catch (Exception)
                {
                }
            }
        }

        private void AddPhotoGallaryOn_AssocField(string Current_Assoc, Plugin.Media.Abstractions.MediaFile file, long size, byte[] fileBytes)
        {
            try
            {
                string File_Name = string.Empty;

                if (Device.RuntimePlatform == Device.UWP)
                {
                    File_Name = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                }
                else
                {
                    File_Name = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
                }

                AddFileToEntityAssocTypeRequest addfile = new AddFileToEntityAssocTypeRequest();
                addfile.ENTITY_ID = EntitySchemaLists._NEntityID;
                addfile.DESCRIPTION = "";
                addfile.FILE_DATE_TIME = DateTime.Now;
                addfile.ENTITY_ASSOC_TYPE_ID = Convert.ToInt32(Current_Assoc);
                addfile.FILE_NAME = File_Name;
                addfile.FILE_SIZE_BYTES = Convert.ToInt32(size);
                addfile.FILE_BLOB = fileBytes;
                addfile.EXTERNAL_URI = "";
                addfile.SHOW_INLINE_NOTES = 'Y';
                addfile.SYSTEM_CODE = "";
                addfile.IS_ACTIVE = 'Y';
                addfile.CREATED_BY = Functions.UserName;

                var AttachmentAPi = EntityAPIMethods.AddFileToEntityAssocType(addfile);

                var response = AttachmentAPi.GetValue("ResponseContent");

                if (!string.IsNullOrEmpty(response?.ToString()) && response.ToString() != "[]")
                {
                    for (int i = 0; i < EntitySchemaLists.AssociationFieldCollection.Count; i++)
                    {
                        var _field_type = EntitySchemaLists.AssociationFieldCollection[i].FieldType;

                        var cnt = FindEntityControl(_field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID);
                        if (cnt != null)
                        {
                            var cnt_type = cnt.GetType();
                            if (cnt_type.Name.ToLower() == "image")
                            {
                                Image img = (Image)cnt;

                                string styleid = img.StyleId;
                                var stack = (StackLayout)img.Parent;

                                if (EntitySchemaLists.AssociationFieldCollection[i].AssocMetaDataText.Count == 0)
                                {
                                    if (!styleid.Contains("|"))
                                        stack.Children.Remove(img);
                                }

                                int imgCon = 0;

                                foreach (var item in stack.Children)
                                {
                                    var type = item.GetType();
                                    if (type.Name.ToLower() == "image")
                                    {
                                        imgCon++;
                                    }
                                }

                                Image im = new Image();
                                im.HeightRequest = 150;
                                im.WidthRequest = 150;
                                im.StyleId = _field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID + "|" + (imgCon + 1) + "|" + response.ToString();

                                im.Source = ImageSource.FromUri(new Uri("http://entities-s-15.boxerproperty.com/Download.aspx?FileID=" + response.ToString()));
                                stack.Children.Add(im);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        #endregion


        #region Set Calculation For CalControls
        public string SetCalControls(List<AssociationField> sControls, List<AssociationField> sControlscal, string Id)
        {
            calculationsFieldlist = new List<KeyValuePair<List<AssociationField>, string>>();
            string sControl = string.Empty;
            KeyValuePair<List<AssociationField>, string> Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);

            foreach (var item in sControls)
            {
                foreach (var itm in sControlscal)
                {
                    string strCalformula = itm.CalculationFormula;
                    if (!string.IsNullOrEmpty(strCalformula))
                    {
                        if (!string.IsNullOrEmpty(itm.AssocSystemCode))
                        {
                            if (strCalformula.Contains(Convert.ToString(itm?.AssocSystemCode)) || strCalformula.Contains(Convert.ToString(itm.AssocName ?? "null")) || strCalformula.Contains(Convert.ToString(strCalformula ?? "null")))
                            {
                                sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                calculationsFieldlist.Add(Itemcontrols);
                            }
                        }
                        else
                        {
                            if (strCalformula.Contains(Convert.ToString(item.AssocName ?? "")) || strCalformula.Contains(Convert.ToString(item.AssocName ?? "")))
                            {
                                sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                calculationsFieldlist.Add(Itemcontrols);
                            }
                        }
                    }
                }
            }
            return sControl;
        }
        public string SetCalControlsForExd(List<AssociationField> sControls, List<AssociationField> sControlscal, string Id, string selectedValue = "")
        {
            calculationsFieldlist = new List<KeyValuePair<List<AssociationField>, string>>();
            string sControl = string.Empty;
            KeyValuePair<List<AssociationField>, string> Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
            foreach (var item in sControls)
            {
                if (item?.ExternalDataSourceID != null)
                {
                    var result = EntityAPIMethods.GetExternalDataSourceByID(Convert.ToString(item.ExternalDataSourceID));
                    var temp = result.GetValue("ResponseContent");

                    if (temp != null)
                    {
                        ExternalDatasource exd = JsonConvert.DeserializeObject<ExternalDatasource>(temp.ToString());
                        if (exd != null)
                        {
                            foreach (var itm in sControlscal)
                            {
                                string strCalformula = itm.CalculationFormula;
                                if (!string.IsNullOrEmpty(strCalformula))
                                {
                                    if (!string.IsNullOrEmpty(itm.AssocSystemCode))
                                    {
                                        if (strCalformula.Contains(Convert.ToString(exd?.DataSourceName)) || strCalformula.Contains(Convert.ToString(itm?.AssocSystemCode)) || strCalformula.Contains(Convert.ToString(itm.AssocName ?? "null")))
                                        {
                                            sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                            Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                            calculationsFieldlist.Add(Itemcontrols);
                                            lstcalculationsFieldlist.Add(new Tuple<List<AssociationField>, string, string>(sControls, sControl, selectedValue));
                                        }
                                        else if (strCalformula.Contains(Convert.ToString(item.AssocName == null ? "null" : item.AssocName)))
                                        {
                                            sControl = "assoc_" + itm.FieldType + "_" + itm.AssocTypeID;
                                            Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                            calculationsFieldlist.Add(Itemcontrols);
                                            lstcalculationsFieldlist.Add(new Tuple<List<AssociationField>, string, string>(sControls, sControl, selectedValue));
                                        }
                                    }
                                    else
                                    {
                                        if (strCalformula.Contains(Convert.ToString(exd?.DataSourceName)) || strCalformula.Contains(Convert.ToString(itm.AssocName == null ? "null" : itm.AssocName)))
                                        {
                                            sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                            Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                            calculationsFieldlist.Add(Itemcontrols);
                                            lstcalculationsFieldlist.Add(new Tuple<List<AssociationField>, string, string>(sControls, sControl, selectedValue));
                                        }
                                        else if (!string.IsNullOrEmpty(item.AssocSystemCode))
                                        {
                                            if (strCalformula.Contains(Convert.ToString(exd?.DataSourceName)) || strCalformula.Contains(Convert.ToString(item?.AssocSystemCode)) || strCalformula.Contains(Convert.ToString(item.AssocName ?? "null")))
                                            {
                                                sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                                Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<AssociationField>, string, string>(sControls, sControl, selectedValue));
                                            }
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    foreach (var itm in sControlscal)
                    {
                        string strCalformula = itm.CalculationFormula;
                        if (!string.IsNullOrEmpty(strCalformula))
                        {
                            if (!string.IsNullOrEmpty(itm.AssocSystemCode))
                            {
                                if (strCalformula.Contains(Convert.ToString(itm?.AssocSystemCode)) || strCalformula.Contains(Convert.ToString(itm.AssocName ?? "null")) || strCalformula.Contains(Convert.ToString(itm.CalculationFormula ?? "null")))
                                {
                                    sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                    Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                    calculationsFieldlist.Add(Itemcontrols);
                                }
                            }
                            else
                            {
                                if (strCalformula.Contains(Convert.ToString(item.AssocName ?? "null")) || strCalformula.Contains(Convert.ToString(item.AssocName ?? "null")))
                                {
                                    sControl = itm.FieldType + "_" + itm.AssocTypeID;
                                    Itemcontrols = new KeyValuePair<List<AssociationField>, string>(sControls, sControl);
                                    calculationsFieldlist.Add(Itemcontrols);
                                }
                            }
                        }

                    }

                }
            }
            return sControl;
        }

        public void SetDictionary(Dictionary<string, string> assocFieldValues, Dictionary<string, string> assocFieldTexts, string CurrentStyleId, string selectedId, string selectedname, int AssocTypeID)
        {
            if (!assocFieldValues.ContainsKey(CurrentStyleId.Split('|')[0].ToUpper()))
            {
                assocFieldValues.Add(CurrentStyleId.Split('|')[0].ToUpper(), AssocTypeID + "|" + selectedId);
                assocFieldTexts.Add(CurrentStyleId.Split('|')[0].ToUpper(), selectedname);
            }
            else
            {
                assocFieldValues[CurrentStyleId.Split('|')[0].ToUpper()] = AssocTypeID + "|" + selectedId;
                assocFieldTexts[CurrentStyleId.Split('|')[0].ToUpper()] = selectedname;
            }
        }
        public void DyanmicSetCalc(string CurrentStyleId, List<AssociationField> EntitySchemaListsAssociationFieldCollection = null)
        {
            try
            {
                //string CalTxtbox = string.Empty;
                object cntrl = new object();
                string selectedId = string.Empty;
                string selectedname = string.Empty;
                if (App.Isonline)
                {
                    var dds = EntitySchemaListsAssociationFieldCollection.Where(v => v.FieldType != "EL" && v.FieldType != "ME" && v.FieldType != "MS" && v.FieldType != "MT" && v.FieldType != "SE" && v.FieldType != "SS" && v.FieldType != "MF" && v.FieldType != "SF" && v.FieldType != "PG" && v.FieldType != "LN" && v.FieldType != "CB");

                    foreach (var item in dds)
                    {
                        var subitem = FindEntityControl(CurrentStyleId);

                        if (subitem != null)
                        {
                            Type ty = subitem.GetType();
                            if (ty.Name.ToLower() != "stacklayout")
                            {
                                if (ty.Name.ToLower() == "picker")
                                {
                                    var picker = (Picker)subitem;
                                    picker.TextColor = Color.Gray;
                                    if (picker.StyleId == CurrentStyleId)
                                    {
                                        cntrl = subitem;
                                        selectedId = Convert.ToString((picker.SelectedItem as EXTERNAL_DATASOURCE1).ID);
                                        selectedname = Convert.ToString((picker.SelectedItem as EXTERNAL_DATASOURCE1).EXTERNAL_DATASOURCE_NAME);
                                        SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.ToUpper(), selectedId, selectedname, item.AssocTypeID);
                                    }
                                }
                                else if (ty.Name.ToLower() == "entry")
                                {
                                    var en = (Entry)subitem;
                                    en.FontFamily = "Soin Sans Neue";
                                    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                    {
                                        if (en.StyleId == CurrentStyleId)
                                        {
                                            SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.ToUpper(), Convert.ToString(CurrentStyleId), en.Text, item.AssocTypeID);
                                            cntrl = subitem;
                                        }
                                    }
                                }
                                else if (ty.Name.ToLower() == "datepicker")
                                {
                                    var en = (DatePicker)subitem;
                                    en.TextColor = Color.Gray;
                                    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                    {
                                        if (en.StyleId == CurrentStyleId)
                                        {

                                            SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.ToUpper(), Convert.ToString(CurrentStyleId), App.DateFormatStringToString(en.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"), item.AssocTypeID);
                                            cntrl = subitem;
                                        }
                                    }
                                }
                            }

                            var x = cntrl.GetType();
                            if (x.Name == "Picker")
                            {
                                CalTxtbox = SetCalControlsForExd(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0], selectedId + "|" + selectedname);
                            }
                            else if (x.Name == "Entry")
                            {
                                CalTxtbox = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                            }
                            else if (x.Name == "DatePicker")
                            {

                                CalTxtbox = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                            }

                        }

                        if (!string.IsNullOrEmpty(CalTxtbox))
                        {
                            int cnt = 0;
                            string sCalId = string.Empty;
                            foreach (var itm in calculationsFieldlist)
                            {
                                sCalId = itm.Value?.ToString();
                                cnt++;
                                if (cnt > calculationsFieldlist.Count)
                                    return;
                                DataTypes.DataType.Cases.RefreshCalculationFieldsRequest rf = new DataTypes.DataType.Cases.RefreshCalculationFieldsRequest();

                                rf.assocFieldCollection = JsonConvert.SerializeObject(EntitySchemaLists.AssociationFieldCollection);
                                rf.assocFieldValues = assocFieldValues;
                                rf.assocFieldTexts = assocFieldTexts;
                                rf.calculatedAssocId = sCalId;
                                rf.sdateFormats = "MM/dd/yyyy";
                                JToken CalResult = null;

                                var Result = EntityAPIMethods.RefreshCalculationFields(rf);
                                CalResult = Result.GetValue("ResponseContent");

                                var subitem1 = FindEntityControl(sCalId);

                                Type typ = subitem1.GetType();

                                if (typ.Name.ToLower() != "stacklayout")
                                {
                                    if (typ.Name.ToLower() == "entry")
                                    {
                                        var en = (Entry)subitem1;
                                        if (en.StyleId == Convert.ToString(sCalId))
                                        {
                                            en.Text = Convert.ToString(CalResult);

                                            //return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // throw ex;
            }
        }

        public void DyanmicSetCalcexd(string CurrentStyleId, List<AssociationField> EntitySchemaListsAssociationFieldCollection = null)
        {
            try
            {
                //string CalTxtbox = string.Empty;
                object cntrl = new object();
                string selectedId = string.Empty;
                string selectedname = string.Empty;
                if (App.Isonline)
                {
                    foreach (var item in EntitySchemaListsAssociationFieldCollection)
                    {
                        var subitem = FindEntityControl(CurrentStyleId);

                        if (subitem != null)
                        {
                            Type ty = subitem.GetType();
                            if (ty.Name.ToLower() != "stacklayout")
                            {
                                if (ty.Name.ToLower() == "picker")
                                {
                                    var picker = (Picker)subitem;
                                    picker.TextColor = Color.Gray;
                                    if (picker.StyleId == CurrentStyleId)
                                    {
                                        cntrl = subitem;
                                        selectedId = Convert.ToString((picker.SelectedItem as EXTERNAL_DATASOURCE1).ID);
                                        selectedname = Convert.ToString((picker.SelectedItem as EXTERNAL_DATASOURCE1).EXTERNAL_DATASOURCE_NAME);
                                        SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.ToUpper(), selectedId, selectedname, item.AssocTypeID);
                                    }
                                }
                            }

                            var x = cntrl.GetType();
                            if (x.Name == "Picker")
                            {
                                CalTxtbox = SetCalControlsForExd(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0]))?.ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL")?.ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0], (selectedId == "-1" ? "0|" + selectedname : selectedId + "|" + selectedname));
                            }
                            else if (x.Name == "Entry")
                            {
                                CalTxtbox = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0]))?.ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL")?.ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                            }
                            else if (x.Name == "DatePicker")
                            {
                                CalTxtbox = SetCalControls(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), EntitySchemaLists.AssociationFieldCollection.Where(v => v.FieldType == "CL").ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                            }
                        }

                        if (!string.IsNullOrEmpty(CalTxtbox))
                        {
                            int cnt = 0;
                            string sCalId = string.Empty;
                            foreach (var itm in calculationsFieldlist)
                            {
                                sCalId = itm.Value?.ToString();
                                cnt++;
                                if (cnt > calculationsFieldlist.Count)
                                    return;
                                DataTypes.DataType.Cases.RefreshCalculationFieldsRequest rf = new DataTypes.DataType.Cases.RefreshCalculationFieldsRequest();

                                rf.assocFieldCollection = JsonConvert.SerializeObject(EntitySchemaLists.AssociationFieldCollection);
                                rf.assocFieldValues = assocFieldValues;
                                rf.assocFieldTexts = assocFieldTexts;
                                rf.calculatedAssocId = sCalId;
                                rf.sdateFormats = "MM/dd/yyyy";
                                JToken CalResult = null;

                                var Result = EntityAPIMethods.RefreshCalculationFields(rf);
                                CalResult = Result.GetValue("ResponseContent");

                                var subitem1 = FindEntityControl(sCalId);

                                Type typ = subitem1?.GetType();

                                if (typ?.Name?.ToLower() != "stacklayout")
                                {
                                    if (typ?.Name?.ToLower() == "entry")
                                    {
                                        var en = (Entry)subitem1;
                                        en.FontFamily = "Soin Sans Neue";
                                        if (en.StyleId == Convert.ToString(sCalId))
                                        {
                                            en.Text = Convert.ToString(CalResult);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // throw ex;
            }
        }
        #endregion

        Dictionary<string, string> assocFieldValues = new Dictionary<string, string>();
        Dictionary<string, string> assocFieldTexts = new Dictionary<string, string>();
        private void DO_DateSelected(object sender, DateChangedEventArgs e)
        {
            DyanmicSetCalc(((DatePicker)sender).StyleId, EntitySchemaLists.AssociationFieldCollection);
        }
        private void ST_Unfocused(object sender, FocusEventArgs e)
        {
            DyanmicSetCalc(((Entry)sender).StyleId, EntitySchemaLists.AssociationFieldCollection);

        }
        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (App.Isonline)
                {
                    var pk = (Picker)sender;
                    pk.TextColor = Color.Gray;
                    int AssocID = Convert.ToInt32(pk.StyleId?.Split('_')[1]);
                    ClearEntityChildControls(Convert.ToInt32(pk.StyleId.Split('_')[1]));

                    if (pk.ItemsSource != null)
                    {
                        if (pk.ItemsSource.Count > 0)
                        {
                            if (pk.SelectedItem != null)
                            {
                                if ((pk.SelectedItem as EXTERNAL_DATASOURCE1).ID > 0)
                                {
                                    EXTERNAL_DATASOURCE1 ls = pk.SelectedItem as EXTERNAL_DATASOURCE1;
                                    var CalculatedAssocs = EntitySchemaLists.AssociationFieldCollection.Where(x => x.FieldType == "CL" || x.FieldType == "CA")?.ToList();

                                    //CalTxtbox = SetCalControlsForExd(EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(pk.StyleId?.Split('_')[1]))?.ToList(), CalculatedAssocs, pk.StyleId?.Split('_')[0]?.ToUpper() + "_" + pk.StyleId?.Split('_')[1]);
                                    //if (!string.IsNullOrEmpty(CalTxtbox))
                                    {
                                        if (ls != null && ls?.ID > 0)
                                        {
                                            DyanmicSetCalcexd(((Picker)sender).StyleId, EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == Convert.ToInt32(pk.StyleId?.Split('_')[1]))?.ToList());
                                        }

                                        var assc = EntitySchemaLists.AssociationFieldCollection.Where(v => v.AssocTypeID == AssocID).FirstOrDefault();
                                        var AssocTypeCascades = assc.EntityAssocTypeCascade.ToList();
                                        if (lstcalculationsFieldlist.Count > 0)
                                        {
                                            if (ls?.EXTERNAL_DATASOURCE_NAME.ToLower() == "-- select item --")
                                            {
                                                int aId = 0;
                                                if (AssocTypeCascades.Where(v => v.EntityAssocTypeIDParent == AssocID)?.Count() > 0)
                                                    aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v.EntityAssocTypeIDParent == AssocID));
                                                else if (AssocTypeCascades.Where(v => v.EntityAssocTypeIDChild == AssocID)?.Count() > 0)
                                                    aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v.EntityAssocTypeIDChild == AssocID));
                                                //aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v._CASE_ASSOC_TYPE_ID_CHILD == AssocID));
                                                for (int i = aId; i < AssocTypeCascades.Count; i++)
                                                {
                                                    var av = lstcalculationsFieldlist.Distinct().Where(v => v.Item1.FirstOrDefault().AssocTypeID == AssocID);
                                                    foreach (var ii in av)
                                                    {

                                                        //string str = (FindControls(ii.Item2) as Entry).Text;
                                                        foreach (StackLayout v in TextFieldsLayout.Children)
                                                        {
                                                            foreach (StackLayout item in v.Children)
                                                            {
                                                                foreach (var subitem in item.Children)
                                                                {
                                                                    var xy = subitem;
                                                                    Type ty = xy.GetType();
                                                                    if (ty.Name.ToLower() != "stacklayout")
                                                                    {
                                                                        if (ty.Name.ToLower() == "entry")
                                                                        {
                                                                            var en = (Entry)xy;
                                                                            if (en.StyleId == Convert.ToString((ii.Item2.ToLower().Substring((ii.Item2.ToLower().IndexOf('_') + 1)).ToLower())))
                                                                            {
                                                                                int id = Convert.ToInt32(ii.Item2.Replace("CL_", ""));
                                                                                var calinfo = EntitySchemaLists.AssociationFieldCollection.Where(a => a.FieldType == "CL" && a.AssocTypeID == id)?.FirstOrDefault();
                                                                                if (!string.IsNullOrEmpty(calinfo.CalculationFormula))
                                                                                {
                                                                                    string avName = ii.Item1.FirstOrDefault().AssocName;
                                                                                    string avSyscode = ii.Item1.FirstOrDefault().AssocSystemCode;

                                                                                    if (calinfo.CalculationFormula.IndexOf(avName) != -1)
                                                                                    {
                                                                                        string str = calinfo.CalculationFormula.Substring(calinfo.CalculationFormula.IndexOf(avName) + 1 + avName.Length)?.Split('}')[0];
                                                                                        if (str == "ID")
                                                                                            en.Text = en.Text.Replace(ii.Item3.Split('|')[0], "");
                                                                                        else
                                                                                            en.Text = en.Text.Replace(ii.Item3.Split('|')[1], "");
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        string str1 = calinfo.CalculationFormula.Substring(calinfo.CalculationFormula.IndexOf(avSyscode) + 1 + avSyscode.Length)?.Split('}')[0];
                                                                                        if (str1 == "ID")
                                                                                            en.Text = en.Text.Replace(ii.Item3.Split('|')[0], "");
                                                                                        else
                                                                                            en.Text = en.Text.Replace(ii.Item3.Split('|')[1], "");
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FillEntityChildControl(Convert.ToInt32(pk.StyleId.Split('_')[1]), ls.ID.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void Btn_addnotes_Clicked(object sender, EventArgs e)
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                string recID = "";
                if (!string.IsNullOrEmpty(txt_EntNotes.Text))
                {
                    await Task.Run(async () =>
                     {
                         recID = await EntitySyncAPIMethods.StoreEntityNotes(App.Isonline, mbView.EntityDetails.EntityTypeID, mbView.EntityDetails.EntityID.ToString(), txt_EntNotes.Text, Functions.UserName, "Notes Type ID is Static", (Enum.GetNames(typeof(ActionTypes)))[1], App.DBPath, Functions.UserFullName);
                     });
                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                    if (!string.IsNullOrEmpty(recID?.ToString()) && recID.ToString() != "[]" && recID != "0")
                    {
                        if (NotesGroups.Count != 0)
                        {
                            NotesGroups.Insert(0, new EntityNotesGroup("", DateTime.Now.ToString(), Functions.UserFullName)
                            {
                                new Entity_Notes { Note = txt_EntNotes.Text ,ImageVisible = false, LabelVisible = true }
                            });
                        }
                        else
                        {
                            NotesGroups.Add(new EntityNotesGroup("", DateTime.Now.ToString(), Functions.UserFullName)
                            {
                                 new Entity_Notes { Note = txt_EntNotes.Text ,ImageVisible = false, LabelVisible = true }
                            });

                        }
                        gridEntitynotes.ItemsSource = NotesGroups;

                        txt_EntNotes.Text = "";
                        FormattedString s = new FormattedString();
                        if (mbView.EntityDetails.EntityModifiedByFullName != null)
                        {
                            s.Spans.Add(new Span { Text = Functions.UserFullName + "\r\n", FontSize = 14 });
                            s.Spans.Add(new Span { Text = (DateTime.Now).ToString(), FontSize = 14 });
                        }
                        lbl_modifiedname.FormattedText = s;
                    }
                }
                else
                {
                    DisplayAlert(null, "Please enter notes.", "Ok");
                }
            }
            catch (Exception ex)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        public object FindEntityControl(string type)
        {
            foreach (StackLayout v in TextFieldsLayout.Children)
            {
                foreach (StackLayout item in v.Children)
                {
                    foreach (var subitem in item.Children)
                    {
                        var xy = subitem.StyleId;
                        if (xy != null)
                        {
                            if (xy.Contains(type))
                            {
                                var d = subitem.GetType();
                                if ((subitem.GetType()).Name.ToLower() != "button")
                                    return subitem;
                            }
                        }
                    }
                }
            }
            return null;
        }
        private async void Tool_Create_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (Functions.IsEditEntity)
                {
                    var action = await DisplayActionSheet(null, "Cancel", null, "Save", "Save & Exit");
                    switch (action)
                    {
                        case "Save":
                            int result = await CreateEntitycall();
                            if (result > 0)
                            {
                                mbView = new EntityListMBView();
                                mbView.EntityDetails = new EntityClass();
                                mbView.EntityDetails.EntityID = result;
                                mbView.EntityDetails.EntityTypeID = EntitySchemaLists.EntityTypeID;
                                mbView.Title = EntitySchemaLists.EntityTypeName;

                                await Navigation.PopAsync();
                                //await Navigation.PushAsync(new Entity_View(mbView));
                            }
                            break;
                        case "Save & Exit":
                            result = await CreateEntitycall();
                            if (result > 0)
                            {
                                EntityOrgCenterList t = new EntityOrgCenterList();
                                t.EntityTypeID = EntitySchemaLists.EntityTypeID;
                                t.EntityTypeName = EntitySchemaLists.EntityTypeName;
                                //await Navigation.PushAsync(new EntityDetailsSubtype(t, null));
                                EntityDetailsSubtype p = new EntityDetailsSubtype(t, null);
                                var s = p.GetType().Name;
                                try
                                {
                                    var existingPages = this.Navigation.NavigationStack.ToList();

                                    if (existingPages.Count > 0)
                                    {
                                        int Counter = 0;
                                        for (int i = 0; i < existingPages.Count; i++)
                                        {
                                            var typ = existingPages[i].GetType().Name;
                                            if (typ == s)
                                            {
                                                Counter = i;
                                                break;
                                            }
                                        }

                                        for (int i = Counter + 1; i <= existingPages.Count; i++)
                                        {
                                            var ph = existingPages[i];
                                            this.Navigation.RemovePage(ph);
                                            var ta = this.Navigation.NavigationStack.ToList();
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                            break;
                    }
                }
                else
                {
                    var action = await DisplayActionSheet(null, "Cancel", null, "Create", "Create & Exit", "Create & New");
                    switch (action)
                    {
                        case "Create":
                            int result = await CreateEntitycall();
                            if (result > 0)
                            {
                                mbView = new EntityListMBView
                                {
                                    EntityDetails = new EntityClass
                                    {
                                        EntityID = result,
                                        EntityTypeID = EntitySchemaLists.EntityTypeID
                                    },
                                    Title = EntitySchemaLists.EntityTypeName
                                };
                                try
                                {
                                    //await Navigation.PushAsync(new Entity_View(mbView));

                                    //var existingPages = this.Navigation.NavigationStack.ToList();
                                    //var ph = existingPages[existingPages.Count - 1];
                                    //this.Navigation.RemovePage(ph);


                                    var existingPages = this.Navigation.NavigationStack.ToList();
                                    // Get the page before Create Entity
                                    //              or
                                    // Get the Navigation Parent Page
                                    var ph = existingPages[existingPages.Count - 2];


                                    if (ph.GetType().Name.ToString().ToLower() == "entity_typedetails")
                                    {
                                        await Navigation.PushAsync(new Entity_View(mbView));
                                        Navigation.RemovePage(ph);// remove entity_typedetails() Page from Queue

                                        existingPages = this.Navigation.NavigationStack.ToList();
                                        var Rph = existingPages[existingPages.Count - 2];
                                        this.Navigation.RemovePage(Rph);//remove only Create entity Page
                                    }
                                    else
                                    {
                                        //var Rph = existingPages[existingPages.Count - 1];
                                        //this.Navigation.RemovePage(Rph);
                                        /// popout to the EntityDetailsSubtype() Page
                                        await Navigation.PopAsync();
                                    }


                                }
                                catch (Exception)
                                {
                                }
                            }
                            break;
                        case "Create & Exit":
                            result = await CreateEntitycall();

                            if (result >= 0)
                            {
                                EntityOrgCenterList t = new EntityOrgCenterList();
                                t.EntityTypeID = EntitySchemaLists.EntityTypeID;
                                t.EntityTypeName = EntitySchemaLists.EntityTypeName;


                                var existingPages = this.Navigation.NavigationStack.ToList();
                                // Get the page before Create Entity
                                //              or
                                // Get the Navigation Parent Page
                                var ph = existingPages[existingPages.Count - 2];


                                if (ph.GetType().Name.ToString().ToLower() == "entity_typedetails")
                                {
                                    await Navigation.PushAsync(new EntityDetailsSubtype(t, null));
                                    Navigation.RemovePage(ph);// remove entity_typedetails() Page from Queue

                                    existingPages = this.Navigation.NavigationStack.ToList();
                                    var Rph = existingPages[existingPages.Count - 2];
                                    this.Navigation.RemovePage(Rph);//remove only Create entity Page
                                }
                                else
                                {
                                    //var Rph = existingPages[existingPages.Count - 1];
                                    //this.Navigation.RemovePage(Rph);
                                    /// popout to the EntityDetailsSubtype() Page
                                    await Navigation.PopAsync();
                                }
                            }
                            break;
                        case "Create & New":
                            result = await CreateEntitycall();

                            if (result >= 0)
                            {
                                Functions.IsEditEntity = false;
                                DesignFormDynamic();

                                //var existingPages = this.Navigation.NavigationStack.ToList();
                                //var ph = existingPages[existingPages.Count - 1];
                                //this.Navigation.RemovePage(ph);
                            }

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            txt_EntNotes.Text = String.Empty;
        }
        public async Task<int> CreateEntitycall()
        {
            int insertedRecordid = -1;
            bool IsRequiredFieldEmpty = false;
            string ReqFieldAssoc = "";
            try
            {
                for (int i = 0; i < EntitySchemaLists.AssociationFieldCollection.Count; i++)
                {
                    var _field_type1 = EntitySchemaLists.AssociationFieldCollection[i].FieldType;

                    var cnt = FindEntityControl(_field_type1 + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID);
                    if (cnt != null)
                    {
                        Type cnt_type = cnt.GetType();
                        var ent = new Entry();
                        ent.FontFamily = "Soin Sans Neue";
                        var editor = new BorderEditor();
                        editor.FontFamily = "Soin Sans Neue";
                        var img = new Image();
                        if (cnt_type.Name.ToLower() == "picker")
                        {
                            if (EntitySchemaLists.AssociationFieldCollection[i].IsRequired == "Y")
                            {
                                if ((((Picker)cnt).SelectedItem as EXTERNAL_DATASOURCE1).ID == 0)
                                {
                                    IsRequiredFieldEmpty = true;
                                    ((Picker)cnt).Focus();
                                    ReqFieldAssoc = EntitySchemaLists.AssociationFieldCollection[i].AssocName;
                                    break;
                                }
                            }
                        }
                        if (cnt_type.Name.ToLower() == "entry")
                        {
                            ent = (Entry)cnt;
                            if (EntitySchemaLists.AssociationFieldCollection[i].IsRequired == "Y")
                            {
                                if (string.IsNullOrEmpty(ent.Text))
                                {
                                    IsRequiredFieldEmpty = true;
                                    ent.Focus();
                                    ReqFieldAssoc = EntitySchemaLists.AssociationFieldCollection[i].AssocName;
                                    break;
                                }
                            }
                        }
                        else if (cnt_type.Name.ToLower() == "bordereditor")
                        {
                            editor = (BorderEditor)cnt;
                            if (EntitySchemaLists.AssociationFieldCollection[i].IsRequired == "Y")
                            {
                                if (string.IsNullOrEmpty(editor.Text))
                                {
                                    IsRequiredFieldEmpty = true;
                                    editor.Focus();
                                    ReqFieldAssoc = EntitySchemaLists.AssociationFieldCollection[i].AssocName;
                                    break;
                                }
                            }

                        }
                        else if (cnt_type.Name.ToLower() == "datepicker")
                        {
                            var dt = (DatePicker)cnt;
                            if (EntitySchemaLists.AssociationFieldCollection[i].IsRequired == "Y")
                            {
                                if (dt.Date == Convert.ToDateTime("01/01/1900"))
                                {
                                    IsRequiredFieldEmpty = true;
                                    dt.Focus();
                                    ReqFieldAssoc = EntitySchemaLists.AssociationFieldCollection[i].AssocName;
                                    break;
                                }
                            }

                        }
                    }
                }

                if (!IsRequiredFieldEmpty)
                {
                    EntityClass entity = new EntityClass()
                    {
                        EntityTypeName = EntitySchemaLists.EntityTypeName,
                        EntityTypeID = Convert.ToInt32(EntitySchemaLists.EntityTypeID),
                        InstanceSingular = EntitySchemaLists.EntityTypeName,
                        InstancePlural = EntitySchemaLists.EntityTypeName,
                        EntityTypeOwener = EntitySchemaLists.EntityTypeOwener,
                        EntityTypeHelpURL = EntitySchemaLists.EntityTypeHelpURL,
                        AssociationFieldCollection = new List<AssociationField>(),
                        EntitiyOwnedByUserName = Functions.UserName,
                        EntityOwnedByDateTime = DateTime.Now.ToString(),
                        EntityOwnedByFullName = Functions.UserFullName,
                        EntityCreatedByFullName = Functions.UserFullName,
                        EntityCreatedDateTime = DateTime.Now.ToString(),
                        EntityCreatedByUserName = Functions.UserName,
                        EntityTypeRelationship = new List<EntityTypeRelationship>(),
                        EntityTypeTemplates = new List<EntityTypeTemplate>(),
                        ScreenConfigurationSettings = new List<EntityTypeScreenConfiguration>()
                    };
                    entity.EntityModifiedByFullName = Functions.UserFullName;
                    entity.EntityModifiedByUserName = Functions.UserName;
                    entity.EntityModifiedDateTime = DateTime.Now.ToString();

                    for (int i = 0; i < EntitySchemaLists.AssociationFieldCollection.Count; i++)
                    {
                        AssociationField eAssociationField = new AssociationField();
                        eAssociationField.AssocMetaData = new List<AssociationMetaData>();
                        eAssociationField.AssocDecode = new List<AssociationFieldValue>();
                        eAssociationField.AssocMetaDataText = new List<AssociationMetaDataText>();
                        var _field_type = EntitySchemaLists.AssociationFieldCollection[i].FieldType;
                        switch (_field_type)
                        {
                            #region SE -- EL -- ME
                            case "SE":
                            case "EL":
                            case "ME":

                                var cnt = FindEntityControl(_field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID);
                                Type cnt_type = cnt.GetType();
                                var pick_Ext_datasrc = new Picker();
                                if (cnt_type.Name.ToLower() == "picker")
                                {
                                    pick_Ext_datasrc = (Picker)cnt;
                                }
                                var pick_value = pick_Ext_datasrc.SelectedItem as EXTERNAL_DATASOURCE1;
                                if (pick_value == null) break;
                                AssociationMetaData Asso_metadata_value = new AssociationMetaData();
                                Asso_metadata_value.AssocTypeID = EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;
                                Asso_metadata_value.ExternalDatasourceObjectID = pick_value.ID.ToString();
                                Asso_metadata_value.FieldValue = pick_value.EXTERNAL_DATASOURCE_NAME;
                                Asso_metadata_value.FieldName = EntitySchemaLists.AssociationFieldCollection[i].AssocName;

                                eAssociationField.AssocMetaData.Add(Asso_metadata_value);
                                break;
                            #endregion

                            #region SS - MS
                            case "SS":
                            case "MS":
                                var cnt1 = FindEntityControl(_field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID);
                                Type cnt_type1 = cnt1.GetType();
                                var pick_assocDeccod = new Picker();
                                if (cnt_type1.Name.ToLower() == "picker")
                                {
                                    pick_assocDeccod = (Picker)cnt1;
                                }

                                var pick_value1 = pick_assocDeccod.SelectedItem as EXTERNAL_DATASOURCE1;
                                AssociationMetaData Asso_metadata_value1 = new AssociationMetaData();
                                Asso_metadata_value1.AssocTypeID = EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;
                                Asso_metadata_value1.ExternalDatasourceObjectID = Convert.ToString(pick_value1.ID);
                                Asso_metadata_value1.AssocDecodeID = pick_value1.ID;
                                Asso_metadata_value1.FieldValue = pick_value1.EXTERNAL_DATASOURCE_NAME;
                                Asso_metadata_value1.FieldName = EntitySchemaLists.AssociationFieldCollection[i].AssocName;

                                eAssociationField.AssocMetaData.Add(Asso_metadata_value1);
                                break;
                            #endregion

                            #region DO - DT - MN - TA - SN - CL - ST - ET - PG - LN - RT - MT - CB - MF - SF
                            case "DO":
                            case "DT":
                            case "MN":
                            case "TA":
                            case "RT":
                            case "MT":
                            case "SN":
                            case "CL":
                            case "ST":
                            case "ET":
                            case "PG":
                            case "LN":
                            case "CB":
                            case "MF":
                            case "SF":
                                AssociationMetaDataText Assoc_metatext = new AssociationMetaDataText();
                                cnt = FindEntityControl(_field_type + "_" + EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID);
                                if (cnt != null)
                                {
                                    cnt_type = cnt.GetType();
                                    var date_pick = new DatePicker();
                                    var ent = new Entry();
                                    ent.FontSize = 16;
                                    var edit = new BorderEditor();
                                    edit.FontSize = 16;
                                    var img = new Image();
                                    var _checkbox = new Switch();
                                    var lbl = new Label();
                                    lbl.FontSize = 16;
                                    if (cnt_type.Name.ToLower() == "datepicker")
                                    {
                                        date_pick = (DatePicker)cnt;
                                        if (date_pick.Date != Convert.ToDateTime("01/01/1900"))
                                        {
                                            Assoc_metatext.TextValue = App.DateFormatStringToString(date_pick.Date.ToString());
                                        }
                                        else
                                        {
                                            Assoc_metatext.TextValue = "";
                                        }
                                    }
                                    else if (cnt_type.Name.ToLower() == "entry")
                                    {
                                        ent = (Entry)cnt;
                                        Assoc_metatext.TextValue = ent.Text;
                                    }
                                    else if (cnt_type.Name.ToLower() == "bordereditor")
                                    {
                                        edit = (BorderEditor)cnt;
                                        Assoc_metatext.TextValue = edit.Text;
                                    }
                                    else if (cnt_type.Name.ToLower() == "label")
                                    {
                                        lbl = (Label)cnt;
                                        Assoc_metatext.TextValue = lbl.Text;
                                    }
                                    else if (cnt_type.Name.ToLower() == "image")
                                    {
                                        img = (Image)cnt;

                                        var stack = (StackLayout)img.Parent;
                                        foreach (var item in stack.Children)
                                        {
                                            var tp = item.GetType();
                                            if (tp.Name.ToLower() == "image")
                                            {
                                                Image im = (Image)item;
                                                Assoc_metatext = new AssociationMetaDataText();
                                                if (im.StyleId.ToString().Contains("|"))
                                                {
                                                    Assoc_metatext.EntityFileID = Convert.ToInt32((im.StyleId.ToString().Substring(im.StyleId.LastIndexOf('|') + 1)));
                                                }
                                                else
                                                {
                                                    Assoc_metatext.EntityFileID = 0;
                                                }

                                                eAssociationField.AssocMetaDataText.Add(Assoc_metatext);
                                            }
                                        }

                                    }
                                    else if (cnt_type.Name.ToLower() == "switch")
                                    {
                                        _checkbox = (Switch)cnt;
                                        //var data = EntitySchemaLists.AssociationFieldCollection[i].AssocMetaDataText;
                                        //if (data.Count != 0)
                                        {
                                            Assoc_metatext.TextValue = _checkbox.IsToggled == true ? "Y" : "N";
                                            //if (_checkbox.IsToggled)
                                            //{
                                            //    Assoc_metatext.TextValue = "Y";
                                            //}
                                            //else
                                            //    Assoc_metatext.TextValue = "N";
                                        }
                                    }

                                    Assoc_metatext.AssocTypeID = EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;

                                    if (cnt_type.Name.ToLower() != "image")
                                        eAssociationField.AssocMetaDataText.Add(Assoc_metatext);
                                }
                                break;
                                #endregion
                        }

                        eAssociationField.EntityTypeID = EntitySchemaLists.AssociationFieldCollection[i].EntityTypeID;
                        eAssociationField.AssocTypeID = EntitySchemaLists.AssociationFieldCollection[i].AssocTypeID;
                        eAssociationField.FieldType = EntitySchemaLists.AssociationFieldCollection[i].FieldType;
                        eAssociationField.AssocName = EntitySchemaLists.AssociationFieldCollection[i].AssocName;
                        eAssociationField.CurrencyTypeID = EntitySchemaLists.AssociationFieldCollection[i].CurrencyTypeID;
                        eAssociationField.CurrencyIndicator = EntitySchemaLists.AssociationFieldCollection[i].CurrencyIndicator;
                        eAssociationField.DecimalPrecesion = EntitySchemaLists.AssociationFieldCollection[i].DecimalPrecesion;
                        eAssociationField.IsRequired = EntitySchemaLists.AssociationFieldCollection[i].IsRequired;
                        eAssociationField.ItemDesktopPriorityValue = EntitySchemaLists.AssociationFieldCollection[i].ItemDesktopPriorityValue;
                        eAssociationField.ItemMobilePriorityValue = EntitySchemaLists.AssociationFieldCollection[i].ItemMobilePriorityValue;
                        eAssociationField.ListDesktopPriorityValue = EntitySchemaLists.AssociationFieldCollection[i].ListDesktopPriorityValue;
                        eAssociationField.ListMobilePriorityValue = EntitySchemaLists.AssociationFieldCollection[i].ListMobilePriorityValue;
                        eAssociationField.AssocShowOnList = EntitySchemaLists.AssociationFieldCollection[i].AssocShowOnList;
                        eAssociationField.AssocSystemCode = EntitySchemaLists.AssociationFieldCollection[i].AssocSystemCode;
                        eAssociationField.AssocSystemCodeName = EntitySchemaLists.AssociationFieldCollection[i].AssocSystemCodeName;
                        eAssociationField.AssocDecode = new List<AssociationFieldValue>();
                        eAssociationField.CalculationFormula = EntitySchemaLists.AssociationFieldCollection[i].CalculationFormula;
                        eAssociationField.CalculationFrequencyMin = EntitySchemaLists.AssociationFieldCollection[i].CalculationFrequencyMin;
                        entity.AssociationFieldCollection.Add(eAssociationField);
                    }

                    CreateNewEntityItemRequest Create_entity = new CreateNewEntityItemRequest();
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                    if (Functions.IsEditEntity)
                    {
                        Create_entity.User = Functions.UserName;
                        entity.EntityID = ((StemmonsMobile.DataTypes.DataType.Entity.EntityClass)EntityListsValues).EntityID;
                        entity.ListID = ((StemmonsMobile.DataTypes.DataType.Entity.EntityClass)EntityListsValues).ListID;
                        entity.EntityTypeSecurityType = EntityListsValues.EntityTypeSecurityType;
                        entity.EntityAssignedToFullName = EntityListsValues.EntityAssignedToFullName;
                        entity.EntityAssignedToDateTime = EntityListsValues.EntityAssignedToDateTime;
                        entity.EntityModifiedByEmail = Functions.UserFullName;
                        entity.EntityModifiedByUserName = Functions.UserName;
                        entity.EntitiyOwnedByUserName = EntityListsValues.EntitiyOwnedByUserName;
                        entity.EntityOwnedByDateTime = EntityListsValues.EntityOwnedByDateTime;
                        entity.EntityOwnedByFullName = EntityListsValues.EntityOwnedByFullName;
                        entity.EntityCreatedByFullName = EntityListsValues.EntityCreatedByFullName;
                        entity.EntityCreatedDateTime = EntityListsValues.EntityCreatedDateTime;
                        entity.EntityCreatedByUserName = EntityListsValues.EntityCreatedByUserName;

                        Create_entity.entityTypeSchema = entity;

                        await Task.Run(async () =>
                        {
                            insertedRecordid = await EntitySyncAPIMethods.StoreAndcreateEntity(App.Isonline, entity.EntityTypeID, Functions.IsEditEntity, Create_entity, App.DBPath);
                        });
                        string str = "";
                        if (!string.IsNullOrEmpty(txt_EntNotes.Text) && insertedRecordid > 0)
                        {
                            await Task.Run(async () =>
                            {
                                str = await EntitySyncAPIMethods.StoreEntityNotes(App.Isonline, mbView.EntityDetails.EntityTypeID, insertedRecordid.ToString(), txt_EntNotes.Text, Functions.UserName, "Notes Type ID is Static", ((Enum.GetNames(typeof(ActionTypes)))[5] + "," + (Enum.GetNames(typeof(ActionTypes)))[1]), App.DBPath, Functions.UserFullName);
                            });
                        }
                    }
                    else
                    {
                        Create_entity.User = Functions.UserName;
                        Create_entity.entityTypeSchema = entity;
                        entity.EntityTypeSecurityType = EntitySchemaLists.EntityTypeSecurityType;

                        await Task.Run(async () =>
                        {
                            insertedRecordid = await EntitySyncAPIMethods.StoreAndcreateEntity(App.Isonline, entity.EntityTypeID, Functions.IsEditEntity, Create_entity, App.DBPath);
                        });

                        string str = "";
                        if (!string.IsNullOrEmpty(txt_EntNotes.Text) && insertedRecordid > 0)
                        {
                            await Task.Run(async () =>
                            {
                                str = await EntitySyncAPIMethods.StoreEntityNotes(App.Isonline, entity.EntityTypeID, insertedRecordid.ToString(), txt_EntNotes.Text, Functions.UserName, "Notes Type ID is Static so change on requirement", (Enum.GetNames(typeof(ActionTypes)))[4] + "," + (Enum.GetNames(typeof(ActionTypes)))[1], App.DBPath, Functions.UserFullName);
                            });
                        }
                    }

                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                }
                else
                {
                    DisplayAlert("Required Data", ReqFieldAssoc + " field is Required.", "Ok");
                }
            }
            catch (Exception ex)
            {


            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            return insertedRecordid;
        }

        private Dictionary<string, string> dctForumulaQuery = new Dictionary<string, string>();

        private void ClearEntityChildControls(int iAssocTypeID)
        {
            List<AssociationField> ls = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == iAssocTypeID)?.ToList();

            foreach (var item in ls)
            {
                if (item.EntityAssocTypeCascade.Count > 0)
                {
                    List<int> CHildLst = item.EntityAssocTypeCascade.Where(t => t.EntityAssocTypeIDParent == iAssocTypeID)?.ToList()?.Select(x => x.EntityAssocTypeIDChild)?.ToList();

                    if (CHildLst.Count <= 0)
                        break;

                    foreach (var subItem in CHildLst)
                    {
                        string sFieldType = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == subItem)?.FirstOrDefault()?.FieldType;
                        if (sFieldType.ToUpper() == "EL" || sFieldType.ToUpper() == "SE" || sFieldType.ToUpper() == "ME")
                        {
                            var pick = FindEntityControl(sFieldType + "_" + subItem) as Picker;
                            List<EXTERNAL_DATASOURCE1> _list_ed1 = new List<EXTERNAL_DATASOURCE1>();
                            EXTERNAL_DATASOURCE1 ed1 = new EXTERNAL_DATASOURCE1();
                            ed1.Count = 0;
                            ed1.EXTERNAL_DATASOURCE_DESCRIPTION = "-- Select Item --";
                            ed1.EXTERNAL_DATASOURCE_NAME = "-- Select Item --";
                            ed1.ID = 0;
                            _list_ed1.Add(ed1);
                            pick.ItemsSource = _list_ed1;
                            pick.SelectedIndex = 0;
                        }

                        ClearEntityChildControls(subItem);
                    }
                }
            }
        }
        private void FillEntityChildControl(int iAssocTypeID, string sSelectedValue)
        {
            string strconection = "";
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                List<AssociationField> ls = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == iAssocTypeID).ToList();

                foreach (var item in ls)
                {
                    if (item.EntityAssocTypeCascade.Count > 0)
                    {
                        List<int> CHildLst = item.EntityAssocTypeCascade.Where(t => t.EntityAssocTypeIDParent == iAssocTypeID).ToList().Select(x => x.EntityAssocTypeIDChild).ToList();
                        int iFilter = 0;
                        foreach (var subItem in CHildLst)
                        {
                            string sFieldType = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == subItem).FirstOrDefault().FieldType;
                            string sQuery = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == subItem).ToList().Select(x => x.ExternalDataSource)?.FirstOrDefault()?.Query;
                            if (string.IsNullOrEmpty(sQuery)) break;
                            string sExternalDatasourceName = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == subItem).ToList().Select(x => x.ExternalDataSource)?.FirstOrDefault()?.DataSourceName;
                            var childAssocObject = EntitySchemaLists.AssociationFieldCollection.Where(i => i.AssocTypeID == subItem).ToList();
                            int? externalDatasourceIdChild = EntitySchemaLists.AssociationFieldCollection.Where(i => i.AssocTypeID == subItem).FirstOrDefault()?.ExternalDataSource?.ExternalDatasourceID;
                            List<int> ParentLst = childAssocObject.FirstOrDefault()?.EntityAssocTypeCascade.Where(t => t.EntityAssocTypeIDChild == subItem).ToList().Select(x => x.EntityAssocTypeIDParent).ToList();
                            Dictionary<int, string> dctFilter = new Dictionary<int, string>();

                            foreach (var parentID in ParentLst)
                            {
                                if (parentID == iAssocTypeID)
                                {
                                    dctFilter.Add(parentID, sSelectedValue);
                                }
                                else
                                {
                                    string sFieldTypeParent = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == parentID).FirstOrDefault().FieldType;

                                    if (sFieldTypeParent.ToUpper() == "SE" || sFieldTypeParent.ToUpper() == "ME" || sFieldTypeParent.ToUpper() == "EL")
                                    {
                                        var cnt = FindEntityControl(sFieldTypeParent.ToUpper() + "_" + parentID);

                                        var ddlEds = (Picker)cnt;
                                        List<EXTERNAL_DATASOURCE1> src = ddlEds.ItemsSource as List<EXTERNAL_DATASOURCE1>;
                                        dctFilter.Add(parentID, src[0].EXTERNAL_DATASOURCE_NAME);
                                    }
                                }
                                iFilter = 1;
                            }

                            foreach (var dct in dctFilter.OrderBy(v => v.Key))
                            {
                                if (dct.Value != "-- Select Item --")
                                {
                                    var assoc = EntitySchemaLists.AssociationFieldCollection.FirstOrDefault(t => t.AssocTypeID == subItem);
                                    int? externalDataSourceIdParent = EntitySchemaLists.AssociationFieldCollection.Where(i => i.AssocTypeID == dct.Key).FirstOrDefault()?.ExternalDataSource.ExternalDatasourceID;

                                    if (assoc.ExternalDataSource != null)
                                    {
                                        //await Task.Run(() =>
                                        // {
                                        strconection = Functions.GetDecodeConnectionString(assoc.ExternalDataSource.ConnectionString);
                                        // });

                                        if (strconection.ToUpper().Contains("BOXER_ENTITIES"))
                                        {
                                            var assocCasd = assoc.EntityAssocTypeCascade.FirstOrDefault();

                                            string filterQueryChild = "";

                                            //await Task.Run(() =>
                                            //{
                                            /**/
                                            var data = EntityAPIMethods.getConnectionString(Convert.ToString(assoc.ExternalDataSource.ExternalDatasourceID)).GetValue("ResponseContent");

                                            if (!string.IsNullOrEmpty(data?.ToString()) && data.ToString() != "[]")
                                            {
                                                List<ConnectionStringCls> responsejson = JsonConvert.DeserializeObject<List<ConnectionStringCls>>(data.ToString());

                                                filterQueryChild = responsejson[0]._FILTER_QUERY;
                                            }
                                            //});

                                            string sFilterQuery = string.Empty;

                                            if (!string.IsNullOrEmpty(filterQueryChild))
                                            {
                                                sFilterQuery = filterQueryChild;

                                                var cnt = 0;
                                                if (sFilterQuery.Contains("{'ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID'}")) cnt++;
                                                if (sFilterQuery.Contains("'%BOXER_ENTITIES%'")) cnt++;
                                                try
                                                {
                                                    for (int i = 0; i < cnt; i++)
                                                    {
                                                        int s1 = sFilterQuery.IndexOf("/*");
                                                        int e1 = sFilterQuery.IndexOf("*/");
                                                        string f1 = sFilterQuery.Substring(s1, (e1 + 2) - s1);
                                                        if (f1.IndexOf("{'ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID'}") > 0)
                                                        {
                                                            s1 = s1 + 2;
                                                            string r1 = sFilterQuery.Substring(s1, e1 - s1);
                                                            sFilterQuery = sFilterQuery.Replace(f1, " and " + r1);

                                                            sFilterQuery = sFilterQuery.Replace("{'ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID'}", externalDataSourceIdParent.ToString());
                                                        }
                                                        else
                                                        {
                                                            sFilterQuery = sFilterQuery.Replace(f1, "");
                                                        }
                                                    }
                                                    if (dct.Value != "-- Select Item --")
                                                        sFilterQuery = sFilterQuery.Replace("{'EXTERNAL_DATASOURCE_OBJECT_ID'}", "( " + dct.Value + " )");
                                                    else
                                                    { sFilterQuery = sFilterQuery.Replace("{'EXTERNAL_DATASOURCE_OBJECT_ID'}", "( " + 0 + " )"); }
                                                }
                                                catch (Exception ex) { continue; }
                                            }

                                            sQuery = sQuery.Replace("/*{ENTITY_FILTER_QUERY_" + iFilter + "}*/", "  " + sFilterQuery + " ");
                                            iFilter++;
                                        }
                                    }
                                    var sbitem = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == dct.Key).FirstOrDefault();
                                    sQuery = GetQueryStringWithParamaters(sQuery, sbitem.ExternalDataSource?.DataSourceName, dct.Value, sbitem.AssocName);
                                }

                            }

                            if (dctForumulaQuery.ContainsKey(sExternalDatasourceName))
                            {
                                dctForumulaQuery.Remove(sExternalDatasourceName);
                            }
                            dctForumulaQuery.Add(sExternalDatasourceName, sQuery);
                            List<EXTERNAL_DATASOURCE1> dt = new List<EXTERNAL_DATASOURCE1>();

                            //await Task.Run(() =>
                            //{
                            var p = EntitySchemaLists.AssociationFieldCollection.Where(t => t.AssocTypeID == subItem)?.ToList()?.Select(x => x.ExternalDataSource).FirstOrDefault().ConnectionString;

                            var Response = EntityAPIMethods.ExternalDatasourceByQuery(Functions.GetDecodeConnectionString(item.ExternalDataSource.ConnectionString), sQuery);
                            var result = Response.GetValue("ResponseContent");
                            dt = JsonConvert.DeserializeObject<List<EXTERNAL_DATASOURCE1>>(result.ToString());
                            //});

                            string controlId = sFieldType + "_" + subItem;

                            if (sFieldType.ToUpper() == "SE" || sFieldType.ToUpper() == "ME" || sFieldType.ToUpper() == "EL")
                            {

                                var ddlEds = (Picker)FindEntityControl(controlId);
                                try
                                {
                                    List<EXTERNAL_DATASOURCE1> _list_ed1 = new List<EXTERNAL_DATASOURCE1>();
                                    ddlEds.ItemsSource = _list_ed1;
                                    EXTERNAL_DATASOURCE1 ed1 = new EXTERNAL_DATASOURCE1();
                                    ed1.Count = 0;
                                    ed1.EXTERNAL_DATASOURCE_DESCRIPTION = "-- Select Item --";
                                    ed1.EXTERNAL_DATASOURCE_NAME = "-- Select Item --";
                                    ed1.ID = 0;
                                    dt.Insert(0, ed1);
                                    ddlEds.ItemsSource = dt;
                                    ddlEds.SelectedIndex = 0;
                                }
                                catch (Exception ex)
                                {
                                    ddlEds.ItemsSource = dt;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        public static string GetQueryStringWithParamaters(string query, string externalDsName, string value, string field = "", bool checkIn = false, bool checkLike = false)
        {
            string results = query;

            ICollection<string> matches = Regex.Matches(
                    results.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                    .Cast<Match>()
                    .Select(x => x.Groups[1].Value)
                    .ToList();

            foreach (string match in matches)
            {
                if (match.Contains("|"))
                {
                    var split = match.Split('|');
                    if (!string.IsNullOrEmpty(externalDsName))
                    {
                        if (split[1].ToUpper().Trim() == externalDsName.ToUpper().Trim())
                        {
                            if (!checkLike)
                            {
                                results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " IN (" + value.ToString().Trim() + ")");
                            }
                            else
                                results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " LIKE '%" + value.ToString().Trim() + "%'");
                        }

                        else
                        if (!string.IsNullOrEmpty(field))
                        {
                            if (split[1].ToUpper().Trim() == field.ToUpper().Trim())
                            {
                                if (!checkLike)
                                {
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " IN (" + value.ToString().Trim() + ")");
                                }
                                else
                                    results = results.Replace("/*{" + match + "}*/", "AND " + split[0] + " LIKE '%" + value.ToString().Trim() + "%'");
                            }
                        }
                    }
                }
            }

            return results.ToString();
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

        private void btn_more_Clicked(object sender, EventArgs e)
        {
            Tool_Create_Clicked(sender, e);
        }

        private async void gridEntitynotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var notes = gridEntitynotes.SelectedItem as Entity_Notes;
                if (notes.ImageVisible)
                {
                    await Navigation.PushAsync(new ViewAttachment
                    (notes.ImageURL));
                }
                gridEntitynotes.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}