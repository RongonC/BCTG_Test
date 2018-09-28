
using Acr.UserDialogs;
using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;

using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.View_Case_Origination_Center;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;
using static StemmonsMobile.DataTypes.DataType.Cases.GetCaseTypesResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.GetFavoriteResponse;
using static StemmonsMobile.DataTypes.DataType.Cases.GetTypeValuesByAssocCaseTypeExternalDSResponse;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewCase : ContentPage
    {
        string Casetypeid = string.Empty;
        string Favoriteid = string.Empty;
        string fav_json = "";
        CaseData casedatalist = new CaseData();
        List<GetCaseTypesResponse.ItemType> sControls = new List<GetCaseTypesResponse.ItemType>();

        string ContolrLst = string.Empty;
        string ischeckcalControl = string.Empty;

        Dictionary<int, string> metaDataValues_fav = new Dictionary<int, string>();
        List<CreateCaseOptimizedRequest.TextValue> metaDatatextValues_fav = new List<CreateCaseOptimizedRequest.TextValue>();

        Dictionary<string, string> assocFieldValues = new Dictionary<string, string>();
        Dictionary<string, string> assocFieldTexts = new Dictionary<string, string>();
        Dictionary<string, string> tempassocFieldValues = new Dictionary<string, string>();
        BorderEditor txt_CasNotes = new BorderEditor();


        List<KeyValuePair<List<ItemType>, string>> calculationsFieldlist = new List<KeyValuePair<List<ItemType>, string>>();
        List<Tuple<List<ItemType>, string, string>> lstcalculationsFieldlist = new List<Tuple<List<ItemType>, string, string>>();
        List<AssocCascadeInfo> AssocTypeCascades = new List<AssocCascadeInfo>();

        string _TitleFieldControlID = string.Empty;

        public NewCase(string FAVORITEID, string CASETYPEID, string _Tittle = "")
        {
            InitializeComponent();
            try
            {
                App.SetConnectionFlag();

                Favoriteid = FAVORITEID;
                Casetypeid = CASETYPEID;
            }
            catch (Exception ex)
            {
            }
        }

        List<spi_MobileApp_GetTypesByCaseTypeResult> metadata = new List<spi_MobileApp_GetTypesByCaseTypeResult>();

        List<spi_MobileApp_GetCaseTypeDataByUserResult> User_CaseTypeData = new List<spi_MobileApp_GetCaseTypeDataByUserResult>();

        string CasetypeSecurity = string.Empty;

        protected async override void OnAppearing()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {

                Cases_EntryStack.Children.Clear();

                base.OnAppearing();

                List<GetFavorite> list_favorite = new List<GetFavorite>();

                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                masterGrid.Opacity = 0;

                await Task.Run(async () =>
                {
                    if (string.IsNullOrEmpty(Favoriteid) || Favoriteid != "0")
                    {
                        var result_fav = CasesSyncAPIMethods.GetFavorite(App.Isonline, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

                        if (result_fav.Result != null && result_fav.Result.ToString() != "[]")
                        {
                            list_favorite = result_fav.Result;
                        }
                    }

                    metadata = await CasesSyncAPIMethods.AssignControlsAsync(App.Isonline, Convert.ToInt32(Casetypeid), App.DBPath, Functions.UserName);

                    if (metadata.Count > 0)
                    {
                        Parallel.ForEach(metadata, imetadata =>
                        {
                            try
                            {
                                ItemType itmControl = new ItemType()
                                {
                                    AssocFieldType = Convert.ToChar(imetadata.ASSOC_FIELD_TYPE),
                                    AssociatedTypeSecurity = imetadata.ASSOC_SECURITY_TYPE,
                                    AssocTypeID = imetadata.ASSOC_TYPE_ID,
                                    CalculationFormula = imetadata.CALCULATION_FORMULA,
                                    CalculationFrequencyMin = imetadata.CALCULATION_FREQUENCY_MIN,
                                    CaseTypeID = imetadata.CASE_TYPE_ID,
                                    Child = imetadata.CHILD_ID,
                                    CreatedBy = imetadata.CREATED_BY,
                                    CreatedDateTime = imetadata.CREATED_DATETIME,
                                    Description = imetadata.DESCRIPTION,
                                    ExternalDataSourceEntityTypeID = Convert.ToInt32(imetadata.ExternalDataSourceEntityTypeID),
                                    ExternalDataSourceID = imetadata.EXTERNAL_DATASOURCE_ID,
                                    IsActive = Convert.ToChar(imetadata.IS_ACTIVE),
                                    IsRequired = Convert.ToChar(imetadata.IS_REQUIRED),
                                    ModifiedBy = imetadata.MODIFIED_BY,
                                    IsFroceRecalculation = imetadata.IS_FORCE_RECALCULATION,
                                    ModifiedDateTime = imetadata.MODIFIED_DATETIME,
                                    Name = imetadata.NAME,
                                    Parent = imetadata.PARENT_ID,
                                    SecurityType = imetadata.SECURITY_TYPE,
                                    SeparatorCharactor = imetadata.SEPARATOR_CHARACTOR,
                                    ShowOnList = Convert.ToChar(imetadata.SHOW_ON_LIST == null ? "N" : imetadata.SHOW_ON_LIST),
                                    SystemCode = imetadata.SYSTEM_CODE,
                                    SystemPriority = Convert.ToInt32(imetadata.SYSTEM_PRIORITY),
                                    UIWidth = imetadata.UI_WIDTH,
                                    UseCommaSeparator = imetadata.USE_COMMA_SEPARATOR == 'Y' ? true : false,
                                };
                                lock (this)
                                    sControls.Add(itmControl);
                            }
                            catch (Exception ex)
                            {
                            }
                        });
                        ContolrLst = JsonConvert.SerializeObject(sControls);
                    }
                });


                #region  My Favorite New Cases
                try
                {
                    if (list_favorite.Count >= 0)
                    {
                        var fav_data = list_favorite.Where(t => t.FavoriteId.ToString() == Favoriteid);
                        foreach (var item in fav_data)
                        {
                            if (item.FavoriteId.ToString() == Favoriteid.ToString())
                            {
                                fav_json = item.FieldValues;
                            }
                            var CreateCaseData = JsonConvert.DeserializeObject<CreateCaseOptimizedRequest.CreateCaseModelOptimized>(fav_json);
                            metaDataValues_fav = CreateCaseData.metaDataValues;
                            metaDatatextValues_fav = CreateCaseData.textValues;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                #endregion

                CasetypeSecurity = string.IsNullOrEmpty(metadata?.FirstOrDefault()?.SECURITY_TYPE) ? "OPEN" : metadata?.FirstOrDefault()?.SECURITY_TYPE;

                if (CasetypeSecurity != null && (CasetypeSecurity.ToLower().Equals("open") || CasetypeSecurity.ToLower().Contains("c")))
                {

                    metadata = metadata.OrderBy(x => x.SYSTEM_PRIORITY).ToList();

                    #region Dynamic Control Generate API 
                    foreach (var item in metadata)
                    {
                        if ((string.IsNullOrEmpty(item.ASSOC_SECURITY_TYPE) || item.ASSOC_SECURITY_TYPE.ToLower() == "c") && item.IS_REQUIRED.ToLower() == "y")
                        {
                            await DisplayAlert("Alert!", "You do not have permission to create this page.", "Ok");
                            await Navigation.PopAsync();
                            goto securityjump;
                        }

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
                        FormattedString frmtText = new FormattedString();
                        frmtText.Spans.Add(new Span { Text = item.NAME + ":" });

                        if (item.IS_REQUIRED.ToLower() == "y")
                            frmtText.Spans.Add(new Span { Text = " *", ForegroundColor = Color.Red });

                        Label1.FormattedText = frmtText;

                        Label1.HorizontalOptions = LayoutOptions.Start;
                        Label1.FontSize = 16;
                        Label1.WidthRequest = 200;

                        LeftLayout.Children.Add(Label1);
                        Mainlayout.Children.Add(LeftLayout);

                        Picker pk = new Picker();
                        pk.WidthRequest = 200;
                        pk.TextColor = Color.Gray;

                        Entry entry_number = new Entry();
                        entry_number.WidthRequest = 200;
                        entry_number.FontSize = 16;
                        entry_number.Keyboard = Keyboard.Numeric;


                        switch (item.ASSOC_FIELD_TYPE.ToLower())
                        {
                            case "o":
                            case "e":
                                pk.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID + "|" + item.EXTERNAL_DATASOURCE_ID;

                                try
                                {
                                    pk.SelectedIndexChanged += Pk_SelectedIndexChanged;
                                    List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();
                                    GetExternalDataSourceByIdResponse.ExternalDatasource cases_extedataSource = new GetExternalDataSourceByIdResponse.ExternalDatasource
                                    {
                                        NAME = "-- Select Item --",
                                        DESCRIPTION = "-- Select Item --",
                                        ID = 0
                                    };

                                    lst_extdatasource.Add(cases_extedataSource);

                                    string fieldName = string.Empty;

                                    if (App.Isonline)
                                    {
                                        await Task.Run(() =>
                                        {
                                            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                                            var AssocType = json.GetValue("ResponseContent");
                                            AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                                        });

                                        var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == item.ASSOC_TYPE_ID).ToList();
                                        if (assocChild.Count < 1)
                                        {
                                            await Task.Run(() =>
                                            {
                                                var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(App.Isonline, item.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);

                                                temp_extdatasource.Wait();
                                                if (temp_extdatasource.Result.Count > 0)
                                                {
                                                    lst_extdatasource.AddRange(temp_extdatasource.Result);
                                                }
                                            });
                                        }
                                    }
                                    else
                                    {
                                        await Task.Run(() =>
                                        {
                                            var temp_extdatasource = CasesSyncAPIMethods.GetExternalDataSourceById(App.Isonline, item.EXTERNAL_DATASOURCE_ID.ToString(), "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToInt32(Casetypeid), Convert.ToInt32(item.ASSOC_TYPE_ID));

                                            temp_extdatasource.Wait();
                                            if (temp_extdatasource.Result.Count > 0)
                                            {
                                                lst_extdatasource.AddRange(temp_extdatasource.Result);
                                            }
                                        });
                                    }
                                    pk.ItemsSource = lst_extdatasource;

                                    pk.ItemDisplayBinding = new Binding("NAME");

                                    try
                                    {
                                        var pkValuetext = metaDataValues_fav.Where(t => t.Key == item.ASSOC_TYPE_ID).FirstOrDefault().Value;
                                        for (int j = 0; j < lst_extdatasource.Count; j++)
                                        {
                                            if (lst_extdatasource[j].NAME.ToLower().Trim() == pkValuetext?.ToLower()?.Trim())
                                            {
                                                pk.SelectedIndex = j;
                                                break;
                                            }
                                        }
                                        if (pk.SelectedIndex <= 0)
                                        {
                                            pk.SelectedIndex = 0;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }
                                Mainlayout.Children.Add(pk);
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

                                    if (App.Isonline)
                                    {
                                        await Task.Run(() =>
                                        {
                                            var json = CasesAPIMethods.GetAssocCascadeInfoByCaseType(Casetypeid);
                                            var AssocType = json.GetValue("ResponseContent");
                                            AssocTypeCascades = JsonConvert.DeserializeObject<List<AssocCascadeInfo>>(AssocType.ToString());
                                        });


                                        var assocChild = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == item.ASSOC_TYPE_ID).ToList();
                                        if (assocChild.Count < 1)
                                        {
                                            await Task.Run(() =>
                                            {
                                                var result_extdatasource1 = CasesSyncAPIMethods.GetTypeValuesByAssocCaseType(App.Isonline, Request, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                                result_extdatasource1.Wait();
                                                if (result_extdatasource1.Result.Count() > 0)
                                                {
                                                    lst_SSsource = result_extdatasource1.Result;
                                                }
                                            });
                                        }
                                    }
                                    else
                                    {
                                        await Task.Run(() =>
                                        {
                                            var result_extdatasource1 = CasesSyncAPIMethods.GetTypeValuesByAssocCaseType(App.Isonline, Request, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                            result_extdatasource1.Wait();
                                            if (result_extdatasource1.Result.Count() > 0)
                                            {
                                                lst_SSsource = result_extdatasource1.Result;
                                            }
                                        });
                                    }

                                    pk.ItemsSource = lst_SSsource;
                                    pk.ItemDisplayBinding = new Binding("Name");
                                    pk.SelectedIndex = 0;

                                    try
                                    {
                                        // for Set Status Dropdown
                                        string Name_value = "New";

                                        var records = lst_SSsource.Where(v => v.Name.ToLower().Trim() == Name_value.ToLower().Trim()).FirstOrDefault();
                                        int j = lst_SSsource.FindIndex(v => v.ID == records?.ID);

                                        pk.SelectedIndex = j == -1 ? 0 : j;
                                        if (j > 0)
                                        {
                                            pk.IsEnabled = false;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                catch (Exception ex)
                                {
                                }

                                Mainlayout.Children.Add(pk);
                                break;
                            case "x":
                                BorderEditor entry_notes = new BorderEditor();
                                entry_notes.HeightRequest = 100;
                                entry_notes.BorderWidth = 1;
                                entry_notes.CornerRadius = 5;
                                entry_notes.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;
                                try
                                {
                                    entry_notes.WidthRequest = 200;
                                    entry_notes.FontSize = 16;
                                    entry_notes.Keyboard = Keyboard.Default;
                                    entry_notes.Text = metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value;
                                }
                                catch (Exception ex)
                                {
                                }
                                Mainlayout.Children.Add(entry_notes);
                                break;

                            case "a":
                                #region DatePicker
                                //DatePicker DO = new DatePicker();
                                //DO.Format = "MM/dd/yyyy";
                                //DO.WidthRequest = 200;
                                //DO.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;
                                //try
                                //{
                                //    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(DO.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), DO.StyleId?.Split('_')[0]?.ToUpper() + "_" + DO.StyleId?.Split('_')[1]?.Split('|')[0]);
                                //    if (!string.IsNullOrEmpty(ischeckcalControl))
                                //        DO.DateSelected += DO_DateSelected;

                                //    var metaDatatextValueDate = metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value;

                                //    if (metaDatatextValueDate != null)
                                //    {
                                //        if (!string.IsNullOrEmpty(metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value))
                                //        {
                                //            var Str = App.DateFormatStringToString(metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value);
                                //            DateTime dt = Convert.ToDateTime(Str);
                                //            DO.Date = dt;
                                //        }
                                //    }
                                //}
                                //catch (Exception ex)
                                //{

                                //    // throw;
                                //}


                                //Mainlayout.Children.Add(DO);

                                #endregion

                                #region txt_Date
                                Entry txt_Date = new Entry();
                                txt_Date.Placeholder = "Select Date";
                                txt_Date.WidthRequest = 175;
                                txt_Date.TextColor = Color.Gray;
                                txt_Date.Keyboard = Keyboard.Numeric;
                                txt_Date.StyleId = "a_" + item.ASSOC_TYPE_ID;
                                txt_Date.Text = "";

                                try
                                {
                                    ischeckcalControl = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(txt_Date.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), txt_Date.StyleId?.Split('_')[0]?.ToUpper() + "_" + txt_Date.StyleId?.Split('_')[1]?.Split('|')[0]);
                                }
                                catch (Exception)
                                {
                                }

                                if (!string.IsNullOrEmpty(ischeckcalControl))
                                    txt_Date.TextChanged += DO_TextChanged;

                                #endregion

                                Image img_clr = new Image();
                                img_clr.StyleId = "imgcl_" + item.ASSOC_TYPE_ID;
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
                                date_pick.StyleId = "dt_" + item.ASSOC_TYPE_ID;
                                #endregion

                                Mainlayout.Children.Add(txt_Date);
                                Mainlayout.Children.Add(img_clr);
                                Mainlayout.Children.Add(date_pick);

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

                                var clr_img_click = new TapGestureRecognizer();
                                clr_img_click.Tapped += (s, e) =>
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
                                img_clr.GestureRecognizers.Add(clr_img_click);

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
                                Entry entry_tx = new Entry();
                                entry_tx.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;

                                try
                                {
                                    entry_tx.WidthRequest = 200;
                                    entry_tx.FontSize = 16;
                                    entry_tx.Keyboard = Keyboard.Default;


                                    ischeckcalControl = SetCalControls(sControls?.Where(v => v.AssocTypeID == Convert.ToInt32(entry_tx.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls?.Where(v => v.AssocFieldType == 'C').ToList(), entry_tx.StyleId?.Split('_')[0]?.ToUpper() + "_" + entry_tx.StyleId?.Split('_')[1]?.Split('|')[0]);
                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                        entry_tx.Unfocused += Entry_Unfocused;
                                    entry_tx.Text = metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value;
                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }


                                Mainlayout.Children.Add(entry_tx);

                                break;
                            case "c":
                                Entry entry_cl = new Entry();
                                entry_cl.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;

                                try
                                {
                                    entry_cl.WidthRequest = 200;
                                    entry_cl.IsEnabled = false;
                                    entry_cl.FontSize = 16;
                                    entry_cl.Keyboard = Keyboard.Default;

                                    entry_cl.Text = metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value;
                                }
                                catch (Exception ex)
                                {

                                    //   throw;
                                }

                                Mainlayout.Children.Add(entry_cl);

                                break;
                            case "n":
                                entry_number.StyleId = item.ASSOC_FIELD_TYPE.ToLower() + "_" + item.ASSOC_TYPE_ID;

                                try
                                {
                                    entry_number.WidthRequest = 200;
                                    entry_number.FontSize = 16;
                                    entry_number.Keyboard = Keyboard.Numeric;


                                    ischeckcalControl = SetCalControls(sControls?.Where(v => v.AssocTypeID == Convert.ToInt32(entry_number.StyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls?.Where(v => v.AssocFieldType == 'C').ToList(), entry_number.StyleId?.Split('_')[0]?.ToUpper() + "_" + entry_number.StyleId?.Split('_')[1]?.Split('|')[0]);
                                    if (!string.IsNullOrEmpty(ischeckcalControl))
                                        entry_number.Unfocused += entry_number_Unfocused;
                                    entry_number.Text = metaDatatextValues_fav?.Where(c => c.Key == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.Value;
                                }
                                catch (Exception ex)
                                {

                                    // throw;
                                }

                                Mainlayout.Children.Add(entry_number);

                                break;
                            default:
                                break;
                        }
                        Cases_EntryStack.Children.Add(Mainlayout);
                    }
                    #endregion

                    #region  Create Form Control
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
                    txt_CasNotes.BackgroundColor = Color.Transparent;
                    txt_CasNotes.FontFamily = "Soin Sans Neue";

                    layout14.Children.Add(txt_CasNotes);

                    layout1.Children.Add(Label11);
                    layout.Children.Add(layout1);
                    layout.Children.Add(layout14);

                    Cases_EntryStack.Children.Add(layout);

                    if (metaDatatextValues_fav?.Count > 0)
                    {
                        foreach (var iitem in metadata)
                        {

                            try
                            {
                                switch (iitem.ASSOC_FIELD_TYPE.ToLower())
                                {
                                    case "o":
                                    case "e":


                                        var control = FindPickerControls(iitem.ASSOC_TYPE_ID) as Picker;
                                        if (control != null)
                                        {
                                            List<GetExternalDataSourceByIdResponse.ExternalDatasource> lst_extdatasource = new List<GetExternalDataSourceByIdResponse.ExternalDatasource>();

                                            lst_extdatasource = control.ItemsSource as List<GetExternalDataSourceByIdResponse.ExternalDatasource>;

                                            try
                                            {

                                                int idd = 0;
                                                if (App.Isonline)
                                                {
                                                    if (iitem.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    {
                                                        string exdvalue = Convert.ToString(metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault().Value);
                                                        if (!string.IsNullOrEmpty(exdvalue) && lst_extdatasource.Count > 1)
                                                        {
                                                            idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION.ToLower() == exdvalue.ToLower()).FirstOrDefault());
                                                            if (idd == -1)
                                                            {
                                                                var rec = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                if (rec != null)
                                                                {
                                                                    GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                    {
                                                                        DESCRIPTION = rec.Value.Value,
                                                                        NAME = rec.Value.Value,
                                                                        ID = rec.Value.Key
                                                                    };
                                                                    lst_extdatasource.Add(lst);
                                                                    control.ItemsSource = null;
                                                                    control.ItemsSource = lst_extdatasource;
                                                                    idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == iitem.ASSOC_TYPE_ID));
                                                                }
                                                            }

                                                            control.SelectedIndex = idd;
                                                        }
                                                        else
                                                        {
                                                            var rec = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                            if (rec != null)
                                                            {
                                                                GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                {
                                                                    DESCRIPTION = rec.Value.Value,
                                                                    NAME = rec.Value.Value,
                                                                    ID = rec.Value.Key
                                                                };
                                                                lst_extdatasource.Add(lst);
                                                                control.ItemsSource = null;
                                                                control.ItemsSource = lst_extdatasource;
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == iitem.ASSOC_TYPE_ID));
                                                            }
                                                            control.SelectedIndex = idd;
                                                        }
                                                    }
                                                    else if (iitem.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    {
                                                        string exdvalue = Convert.ToString(metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault().Value);
                                                        if (!string.IsNullOrEmpty(exdvalue) && lst_extdatasource.Count > 1)
                                                        {
                                                            idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.DESCRIPTION.ToLower() == exdvalue.ToLower()).FirstOrDefault());
                                                            control.SelectedIndex = idd;
                                                        }
                                                        else
                                                        {
                                                            var rec = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                            if (rec != null)
                                                            {
                                                                GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                {
                                                                    DESCRIPTION = rec.Value.Value,
                                                                    NAME = rec.Value.Value,
                                                                    ID = rec.Value.Key
                                                                };
                                                                lst_extdatasource.Add(lst);
                                                                control.ItemsSource = null;
                                                                control.ItemsSource = lst_extdatasource;
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == iitem.ASSOC_TYPE_ID));
                                                                control.SelectedIndex = idd;
                                                            }

                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (iitem.ASSOC_FIELD_TYPE.ToLower() == "e")
                                                    {
                                                        var reclst = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                        if (reclst != null)
                                                        {
                                                            string strrec = reclst.Value.Value;
                                                            if (!string.IsNullOrEmpty(strrec))
                                                            {
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.NAME.ToLower().Trim() == strrec.ToLower().Trim()).FirstOrDefault());
                                                                if (idd == -1)
                                                                {
                                                                    var rec1 = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                    if (rec1 != null)
                                                                    {
                                                                        GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                        {
                                                                            DESCRIPTION = rec1.Value.Value,
                                                                            NAME = rec1.Value.Value,
                                                                            ID = rec1.Value.Key
                                                                        };
                                                                        lst_extdatasource.Add(lst);
                                                                        control.ItemsSource = null;
                                                                        control.ItemsSource = lst_extdatasource;
                                                                        idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == iitem.ASSOC_TYPE_ID));

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        control.SelectedIndex = idd;
                                                    }
                                                    else if (iitem.ASSOC_FIELD_TYPE.ToLower() == "o")
                                                    {
                                                        var rec = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                        if (rec != null)
                                                        {
                                                            string strrec = rec.Value.Value;
                                                            if (!string.IsNullOrEmpty(strrec))
                                                            {
                                                                idd = lst_extdatasource.IndexOf(lst_extdatasource.Where(v => v.NAME.ToLower().Trim() == strrec.ToLower().Trim()).FirstOrDefault());
                                                                if (idd == -1)
                                                                {
                                                                    var rec1 = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault();
                                                                    if (rec1 != null)
                                                                    {
                                                                        GetExternalDataSourceByIdResponse.ExternalDatasource lst = new GetExternalDataSourceByIdResponse.ExternalDatasource()
                                                                        {
                                                                            DESCRIPTION = rec1.Value.Value,
                                                                            NAME = rec1.Value.Value,
                                                                            ID = rec1.Value.Key
                                                                        };
                                                                        lst_extdatasource.Add(lst);
                                                                        control.ItemsSource = null;
                                                                        control.ItemsSource = lst_extdatasource;
                                                                        idd = lst_extdatasource.IndexOf(lst_extdatasource.Single(v => v.ID == iitem.ASSOC_TYPE_ID));

                                                                    }
                                                                }
                                                            }
                                                            control.SelectedIndex = idd;
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
                                        control = FindPickerControls(iitem.ASSOC_TYPE_ID) as Picker;
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
                                                        var i = metaDataValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID).FirstOrDefault();

                                                        if (i?.Value?.ToLower() == lst_SSsource[j]?.Name?.ToLower() || i?.Value?.ToLower() == lst_SSsource[j]?.Name?.ToLower())
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
                                        var cnt = FindCasesControls(iitem.ASSOC_TYPE_ID);
                                        if (cnt != null)
                                        {
                                            DatePicker DO = new DatePicker();
                                            DO = cnt as DatePicker;
                                            DO.TextColor = Color.Gray;
                                            //DO.FontSize = 16;
                                            var Str = App.DateFormatStringToString(metaDatatextValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault().Value);
                                            DateTime dt = Convert.ToDateTime(Str);
                                            DO.Date = dt;
                                        }
                                        break;

                                    case "x":
                                        cnt = FindCasesControls(iitem.ASSOC_TYPE_ID);
                                        if (cnt != null)
                                        {
                                            BorderEditor bd = cnt as BorderEditor;
                                            bd.TextColor = Color.LightGray;
                                            bd.FontSize = 16;
                                            bd.Text = metaDatatextValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault().Value;
                                        }
                                        break;

                                    case "t":
                                    case "c":
                                    case "n":
                                        cnt = FindCasesControls(iitem.ASSOC_TYPE_ID);
                                        if (cnt != null)
                                        {
                                            Entry entry = new Entry();
                                            entry = cnt as Entry;
                                            entry.FontSize = 16;
                                            entry.FontFamily = "Soin Sans Neue";
                                            entry.Text = metaDatatextValues_fav?.Where(c => c.Key == iitem.ASSOC_TYPE_ID)?.FirstOrDefault().Value;
                                        }
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    #endregion

                    if (App.Isonline)
                        setStaticCal(ContolrLst);
                }
                else
                {
                    await DisplayAlert("Alert!", "You do not have permission to create this page.", "Ok");
                    await Navigation.PopAsync();
                }
                securityjump:
                int a = 0;

            }
            catch (Exception ex)
            {
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private object FindCasesControls(int AssocID, string Cnt_type = "")
        {
            try
            {
                if (string.IsNullOrEmpty(Cnt_type))
                {
                    foreach (StackLayout infofield in Cases_EntryStack.Children)
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
                }
                else if (Cnt_type == "DatePicker")
                {
                    foreach (StackLayout infofield in Cases_EntryStack.Children)
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
                                        return xy;
                                }
                            }
                        }
                    }
                }
                else if (Cnt_type == "Image")
                {
                    foreach (StackLayout infofield in Cases_EntryStack.Children)
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
                                        return xy;
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

        private void Date_pick_Unfocused(object sender, FocusEventArgs e)
        {
            Date_pick_DateSelected(sender, null);
        }

        private void Date_pick_DateSelected(object sender, DateChangedEventArgs e)
        {
            Entry dtp = new Entry();
            try
            {
                var cnt = (DatePicker)sender;
                var sty_id = cnt.StyleId?.Split('_')[1];
                var dt_Entry = FindCasesControls(Convert.ToInt32(sty_id)) as Entry;
                DateTime dt = cnt.Date;
                dt_Entry.Text = dt.Date.ToString("d");
                // dt_Entry.Unfocus();
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
                Entry en = (Entry)sender;
                if (en.Text.Length > 10)
                    en.Text = e.OldTextValue;

                DyanmicSetCalc(en.StyleId);
            }
            catch (Exception ex)
            {

            }

            try
            {

                //var currText = ((Entry)sender).Text;

                //string oldtext = currText;
                //if (!string.IsNullOrEmpty(currText) && currText.Length >= 8 && currText.Length <= 10)
                //{
                //    oldtext = Convert.ToInt32(currText).ToString("##/##/####");
                //    ((Entry)sender).Text = oldtext;
                //}
                //else if (currText.Length > 10)
                //{
                //    //oldtext = Convert.ToDecimal(currText).ToString("##/##/####");
                //    ((Entry)sender).Text = e.OldTextValue;
                //}
                //DateTime d = new DateTime();
                //DateTime.TryParse(((Entry)sender).Text, out d);
                ////{01/01/01 12:00:00 AM}
            }
            catch (Exception c)
            {
            }
        }

        private void DO_DateSelected(object sender, DateChangedEventArgs e)
        {
            try
            {
                DatePicker en = (DatePicker)sender;
                en.TextColor = Color.Gray;
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
                en.FontFamily = "Soin Sans Neue";
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
                en.FontFamily = "Soin Sans Neue";
                DyanmicSetCalc(en.StyleId);
            }
            catch (Exception ex)
            {
            }
        }

        public string SetCalControls(List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal, string Id)
        {
            try
            {
                calculationsFieldlist = new List<KeyValuePair<List<GetCaseTypesResponse.ItemType>, string>>();
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

        public void setStaticCal(string ContolrLst)
        {
            try
            {
                List<ItemType> sControls = JsonConvert.DeserializeObject<List<ItemType>>(ContolrLst);
                string calc = string.Empty;
                List<ItemType> calControls = new List<ItemType>();
                calControls = sControls.Where(v => v.AssocFieldType == 'C').ToList();

                try
                {
                    List<KeyValuePair<List<ItemType>, string>> lst = new List<KeyValuePair<List<ItemType>, string>>();
                    SetCalControlsForExd(sControls.Where(v => v.AssocFieldType == 'E' || v.AssocFieldType == 'O' || v.AssocFieldType == 'D').ToList(), calControls, "");
                    lst.AddRange(calculationsFieldlist);
                    SetCalControls(sControls.Where(v => v.AssocFieldType != 'E' || v.AssocFieldType != 'O' || v.AssocFieldType != 'D').ToList(), calControls, "");
                    lst.AddRange(calculationsFieldlist);

                    var l = lst.Where(v => v.Value != null).Select(a => a.Value.Replace("assoc_C_", ""))?.Distinct()?.ToList();
                    var r = calControls.Where(v => v.CalculationFormula != null).Select(a => a.AssocTypeID.ToString())?.Distinct()?.ToList();
                    var expectlst = r.Except(l)?.ToList();

                    if (l.Count != r.Count)
                    {
                        foreach (var itm in expectlst)
                        {
                            try
                            {
                                RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();

                                rf.assocFieldCollection = ContolrLst;
                                rf.assocFieldValues = assocFieldValues;
                                rf.assocFieldTexts = assocFieldTexts;
                                rf.calculatedAssocId = "assoc_C_" + itm;
                                rf.sdateFormats = "MM/dd/yyyy";

                                var Result = CasesSyncAPIMethods.CallCalculations(App.Isonline, App.DBPath, rf);
                                Result.Wait();
                                if (!string.IsNullOrEmpty(Result.Result))
                                {
                                    var cnt = FindCasesControls(Convert.ToInt32(itm));
                                    if (cnt != null)
                                    {
                                        Entry entry = new Entry();
                                        entry = cnt as Entry;
                                        entry.Text = Result.Result;
                                    }
                                    calc = "";
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }
                }
                catch
                {
                }
            }
            catch
            {
            }
        }


        public string SetCalControlsForExd(List<GetCaseTypesResponse.ItemType> sControls, List<GetCaseTypesResponse.ItemType> sControlscal, string Id = "", string selectedValue = "")
        {
            calculationsFieldlist = new List<KeyValuePair<List<ItemType>, string>>();
            try
            {
                string sControl = string.Empty;
                KeyValuePair<List<ItemType>, string> Itemcontrols = new KeyValuePair<List<ItemType>, string>(sControls, sControl);
                foreach (var item in sControls)
                {
                    if (item.ExternalDataSourceID != null)
                    {
                        var sxternal = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(item.ExternalDataSourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                        sxternal.Wait();

                        if (sxternal?.Result?.Count > 0)
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

                                                Itemcontrols = new KeyValuePair<List<ItemType>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<ItemType>, string, string>(sControls, sControl, selectedValue));
                                            }
                                            else if (strCalformula.Contains(Convert.ToString(item.Description == null ? "null" : item.Description)) || strCalformula.Contains(Convert.ToString(item.Name == null ? "null" : item.Name)))
                                            {
                                                sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;
                                                Itemcontrols = new KeyValuePair<List<ItemType>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<ItemType>, string, string>(sControls, sControl, selectedValue));
                                            }
                                        }
                                        else
                                        {
                                            if (strCalformula.Contains(Convert.ToString(exd?.Name)) || strCalformula.Contains(Convert.ToString(itm.Description == null ? "null" : itm.Description)))
                                            {
                                                sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;

                                                Itemcontrols = new KeyValuePair<List<ItemType>, string>(sControls, sControl);
                                                calculationsFieldlist.Add(Itemcontrols);
                                                lstcalculationsFieldlist.Add(new Tuple<List<ItemType>, string, string>(sControls, sControl, selectedValue));
                                            }
                                            else if (!string.IsNullOrEmpty(item.SystemCode))
                                            {
                                                if (strCalformula.ToLower().Contains(Convert.ToString(exd?.Name?.ToLower())) || strCalformula.Contains(Convert.ToString(item?.SystemCode)) || strCalformula.ToLower().Contains(Convert.ToString(item.Name?.ToLower() ?? "null")))
                                                {
                                                    sControl = "assoc_" + itm.AssocFieldType + "_" + itm.AssocTypeID;

                                                    Itemcontrols = new KeyValuePair<List<ItemType>, string>(sControls, sControl);
                                                    calculationsFieldlist.Add(Itemcontrols);
                                                    lstcalculationsFieldlist.Add(new Tuple<List<ItemType>, string, string>(sControls, sControl, selectedValue));
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

        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (App.Isonline)
            {
                try
                {

                    Picker Picker = (Picker)sender;
                    var sd = Picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource;
                    int AssocID = int.Parse(Picker.StyleId.Split('_')[1].Split('|')[0].ToString());

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

                                    foreach (StackLayout infofield in Cases_EntryStack.Children)
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

                    ClearChildControl(AssocID, sControls);
                    try
                    {
                        if (Picker.ItemsSource != null)
                        {
                            if (Picker.ItemsSource.Count > 0)
                            {
                                if (Picker.SelectedItem != null)
                                {
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
                catch (Exception ex)
                {
                }
            }
        }

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
                    // var control = FindPickerControlsValues(child._CASE_ASSOC_TYPE_ID_CHILD);
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

                                filterQueryOrg = Result1.Result?.FirstOrDefault()?._FILTER_QUERY; //responsejson[0]._FILTER_QUERY;/*wrong*/
                            }



                            // If assoctype have more than 1 parent

                            var assocParents = AssocTypeCascades.Where(t => t._CASE_ASSOC_TYPE_ID_CHILD == child._CASE_ASSOC_TYPE_ID_CHILD);
                            int parentCount = 1;
                            foreach (var p in assocParents)
                            {
                                string parentSelectedValue = null;

                                parentSelectedValue = Convert.ToString((((Picker)FindPickerControls(p._CASE_ASSOC_TYPE_ID_PARENT)).SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).ID);


                                string parentFieldName = ItemTypes.Where(t => t.AssocTypeID == p._CASE_ASSOC_TYPE_ID_PARENT).FirstOrDefault().Name;

                                var externalDatasourceinfo = CasesSyncAPIMethods.GetExternalDataSourceItemsById(App.Isonline, Convert.ToString(ItemTypes.Where(t => t.AssocTypeID == p._CASE_ASSOC_TYPE_ID_PARENT).FirstOrDefault().ExternalDataSourceID.Value), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Casetypeid);
                                string ParentExternalDatasourceName = externalDatasourceinfo?.Result?.AsQueryable()?.FirstOrDefault()?.Name;


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
            catch (Exception)
            {
            }
        }

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
                // 
            }
        }

        public void DyanmicSetCalc(string CurrentStyleId)
        {
            try
            {
                object cntrl = new object();
                string selectedId = string.Empty;
                string selectedname = string.Empty;
                foreach (var item in sControls.Where(v => v.AssocFieldType != 'E' || v.AssocFieldType != 'O'))
                {
                    foreach (StackLayout infofield in Cases_EntryStack.Children)
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
                                        selectedId = Convert.ToString((picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).ID);
                                        selectedname = Convert.ToString((picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).NAME);
                                        SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.Split('|')[0].ToUpper(), selectedId, selectedname, item.AssocTypeID);
                                    }
                                }
                                else if (ty.Name.ToLower() == "entry")
                                {
                                    var en = (Entry)subitem;
                                    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                    {
                                        if (en.StyleId == CurrentStyleId)
                                        {
                                            SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.Split('|')[0].ToUpper(), Convert.ToString(CurrentStyleId), en.Text, item.AssocTypeID);
                                            cntrl = subitem;
                                        }
                                    }
                                }
                                else if (ty.Name.ToLower() == "datepicker")
                                {
                                    var en = (DatePicker)subitem;
                                    if (en.StyleId != null && !string.IsNullOrEmpty(CurrentStyleId))
                                    {
                                        if (en.StyleId == CurrentStyleId)
                                        {
                                            var sDate = App.DateFormatStringToString(en.Date.ToString());
                                            DateTime dt = Convert.ToDateTime(sDate);
                                            SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.Split('|')[0].ToUpper(), Convert.ToString(CurrentStyleId), dt.Date.ToString("MM/dd/yyyy"), item.AssocTypeID);
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
                    CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0], selectedId + "|" + selectedname);
                }
                else if (x.Name == "Entry")
                {
                    CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
                }
                else if (x.Name == "DatePicker")
                {

                    CalTxtbox = SetCalControls(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControls.Where(v => v.AssocFieldType == 'C').ToList(), CurrentStyleId?.Split('_')[0]?.ToUpper() + "_" + CurrentStyleId?.Split('_')[1]?.Split('|')[0]);
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
                        RefreshCalculationFieldsRequest rf = new RefreshCalculationFieldsRequest();

                        rf.assocFieldCollection = ContolrLst;
                        rf.assocFieldValues = assocFieldValues;
                        rf.assocFieldTexts = assocFieldTexts;
                        rf.calculatedAssocId = sCalId;
                        rf.sdateFormats = "MM/dd/yyyy";

                        //var CalResult = CasesAPIMethods.RefreshCalculationFields(rf);
                        var Result = CasesSyncAPIMethods.CallCalculations(App.Isonline, App.DBPath, rf);
                        Result.Wait();


                        foreach (StackLayout infofield in Cases_EntryStack.Children)
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
            catch (Exception)
            {
            }
        }

        public void DyanmicSetCalcexd(string CurrentStyleId, List<ItemType> sControls, List<ItemType> sControlscal)
        {
            try
            {
                object cntrl = new object();
                string selectedId = string.Empty;
                string selectedname = string.Empty;
                foreach (var item in sControls)
                {
                    foreach (StackLayout infofield in Cases_EntryStack.Children)
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
                                        selectedId = Convert.ToString((picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).ID);
                                        selectedname = Convert.ToString((picker.SelectedItem as GetExternalDataSourceByIdResponse.ExternalDatasource).NAME);
                                        SetDictionary(assocFieldValues, assocFieldTexts, CurrentStyleId.Split('|')[0].ToUpper(), selectedId, selectedname, item.AssocTypeID);
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
                    CalTxtbox = SetCalControlsForExd(sControls.Where(v => v.AssocTypeID == Convert.ToInt32(CurrentStyleId?.Split('_')[1]?.Split('|')[0])).ToList(), sControlscal, "", (selectedId == "-1" ? "0|" + selectedname : selectedId + "|" + selectedname));
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

                        rf.assocFieldCollection = ContolrLst;
                        rf.assocFieldValues = assocFieldValues;
                        rf.assocFieldTexts = assocFieldTexts;
                        rf.calculatedAssocId = sCalId;
                        rf.sdateFormats = "MM/dd/yyyy";

                        //var CalResult = CasesAPIMethods.RefreshCalculationFields(rf);
                        var Result = CasesSyncAPIMethods.CallCalculations(App.Isonline, App.DBPath, rf);

                        Result.Wait();

                        foreach (StackLayout infofield in Cases_EntryStack.Children)
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
            catch (Exception)
            {

                //throw ex;
            }
        }

        public void SetDictionary(Dictionary<string, string> assocFieldValues, Dictionary<string, string> assocFieldTexts, string CurrentStyleId, string selectedId, string selectedname, int AssocTypeID)
        {
            try
            {
                if (!assocFieldValues.ContainsKey("assoc_" + CurrentStyleId.Split('|')[0].ToUpper()))
                {
                    assocFieldValues.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), AssocTypeID + "|" + selectedId);
                    assocFieldTexts.Add("assoc_" + CurrentStyleId.Split('|')[0].ToUpper(), selectedname);
                }
                else
                {
                    assocFieldValues["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = AssocTypeID + "|" + selectedId;
                    assocFieldTexts["assoc_" + CurrentStyleId.Split('|')[0].ToUpper()] = selectedname;
                }
            }
            catch
            {
            }
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            try
            {
                string action = string.Empty;
                string[] buttons;

                if (CasetypeSecurity.ToLower().Equals("open") || CasetypeSecurity.ToLower().Contains("c"))
                {
                    buttons = new string[] { "Create", "Create & Assign", "Create & Exit", "Save Favorite" };
                }
                else if (CasetypeSecurity.ToLower().Equals("open") || CasetypeSecurity.ToLower().Contains("r"))
                {
                    buttons = new string[] { "Save Favorite" };
                }
                else if (CasetypeSecurity.ToLower().Equals("open") || CasetypeSecurity.ToLower().Contains("cr"))
                {
                    buttons = new string[] { "Create", "Create & Assign", "Create & Exit", "Save Favorite" };
                }
                else
                {
                    buttons = new string[] { "Create", "Create & Assign", "Create & Exit", "Save Favorite" };
                }

                action = await this.DisplayActionSheet(null, "Cancel", null, buttons);

                CaseData casedata = new CaseData();
                casedata.CreateBy = Functions.UserName;
                casedata.CaseTypeID = Convert.ToInt32(Casetypeid);

                CreateCaseOptimizedRequest.CreateCaseModelOptimized createcase = new CreateCaseOptimizedRequest.CreateCaseModelOptimized();
                List<CreateCaseOptimizedRequest.TextValue> textValues = new List<CreateCaseOptimizedRequest.TextValue>();
                Dictionary<int, string> metaDataValues = new Dictionary<int, string>();
                List<CreateCaseOptimizedRequest.LinkValue> linkValue = new List<CreateCaseOptimizedRequest.LinkValue>();

                #region  Cases Fill Value
                foreach (StackLayout infofield in Cases_EntryStack.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {
                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name.ToLower() != "stacklayout")
                        {
                            var en = new Entry();
                            var picker = new Picker();
                            var editor = new BorderEditor();
                            var datepicker = new DatePicker();

                            if (ty.Name.ToLower() == "entry")
                            {
                                try
                                {
                                    en = (Entry)xy;
                                    if (en.StyleId.Split('_')[1] == Convert.ToString(metadata.Where(c => c.SYSTEM_CODE == "TITLE").Select(v => v?.ASSOC_TYPE_ID)?.FirstOrDefault()))
                                    {
                                        createcase.caseTitle = en.Text;
                                    }

                                    int Key = int.Parse(en.StyleId.Split('_')[1]?.ToString());
                                    string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                    string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                    if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(en.Text) && action != "Save Favorite")
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");

                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        if (!en.StyleId.Contains("a_"))
                                        {
                                            textValues.Add(new CreateCaseOptimizedRequest.TextValue { Key = Key, Value = en.Text });
                                        }
                                        else
                                        {
                                            var stDate = string.Empty;
                                            if (!string.IsNullOrEmpty(en.Text))
                                            {
                                                var sDate = Convert.ToDateTime(en.Text);
                                                stDate = sDate.Date.ToString("MM/dd/yyyy");
                                            }
                                            textValues.Add(new CreateCaseOptimizedRequest.TextValue { Key = Key, Value = stDate });
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
                                    picker = (Picker)xy;

                                    string styletype = picker.StyleId.Split('_')[0].Split('|')[0].ToString();

                                    if (styletype.ToLower() == "d")
                                    {
                                        int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                        metaDataValues.Add(Key, picker.SelectedItem.ToString());
                                    }
                                    else
                                    {

                                        if ((GetExternalDataSourceByIdResponse.ExternalDatasource)picker.SelectedItem != null)
                                        {
                                            GetExternalDataSourceByIdResponse.ExternalDatasource selectedValue = (GetExternalDataSourceByIdResponse.ExternalDatasource)picker.SelectedItem;

                                            int Key = int.Parse(picker.StyleId.Split('_')[1].Split('|')[0].ToString());
                                            if (picker.StyleId.Split('|')[0] == _TitleFieldControlID)
                                            {
                                                createcase.caseTitle = selectedValue.NAME;
                                            }
                                            else
                                            {
                                                string isReq = metadata.Where(c => c.ASSOC_TYPE_ID == Key).FirstOrDefault().IS_REQUIRED.ToString();
                                                string FiledName = metadata.Where(c => c.ASSOC_TYPE_ID == Key)?.FirstOrDefault()?.NAME?.ToString();
                                                if (!string.IsNullOrEmpty(isReq) && isReq.ToUpper().Trim() == "Y" && string.IsNullOrEmpty(FiledName))
                                                {
                                                    DisplayAlert("Field Required.", "Please enter valid data in " + FiledName, "OK");
                                                    goto requiredJump;
                                                }
                                                metaDataValues.Add(Key, selectedValue.NAME.ToString().Trim());
                                            }
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
                                    editor = (BorderEditor)xy;
                                    if (editor.StyleId == _TitleFieldControlID)
                                    {
                                        createcase.caseTitle = en.Text;
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
                                        textValues.Add(new CreateCaseOptimizedRequest.TextValue { Key = int.Parse(editor.StyleId.Split('_')[1]?.ToString()), Value = editor.Text });
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
                                    //datepicker = (DatePicker)xy;
                                    //if (datepicker.Date != Convert.ToDateTime("01/01/1900"))
                                    //{
                                    //    string str = App.DateFormatStringToString(datepicker.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy");

                                    //    textValues.Add(new CreateCaseOptimizedRequest.TextValue { Key = int.Parse(datepicker.StyleId.Split('_')[1]?.ToString()), Value = str });
                                    //}
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
                #endregion

                createcase.caseType = casedata.CaseTypeID;
                createcase.currentUser = casedata.CreateBy;
                createcase.caseNotes = "";
                createcase.assignTo = "";
                createcase.linkValues = linkValue;
                createcase.textValues = textValues;
                createcase.metaDataValues = metaDataValues;
                createcase.TransactionType = App.Isonline == true ? "M" : "T";
                string CASEID = "";
                int status = -1;
                switch (action)
                {
                    case "Create":

                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        await Task.Run(async () =>
                        {
                            status = await CasesSyncAPIMethods.StoreAndcreateCase(App.Isonline, casedata.CaseTypeID, createcase, txt_CasNotes.Text, Functions.UserName, App.DBPath, null, Functions.UserFullName, "19");
                        });
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        if (status > 0)
                        {
                            CASEID = status.ToString();
                            //App.BackPageNavigation("NewCase", this);
                            //CasesSyncAPIMethods.AddCasesLogs(App.DBPath, 2, "User " + Functions.UserName + " Created The Case " + CASEID + ".", CASEID, Convert.ToString(casedata.CaseTypeID), Functions.UserFullName, "User created this new case", "Created Case", Functions.UserName, "CASEC");

                            await this.Navigation.PushAsync(new ViewCasePage(CASEID, Casetypeid, ""));
                        }
                        else
                        {
                            // DisplayAlert(null, status + "Case Id: " + status.ToString(), "OK");
                        }
                        break;
                    case "Create & Assign":
                        //CasesSyncAPIMethods.AddCasesLogs(App.DBPath, 2, "User " + Functions.UserName + " Created The Case " + CASEID + ".", CASEID, Convert.ToString(casedata.CaseTypeID), Functions.UserFullName, "User created this new case", "Created Case", Functions.UserName, "CASEC");
                        await this.Navigation.PushAsync(new AssignCase(CASEID, Convert.ToString(casedata.CaseTypeID), createcase, txt_CasNotes.Text, Functions.UserName, "C", null, false, false, "", App.Isonline));

                        break;

                    case "Create & Exit":
                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                        await Task.Run(async () =>
                        {

                            status = await CasesSyncAPIMethods.StoreAndcreateCase(App.Isonline, casedata.CaseTypeID, createcase, txt_CasNotes.Text, Functions.UserName, App.DBPath, null, Functions.UserFullName, "19");
                        });
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                        if (status > 0)
                        {
                            // CasesSyncAPIMethods.AddCasesLogs(App.DBPath, 2, "User " + Functions.UserName + " Created The Case " + CASEID + ".", CASEID, Convert.ToString(casedata.CaseTypeID), Functions.UserFullName, "User created this new case", "Created Case", Functions.UserName, "CASEC");
                            //await Navigation.PushAsync(new CaseList("casetypeid", Casetypeid, ""));
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            DisplayAlert(null, status + "Case Id: " + status.ToString(), "OK");
                        }
                        break;

                    case "Save Favorite":

                        var user_dialogs = await UserDialogs.Instance.PromptAsync(new PromptConfig().SetTitle(
                            "Name of Favorite").
                            SetInputMode(InputType.Name).SetOkText("Create"));

                        if (user_dialogs.Ok)
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
                                await Navigation.PushAsync(new SelectCaseType());
                            }
                        }
                        break;
                }
            }
            catch (Exception)
            {
                //   
            }
            requiredJump:
            int abc = 0;
        }

        private object FindControls(string AssocID)
        {
            try
            {

                foreach (StackLayout infofield in Cases_EntryStack.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {
                        var xy = subitem;
                        Type ty = xy.GetType();
                        {

                            if (xy.StyleId.Contains(AssocID.ToLower().Substring(AssocID.ToLower().IndexOf('_') + 1)))
                                return xy;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                // 
                return null;
            }
        }

        private object FindPickerControls(int AssocID)
        {
            try
            {

                foreach (StackLayout infofield in Cases_EntryStack.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {
                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name.ToLower() == "picker")
                        {
                            Picker Picker = new Picker();
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

        private List<GetCaseTypesRequest.ParentSelectedValues> FindPickerControlsValues(int AssocID)
        {
            try
            {

                List<GetCaseTypesRequest.ParentSelectedValues> lsPickerWithValues = new List<GetCaseTypesRequest.ParentSelectedValues>();
                foreach (StackLayout infofield in Cases_EntryStack.Children)
                {
                    foreach (var subitem in infofield.Children)
                    {
                        var xy = subitem;
                        Type ty = xy.GetType();
                        if (ty.Name.ToLower() == "picker")
                        {
                            Picker Picker = new Picker();
                            Picker = (Picker)xy;
                            if (Picker.StyleId.Split('|')[0] != "d_" + AssocID)
                            {
                                //lsPickerWithValues
                                GetExternalDataSourceByIdResponse.ExternalDatasource selectedValue = (GetExternalDataSourceByIdResponse.ExternalDatasource)Picker.SelectedItem;
                                lsPickerWithValues.Add(new GetCaseTypesRequest.ParentSelectedValues { ParentAssocTypeId = int.Parse(Picker.StyleId.Split('|')[0].Split('_')[1]), ParentSelectedExternalDatasourceObjectId = Convert.ToString(selectedValue?.ID) });
                                return lsPickerWithValues;
                            }
                        }
                    }
                }
                return lsPickerWithValues;
            }
            catch (Exception ex)
            {

                // 
                return new List<GetCaseTypesRequest.ParentSelectedValues>();
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
                //
                return null;
            }
        }

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

        private void btn_more_Clicked(object sender, EventArgs e)
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
