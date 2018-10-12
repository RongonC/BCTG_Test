using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.GetTypeValuesByAssocCaseTypeExternalDSResponse;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewcasePage_New : ContentPage
    {

        ReturnCaseToLastAssigneeResponse.UserInfo casedatavalue;
        ObservableCollection<CasesNotesGroup> CasesnotesGroups = new ObservableCollection<CasesNotesGroup>();
        CaseData _Casedata;

        public string strTome = string.Empty;
        public string Casetitle = string.Empty;
        public string Casetypeid = string.Empty;
        public string CaseID = string.Empty;

        string _TitleFieldControlID = string.Empty;
        List<GetCaseTypesResponse.ItemType> sControls = new List<GetCaseTypesResponse.ItemType>();
        List<GetCaseTypesResponse.ItemType> sControlsList = new List<GetCaseTypesResponse.ItemType>();
        string ContolrLst = string.Empty;
        Dictionary<string, string> assocFieldValues = new Dictionary<string, string>();
        Dictionary<string, string> assocFieldTexts = new Dictionary<string, string>();
        string ischeckcalControl = string.Empty;
        List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>> calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
        List<Tuple<List<GetCaseTypesResponse.ItemType>, string, string>> lstcalculationsFieldlist = new List<Tuple<List<GetCaseTypesResponse.ItemType>, string, string>>();
        List<AssocCascadeInfo> AssocTypeCascades = new List<AssocCascadeInfo>();
        BorderEditor txt_CasNotes = new BorderEditor();
        // Button btnIsRefresh = new Button();
        Label WarningLabel = new Label();
        public bool Onlineflag = true;
        string str_CaseFooter = string.Empty;
        bool IsModifiedDate_Same = false;

        List<spi_MobileApp_GetTypesByCaseTypeResult> AssignControlsmetadata = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
        StackLayout layout15 = new StackLayout();



        public ViewcasePage_New()
        {
            InitializeComponent();
            Casetitle = "New View case Page";
            CaseID = "1990";
            Casetypeid = "22";
            strTome = "";
            WarningLabel = new Label()
            {
                Text = "Checking for update... Click here to force.",
                BackgroundColor = Color.White,
                FontSize = 14,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalOptions = LayoutOptions.Center
            };
            WarningLabel.IsVisible = true;
            grd_warning.Children.Add(WarningLabel);
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetCasesData();
            if (App.Isonline)
            {
                await Task.Run(() =>
                {
                    try
                    {
                        var temp = CasesAPIMethods.GetCaseModifiedDate(Convert.ToInt32(CaseID), Convert.ToDateTime(_Casedata.ModifiedDateTime));
                        if (!string.IsNullOrEmpty(Convert.ToString(temp)))
                            str_CaseFooter = Convert.ToString(temp);
                    }
                    catch (Exception)
                    {
                    }
                });

                var spl = str_CaseFooter.Split('|').ToList();

                try
                {
                    if (Convert.ToDateTime(_Casedata.ModifiedDateTime) < Convert.ToDateTime(spl.Where(c => c.Contains("MODIFIED_DATETIME")).FirstOrDefault().ToString().Split('=')[1]))
                    {
                        IsModifiedDate_Same = true;
                    }
                    else
                    {
                        IsModifiedDate_Same = false;
                    }
                }
                catch (Exception ex)
                {
                }

                if (!string.IsNullOrEmpty(strTome))
                    grd_warning.Children.Add(WarningLabel);
            }
            DesignCaseForm_withvalue();
        }
        public async Task GetCasesData()
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        // For Create Case Controls Dynamically
                        var resultR = CasesSyncAPIMethods.AssignControlsAsync(Onlineflag, Convert.ToInt32(Casetypeid), App.DBPath, Functions.UserName);
                        resultR.Wait();
                        AssignControlsmetadata = resultR.Result;
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        var result_getcase = CasesSyncAPIMethods.GetCaseBasicInfo(Onlineflag, Functions.UserName, CaseID, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid, strTome);
                        result_getcase.Wait();
                        _Casedata = result_getcase.Result?.FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
            catch (Exception)
            {
            }
        }

        private void SetBottomBarDetails()
        {
            try
            {
                StackLayout StkFoot = new StackLayout();



                var spl = str_CaseFooter.Split('|').ToList();

                //if (_Casedata != null)
                //{
                //    string CREATED_BY = spl?.Where(c => c.Contains("CREATED_BY"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string CREATED_DATETIME = spl?.Where(c => c.Contains("CREATED_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string MODIFIED_BY = spl?.Where(c => c.Contains("MODIFIED_BY"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string MODIFIED_DATETIME = spl?.Where(c => c.Contains("MODIFIED_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string CASE_ASSGN_TO = spl?.Where(c => c.Contains("CASE_ASSGN_TO"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string CASE_ASSGN_DATETIME = spl?.Where(c => c.Contains("CASE_ASSGN_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string CASE_OWNER = spl?.Where(c => c.Contains("CASE_OWNER"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                //    string CASE_OWNER_DATETIME = spl?.Where(c => c.Contains("CASE_OWNER_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];


                //    _Casedata.CaseAssignedToDisplayName = _Casedata.CaseAssignedToDisplayName ?? _Casedata.CaseAssignedTo;

                //    var s = new FormattedString();
                //    if (_Casedata?.CreateByDisplayName != null)
                //    {
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CREATED_BY)) ? CREATED_BY + "\r\n" : _Casedata.CreateByDisplayName + "\r\n", FontSize = 14 });
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CREATED_DATETIME)) ? Convert.ToString(Convert.ToDateTime(CREATED_DATETIME)) : (Convert.ToDateTime(_Casedata.CreateDateTime)).ToString(), FontSize = 14 });
                //    }

                //    lbl_createname.FormattedText = s;
                //    // StkFoot.Children.Add(lbl_createname);

                //    s = new FormattedString();
                //    if (_Casedata?.CaseAssignedToDisplayName != null)
                //    {
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_ASSGN_TO)) ? CASE_ASSGN_TO + "\r\n" : _Casedata.CaseAssignedToDisplayName + "\r\n", FontSize = 14 });
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_ASSGN_DATETIME)) ? Convert.ToString(Convert.ToDateTime(CASE_ASSGN_DATETIME)) : Convert.ToString(_Casedata.CaseAssignedDateTime == default(DateTime) ? DateTime.Now : _Casedata.CaseAssignedDateTime), FontSize = 14 });

                //    }

                //    // Label lbl_assignto = new Label();
                //    lbl_assignto.FormattedText = s;
                //    // StkFoot.Children.Add(lbl_assignto);

                //    s = new FormattedString();
                //    if (_Casedata?.CaseOwnerDisplayName != null)
                //    {
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_OWNER)) ? CASE_OWNER + "\r\n" : _Casedata.CaseOwnerDisplayName + "\r\n", FontSize = 14 });
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_OWNER_DATETIME)) ? Convert.ToString(Convert.ToDateTime(CASE_OWNER_DATETIME)) : (Convert.ToDateTime(_Casedata.CaseOwnerDateTime)).ToString(), FontSize = 14 });
                //    }

                //    //Label lbl_ownername = new Label();
                //    lbl_ownername.FormattedText = s;
                //    // StkFoot.Children.Add(lbl_ownername);


                //    s = new FormattedString();
                //    if (_Casedata?.ModifiedByDisplayName != null)
                //    {
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(MODIFIED_BY)) ? MODIFIED_BY + "\r\n" : _Casedata.ModifiedByDisplayName + "\r\n", FontSize = 14 });
                //        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(MODIFIED_DATETIME)) ? Convert.ToString(Convert.ToDateTime(MODIFIED_DATETIME)) : (Convert.ToDateTime(_Casedata.ModifiedDateTime)).ToString(), FontSize = 14 });
                //    }

                //    // Label lbl_modifiedname = new Label();
                //    lbl_modifiedname.FormattedText = s;
                //    // StkFoot.Children.Add(lbl_modifiedname);
                //}

                //gridCasesnotes.Footer = StkFoot;

            }
            catch (Exception)
            {
            }
        }


        public async void DesignCaseForm_withvalue()
        {
            try
            {
                CasesnotesGroups.Clear();
                CasesView_ControlStack.Children.Clear();
                bool allowEditCaseData = false; //UPDATE right
                bool showCaseData = false; // Read Right
                List<MetaData> metadatacollection = new List<MetaData>();

                if (_Casedata == null)
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    return;
                }
                else
                {
                    if (AssignControlsmetadata.Count > 0)
                    {
                        Casetypeid = Convert.ToString(_Casedata?.CaseTypeID);
                        Casetitle = _Casedata?.CaseTitle;

                        foreach (var ritem in AssignControlsmetadata)
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
                        string CasetypeSecurity = AssignControlsmetadata[0].SECURITY_TYPE;
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
                            await Navigation.PopAsync();
                        }
                    }

                    #region  Dynamic Control Generate

                    if (AssignControlsmetadata.Count > 0)
                    {
                        //if (_Casedata != null)
                        //{
                        metadatacollection = _Casedata?.MetaDataCollection;

                        //    foreach (var iitem in metadatacollection)
                        //    {
                        //        var exdvalue = AssignControlsmetadata.Where(v => v.ASSOC_TYPE_ID == iitem.AssociatedTypeID).Select(a => a.ExternalDataSourceEntityTypeID);
                        //        if (exdvalue.FirstOrDefault() != null)
                        //        {
                        //            if (!Onlineflag)
                        //                metadatacollection.Where(v => v.AssociatedTypeID == iitem.AssociatedTypeID).Select(s => { s.ExternalDatasourceObjectID = exdvalue.FirstOrDefault(); return s; }).ToList();
                        //        }
                        //    }
                        //}

                        spi_MobileApp_GetTypesByCaseTypeResult itemTypes = new spi_MobileApp_GetTypesByCaseTypeResult();

                        #region Draw Control on Layout

                        for (int i = 0; i < AssignControlsmetadata.Count; i++)
                        {
                            spi_MobileApp_GetTypesByCaseTypeResult ControlsItem = AssignControlsmetadata[i];
                            bool filedsecuritytype_update = false;
                            bool filedsecuritytype_read = false;


                            if (!string.IsNullOrEmpty(ControlsItem.ASSOC_SECURITY_TYPE) && ControlsItem.ASSOC_SECURITY_TYPE.ToLower() != "c")
                            {
                                if (!string.IsNullOrEmpty(ControlsItem.ASSOC_SECURITY_TYPE) || ControlsItem.ASSOC_SECURITY_TYPE.ToLower().Contains("r") || ControlsItem.ASSOC_SECURITY_TYPE.ToLower() == "open")
                                {
                                    filedsecuritytype_read = true;
                                    if (ControlsItem.ASSOC_SECURITY_TYPE.ToLower().Contains("u") || ControlsItem.ASSOC_SECURITY_TYPE.ToLower() == "open")
                                    {
                                        filedsecuritytype_update = true;
                                    }
                                }

                                if (ControlsItem.SYSTEM_CODE?.ToLower() == "title")
                                {
                                    _TitleFieldControlID = ControlsItem.ASSOC_FIELD_TYPE + "_" + ControlsItem.ASSOC_TYPE_ID;
                                    Casetitle = metadatacollection?.Where(c => c.AssociatedTypeID == ControlsItem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;
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
                                    Label1.Text = ControlsItem.NAME;
                                    Label1.HorizontalOptions = LayoutOptions.Start;
                                    Label1.FontSize = 16;
                                    Label1.WidthRequest = 200;

                                    LeftLayout.Children.Add(Label1);
                                    Mainlayout.Children.Add(LeftLayout);

                                    try
                                    {
                                        switch (ControlsItem.ASSOC_FIELD_TYPE.ToLower())
                                        {
                                            case "o":
                                            case "e":

                                                Picker pk = new Picker();
                                                pk.WidthRequest = 200;
                                                pk.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID + "|" + ControlsItem.EXTERNAL_DATASOURCE_ID;
                                                try
                                                {
                                                    //pk.SelectedIndexChanged += Pk_SelectedIndexChanged;

                                                    {
                                                        GetExternalDataSourceByIdResponse.ExternalDatasource cases_extedataSource = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                        {
                                                            NAME = "-- Select Item --",
                                                            DESCRIPTION = "-- Select Item --",
                                                            ID = 0
                                                        };

                                                        List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                                                        if (Onlineflag)
                                                        {
                                                            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                                                            var AssocType = json.GetValue("ResponseContent");
                                                            AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                                                            var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == ControlsItem.ASSOC_TYPE_ID).ToList();
                                                            if (assocChild.Count < 1)
                                                            {
                                                                var result_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(Onlineflag, ControlsItem.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID));
                                                                if (result_extdatasource.Result != null && result_extdatasource.Result.ToString() != "[]")
                                                                {
                                                                    lst_extdatasource.AddRange(result_extdatasource.Result);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(Onlineflag, ControlsItem.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID));

                                                            temp_extdatasource.Wait();
                                                            if (temp_extdatasource.Result.Count > 0)
                                                            {
                                                                lst_extdatasource.AddRange(temp_extdatasource.Result);
                                                            }
                                                        }

                                                        lst_extdatasource.Insert(0, cases_extedataSource);


                                                        pk.ItemsSource = lst_extdatasource;
                                                        pk.ItemDisplayBinding = new Binding("NAME");
                                                    }
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        pk.IsEnabled = false;
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                Mainlayout.Children.Add(pk);
                                                break;

                                            case "d":
                                                Picker pkr = new Picker();
                                                pkr.WidthRequest = 200;
                                                pkr.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID + "|" + ControlsItem.EXTERNAL_DATASOURCE_ID;

                                                try
                                                {
                                                    //  pkr.SelectedIndexChanged += Pk_SelectedIndexChanged;

                                                    List<ItemValue> lst_SSsource = new List<ItemValue>();
                                                    ItemValue Value = new ItemValue
                                                    {
                                                        Name = "-- Select Item --",
                                                        Description = "-- Select Item --",
                                                        ID = 0
                                                    };

                                                    lst_SSsource.Add(Value);

                                                    GetTypeValuesByAssocCaseTypeExternalDSRequest.GetTypeValuesByAssocCaseTypeExternalDS Request = new GetTypeValuesByAssocCaseTypeExternalDSRequest.GetTypeValuesByAssocCaseTypeExternalDS();

                                                    Request.assocCaseTypeID = ControlsItem.ASSOC_TYPE_ID;
                                                    Request.caseTypeID = Convert.ToInt32(Casetypeid);
                                                    Request.caseTypeDesc = ControlsItem.DESCRIPTION;
                                                    Request.systemCode = ControlsItem.SYSTEM_CODE;
                                                    var result_extdatasource1 = CasesSyncAPIMethods.GetTypeValuesByAssocCaseType(Onlineflag, Request, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                                    result_extdatasource1.Wait();
                                                    if (result_extdatasource1.Result != null && result_extdatasource1.Result.ToString() != "[]")
                                                    {
                                                        lst_SSsource = result_extdatasource1?.Result;
                                                    }
                                                    pkr.ItemsSource = lst_SSsource;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                Mainlayout.Children.Add(pkr);
                                                break;

                                            case "x":
                                                BorderEditor editor = new BorderEditor();
                                                editor.StyleId = ControlsItem.ASSOC_FIELD_TYPE + "_" + ControlsItem.ASSOC_TYPE_ID;
                                                try
                                                {
                                                    editor.HeightRequest = 100;
                                                    editor.BorderColor = Color.LightGray;
                                                    editor.BorderWidth = 1;
                                                    editor.CornerRadius = 5;
                                                    editor.FontSize = 16;
                                                    editor.WidthRequest = 200;
                                                    editor.Text = "";

                                                    // ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());
                                                    // if (!string.IsNullOrEmpty(ischeckcalControl))
                                                    // editor.Unfocused += Entry_Unfocused;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                Mainlayout.Children.Add(editor);
                                                break;

                                            case "a":
                                                #region Date Picker
                                                //DatePicker DO = new DatePicker();
                                                //DO.Date = Convert.ToDateTime("01/01/1900");
                                                //DO.Format = "MM/dd/yyyy";
                                                //DO.WidthRequest = 200;
                                                //DO.TextColor = Color.Gray;
                                                //DO.StyleId = "a_" + item.ASSOC_TYPE_ID;
                                                //try
                                                //{
                                                //    if (!filedsecuritytype_update)
                                                //    {
                                                //        DO.IsEnabled = false;
                                                //    }
                                                //    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(DO.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), DO.StyleId?.Split('_')[0]?.ToUpper() + "_" + DO.StyleId?.Split('_')[1]?.Split('|')[0]);
                                                //    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                //        DO.DateSelected += DO_DateSelected;
                                                //    DO.WidthRequest = 200;
                                                //}
                                                //catch (Exception ex)
                                                //{

                                                //    //throw;
                                                //}

                                                //Mainlayout.Children.Add(DO); 
                                                #endregion

                                                #region Date text Box
                                                //Entry DO = new Entry();
                                                //DO.Placeholder = "Enter Date";
                                                //DO.WidthRequest = 200;
                                                //DO.TextColor = Color.Gray;
                                                //DO.Keyboard = Keyboard.Numeric;
                                                //DO.StyleId = "a_" + item.ASSOC_TYPE_ID;
                                                //DO.TextChanged += DO_TextChanged;
                                                //DO.Text = "";
                                                //try
                                                //{
                                                //    if (!filedsecuritytype_update)
                                                //    {
                                                //        DO.IsEnabled = false;
                                                //    }
                                                //    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(DO.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), DO.StyleId?.Split('_')[0]?.ToUpper() + "_" + DO.StyleId?.Split('_')[1]?.Split('|')[0]);
                                                //    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                //        DO.Unfocused += Entry_Unfocused;
                                                //    DO.WidthRequest = 200;
                                                //}
                                                //catch (Exception ex)
                                                //{
                                                //} 
                                                #endregion

                                                #region txt_Date
                                                Entry txt_Date = new Entry();
                                                txt_Date.Placeholder = "Select Date";
                                                txt_Date.WidthRequest = 175;
                                                txt_Date.TextColor = Color.Gray;
                                                txt_Date.Keyboard = Keyboard.Numeric;
                                                txt_Date.StyleId = "a_" + ControlsItem.ASSOC_TYPE_ID;
                                                txt_Date.Text = "";

                                                //ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());

                                                // if (!string.IsNullOrEmpty(ischeckcalControl))
                                                // txt_Date.TextChanged += DO_TextChanged;

                                                #endregion

                                                Image im = new Image();
                                                im.StyleId = "imgcl_" + ControlsItem.ASSOC_TYPE_ID;
                                                im.Source = ImageSource.FromFile("Assets/erase16.png");

                                                #region date_pick
                                                DatePicker date_pick = new DatePicker();
                                                date_pick.IsVisible = false;
                                                date_pick.Format = "MM/dd/yyyy";
                                                date_pick.WidthRequest = 200;
                                                date_pick.TextColor = Color.Gray;
                                                date_pick.StyleId = "dt_" + ControlsItem.ASSOC_TYPE_ID;
                                                #endregion


                                                Mainlayout.Children.Add(txt_Date);
                                                Mainlayout.Children.Add(im);
                                                Mainlayout.Children.Add(date_pick);

                                                txt_Date.Focused += (sender, e) =>
                                                {
                                                    try
                                                    {
                                                        var cnt = (Entry)sender;
                                                        var sty_id = cnt.StyleId?.Split('_')[1];
                                                        var dt_c = FindCasesControls(Convert.ToInt32(sty_id), "DatePicker") as DatePicker;
                                                        Device.BeginInvokeOnMainThread(() =>
                                                        {
                                                            dt_c.Focus();
                                                        });
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                };

                                                var tgr = new TapGestureRecognizer();
                                                tgr.Tapped += async (s, e) =>
                                                {
                                                    Entry C_ent = new Entry();
                                                    try
                                                    {
                                                        var ct = (Image)s;
                                                        var sty_id = ct.StyleId?.Split('_')[1];
                                                        C_ent = FindCasesControls(Convert.ToInt32(sty_id)) as Entry;
                                                        C_ent.Text = "";
                                                    }
                                                    catch (Exception)
                                                    {
                                                        C_ent.Text = "";
                                                    }
                                                };
                                                im.GestureRecognizers.Add(tgr);

                                                if (Device.RuntimePlatform == "iOS")
                                                {
                                                    // date_pick.Unfocused += Date_pick_Unfocused;
                                                }
                                                else
                                                {
                                                    // d//ate_pick.DateSelected += Date_pick_DateSelected;
                                                }
                                                break;

                                            case "t":
                                                Entry entry = new Entry();

                                                entry.StyleId = "t_" + ControlsItem.ASSOC_TYPE_ID;

                                                try
                                                {
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        entry.IsEnabled = false;
                                                    }
                                                    // ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());
                                                    // if (!string.IsNullOrEmpty(ischeckcalControl))
                                                    //    entry.Unfocused += Entry_Unfocused;
                                                    entry.Keyboard = Keyboard.Default;
                                                    entry.Text = "";
                                                }
                                                catch (Exception ex)
                                                {
                                                    entry.Text = "";
                                                }
                                                entry.WidthRequest = 200;
                                                entry.FontSize = 16;
                                                Mainlayout.Children.Add(entry);
                                                break;
                                            case "c":
                                                Entry _entry = new Entry();

                                                _entry.StyleId = "c_" + ControlsItem.ASSOC_TYPE_ID;
                                                _entry.IsEnabled = false;
                                                _entry.WidthRequest = 200;
                                                _entry.FontSize = 16;
                                                _entry.Keyboard = Keyboard.Default;
                                                _entry.Text = "";
                                                Mainlayout.Children.Add(_entry);

                                                break;
                                            case "n":
                                                Entry entry_number = new Entry();
                                                entry_number.WidthRequest = 200;
                                                entry_number.FontSize = 16;
                                                entry_number.Keyboard = Keyboard.Numeric;
                                                entry_number.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID;

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
                                                    //ischeckcalControl = SetCalControls(sControls?.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(),
                                                    // sControls?.Where(v => v.AssocFieldType == 'C').ToList());
                                                    //if (!string.IsNullOrEmpty(ischeckcalControl))
                                                    // entry_number.Unfocused += entry_number_Unfocused;
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
                                    CasesView_ControlStack.Children.Add(Mainlayout);
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



                        CasesView_ControlStack.Children.Add(layout_CNotes);
                        CasesView_ControlStack.Children.Add(layout);
                        if (!allowEditCaseData)
                        {
                            CasesView_ControlStack.IsEnabled = false;
                        }

                        #endregion

                        #region Bind Data on Control
                        foreach (var Metaitem in AssignControlsmetadata)
                        {
                            try
                            {
                                switch (Metaitem.ASSOC_FIELD_TYPE.ToLower())
                                {
                                    case "o":
                                    case "e":

                                        var control = FindPickerControls(Metaitem.ASSOC_TYPE_ID) as Picker;
                                        if (control != null)
                                        {
                                            List<GetExternalDataSourceByIdResponse.ExternalDatasource> cnt_DataSource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                                            cnt_DataSource = control.ItemsSource as List<GetExternalDataSourceByIdResponse.ExternalDatasource>;

                                            try
                                            {
                                                int idd = 0;
                                                if (Onlineflag)
                                                {
                                                    if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    {
                                                        int id = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID);
                                                        if (id > 0)
                                                        {
                                                            idd = cnt_DataSource.IndexOf(cnt_DataSource.Single(v => v.ID == id));
                                                        }
                                                    }
                                                    else if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    {
                                                        int id = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID);
                                                        if (id > 0)
                                                        {
                                                            idd = cnt_DataSource.IndexOf(cnt_DataSource.Single(v => v.ID == id));
                                                        }
                                                    }
                                                    control.SelectedIndex = idd;
                                                }
                                                else
                                                {
                                                    if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    {
                                                        int Sel_id = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID);
                                                        if (Sel_id > 0)
                                                        {
                                                            idd = cnt_DataSource.IndexOf(cnt_DataSource.Single(v => v.ID == Sel_id));
                                                        }
                                                        else if (Sel_id != -1 && Sel_id != 0)
                                                        {
                                                            var SelItmname = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue;
                                                            cnt_DataSource.Add(new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                            {
                                                                ID = Sel_id,
                                                                NAME = SelItmname

                                                            });

                                                            control.ItemsSource = cnt_DataSource;
                                                            control.SelectedIndex = cnt_DataSource.Count - 1;
                                                        }
                                                    }
                                                    else if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    {
                                                        int Ext_Objid = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID);
                                                        if (Ext_Objid > 0)
                                                        {
                                                            idd = cnt_DataSource.IndexOf(cnt_DataSource.Single(v => v.ID == Ext_Objid));
                                                        }
                                                        else if (Ext_Objid != -1 && Ext_Objid != 0)
                                                        {
                                                            var F_Value = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue;
                                                            if (!string.IsNullOrEmpty(F_Value))
                                                            {
                                                                cnt_DataSource.Add(new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                {
                                                                    ID = Ext_Objid,
                                                                    NAME = F_Value
                                                                });
                                                                idd = cnt_DataSource.IndexOf(cnt_DataSource.Single(v => v.ID == Ext_Objid));
                                                                control.ItemsSource = cnt_DataSource;
                                                            }
                                                            control.SelectedIndex = idd;
                                                        }
                                                    }
                                                    control.SelectedIndex = idd;

                                                    #region old
                                                    //if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    //{
                                                    //    string sTextvalue = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                    //    if (!string.IsNullOrEmpty(sTextvalue))
                                                    //    {
                                                    //        try
                                                    //        {
                                                    //            idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalue?.ToLower()?.Trim()).FirstOrDefault());
                                                    //        }
                                                    //        catch
                                                    //        {
                                                    //            List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                    //            var rec = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                    //            if (rec != null)
                                                    //            {
                                                    //                if (lst_extdatasource == null)
                                                    //                {
                                                    //                    GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                    //                    {
                                                    //                        NAME = "-- Select Item --",
                                                    //                        DESCRIPTION = "-- Select Item --",
                                                    //                        ID = 0
                                                    //                    };
                                                    //                    lst_extdatasource1.Add(lst);
                                                    //                    lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                    //                    {
                                                    //                        DESCRIPTION = rec.TextValue,
                                                    //                        NAME = rec.TextValue,
                                                    //                        ID = rec.AssociatedTypeID
                                                    //                    };
                                                    //                    lst_extdatasource1.Add(lst);
                                                    //                    control.ItemsSource = null;
                                                    //                    control.ItemsSource = lst_extdatasource1;
                                                    //                    idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Single(v => v.ID == Metaitem.ASSOC_TYPE_ID));
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //        control.SelectedIndex = idd;
                                                    //    }
                                                    //    else
                                                    //    {
                                                    //        string sTextvalues = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue);
                                                    //        if (!string.IsNullOrEmpty(sTextvalues))
                                                    //        {
                                                    //            try
                                                    //            {
                                                    //                idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalues?.ToLower()?.Trim()).FirstOrDefault());
                                                    //            }
                                                    //            catch
                                                    //            {
                                                    //                List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                    //                var rec = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                    //                if (rec != null)
                                                    //                {
                                                    //                    if (lst_extdatasource == null)
                                                    //                    {
                                                    //                        GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                    //                        {
                                                    //                            NAME = "-- Select Item --",
                                                    //                            DESCRIPTION = "-- Select Item --",
                                                    //                            ID = 0
                                                    //                        };
                                                    //                        lst_extdatasource1.Add(lst);
                                                    //                        lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                    //                        {
                                                    //                            DESCRIPTION = rec.TextValue,
                                                    //                            NAME = rec.TextValue,
                                                    //                            ID = rec.AssociatedTypeID
                                                    //                        };
                                                    //                        lst_extdatasource1.Add(lst);
                                                    //                        control.ItemsSource = null;
                                                    //                        control.ItemsSource = lst_extdatasource1;
                                                    //                        idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Single(v => v.ID == Metaitem.ASSOC_TYPE_ID));
                                                    //                    }
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //        control.SelectedIndex = idd;
                                                    //    }
                                                    //}
                                                    //else if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    //{
                                                    //    List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource1 = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                                    //    var rec = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                    //    if (rec != null)
                                                    //    {
                                                    //        if (lst_extdatasource == null)
                                                    //        {

                                                    //            GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                    //            {
                                                    //                NAME = "-- Select Item --",
                                                    //                DESCRIPTION = "-- Select Item --",
                                                    //                ID = 0
                                                    //            };
                                                    //            lst_extdatasource1.Add(lst);
                                                    //            lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                    //            {
                                                    //                DESCRIPTION = rec.TextValue,
                                                    //                NAME = rec.TextValue,
                                                    //                ID = rec.AssociatedTypeID
                                                    //            };
                                                    //            lst_extdatasource1.Add(lst);
                                                    //            control.ItemsSource = null;
                                                    //            control.ItemsSource = lst_extdatasource1;
                                                    //            idd = lst_extdatasource1.IndexOf(lst_extdatasource1.Single(v => v.ID == Metaitem.ASSOC_TYPE_ID));
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            string sTextvalue = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                    //            if (!string.IsNullOrEmpty(sTextvalue))
                                                    //            {
                                                    //                try
                                                    //                {
                                                    //                    idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalue.ToLower()?.Trim()).FirstOrDefault());
                                                    //                    if (idd == -1)
                                                    //                    {
                                                    //                        var lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                    //                        {
                                                    //                            DESCRIPTION = rec.TextValue,
                                                    //                            NAME = rec.TextValue,
                                                    //                            ID = rec.AssociatedTypeID
                                                    //                        };
                                                    //                        lst_extdatasource.Add(lst);
                                                    //                        control.ItemsSource = null;
                                                    //                        control.ItemsSource = lst_extdatasource;
                                                    //                        idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalue.ToLower()?.Trim()).FirstOrDefault());
                                                    //                    }
                                                    //                }
                                                    //                catch
                                                    //                {

                                                    //                }
                                                    //            }
                                                    //            else
                                                    //            {
                                                    //                string sTextvalues1 = Convert.ToString(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue);
                                                    //                if (!string.IsNullOrEmpty(sTextvalues1))
                                                    //                {
                                                    //                    try
                                                    //                    {
                                                    //                        idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalues1.ToLower()?.Trim()).FirstOrDefault());
                                                    //                        if (idd == -1)
                                                    //                        {
                                                    //                            var lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                    //                            {
                                                    //                                DESCRIPTION = rec.FieldValue,
                                                    //                                NAME = rec.FieldValue,
                                                    //                                ID = rec.AssociatedTypeID
                                                    //                            };
                                                    //                            lst_extdatasource.Add(lst);
                                                    //                            control.ItemsSource = null;
                                                    //                            control.ItemsSource = lst_extdatasource;
                                                    //                            idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION?.ToLower()?.Trim() == sTextvalues1.ToLower()?.Trim()).FirstOrDefault());
                                                    //                        }
                                                    //                    }
                                                    //                    catch
                                                    //                    {

                                                    //                    }
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //    }
                                                    //}
                                                    //control.SelectedIndex = idd; 
                                                    #endregion
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                        break;

                                    case "d":
                                        control = FindPickerControls(Metaitem.ASSOC_TYPE_ID) as Picker;
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
                                                        var i = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID).FirstOrDefault();

                                                        if (i?.AssociatedDecodeName?.ToLower() == lst_SSsource[j]?.Name?.ToLower() || i?.TextValue?.ToLower() == lst_SSsource[j]?.Name?.ToLower())
                                                            break;
                                                    }
                                                    else
                                                        break;
                                                }
                                                control.SelectedIndex = j;
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        break;

                                    case "a":
                                        try
                                        {
                                            var cnt = FindCasesControls(Metaitem.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                Entry DO = new Entry();
                                                DO = cnt as Entry;
                                                DO.TextColor = Color.Gray;
                                                string Dateval = (metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                var dt_c = FindCasesControls(Convert.ToInt32(Metaitem.ASSOC_TYPE_ID), "DatePicker") as DatePicker;

                                                if (!String.IsNullOrEmpty(Dateval))
                                                {
                                                    var Str = App.DateFormatStringToString(Dateval);
                                                    DO.Text = Str;

                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dt_c.Date = Convert.ToDateTime(Dateval);
                                                    });
                                                }

                                                //DatePicker DO = new DatePicker();
                                                //DO = cnt as DatePicker;
                                                //DO.TextColor = Color.Gray;
                                                //string Dateval = (metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                //if (!String.IsNullOrEmpty(Dateval))
                                                //{
                                                //    var Str = App.DateFormatStringToString(Dateval);
                                                //    DateTime dt = Convert.ToDateTime(Str);
                                                //    DO.Date = dt;
                                                //}
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;

                                    case "x":
                                        try
                                        {
                                            var cnt = FindCasesControls(Metaitem.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                BorderEditor bd = cnt as BorderEditor;
                                                bd.FontSize = 16;
                                                bd.FontFamily = "Soin Sans Neue";
                                                bd.Text = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;

                                    case "t":
                                    case "c":
                                    case "n":
                                        try
                                        {
                                            var cnt = FindCasesControls(Metaitem.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                Entry entry = new Entry();
                                                entry.FontSize = 16;
                                                entry = cnt as Entry;
                                                entry.FontFamily = "Soin Sans Neue";
                                                if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "c" && !Onlineflag)
                                                {
                                                    entry.Text = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;
                                                }
                                                else
                                                {
                                                    entry.Text = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;

                                                }
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
                        #endregion
                    }
                    else
                    {
                        await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
            }

            ReloadNotesArea();

            #region Set Bottom Details
            SetBottomBarDetails();
            #endregion

            //if (Onlineflag)
            //    SetStaticCal(ContolrLst);

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                // Do something
                //if (!IsModifiedDate_Same)
                {
                    WarningLabel.IsVisible = false;
                    grd_warning.Children.Remove(WarningLabel);
                }
                return true; // True = Repeat again, False = Stop the timer
            });
        }
        public void ReloadNotesArea()
        {
            //try
            //{
            //    gridCasesnotes.ItemsSource = null;
            //    CasesnotesGroups.Clear();

            //    Task<List<GetCaseNotesResponse.NoteData>> NotesResponse = CasesSyncAPIMethods.GetCaseNotes(Onlineflag, CaseID, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
            //    NotesResponse.Wait();
            //    var Noteslist = NotesResponse?.Result;

            //    ObservableCollection<CasesNotesGroup> Temp = new ObservableCollection<CasesNotesGroup>();
            //    if (Noteslist?.Count > 0)
            //    {
            //        for (int i = 0; i < Noteslist.Count; i++)
            //        {
            //            CasesNotesGroup grp = new CasesNotesGroup("", Convert.ToString(Noteslist[i].CreatedDateTime), Noteslist[i]?.CreatedByUser == null ? Functions.UserFullName : Noteslist[i]?.CreatedByUser?.DisplayName)
            //                {
            //                    new GetCaseNotesResponse.NoteData
            //                    {
            //                        Note = Noteslist[i].Note
            //                    }
            //                };
            //            Temp.Add(grp);
            //        }

            //        foreach (var item in Temp)
            //        {
            //            //if (item.FirstOrDefault().Note.Contains("<img"))
            //            //{
            //            //    item.FirstOrDefault().ImageVisible = true;
            //            //    item.FirstOrDefault().LabelVisible = true;
            //            //    item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
            //            //    item.FirstOrDefault().ImageURL = App.CasesImgURL + "/" + item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1];
            //            //    item.FirstOrDefault().Note = item.FirstOrDefault().Note;
            //            //}
            //            //else
            //            //{
            //            //    item.FirstOrDefault().ImageVisible = false;
            //            //    item.FirstOrDefault().LabelVisible = true;
            //            //    item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
            //            //    item.FirstOrDefault().Note = (item.FirstOrDefault().Note);
            //            //}
            //            if (item.FirstOrDefault().Note.Contains("<img"))
            //            {
            //                item.FirstOrDefault().ImageVisible = true;
            //                item.FirstOrDefault().LabelVisible = true;
            //                item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
            //                item.FirstOrDefault().ImageURL = App.CasesImgURL + "/" + Functions.HTMLToText(item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1]);
            //                item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
            //            }
            //            else
            //            {
            //                item.FirstOrDefault().ImageVisible = false;
            //                item.FirstOrDefault().LabelVisible = true;
            //                item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
            //                item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
            //            }

            //            CasesnotesGroups.Add(item);
            //        }

            //        gridCasesnotes.ItemsSource = null;
            //        gridCasesnotes.ItemsSource = CasesnotesGroups;
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
            //if (CasesnotesGroups.Count <= 0)
            //{
            //    gridCasesnotes.HeightRequest = 0;
            //}
            //else
            //    gridCasesnotes.HeightRequest = 700;
        }

        private object FindPickerControls(int AssocID)
        {
            try
            {
                foreach (StackLayout infofield in CasesView_ControlStack.Children)
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

        private object FindCasesControls(int AssocID, string Cnt_type = "")
        {
            try
            {
                if (string.IsNullOrEmpty(Cnt_type))
                {
                    foreach (StackLayout infofield in CasesView_ControlStack.Children)
                    {
                        foreach (var subitem in infofield.Children)
                        {
                            var xy = subitem;
                            Type ty = xy.GetType();
                            if (ty.Name != "StackLayout")
                            {
                                var id = xy.StyleId.Contains("|") ? xy.StyleId.Split('|')[0] : xy.StyleId;

                                if (id.Contains(AssocID.ToString()))
                                    return xy;
                            }
                        }
                    }
                }
                else if (Cnt_type == "DatePicker")
                {
                    foreach (StackLayout infofield in CasesView_ControlStack.Children)
                    {
                        foreach (var subitem in infofield.Children)
                        {
                            var xy = subitem;
                            Type ty = xy.GetType();
                            if (ty.Name != "StackLayout")
                            {
                                if (ty.Name == "DatePicker")
                                {
                                    if (xy.StyleId.Contains(AssocID.ToString()))
                                    {
                                        return xy;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (Cnt_type == "Image")
                {
                    foreach (StackLayout infofield in CasesView_ControlStack.Children)
                    {
                        foreach (var subitem in infofield.Children)
                        {
                            var xy = subitem;
                            Type ty = xy.GetType();
                            if (ty.Name != "StackLayout")
                            {
                                if (ty.Name == "Image")
                                {
                                    if (xy.StyleId.Contains(AssocID.ToString()))
                                    {
                                        return xy;
                                    }
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

        private void GridCasesnotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}