using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.GetTypeValuesByAssocCaseTypeExternalDSResponse;
using System.Text.RegularExpressions;
using System.Net;
using PCLStorage;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using StemmonsMobile.Views.View_Case_Origination_Center;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes;
using Plugin.Media;
using System.IO;
using System.Text;
using System.Globalization;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ViewCasePage1 : ContentPage
    {

        ReturnCaseToLastAssigneeResponse.UserInfo casedatavalue;
        ObservableCollection<CasesNotesGroup> Groups = new ObservableCollection<CasesNotesGroup>();
        CaseData _Casedata;

        string Casetitle = string.Empty;
        string Casetypeid = string.Empty;
        string CaseID = string.Empty;
        string _TitleFieldControlID = string.Empty;
        List<GetCaseTypesResponse.ItemType> sControls = new List<GetCaseTypesResponse.ItemType>();
        List<GetCaseTypesResponse.ItemType> sControlsList = new List<GetCaseTypesResponse.ItemType>();
        string ContolrLst = string.Empty;
        private ObservableCollection<CasesNotesGroup> _allGroups = new ObservableCollection<CasesNotesGroup>();
        private ObservableCollection<CasesNotesGroup> _expandedGroups;
        Dictionary<string, string> assocFieldValues = new Dictionary<string, string>();
        Dictionary<string, string> assocFieldTexts = new Dictionary<string, string>();
        string ischeckcalControl = string.Empty;
        List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>> calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
        List<Tuple<List<GetCaseTypesResponse.ItemType>, string, string>> lstcalculationsFieldlist = new List<Tuple<List<GetCaseTypesResponse.ItemType>, string, string>>();
        List<AssocCascadeInfo> AssocTypeCascades = new List<AssocCascadeInfo>();
        bool flgvisible = false;
        BorderEditor txt_CasNotes = new BorderEditor();
        string strTome = string.Empty;
        Dictionary<string, spi_MobileApp_GetTypesByCaseTypeResult> metaDitems = new Dictionary<string, spi_MobileApp_GetTypesByCaseTypeResult>();
        const int restrictCount = 3;
        Dictionary<string, List<GetExternalDataSourceByIdResponse.ExternalDatasource>> itmLookUp = new Dictionary<string, List<GetExternalDataSourceByIdResponse.ExternalDatasource>>();
        bool isload = false;

        public ViewCasePage1(string CASEID, string CASETYPEID, string CASETITLE, string ToMe = "")
        {
            InitializeComponent();

            Casetitle = CASETITLE;
            CaseID = CASEID;
            Casetypeid = CASETYPEID;
            strTome = ToMe;
            Groups.Clear();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Groups.Clear();
            EntryFieldsStack_CasesView.Children.Clear();
            _allGroups.Clear();

            if (!flgvisible)
                GetCasesDetails();
        }
        string CasesImgURL = string.Empty;
        public async void GetCasesDetails()
        {

            try
            {
                Groups.Clear();
                EntryFieldsStack_CasesView.Children.Clear();
                _allGroups.Clear();
                bool allowEditCaseData = false; //UPDATE right
                bool showCaseData = false; // Read Right
                List<MetaData> metadatacollection = new List<MetaData>();
                Task<List<GetCaseTypesResponse.CaseData>> result_getcase = null;
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                await Task.Run(() =>
                {
                    // For Create Case Controls Dynamically
                    var resultR = CasesSyncAPIMethods.AssignControlsAsync(App.Isonline, Convert.ToInt32(Casetypeid), App.DBPath, Functions.UserName);
                    resultR.Wait();
                    metadata = resultR.Result;

                    //get Record for View 
                    if (string.IsNullOrEmpty(strTome))
                    {
                        result_getcase = CasesSyncAPIMethods.GetCaseBasicInfo(App.Isonline, Functions.UserName, CaseID, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                        result_getcase.Wait();
                    }
                    else
                    {
                        result_getcase = CasesSyncAPIMethods.GetCaseListToMeMetadata(App.Isonline, CaseID, strTome, App.DBPath, Functions.UserName, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID);
                        result_getcase.Wait();
                    }

                    try
                    {
                        var lst = Functions.GetImageDownloadURL();
                        CasesImgURL = lst.Where(v => v.SYSTEM_CODE.ToUpper() == "CSHOM").FirstOrDefault().VALUE;
                    }
                    catch (Exception)
                    {
                    }

                });

                if (result_getcase?.Result?.Count == 0)
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    flgvisible = true;
                }
                else
                {
                    if (metadata.Count > 0)
                    {
                        _Casedata = result_getcase.Result.FirstOrDefault();
                        if (_Casedata == null)
                        {
                            await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                        }

                        Casetypeid = Convert.ToString(_Casedata?.CaseTypeID);
                        Casetitle = _Casedata?.CaseTitle;
                        if (result_getcase.Result != null && result_getcase.Result.ToString() != "[]")
                        {
                            metadatacollection = result_getcase.Result?.AsQueryable()?.FirstOrDefault()?.MetaDataCollection;

                            foreach (var iitem in metadatacollection)
                            {
                                var exdvalue = metadata.Where(v => v.ASSOC_TYPE_ID == iitem.AssociatedTypeID).Select(a => a.ExternalDataSourceEntityTypeID);
                                if (exdvalue.FirstOrDefault() != null)
                                {
                                    if (!App.Isonline)
                                        metadatacollection.Where(v => v.AssociatedTypeID == iitem.AssociatedTypeID).Select(s => { s.ExternalDatasourceObjectID = exdvalue.FirstOrDefault(); return s; }).ToList();
                                }
                            }
                        }


                        foreach (var ritem in metadata)
                        {
                            GetCaseTypesResponse.ItemType iItem = new GetCaseTypesResponse.ItemType()
                            {
                                AssocFieldType = Convert.ToChar(ritem.ASSOC_FIELD_TYPE),
                                AssociatedTypeSecurity = ritem.SECURITY_TYPE,
                                AssocTypeID = ritem.ASSOC_TYPE_ID,
                                CalculationFormula = ritem.CALCULATION_FORMULA,
                                CalculationFrequencyMin = ritem.CALCULATION_FREQUENCY_MIN,
                                CaseTypeID = ritem.CASE_TYPE_ID,
                                Child = ritem.CHILD_ID,
                                CreatedBy = ritem.CREATED_BY,
                                CreatedDateTime = ritem.CREATED_DATETIME,
                                Description = ritem.DESCRIPTION,
                                ExternalDataSourceEntityTypeID = Convert.ToInt32(ritem.ExternalDataSourceEntityTypeID),
                                ExternalDataSourceID = ritem.EXTERNAL_DATASOURCE_ID,
                                IsActive = Convert.ToChar(ritem.IS_ACTIVE),
                                IsFroceRecalculation = ritem.IS_FORCE_RECALCULATION,
                                IsRequired = Convert.ToChar(ritem.IS_REQUIRED),
                                ModifiedBy = ritem.MODIFIED_BY,
                                ModifiedDateTime = ritem.MODIFIED_DATETIME,
                                Name = ritem.NAME,
                                Parent = ritem.PARENT_ID,
                                SecurityType = ritem.SECURITY_TYPE,
                                ShowOnList = Convert.ToChar(ritem.SHOW_ON_LIST),
                                SeparatorCharactor = ritem.SEPARATOR_CHARACTOR,
                                SystemCode = ritem.SYSTEM_CODE,
                                SystemPriority = Convert.ToInt32(ritem.SYSTEM_PRIORITY),
                                UseCommaSeparator = ritem.USE_COMMA_SEPARATOR == 'Y' ? true : false,
                                UIWidth = ritem.UI_WIDTH
                            };
                            sControls.Add(iItem);
                        }
                        sControlsList.AddRange(sControls);
                        string CasetypeSecurity = metadata[0].SECURITY_TYPE;
                        if (CasetypeSecurity != null)
                        {
                            if (CasetypeSecurity.ToLower().Equals("open"))
                            {
                                allowEditCaseData = true;
                                showCaseData = true;
                            }
                            else
                            {
                                if (CasetypeSecurity.ToLower().Contains("u") || CasetypeSecurity.ToLower().Equals("open") || CasetypeSecurity.ToLower().Contains("u"))
                                {
                                    allowEditCaseData = true;
                                    showCaseData = true;
                                }
                                else
                                {
                                    if (CasetypeSecurity.ToLower().Contains("r") || CasetypeSecurity.ToLower().Equals("open") || CasetypeSecurity.ToLower().Contains("r"))
                                    {
                                        showCaseData = true;
                                    }
                                }
                            }
                        }
                        if (!showCaseData)
                        {
                            DisplayAlert("Alert!", "You do not have permission to view this page.", "OK");
                            Navigation.PopAsync();
                        }
                    }

                    #region  Dynamic Control Generate
                    if (metadata.Count > 0)
                    {
                        spi_MobileApp_GetTypesByCaseTypeResult itemTypes = new spi_MobileApp_GetTypesByCaseTypeResult();

                        if (App.Isonline)
                        {
                            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                            var AssocType = json.GetValue("ResponseContent");
                            AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                        }

                        foreach (var item in metadata)
                        {

                            bool filedsecuritytype_update = false;
                            bool filedsecuritytype_read = false;


                            if (!string.IsNullOrEmpty(item.ASSOC_SECURITY_TYPE) && item.ASSOC_SECURITY_TYPE.ToLower() != "c")
                            {
                                if (!string.IsNullOrEmpty(item.ASSOC_SECURITY_TYPE) || item.ASSOC_SECURITY_TYPE.ToLower().Contains("r") || item.ASSOC_SECURITY_TYPE.ToLower() == "open")
                                {
                                    filedsecuritytype_read = true;
                                    if (item.ASSOC_SECURITY_TYPE.ToLower().Contains("u") || item.ASSOC_SECURITY_TYPE.ToLower() == "open")
                                    {
                                        filedsecuritytype_update = true;
                                    }
                                }

                                if (item.SYSTEM_CODE?.ToLower() == "title")
                                {
                                    _TitleFieldControlID = item.ASSOC_FIELD_TYPE + "_" + item.ASSOC_TYPE_ID;
                                }

                                if (filedsecuritytype_read)
                                {

                                    var Mainlayout = new StackLayout();
                                    Mainlayout.Orientation = StackOrientation.Horizontal;
                                    Mainlayout.Margin = new Thickness(10, 0, 0, 0);

                                    var LeftLayout = new StackLayout();
                                    LeftLayout.HorizontalOptions = LayoutOptions.Start;
                                    LeftLayout.VerticalOptions = LayoutOptions.Center;
                                    LeftLayout.WidthRequest = 200;

                                    var Label1 = new Label
                                    {
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    Label1.Text = item.NAME;
                                    Label1.HorizontalOptions = LayoutOptions.Start;
                                    Label1.FontSize = 16;
                                    Label1.WidthRequest = 200;
                                    Label1.FontFamily = "Soin Sans Neue";

                                    LeftLayout.Children.Add(Label1);
                                    Mainlayout.Children.Add(LeftLayout);

                                    Picker pk = new Picker();
                                    pk.WidthRequest = 200;
                                    pk.TextColor = Color.Gray;

                                    DatePicker DO = new DatePicker();
                                    DO.Date = DateTime.Now;
                                    DO.Format = "MM/dd/yyyy";
                                    DO.WidthRequest = 200;
                                    DO.TextColor = Color.Gray;

                                    Entry entry = new Entry();

                                    Editor entry_notes = new Editor();
                                    entry_notes.HeightRequest = 100;
                                    entry_notes.FontSize = 16;
                                    entry_notes.WidthRequest = 200;
                                    entry_notes.Keyboard = Keyboard.Default;
                                    entry_notes.FontFamily = "Soin Sans Neue";

                                    Entry entry_number = new Entry();
                                    entry_number.WidthRequest = 200;
                                    entry_number.FontSize = 16;
                                    entry_number.Keyboard = Keyboard.Numeric;
                                    entry_number.FontFamily = "Soin Sans Neue";

                                    try
                                    {
                                        switch (item.ASSOC_FIELD_TYPE.ToLower())
                                        {
                                            //case "o":
                                            //case "e":

                                            //pk.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID + "|" + item.EXTERNAL_DATASOURCE_ID;


                                            //try
                                            //{
                                            //    pk.SelectedIndexChanged += Pk_SelectedIndexChanged;

                                            //    var result_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(App.Isonline, item.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(item.ASSOC_TYPE_ID));


                                            //    if (result_extdatasource.Result != null && result_extdatasource.Result.ToString() != "[]")
                                            //    {
                                            //        GetExternalDataSourceByIdResponse.ExternalDatasource cases_extedataSource = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                            //        {
                                            //            NAME = "-- Select Item --",
                                            //            DESCRIPTION = "-- Select Item --",
                                            //            ID = 0
                                            //        };

                                            //        List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                                            //        if (App.Isonline)
                                            //        {
                                            //            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                                            //            var AssocType = json.GetValue("ResponseContent");
                                            //            AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                                            //            var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == item.ASSOC_TYPE_ID).ToList();
                                            //            if (assocChild.Count < 1)
                                            //            {
                                            //                lst_extdatasource.AddRange(result_extdatasource.Result);
                                            //            }
                                            //        }
                                            //        else
                                            //        {
                                            //            var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(App.Isonline, item.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(item.ASSOC_TYPE_ID));

                                            //            temp_extdatasource.Wait();
                                            //            if (temp_extdatasource.Result.Count > 0)
                                            //            {
                                            //                lst_extdatasource.AddRange(temp_extdatasource.Result);
                                            //            }

                                            //        }

                                            //        var ts = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID).ToList();

                                            //        lst_extdatasource.Insert(0, cases_extedataSource);


                                            //        pk.ItemsSource = lst_extdatasource;
                                            //        pk.ItemDisplayBinding = new Binding("NAME");
                                            //    }
                                            //    if (!filedsecuritytype_update)
                                            //    {
                                            //        pk.IsEnabled = false;
                                            //    }
                                            //    //string aa = metadatacollection.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID;
                                            //}
                                            //catch (Exception ex)
                                            //{
                                            //}
                                            //Mainlayout.Children.Add(pk);
                                            //break;

                                            case "e":
                                            case "o":
                                                Picker drp = new Picker();
                                                drp.WidthRequest = 160;
                                                entry.WidthRequest = 40;

                                                drp.SelectedIndexChanged += drp_SelectedIndexChanged; ;

                                                drp.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID + "|" + item.EXTERNAL_DATASOURCE_ID;
                                                entry.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;
                                                try
                                                {
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        entry.IsEnabled = false;
                                                    }
                                                    if (metaDitems.ContainsKey(item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID))
                                                        metaDitems.Add(item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID, item);
                                                    else
                                                        metaDitems[item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID] = item;
                                                    entry.TextChanged += OnTextChanged;
                                                    entry.Keyboard = Keyboard.Default;
                                                    drp.ItemsSource = null;
                                                    drp.ItemDisplayBinding = new Binding("NAME");
                                                    entry.Text = "";
                                                }
                                                catch (Exception ex)
                                                {
                                                    entry.Text = "";
                                                }
                                                entry.FontSize = 16;

                                                Mainlayout.Children.Add(entry);
                                                Mainlayout.Children.Add(drp);

                                                break;

                                            case "d":

                                                pk.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID + "|" + item.EXTERNAL_DATASOURCE_ID;

                                                try
                                                {
                                                    pk.SelectedIndexChanged += Pk_SelectedIndexChanged;

                                                    List<ItemValue> lst_SSsource = new List<ItemValue>();
                                                    ItemValue Value = new ItemValue
                                                    {
                                                        Name = "-- Select Item --",
                                                        Description = "-- Select Item --",
                                                        ID = 0
                                                    };

                                                    lst_SSsource.Add(Value);

                                                    GetTypeValuesByAssocCaseTypeExternalDSRequest.GetTypeValuesByAssocCaseTypeExternalDS Request = new GetTypeValuesByAssocCaseTypeExternalDSRequest.GetTypeValuesByAssocCaseTypeExternalDS();

                                                    Request.assocCaseTypeID = item.ASSOC_TYPE_ID;
                                                    Request.caseTypeID = Convert.ToInt32(Casetypeid);
                                                    Request.caseTypeDesc = item.DESCRIPTION;
                                                    Request.systemCode = item.SYSTEM_CODE;
                                                    var result_extdatasource1 = CasesSyncAPIMethods.GetTypeValuesByAssocCaseType(App.Isonline, Request, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);

                                                    if (result_extdatasource1.Result != null && result_extdatasource1.Result.ToString() != "[]")
                                                    {
                                                        lst_SSsource = result_extdatasource1.Result;
                                                    }
                                                    pk.ItemsSource = lst_SSsource;
                                                }
                                                catch (Exception ex)
                                                {
                                                }

                                                Mainlayout.Children.Add(pk);
                                                break;

                                            case "x":
                                                BorderEditor editor = new BorderEditor();
                                                editor.FontFamily = "Soin Sans Neue";
                                                editor.StyleId = item.ASSOC_FIELD_TYPE + "_" + item.ASSOC_TYPE_ID;
                                                try
                                                {
                                                    editor.HeightRequest = 100;
                                                    editor.BorderColor = Color.LightGray;
                                                    editor.BorderWidth = 1;
                                                    editor.CornerRadius = 5;
                                                    editor.FontSize = 16;
                                                    editor.WidthRequest = 200;


                                                    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(editor.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), editor.StyleId?.Split('_')[0]?.ToUpper() + "_" + editor.StyleId?.Split('_')[1]?.Split('|')[0]);
                                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                        editor.Unfocused += Entry_Unfocused;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                Mainlayout.Children.Add(editor);
                                                break;

                                            case "a":
                                                DO.StyleId = "a_" + item.ASSOC_TYPE_ID;
                                                try
                                                {
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        DO.IsEnabled = false;
                                                    }
                                                    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(DO.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), DO.StyleId?.Split('_')[0]?.ToUpper() + "_" + DO.StyleId?.Split('_')[1]?.Split('|')[0]);
                                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                        DO.DateSelected += DO_DateSelected;
                                                    DO.WidthRequest = 200;
                                                }
                                                catch (Exception ex)
                                                {

                                                    //throw;
                                                }

                                                Mainlayout.Children.Add(DO);
                                                break;

                                            case "t":
                                                entry.StyleId = "t_" + item.ASSOC_TYPE_ID;

                                                try
                                                {
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        entry.IsEnabled = false;
                                                    }
                                                    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(entry.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), entry.StyleId?.Split('_')[0]?.ToUpper() + "_" + entry.StyleId?.Split('_')[1]?.Split('|')[0]);
                                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                        entry.Unfocused += Entry_Unfocused;
                                                    entry.Keyboard = Keyboard.Default;
                                                    entry.Text = "";
                                                }
                                                catch (Exception ex)
                                                {
                                                    entry.Text = "";
                                                }
                                                entry.WidthRequest = 200;
                                                entry.FontSize = 16;
                                                entry.FontFamily = "Soin Sans Neue";
                                                Mainlayout.Children.Add(entry);
                                                break;
                                            case "c":
                                                entry.StyleId = "c_" + item.ASSOC_TYPE_ID;

                                                try
                                                {
                                                    entry.IsEnabled = false;
                                                    entry.WidthRequest = 200;
                                                    entry.FontSize = 16;
                                                    entry.Keyboard = Keyboard.Default;
                                                    entry.FontFamily = "Soin Sans Neue";
                                                    entry.Text = "";
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                Mainlayout.Children.Add(entry);

                                                break;
                                            case "n":
                                                entry_number.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;

                                                try
                                                {
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        entry_number.IsEnabled = false;
                                                    }
                                                    entry_number.WidthRequest = 200;
                                                    entry_number.FontSize = 16;
                                                    entry_number.Keyboard = Keyboard.Numeric;

                                                    entry_number.Text = "";
                                                    entry_number.FontFamily = "Soin Sans Neue";
                                                    ischeckcalControl = SetCalControls(sControls?.Where(v => v.AssocTypeID == Convert.ToInt32(entry_number.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls?.Where(v => v.AssocFieldType == 'C').ToList(), entry_number.StyleId?.Split('_')[0]?.ToUpper() + "_" + entry_number.StyleId?.Split('_')[1]?.Split('|')[0]);
                                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                        entry_number.Unfocused += entry_number_Unfocused;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                Mainlayout.Children.Add(entry_number);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                    EntryFieldsStack_CasesView.Children.Add(Mainlayout);
                                }
                            }
                        }

                        var layout = new StackLayout();
                        layout.Orientation = StackOrientation.Horizontal;
                        layout.Margin = new Thickness(10, 0, 0, 0);

                        var layout1 = new StackLayout();
                        layout1.HorizontalOptions = LayoutOptions.Start;

                        layout1.WidthRequest = 200;
                        var Label11 = new Label
                        { VerticalOptions = LayoutOptions.Start };
                        Label11.Text = "Add Notes";
                        Label11.HorizontalOptions = LayoutOptions.Start;
                        Label11.FontSize = 16;
                        Label11.WidthRequest = 200;
                        Label11.FontFamily = "Soin Sans Neue";


                        var layout14 = new StackLayout();
                        layout14.Orientation = StackOrientation.Vertical;
                        layout14.HorizontalOptions = LayoutOptions.Start;
                        layout14.WidthRequest = 200;



                        txt_CasNotes.VerticalOptions = LayoutOptions.Start;
                        txt_CasNotes.WidthRequest = 200;
                        txt_CasNotes.HeightRequest = 100;
                        txt_CasNotes.FontSize = 16;
                        txt_CasNotes.HorizontalOptions = LayoutOptions.Start;
                        txt_CasNotes.BorderColor = Color.LightGray;
                        txt_CasNotes.BorderWidth = 1;
                        txt_CasNotes.CornerRadius = 5;
                        txt_CasNotes.FontFamily = "Soin Sans Neue";


                        layout14.Children.Add(txt_CasNotes);


                        layout1.Children.Add(Label11);
                        layout.Children.Add(layout1);
                        layout.Children.Add(layout14);

                        var layout_CNotes = new StackLayout();
                        layout_CNotes.Orientation = StackOrientation.Horizontal;
                        layout_CNotes.Margin = new Thickness(10, 0, 0, 0);


                        var layout2 = new StackLayout();
                        layout2.HorizontalOptions = LayoutOptions.Start;

                        layout2.WidthRequest = 200;
                        var Label2 = new Label
                        { VerticalOptions = LayoutOptions.Start };
                        Label2.Text = "Notes Type";
                        Label2.HorizontalOptions = LayoutOptions.Start;
                        Label2.FontSize = 16;
                        Label2.WidthRequest = 200;
                        Label2.FontFamily = "Soin Sans Neue";

                        layout15 = new StackLayout();
                        layout15.Orientation = StackOrientation.Vertical;
                        layout15.HorizontalOptions = LayoutOptions.Start;
                        layout15.WidthRequest = 200;

                        var pic_CaseNotes = new Picker { Title = "Notes" };
                        pic_CaseNotes.StyleId = "NoteType" + 000001;
                        pic_CaseNotes.VerticalOptions = LayoutOptions.Start;
                        pic_CaseNotes.WidthRequest = 200;
                        pic_CaseNotes.TextColor = Color.Gray;

                        pic_CaseNotes.HorizontalOptions = LayoutOptions.Start;
                        pic_CaseNotes.BackgroundColor = Color.Transparent;

                        var pic_NoteList = new List<string>();
                        pic_NoteList.Add("Notes");

                        pic_NoteList.Add("Internal Notes");
                        pic_NoteList.Add("Partial Answer");
                        pic_NoteList.Add("Primary Answer");
                        pic_NoteList.Add("Full Answer");
                        pic_NoteList.Add("Phone Call");
                        pic_NoteList.Add("Email Correspondence");
                        pic_NoteList.Add("Messenger Correspondence");
                        pic_NoteList.Add("Additional Test Plan");



                        pic_CaseNotes.ItemsSource = pic_NoteList;
                        pic_CaseNotes.SelectedIndex = 0;

                        layout15.Children.Add(pic_CaseNotes);

                        layout2.Children.Add(Label2);
                        layout_CNotes.Children.Add(layout2);
                        layout_CNotes.Children.Add(layout15);



                        EntryFieldsStack_CasesView.Children.Add(layout_CNotes);
                        EntryFieldsStack_CasesView.Children.Add(layout);
                        if (!allowEditCaseData)
                        {
                            EntryFieldsStack_CasesView.IsEnabled = false;
                        }

                        foreach (var item in metadata)
                        {
                            try
                            {
                                switch (item.ASSOC_FIELD_TYPE.ToLower())
                                {
                                    case "o":
                                    case "e":

                                        var control = FindPickerControls(item.ASSOC_TYPE_ID) as Picker;
                                        if (control != null)
                                        {
                                            List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                                            lst_extdatasource = control.ItemsSource as List<GetExternalDataSourceByIdResponse.ExternalDatasource>;

                                            try
                                            {

                                                int idd = 0;
                                                if (App.Isonline)
                                                {
                                                    if (item.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    {
                                                        int id = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID && c.ExternalDatasourceObjectID != null)?.FirstOrDefault()?.ExternalDatasourceObjectID);
                                                        if (id > 0)
                                                        {
                                                            try
                                                            {
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == id));

                                                                SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(lst_extdatasource.Single(v => v.ID == id).ID), lst_extdatasource.Single(v => v.ID == id).NAME, item.ASSOC_TYPE_ID);
                                                                SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(item.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), item.ASSOC_FIELD_TYPE.ToUpper() + "_" + item.ASSOC_TYPE_ID, (Convert.ToString(lst_extdatasource.Single(v => v.ID == id).ID) == "-1" ? "0|" + lst_extdatasource.Single(v => v.ID == id).NAME : Convert.ToString(lst_extdatasource.Single(v => v.ID == id).ID) + "|" + lst_extdatasource.Single(v => v.ID == id).NAME));
                                                            }
                                                            catch
                                                            {
                                                                List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                                var rec = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                if (rec != null)
                                                                {
                                                                    if (lst_extdatasource?.Count() == 1 || lst_extdatasource == null)
                                                                    {
                                                                        GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                                        {
                                                                            NAME = "-- Select Item --",
                                                                            DESCRIPTION = "-- Select Item --",
                                                                            ID = 0
                                                                        };
                                                                        lst_extdatasource1.Add(lst);
                                                                        lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                        {
                                                                            DESCRIPTION = rec.TextValue == null ? rec.FieldValue : rec.TextValue,
                                                                            NAME = rec.TextValue == null ? rec.FieldValue : rec.TextValue,
                                                                            ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                        };
                                                                        lst_extdatasource1.Add(lst);
                                                                        control.ItemsSource = null;
                                                                        control.ItemsSource = lst_extdatasource1;
                                                                        idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Where(v => v.ID == Convert.ToInt32(rec.ExternalDatasourceObjectID)).FirstOrDefault());
                                                                        SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(lst.ID), lst.NAME, item.ASSOC_TYPE_ID);

                                                                        SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(item.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), item.ASSOC_FIELD_TYPE.ToUpper() + "_" + item.ASSOC_TYPE_ID, (Convert.ToString(lst.ID) == "-1" ? "0|" + lst.NAME : Convert.ToString(lst.ID) + "|" + lst.NAME));
                                                                    }
                                                                }
                                                            }
                                                        }

                                                    }
                                                    else if (item.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    {
                                                        int id = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID);
                                                        if (id > 0)
                                                        {
                                                            try
                                                            {
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == id));
                                                                SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(lst_extdatasource.Single(v => v.ID == id).ID), lst_extdatasource.Single(v => v.ID == id).NAME, item.ASSOC_TYPE_ID);
                                                                SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(item.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), item.ASSOC_FIELD_TYPE.ToUpper() + "_" + item.ASSOC_TYPE_ID, (Convert.ToString(lst_extdatasource.Single(v => v.ID == id).ID) == "-1" ? "0|" + lst_extdatasource.Single(v => v.ID == id).NAME : Convert.ToString(lst_extdatasource.Single(v => v.ID == id).ID) + "|" + lst_extdatasource.Single(v => v.ID == id).NAME));
                                                            }
                                                            catch
                                                            {
                                                                List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                                var rec = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                if (rec != null)
                                                                {
                                                                    if (lst_extdatasource == null)
                                                                    {
                                                                        GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                                        {
                                                                            NAME = "-- Select Item --",
                                                                            DESCRIPTION = "-- Select Item --",
                                                                            ID = 0
                                                                        };
                                                                        lst_extdatasource1.Add(lst);
                                                                        lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                        {
                                                                            DESCRIPTION = rec.TextValue == null ? rec.FieldValue : rec.TextValue,
                                                                            NAME = rec.TextValue == null ? rec.FieldValue : rec.TextValue,
                                                                            ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                        };
                                                                        lst_extdatasource1.Add(lst);
                                                                        control.ItemsSource = null;
                                                                        control.ItemsSource = lst_extdatasource1;
                                                                        idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Where(v => v.ID == Convert.ToInt32(rec.ExternalDatasourceObjectID)).FirstOrDefault());
                                                                        SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(lst.ID), lst.NAME, item.ASSOC_TYPE_ID);
                                                                        SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(item.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), item.ASSOC_FIELD_TYPE.ToUpper() + "_" + item.ASSOC_TYPE_ID, (Convert.ToString(lst_extdatasource1.Single(v => v.ID == id).ID) == "-1" ? "0|" + lst_extdatasource1.Single(v => v.ID == id).NAME : Convert.ToString(lst_extdatasource1.Single(v => v.ID == id).ID) + "|" + lst_extdatasource1.Single(v => v.ID == id).NAME));
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    control.SelectedIndex = idd == 0 ? -1 : idd;

                                                }
                                                else
                                                {
                                                    if (item.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    {
                                                        string sTextvalue = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                        if (!string.IsNullOrEmpty(sTextvalue))
                                                        {
                                                            try
                                                            {
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalue?.ToLower()?.Trim()).FirstOrDefault());
                                                            }
                                                            catch
                                                            {
                                                                List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                                var rec = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                if (rec != null)
                                                                {
                                                                    if (lst_extdatasource == null)
                                                                    {
                                                                        GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                                        {
                                                                            NAME = "-- Select Item --",
                                                                            DESCRIPTION = "-- Select Item --",
                                                                            ID = 0
                                                                        };
                                                                        lst_extdatasource1.Add(lst);
                                                                        lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                        {
                                                                            DESCRIPTION = rec.TextValue,
                                                                            NAME = rec.TextValue,
                                                                            ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                        };
                                                                        lst_extdatasource1.Add(lst);
                                                                        control.ItemsSource = null;
                                                                        control.ItemsSource = lst_extdatasource1;
                                                                        idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Where(v => v.ID == Convert.ToInt32(rec.ExternalDatasourceObjectID)).FirstOrDefault());
                                                                    }
                                                                }
                                                            }
                                                            control.SelectedIndex = idd == 0 ? -1 : idd;
                                                        }
                                                        else
                                                        {
                                                            string sTextvalues = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue);
                                                            if (!string.IsNullOrEmpty(sTextvalues))
                                                            {
                                                                try
                                                                {
                                                                    idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalues?.ToLower()?.Trim()).FirstOrDefault());
                                                                    if (idd == -1)
                                                                    {
                                                                        List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                                        var rec = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                        if (rec != null)
                                                                        {
                                                                            if (lst_extdatasource.Count() == 1)
                                                                            {
                                                                                GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                                                {
                                                                                    NAME = "-- Select Item --",
                                                                                    DESCRIPTION = "-- Select Item --",
                                                                                    ID = 0
                                                                                };
                                                                                lst_extdatasource1.Add(lst);
                                                                                lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                                {
                                                                                    DESCRIPTION = rec.TextValue == null ? rec.FieldValue : rec.TextValue,
                                                                                    NAME = rec.TextValue == null ? rec.FieldValue : rec.TextValue,
                                                                                    ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                                };
                                                                                lst_extdatasource1.Add(lst);
                                                                                control.ItemsSource = null;
                                                                                control.ItemsSource = lst_extdatasource1;
                                                                                idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Where(v => v.ID == Convert.ToInt32(rec.ExternalDatasourceObjectID)).FirstOrDefault());
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                catch
                                                                {
                                                                    List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                                    var rec = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                    if (rec != null)
                                                                    {
                                                                        if (lst_extdatasource == null)
                                                                        {
                                                                            GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                                            {
                                                                                NAME = "-- Select Item --",
                                                                                DESCRIPTION = "-- Select Item --",
                                                                                ID = 0
                                                                            };
                                                                            lst_extdatasource1.Add(lst);
                                                                            lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                            {
                                                                                DESCRIPTION = rec.TextValue,
                                                                                NAME = rec.TextValue,
                                                                                ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                            };
                                                                            lst_extdatasource1.Add(lst);
                                                                            control.ItemsSource = null;
                                                                            control.ItemsSource = lst_extdatasource1;
                                                                            idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Single(v => v.ID == item.ASSOC_TYPE_ID));
                                                                        }
                                                                    }
                                                                }
                                                                control.SelectedIndex = idd == 0 ? -1 : idd;
                                                            }

                                                        }
                                                    }
                                                    else if (item.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    {
                                                        List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                        var rec = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                        if (rec != null)
                                                        {
                                                            if (lst_extdatasource == null)
                                                            {

                                                                GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                                {
                                                                    NAME = "-- Select Item --",
                                                                    DESCRIPTION = "-- Select Item --",
                                                                    ID = 0
                                                                };
                                                                lst_extdatasource1.Add(lst);
                                                                lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                {
                                                                    DESCRIPTION = rec.TextValue,
                                                                    NAME = rec.TextValue,
                                                                    ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                };
                                                                lst_extdatasource1.Add(lst);
                                                                control.ItemsSource = null;
                                                                control.ItemsSource = lst_extdatasource1;
                                                                idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Single(v => v.ID == item.ASSOC_TYPE_ID));
                                                            }
                                                            else
                                                            {
                                                                string sTextvalue = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                                if (!string.IsNullOrEmpty(sTextvalue))
                                                                {
                                                                    try
                                                                    {
                                                                        idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalue.ToLower()?.Trim()).FirstOrDefault());
                                                                        if (idd == -1)
                                                                        {
                                                                            var lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                            {
                                                                                DESCRIPTION = rec.TextValue,
                                                                                NAME = rec.TextValue,
                                                                                ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                            };
                                                                            lst_extdatasource.Add(lst);
                                                                            control.ItemsSource = null;
                                                                            control.ItemsSource = lst_extdatasource;
                                                                            idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalue.ToLower()?.Trim()).FirstOrDefault());
                                                                        }
                                                                    }
                                                                    catch
                                                                    {

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    string sTextvalues1 = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue);
                                                                    if (!string.IsNullOrEmpty(sTextvalues1))
                                                                    {
                                                                        try
                                                                        {
                                                                            idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalues1.ToLower()?.Trim()).FirstOrDefault());
                                                                            if (idd == -1)
                                                                            {
                                                                                var lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                                {
                                                                                    DESCRIPTION = rec.FieldValue,
                                                                                    NAME = rec.FieldValue,
                                                                                    ID = Convert.ToInt32(rec.ExternalDatasourceObjectID)
                                                                                };
                                                                                lst_extdatasource.Add(lst);
                                                                                control.ItemsSource = null;
                                                                                control.ItemsSource = lst_extdatasource;
                                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalues1.ToLower()?.Trim()).FirstOrDefault());
                                                                            }
                                                                        }
                                                                        catch
                                                                        {

                                                                        }
                                                                    }

                                                                }
                                                            }
                                                            control.SelectedIndex = idd == 0 ? -1 : idd;

                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        break;

                                    case "d":
                                        control = FindPickerControls(item.ASSOC_TYPE_ID) as Picker;
                                        if (control != null)
                                        {
                                            List<ItemValue> lst_SSsource = new List<ItemValue>();

                                            lst_SSsource = control.ItemsSource as List<ItemValue>;
                                            try
                                            {
                                                int j = 0;
                                                for (j = 0; j < lst_SSsource.Count; j++)
                                                {
                                                    if (lst_SSsource.Count != 0)
                                                    {
                                                        var i = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID).FirstOrDefault();

                                                        if (i?.AssociatedDecodeName?.ToLower() == lst_SSsource[j]?.Name?.ToLower() || i?.TextValue?.ToLower() == lst_SSsource[j]?.Name?.ToLower())
                                                        {
                                                            SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(lst_SSsource[j].ID), lst_SSsource[j]?.Name, item.ASSOC_TYPE_ID);
                                                            SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(item.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), item.ASSOC_FIELD_TYPE.ToUpper() + "_" + item.ASSOC_TYPE_ID, (Convert.ToString(lst_SSsource[j].ID) == "-1" ? "0|" + lst_SSsource[j]?.Name : Convert.ToString(lst_SSsource[j].ID) + "|" + lst_SSsource[j]?.Name));
                                                            break;
                                                        }
                                                    }
                                                    else
                                                        break;
                                                }
                                                control.SelectedIndex = j == 0 ? -1 : j;

                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "a":
                                        try
                                        {
                                            var cnt = FindCasesControls(item.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                DatePicker DO = new DatePicker();
                                                DO = cnt as DatePicker;
                                                DO.TextColor = Color.Gray;
                                                var Str = App.DateFormatStringToString(metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                DateTime dt = Convert.ToDateTime(Str);

                                                DO.Date = dt;
                                                SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(item.ASSOC_TYPE_ID), App.DateFormatStringToString(Str, CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"), item.ASSOC_TYPE_ID);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;

                                    case "x":
                                        try
                                        {
                                            var cnt = FindCasesControls(item.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                BorderEditor bd = cnt as BorderEditor;
                                                bd.FontSize = 16;
                                                bd.FontFamily = "Soin Sans Neue";
                                                bd.Text = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;

                                                SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(item.ASSOC_TYPE_ID), bd.Text, item.ASSOC_TYPE_ID);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;

                                    case "t":
                                    //case "c":
                                    case "n":
                                        try
                                        {
                                            var cnt = FindCasesControls(item.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                Entry entry = new Entry();
                                                entry.FontSize = 16;
                                                entry = cnt as Entry;
                                                entry.FontFamily = "Soin Sans Neue";
                                                entry.Text = metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;
                                                SetDictionary(assocFieldValues, assocFieldTexts, item.ASSOC_FIELD_TYPE.ToUpper(), Convert.ToString(item.ASSOC_TYPE_ID), entry.Text, item.ASSOC_TYPE_ID);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    }
                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                    isload = true;
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }

            if (!flgvisible)
            {
                reloadNotesArea();
            }

            try
            {
                if (_Casedata != null)
                {
                    _Casedata.CaseAssignedToDisplayName = _Casedata.CaseAssignedToDisplayName == null ? _Casedata.CaseAssignedTo : _Casedata.CaseAssignedToDisplayName;

                    var s = new FormattedString();
                    if (_Casedata?.CreateByDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = _Casedata.CreateByDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(_Casedata.CreateDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_createname.FormattedText = s;

                    s = new FormattedString();
                    if (_Casedata?.CaseAssignedToDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = _Casedata.CaseAssignedToDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = Convert.ToString(_Casedata.CaseAssignedDateTime == default(DateTime) ? DateTime.Now : _Casedata.CaseAssignedDateTime), FontSize = 14 });

                    }
                    lbl_assignto.FormattedText = s;

                    s = new FormattedString();
                    if (_Casedata?.CaseOwnerDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = _Casedata.CaseOwnerDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(_Casedata.CaseOwnerDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_ownername.FormattedText = s;

                    s = new FormattedString();
                    if (_Casedata?.ModifiedByDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = _Casedata.ModifiedByDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Convert.ToDateTime(_Casedata.ModifiedDateTime)).ToString(), FontSize = 14 });
                    }
                    lbl_modifiedname.FormattedText = s;
                }

            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

            Task.Run(() =>
            {
                if (App.Isonline)
                    setStaticCal(calculationsFieldlist);
            });
        }

        public void SetDictionary(Dictionary<string, string> assocFieldValues, Dictionary<string, string> assocFieldTexts, string CurrentStyleId, string selectedId, string selectedname, int AssocTypeID)
        {
            try
            {
                if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper() + "_" + AssocTypeID))
                {
                    assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper() + "_" + AssocTypeID, AssocTypeID + "|" + selectedId);
                    assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper() + "_" + AssocTypeID, selectedname);
                }
                else
                {
                    assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper() + "_" + AssocTypeID] = AssocTypeID + "|" + selectedId;
                    assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper() + "_" + AssocTypeID] = selectedname;
                }
            }
            catch
            {
            }
        }

        void OnTextChanged(object sender, EventArgs e)
        {
            try
            {
                Entry entry = sender as Entry;
                String val = entry.Text;
                if (!string.IsNullOrEmpty(entry.Text))
                {
                    List<ItemValue> lstItemType = new List<ItemValue>();

                    List<GetExternalDataSourceByIdResponse.ExternalDatasource> resultExtdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                    if (val.Length >= restrictCount)
                    {
                        //val = val.Remove(val.Length - 1);
                        entry.Text = val;
                        var result = metaDitems?.Where(v => v.Key.ToUpper().Trim() == entry.StyleId.ToUpper().Trim());
                        int Id = Convert.ToInt32(result?.FirstOrDefault().Value?.EXTERNAL_DATASOURCE_ID);

                        var parentAssoc = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == Convert.ToInt32(entry.StyleId.Split('_')[1])).ToList();
                        if (parentAssoc != null && parentAssoc?.Count > 0)
                        {
                            int parentCount = 1;
                            string parentSelectedValue = string.Empty;
                            string ParentExternalDatasourceName = string.Empty;
                            //if (result_extdatasource.Result != null && result_extdatasource.Result.ToString() != "[]")
                            {
                                var result_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(Id), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                string query = result_extdatasource.Result?.FirstOrDefault()?.Query;
                                string conn = result_extdatasource.Result?.FirstOrDefault()?.ConnectionString;
                                var tempfilterQueryOrg = CasesSyncAPIMethods.GetFilterQueryCases(App.Isonline, Convert.ToString(Id), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                tempfilterQueryOrg.Wait();
                                string filterQueryOrg = tempfilterQueryOrg.Result?.FirstOrDefault()._FILTER_QUERY;

                                var assocParents = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == Convert.ToInt32(entry.StyleId.Split('_')[1]));

                                foreach (var p in assocParents)
                                {
                                    var res = metaDitems.Where(v => v.Value.ASSOC_TYPE_ID == Convert.ToInt32(assocParents.FirstOrDefault()._CASE_ASSOC_TYPE_ID_PARENT)).FirstOrDefault();
                                    string parentFieldName = res.Value.NAME;
                                    var contrl = FindPickerControls(Convert.ToInt32(p._CASE_ASSOC_TYPE_ID_PARENT)) as Picker;
                                    if (contrl != null)
                                    {
                                        var pickerval = contrl.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                                        parentSelectedValue = Convert.ToString(pickerval.ID);
                                        ParentExternalDatasourceName = res.Value.NAME;

                                        query = GetQueryStringWithParamaters(query, parentFieldName, parentSelectedValue, ParentExternalDatasourceName);
                                    }


                                    //replace internal entity type
                                    if (!string.IsNullOrEmpty(filterQueryOrg))
                                    {
                                        var filterQuery = "" + filterQueryOrg;
                                        var cnt = 0;
                                        if (filterQuery.Contains("{'ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID'}")) cnt++;
                                        if (filterQuery.Contains("'%BOXER_ENTITIES%'")) cnt++;
                                        try
                                        {
                                            for (int i = 0; i < cnt; i++)
                                            {
                                                int s1 = filterQuery.IndexOf("/*");
                                                int e1 = filterQuery.IndexOf("*/");
                                                string f1 = filterQuery.Substring(s1, (e1 + 2) - s1);
                                                if (f1.IndexOf("'%BOXER_ENTITIES%'") > 0)
                                                {
                                                    s1 = s1 + 2;
                                                    string r1 = filterQuery.Substring(s1, e1 - s1);
                                                    filterQuery = filterQuery.Replace(f1, " and " + r1);
                                                }
                                                else
                                                {
                                                    filterQuery = filterQuery.Replace(f1, "");
                                                }
                                            }
                                            filterQuery = filterQuery.Replace("{'EXTERNAL_DATASOURCE_OBJECT_ID'}", "( " + parentSelectedValue + " )");
                                        }
                                        catch (Exception ex) { }

                                        query = query.Replace("/*{ENTITY_FILTER_QUERY_" + parentCount + "}*/", filterQuery);
                                        parentCount++;
                                    }
                                }
                                if (!string.IsNullOrEmpty(query))
                                {
                                    ICollection<string> matches = System.Text.RegularExpressions.Regex.Matches(
                    query.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                    .Cast<Match>()
                    .Select(x => x.Groups[1].Value)
                    .ToList();

                                    query = GetQueryStringWithParamaters(query, matches.FirstOrDefault(), entry.Text, true);
                                    var resultExtdatasources = CasesSyncAPIMethods.GetItemValueFromQueryExd(App.Isonline, conn, query, App.DBPath);
                                    resultExtdatasources.Wait();
                                    if (resultExtdatasources.Result != null)
                                    {

                                        resultExtdatasource = lstitemsource(resultExtdatasources.Result);
                                    }
                                }
                            }

                        }
                        else
                        {
                            var result_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(Id), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                            string query = result_extdatasource.Result?.FirstOrDefault()?.Query;
                            string conn = result_extdatasource.Result?.FirstOrDefault()?.ConnectionString;
                            string field = string.Empty;
                            ICollection<string> matches = System.Text.RegularExpressions.Regex.Matches(
                    query.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                    .Cast<Match>()
                    .Select(x => x.Groups[1].Value)
                    .ToList();

                            if (result?.FirstOrDefault().Value?.SYSTEM_CODE?.ToUpper()?.Trim() == "PRNTC")
                                field = "CURRENT_FILTER";
                            else
                                field = matches?.FirstOrDefault()?.ToUpper();
                            query = GetQueryStringWithParamaters(query, field, entry.Text, true);

                            var resultExtdatasources = CasesSyncAPIMethods.GetItemValueFromQueryExd(App.Isonline, conn, query, App.DBPath);
                            resultExtdatasources.Wait();
                            if (resultExtdatasources.Result != null)
                            {
                                resultExtdatasource = lstitemsource(resultExtdatasources.Result);
                            }

                        }

                        var control = FindPickerControls(Convert.ToInt32(entry.StyleId.Split('_')[1])) as Picker;
                        if (control != null)
                        {
                            control.ItemsSource = null;
                            control.ItemsSource = resultExtdatasource;
                            //clearContorls(control.StyleId);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private List<GetExternalDataSourceByIdResponse.ExternalDatasource> lstitemsource(List<GetCaseTypesResponse.ExternalObjectDataItem> resultExtdatasources)
        {
            List<GetExternalDataSourceByIdResponse.ExternalDatasource> exdlst = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
            try
            {
                foreach (var item in resultExtdatasources)
                {
                    GetExternalDataSourceByIdResponse.ExternalDatasource exd = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                    {
                        ID = item.ID,
                        DESCRIPTION = item.DESCRIPTION,
                        NAME = item.NAME
                    };
                    exdlst.Add(exd);
                }

                GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                {
                    NAME = "-- Select Item --",
                    DESCRIPTION = "-- Select Item --",
                    ID = 0
                };
                exdlst.Insert(0, lst);
            }
            catch
            {
            }
            return exdlst;
        }
        private string GetQueryStringWithParamaters(string query, string field, string value, bool IsLike)
        {
            string results = query.ToUpper();

            ICollection<string> matches = System.Text.RegularExpressions.Regex.Matches(
                    results.Replace(Environment.NewLine, ""), @"\{([^}]*)\}")
                    .Cast<Match>()
                    .Select(x => x.Groups[1].Value)
                    .ToList();

            foreach (string match in matches)
            {
                if (match.Contains("|"))
                {
                    // Replaces all /*{Field|UIName}*/ with the approrate value.
                    if (match.Split('|')[1].ToUpper().Trim() == field.ToUpper().Trim())
                    {
                        if (!IsLike)
                            results = results.Replace("/*{" + match + "}*/", "And " + match.Split('|')[0] + " = '" + match.Split('|')[1].Replace(field, value.ToString().Trim()) + "'");
                        else
                            results = results.Replace("/*{" + match + "}*/", "And " + match.Split('|')[0] + " like '%" + match.Split('|')[1].Replace(field, value.ToString().Trim()) + "%'");
                    }
                }
                else
                {
                    if (match.ToUpper().Trim() == field.ToUpper().Trim())
                    {
                        if (!IsLike)
                        {
                            results = results.Replace("/*{" + match + "}*/", "And " + match + " = '" + match.Replace(field, value.ToString().Trim()) + "'");
                        }
                        else
                        {
                            string[] smatchs = results.ToUpper().Split(new string[] { "AS" }, StringSplitOptions.None);
                            var smatch = smatchs.FirstOrDefault(v => v.Contains("NAME,"));
                            results = results.Replace("/*{" + match + "}*/", "And " + smatch.Split(',')[1].ToString() + " like '%" + match.Replace(field, value.ToString().Trim()) + "%'");
                        }
                    }
                }
            }

            return results.ToString();
        }

        public void reloadNotesArea()
        {
            try
            {

                gridCasesnotes.ItemsSource = null;
                Groups.Clear();
                dynamic NotesResponse = null;
                if (string.IsNullOrEmpty(strTome))
                {
                    NotesResponse = CasesSyncAPIMethods.GetCaseNotes(App.Isonline, CaseID, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                    NotesResponse.Wait();
                }
                else
                {
                    NotesResponse = CasesSyncAPIMethods.GetCaseListToMeMetadataNotes(App.Isonline, CaseID, strTome, App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID);
                    NotesResponse.Wait();

                }

                ObservableCollection<CasesNotesGroup> Temp = new ObservableCollection<CasesNotesGroup>();
                if (NotesResponse?.Result?.Count > 0)
                {
                    for (int i = 0; i < NotesResponse.Result.Count; i++)
                    {
                        CasesNotesGroup grp = new CasesNotesGroup("", Convert.ToString(NotesResponse.Result[i].CreatedDateTime), NotesResponse.Result[i]?.CreatedByUser == null ? Functions.UserFullName : NotesResponse.Result[i]?.CreatedByUser?.DisplayName)
                            {
                                new GetCaseNotesResponse.NoteData
                                {
                                    Note = NotesResponse.Result[i].Note
                                }
                            };
                        Temp.Add(grp);
                    }

                    foreach (var item in Temp)
                    {
                        if (item.FirstOrDefault().Note.Contains("<img"))
                        {
                            item.FirstOrDefault().ImageVisible = true;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().ImageURL = CasesImgURL + "/" + Functions.HTMLToText(item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1]);
                            item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                        }
                        else
                        {
                            item.FirstOrDefault().ImageVisible = false;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                        }
                        Groups.Add(item);
                    }

                    gridCasesnotes.ItemsSource = null;
                    gridCasesnotes.ItemsSource = Groups;

                }
                //var iheight = Groups.Where(v => v.FirstOrDefault().ImageVisible == true).ToList();

                //int imgHeight = (180 * (iheight.Count));

                //int lheight = Groups.Where(v => v.FirstOrDefault().ImageVisible == false).Count();

                //int lblHeight = (120 * lheight);

                // gridCasesnotes.HeightRequest = Convert.ToInt32(imgHeight + lblHeight) * Groups.Count < 250 ? 500 : Convert.ToInt32(imgHeight + lblHeight) * Groups.Count;
                //  gridCasesnotes.HeightRequest = (Groups.Count * 110);
            }
            catch (Exception ex)
            {
            }
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            int selectedIndex = _expandedGroups.IndexOf(
                ((CasesNotesGroup)((Button)sender).CommandParameter));
            _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
            UpdateListContent();
        }

        private void UpdateListContent()
        {
            try
            {
                _expandedGroups = new ObservableCollection<CasesNotesGroup>();
                foreach (CasesNotesGroup group in _allGroups)
                {
                    CasesNotesGroup newGroup = new CasesNotesGroup(group.Title, group.ShortName, Functions.UserName);

                    if (group.Expanded)
                    {
                        foreach (GetCaseNotesResponse.NoteData food in group)
                        {
                            newGroup.Add(food);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                gridCasesnotes.ItemsSource = _expandedGroups;
            }
            catch (Exception)
            {
            }
            //int heightRowLista = 90;
            //int iqa = (_allGroups.Count * heightRowLista);
            //gridCasesnotes.HeightRequest = iqa;
        }

        List<spi_MobileApp_GetTypesByCaseTypeResult> metadata = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
        StackLayout layout15 = new StackLayout();

        #region MyRegion : Picker Event
        private void ClearChildControl(int assocTypeId, List<GetCaseTypesResponse.ItemType> ItemTypes)
        {
            try
            {
                var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_PARENT == assocTypeId);
                if (assocChild == null || assocChild.ToList().Count == 0)
                    return;

                foreach (var child in assocChild)
                {
                    var itemType = ItemTypes.Where(t => t.AssocTypeID == child._CASE_ASSOC_TYPE_ID_CHILD).FirstOrDefault();
                    var control = FindPickerControls(child._CASE_ASSOC_TYPE_ID_CHILD);

                    if (control != null && itemType != null)
                    {
                        List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                        GetExternalDataSourceByIdResponse.ExternalDatasource cases_extedataSource = new GetExternalDataSourceByIdResponse.ExternalDatasource
                        {
                            NAME = "-- Select Item --",
                            DESCRIPTION = "-- Select Item --",
                            ID = 0
                        };
                        lst_extdatasource.Add(cases_extedataSource);
                        (control as Picker).ItemsSource = lst_extdatasource;
                        (control as Picker).SelectedIndex = 0;

                        ClearChildControl(child._CASE_ASSOC_TYPE_ID_CHILD, ItemTypes);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void drp_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!isload)
                    return;
                if (App.Isonline)
                {
                    Picker Picker = (Picker)sender;
                    if (Picker.ItemsSource != null)
                    {
                        if (Picker.ItemsSource.Count > 0)
                        {
                            if (Picker.SelectedItem != null)
                            {
                                var r = Picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;

                                int rv = Convert.ToInt32(Picker.StyleId.Split('_')[1].Split('|')[0]);
                                SetDictionary(assocFieldValues, assocFieldTexts, Picker.StyleId.Split('_')[0].ToUpper(), Convert.ToString(r.ID), Convert.ToString(r.NAME), rv);

                                Task.Run(() =>
                                {
                                    setStaticCal(calculationsFieldlist.Where(v => v.Key.FirstOrDefault().AssocTypeID == rv).ToList());

                                });
                                clearContorls(Picker.StyleId);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void clearContorls(String PickerStyleId)
        {
            int indx = AssocTypeCascades.IndexOf(AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_PARENT == Convert.ToInt32(PickerStyleId.Split('_')[1].Split('|')[0])).FirstOrDefault());
            if (indx == -1) return;
            for (int i = indx; i < AssocTypeCascades.Count; i++)
            {
                int id = Convert.ToInt32(AssocTypeCascades[i]._CASE_ASSOC_TYPE_ID_CHILD);
                var cntrl = FindPickerControls(id) as Picker;
                if (cntrl != null)
                {
                    cntrl.ItemsSource = null;
                    assocFieldValues[assocFieldValues.Where(v => v.Key == "assoc_" + cntrl.StyleId.Split('|')[0].ToUpper()).FirstOrDefault().Key] = id + "|" + -1;
                    assocFieldTexts[assocFieldTexts.Where(v => v.Key == "assoc_" + cntrl.StyleId.Split('|')[0].ToUpper()).FirstOrDefault().Key] = "";

                    Task.Run(() =>
                    {
                        setStaticCal(calculationsFieldlist.Where(v => v.Key.FirstOrDefault().AssocTypeID == id).ToList());
                    });
                }

                var cntrol = FindCasesControls(id) as Entry;
                if (cntrol != null)
                {
                    cntrol.Text = "";
                }
            }
        }
        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (App.Isonline)
                {
                    Picker Picker = (Picker)sender;
                    try
                    {
                        if (Picker.ItemsSource != null)
                        {
                            if (Picker.ItemsSource.Count > 0)
                            {
                                if (Picker.SelectedItem != null)
                                {
                                    //var sd = Picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                                    //try
                                    //{
                                    //    if (sd == null)
                                    //    {
                                    //        var newsd = Picker.SelectedItem as GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue;
                                    //        GetExternalDataSourceByIdResponse.ExternalDatasource val = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                    //        {
                                    //            DESCRIPTION = newsd.Description,
                                    //            ID = newsd.ID,
                                    //            NAME = newsd.Name
                                    //        };

                                    //        if (val.ID > 0)
                                    //        {
                                    //            int AssocIDD = int.Parse(Picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                    //            DyanmicSetCalc(Picker.StyleId);
                                    //        }
                                    //    }
                                    //}
                                    //catch
                                    //{
                                    //}
                                    int AssocID = int.Parse(Picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                    //if (sd != null && sd?.ID > 0)
                                    //{
                                    //    DyanmicSetCalcexd(Picker.StyleId, sControls.Where(v => (v.AssocFieldType == 'E' || v.AssocFieldType == 'O') && v.AssocTypeID == Convert.ToInt32(AssocID)).ToList(), sControls.Where(v => (v.AssocFieldType == 'C')).ToList());
                                    //}
                                    //if (lstcalculationsFieldlist.Count > 0)
                                    //{
                                    //    if (sd?.NAME.ToLower() == "-- select item --")
                                    //    {
                                    //        int aId = 0;
                                    //        if (AssocTypeCascades.Where(v => v._CASE_ASSOC_TYPE_ID_CHILD == AssocID)?.Count() > 0)
                                    //            aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v._CASE_ASSOC_TYPE_ID_CHILD == AssocID));
                                    //        else if (AssocTypeCascades.Where(v => v._CASE_ASSOC_TYPE_ID_PARENT == AssocID)?.Count() > 0)
                                    //            aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v._CASE_ASSOC_TYPE_ID_PARENT == AssocID));

                                    //        for (int i = aId; i < AssocTypeCascades.Count; i++)
                                    //        {
                                    //            var av = lstcalculationsFieldlist.Distinct().Where(v => v.Item1.FirstOrDefault().AssocTypeID == AssocID);
                                    //            foreach (var ii in av)
                                    //            {

                                    //                foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                                    //                {
                                    //                    foreach (var subitem in infofield.Children)
                                    //                    {
                                    //                        var xy = subitem;
                                    //                        Type ty = xy.GetType();
                                    //                        if (ty.Name.ToLower() != "stacklayout")
                                    //                        {
                                    //                            if (ty.Name.ToLower() == "entry")
                                    //                            {
                                    //                                var en = (Entry)xy;
                                    //                                if (en.StyleId == Convert.ToString((ii.Item2.ToLower().Substring((ii.Item2.ToLower().IndexOf('_') + 1)).ToLower())))
                                    //                                {
                                    //                                    int id = Convert.ToInt32(ii.Item2.Replace("assoc_C_", ""));
                                    //                                    var calinfo = sControls.Where(v => v.AssocFieldType == 'C' && v.AssocTypeID == id)?.FirstOrDefault();
                                    //                                    if (!string.IsNullOrEmpty(calinfo.CalculationFormula))
                                    //                                    {
                                    //                                        string avName = ii.Item1.FirstOrDefault().Name;
                                    //                                        string avSyscode = ii.Item1.FirstOrDefault().SystemCode;

                                    //                                        if (calinfo.CalculationFormula.IndexOf(avName) != -1)
                                    //                                        {
                                    //                                            string str = calinfo.CalculationFormula.Substring(calinfo.CalculationFormula.IndexOf(avName) + 1 + avName.Length)?.Split('}')[0];
                                    //                                            if (str == "ID")
                                    //                                                en.Text = en.Text.Replace(ii.Item3.Split('|')[0], "");
                                    //                                            else
                                    //                                                en.Text = en.Text.Replace(ii.Item3.Split('|')[1], "");
                                    //                                        }
                                    //                                        else
                                    //                                        {
                                    //                                            string str1 = calinfo.CalculationFormula.Substring(calinfo.CalculationFormula.IndexOf(avSyscode) + 1 + avSyscode.Length)?.Split('}')[0];
                                    //                                            if (str1 == "ID")
                                    //                                                en.Text = en.Text.Replace(ii.Item3.Split('|')[0], "");
                                    //                                            else
                                    //                                                en.Text = en.Text.Replace(ii.Item3.Split('|')[1], "");
                                    //                                        }
                                    //                                    }
                                    //                                }
                                    //                            }
                                    //                        }
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    ClearChildControl(AssocID, sControls);

                                    if ((Picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).ID > 0)
                                    {
                                        //metadata
                                        FillChildControl(AssocID, sControls);
                                    }
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception)
            {


            }
        }

        private int GetNoteTypeId(int AssocID)
        {
            try
            {
                foreach (var item in layout15.Children)
                {
                    Type typ = item.GetType();
                    if (typ.Name.ToLower() == "picker")
                    {
                        Picker pk = new Picker();
                        pk = (Picker)item;
                        if (pk.StyleId.Contains(AssocID.ToString()))
                        {
                            string val = pk.SelectedItem.ToString();
                            if (val == "Notes")
                                return 18;
                            if (val == "Internal Notes")
                                return 19;
                            if (val == "Partial Answer")
                                return 23;
                            if (val == "Primary Answer")
                                return 24;
                            if (val == "Full Answer")
                                return 25;
                            if (val == "Phone Call")
                                return 26;
                            if (val == "Email Correspondence")
                                return 32;
                            if (val == "Messenger Correspondence")
                                return 33;
                            if (val == "Additional Test Plan")
                                return 38;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 0;
        }

        private object FindPickerControls(int AssocID)
        {
            try
            {
                foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {
                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name.ToLower() == "picker")
                        {
                            Picker Picker = new Picker();
                            Picker.TextColor = Color.Gray;
                            Picker = (Picker)xy;
                            if (Picker.StyleId.Contains(AssocID.ToString()))
                                return Picker;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        private object FindCasesControls(int AssocID)
        {
            try
            {
                foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {
                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name != "StackLayout")
                            if (xy.StyleId.Contains(AssocID.ToString()))
                                return xy;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        private void FillChildControl(int assocTypeId, List<GetCaseTypesResponse.ItemType> ItemTypes)
        {
            try
            {
                var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                var AssocType = json.GetValue("ResponseContent");
                AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());

                string fieldName = string.Empty;
                var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_PARENT == assocTypeId).ToList();

                foreach (var child in assocChild)
                {
                    var control = FindPickerControls(child._CASE_ASSOC_TYPE_ID_CHILD) as Picker;
                    if (control != null)
                    {
                        int? externalDatasourceId = null;


                        var itemType = ItemTypes.Where(t => t.AssocTypeID == child._CASE_ASSOC_TYPE_ID_CHILD).FirstOrDefault();

                        // get extenal datasource query
                        if (itemType != null)
                            externalDatasourceId = itemType.ExternalDataSourceID;

                        if (externalDatasourceId != null && externalDatasourceId > 0)
                        {

                            var externalDatasource = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(externalDatasourceId.Value), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);

                            string query = externalDatasource.Result?.AsQueryable()?.FirstOrDefault()?.Query;
                            string conn = externalDatasource.Result?.AsQueryable()?.FirstOrDefault()?.ConnectionString;
                            string filterQueryOrg = "";

                            var Result1 = CasesSyncAPIMethods.GetConnectionString(App.Isonline, externalDatasourceId.Value.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);

                            if (!string.IsNullOrEmpty(Result1?.ToString()) && Result1.ToString() != "[]")
                            {
                                List<ConnectionStringCls> responsejson = Result1.Result;

                                filterQueryOrg = Result1.Result?.FirstOrDefault()?._FILTER_QUERY;
                            }


                            var assocParents = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == child._CASE_ASSOC_TYPE_ID_CHILD);
                            int parentCount = 1;
                            foreach (var p in assocParents)
                            {
                                string parentSelectedValue = null;

                                parentSelectedValue = Convert.ToString((((Picker)FindPickerControls(p._CASE_ASSOC_TYPE_ID_PARENT)).SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).ID);


                                string parentFieldName = ItemTypes.Where(t => t.AssocTypeID == p._CASE_ASSOC_TYPE_ID_PARENT).FirstOrDefault().Name?.ToUpper();


                                var externalDatasourceinfo = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(ItemTypes.Where(t => t.AssocTypeID == p._CASE_ASSOC_TYPE_ID_PARENT).FirstOrDefault().ExternalDataSourceID.Value), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                string ParentExternalDatasourceName = externalDatasourceinfo?.Result?.AsQueryable()?.FirstOrDefault()?.Name;



                                query = GetQueryStringWithParamaters(query, parentFieldName, parentSelectedValue, ParentExternalDatasourceName);

                                //replace internal entity type
                                if (!string.IsNullOrEmpty(filterQueryOrg))
                                {


                                    var filterQuery = "" + filterQueryOrg;

                                    var cnt = 0;
                                    if (filterQuery.Contains("{'ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID'}")) cnt++;
                                    if (filterQuery.Contains("'%BOXER_ENTITIES%'")) cnt++;
                                    try
                                    {
                                        for (int i = 0; i < cnt; i++)
                                        {
                                            int s1 = filterQuery.IndexOf("/*");
                                            int e1 = filterQuery.IndexOf("*/");
                                            string f1 = filterQuery.Substring(s1, (e1 + 2) - s1);
                                            if (f1.IndexOf("'%BOXER_ENTITIES%'") > 0)
                                            {
                                                s1 = s1 + 2;
                                                string r1 = filterQuery.Substring(s1, e1 - s1);
                                                filterQuery = filterQuery.Replace(f1, " and " + r1);
                                            }
                                            else
                                            {
                                                filterQuery = filterQuery.Replace(f1, "");
                                            }
                                        }
                                        filterQuery = filterQuery.Replace("{'EXTERNAL_DATASOURCE_OBJECT_ID'}", "( " + parentSelectedValue + " )");
                                    }
                                    catch (Exception ex) { }

                                    query = query.Replace("/*{ENTITY_FILTER_QUERY_" + parentCount + "}*/", filterQuery);
                                    parentCount++;

                                }
                            }
                            if (itemType != null)
                            {
                                fieldName = ItemTypes.Where(t => t.AssocTypeID == child._CASE_ASSOC_TYPE_ID_CHILD).FirstOrDefault().Name;
                                var ItemValues = JsonConvert.DeserializeObject<List<GetExternalDataSourceByIdResponse.ExternalDatasource>>(CasesAPIMethods.GetValuesQueryAndConnection(Casetypeid, child._CASE_ASSOC_TYPE_ID_CHILD.ToString(), fieldName/*SelectedCaseType.Name*/,
                                     (itemType.IsRequired == 'Y').ToString(), conn, query).GetValue("ResponseContent").ToString());

                                (control as Picker).ItemsSource = ItemValues;
                                (control as Picker).SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            if (itemType != null)
                            {
                                fieldName = ItemTypes.Where(t => t.AssocTypeID == child._CASE_ASSOC_TYPE_ID_CHILD).FirstOrDefault().Name;
                                List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                GetExternalDataSourceByIdResponse.ExternalDatasource cases_extedataSource = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                {
                                    NAME = "-- Select Item --",
                                    DESCRIPTION = "-- Select Item --",
                                    ID = 0
                                };

                                lst_extdatasource.Add(cases_extedataSource);

                                (control as Picker).ItemsSource = lst_extdatasource;

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }


        }

        public string GetQueryStringWithParamaters(string query, string field, string value, string externalDatasourceName)
        {
            try
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
                        // Replaces all /*{Field|UIName}*/ with the approrate value.
                        if (match.Split('|')[1].ToUpper().Trim() == field.ToUpper().Trim())
                        {
                            results = results.Replace("/*{" + match + "}*/", "And " + match.Split('|')[0] + " = '" + match.Split('|')[1].ToUpper().Trim().Replace(field.ToUpper().Trim(), value.ToString().Trim()) + "'");
                        }
                        else if (externalDatasourceName != null && match.Split('|')[1].ToUpper().Trim() == externalDatasourceName.ToUpper().Trim())
                        {
                            results = results.Replace("/*{" + match + "}*/", "And " + match.Split('|')[0] + " = '" + match.Split('|')[1].ToUpper().Trim().Replace(externalDatasourceName.ToUpper().Trim(), value.ToString().Trim()) + "'");
                        }
                    }
                }

                return results.ToString();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object findControl(string type)
        {
            try
            {

                foreach (StackLayout v in EntryFieldsStack_CasesView.Children)
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
                                    return subitem;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        #region MyRegion : Ribbon Menu & Control 
        private async void Menu_Clicked(object sender, EventArgs e)
        {
            try
            {
                var action = await this.DisplayActionSheet(null, "Cancel", null, "Save", "Save & Exit");


                Dictionary<int, string> dctmetadata = new Dictionary<int, string>();
                SaveCaseTypeRequest savecase = new SaveCaseTypeRequest();
                CreateCaseOptimizedRequest.CreateCaseModelOptimized createcase = new CreateCaseOptimizedRequest.CreateCaseModelOptimized();

                Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> dropDownValues = new Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>();
                Dictionary<int, string> textValues = new Dictionary<int, string>();
                List<CreateCaseOptimizedRequest.LinkValue> linkValue = new List<CreateCaseOptimizedRequest.LinkValue>();

                #region Dynamic Control Fill 
                foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {

                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name.ToLower() != "stacklayout")
                        {
                            var en = new Entry();
                            en.FontFamily = "Soin Sans Neue";
                            var picker = new Picker();
                            picker.TextColor = Color.Gray;
                            var editor = new Editor();
                            editor.FontFamily = "Soin Sans Neue";
                            var datepicker = new DatePicker();
                            datepicker.TextColor = Color.Gray;

                            if (ty.Name.ToLower() == "entry")


                            {

                                try
                                {
                                    en = (Entry)xy;
                                    if (en.StyleId == _TitleFieldControlID)
                                    {
                                        savecase.caseTitle = en.Text;
                                    }
                                    int Key = int.Parse(en.StyleId.Split('_')[1]?.ToString());
                                    string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                    string FileldName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();

                                    if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(en.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + FileldName, "OK");

                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        textValues.Add(Key, en.Text);
                                    }
                                }
                                catch (Exception ex)
                                {

                                    //throw;
                                }

                            }
                            else if (ty.Name.ToLower() == "picker")
                            {
                                try
                                {
                                    picker = (Picker)xy;


                                    ItemValue itemValue = new ItemValue();

                                    string styletype = picker.StyleId.Split('_')[0].Split('|')[0].ToString();

                                    if (styletype.ToLower() == "d")
                                    {
                                        int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                        string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                        string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                        if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && picker.SelectedIndex == -1)
                                        {
                                            DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                            goto requiredJump;
                                        }
                                        else
                                        {
                                            var val = picker.SelectedItem as ItemValue;
                                            ItemValue it = new ItemValue();
                                            if (val != null)
                                            {
                                                it.Name = val.Name;
                                                it.AssocDecodeID = val.AssocDecodeID;
                                                dropDownValues.Add(Key, it);
                                                dctmetadata.Add(Convert.ToInt32(Key), Convert.ToString(it.Name));
                                            }


                                            // textValues.Add(Key, picker.SelectedItem.ToString());
                                        }


                                    }
                                    else
                                    {

                                        //GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue itemValue = (GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue)picker.SelectedItem;

                                        int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                        if (picker.StyleId.Split('|')[0] == _TitleFieldControlID)
                                        {
                                            savecase.caseTitle = itemValue.Name;

                                        }
                                        try
                                        {

                                            string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                            string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                            if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(FiledName))
                                            {
                                                DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                                goto requiredJump;
                                            }
                                            else
                                            {

                                                if (picker.StyleId.Split('_')[0].ToLower() == "o")
                                                {
                                                    var val = picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                                                    ItemValue it = new ItemValue();
                                                    if (val != null)
                                                    {
                                                        it.Name = val.NAME;
                                                        it.AssocDecodeID = Convert.ToInt32(val.ID);
                                                        dropDownValues.Add(Key, it);
                                                        dctmetadata.Add(Convert.ToInt32(Key), Convert.ToString(it.Name));
                                                    }
                                                }
                                                else
                                                {
                                                    //dropDownValues.Add(Key, <itemValue>.Na);
                                                    var val = picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                                                    ItemValue it = new ItemValue();
                                                    if (val != null)
                                                    {
                                                        it.Name = val.NAME;
                                                        it.AssocDecodeID = Convert.ToInt32(val.ID);
                                                        dropDownValues.Add(Key, it);
                                                        dctmetadata.Add(Convert.ToInt32(Key), Convert.ToString(it.Name));
                                                    }
                                                }

                                            }


                                        }
                                        catch { }

                                    }
                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }
                            }




                            else if (ty.Name.ToLower() == "bordereditor")
                            {

                                try
                                {
                                    editor = (BorderEditor)xy;
                                    if (editor.StyleId == _TitleFieldControlID)
                                    {
                                        savecase.caseTitle = editor.Text;
                                    }


                                    string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString())).FirstOrDefault().IS_REQUIRED.ToString();
                                    string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString()))?.FirstOrDefault()?.NAME?.ToString();
                                    if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(editor.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        textValues.Add(int.Parse(editor.StyleId.Split('_')[1]?.ToString()), editor.Text);
                                    }
                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }
                            }
                            else if (ty.Name.ToLower() == "datepicker")
                            {
                                try
                                {
                                    datepicker = (DatePicker)xy;
                                    textValues.Add(int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), App.DateFormatStringToString(datepicker.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"));
                                }
                                catch (Exception ex)
                                {

                                    //  throw;
                                }


                            }

                        }
                    }
                }
                #endregion

                savecase.caseId = Convert.ToInt32(CaseID);
                savecase.caseTitle = Casetitle;
                savecase.dropDownValues = dropDownValues;
                savecase.currentUser = Functions.UserName;
                var Id1 = GetNoteTypeId(000001);
                savecase.noteTypeID = Id1;
                savecase.caseNote = "";//addcasenotes(CaseID);
                savecase.textValues = textValues;
                savecase.sendAlerts = false;
                savecase.updateCaseListCache = false;

                List<CreateCaseOptimizedRequest.TextValue> createtextValues = new List<CreateCaseOptimizedRequest.TextValue>();
                createtextValues = textValues.Select(v => new CreateCaseOptimizedRequest.TextValue
                {
                    Key = v.Key,
                    Value = v.Value
                }).ToList();

                Dictionary<int, string> dctmetaDataValues = new Dictionary<int, string>();
                foreach (var item in dropDownValues)
                {
                    dctmetaDataValues.Add(item.Key, Convert.ToString(item.Value));

                }

                createcase.caseTitle = Casetitle;
                createcase.currentUser = Functions.UserName;
                createcase.caseType = Convert.ToInt32(Casetypeid);
                createcase.assignTo = "";
                createcase.linkValues = linkValue;
                createcase.textValues = createtextValues;
                createcase.caseNotes = "";
                createcase.TransactionType = App.Isonline == true ? "M" : "T";
                createcase.metaDataValues = dctmetadata;

                //if (action.Contains("Save") && !action.Contains("Save & Exit"))
                //{
                //    action = "Save";
                //}
                //else if (action.Contains("Save & Exit"))
                //{
                //    action = "Save & Exit";
                //}

                switch (action)
                {

                    case "Save":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        await Task.Run(() =>
                        {
                            Task<int> save = SaveAndUpdate(savecase, createcase);
                            save.Wait();
                        });

                        txt_CasNotes.Text = "";

                        Groups.Clear();
                        EntryFieldsStack_CasesView.Children.Clear();
                        _allGroups.Clear();

                        if (!flgvisible)
                            GetCasesDetails();

                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        //   Functions.

                        //await Navigation.PushAsync(new ViewCasePage(CaseID, Casetypeid, Casetitle));


                        break;

                    case "Save & Exit":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        await Task.Run(() =>
                        {
                            Task<int> saveExit = SaveAndUpdate(savecase, createcase);
                            saveExit.Wait();
                        });
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        await Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(Casetypeid), "", Casetitle));
                        break;

                }

            }
            catch (Exception ex)
            {
            }
            requiredJump: int abc = 0;

        }
        #endregion
        public async Task<int> SaveAndUpdate(SaveCaseTypeRequest savecase, CreateCaseOptimizedRequest.CreateCaseModelOptimized createcase, object obj = null, bool Isapproved = false, bool Isdecline = false)
        {
            int i = 0;
            return i = await CasesSyncAPIMethods.StoreAndUpdateCase(App.Isonline, Convert.ToInt32(Casetypeid), savecase, txt_CasNotes.Text, Functions.UserName, App.DBPath, createcase, null, obj, GetNoteTypeId(000001).ToString(), Isapproved, Isdecline, Functions.UserFullName);
        }

        private void gridCasesnotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                gridCasesnotes.SelectedItem = null;
            }
            catch (Exception ex)
            {
            }
        }

        void emailLink(SaveCaseTypeRequest savecase)
        {
            try
            {
                string shareurl = String.Empty;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    string subject = savecase.currentUser + " wants to share this " + savecase.caseTitle;
                    string body = "please visit this url:  http://cases-s-15.boxerproperty.com/ViewCase.aspx?CaseID=" + savecase.caseId;
                    var email = Regex.Replace("yourcontact@mail.example.com", @"[^\u0000-\u00FF]", string.Empty);
                    shareurl = "mailto:" + email + "?subject=" + WebUtility.UrlEncode(subject) + "&body=" + WebUtility.UrlEncode(body);
                    // shareurl = WebUtility.UrlEncode(shareurl).Replace("+", "");
                }
                else
                {
                    //for Android it is not necessary to code nor is it necessary to assign a destination email
                    string subject = savecase.currentUser + "wants to share this" + savecase.caseTitle;
                    string body = "please visit this url:  http://cases-s-15.boxerproperty.com/ViewCase.aspx?CaseID=" + savecase.caseId;
                    shareurl = "mailto:?subject=" + subject + "&body=" + body;
                }
                Device.OpenUri(new Uri(shareurl));
            }
            catch (Exception ex)
            {

            }
        }

        void copylink(SaveCaseTypeRequest _Casedataclass1)
        {
            try
            {
                string body = "http://cases-s-15.boxerproperty.com/ViewCase.aspx?CaseID=" + _Casedataclass1.caseId;
            }

            catch (Exception ex)
            {
            }
        }

        #region Cases Calculation 
        public string SetCalControls(List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal, string Id)
        {
            try
            {
                //calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
                string sControl = string.Empty;
                KeyValuePair<List<GetCaseTypesResponse.ItemType>, string> Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);

                foreach (var item in sControls)
                {
                    foreach (var itm in sControlscal)
                    {
                        string strCalformula = itm.CalculationFormula;
                        if (!string.IsNullOrEmpty(strCalformula))
                        {
                            if (!string.IsNullOrEmpty(itm.SystemCode))
                            {
                                if (strCalformula.Contains(Convert.ToString(itm?.SystemCode)) || strCalformula.Contains(Convert.ToString(itm.Description == null ? "null" : itm.Description)) || strCalformula.Contains(Convert.ToString(itm.CalculationFormula == null ? "null" : itm.CalculationFormula)))
                                {
                                    sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                    Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                                    calculationsFieldlist.Add(Itemcontrols);
                                }
                            }
                            else
                            {
                                if (strCalformula.Contains(Convert.ToString(item.Description == null ? "null" : item.Description)) || strCalformula.Contains(Convert.ToString(item.Name == null ? "null" : item.Name)))
                                {
                                    sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                    Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                                    calculationsFieldlist.Add(Itemcontrols);
                                }

                            }
                        }


                    }

                }
                return sControl;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public string SetCalControlsForExd(List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal, string Id = "", string selectedValue = "")
        {
            try
            {
                //calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
                string sControl = string.Empty;
                KeyValuePair<List<GetCaseTypesResponse.ItemType>, string> Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                foreach (var item in sControls)
                {
                    if (item.ExternalDataSourceID != null)
                    {

                        var sxternal = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(item.ExternalDataSourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                        sxternal.Wait();
                        if (sxternal.Result != null)
                        {
                            ExternalDatasourceInfo exd = sxternal.Result.FirstOrDefault();

                            if (exd != null)
                            {
                                foreach (var itm in sControlscal)
                                {
                                    string strCalformula = itm.CalculationFormula;
                                    if (!string.IsNullOrEmpty(strCalformula))
                                    {
                                        if (!string.IsNullOrEmpty(itm.SystemCode))
                                        {
                                            if (strCalformula.Contains(Convert.ToString(exd?.Name)) || strCalformula.Contains(Convert.ToString(itm?.SystemCode)) || strCalformula.Contains(Convert.ToString(itm.Description == null ? "null" : itm.Description)))
                                            {
                                                sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                                Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<GetCaseTypesResponse.ItemType>, string, string>(sControls, sControl, selectedValue));
                                            }
                                            else if (strCalformula.Contains(Convert.ToString(item.Description == null ? "null" : item.Description)) || strCalformula.Contains(Convert.ToString(item.Name == null ? "null" : item.Name)))
                                            {
                                                sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                                Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<GetCaseTypesResponse.ItemType>, string, string>(sControls, sControl, selectedValue));
                                            }
                                        }
                                        else
                                        {
                                            if (strCalformula.Contains(Convert.ToString(exd?.Name)) || strCalformula.Contains(Convert.ToString(itm.Description == null ? "null" : itm.Description)))
                                            {
                                                sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                                Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<GetCaseTypesResponse.ItemType>, string, string>(sControls, sControl, selectedValue));
                                            }
                                            else if (!string.IsNullOrEmpty(item.SystemCode))
                                            {
                                                if (strCalformula.ToLower().Contains(Convert.ToString(exd?.Name?.ToLower())) || strCalformula.Contains(Convert.ToString(item?.SystemCode)) || strCalformula.ToLower().Contains(Convert.ToString(item.Name?.ToLower() ?? "null")))
                                                {
                                                    sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                                    Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                                                    calculationsFieldlist.Add(Itemcontrols);
                                                    lstcalculationsFieldlist.Add(new Tuple<List<GetCaseTypesResponse.ItemType>, string, string>(sControls, sControl, selectedValue));
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                return sControl;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        private void DO_DateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                DatePicker en = (DatePicker)sender;
                DyanmicSetCalc(en.StyleId);
            }
            catch (Exception ex)
            {
            }
        }

        private void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                Entry en = (Entry)sender;

                DyanmicSetCalc(en.StyleId);
            }
            catch (Exception ex)
            {

            }
        }

        private void entry_number_Unfocused(object sender, FocusEventArgs e)
        {
            try
            {
                Entry en = (Entry)sender;

                DyanmicSetCalc(en.StyleId);
            }
            catch (Exception ex)
            {

            }
        }

        public void DyanmicSetCalc(string CurrentStyleId)
        {
            try
            {
                if (!isload)
                    return;

                object cntrl = new object();
                bool ISadd = false;
                string selectedId = string.Empty;
                string selectedname = string.Empty;

                foreach (var item in sControls.Where(v => v.AssocFieldType != 'E' || v.AssocFieldType != 'O'))
                {


                    foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                    {
                        foreach (var subitem in infofield.Children)
                        {

                            Type ty = subitem.GetType();
                            if (ty.Name.ToLower() != "stacklayout")
                            {
                                if (ty.Name.ToLower() == "picker")
                                {
                                    var picker = (Picker)subitem;
                                    if (picker.StyleId == CurrentStyleId && !ISadd)
                                    {
                                        ISadd = true;
                                        cntrl = subitem;
                                        dynamic selectedValue = null;
                                        try
                                        {
                                            selectedValue = picker.SelectedItem as GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue;
                                            selectedId = Convert.ToString(selectedValue.ID);
                                            selectedname = Convert.ToString(selectedValue.Name);
                                            if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                            {
                                                assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + selectedId);
                                                assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), selectedname);
                                            }
                                            else
                                            {
                                                assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + selectedId;
                                                assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = selectedname;
                                            }
                                        }
                                        catch
                                        {
                                            selectedId = Convert.ToString((picker.SelectedItem as Cases_extedataSource)?.ID);
                                            selectedname = (picker.SelectedItem as Cases_extedataSource)?.NAME;
                                            assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + Convert.ToString((picker.SelectedItem as Cases_extedataSource)?.ID));
                                            assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), (picker.SelectedItem as Cases_extedataSource)?.NAME);
                                        }
                                    }
                                }
                                else if (ty.Name.ToLower() == "entry")
                                {
                                    var en = (Entry)subitem;
                                    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                    {
                                        if (en.StyleId == CurrentStyleId && !ISadd)
                                        {
                                            ISadd = true;

                                            if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                            {
                                                assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + Convert.ToString(CurrentStyleId));
                                            }
                                            else
                                            {
                                                assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + Convert.ToString(CurrentStyleId);

                                            }
                                            if (!assocFieldTexts.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                            {
                                                assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), en.Text);
                                            }
                                            else
                                            {
                                                assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = en.Text;
                                            }
                                            cntrl = subitem;
                                        }
                                    }
                                }
                                else if (ty.Name.ToLower() == "datepicker")
                                {
                                    var en = (DatePicker)subitem;
                                    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                    {
                                        if (en.StyleId == CurrentStyleId && !ISadd)
                                        {
                                            ISadd = true;

                                            if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))

                                            {

                                                assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + Convert.ToString(CurrentStyleId));

                                                assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), App.DateFormatStringToString(en.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"));

                                            }
                                            else

                                            {

                                                assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + Convert.ToString(CurrentStyleId);

                                                assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = App.DateFormatStringToString(en.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy");

                                            }

                                            cntrl = subitem;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                string CalTxtbox = string.Empty;
                var x = cntrl.GetType();
                if (x.Name == "Picker")
                {
                    if (CurrentStyleId.Contains("d"))
                        CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);

                    else
                        CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0], (selectedId == "-1" ? "0|" + selectedname : selectedId + "|" + selectedname));
                }
                else if (x.Name == "Entry")
                {
                    CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                }
                else if (x.Name == "DatePicker")
                {

                    CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                }

                Task.Run(() =>
                {
                    setStaticCal(calculationsFieldlist.Where(v => v.Key.FirstOrDefault().AssocTypeID == Convert.ToInt32(CurrentStyleId.Split('_')[1])).ToList());
                });

                //if (!string.IsNullOrEmpty(CalTxtbox))
                //{

                //    int cnt = 0;
                //    string sCalId = string.Empty;
                //    foreach (var item in calculationsFieldlist)
                //    {
                //        sCalId = item.Value?.ToString();
                //        cnt++;
                //        if (cnt > calculationsFieldlist.Count)
                //            return;

                //        RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();
                //        rf.assocFieldCollection = JsonConvert.SerializeObject(sControlsList);
                //        rf.assocFieldValues = assocFieldValues;
                //        rf.assocFieldTexts = assocFieldTexts;
                //        rf.calculatedAssocId = sCalId;
                //        rf.sdateFormats = "MM/dd/yyyy";


                //        var Result = CasesSyncAPIMethods.CallCalculations(App.Isonline, App.DBPath, rf);
                //        Result.Wait();

                //        foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                //        {
                //            foreach (var subitem in infofield.Children)
                //            {
                //                var xy = subitem;
                //                Type ty = xy.GetType();
                //                if (ty.Name.ToLower() != "stacklayout")
                //                {
                //                    if (ty.Name.ToLower() == "entry")
                //                    {
                //                        var en = (Entry)xy;
                //                        if (en.StyleId == Convert.ToString(sCalId.ToLower().Substring(sCalId.ToLower().IndexOf('_') + 1)).ToLower())
                //                        {
                //                            en.Text = Convert.ToString(Result.Result);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

            }
        }

        public void DyanmicSetCalcexd(string CurrentStyleId, List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal)
        {
            try
            {
                object cntrl = new object();
                string selectedId = string.Empty;
                string selectedname = string.Empty;
                foreach (var item in sControls)
                {
                    foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                    {
                        foreach (var subitem in infofield.Children)
                        {

                            Type ty = subitem.GetType();
                            if (ty.Name.ToLower() != "stacklayout")
                            {
                                if (ty.Name.ToLower() == "picker")
                                {
                                    var picker = (Picker)subitem;
                                    if (picker.StyleId == CurrentStyleId)
                                    {
                                        cntrl = subitem;
                                        dynamic selectedValue = null;
                                        try
                                        {
                                            selectedValue = picker.SelectedItem as GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue;
                                            selectedId = Convert.ToString(selectedValue.ID);
                                            selectedname = Convert.ToString(selectedValue.Name);
                                            if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                            {
                                                assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + selectedId);
                                                assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), selectedname);
                                            }
                                            else
                                            {
                                                assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + selectedId;
                                                assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = selectedname;
                                            }
                                        }
                                        catch
                                        {
                                            selectedValue = picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                                            selectedId = Convert.ToString(selectedValue.ID);
                                            selectedname = selectedValue.NAME;
                                            if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                            {
                                                assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + Convert.ToString((picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource)?.ID));
                                                assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), (picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource)?.NAME);
                                            }
                                            else
                                            {
                                                assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + Convert.ToString((picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource)?.ID);
                                                assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = (picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource)?.NAME;
                                            }

                                        }
                                    }
                                }
                                //else if (ty.Name.ToLower() != "entry")
                                //{
                                //    var en = (DatePicker)subitem;
                                //    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                //    {
                                //        if (en.StyleId == CurrentStyleId)
                                //        {

                                //            cntrl = subitem;
                                //        }
                                //    }
                                //}
                            }
                        }
                    }
                }
                string CalTxtbox = string.Empty;
                var x = cntrl.GetType();
                if (x.Name == "Picker")
                {
                    CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControlscal, selectedId + "|" + selectedname);
                }

                //if (!string.IsNullOrEmpty(CalTxtbox))
                //{

                //    int cnt = 0;
                //    string sCalId = string.Empty;
                //    foreach (var item in calculationsFieldlist)
                //    {
                //        sCalId = item.Value?.ToString();
                //        cnt++;
                //        if (cnt > calculationsFieldlist.Count)
                //            return;

                //        RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();
                //        rf.assocFieldCollection = JsonConvert.SerializeObject(sControlsList);
                //        rf.assocFieldValues = assocFieldValues;
                //        rf.assocFieldTexts = assocFieldTexts;
                //        rf.calculatedAssocId = sCalId;
                //        rf.sdateFormats = "MM/dd/yyyy";

                //        var Result = CasesSyncAPIMethods.CallCalculations(App.Isonline, App.DBPath, rf);

                //        Result.Wait();

                //        foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                //        {
                //            foreach (var subitem in infofield.Children)
                //            {
                //                var xy = subitem;
                //                Type ty = xy.GetType();
                //                if (ty.Name.ToLower() != "stacklayout")
                //                {
                //                    if (ty.Name.ToLower() == "entry")
                //                    {
                //                        var en = (Entry)xy;
                //                        en.FontFamily = "Soin Sans Neue";
                //                        if (en.StyleId == Convert.ToString(sCalId.ToLower().Substring(sCalId.ToLower().IndexOf('_') + 1)).ToLower())
                //                        {
                //                            en.Text = Convert.ToString(Result.Result);
                //                        }
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

                //throw ex;
            }
        }

        public async void setStaticCal(List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>> calculationsFieldlist)
        {
            try
            {
                int cnt = 0;
                string sCalId = string.Empty;
                foreach (var item in calculationsFieldlist)
                {
                    sCalId = item.Value?.ToString();
                    cnt++;
                    if (cnt > calculationsFieldlist.Count)
                        return;

                    RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();
                    rf.assocFieldCollection = JsonConvert.SerializeObject(sControlsList);
                    rf.assocFieldValues = assocFieldValues;
                    rf.assocFieldTexts = assocFieldTexts;
                    rf.calculatedAssocId = sCalId;
                    rf.sdateFormats = "MM/dd/yyyy";

                    var Result = CasesSyncAPIMethods.CallCalculations(App.Isonline, App.DBPath, rf);

                    Result.Wait();

                    var cntrol = FindCasesControls(Convert.ToInt32(sCalId.Split('_')[2])) as Entry;
                    if (cntrol != null)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                                      {
                                          cntrol.Text = Convert.ToString(Result.Result);
                                      });
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        private void HomeMenu_Click(object sender, EventArgs e)
        {
            try
            {
                App.GotoHome(this);
            }
            catch (Exception)
            {
            }
        }

        private async void Navigation_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Switch to ");
                if (App.Isonline)
                    sb.Append("Offline Mode");
                else
                    sb.Append("Online Mode");

                string[] buttons;
                buttons = new string[] { sb.ToString(), "Run Synchronization", "Assign", "Return to Last Assigner", "Return to Last Assignee","Approve and Return", "Approve and Assign","Decline and Return", "Decline and Assign",
                                                           "Close Case", "Take Ownership", "Email Link","Add Attachment","Activity Log", "Logout" };
                var action = await this.DisplayActionSheet(null, "Cancel", null, buttons);

                if (action.ToLower().Contains("offline"))
                    action = "offline";
                else if (action.ToLower().Contains("online"))
                    action = "online";

                #region create Json
                SaveCaseTypeRequest savecase = new SaveCaseTypeRequest();
                CreateCaseOptimizedRequest.CreateCaseModelOptimized createcase = new CreateCaseOptimizedRequest.CreateCaseModelOptimized();

                Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> dropDownValues = new Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>();
                Dictionary<int, string> textValues = new Dictionary<int, string>();
                List<CreateCaseOptimizedRequest.LinkValue> linkValue = new List<CreateCaseOptimizedRequest.LinkValue>();

                #region  Dynamic Control Fill 
                foreach (StackLayout infofield in EntryFieldsStack_CasesView.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {

                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name.ToLower() != "stacklayout")
                        {
                            var en = new Entry();
                            var picker = new Picker();
                            var editor = new Editor();
                            var datepicker = new DatePicker();

                            if (ty.Name.ToLower() == "entry")
                            {
                                en = (Entry)xy;
                                if (en.StyleId == _TitleFieldControlID)
                                {
                                    savecase.caseTitle = en.Text;
                                }
                                int Key = int.Parse(en.StyleId.Split('_')[1]?.ToString());
                                string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                string FileldName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();

                                if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(en.Text))
                                {
                                    DisplayAlert("Field Required.", "Please enter valid data in " + FileldName, "OK");

                                    goto requiredJump;
                                }
                                else
                                {
                                    textValues.Add(Key, en.Text);
                                }

                            }
                            else if (ty.Name.ToLower() == "picker")
                            {
                                picker = (Picker)xy;


                                ItemValue itemValue = new ItemValue();

                                string styletype = picker.StyleId.Split('_')[0].Split('|')[0].ToString();

                                if (styletype.ToLower() == "d")
                                {
                                    int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                    string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                    string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                    if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && picker.SelectedIndex == -1)
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        var val = picker.SelectedItem as ItemValue;
                                        ItemValue it = new ItemValue();
                                        if (val != null)
                                        {
                                            it.Name = val.Name;
                                            it.AssocDecodeID = val.AssocDecodeID;
                                            dropDownValues.Add(Key, it);
                                        }


                                        // textValues.Add(Key, picker.SelectedItem.ToString());
                                    }


                                }
                                else
                                {


                                    int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                    if (picker.StyleId.Split('|')[0] == _TitleFieldControlID)
                                    {
                                        savecase.caseTitle = itemValue.Name;

                                    }
                                    try
                                    {

                                        string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                        string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                        if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(FiledName))
                                        {
                                            DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                            goto requiredJump;
                                        }
                                        else
                                        {

                                            if (picker.StyleId.Split('_')[0].ToLower() == "o" || picker.StyleId.Split('_')[0].ToLower() == "e")
                                            {
                                                var val = picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                                                ItemValue it = new ItemValue();
                                                if (val != null)
                                                {
                                                    it.Name = val.NAME;
                                                    it.AssocDecodeID = Convert.ToInt32(val.ID);
                                                    dropDownValues.Add(Key, it);
                                                }
                                            }
                                            else
                                            {
                                                //dropDownValues.Add(Key, <itemValue>.Na);
                                                var val = picker.SelectedItem as Cases_extedataSource;
                                                ItemValue it = new ItemValue();
                                                if (val != null)
                                                {
                                                    it.Name = val.NAME;
                                                    it.AssocDecodeID = val.ID;
                                                    dropDownValues.Add(Key, it);
                                                }
                                            }

                                        }


                                    }
                                    catch { }

                                }
                            }
                            else if (ty.Name.ToLower() == "editor")
                            {
                                editor = (Editor)xy;
                                if (editor.StyleId == _TitleFieldControlID)
                                {
                                    savecase.caseTitle = en.Text;
                                }


                                string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString())).FirstOrDefault().IS_REQUIRED.ToString();
                                string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString()))?.FirstOrDefault()?.NAME?.ToString();
                                if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(en.Text))
                                {
                                    DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                    goto requiredJump;
                                }
                                else
                                {
                                    textValues.Add(int.Parse(editor.StyleId.Split('_')[1]?.ToString()), en.Text);
                                }
                            }
                            else if (ty.Name.ToLower() == "datepicker")
                            {

                                datepicker = (DatePicker)xy;
                                textValues.Add(int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), App.DateFormatStringToString(datepicker.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"));
                            }

                        }
                    }
                }
                #endregion

                savecase.caseId = Convert.ToInt32(CaseID);
                savecase.caseTitle = Casetitle;
                savecase.dropDownValues = dropDownValues;
                savecase.currentUser = Functions.UserName;
                var Id1 = GetNoteTypeId(000001);
                savecase.noteTypeID = Id1;
                savecase.caseNote = "";//addcasenotes(CaseID);
                savecase.textValues = textValues;
                savecase.sendAlerts = false;
                savecase.updateCaseListCache = false;

                List<CreateCaseOptimizedRequest.TextValue> createtextValues = new List<CreateCaseOptimizedRequest.TextValue>();
                createtextValues = textValues.Select(v => new CreateCaseOptimizedRequest.TextValue
                {
                    Key = v.Key,
                    Value = v.Value
                }).ToList();

                Dictionary<int, string> dctmetaDataValues = new Dictionary<int, string>();
                foreach (var item in dropDownValues)
                {
                    dctmetaDataValues.Add(item.Key, Convert.ToString(item.Value));

                }

                createcase.caseTitle = Casetitle;
                createcase.currentUser = Functions.UserName;
                createcase.caseType = Convert.ToInt32(Casetypeid);
                createcase.assignTo = "";
                createcase.linkValues = linkValue;
                createcase.textValues = createtextValues;
                createcase.caseNotes = "";
                createcase.TransactionType = App.Isonline == true ? "M" : "T";
                createcase.metaDataValues = dctmetaDataValues;
                #endregion

                switch (action)
                {
                    case "offline"://Switch to offline
                        App.SetForceFullOnLineOffline(false);
                        App.SetConnectionFlag();
                        break;
                    case "online"://Switch to online
                        App.SetForceFullOnLineOffline(true);
                        App.SetConnectionFlag();
                        break;
                    case "Run Synchronization":
                        if (Functions.CheckInternetWithAlert())
                        {
                            App.isFirstcall = true;
                            App.OnlineSyncRecord();
                        }
                        break;

                    case "Assign":

                        await this.Navigation.PushAsync(new AssignCase(Convert.ToString(savecase.caseId), Casetypeid, createcase, txt_CasNotes.Text, Functions.UserName, "U", savecase, false, false, strTome));

                        break;

                    case "Return to Last Assignee":
                        Task<int> ReturnToLastAssignee = null;
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            await Task.Run(() =>
                            {
                                ReturnToLastAssignee = SaveAndUpdate(savecase, createcase, null);
                                ReturnToLastAssignee.Wait();
                                if (ReturnToLastAssignee != null && ReturnToLastAssignee?.Result > 0)
                                {
                                    ReturnCaseToLastAssigneeRequest objReturnCaseToLastAssignee = new ReturnCaseToLastAssigneeRequest()
                                    {
                                        caseID = Convert.ToInt32(CaseID),
                                        username = Functions.UserName
                                    };
                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList", "T");
                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                        catch
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                    }
                                    CasesSyncAPIMethods.storeReturnToLastAssignee(App.Isonline, objReturnCaseToLastAssignee, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, Functions.UserFullName);
                                }
                            });

                            if (ReturnToLastAssignee != null && ReturnToLastAssignee?.Result > 0)
                            {
                                var answer = DisplayAlert("Return Case To Last Assignee", "Case Assigned Successfully.", "OK");
                                this.Navigation.PushAsync(new ViewCasePage(CaseID, Casetypeid, Casetitle));
                            }
                            else
                            {
                                var answer = DisplayAlert("Return Case To Last Assignee", "Case Not Assigned To Last Assignee. Please Try Again Later.", "OK");
                            }
                        }
                        catch (Exception)
                        {
                        }

                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;

                    case "Return to Last Assigner":
                        Task<int> ReturnToLastAssigner = null;
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            await Task.Run(() =>
                            {
                                ReturnToLastAssigner = SaveAndUpdate(savecase, createcase, null);
                                ReturnToLastAssigner.Wait();
                                if (ReturnToLastAssigner != null && ReturnToLastAssigner?.Result > 0)
                                {
                                    ReturnCaseToLastAssignerRequest objReturnCaseToLastAssigner = new ReturnCaseToLastAssignerRequest()
                                    {
                                        caseid = Convert.ToInt32(CaseID),
                                        username = Functions.UserName
                                    };

                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList", "T");
                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    offlinerecord.Wait();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                        catch
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                    }
                                    if (offlinerecord.Result?.IS_ONLINE == true)
                                        CasesSyncAPIMethods.storeReturnToLastAssigner(App.Isonline, objReturnCaseToLastAssigner, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle);
                                    else
                                        CasesSyncAPIMethods.storeReturnToLastAssigner(App.Isonline, objReturnCaseToLastAssigner, Convert.ToInt32(Casetypeid), Convert.ToString(ReturnToLastAssigner.Result), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle);
                                }
                            });

                            if (ReturnToLastAssigner != null && ReturnToLastAssigner?.Result > 0)
                            {
                                var answer = DisplayAlert("Return Case To Last Assigner", "Case Assigned Successfully.", "OK");
                                this.Navigation.PushAsync(new ViewCasePage(CaseID, Casetypeid, Casetitle));
                            }
                            else
                            {
                                var answer = DisplayAlert("Return Case To Last Assigner", "Case Not Assigned To Last Assigner. Please Try Again Later.", "OK");
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;

                    case "Approve and Return":
                        Task<int> ApproveAndReturn = null;
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            await Task.Run(() =>
                            {
                                ApproveAndReturn = SaveAndUpdate(savecase, createcase, null);
                                ApproveAndReturn.Wait();
                                if (ApproveAndReturn != null && ApproveAndReturn.Result > 0)
                                {
                                    ReturnCaseToLastAssigneeRequest objReturnCaseToLastAssignee = new ReturnCaseToLastAssigneeRequest()
                                    {
                                        caseID = Convert.ToInt32(CaseID),
                                        username = Functions.UserName
                                    };
                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList", "T");
                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                        catch
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                    }

                                    if (offlinerecord.Result.IS_ONLINE == true)

                                        CasesSyncAPIMethods.storeApproveandReturn(App.Isonline, objReturnCaseToLastAssignee, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, "", objview, Casetitle, Functions.UserFullName);
                                    else
                                        CasesSyncAPIMethods.storeApproveandReturn(App.Isonline, objReturnCaseToLastAssignee, Convert.ToInt32(Casetypeid), Convert.ToString(ApproveAndReturn.Result), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, "", objview, Casetitle, Functions.UserFullName);
                                }
                            });

                            if (ApproveAndReturn != null && ApproveAndReturn.Result > 0)
                            {
                                var answer = DisplayAlert("Approve and Return", "Case Approve and Return Successfully.", "OK");
                                this.Navigation.PushAsync(new ViewCasePage(CaseID, Casetypeid, Casetitle));
                            }
                            else
                            {
                                var answer = DisplayAlert("Approve and Return", "Case Not Approve and Return. Please Try Again Later.", "OK");
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;

                    case "Approve and Assign":
                        await this.Navigation.PushAsync(new AssignCase(Convert.ToString(savecase.caseId), Casetypeid, createcase, txt_CasNotes.Text, Functions.UserName, "U", savecase, true, false, strTome));
                        txt_CasNotes.Text = "";
                        break;

                    case "Decline and Return":
                        Task<int> DeclineAndReturn = null;
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            await Task.Run(() =>
                            {
                                DeclineAndReturn = SaveAndUpdate(savecase, createcase, null);
                                DeclineAndReturn.Wait();
                                if (DeclineAndReturn != null && DeclineAndReturn.Result > 0)
                                {
                                    DeclineAndReturnRequest objDeclineAndReturn = new DeclineAndReturnRequest()
                                    {
                                        caseid = Convert.ToInt32(CaseID),
                                        Username = Functions.UserName,
                                        notes = txt_CasNotes.Text
                                    };

                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList", "T");
                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                        catch
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                    }
                                    CasesSyncAPIMethods.storeDeclineAndReturn(App.Isonline, objDeclineAndReturn, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, Functions.UserFullName);

                                }
                            });

                            if (DeclineAndReturn != null && DeclineAndReturn.Result > 0)
                            {
                                var answer = DisplayAlert("Decline and Return", "Case Decline and Return Successfully.", "OK");
                                this.Navigation.PushAsync(new ViewCasePage(CaseID, Casetypeid, Casetitle));
                            }
                            else
                            {
                                var answer = DisplayAlert("Decline and Return", "Case Not Decline and Return. Please Try Again Later.", "OK");
                            }
                        }
                        catch (Exception)
                        {
                        }

                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    case "Decline and Assign":

                        await this.Navigation.PushAsync(new AssignCase(Convert.ToString(savecase.caseId), Casetypeid, createcase, txt_CasNotes.Text, Functions.UserName, "U", savecase, false, true, strTome));

                        txt_CasNotes.Text = string.Empty;
                        break;

                    case "Close Case":
                        Task<int> Close = null;
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                        try
                        {
                            await Task.Run(() =>
                            {
                                Close = SaveAndUpdate(savecase, createcase, null);
                                Close.Wait();

                                if (Close != null && Close.Result > 0)
                                {
                                    CloseCaseRequest objClose = new CloseCaseRequest()
                                    {
                                        caseID = Convert.ToInt32(CaseID),
                                        user = Functions.UserName
                                    };

                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList", "T");
                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                        catch
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                    }
                                    CasesSyncAPIMethods.storeClose(App.Isonline, objClose, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, Functions.UserFullName);
                                }
                            });

                            if (Close != null && Close.Result > 0)
                            {
                                var answer = DisplayAlert("Close Case", "Case Close Successfully.", "OK");
                                await this.Navigation.PushAsync(new CaseList("casetypeid", Casetypeid, "", ""));
                            }
                            else
                            {
                                var answer = DisplayAlert("Close Case", "Case Not Close. Please Try Again Later.", "OK");
                            }
                        }
                        catch (Exception ex)
                        {
                        }


                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;


                    case "Take Ownership":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            AcceptCaseRequest obj = new AcceptCaseRequest()
                            {
                                CaseId = Convert.ToInt32(CaseID),
                                caseOwnerSAM = Functions.UserName,
                                username = Functions.UserName
                            };
                            Task<int> TakeOwnership = null;

                            await Task.Run(() =>
                            {
                                TakeOwnership = SaveAndUpdate(savecase, createcase, obj);
                                TakeOwnership.Wait();
                            });

                            if (TakeOwnership.Result > 0)
                            {
                                var answer = DisplayAlert("Ownership", "You have Successully Taken Ownership for this case.", "OK");
                                this.Navigation.PushAsync(new ViewCasePage(CaseID, Casetypeid, Casetitle));
                            }
                            else
                            {
                                var answer = DisplayAlert("Ownership", "Ownership for this case not done. Please try again later", "OK");
                            }
                        }
                        catch (Exception)
                        {
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;


                    case "Email Link":
                        emailLink(savecase);
                        break;

                    case "Add Attachment":

                        try
                        {
                            if (App.Isonline)
                            {
                                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                // if (Device.RuntimePlatform != Device.Android)
                                {
                                    try
                                    {
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

                                        long size = file.Path.Length;
                                        byte[] fileBytes = null;
                                        var bytesStream = file.GetStream();
                                        using (var memoryStream = new MemoryStream())
                                        {
                                            bytesStream.CopyTo(memoryStream);
                                            fileBytes = memoryStream.ToArray();
                                        }
                                        size = fileBytes.Count();

                                        string File_Name = string.Empty;

                                        try
                                        {
                                            if (Device.RuntimePlatform == Device.UWP)
                                            {
                                                File_Name = file.Path.Substring(file.Path.LastIndexOf('\\') + 1);
                                            }
                                            else
                                                File_Name = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        string FileId = string.Empty;
                                        await Task.Run(() =>
                                        {
                                            var d = CasesAPIMethods.UploadFileToCase(Convert.ToInt32(CaseID), "", DateTime.Now, File_Name, size.ToString(), fileBytes, "", 'Y', "", 'y', Functions.UserName);

                                            FileId = d.GetValue("ResponseContent").ToString();
                                        });

                                        if (!string.IsNullOrEmpty(FileId?.ToString()) && FileId.ToString() != "[]")
                                        {
                                            string Notes = "User Uploaded File: <a href=\"/DownloadFile.aspx?CaseFileID=" + FileId + "\">" + File_Name + "</a>.<br />Hash Code: <b></b><br/>Description: <br /><img src=\"/DownloadFile.aspx?CaseFileID=" + FileId + "\" alt=\"\" style=\"max-width: {PXWIDE};\" />";
                                            await Task.Run(() =>
                                             {
                                                 CasesSyncAPIMethods.AddCasNotes(App.Isonline, Convert.ToInt32(Casetypeid), CaseID, Notes, Functions.UserName, "19", App.DBPath, Functions.UserFullName);
                                             });


                                            if (!string.IsNullOrEmpty(txt_CasNotes.Text))
                                            {
                                                CasesSyncAPIMethods.AddCasNotes(App.Isonline, Convert.ToInt32(Casetypeid), CaseID, txt_CasNotes.Text, Functions.UserName, "19", App.DBPath, Functions.UserFullName);

                                            }
                                            Functions.ShowtoastAlert("File Attached Successfully.");
                                        }
                                        else
                                        {
                                            Functions.ShowtoastAlert("Something went wrong in File Attachment. Please try again later.");
                                        }
                                        gridCasesnotes.ItemsSource = Groups;
                                        txt_CasNotes.Text = string.Empty;
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
                            else
                                Functions.ShowtoastAlert("Please Go online to use this functionality!");

                            reloadNotesArea();
                        }
                        catch (Exception)
                        {
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;

                    case "Activity Log":
                        //await this.Navigation.PushAsync(new CaseActivityLog(savecase.caseId.ToString()));
                        break;
                    case "Logout":
                        App.Logout(this);
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            requiredJump: int abc = 0;
        }
    }
}