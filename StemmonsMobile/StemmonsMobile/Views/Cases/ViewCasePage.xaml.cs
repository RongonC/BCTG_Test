using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.Connectivity;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using StemmonsMobile.Views.People_Screen;
using StemmonsMobile.Views.View_Case_Origination_Center;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using Plugin.Clipboard;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.GetTypeValuesByAssocCaseTypeExternalDSResponse;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ViewCasePage : ContentPage
    {

        ReturnCaseToLastAssigneeResponse.UserInfo casedatavalue;
        ObservableCollection<CasesNotesGroup> CasesnotesGroups = new ObservableCollection<CasesNotesGroup>();
        BasicCase _Casedata;

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
        Label WarningLabel = new Label();

        string Assign_Sam = string.Empty;
        string Create_Sam = string.Empty;
        string Modify_Sam = string.Empty;
        string Owner_Sam = string.Empty;

        int iSelectedItemlookupId = 0;

        List<ExternalDatasourceValue> lstexternaldatasource = new List<ExternalDatasourceValue>();

        Dictionary<int, List<ExternalDatasourceValue>> lstextdatasourceHistory = new Dictionary<int, List<ExternalDatasourceValue>>();

        ExternalDatasourceValue extDSdefaultValues = new ExternalDatasourceValue
        {
            NAME = "-- Select Item --",
            DESCRIPTION = "-- Select Item --",
            ID = 0
        };

        List<MetaData> metadatacollection = new List<MetaData>();

        public ViewCasePage(string CASEID, string CASETYPEID, string CASETITLE, string ToMe = "")
        {
            InitializeComponent();
            Casetitle = CASETITLE;
            CaseID = CASEID;
            Casetypeid = CASETYPEID;
            strTome = ToMe;
            CasesnotesGroups.Clear();
            grd_warning.Children.Clear();


            CasesView_ControlStack.Children.Clear();

            #region WarningLabel
            WarningLabel = new Label()
            {
                Text = "Checking for update... Click here to force.",
                BackgroundColor = Color.White,
                FontSize = 14,
                LineBreakMode = LineBreakMode.WordWrap,
                HorizontalOptions = LayoutOptions.Center
            };

            var tgr = new TapGestureRecognizer();
            tgr.Tapped += async (s, e) =>
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                Onlineflag = true;
                await GetCasesData();
                LoadControls();
                WarningLabel.IsVisible = false;
                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

            };
            WarningLabel.GestureRecognizers.Add(tgr);
            WarningLabel.IsVisible = true;
            #endregion

            BindingContext = new CasesPulltorefreshViewmodel(this);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var refreshView = new PullToRefreshLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = Main_stack,
                RefreshColor = Color.FromHex("#3498db")
            };

            refreshView.SetBinding<CasesPulltorefreshViewmodel>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            refreshView.SetBinding<CasesPulltorefreshViewmodel>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);

            masterGrid.Children.Add(refreshView, 0, 2);
            //masterGrid.Children.Add(btm_stack, 0, 3);

            //abs_layout.Children.Add(refreshView);

            // abs_layout.Children.Add(popupLT);
            abs_layout.Children.Add(overlay);

            Content = abs_layout;

            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

            await PageAppearCall(false);

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        public async Task ExtendedOnAppearing()
        {
            base.OnAppearing();
            await PageAppearCall(true);
        }
        public async Task PageAppearCall(bool isExtenderCalls)
        {
            if (!isExtenderCalls)
            {
                if (!string.IsNullOrEmpty(strTome))
                    Onlineflag = false;
                else
                    Onlineflag = App.Isonline;
            }

            await GetCasesData();

            if (App.Isonline)
            {

                await Task.Run(() =>
                {
                    try
                    {
                        var temp = CasesAPIMethods.GetCaseModifiedDate(Convert.ToInt32(CaseID), string.IsNullOrEmpty(_Casedata.CaseModifiedDateTime) ? DateTime.Now : Convert.ToDateTime(_Casedata.CaseModifiedDateTime));
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
                    // var ts = string.IsNullOrEmpty(_Casedata.CaseModifiedDateTime) ? DateTime.Now : Convert.ToDateTime(_Casedata.CaseModifiedDateTime);

                    if (!string.IsNullOrEmpty(_Casedata.CaseModifiedDateTime) && Convert.ToDateTime(_Casedata.CaseModifiedDateTime) < Convert.ToDateTime(spl.Where(c => c.Contains("MODIFIED_DATETIME")).FirstOrDefault().ToString().Split('=')[1]))
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

        string str_CaseFooter = string.Empty;
        bool IsModifiedDate_Same = false;

        public async Task GetCasesData()
        {
            try
            {
                await Task.Run(() =>
                {
                    try
                    {
                        if (CrossConnectivity.Current.IsConnected)
                        {
                            var Assoc = CasesSyncAPIMethods.GetAssocCascadeInfo(CrossConnectivity.Current.IsConnected, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                            Assoc.Wait();
                            AssocTypeCascades = Assoc.Result;

                            //var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                            //var AssocType = json.GetValue("ResponseContent");
                            //AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                        }
                    }
                    catch (Exception)
                    {
                    }

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

        public void LoadControls()
        {
            CasesnotesGroups.Clear();
            CasesView_ControlStack.Children.Clear();

            DesignCaseForm_withvalue();
        }

        public bool Onlineflag = false;
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
                        metadatacollection = _Casedata?.MetaDataCollection;

                        spi_MobileApp_GetTypesByCaseTypeResult itemTypes = new spi_MobileApp_GetTypesByCaseTypeResult();

                        #region Draw Control on Layout
                        AssignControlsmetadata = AssignControlsmetadata.OrderBy(m => m.LIST_MOBILE_PRIORITY_VALUE).ToList();
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
                                    var MainLayout = new StackLayout();
                                    MainLayout.Orientation = StackOrientation.Horizontal;
                                    MainLayout.Margin = new Thickness(10, 0, 0, 0);

                                    var RightLayout = new StackLayout();
                                    RightLayout.HorizontalOptions = LayoutOptions.Start;
                                    RightLayout.VerticalOptions = LayoutOptions.Center;
                                    RightLayout.Orientation = StackOrientation.Horizontal;

                                    var LeftLayout = new StackLayout();
                                    LeftLayout.HorizontalOptions = LayoutOptions.Start;
                                    LeftLayout.VerticalOptions = LayoutOptions.Center;
                                    LeftLayout.WidthRequest = 200;

                                    var Label1 = new Label
                                    {
                                        VerticalOptions = LayoutOptions.Start
                                    };
                                    Label1.Text = ControlsItem.NAME;

                                    FormattedString Format = new FormattedString();
                                    Format.Spans.Add(new Span { Text = ControlsItem.NAME + ":" });

                                    if (ControlsItem.IS_REQUIRED.ToLower() == "y")
                                        Format.Spans.Add(new Span { Text = " *", ForegroundColor = Color.Red });

                                    Label1.FormattedText = Format;

                                    Label1.HorizontalOptions = LayoutOptions.Start;
                                    Label1.FontSize = 16;
                                    Label1.WidthRequest = 200;

                                    LeftLayout.Children.Add(Label1);
                                    MainLayout.Children.Add(LeftLayout);

                                    try
                                    {
                                        switch (ControlsItem.ASSOC_FIELD_TYPE.ToLower())
                                        {
                                            case "o":
                                            case "e":

                                                Picker pk = new Picker();
                                                pk.WidthRequest = 200;
                                                pk.IsVisible = false;


                                                Button pk_button = new Button();
                                                pk_button.WidthRequest = 200;
                                                pk_button.TextColor = Color.Gray;
                                                pk_button.BackgroundColor = Color.White;
                                                if (Device.RuntimePlatform == "Android")
                                                {
                                                    pk_button.Margin = new Thickness(0, 0, 0, 1);
                                                    pk_button.CornerRadius = 0;
                                                }
                                                if (Device.RuntimePlatform == "iOS")
                                                {
                                                    pk_button.BorderWidth = 1;
                                                    pk_button.CornerRadius = 5;
                                                    pk_button.BorderColor = Color.Gray;
                                                }

                                                pk_button.IsVisible = true;

                                                pk.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID + "|" + ControlsItem.EXTERNAL_DATASOURCE_ID;

                                                try
                                                {
                                                    //    pk.SelectedIndexChanged += Pk_SelectedIndexChanged;

                                                    //    {
                                                    //        GetExternalDataSourceByIdResponse.ExternalDatasource cases_extedataSource = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                                    //        {
                                                    //            NAME = "-- Select Item --",
                                                    //            DESCRIPTION = "-- Select Item --",
                                                    //            ID = 0
                                                    //        };

                                                    //        List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                                                    //        if (Onlineflag)
                                                    //        {
                                                    //            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                                                    //            var AssocType = json.GetValue("ResponseContent");
                                                    //            AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                                                    //            var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == ControlsItem.ASSOC_TYPE_ID).ToList();
                                                    //            if (assocChild.Count < 1)
                                                    //            {
                                                    //                var result_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(Onlineflag, ControlsItem.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID));
                                                    //                if (result_extdatasource.Result != null && result_extdatasource.Result.ToString() != "[]")
                                                    //                {
                                                    //                    lst_extdatasource.AddRange(result_extdatasource.Result);
                                                    //                }
                                                    //            }
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(Onlineflag, ControlsItem.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID));

                                                    //            temp_extdatasource.Wait();
                                                    //            if (temp_extdatasource.Result.Count > 0)
                                                    //            {
                                                    //                lst_extdatasource.AddRange(temp_extdatasource.Result);
                                                    //            }
                                                    //        }

                                                    //        lst_extdatasource.Insert(0, cases_extedataSource);


                                                    //        pk.ItemsSource = lst_extdatasource;
                                                    //        pk.ItemDisplayBinding = new Binding("NAME");
                                                    //    }
                                                    //    if (!filedsecuritytype_update)
                                                    //    {
                                                    //        pk.IsEnabled = false;
                                                    //    }
                                                }
                                                catch (Exception ex)
                                                {
                                                }

                                                try
                                                {
                                                    pk_button.StyleId = ControlsItem.ASSOC_TYPE_ID.ToString();
                                                    pk_button.Clicked += Pk_button_Clicked;
                                                    pk_button.Text = "-- Select Item --";
                                                    pk_button.WidthRequest = 200;
                                                }
                                                catch (Exception)
                                                {
                                                }

                                                RightLayout.BackgroundColor = Color.Gray;
                                                RightLayout.Children.Add(pk_button);

                                                #region old Picker code
                                                /*Picker pk = new Picker();
                                                pk.WidthRequest = 200;
                                                pk.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID + "|" + ControlsItem.EXTERNAL_DATASOURCE_ID;
                                                try
                                                {
                                                    pk.SelectedIndexChanged += Pk_SelectedIndexChanged;

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
                                                RightLayout.Children.Add(pk); */
                                                #endregion

                                                break;

                                            case "d":
                                                Picker pkr = new Picker();
                                                pkr.WidthRequest = 200;
                                                pkr.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID;// + "|" + ControlsItem.EXTERNAL_DATASOURCE_ID;

                                                try
                                                {
                                                    pkr.SelectedIndexChanged += Pk_SelectedIndexChanged;

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
                                                    if (result_extdatasource1.Result.Count > 0)
                                                    {
                                                        lst_SSsource = result_extdatasource1?.Result;
                                                    }
                                                    pkr.ItemsSource = lst_SSsource;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                RightLayout.Children.Add(pkr);
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

                                                    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());
                                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                        editor.Unfocused += Entry_Unfocused;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                RightLayout.Children.Add(editor);
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
                                                txt_Date.WidthRequest = 170;
                                                txt_Date.TextColor = Color.Gray;
                                                txt_Date.Keyboard = Keyboard.Numeric;
                                                txt_Date.StyleId = "a_" + ControlsItem.ASSOC_TYPE_ID;
                                                txt_Date.Text = "";

                                                ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());

                                                if (!string.IsNullOrEmpty(ischeckcalControl))
                                                    txt_Date.TextChanged += DO_TextChanged;

                                                #endregion

                                                Image img_clr = new Image();
                                                img_clr.StyleId = "imgcl_" + ControlsItem.ASSOC_TYPE_ID;
                                                if (Device.RuntimePlatform == Device.Android)
                                                    img_clr.Source = ImageSource.FromFile("erase16.png");
                                                else
                                                    img_clr.Source = ImageSource.FromFile("Assets/erase16.png");

                                                img_clr.HeightRequest = 25;
                                                img_clr.WidthRequest = 25;


                                                #region date_pick
                                                DatePicker date_pick = new DatePicker();
                                                date_pick.IsVisible = false;
                                                date_pick.WidthRequest = 200;
                                                date_pick.TextColor = Color.Gray;
                                                date_pick.StyleId = "dt_" + ControlsItem.ASSOC_TYPE_ID;
                                                #endregion


                                                RightLayout.Children.Add(txt_Date);
                                                RightLayout.Children.Add(img_clr);
                                                RightLayout.Children.Add(date_pick);

                                                txt_Date.Focused += (sender, e) =>
                                                {
                                                    try
                                                    {
                                                        var cnt = (Entry)sender;
                                                        var sty_id = cnt.StyleId?.Split('_')[1];
                                                        var dt_c = FindCasesControls(Convert.ToInt32(sty_id), "DatePicker") as DatePicker;
                                                        try
                                                        {
                                                            cnt.Unfocus();
                                                        }
                                                        catch (Exception)
                                                        {
                                                        }
                                                        Device.BeginInvokeOnMainThread(() =>
                                                        {
                                                            cnt.Unfocus();
                                                            dt_c.Focus();
                                                        });
                                                    }
                                                    catch (Exception)
                                                    {
                                                    }
                                                };

                                                var img_trgr = new TapGestureRecognizer();
                                                img_trgr.Tapped += (s, e) =>
                                                {
                                                    Entry C_ent = new Entry();
                                                    try
                                                    {
                                                        var ct = (Image)s;
                                                        var sty_id = ct.StyleId?.Split('_')[1];
                                                        C_ent = FindCasesControls(Convert.ToInt32(sty_id)) as Entry;
                                                        C_ent.Text = "";
                                                        C_ent.Unfocus();
                                                    }
                                                    catch (Exception)
                                                    {
                                                        //C_ent.Text = "";
                                                    }
                                                };
                                                img_clr.GestureRecognizers.Add(img_trgr);

                                                if (Device.RuntimePlatform == "iOS")
                                                {
                                                    date_pick.Unfocused += Date_pick_Unfocused;
                                                }
                                                else
                                                {
                                                    date_pick.DateSelected += Date_pick_DateSelected;
                                                }
                                                break;

                                            case "t":
                                            case "h":
                                                Entry entry = new Entry();
                                                entry.StyleId = ControlsItem.ASSOC_FIELD_TYPE.ToLower() + "_" + ControlsItem.ASSOC_TYPE_ID;

                                                try
                                                {
                                                    if (!filedsecuritytype_update)
                                                    {
                                                        entry.IsEnabled = false;
                                                    }
                                                    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());
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
                                                RightLayout.Children.Add(entry);
                                                break;
                                            case "c":
                                                Entry _entry = new Entry();

                                                _entry.StyleId = "c_" + ControlsItem.ASSOC_TYPE_ID;
                                                _entry.IsEnabled = false;
                                                _entry.WidthRequest = 200;
                                                _entry.FontSize = 16;
                                                _entry.Keyboard = Keyboard.Default;
                                                _entry.Text = "";
                                                RightLayout.Children.Add(_entry);

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
                                                    ischeckcalControl = SetCalControls(sControls?.Where(v => v.AssocTypeID == Convert.ToInt32(ControlsItem.ASSOC_TYPE_ID)).ToList(),
                                                        sControls?.Where(v => v.AssocFieldType == 'C').ToList());
                                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                                        entry_number.Unfocused += entry_number_Unfocused;
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                RightLayout.Children.Add(entry_number);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    MainLayout.Children.Add(RightLayout);
                                    CasesView_ControlStack.Children.Add(MainLayout);
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
                                        var controlbtn = FindPickerControls(Metaitem.ASSOC_TYPE_ID);

                                        if (controlbtn != null)
                                        {
                                            Button btn = controlbtn as Button;
                                            var SelItmname = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.FieldValue; //null than make it blank
                                            SelItmname = SelItmname ?? metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;

                                            List<ExternalDatasourceValue> lst = new List<ExternalDatasourceValue>();
                                            ExternalDatasourceValue cases_extedataSource = new ExternalDatasourceValue
                                            {
                                                NAME = SelItmname,
                                                DESCRIPTION = SelItmname,
                                                ID = Convert.ToInt32(metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.ExternalDatasourceObjectID)
                                            };
                                            lst.Add(cases_extedataSource);

                                            if (!lstextdatasourceHistory.ContainsKey(Metaitem.ASSOC_TYPE_ID))
                                                lstextdatasourceHistory.Add(Metaitem.ASSOC_TYPE_ID, lst);
                                            else
                                                lstextdatasourceHistory[Metaitem.ASSOC_TYPE_ID] = lst;




                                            if (!string.IsNullOrEmpty(SelItmname))
                                            {
                                                btn.Text = SelItmname;
                                            }
                                            else
                                            {
                                                btn.Text = "-- Select Item --";
                                                //btn.IsEnabled = false;
                                            }
                                            break;

                                            #region Old Picker Code
                                            /*
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


                                                        #region old Selection Code
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
                                            */
                                            #endregion
                                        }
                                        break;

                                    case "d":
                                        var control = FindPickerControls(Metaitem.ASSOC_TYPE_ID) as Picker;
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
                                                    var Str = CommonConstants.DateFormatStringToString(Dateval);
                                                    DO.Text = Convert.ToDateTime(Str).Date.ToString("d");

                                                    Device.BeginInvokeOnMainThread(() =>
                                                    {
                                                        dt_c.Date = Convert.ToDateTime(Str);
                                                    });
                                                }
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
                                    case "h":
                                        try
                                        {
                                            var cnt = FindCasesControls(Metaitem.ASSOC_TYPE_ID);
                                            if (cnt != null)
                                            {
                                                Entry entry = cnt as Entry;
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    if (Metaitem.ASSOC_FIELD_TYPE.ToLower() == "c" && !Onlineflag)
                                                    {
                                                        entry.Text = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;
                                                    }
                                                    else
                                                    {
                                                        entry.Text = metadatacollection?.Where(c => c.AssociatedTypeID == Metaitem.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue;

                                                    }
                                                });
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

            if (Onlineflag)
                SetStaticCal(ContolrLst);

            Device.StartTimer(TimeSpan.FromSeconds(2), () =>
            {
                // Do something
                if (!IsModifiedDate_Same)
                {
                    WarningLabel.IsVisible = false;
                    grd_warning.Children.Remove(WarningLabel);
                }
                return true; // True = Repeat again, False = Stop the timer
            });

        }

        SearchBar ext_search = new SearchBar();
        private async void Pk_button_Clicked(object sender, EventArgs e)
        {
            try
            {
                var btn = sender as Button;
                btn.Focus();
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
                iSelectedItemlookupId = Convert.ToInt32(btn.StyleId.ToString());
                dynamic Exditemslst = null;

                var Ascitem = AssignControlsmetadata.Where(v => v.ASSOC_TYPE_ID == iSelectedItemlookupId).FirstOrDefault();

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                List<ExternalDatasourceValue> lst_extdatasource = new List<ExternalDatasourceValue>();

                lst_extdatasource.Add(extDSdefaultValues);

                string fieldName = string.Empty;

                var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == Ascitem.ASSOC_TYPE_ID).ToList();
                if (assocChild.Count < 1)
                {
                    await Task.Run(() =>
                    {
                        var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(CrossConnectivity.Current.IsConnected, Ascitem.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Ascitem.ASSOC_TYPE_ID);

                        temp_extdatasource.Wait();
                        if (temp_extdatasource.Result.Count > 0)
                        {
                            lst_extdatasource.AddRange(temp_extdatasource.Result);
                        }
                    });
                    lstexternaldatasource = lst_extdatasource;
                }
                else
                {
                    FillChildControl(Convert.ToInt32(assocChild.FirstOrDefault()._CASE_ASSOC_TYPE_ID_PARENT), sControls);

                    if (lstexternaldatasource != null)
                        Exditemslst = lstexternaldatasource.Select(v => v.NAME);
                    else
                    {
                        List<ExternalDatasourceValue> lst_extdatasourcee = new List<ExternalDatasourceValue>();

                        lst_extdatasourcee.Add(extDSdefaultValues);
                        lstexternaldatasource = lst_extdatasourcee;
                    }
                }

                Exditemslst = lstexternaldatasource.OrderBy(v => v.NAME).Select(v => v.NAME);

                #region Popup Initialization

                lst_itemlookup.WidthRequest = 350;
                lst_itemlookup.IsPullToRefreshEnabled = true;
                lst_itemlookup.Refreshing += OnRefresh;
                lst_itemlookup.ItemSelected += lst_itemlookup_ItemSelected;
                lst_itemlookup.ItemsSource = Exditemslst;
                lst_itemlookup.BackgroundColor = Color.White;
                lst_itemlookup.HasUnevenRows = true;

                ext_search.Text = "";
                ext_search.TextChanged += ext_serch;
                ext_search.HorizontalOptions = LayoutOptions.FillAndExpand;

                Button btn_cancel = new Button()
                {
                    Text = "Cancel",
                    WidthRequest = 100,
                    HeightRequest = 40,
                    TextColor = Color.Accent,
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.Center,
                };

                btn_cancel.Clicked += Btn_cancel_Clicked;


                var temp = new DataTemplate(typeof(TextViewCell));
                lst_itemlookup.ItemTemplate = temp;

                popupLT.Children.Clear();
                popupLT.Children.Add(new StackLayout
                {
                    Children =
                    {
                        new StackLayout
                        {
                            VerticalOptions =LayoutOptions.FillAndExpand,
                            HorizontalOptions =LayoutOptions.FillAndExpand,
                            Children =
                            {
                                ext_search
                            }
                        },
                        new StackLayout
                        {
                            VerticalOptions =LayoutOptions.Center,
                            HorizontalOptions =LayoutOptions.Center,
                            Children =
                            {
                                lst_itemlookup
                            }
                        },
                        new StackLayout
                        {
                            HorizontalOptions =LayoutOptions.EndAndExpand,
                            VerticalOptions =LayoutOptions.Center,
                            Margin = new Thickness(0,0,1,10),
                            Children =
                            {
                                btn_cancel
                            }
                        }
                    }
                });
                Stack_Popup.IsVisible = true;
                masterGrid.IsVisible = false;


                Stack_Popup.HeightRequest = this.Height - 20;
                Stack_Popup.WidthRequest = this.Width - 20;
                #endregion
            }
            catch (Exception)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

        }
        ListView lst_itemlookup = new ListView();

        #region PUll to refresh in Item Look up control 
        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public ICommand PulltoRefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;

                    try
                    {
                        var Ascitem = AssignControlsmetadata.Where(v => v.ASSOC_TYPE_ID == iSelectedItemlookupId).FirstOrDefault();
                        List<ExternalDatasourceValue> lst_extdatasource = new List<ExternalDatasourceValue>();

                        lst_extdatasource.Add(extDSdefaultValues);
                        await Task.Run(() =>
                        {
                            var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(CrossConnectivity.Current.IsConnected, Ascitem.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Ascitem.ASSOC_TYPE_ID);

                            temp_extdatasource.Wait();
                            if (temp_extdatasource.Result.Count > 0)
                            {
                                lst_extdatasource.AddRange(temp_extdatasource.Result);
                            }
                        });
                        lstexternaldatasource = lst_extdatasource;

                        lst_itemlookup.ItemsSource = lstexternaldatasource.OrderBy(v => v.NAME).Select(v => v.NAME);
                    }
                    catch (Exception)
                    {
                    }
                    IsRefreshing = false;
                });
            }
        }
        #endregion

        private void ext_serch(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (lstexternaldatasource.Count > 0)
                {
                    if (string.IsNullOrEmpty(e.NewTextValue))
                    {
                        lst_itemlookup.ItemsSource = lstexternaldatasource.Select(v => v.NAME);
                    }
                    else
                    {
                        var list = lstexternaldatasource.Where(v => v.NAME.ToLower().Contains(e.NewTextValue.ToLower().ToString())).ToList();
                        if (list.Count > 0)
                        {
                            lst_itemlookup.ItemsSource = list.Select(v => v.NAME).ToList();
                        }
                        else
                        {
                            lst_itemlookup.ItemsSource = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void lst_itemlookup_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            ext_search.Unfocus();
            try
            {
                DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            }
            catch (Exception)
            {
            }
            var ctrl = FindPickerControls(iSelectedItemlookupId);
            if (ctrl != null)
            {
                Button btn = ctrl as Button;
                btn.FontSize = 16;
                btn.Text = e.SelectedItem.ToString();
                btn.Focus();

                var lst = lstexternaldatasource.Where(v => v.NAME.ToLower().Contains(btn.Text.ToLower())).ToList();
                if (!lstextdatasourceHistory.ContainsKey(iSelectedItemlookupId))
                    lstextdatasourceHistory.Add(iSelectedItemlookupId, lst);
                else
                    lstextdatasourceHistory[iSelectedItemlookupId] = lst;

                ClearChildControl(iSelectedItemlookupId, sControls);


                var asChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_PARENT == iSelectedItemlookupId);

                foreach (var child in asChild)
                {
                    var control = FindPickerControls(child._CASE_ASSOC_TYPE_ID_CHILD);

                    if (control != null)
                    {
                        Button Ctbtn = ctrl as Button;
                        Ctbtn.IsEnabled = true;
                    }
                }

                Task.Run(() =>
                {
                    DyanmicSetCalcexd(Convert.ToString(iSelectedItemlookupId), sControls.Where(v => (v.AssocFieldType == 'E' || v.AssocFieldType == 'O') && v.AssocTypeID == Convert.ToInt32(iSelectedItemlookupId)).ToList(), sControls.Where(v => (v.AssocFieldType == 'C')).ToList());
                });
            }

            this.Stack_Popup.IsVisible = false;
            this.masterGrid.IsVisible = true;
        }

        private void Btn_cancel_Clicked(object sender, EventArgs e)
        {
            Stack_Popup.IsVisible = false;
            masterGrid.IsVisible = true;
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            list.IsRefreshing = false;
        }

        private void SetBottomBarDetails()
        {
            try
            {
                StackLayout StkFoot = new StackLayout();

                var spl = str_CaseFooter.Split('|').ToList();

                if (_Casedata != null)
                {
                    string CREATED_BY = spl?.Where(c => c.Contains("CREATED_BY"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string CREATED_DATETIME = spl?.Where(c => c.Contains("CREATED_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string MODIFIED_BY = spl?.Where(c => c.Contains("MODIFIED_BY"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string MODIFIED_DATETIME = spl?.Where(c => c.Contains("MODIFIED_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string CASE_ASSGN_TO = spl?.Where(c => c.Contains("CASE_ASSGN_TO"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string CASE_ASSGN_DATETIME = spl?.Where(c => c.Contains("CASE_ASSGN_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string CASE_OWNER = spl?.Where(c => c.Contains("CASE_OWNER"))?.FirstOrDefault()?.ToString()?.Split('=')[1];
                    string CASE_OWNER_DATETIME = spl?.Where(c => c.Contains("CASE_OWNER_DATETIME"))?.FirstOrDefault()?.ToString()?.Split('=')[1];

                    // _Casedata.CaseAssignedToDisplayName = _Casedata.CaseAssignedToDisplayName ?? _Casedata.CaseAssignedToDisplayName;

                    #region Created By Name
                    var s = new FormattedString();
                    if (_Casedata?.CaseCreatedDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CREATED_BY)) ? CREATED_BY + "\r\n" : _Casedata.CaseCreatedDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CREATED_DATETIME)) ? Convert.ToString(CREATED_DATETIME) : _Casedata.CaseCreatedDateTime, FontSize = 14 });
                    }

                    lbl_createname.FormattedText = (s);
                    #endregion  

                    #region Assigned to Name
                    s = new FormattedString();
                    if (_Casedata?.CaseAssignedToDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_ASSGN_TO)) ? CASE_ASSGN_TO + "\r\n" : _Casedata.CaseAssignedToDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_ASSGN_DATETIME)) ? Convert.ToString(CASE_ASSGN_DATETIME) : _Casedata.CaseAssignDateTime ?? "", FontSize = 14 });

                    }

                    lbl_assignto.FormattedText = (s);

                    #endregion

                    #region Owned By Name

                    s = new FormattedString();
                    if (_Casedata?.CaseOwnerDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_OWNER)) ? CASE_OWNER + "\r\n" : _Casedata.CaseOwnerDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(CASE_OWNER_DATETIME)) ? Convert.ToString(CASE_OWNER_DATETIME) : _Casedata.CaseOwnerDateTime, FontSize = 14 });
                    }

                    lbl_ownername.FormattedText = (s);


                    #endregion

                    #region Modified BY Name
                    s = new FormattedString();
                    if (_Casedata?.CaseModifiedByDisplayName != null)
                    {
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(MODIFIED_BY)) ? MODIFIED_BY + "\r\n" : _Casedata.CaseModifiedByDisplayName + "\r\n", FontSize = 14 });
                        s.Spans.Add(new Span { Text = (Onlineflag && !string.IsNullOrEmpty(MODIFIED_DATETIME)) ? Convert.ToString(MODIFIED_DATETIME) : _Casedata.CaseModifiedDateTime, FontSize = 14 });
                    }

                    lbl_modifiedname.FormattedText = (s);

                    #endregion
                }

                Grd_Footer.IsVisible = true;

                lbl_line.IsVisible = _Casedata.IsClosed;
                lbl_Casestatus.IsVisible = _Casedata.IsClosed;

                DateTime Dout = new DateTime();
                DateTime.TryParse(_Casedata.CaseClosedDateTime?.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out Dout);

                lbl_Casestatus.Text = "Closed By " + (_Casedata.CaseClosedByDisplayName ?? _Casedata.CaseClosedBySAM) + " at " + Convert.ToString(Dout.ToString());

                Assign_Sam = _Casedata.CaseAssignedToSAM;
                Create_Sam = _Casedata.CaseCreatedSAM;
                Modify_Sam = _Casedata.CaseModifiedBySAM;
                Owner_Sam = _Casedata.CaseOwnerSAM;

            }
            catch (Exception qw)
            {
            }
        }



        public async void ReloadNotesArea()
        {
            try
            {
                gridCasesnotes.ItemsSource = null;

                ObservableCollection<CasesNotesGroup> Temp = new ObservableCollection<CasesNotesGroup>();
                List<GetCaseNotesResponse.NoteData> Noteslist = new List<GetCaseNotesResponse.NoteData>();
                await Task.Run(() =>
                {
                    Task<List<GetCaseNotesResponse.NoteData>> NotesResponse = CasesSyncAPIMethods.GetCaseNotes(Onlineflag, CaseID, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, _Casedata.NewestNoteOnTop);
                    NotesResponse.Wait();
                    Noteslist = NotesResponse?.Result;
                });

                if (Noteslist?.Count > 0)
                {
                    for (int i = 0; i < Noteslist.Count; i++)
                    {
                        CasesNotesGroup grp = new CasesNotesGroup("", Convert.ToString(Noteslist[i].CreatedDateTime), Noteslist[i]?.CreatedByUser == null ? Functions.UserFullName : Noteslist[i]?.CreatedByUser?.DisplayName)
                            {
                                new GetCaseNotesResponse.NoteData
                                {
                                    Note = Noteslist[i].Note.Replace("href=\"/DownloadFile.aspx?CaseFileID=","href=\""+App.CasesImgURL+"/DownloadFile.aspx?CaseFileID=")
                                }
                            };
                        Temp.Add(grp);
                    }


                    CasesnotesGroups.Clear();
                    foreach (var item in Temp)
                    {
                        if (item.FirstOrDefault().Note.Contains("<img"))
                        {
                            item.FirstOrDefault().ImageVisible = true;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().ImageURL =/* App.CasesImgURL + "/" +*/ item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1];
                            item.FirstOrDefault().Note = item.FirstOrDefault().Note;
                        }
                        else
                        {
                            item.FirstOrDefault().ImageVisible = false;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().Note = (item.FirstOrDefault().Note);
                        }
                        //if (item.FirstOrDefault().Note.Contains("<img"))
                        //{
                        //    item.FirstOrDefault().ImageVisible = true;
                        //    item.FirstOrDefault().LabelVisible = true;
                        //    item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
                        //    item.FirstOrDefault().ImageURL = App.CasesImgURL + "/" + Functions.HTMLToText(item.FirstOrDefault().Note.Replace("'", "\"").Split('\"')[1]);
                        //    item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                        //}
                        //else
                        //{
                        //    item.FirstOrDefault().ImageVisible = false;
                        //    item.FirstOrDefault().LabelVisible = true;
                        //    item.FirstOrDefault().HtmlNote = item.FirstOrDefault().Note;
                        //    item.FirstOrDefault().Note = Functions.HTMLToText(item.FirstOrDefault().Note);
                        //}

                        CasesnotesGroups.Add(item);
                    }

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        gridCasesnotes.ItemsSource = null;
                        gridCasesnotes.ItemsSource = CasesnotesGroups;
                    });
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Date_pick_Unfocused(object sender, FocusEventArgs e)
        {
            Date_pick_DateSelected(sender, null);
        }

        private void Date_pick_DateSelected(object sender, DateChangedEventArgs e)
        {
            Entry dtp = new Entry();
            try
            {
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
                var cnt = (DatePicker)sender;
                var sty_id = cnt.StyleId?.Split('_')[1];
                var dt_Entry = FindCasesControls(Convert.ToInt32(sty_id)) as Entry;
                DateTime dt = cnt.Date;
                dt_Entry.Text = dt.Date.ToString("d");
            }
            catch (Exception)
            {
                dtp.Text = "";
            }
        }

        private void DO_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
                Entry en = (Entry)sender;

                DyanmicSetCalc(en.StyleId);
            }
            catch (Exception ex)
            {

            }
        }


        List<spi_MobileApp_GetTypesByCaseTypeResult> AssignControlsmetadata = new List<spi_MobileApp_GetTypesByCaseTypeResult>();
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
                    //var itemType = ItemTypes.Where(t => t.AssocTypeID == child._CASE_ASSOC_TYPE_ID_CHILD).FirstOrDefault();
                    var Childcontrol = FindPickerControls(child._CASE_ASSOC_TYPE_ID_CHILD);

                    if (Childcontrol != null)//&& itemType != null)
                    {
                        List<ExternalDatasourceValue> lst_extdatasource = new List<ExternalDatasourceValue>();

                        lst_extdatasource.Add(extDSdefaultValues);

                        //(control as Picker).ItemsSource = lst_extdatasource;
                        //(control as Picker).SelectedIndex = 0;
                        var PNctrl = FindPickerControls(assocTypeId) as Button;

                        Button btn = Childcontrol as Button;
                        btn.Text = "-- Select Item --";

                        if (PNctrl.Text == "-- Select Item --")
                        {
                            btn.IsEnabled = false;
                        }
                        else
                            btn.IsEnabled = true;

                        ClearChildControl(child._CASE_ASSOC_TYPE_ID_CHILD, ItemTypes);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            try
            {
                if (Onlineflag)
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
                                    int AssocID = int.Parse(Picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                    #region MyRegion
                                    var sd = Picker.SelectedItem as ExternalDatasourceValue;
                                    try
                                    {
                                        if (sd == null)
                                        {
                                            var newsd = Picker.SelectedItem as GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue;
                                            ExternalDatasourceValue val = new ExternalDatasourceValue
                                            {
                                                DESCRIPTION = newsd.Description,
                                                ID = newsd.ID,
                                                NAME = newsd.Name
                                            };

                                            if (val.ID > 0)
                                            {
                                                int AssocIDD = int.Parse(Picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                                DyanmicSetCalc(Picker.StyleId);
                                            }
                                        }
                                    }
                                    catch
                                    {
                                    }

                                    if (sd != null && sd?.ID > 0)
                                    {
                                        DyanmicSetCalcexd(Picker.StyleId, sControls.Where(v => (v.AssocFieldType == 'E' || v.AssocFieldType == 'O') && v.AssocTypeID == Convert.ToInt32(AssocID)).ToList(), sControls.Where(v => (v.AssocFieldType == 'C')).ToList());
                                    }
                                    if (lstcalculationsFieldlist.Count > 0)
                                    {
                                        if (sd?.NAME.ToLower() == "-- select item --")
                                        {
                                            int aId = 0;
                                            if (AssocTypeCascades.Where(v => v._CASE_ASSOC_TYPE_ID_CHILD == AssocID)?.Count() > 0)
                                                aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v._CASE_ASSOC_TYPE_ID_CHILD == AssocID));
                                            else if (AssocTypeCascades.Where(v => v._CASE_ASSOC_TYPE_ID_PARENT == AssocID)?.Count() > 0)
                                                aId = AssocTypeCascades.IndexOf(AssocTypeCascades.Single(v => v._CASE_ASSOC_TYPE_ID_PARENT == AssocID));

                                            for (int i = aId; i < AssocTypeCascades.Count; i++)
                                            {
                                                var av = lstcalculationsFieldlist.Distinct().Where(v => v.Item1.FirstOrDefault().AssocTypeID == AssocID);
                                                foreach (var ii in av)
                                                {
                                                    foreach (StackLayout infofield in CasesView_ControlStack.Children)
                                                    {
                                                        foreach (var subitem in infofield.Children)
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
                                                                        int id = Convert.ToInt32(ii.Item2.Replace("assoc_C_", ""));
                                                                        var calinfo = sControls.Where(v => v.AssocFieldType == 'C' && v.AssocTypeID == id)?.FirstOrDefault();
                                                                        if (!string.IsNullOrEmpty(calinfo.CalculationFormula))
                                                                        {
                                                                            string avName = ii.Item1.FirstOrDefault().Name;
                                                                            string avSyscode = ii.Item1.FirstOrDefault().SystemCode;

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
                                    #endregion

                                    ClearChildControl(AssocID, sControls);

                                    if ((Picker.SelectedItem as ExternalDatasourceValue).ID > 0)
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
            Debug.WriteLine("Selected Index Action Time => " + sp.ElapsedMilliseconds);
            sp.Stop();
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
                foreach (StackLayout infofield in CasesView_ControlStack.Children)
                {
                    foreach (StackLayout item in infofield.Children)
                    {
                        foreach (var subitem in item.Children)
                        {
                            var xy = subitem;
                            Type ty = xy.GetType();
                            if (ty.Name.ToLower() == "picker")
                            {
                                Picker Picker = new Picker();
                                Picker = (Picker)xy;

                                if (Picker.StyleId.Contains("|"))
                                {
                                    if (Picker.StyleId.Split('_')[1].Split('|')[0] == AssocID.ToString())
                                        return Picker;
                                }
                                else if (Picker.StyleId.Contains("_"))
                                {
                                    if (Picker.StyleId.Split('_')[1] == AssocID.ToString())
                                        return Picker;
                                }
                                else
                                {
                                    if (Picker.StyleId == AssocID.ToString())
                                        return Picker;
                                }
                            }
                            else if (ty.Name.ToLower() == "button")
                            {
                                Button btn = new Button();
                                btn = (Button)xy;
                                if (btn.StyleId.Contains("|"))
                                {
                                    if (btn.StyleId.Split('_')[1].Split('|')[0] == AssocID.ToString())
                                        return btn;
                                }
                                else if (btn.StyleId.Contains("_"))
                                {
                                    if (btn.StyleId.Split('_')[1] == AssocID.ToString())
                                        return btn;
                                }
                                else
                                {
                                    if (btn.StyleId == AssocID.ToString())
                                        return btn;
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
        private object FindCasesControls(int AssocID, string Cnt_type = "")
        {
            try
            {
                foreach (StackLayout infofield in CasesView_ControlStack.Children)
                {
                    foreach (StackLayout item in infofield.Children)
                    {
                        foreach (var subitem in item.Children)
                        {
                            var xy = subitem;
                            Type ty = xy.GetType();
                            if (ty.Name != "StackLayout")
                            {
                                if (string.IsNullOrEmpty(Cnt_type))
                                {
                                    if (xy.StyleId != null)
                                    {
                                        var sID = xy.StyleId;

                                        if (sID.Contains("|"))
                                        {
                                            if (sID.Split('_')[1].Split('|')[0] == AssocID.ToString())
                                                return xy;
                                        }
                                        else if (sID.Contains("_"))
                                        {
                                            if (sID.Split('_')[1] == AssocID.ToString())
                                                return xy;
                                        }
                                        else
                                        {
                                            if (sID == AssocID.ToString())
                                                return xy;
                                        }
                                    }
                                }
                                else if (Cnt_type == "DatePicker" && ty.Name == "DatePicker")
                                {
                                    var sID = xy.StyleId.Split('_')[1];
                                    if (sID == AssocID.ToString())
                                        return xy;
                                }

                                //if (Cnt_type == "DatePicker" && ty.Name == "DatePicker")
                                //{
                                //    var sID = xy.StyleId.Split('_')[1];
                                //    if (sID == AssocID.ToString())
                                //        return xy;
                                //}
                                //else if (Cnt_type == "Image" && ty.Name == "Image")
                                //{
                                //    var sID = xy.StyleId.Split('_')[1];
                                //    if (sID == AssocID.ToString())
                                //        return xy;
                                //}
                                //else if (Cnt_type == "Button" && ty.Name == "Button")
                                //{
                                //    var sID = xy.StyleId.Split('_')[1].Split('|')[0];
                                //    if (sID == AssocID.ToString())
                                //        return xy;
                                //}
                                //else if (Cnt_type == "Picker" && ty.Name == "Picker")
                                //{
                                //    var sID = xy.StyleId.Split('_')[1].Split('|')[0];
                                //    if (sID == AssocID.ToString())
                                //        return xy;
                                //}
                                //else if (Cnt_type == "Entry" && ty.Name == "Entry")
                                //{
                                //    var sID = xy.StyleId.Split('_')[1];
                                //    if (sID == AssocID.ToString())
                                //        return xy;
                                //}
                                //else if (Cnt_type == "BorderEditor" && ty.Name == "BorderEditor")
                                //{
                                //    var sID = xy.StyleId.Split('_')[1];
                                //    if (sID == AssocID.ToString())
                                //        return xy;
                                //}
                                //else if (!string.IsNullOrEmpty(xy.StyleId) && xy.StyleId.Contains(AssocID.ToString()))
                                //    return xy;
                            }
                        }
                    }
                }






                //if (string.IsNullOrEmpty(Cnt_type))
                //{
                //    foreach (StackLayout infofield in Cases_EntryStack.Children)
                //    {
                //        foreach (StackLayout item in infofield.Children)
                //        {

                //            foreach (var subitem in item.Children)
                //            {
                //                var xy = subitem;
                //                Type ty = xy.GetType();
                //                if (ty.Name != "StackLayout")
                //                {
                //                    if (Cnt_type == "DatePicker" && ty.Name == "DatePicker")
                //                    {
                //                        var sID = xy.StyleId.Split('_')[1];
                //                        if (sID == AssocID.ToString())
                //                            return xy;
                //                    }
                //                    else
                //                    if (Cnt_type == "Image" && ty.Name == "Image")
                //                    {
                //                        var sID = xy.StyleId.Split('_')[1];
                //                        if (sID == AssocID.ToString())
                //                            return xy;

                //                    }
                //                    else if (Cnt_type == "Button" && ty.Name == "Button")
                //                    {
                //                        var sID = xy.StyleId.Split('_')[1].Split('|')[0];
                //                        if (sID == AssocID.ToString())
                //                            return xy;
                //                    }
                //                    else if (Cnt_type == "Picker" && ty.Name == "Picker")
                //                    {
                //                        var sID = xy.StyleId.Split('_')[1].Split('|')[0];
                //                        if (sID == AssocID.ToString())
                //                            return xy;
                //                    }
                //                    else if (Cnt_type == "Entry" && ty.Name == "Entry")
                //                    {
                //                        var sID = xy.StyleId.Split('_')[1];
                //                        if (sID == AssocID.ToString())
                //                            return xy;
                //                    }
                //                    else if (Cnt_type == "BorderEditor" && ty.Name == "BorderEditor")
                //                    {
                //                        var sID = xy.StyleId.Split('_')[1];
                //                        if (sID == AssocID.ToString())
                //                            return xy;
                //                    }
                //                    else if (!string.IsNullOrEmpty(xy.StyleId) && xy.StyleId.Contains(AssocID.ToString()))
                //                        return xy;
                //                }
                //            }
                //        }
                //    }
                //}
                //else if (Cnt_type == "DatePicker")
                //{
                //    foreach (StackLayout infofield in Cases_EntryStack.Children)
                //    {
                //        foreach (StackLayout item in infofield.Children)
                //        {
                //            foreach (var subitem in item.Children)
                //            {
                //                var xy = subitem;
                //                Type ty = xy.GetType();
                //                if (ty.Name != "StackLayout")
                //                {
                //                    if (ty.Name == "DatePicker")
                //                    {
                //                        if (xy.StyleId.Contains(AssocID.ToString()))
                //                            return xy;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                //else if (Cnt_type == "Image")
                //{
                //    foreach (StackLayout infofield in Cases_EntryStack.Children)
                //    {
                //        foreach (StackLayout item in infofield.Children)
                //        {
                //            foreach (var subitem in item.Children)
                //            {
                //                var xy = subitem;
                //                Type ty = xy.GetType();
                //                if (ty.Name != "StackLayout")
                //                {
                //                    if (ty.Name == "Image")
                //                    {
                //                        if (xy.StyleId.Contains(AssocID.ToString()))
                //                            return xy;
                //                    }
                //                }
                //            }
                //        }
                //    }
                //}
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        //private object FindCasesControls(int AssocID, string Cnt_type = "")
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(Cnt_type))
        //        {
        //            foreach (StackLayout infofield in CasesView_ControlStack.Children)
        //            {
        //                foreach (StackLayout item in infofield.Children)
        //                {

        //                    foreach (var subitem in item.Children)
        //                    {
        //                        var xy = subitem;
        //                        Type ty = xy.GetType();

        //                        if (xy.StyleId != null)
        //                        {
        //                            if (ty.Name != "StackLayout")
        //                            {
        //                                var id = xy.StyleId.Contains("|") ? xy.StyleId.Split('|')[0] : xy.StyleId;

        //                                if (id.Contains(AssocID.ToString()))
        //                                    return xy;
        //                            }
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //        else if (Cnt_type == "DatePicker")
        //        {
        //            foreach (StackLayout infofield in CasesView_ControlStack.Children)
        //            {

        //                foreach (StackLayout item in infofield.Children)
        //                {

        //                    foreach (var subitem in item.Children)
        //                    {
        //                        var xy = subitem;
        //                        Type ty = xy.GetType();
        //                        if (ty.Name != "StackLayout")
        //                        {
        //                            if (ty.Name == "DatePicker")
        //                            {
        //                                if (xy.StyleId.Contains(AssocID.ToString()))
        //                                {
        //                                    return xy;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //        else if (Cnt_type == "Image")
        //        {
        //            foreach (StackLayout infofield in CasesView_ControlStack.Children)
        //            {

        //                foreach (StackLayout item in infofield.Children)
        //                {

        //                    foreach (var subitem in item.Children)
        //                    {
        //                        var xy = subitem;
        //                        Type ty = xy.GetType();
        //                        if (ty.Name != "StackLayout")
        //                        {
        //                            if (ty.Name == "Image")
        //                            {
        //                                if (xy.StyleId.Contains(AssocID.ToString()))
        //                                {
        //                                    return xy;
        //                                }
        //                            }

        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        #endregion

        private async void FillChildControl(int assocTypeId, List<GetCaseTypesResponse.ItemType> ItemTypes)
        {
            try
            {
                string fieldName = string.Empty;
                var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_PARENT == assocTypeId).ToList();

                foreach (var child in assocChild)
                {
                    var Bcontrol = FindPickerControls(child._CASE_ASSOC_TYPE_ID_CHILD) as Button;
                    if (Bcontrol != null)
                    {

                        var itemType = ItemTypes.Where(t => t.AssocTypeID == child._CASE_ASSOC_TYPE_ID_CHILD).FirstOrDefault();

                        if (itemType.ExternalDataSourceID != null && itemType.ExternalDataSourceID > 0 && CrossConnectivity.Current.IsConnected)
                        {
                            if (CrossConnectivity.Current.IsConnected)
                            {
                                string query = string.Empty;
                                string conn = string.Empty;
                                string ParentExternalDatasourceName = string.Empty;
                                var externalDatasource = CasesSyncAPIMethods.GetExternalDataSourceItemsById(CrossConnectivity.Current.IsConnected, Convert.ToString(itemType.ExternalDataSourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);

                                query = externalDatasource.Result?.FirstOrDefault()?.Query;
                                conn = externalDatasource.Result?.FirstOrDefault()?.ConnectionString;
                                ParentExternalDatasourceName = externalDatasource?.Result?.FirstOrDefault()?.Name;

                                string filterQueryOrg = "";

                                var Result1 = CasesSyncAPIMethods.GetConnectionString(CrossConnectivity.Current.IsConnected, itemType.ExternalDataSourceID.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);

                                if (!string.IsNullOrEmpty(Result1?.ToString()) && Result1.ToString() != "[]")
                                {
                                    filterQueryOrg = Result1.Result?.FirstOrDefault()?._FILTER_QUERY;
                                }

                                var assocParents = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == child._CASE_ASSOC_TYPE_ID_CHILD);
                                int parentCount = 1;
                                foreach (var p in assocParents)
                                {
                                    string parentSelectedValue = null;
                                    var ParntItmType = ItemTypes.Where(t => t.AssocTypeID == p._CASE_ASSOC_TYPE_ID_PARENT).FirstOrDefault();
                                    //parentSelectedValue = Convert.ToString((((Picker)FindPickerControls(p._CASE_ASSOC_TYPE_ID_PARENT)).SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).ID);

                                    List<ExternalDatasourceValue> lst = lstextdatasourceHistory.Where(v => v.Key == p._CASE_ASSOC_TYPE_ID_PARENT)?.FirstOrDefault().Value;

                                    parentSelectedValue = Convert.ToString(lst?.FirstOrDefault().ID);

                                    string parentFieldName = ParntItmType.Name;

                                    var externalDatasourceinfo = CasesSyncAPIMethods.GetExternalDataSourceItemsById(Onlineflag, Convert.ToString(ParntItmType.ExternalDataSourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);

                                    ParentExternalDatasourceName = externalDatasourceinfo?.Result?.FirstOrDefault()?.Name;

                                    query = GetQueryStringWithParamaters(query, parentFieldName, parentSelectedValue, ParentExternalDatasourceName);

                                    //replace internal entity type
                                    if (!string.IsNullOrEmpty(filterQueryOrg))
                                    {
                                        var filterQuery = "" + filterQueryOrg;

                                        var cnt = 0;
                                        if (filterQuery.Contains("{'ENTITY_ASSOC_EXTERNAL_DATASOURCE_ID'}"))
                                            cnt++;
                                        if (filterQuery.Contains("'%BOXER_ENTITIES%'"))
                                            cnt++;
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
                                    fieldName = itemType.Name;
                                    var DSValues = JsonConvert.DeserializeObject<List<ExternalDatasourceValue>>(CasesAPIMethods.GetValuesQueryAndConnection(Casetypeid, child._CASE_ASSOC_TYPE_ID_CHILD.ToString(), fieldName/*SelectedCaseType.Name*/,
                                         (itemType.IsRequired == 'Y').ToString(), conn, query).GetValue("ResponseContent").ToString());

                                    //(control as Picker).ItemsSource = ItemValues;
                                    //(control as Picker).SelectedIndex = 0;
                                    lstexternaldatasource = DSValues.OrderBy(v => v.NAME).ToList();
                                }
                            }
                        }
                        else
                        {
                            List<ExternalDatasourceValue> lst_extdatasource = new List<ExternalDatasourceValue>();
                            lst_extdatasource.Add(extDSdefaultValues);

                            var GetAppTypeInfo = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(ConstantsSync.CasesInstance, "C1_C2_CASES_CASETYPELIST", Convert.ToInt32(Casetypeid), App.DBPath, null);
                            GetAppTypeInfo.Wait();

                            Task<EDSResultList> Result = DBHelper.GetEDSResultListwithId(Convert.ToInt32(child._CASE_ASSOC_TYPE_ID_CHILD), Convert.ToInt32(GetAppTypeInfo?.Result?.APP_TYPE_INFO_ID), App.DBPath);
                            Result.Wait();
                            if (Result?.Result?.ASSOC_FIELD_ID > 0)
                            {
                                string jsonvalue = Result.Result.EDS_RESULT;
                                var lstResult = JsonConvert.DeserializeObject<List<ExternalDatasourceValue>>(jsonvalue);
                                lst_extdatasource.AddRange(lstResult.OrderBy(v => v.NAME));
                            }

                            //(control as Picker).ItemsSource = lst_extdatasource;
                            lstexternaldatasource = lst_extdatasource;
                            //if (itemType != null)
                            //{
                            //    fieldName = itemType.Name;
                            //}
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
                foreach (StackLayout infofield in CasesView_ControlStack.Children)
                {

                    foreach (StackLayout item in infofield.Children)
                    {

                        foreach (var subitem in item.Children)
                        {
                            var xy = subitem;
                            Type ty = xy.GetType();
                            if (ty.Name.ToLower() != "stacklayout")
                            {
                                if (ty.Name.ToLower() == "entry")
                                {
                                    try
                                    {
                                        var en = (Entry)xy;
                                        if (en.StyleId == _TitleFieldControlID)
                                        {
                                            savecase.caseTitle = en.Text;
                                        }
                                        int Key = int.Parse(en.StyleId.Split('_')[1]?.ToString());
                                        string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                        string FileldName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();

                                        if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(en.Text))
                                        {
                                            DisplayAlert("Field Required.", "Please enter valid data in " + FileldName, "OK");

                                            goto requiredJump;
                                        }
                                        else
                                        {
                                            if (!en.StyleId.Contains("a_"))
                                            {
                                                textValues.Add(Key, en.Text);
                                            }
                                            else
                                            {
                                                var stDate = string.Empty;
                                                if (!string.IsNullOrEmpty(en.Text))
                                                {
                                                    var sDate = Convert.ToDateTime(en.Text);
                                                    stDate = sDate.Date.ToString("MM/dd/yyyy");
                                                }
                                                textValues.Add(Key, stDate);
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else if (ty.Name.ToLower() == "picker")
                                {
                                    try
                                    {
                                        var picker = (Picker)xy;

                                        ItemValue itemValue = new ItemValue();

                                        if (!picker.StyleId.Contains("NoteType"))
                                        {
                                            string styletype = picker.StyleId.Split('_')[0].Split('|')[0].ToString();

                                            if (styletype.ToLower() == "d")
                                            {
                                                int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                                string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                                string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
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

                                                    string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                                    string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                                    if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(FiledName))
                                                    {
                                                        DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                                        goto requiredJump;
                                                    }
                                                    else
                                                    {

                                                        if (picker.StyleId.Split('_')[0].ToLower() == "o")
                                                        {
                                                            var val = picker.SelectedItem as ExternalDatasourceValue;
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
                                                            var val = picker.SelectedItem as ExternalDatasourceValue;
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
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                else if (ty.Name.ToLower() == "button")
                                {
                                    try
                                    {
                                        Button btn = new Button();
                                        btn = (Button)xy;

                                        ItemValue it = new ItemValue();
                                        if (btn != null)
                                        {
                                            int Key = int.Parse(btn.StyleId);
                                            if (btn.StyleId.Split('|')[0] == _TitleFieldControlID)
                                            {
                                                savecase.caseTitle = btn.Text;
                                            }

                                            var tempassocdecodeid = lstextdatasourceHistory.Where(v => v.Key == Convert.ToInt32(btn.StyleId))?.FirstOrDefault().Value;

                                            string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                            string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                            if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && btn.Text == "-- Select Item --")
                                            {
                                                DisplayAlert("Field Required.", "Please Select valid item in " + FiledName, "OK");

                                                goto requiredJump;
                                            }
                                            else
                                            {

                                                it.Name = btn.Text;
                                                it.AssocDecodeID = Convert.ToInt32(tempassocdecodeid?.FirstOrDefault().ID);
                                                dropDownValues.Add(Key, it);
                                                dctmetadata.Add(Convert.ToInt32(Key), Convert.ToString(it.Name));
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //throw ex;
                                    }
                                }
                                else if (ty.Name.ToLower() == "bordereditor")
                                {
                                    try
                                    {
                                        var editor = (BorderEditor)xy;
                                        if (editor.StyleId == _TitleFieldControlID)
                                        {
                                            savecase.caseTitle = editor.Text;
                                        }

                                        string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString())).FirstOrDefault().IS_REQUIRED.ToString();
                                        string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString()))?.FirstOrDefault()?.NAME?.ToString();
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
                                    }
                                }
                                else if (ty.Name.ToLower() == "datepicker")
                                {
                                    try
                                    {
                                        //var datepicker = (DatePicker)xy;

                                        //if (datepicker.Date != Convert.ToDateTime("01/01/1900"))
                                        //{
                                        //    textValues.Add(int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), CommonConstants.DateFormatStringToString(datepicker.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"));
                                        //}
                                        //else
                                        //    textValues.Add(int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), "");
                                    }
                                    catch (Exception ex)
                                    {
                                    }
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

                createcase.caseTitle = Casetitle;
                createcase.currentUser = Functions.UserName;
                createcase.caseType = Convert.ToInt32(Casetypeid);
                createcase.assignTo = "";
                createcase.linkValues = linkValue;
                createcase.textValues = createtextValues;
                createcase.caseNotes = "";
                createcase.TransactionType = App.Isonline == true ? "M" : "T";
                createcase.metaDataValues = dctmetadata;

                switch (action)
                {

                    case "Save":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            await Task.Run(() =>
                            {
                                Task<int> save = SaveAndUpdate(savecase, createcase);
                                save.Wait();
                            });
                        }
                        catch (Exception)
                        {
                        }
                        txt_CasNotes.Text = "";

                        ReloadNotesArea();
                        SetBottomBarDetails();
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                        break;

                    case "Save & Exit":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        try
                        {
                            await Task.Run(() =>
                            {
                                Task<int> saveExit = SaveAndUpdate(savecase, createcase);
                                saveExit.Wait();
                            });
                        }
                        catch (Exception)
                        {
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                        switch (strTome)
                        {
                            case "_AssignedToMe":
                                await this.Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));
                                break;
                            case "_CreatedByMe":
                                await this.Navigation.PushAsync(new CaseList("caseCreateBySAM", Functions.UserName, ""));
                                break;
                            case "_OwnedByMe":
                                await this.Navigation.PushAsync(new CaseList("caseOwnerSAM", Functions.UserName, ""));
                                break;
                            case "_AssignedToMyTeam":
                                {
                                    var tm_uname = DBHelper.GetAppTypeInfo_tmname(ConstantsSync.CasesInstance, _Casedata.CaseID, _Casedata.CaseTypeID, "E2_GetCaseList_AssignedToMyTeam", App.DBPath).Result?.TM_Username;

                                    await this.Navigation.PushAsync(new CaseList("caseAssgnTM", tm_uname ?? Functions.UserName, ""));
                                    break;
                                }

                            default:
                                await Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(Casetypeid), "", Casetitle));
                                break;
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
            }
            requiredJump:
            int abc = 0;
        }
        #endregion
        public async Task<int> SaveAndUpdate(SaveCaseTypeRequest savecase, CreateCaseOptimizedRequest.CreateCaseModelOptimized createcase, object obj = null, bool Isapproved = false, bool Isdecline = false)
        {
            int i = 0;
            return i = await CasesSyncAPIMethods.StoreAndUpdateCase(Onlineflag, Convert.ToInt32(Casetypeid), savecase, txt_CasNotes.Text, Functions.UserName, App.DBPath, createcase, null, obj, GetNoteTypeId(000001).ToString(), Isapproved, Isdecline, Functions.UserFullName, strTome);
        }

        private async void GridCasesnotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                //string url = string.Empty;
                //var d = DataServiceBus.OnlineHelper.DataTypes.Constants.Baseurl.Split(new string[] { "/mobileapi" }, StringSplitOptions.None);
                //url = d[0];

                //var notes = gridCasesnotes.SelectedItem as GetCaseNotesResponse.NoteData;

                //if (notes.ImageVisible)
                //{
                //    await Navigation.PushAsync(new ViewAttachment(notes.ImageURL));
                //}
            }
            catch (Exception ex)
            {
            }
        }

        void EmailLink(SaveCaseTypeRequest savecase)
        {
            try
            {
                string shareurl = String.Empty;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    string subject = Functions.UserFullName + " wants to share this " + Casetitle;
                    subject = WebUtility.UrlEncode(subject).Replace("+", "%20");
                    string body = "Please visit this url:  " + App.CasesImgURL + "/ViewCase.aspx?CaseID=" + CaseID;
                    body = WebUtility.UrlEncode(body).Replace("+", "%20");
                    shareurl = "mailto:?subject=" + (subject) + "&body=" + (body);
                }
                else
                {
                    //for Android it is not necessary to code nor is it necessary to assign a destination email
                    string subject = Functions.UserFullName + " wants to share this " + Casetitle;
                    string body = "Please visit this url:  " + App.CasesImgURL + "/ViewCase.aspx?CaseID=" + CaseID;
                    shareurl = "mailto:?subject=" + subject + "&body=" + body;
                }
                Device.OpenUri(new Uri(shareurl));
            }
            catch (Exception ex)
            {

            }
        }

        void Copylink(SaveCaseTypeRequest _Casedataclass1)
        {
            try
            {
                string body = App.CasesImgURL + "/ViewCase.aspx?CaseID=" + _Casedataclass1?.caseId;
            }
            catch (Exception ex)
            {
            }
        }

        #region Cases Calculation 
        public string SetCalControls(List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal)
        {
            try
            {
                string sControl = string.Empty;
                if (Onlineflag)
                {
                    calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
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
                string sControl = string.Empty;
                if (Onlineflag)
                {
                    calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
                    KeyValuePair<List<GetCaseTypesResponse.ItemType>, string> Itemcontrols = new KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>(sControls, sControl);
                    foreach (var item in sControls)
                    {
                        if (item.ExternalDataSourceID != null)
                        {

                            var sxternal = CasesSyncAPIMethods.GetExternalDataSourceItemsById(Onlineflag, Convert.ToString(item.ExternalDataSourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                            sxternal.Wait();
                            if (sxternal.Result != null)
                            {
                                DataTypes.DataType.Cases.ExternalDatasourceInfo exd = sxternal.Result.FirstOrDefault();

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
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
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
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
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
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
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
                DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            }
            catch (Exception)
            {
            }
            try
            {
                if (Onlineflag)
                {
                    object cntrl = new object();
                    bool ISadd = false;
                    string selectedId = string.Empty;
                    string selectedname = string.Empty;
                    var cntr = sControls.Where(v => v.AssocFieldType != 'E' || v.AssocFieldType != 'O');
                    foreach (var item in cntr)
                    {
                        foreach (StackLayout infofield in CasesView_ControlStack.Children)
                        {
                            foreach (StackLayout sitem in infofield.Children)
                            {
                                foreach (var subitem in sitem.Children)
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
                                                    selectedId = Convert.ToString((picker.SelectedItem as ExternalDatasourceValue)?.ID);
                                                    selectedname = (picker.SelectedItem as ExternalDatasourceValue)?.NAME;
                                                    assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + Convert.ToString((picker.SelectedItem as ExternalDatasourceValue)?.ID));
                                                    assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), (picker.SelectedItem as ExternalDatasourceValue)?.NAME);
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

                                                        assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), CommonConstants.DateFormatStringToString(en.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"));

                                                    }
                                                    else
                                                    {
                                                        assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + Convert.ToString(CurrentStyleId);

                                                        assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = CommonConstants.DateFormatStringToString(en.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy");
                                                    }
                                                    cntrl = subitem;
                                                }
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
                            CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());

                        else
                            CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(),
                                sControls.Where(v => v.AssocFieldType == 'C').ToList(),
                                CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0], (selectedId == "-1" ? "0|" + selectedname : selectedId + "|" + selectedname));
                    }
                    else if (x.Name == "Entry")
                    {
                        CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());
                    }
                    else if (x.Name == "DatePicker")
                    {
                        CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList());
                    }

                    if (!string.IsNullOrEmpty(CalTxtbox))
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


                            var Result = CasesSyncAPIMethods.CallCalculations(Onlineflag, App.DBPath, rf);
                            Result.Wait();

                            foreach (StackLayout infofield in CasesView_ControlStack.Children)
                            {
                                foreach (StackLayout sitem in infofield.Children)
                                {
                                    foreach (var subitem in sitem.Children)
                                    {
                                        var xy = subitem;
                                        Type ty = xy.GetType();
                                        if (ty.Name.ToLower() != "stacklayout")
                                        {
                                            if (ty.Name.ToLower() == "entry")
                                            {
                                                var en = (Entry)xy;
                                                if (en.StyleId == Convert.ToString(sCalId.ToLower().Substring(sCalId.ToLower().IndexOf('_') + 1)).ToLower())
                                                {
                                                    en.Text = Convert.ToString(Result.Result);
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
            catch (Exception ex)
            {

            }
        }

        public void DyanmicSetCalcexd(string CurrentStyleId, List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal)
        {
            try
            {
                if (Onlineflag)
                {
                    object cntrl = new object();
                    string selectedId = string.Empty;
                    string selectedname = string.Empty;
                    foreach (var item in sControls)
                    {
                        foreach (StackLayout infofield in CasesView_ControlStack.Children)
                        {
                            foreach (StackLayout sitem in infofield.Children)
                            {
                                foreach (var subitem in sitem.Children)
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
                                                    selectedValue = picker.SelectedItem as ExternalDatasourceValue;
                                                    selectedId = Convert.ToString(selectedValue.ID);
                                                    selectedname = selectedValue.NAME;
                                                    if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                                    {
                                                        assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + Convert.ToString((picker.SelectedItem as ExternalDatasourceValue)?.ID));
                                                        assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), (picker.SelectedItem as ExternalDatasourceValue)?.NAME);
                                                    }
                                                    else
                                                    {
                                                        assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + Convert.ToString((picker.SelectedItem as ExternalDatasourceValue)?.ID);
                                                        assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = (picker.SelectedItem as ExternalDatasourceValue)?.NAME;
                                                    }
                                                }
                                            }
                                        }
                                        else if (ty.Name.ToLower() == "button")
                                        {
                                            var btn = (Button)subitem;
                                            if (btn.StyleId == CurrentStyleId)
                                            {
                                                cntrl = subitem;
                                                List<ExternalDatasourceValue> lst = lstextdatasourceHistory.Where(v => v.Key == Convert.ToInt32(CurrentStyleId))?.FirstOrDefault().Value;

                                                try
                                                {
                                                    selectedId = lst == null ? Convert.ToString(metadatacollection.Where(v => v.AssociatedTypeID == Convert.ToInt32(CurrentStyleId)).FirstOrDefault().ExternalDatasourceObjectID) : Convert.ToString(lst?.FirstOrDefault().ID);
                                                    selectedname = Convert.ToString(btn.Text);

                                                    var aTypeFiled = sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId)).Select(a => a.AssocFieldType)?.FirstOrDefault();

                                                    if (!assocFieldValues.ContainsKey("assoc_" + aTypeFiled.ToString() + "_" + CurrentStyleId.Split('|')[0].ToUpper()))
                                                    {
                                                        assocFieldValues.Add("assoc_" + aTypeFiled.ToString() + "_" + CurrentStyleId.Split('|')[0].ToUpper(), item.AssocTypeID + "|" + selectedId);
                                                        assocFieldTexts.Add("assoc_" + aTypeFiled.ToString() + "_" + CurrentStyleId.Split('|')[0].ToUpper(), selectedname);
                                                    }
                                                    else
                                                    {
                                                        assocFieldValues["assoc_" + aTypeFiled.ToString() + "_" + CurrentStyleId.Split('|')[0].ToUpper()] = item.AssocTypeID + "|" + selectedId;
                                                        assocFieldTexts["assoc_" + aTypeFiled.ToString() + "_" + CurrentStyleId.Split('|')[0].ToUpper()] = selectedname;
                                                    }
                                                }
                                                catch
                                                {
                                                }
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
                        CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControlscal, selectedId + "|" + selectedname);
                    }
                    else if (x.Name == "Button")
                    {
                        CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId)).ToList(), sControlscal, "", selectedId + "|" + selectedname);
                    }

                    if (!string.IsNullOrEmpty(CalTxtbox))
                    {

                        int cnt = 0;
                        string sCalId = string.Empty;
                        foreach (var item in calculationsFieldlist)
                        {
                            sCalId = item.Value?.ToString();
                            cnt++;
                            if (cnt > calculationsFieldlist.Count)
                                return;

                            foreach (var iitem in assocFieldValues.Keys.ToList())
                            {
                                var ctrl = FindPickerControls(Convert.ToInt32(iitem.Split('_').Last()));
                                if (ctrl != null)
                                {
                                    Button btn = ctrl as Button;
                                    btn.FontSize = 16;
                                    if (btn.Text == "-- Select Item --")
                                    {
                                        assocFieldValues[iitem] = iitem.Split('_').Last() + "|" + -1;

                                        assocFieldTexts[iitem] = "-- Select Item --";
                                    }
                                }
                            }
                            RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();
                            rf.assocFieldCollection = JsonConvert.SerializeObject(sControlsList);
                            rf.assocFieldValues = assocFieldValues;
                            rf.assocFieldTexts = assocFieldTexts;
                            rf.calculatedAssocId = sCalId;
                            rf.sdateFormats = "MM/dd/yyyy";

                            var Result = CasesSyncAPIMethods.CallCalculations(Onlineflag, App.DBPath, rf);

                            Result.Wait();

                            foreach (StackLayout infofield in CasesView_ControlStack.Children)
                            {
                                foreach (StackLayout sitem in infofield.Children)
                                {

                                    foreach (var subitem in sitem.Children)
                                    {
                                        var xy = subitem;
                                        Type ty = xy.GetType();
                                        if (ty.Name.ToLower() != "stacklayout")
                                        {
                                            if (ty.Name.ToLower() == "entry")
                                            {
                                                var en = (Entry)xy;
                                                Device.BeginInvokeOnMainThread(() =>
                                                {
                                                    if (en.StyleId.ToLower() == Convert.ToString(sCalId.ToLower().Substring(sCalId.ToLower().IndexOf('_') + 1)).ToLower())
                                                    {
                                                        en.Text = "";
                                                        en.Text = Convert.ToString(Result.Result);
                                                    }
                                                });
                                            }
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
            }
        }

        public void SetStaticCal(string ContolrLst)
        {
            string calc = string.Empty;
            List<GetCaseTypesResponse.ItemType> calControls = new List<GetCaseTypesResponse.ItemType>();
            calControls = sControls.Where(v => v.AssocFieldType == 'C').ToList();

            foreach (var item in calControls)
            {
                try
                {
                    RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();
                    rf.assocFieldCollection = JsonConvert.SerializeObject(sControlsList);
                    rf.assocFieldValues = assocFieldValues;
                    rf.assocFieldTexts = assocFieldTexts;
                    rf.calculatedAssocId = "assoc_C_" + item.AssocTypeID;
                    rf.sdateFormats = "MM/dd/yyyy";

                    var Result = CasesSyncAPIMethods.CallCalculations(Onlineflag, App.DBPath, rf);
                    Result.Wait();
                    if (!string.IsNullOrEmpty(Result.Result))
                    {
                        var cnt = FindCasesControls(Convert.ToInt32(item.AssocTypeID));
                        if (cnt != null)
                        {
                            Entry ent = new Entry();
                            ent = cnt as Entry;
                            ent.Text = Result.Result;
                        }
                        calc = "";
                    }
                }
                catch (Exception ex)
                {
                }
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

                if (_Casedata.CaseOwnerSAM.ToLower() == Functions.UserName && _Casedata.CaseAssignedToSAM.ToLower() == Functions.UserName)
                {
                    //if Case Owner and CaseAssignedTo to the Current User
                    buttons = new string[] { sb.ToString(), "Run Synchronization", "Assign", "Return to Last Assigner", "Return to Last Assignee", "Approve and Return", "Approve and Assign", "Decline and Return", "Decline and Assign", "Close Case", "Save As Favourite", "Email Link", "Add Attachment", "Activity Log", "Logout" };
                }
                else if (_Casedata.CaseOwnerSAM.ToLower() == Functions.UserName)
                {
                    //if Case Owner is Current User
                    buttons = new string[] { sb.ToString(), "Run Synchronization", "Assign", "Return to Last Assigner", "Return to Last Assignee", "Approve and Return", "Approve and Assign", "Decline and Return", "Decline and Assign", "Close Case", "Save As Favourite", "Assigned To Me", "Email Link", "Add Attachment", "Activity Log", "Logout" };
                }
                else if (_Casedata.CaseAssignedToSAM.ToLower() == Functions.UserName)
                {
                    //if CaseAssignedTo is Current User
                    buttons = new string[] { sb.ToString(), "Run Synchronization", "Assign", "Return to Last Assigner", "Return to Last Assignee", "Approve and Return", "Approve and Assign", "Decline and Return", "Decline and Assign", "Close Case", "Save As Favourite", "Take Ownership", "Email Link", "Add Attachment", "Activity Log", "Logout" };
                }
                else
                {
                    buttons = new string[] { sb.ToString(), "Run Synchronization", "Assign", "Return to Last Assigner", "Return to Last Assignee", "Approve and Return", "Approve and Assign", "Decline and Return", "Decline and Assign", "Close Case", "Save As Favourite", "Assigned To Me", "Take Ownership", "Email Link", "Add Attachment", "Activity Log", "Logout" };
                }

                var action = await this.DisplayActionSheet(null, "Cancel", null, buttons);

                if (action.ToLower().Contains("offline"))
                    action = "offline";
                else if (action.ToLower().Contains("online"))
                    action = "online";

                SaveCaseTypeRequest savecase = new SaveCaseTypeRequest();
                CreateCaseOptimizedRequest.CreateCaseModelOptimized createcase = new CreateCaseOptimizedRequest.CreateCaseModelOptimized();

                Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue> dropDownValues = new Dictionary<int, GetTypeValuesByAssocCaseTypeExternalDSResponse.ItemValue>();
                Dictionary<int, string> textValues = new Dictionary<int, string>();
                List<CreateCaseOptimizedRequest.LinkValue> linkValue = new List<CreateCaseOptimizedRequest.LinkValue>();


                if (action != "Run Synchronization" && action != "Cancel" && action != "Logout" && action != "Add Attachment" && action != "Email Link"
                    && action != "Activity Log" && action != "offline" && action != "online")
                {

                    #region create Json
                    #region  Dynamic Control Fill 
                    foreach (StackLayout infofield in CasesView_ControlStack.Children)
                    {
                        foreach (StackLayout item in infofield.Children)
                        {
                            foreach (var subitem in item.Children)
                            {
                                var xy = subitem;
                                Type ty = xy.GetType();
                                if (ty.Name.ToLower() != "stacklayout")
                                {
                                    if (ty.Name.ToLower() == "entry")
                                    {
                                        try
                                        {
                                            var en = (Entry)xy;
                                            if (en.StyleId == _TitleFieldControlID)
                                            {
                                                savecase.caseTitle = en.Text;
                                            }
                                            int Key = int.Parse(en.StyleId.Split('_')[1]?.ToString());
                                            string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                            string FileldName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();

                                            if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(en.Text))
                                            {
                                                DisplayAlert("Field Required.", "Please enter valid data in " + FileldName, "OK");

                                                goto requiredJump;
                                            }
                                            else
                                            {
                                                // To Save Date in Standard Format
                                                if (!en.StyleId.Contains("a_"))
                                                {
                                                    textValues.Add(Key, en.Text);
                                                }
                                                else
                                                {
                                                    var stDate = string.Empty;
                                                    if (!string.IsNullOrEmpty(en.Text))
                                                    {
                                                        var sDate = Convert.ToDateTime(en.Text);
                                                        stDate = sDate.Date.ToString("MM/dd/yyyy");
                                                    }
                                                    textValues.Add(Key, stDate);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    else if (ty.Name.ToLower() == "picker")
                                    {
                                        try
                                        {
                                            var picker = (Picker)xy;

                                            ItemValue itemValue = new ItemValue();

                                            if (!picker.StyleId.Contains("NoteType"))
                                            {
                                                string styletype = picker.StyleId.Split('_')[0].Split('|')[0].ToString();

                                                if (styletype.ToLower() == "d")
                                                {
                                                    int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                                    string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                                    string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
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
                                                        string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                                        string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                                        if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(FiledName))
                                                        {
                                                            DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                                            goto requiredJump;
                                                        }
                                                        else
                                                        {
                                                            if (picker.StyleId.Split('_')[0].ToLower() == "o")
                                                            {
                                                                var val = picker.SelectedItem as ExternalDatasourceValue;
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
                                                                var val = picker.SelectedItem as ExternalDatasourceValue;
                                                                ItemValue it = new ItemValue();
                                                                if (val != null)
                                                                {
                                                                    it.Name = val.NAME;
                                                                    it.AssocDecodeID = Convert.ToInt32(val.ID);
                                                                    dropDownValues.Add(Key, it);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    else if (ty.Name.ToLower() == "button")
                                    {
                                        try
                                        {
                                            Button btn = new Button();
                                            btn = (Button)xy;

                                            ItemValue it = new ItemValue();
                                            if (btn != null)
                                            {
                                                int Key = int.Parse(btn.StyleId);
                                                if (btn.StyleId.Split('|')[0] == _TitleFieldControlID)
                                                {
                                                    savecase.caseTitle = btn.Text;
                                                }

                                                var tempassocdecodeid = lstextdatasourceHistory.Where(v => v.Key == Convert.ToInt32(btn.StyleId))?.FirstOrDefault().Value;

                                                string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                                string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                                if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && btn.Text == "-- Select Item --")
                                                {
                                                    DisplayAlert("Field Required.", "Please Select valid item in " + FiledName, "OK");

                                                    goto requiredJump;
                                                }
                                                else
                                                {
                                                    it.Name = btn.Text;
                                                    it.AssocDecodeID = Convert.ToInt32(tempassocdecodeid?.FirstOrDefault().ID);
                                                    dropDownValues.Add(Key, it);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            //throw ex;
                                        }
                                    }
                                    else if (ty.Name.ToLower() == "bordereditor")
                                    {
                                        try
                                        {
                                            var editor = (BorderEditor)xy;
                                            if (editor.StyleId == _TitleFieldControlID)
                                            {
                                                savecase.caseTitle = editor.Text;
                                            }

                                            string isReq = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString())).FirstOrDefault().IS_REQUIRED.ToString();
                                            string FiledName = AssignControlsmetadata.Where(c => c.ASSOC_TYPE_ID == int.Parse(editor.StyleId.Split('_')[1]?.ToString()))?.FirstOrDefault()?.NAME?.ToString();
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
                                        }
                                    }
                                    else if (ty.Name.ToLower() == "datepicker")
                                    {
                                        try
                                        {
                                            //var datepicker = (DatePicker)xy;

                                            //if (datepicker.Date != Convert.ToDateTime("01/01/1900"))
                                            //{
                                            //    textValues.Add(int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), CommonConstants.DateFormatStringToString(datepicker.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy"));
                                            //}
                                            //else
                                            //    textValues.Add(int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), "");
                                        }
                                        catch (Exception ex)
                                        {
                                        }
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
                    createcase.metaDataValues = dctmetaDataValues;
                    #endregion
                }
                string tm_uname = null;
                try
                {
                    tm_uname = DBHelper.GetAppTypeInfo_tmname(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), Convert.ToInt32(Casetypeid), "E2_GetCaseList" + strTome, App.DBPath).Result?.TM_Username;
                    tm_uname = tm_uname ?? Functions.UserName;
                }
                catch (Exception)
                {
                    tm_uname = Functions.UserName;
                }
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

                        await this.Navigation.PushAsync(new AssignCase(Convert.ToString(CaseID), Casetypeid, createcase, txt_CasNotes.Text, Functions.UserName, "U", savecase, false, false, strTome, Onlineflag));
                        txt_CasNotes.Text = string.Empty;
                        break;

                    #region Return to Last Assignee
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
                                  Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "T", tm_uname);
                                  if (offlinerecord == null)
                                  {
                                      offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "M", tm_uname);
                                  }

                                  offlinerecord.Wait();
                                  BasicCase objview = new BasicCase();
                                  if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                  {
                                      try
                                      {
                                          var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                          objview = objviewtemp?.FirstOrDefault();
                                          if (objview == null)
                                          {
                                              objview = objviewtemp.FirstOrDefault();
                                          }
                                      }
                                      catch
                                      {
                                          objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                      }
                                  }


                                  CasesSyncAPIMethods.storeReturnToLastAssignee(Onlineflag, objReturnCaseToLastAssignee, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, Functions.UserFullName, strTome);

                                  // To manage the footer for Return To Functionality Only
                                  HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);

                              }
                          });

                            if (ReturnToLastAssignee != null && ReturnToLastAssignee?.Result > 0)
                            {
                                Functions.ShowtoastAlert("Case Returned to Assignee Successfully.");
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Case Return to Assignee Operation failed. Please Try Again Later.");
                            }
                            this.OnAppearing();
                        }
                        catch (Exception)
                        {
                        }
                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    #endregion

                    #region Return to Last Assigner
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

                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "T", tm_uname);

                                    if (offlinerecord == null)
                                    {
                                        offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "M", tm_uname);
                                    }
                                    offlinerecord.Wait();

                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                        catch
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                    }

                                    string _caseid = string.Empty;
                                    switch (offlinerecord.Result?.IS_ONLINE)
                                    {
                                        case true:
                                            _caseid = Convert.ToString(CaseID);
                                            //CasesSyncAPIMethods.storeReturnToLastAssigner(Onlineflag, objReturnCaseToLastAssigner, Convert.ToInt32(Casetypeid), _caseid, Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, strTome);
                                            break;
                                        default:
                                            _caseid = Convert.ToString(ReturnToLastAssigner.Result);
                                            //CasesSyncAPIMethods.storeReturnToLastAssigner(Onlineflag, objReturnCaseToLastAssigner, Convert.ToInt32(Casetypeid), Convert.ToString(ReturnToLastAssigner.Result), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, strTome);
                                            break;
                                    }

                                    CasesSyncAPIMethods.storeReturnToLastAssigner(Onlineflag, objReturnCaseToLastAssigner, Convert.ToInt32(Casetypeid), _caseid, Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, strTome);

                                    // To manage the footer for Return To Functionality Only
                                    HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);

                                }
                            });

                            if (ReturnToLastAssigner != null && ReturnToLastAssigner?.Result > 0)
                            {
                                Functions.ShowtoastAlert("Case Returned to last Assigner Successfully.");
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Case Returned To Last Assigner Operation failed. Please Try Again Later.");
                            }
                            this.OnAppearing();
                        }
                        catch (Exception)
                        {
                        }
                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    #endregion

                    #region Approve and Return
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
                                    Task<AppTypeInfoList> offlinerecord = null;
                                    offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "T", tm_uname);

                                    if (offlinerecord.Result == null)
                                    {
                                        offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "M", tm_uname);
                                    }

                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                        catch
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                    }
                                    string _caseID = string.Empty;

                                    if (offlinerecord.Result.IS_ONLINE == true)
                                        _caseID = CaseID;
                                    else
                                        _caseID = Convert.ToString(ApproveAndReturn.Result);


                                    CasesSyncAPIMethods.storeApproveandReturn(Onlineflag, objReturnCaseToLastAssignee, Convert.ToInt32(Casetypeid), Convert.ToString(_caseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, "", objview, Casetitle, Functions.UserFullName, strTome);


                                    // To manage the footer for Return To Functionality Only
                                    HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);
                                }
                            });

                            if (ApproveAndReturn != null && ApproveAndReturn.Result > 0)
                            {
                                Functions.ShowtoastAlert("Case Approved and Returned Successfully.");
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Case Approved and Return operation failed. Please Try Again Later.");
                            }
                            this.OnAppearing();
                        }
                        catch (Exception)
                        {
                        }
                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    #endregion

                    #region Approve and Assign
                    case "Approve and Assign":
                        await this.Navigation.PushAsync(new AssignCase(Convert.ToString(CaseID), Casetypeid, createcase, txt_CasNotes.Text, Functions.UserName, "U", savecase, true, false, strTome, Onlineflag));
                        txt_CasNotes.Text = string.Empty;
                        break;
                    #endregion

                    #region Decline and Return
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
                                        Username = Functions.UserName
                                        //notes = txt_CasNotes.Text
                                    };

                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "T", tm_uname);
                                    if (offlinerecord == null)
                                    {
                                        offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "M", tm_uname);
                                    }

                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                        catch
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                    }

                                    CasesSyncAPIMethods.storeDeclineAndReturn(Onlineflag, objDeclineAndReturn, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, Functions.UserFullName, strTome);

                                    // To manage the footer for Return To Functionality Only
                                    HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);

                                }
                            });

                            if (DeclineAndReturn != null && DeclineAndReturn.Result > 0)
                            {
                                Functions.ShowtoastAlert("Case Declined and Returned Successfully.");
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Case Decline and Return Operation failed. Please Try Again Later.");
                            }
                            this.OnAppearing();
                        }
                        catch (Exception)
                        {
                        }
                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    #endregion

                    #region Decline and Assign
                    case "Decline and Assign":

                        await this.Navigation.PushAsync(new AssignCase(Convert.ToString(CaseID), Casetypeid, createcase, txt_CasNotes.Text, Functions.UserName, "U", savecase, false, true, strTome, Onlineflag));

                        txt_CasNotes.Text = string.Empty;
                        break;
                    #endregion

                    #region Close Case
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

                                    Task<AppTypeInfoList> offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "T", tm_uname);

                                    if (offlinerecord == null)
                                    {
                                        offlinerecord = DBHelper.GetAppTypeInfoByTransTypeSyscodewithouttypeid(ConstantsSync.CasesInstance, Convert.ToInt32(CaseID), App.DBPath, "E2_GetCaseList" + strTome, "M", tm_uname);
                                    }

                                    offlinerecord.Wait();
                                    BasicCase objview = new BasicCase();
                                    if (!string.IsNullOrEmpty(offlinerecord.Result?.ASSOC_FIELD_INFO))
                                    {
                                        try
                                        {
                                            var objviewtemp = JsonConvert.DeserializeObject<List<BasicCase>>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                            objview = objviewtemp?.Where(v => v.CaseID == Convert.ToInt32(CaseID))?.FirstOrDefault();
                                            if (objview == null)
                                            {
                                                objview = objviewtemp.FirstOrDefault();
                                            }
                                        }
                                        catch
                                        {
                                            objview = JsonConvert.DeserializeObject<BasicCase>(offlinerecord?.Result?.ASSOC_FIELD_INFO);
                                        }
                                    }
                                    CasesSyncAPIMethods.storeClose(Onlineflag, objClose, Convert.ToInt32(Casetypeid), Convert.ToString(CaseID), Functions.UserName, App.DBPath, "C1_C2_CASES_CASETYPELIST", "", ConstantsSync.CasesInstance, objview, Casetitle, Functions.UserFullName, strTome);


                                }
                            });

                            if (Close != null && Close.Result > 0)
                            {
                                Functions.ShowtoastAlert("Case Closed Successfully.");
                                //var answer = DisplayAlert("Close Case", "Case Close Successfully.", "OK");
                                if (strTome == "_AssignedToMe")
                                {
                                    await this.Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));
                                }
                                else if (strTome == "_CreatedByMe")
                                {
                                    await this.Navigation.PushAsync(new CaseList("caseCreateBySAM", Functions.UserName, ""));
                                }
                                else if (strTome == "_OwnedByMe")
                                {
                                    await this.Navigation.PushAsync(new CaseList("caseOwnerSAM", Functions.UserName, ""));
                                }
                                else if (strTome == "_AssignedToMyTeam")
                                {
                                    var tm_uname1 = DBHelper.GetAppTypeInfo_tmname(ConstantsSync.CasesInstance, _Casedata.CaseID, _Casedata.CaseTypeID, "E2_GetCaseList_AssignedToMyTeam", App.DBPath).Result?.TM_Username;

                                    await this.Navigation.PushAsync(new CaseList("caseAssgnTM", tm_uname1 ?? Functions.UserName, ""));
                                }
                                else
                                    await Navigation.PushAsync(new CaseList("casetypeid", Convert.ToString(Casetypeid), "", Casetitle));
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Case Close Operation failed. Please Try Again Later.");
                                // var answer = DisplayAlert("Close Case", "Case Not Close. Please Try Again Later.", "OK");
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;

                    #endregion

                    #region Save As Favourite
                    case "Save As Favourite":
                        var user_dialogs = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                            "Name of Favorite").
                            SetInputMode(InputType.Name).SetOkText("Create"));

                        if (user_dialogs.Ok)
                        {
                            if (user_dialogs.Text != "")
                            {
                                string txt_search = user_dialogs.Text;
                                var fav_metadata = JsonConvert.SerializeObject(createcase);
                                Task<int> result_fav = null;
                                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                await Task.Run(() =>
                                {
                                    result_fav = CasesSyncAPIMethods.AddFavorite(App.Isonline, txt_search, fav_metadata,
                      "Y", Functions.UserName, DateTime.Now.ToString(), DateTime.Now.ToString(), DateTime.Now.ToString(), App.DBPath, Casetypeid, ConstantsSync.INSTANCE_USER_ASSOC_ID.ToString(), Convert.ToString((int)Applications.Cases));
                                });
                                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

                                if (result_fav != null && result_fav?.Result > 0)
                                {
                                    DisplayAlert("Success", "Added Favourite Successfully.", "OK");

                                    var existingPages = this.Navigation.NavigationStack.ToList();
                                    // Get the page before Create Case
                                    //              or
                                    // Get the Navigation Parent Page

                                    var ph = existingPages[existingPages.Count - 3];

                                    if (ph.GetType().Name.ToString().ToLower() == "selectcasetype")
                                    {
                                        await Navigation.PushAsync(new SelectCaseType());

                                        Navigation.RemovePage(ph);// remove SelectCaseType() Page from Queue

                                        existingPages = this.Navigation.NavigationStack.ToList();
                                        var rm_cp = existingPages[existingPages.Count - 3];
                                        this.Navigation.RemovePage(rm_cp);//remove only CaseList() Page                                   

                                        existingPages = this.Navigation.NavigationStack.ToList();
                                        var rm_vc = existingPages[existingPages.Count - 2];
                                        this.Navigation.RemovePage(rm_vc);//remove only ViewCase() Page
                                    }
                                    else if (ph.GetType().Name.ToString().ToLower() == "landingpage")
                                    {
                                        await Navigation.PushAsync(new CaseList("caseAssgnSAM", Functions.UserName, ""));

                                        existingPages = this.Navigation.NavigationStack.ToList();
                                        var rm_vc = existingPages[existingPages.Count - 3];
                                        this.Navigation.RemovePage(rm_vc);//remove only ViewCase() Page

                                        existingPages = this.Navigation.NavigationStack.ToList();
                                        var rm_cl = existingPages[existingPages.Count - 2];
                                        this.Navigation.RemovePage(rm_cl);//remove only ViewCase() Page
                                    }
                                }
                            }
                            else
                            {
                                DisplayAlert("Alert", "Please Add Favourite Name.", "OK");
                            }
                        }
                        break;
                    #endregion

                    #region Assigned To Me
                    case "Assigned To Me":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                        try
                        {
                            AcceptCaseRequest obj = new AcceptCaseRequest()
                            {
                                CaseId = Convert.ToInt32(CaseID),
                                caseOwnerSAM = Functions.UserName,
                                username = Functions.UserName
                            };
                            GetUserInfoResponse.UserInfo userInfo = new GetUserInfoResponse.UserInfo();
                            userInfo.SAMName = Functions.UserName;
                            userInfo.DisplayName = Functions.UserFullName;
                            int Assigntome = 0;

                            try
                            {
                                await Task.Run(async () =>
                                {
                                    Assigntome = await CasesSyncAPIMethods.StoreAndUpdateCase(Onlineflag, Convert.ToInt32(Casetypeid), savecase, txt_CasNotes.Text, Functions.UserName, App.DBPath, createcase, userInfo, null, "19", false, false, Functions.UserFullName, strTome);
                                });
                            }
                            catch (Exception)
                            {
                            }

                            //await Task.Run(() =>
                            //{
                            //    Assigntome = SaveAndUpdate(savecase, createcase, obj);
                            //    Assigntome.Wait();
                            //});

                            if (Assigntome > 0)
                            {
                                Functions.ShowtoastAlert("You have Successfully Assinged this case.");
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Case Assignment Operation has not been Success. Please try again later");
                            }
                            this.OnAppearing();
                        }
                        catch (Exception)
                        {
                        }

                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    #endregion

                    #region Take Ownership
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
                                Functions.ShowtoastAlert("You have Successfully Owned this case.");
                            }
                            else
                            {
                                Functions.ShowtoastAlert("Ownership has not been taken. Please try again later");
                            }
                            this.OnAppearing();
                        }
                        catch (Exception)
                        {
                        }
                        txt_CasNotes.Text = string.Empty;
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        break;
                    #endregion


                    case "Email Link":
                        EmailLink(savecase);
                        break;

                    case "Add Attachment":
                        AddAttachment();
                        break;

                    case "Activity Log":
                        await this.Navigation.PushAsync(new CaseActivityLog(CaseID.ToString(), _Casedata.CaseTypeID, Onlineflag));
                        break;
                    case "Logout":
                        App.Logout(this);
                        break;
                }
            }
            catch (Exception ex)
            {
            }

            requiredJump:
            int abc = 0;

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        async void AddAttachment()
        {
            try
            {
                string[] buttons;
                buttons = new string[] { "Upload Document", "From Photo Gallery", "Camera" };

                var action = await this.DisplayActionSheet(null, "Cancel", null, buttons);

                switch (action)
                {
                    case "Upload Document":
                        PickFile();
                        break;
                    case "From Photo Gallery":
                        OpenGallery();
                        break;
                    case "Camera":
                        TakePhoto();
                        break;

                }
            }
            catch (Exception ex)
            {

            }
        }

        async void PickFile()
        {
            //string[] fileTypes = new string[] { "application/pdf", "text/plain", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword", "application/vnd.ms-outlook", "application/zip", "application/x-rar-compressed", "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-powerpoint", "application/vnd.openxmlformats-officedocument.presentationml.presentation", "application/json", "text/javascript", "application/sql", "application/vnd.ms-visio.drawing.main+xml", "application/vnd.ms-visio.drawing.macroEnabled.main+xml", "application/vnd.ms-visio.stencil.main+xml", "application/vnd.ms-visio.stencil.macroEnabled.main+xml", "application/vnd.ms-visio.template.main+xml", "application/vnd.ms-visio.template.macroEnabled.main+xml", "application/acad", "application/x-pointplus", "text/css", "text/x-java-source", "text/csv", "application/octet-stream", "application/xml", "text/xml", "application/vnd.ms-xpsdocument", "application/rtf", "application/x-rtf", "text/richtext", "application/postscript", "font/opentype", "application/vnd.oasis.opendocument.spreadsheet", "application/x-visio", "application/x-mspublisher", "text/html" };

            try
            {

                if (App.Isonline)
                {
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                    var file = await CrossFilePicker.Current.PickFile();

                    byte[] fileBytes = null;
                    long size = 0;

                    if (file != null)
                    {
                        string FileName = file.FileName;


                        fileBytes = file.DataArray;

                        size = fileBytes.Count();

                        string FileId = string.Empty;

                        var d = CasesAPIMethods.UploadFileToCase(Convert.ToInt32(CaseID), "", DateTime.Now, FileName, size.ToString(), fileBytes, "", 'Y', "", 'y', Functions.UserName);

                        FileId = d.GetValue("ResponseContent").ToString();

                        //FileId = d;

                        if (FileId == null || FileId == "-1")
                        {
                            Functions.ShowtoastAlert("Something went wrong in file attachment");
                            return;
                        }

                        if (!string.IsNullOrEmpty(FileId?.ToString()) && FileId.ToString() != "[]") //FileID not null
                        {
                            string Notes = "User Uploaded File: <a href=\"/DownloadFile.aspx?CaseFileID=" + FileId + "\">" + FileName + "</a>.<br />Hash Code: <b></b><br/>Description: <br /><img src=\"/DownloadFile.aspx?CaseFileID=" + FileId + "\" alt=\"\" style=\"max-width: {PXWIDE};\" />";

                            await Task.Run(() =>
                            {
                                CasesSyncAPIMethods.AddCasNotes(App.Isonline, Convert.ToInt32(Casetypeid), CaseID, Notes, Functions.UserName, "19", App.DBPath, Functions.UserFullName);
                            });

                            if (!string.IsNullOrEmpty(txt_CasNotes.Text))
                            {
                                CasesSyncAPIMethods.AddCasNotes(App.Isonline, Convert.ToInt32(Casetypeid), CaseID, txt_CasNotes.Text, Functions.UserName, "19", App.DBPath, Functions.UserFullName);

                                Functions.ShowtoastAlert("File Attached Successfully");
                            }
                        }
                    }
                    else
                    {
                        Functions.ShowtoastAlert("Please go online to use this functionality!");
                    }

                    txt_CasNotes.Text = string.Empty;

                    ReloadNotesArea();
                }
            }
            catch (Exception ex)
            {
                Functions.ShowtoastAlert("An exception occured");
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }


        async void OpenGallery()
        {
            try
            {
                if (App.Isonline)
                {
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    try
                    {
                        await CrossMedia.Current.Initialize();

                        if (!CrossMedia.Current.IsPickPhotoSupported)
                        {
                            DisplayAlert("No Gallery", ":( No Gallery available.", "Ok");
                            return;
                        }

                        var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                        {
                            PhotoSize = PhotoSize.Medium,
                            CompressionQuality = 80,
                        });

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
                else
                    Functions.ShowtoastAlert("Please Go online to use this functionality!");

                ReloadNotesArea();
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

        }

        async void TakePhoto()
        {

            try
            {
                if (App.Isonline)
                {
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    try
                    {
                        await CrossMedia.Current.Initialize();

                        if (!CrossMedia.Current.IsPickPhotoSupported)
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
                else
                    Functions.ShowtoastAlert("Please Go online to use this functionality!");

                ReloadNotesArea();
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
        private async void lbl_createname_tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Create_Sam))
                {
                    await this.Navigation.PushAsync(new UserDetail(Create_Sam));
                }
            }
            catch (Exception)
            {
            }
        }

        private async void lbl_assignto_tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Assign_Sam))
                {
                    await this.Navigation.PushAsync(new UserDetail(Assign_Sam));
                }
            }
            catch (Exception)
            {
            }
        }

        private async void lbl_ownername_tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Owner_Sam))
                {
                    await this.Navigation.PushAsync(new UserDetail(Owner_Sam));
                }
            }
            catch (Exception)
            {
            }
        }

        private async void lbl_modifiedname_tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Modify_Sam))
                {
                    await this.Navigation.PushAsync(new UserDetail(Modify_Sam));
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public class CasesPulltorefreshViewmodel : INotifyPropertyChanged
    {
        Page page;
        public CasesPulltorefreshViewmodel(Page page)
        {
            this.page = page;
        }

        bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy == value)
                    return;

                isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }

        ICommand refreshCommand;

        public ICommand RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        async Task ExecuteRefreshCommand()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                var p = page as ViewCasePage;
                p.Onlineflag = true;

                Task.Run(() =>
                {
                    HelperProccessQueue.SyncSqlLiteTableWithSQLDatabase(App.DBPath, ConstantsSync.INSTANCE_USER_ASSOC_ID, Functions.UserName);
                });

                await p.ExtendedOnAppearing();

                p.IsBusy = false;
                IsBusy = false;
            }
            catch (Exception)
            {
            }
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
