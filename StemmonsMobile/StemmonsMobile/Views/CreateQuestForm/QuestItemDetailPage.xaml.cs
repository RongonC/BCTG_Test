using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using static StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse;
using StemmonsMobile.DataTypes.DataType;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuestItemDetailPage : ContentPage
    {
        ToolbarItem Save;
        string itemid, Iteminstancetranid, SecurityArea, SecurityItem, SecurityTran;
        int ViewCount;
        string clickedCategoryId = string.Empty;
        double tottalpoints, totalearnedpoints, totalpercentage = 0.0;
        List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData> ControlSchema = new List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>();
        string scatid = string.Empty;
        bool _isEditable;
        List<string> CategoryIdList = new List<string>();

        List<Add_Questions_MetadataRequest> CategoryObjectlist = new List<Add_Questions_MetadataRequest>();
        List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> lst = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();

        //ArpanB Start
        public static ObservableCollection<string> Exditems { get; set; }
        public Dictionary<int, List<ExternalDatasourceValue>> lstexnaldatasouce = new Dictionary<int, List<ExternalDatasourceValue>>();
        List<ExternalDatasourceValue> lstexternaldatasource = new List<ExternalDatasourceValue>();
        int iSelectedItemlookupId = 0;
        ListView lstView = new ListView();


        ExternalDatasourceValue ExdDefaultValue = new ExternalDatasourceValue
        {
            ID = 0,
            NAME = "-- Select Item --",
            DESCRIPTION = "-- Select Item --"
        };
        //ArpanB End

        public QuestItemDetailPage(string itemId, string ItemInstanceTranID, string security_area, string security_item, string security_tran, bool isEditable, string CatId = "")
        {
            try
            {
                InitializeComponent();

                Save = new ToolbarItem
                {
                    Order = ToolbarItemOrder.Primary,
                    Text = "Save",
                };

                Iteminstancetranid = ItemInstanceTranID;
                SecurityArea = security_area;
                SecurityItem = security_item;
                SecurityTran = security_tran;
                tottalpoints = 0;
                totalearnedpoints = 0;
                totalpercentage = 0;
                ViewCount = 0;
                _isEditable = isEditable;
                itemid = itemId;
                ControlSchema = new List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>();
                scatid = CatId;

                CategoryIdList = new List<string>();
                CategoryObjectlist = new List<Add_Questions_MetadataRequest>();
            }
            catch (Exception ex)
            {
            }
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

            if (_isEditable)
            {
                ToolbarItems.Clear();
                Save.Clicked += SaveClicked;
                ToolbarItems.Add(Save);
            }
            else
            {
                if (ToolbarItems.Count > 0)
                {
                    ToolbarItems.Remove(Save);
                }
            }
            if (!string.IsNullOrEmpty(clickedCategoryId) && Functions.questObjectData != null)
            {

                if (CategoryIdList.Count > 0)
                {
                    if (CategoryIdList.Contains(clickedCategoryId))
                    {
                        int indexofcategory = (CategoryIdList.IndexOf(clickedCategoryId));
                        CategoryObjectlist[indexofcategory] = Functions.questObjectData;

                    }
                    else
                    {
                        CategoryIdList.Add(clickedCategoryId);
                        CategoryObjectlist.Add(Functions.questObjectData);
                    }
                }
                else
                {
                    CategoryIdList.Add(clickedCategoryId);
                    CategoryObjectlist.Add(Functions.questObjectData);
                }


            }
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            masterGrid.Opacity = 0;

            try
            {
                if ((SecurityArea.ToUpper().Contains("R") || SecurityArea.ToUpper().Contains("U") || SecurityArea.ToUpper().Contains("D") || SecurityArea.ToUpper().Contains("OPEN") || string.IsNullOrEmpty(SecurityArea))
                       && (SecurityItem.ToUpper().Contains("R") || SecurityItem.ToUpper().Contains("U") || SecurityItem.ToUpper().Contains("D") || SecurityItem.ToUpper().Contains("OPEN") || string.IsNullOrEmpty(SecurityItem))
                       && (SecurityTran.ToUpper().Contains("R") || SecurityTran.ToUpper().Contains("U") || SecurityTran.ToUpper().Contains("D") || SecurityTran.ToUpper().Contains("OPEN") || string.IsNullOrEmpty(SecurityTran)))
                {
                    if (ViewCount == 0)
                    {
                        if (!App.Isonline)
                        {
                            var Appinfo = DBHelper.GetAppTypeInfoListByTypeIdTransTypeSyscode(ConstantsSync.QuestInstance, Convert.ToInt32(itemid), App.DBPath, "H1_H2_H3_QUEST_AREA_FORM", "M");
                            Appinfo.Wait();
                            ControlSchema = new List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>();

                            ControlSchema = JsonConvert.DeserializeObject<List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>>(Appinfo.Result[0].ASSOC_FIELD_INFO);

                            ControlSchema = ControlSchema.Select(v =>
                            {
                                v.blnIsEdit = (v.strIsActive == "Y" ? true : false);
                                v.strFieldType = (v.strFieldType ?? "");
                                return v;
                            }).ToList();
                        }
                        else
                        {
                            List<ItemInfoField> lstQuestFormFields = new List<ItemInfoField>();

                            await Task.Run(() =>
                            {
                                var result = QuestSyncAPIMethods.AssignControlsAsync(App.Isonline, Convert.ToString(itemid), Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                                lstQuestFormFields = result.Result;

                            });
                            foreach (var itm in lstQuestFormFields)
                            {
                                GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData record = new GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData();
                                record.blnHidePoints = itm.blnHidePoints;
                                record.blnIsEdit = itm.strIsActive.ToUpper() == "Y" ? true : false;
                                record.blnSuppressAlert = itm.blnSuppressAlert;
                                record.FIELD_SECURITY = itm.FIELD_SECURITY;
                                record.intAreaID = itm.intItemID;
                                record.intExternalDatasourceID = itm.intExternalDatasourceID;
                                record.intItemInfoFieldID = itm.intItemInfoFieldID;
                                record.strAreaName = itm.strItemName;
                                record._IS_REQUIRED = itm.strIsRequired;
                                record.intListID = Convert.ToInt32(itm.intDisplayOrder);
                                record.strDisplayName = itm.strDisplayName;
                                record.strItemInfoFieldName = itm.strItemInfoFieldDesc;//strItemInfoFieldName
                                record.strExternalDataSourceName = itm.strExternalDatasourceName;
                                record.strFieldType = itm.strFieldType;
                                record.strSupervisorEmail = itm.strSupervisorEmail;
                                record.strExtraField1 = itm.strExtraField1;
                                record.strDisplayName = itm.strDisplayName;

                                ControlSchema.Add(record);
                            }

                        }

                        ViewCount = ViewCount + 1;
                        TextFieldsLayout.Children.Clear();

                        List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData> ControlsValues = new List<GetItemInfoFieldMetaDataResponse.ItemInfoFieldMetaData>();

                        try
                        {
                            await Task.Run(() =>
                            {
                                var result = QuestSyncAPIMethods.GetItemInfoFieldMetaData(App.Isonline, Convert.ToString(Iteminstancetranid), "", "", ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid, Functions.UserName, Convert.ToInt32(scatid));
                                result.Wait();
                                ControlsValues = result.Result;


                                var getInfoDepeResult = QuestSyncAPIMethods.GetItemInfoDependency(App.Isonline, "0", itemid.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Iteminstancetranid);
                                lstItemInfoDependancy = getInfoDepeResult.Result;

                                var apicall = QuestSyncAPIMethods.GetItemQuestionMetaData(App.Isonline, Convert.ToString(Iteminstancetranid), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid, "", scatid);

                                lst = apicall?.Result;
                            });
                        }
                        catch (Exception ex) { }

                        try
                        {
                            for (int i = 0; i < ControlSchema?.Count(); i++)
                            {
                                var layout = new StackLayout();
                                layout.Orientation = StackOrientation.Horizontal;

                                var layout1 = new StackLayout();
                                layout1.HorizontalOptions = LayoutOptions.Start;
                                layout1.WidthRequest = 200;

                                var Label1 = new Label
                                { VerticalOptions = LayoutOptions.Start };

                                FormattedString frmtText = new FormattedString();
                                frmtText.Spans.Add(new Span { Text = ControlSchema[i].strItemInfoFieldName + ":" });

                                if (ControlSchema[i]._IS_REQUIRED.ToLower() == "y")
                                    frmtText.Spans.Add(new Span { Text = " *", ForegroundColor = Color.Red });

                                Label1.FormattedText = frmtText;

                                //Label1.Text = ControlSchema[i].strItemInfoFieldName;
                                Label1.HorizontalOptions = LayoutOptions.Start;
                                Label1.FontSize = 16;

                                layout1.Children.Add(Label1);
                                layout.Children.Add(layout1);

                                var Rightlayout = new StackLayout();
                                Rightlayout.HorizontalOptions = LayoutOptions.Start;

                                Entry ST = new Entry();
                                ST.WidthRequest = 200;
                                ST.FontSize = 16;
                                ST.HorizontalOptions = LayoutOptions.Start;
                                ST.Keyboard = Keyboard.Default;

                                Entry HL = new Entry();
                                HL.WidthRequest = 200;
                                HL.FontSize = 16;
                                HL.FontFamily = "Soin Sans Neue";
                                HL.Keyboard = Keyboard.Default;

                                Entry MN = new Entry();
                                MN.WidthRequest = 200;
                                MN.FontSize = 16;
                                MN.Keyboard = Keyboard.Numeric;
                                MN.HorizontalOptions = LayoutOptions.Start;

                                Entry SN = new Entry();
                                SN.WidthRequest = 200;
                                SN.FontSize = 16;
                                SN.Keyboard = Keyboard.Numeric;
                                SN.HorizontalOptions = LayoutOptions.Start;

                                BorderEditor TA = new BorderEditor();
                                TA.HeightRequest = 100;
                                TA.FontSize = 16;
                                TA.BorderColor = Color.LightGray;
                                TA.BorderWidth = 1;
                                TA.CornerRadius = 5;
                                TA.WidthRequest = 200;
                                TA.Keyboard = Keyboard.Default;

                                Entry CL = new Entry();
                                CL.WidthRequest = 150;
                                CL.FontSize = 16;
                                CL.IsEnabled = false;
                                CL.HorizontalOptions = LayoutOptions.Start;

                                switch (ControlSchema[i].strFieldType.ToLower())
                                {
                                    #region SE -- EL -- ME
                                    case "se":
                                    case "el":
                                    case "me":
                                        //Picker pk = new Picker();
                                        //pk.HorizontalOptions = LayoutOptions.Start;
                                        //pk.WidthRequest = 200;
                                        //pk.TextColor = Color.Gray;
                                        //pk.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                        //Rightlayout.Children.Add(pk);
                                        //pk.SelectedIndexChanged += Pk_SelectedIndexChanged;
                                        //pk.SelectedIndex = 0;
                                        //try
                                        //{
                                        //    List<GetExternalDatasourceByIDResponse.ExternalDataSource> view = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
                                        //    GetExternalDatasourceByIDResponse.ExternalDataSource dt = new GetExternalDatasourceByIDResponse.ExternalDataSource();

                                        //    dt.intExternalDataSourceID = -1;
                                        //    dt.strName = "-- Select Item --";
                                        //    dt.strObjectDescription = "-- Select Item --";


                                        //    if (App.Isonline)
                                        //    {
                                        //        List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == ControlSchema[i].intItemInfoFieldID).ToList();

                                        //        if (infoFieldChild.Count < 1)
                                        //        {
                                        //            var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(ControlSchema[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid);
                                        //            view = ExternalCall.Result;

                                        //        }
                                        //    }
                                        //    else
                                        //    {
                                        //        var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(ControlSchema[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid);
                                        //        view = ExternalCall.Result;
                                        //    }


                                        //    view.Insert(0, dt);
                                        //    pk.ItemsSource = view;
                                        //    pk.ItemDisplayBinding = new Binding("strName");
                                        //    if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                        //    {
                                        //        pk.IsEnabled = false;
                                        //    }

                                        //}
                                        //catch (Exception ex)
                                        //{

                                        //}

                                        Picker pk = new Picker();
                                        pk.WidthRequest = 200;

                                        pk.IsVisible = false;
                                        pk.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;

                                        List<ExternalDatasourceValue> view = new List<ExternalDatasourceValue>();
                                        view.Insert(0, ExdDefaultValue);

                                        pk.ItemsSource = view;
                                        pk.SelectedIndex = 0;
                                        //pk.ItemDisplayBinding = new Binding("strName");

                                        try
                                        {
                                            //GetExternalDatasourceByIDResponse.ExternalDataSource dt = new GetExternalDatasourceByIDResponse.ExternalDataSource();

                                            //dt.intExternalDataSourceID = -1;
                                            //dt.strName = "-- Select Item --";
                                            //dt.strObjectDescription = "-- Select Item --";


                                            //if (App.Isonline)
                                            //{
                                            //    List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == ControlSchema[i].intItemInfoFieldID).ToList();

                                            //    if (infoFieldChild.Count < 1)
                                            //    {
                                            //        var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(ControlSchema[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid);
                                            //        view = ExternalCall.Result;

                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(ControlSchema[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid);
                                            //    view = ExternalCall.Result;
                                            //}
                                        }
                                        catch (Exception ex)
                                        {

                                        }

                                        Button itm_look_button = new Button();
                                        itm_look_button.WidthRequest = 200;
                                        itm_look_button.TextColor = Color.Gray;
                                        itm_look_button.BackgroundColor = Color.White;
                                        if (Device.RuntimePlatform == "Android")
                                        {
                                            itm_look_button.Margin = new Thickness(0, 0, 0, 1);
                                            itm_look_button.CornerRadius = 0;
                                        }
                                        if (Device.RuntimePlatform == "iOS")
                                        {
                                            itm_look_button.BorderWidth = 1;
                                            itm_look_button.CornerRadius = 5;
                                            itm_look_button.BorderColor = Color.Gray;
                                            itm_look_button.CornerRadius = 5;
                                        }

                                        itm_look_button.IsVisible = true;
                                        itm_look_button.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                        itm_look_button.Clicked += itm_look_button_Clicked;
                                        itm_look_button.Text = "-- Select Item --";
                                        itm_look_button.WidthRequest = 200;

                                        List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == ControlSchema[i].intItemInfoFieldID).ToList();

                                        if (infoFieldChild.Count >= 1)
                                            itm_look_button.IsEnabled = false;


                                        if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                        {
                                            pk.IsEnabled = false;
                                            itm_look_button.IsEnabled = false;
                                        }

                                        Rightlayout.Children.Add(pk);
                                        Rightlayout.Children.Add(itm_look_button);
                                        Rightlayout.BackgroundColor = Color.Gray;

                                        break;

                                    #endregion

                                    #region DO -- DT
                                    case "do":
                                    case "dt":
                                        #region OLD
                                        //DO.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                        //try
                                        //{
                                        //    if (ControlsValues[i].strItemInfoFieldValue != "")
                                        //    {
                                        //        string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                        //        string Date = CommonConstants.DateFormatStringToString(ControlsValues[i].strItemInfoFieldValue, "MM/dd/yyyy", "yyyy/MM/dd");
                                        //        DateTime sd = Convert.ToDateTime(Date);
                                        //        DO.Date = Convert.ToDateTime(Date);
                                        //    }
                                        //    if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                        //    {
                                        //        DO.IsEnabled = false;
                                        //    }
                                        //}
                                        //catch (Exception ex)
                                        //{

                                        //}
                                        //Rightlayout.Children.Add(DO);

                                        #endregion

                                        #region txt_Date
                                        Entry txt_Date = new Entry();
                                        txt_Date.Placeholder = "Select Date";
                                        txt_Date.WidthRequest = 170;
                                        txt_Date.TextColor = Color.Gray;
                                        txt_Date.Keyboard = Keyboard.Numeric;

                                        txt_Date.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;

                                        #endregion

                                        Image im = new Image();
                                        im.StyleId = "imgdo_" + ControlSchema[i].intItemInfoFieldID;
                                        im.Source = ImageSource.FromFile("Assets/erase16.png");
                                        im.HeightRequest = 25;
                                        im.WidthRequest = 25;

                                        #region date_pick
                                        DatePicker date_pick = new DatePicker();
                                        date_pick.IsVisible = false;
                                        date_pick.Format = "MM/dd/yyyy";
                                        date_pick.WidthRequest = 200;
                                        date_pick.TextColor = Color.Gray;
                                        date_pick.StyleId = "do_" + ControlSchema[i].intItemInfoFieldID;
                                        #endregion

                                        Rightlayout.Orientation = StackOrientation.Horizontal;
                                        Rightlayout.Children.Add(txt_Date);
                                        Rightlayout.Children.Add(im);
                                        Rightlayout.Children.Add(date_pick);

                                        //if (ControlsValues[i].strItemInfoFieldValue != "")
                                        //{
                                        //    string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                        //    string Date = CommonConstants.DateFormatStringToString(ControlsValues[i - 1].strItemInfoFieldValue, "MM/dd/yyyy", "yyyy/MM/dd");
                                        //    DateTime sd = Convert.ToDateTime(Date);
                                        //    var dt_c = FindQuestControl(Convert.ToString(ControlsValues[i - 1].intItemInfoFieldID), "DatePicker") as DatePicker;
                                        //    if (!String.IsNullOrEmpty(Date))
                                        //    {
                                        //        var Str = CommonConstants.DateFormatStringToString(Date);
                                        //        txt_Date.Text = Str;

                                        //        Device.BeginInvokeOnMainThread(() =>
                                        //        {
                                        //            dt_c.Date = Convert.ToDateTime(Date);
                                        //        });
                                        //    }
                                        //    //DO.Date = Convert.ToDateTime(Date);
                                        //}

                                        txt_Date.Focused += (sender, e) =>
                                        {
                                            try
                                            {
                                                var cnt = (Entry)sender;
                                                var sty_id = cnt.StyleId?.Split('_')[1];
                                                var dt_c = FindQuestControl(sty_id, "datepicker") as DatePicker;
                                                cnt.Unfocus();
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

                                        var tgr = new TapGestureRecognizer();
                                        tgr.Tapped += async (s, e) =>
                                        {
                                            Entry C_ent = new Entry();
                                            try
                                            {
                                                var ct = (Image)s;
                                                var sty_id = ct.StyleId?.Split('_')[1];
                                                C_ent = FindQuestControl(Convert.ToString(sty_id)) as Entry;
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
                                            date_pick.Unfocused += Date_pick_Unfocused;
                                        }
                                        else
                                        {
                                            date_pick.DateSelected += Date_pick_DateSelected;
                                        }

                                        break;
                                    #endregion

                                    #region ET
                                    case "et":
                                        TA.Text = ControlsValues[i].strItemInfoFieldValue == ",0" ? "" : ControlsValues[i].strItemInfoFieldValue;
                                        try
                                        {
                                            TA.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                            if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                            {
                                                TA.IsEnabled = false;
                                            }
                                        }
                                        catch (Exception ex) { }

                                        Rightlayout.Children.Add(TA);
                                        break;

                                    #endregion

                                    #region ST -- HL
                                    case "st":
                                    case "hl":
                                        if (string.IsNullOrEmpty(ControlsValues[i].strItemInfoFieldValue))
                                        {
                                            ST.Text = " ";
                                        }
                                        else
                                        {
                                            ST.Text = ControlsValues[i].strItemInfoFieldValue;
                                        }

                                        ST.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                        if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                        {
                                            ST.IsEnabled = false;
                                        }

                                        Rightlayout.Children.Add(ST);
                                        break;
                                    #endregion

                                    #region MN -- SN
                                    case "mn":
                                    case "sn":
                                        MN.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                        try
                                        {
                                            MN.Text = ControlsValues[i].strItemInfoFieldValue;
                                            if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                            {
                                                MN.IsEnabled = false;
                                            }
                                        }
                                        catch (Exception ex) { }

                                        Rightlayout.Children.Add(MN);
                                        break;
                                    #endregion

                                    #region TA
                                    case "ta":
                                        TA.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;
                                        try
                                        {
                                            if (ControlsValues[i].strItemInfoFieldValue != "")
                                            {
                                                TA.Text = ControlsValues[i].strItemInfoFieldValue;
                                            }

                                            if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                            {
                                                TA.IsEnabled = false;
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        Rightlayout.Children.Add(TA);
                                        break;
                                    #endregion

                                    #region CL
                                    case "cl":
                                        CL.StyleId = ControlSchema[i].strFieldType.ToLower() + "_" + ControlSchema[i].intItemInfoFieldID;

                                        try
                                        {
                                            CL.Text = ControlsValues[i].strItemInfoFieldValue;

                                            if (!(bool)ControlSchema[i].blnIsEdit && (ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("R") || !ControlSchema[i].FIELD_SECURITY.ToUpper().Contains("U")))
                                            {
                                                CL.IsEnabled = false;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        Rightlayout.Children.Add(CL);
                                        break;
                                    #endregion

                                    default:
                                        break;
                                }
                                layout.Children.Add(Rightlayout);
                                TextFieldsLayout.Children.Add(layout);
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                        try
                        {
                            for (int i = 0; i < ControlsValues.Count(); i++)
                            {
                                switch (ControlsValues[i]?.strFieldType.ToLower())
                                {
                                    case "se":
                                    case "el":
                                    case "me":

                                        //Picker Pick = FindQuestControl(Convert.ToString(ControlsValues[i]?.intItemInfoFieldID)) as Picker;
                                        //var view = Pick.ItemsSource as List<GetExternalDatasourceByIDResponse.ExternalDataSource>;


                                        //int index = 0;
                                        //if (view != null)
                                        //{
                                        //    if (App.Isonline)
                                        //    {
                                        //        var records = view.Where(v => v.strObjectID == ControlsValues[i].strExternalDatasourceObjectID).FirstOrDefault();
                                        //        index = view.FindIndex(v => v.strObjectID == records?.strObjectID);
                                        //    }
                                        //    else
                                        //    {
                                        //        string str = ControlsValues[i].strItemInfoFieldValue;

                                        //        var rec = view.Where(v => v.strName.ToLower().Trim() == ControlSchema[i].strItemInfoFieldValue.ToLower().Trim()).FirstOrDefault();
                                        //        if (rec != null)
                                        //        {
                                        //            index = view.FindIndex(v => v.strObjectID == rec?.strObjectID);
                                        //        }
                                        //        if (rec == null)
                                        //        {
                                        //            GetExternalDatasourceByIDResponse.ExternalDataSource templst = new GetExternalDatasourceByIDResponse.ExternalDataSource()
                                        //            {
                                        //                dtCreatedDataTime = Convert.ToDateTime(ControlsValues[i].dtFormCreatedDatetime),
                                        //                strDescription = ControlsValues[i].strDisplayName,
                                        //                strObjectID = Convert.ToString(ControlsValues[i].strExternalDatasourceObjectID),
                                        //                strName = ControlsValues[i].strItemInfoFieldValue,


                                        //            };
                                        //            List<GetExternalDatasourceByIDResponse.ExternalDataSource> lst = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
                                        //            lst.Add(templst);
                                        //            view.AddRange(lst);
                                        //            index = view.FindIndex(v => v.strName.ToLower().Trim() == ControlsValues[i].strItemInfoFieldValue.ToLower().Trim());
                                        //            Pick.ItemsSource = null;
                                        //            Pick.ItemsSource = view;
                                        //        }

                                        //    }
                                        //    Pick.SelectedIndex = index;
                                        //}

                                        Picker Pick = FindQuestControl(Convert.ToString(ControlsValues[i]?.intItemInfoFieldID), "picker") as Picker;
                                        Button btn_pik = FindQuestControl(Convert.ToString(ControlsValues[i]?.intItemInfoFieldID), "button") as Button;

                                        List<ExternalDatasourceValue> view = Pick.ItemsSource as List<ExternalDatasourceValue>;

                                        view.Add(new ExternalDatasourceValue()
                                        {
                                            ID = Convert.ToInt32(ControlsValues[i].strExternalDatasourceObjectID),
                                            NAME = ControlsValues[i].strItemInfoFieldValue.ToString(),
                                            DESCRIPTION = ControlsValues[i].strItemInfoFieldValue.ToString()
                                        });

                                        Pick.ItemsSource = null;
                                        Pick.ItemsSource = view;
                                        if (!string.IsNullOrEmpty(ControlsValues[i].strItemInfoFieldValue))
                                        {
                                            btn_pik.Text = ControlsValues[i].strItemInfoFieldValue.ToString();
                                            Pick.SelectedIndex = 1;
                                            btn_pik.IsEnabled = true;
                                        }
                                        else
                                        {
                                            Pick.SelectedIndex = 0;
                                        }

                                        break;

                                    case "do":
                                    case "dt":
                                        try
                                        {
                                            var cnt = FindQuestControl(Convert.ToString(ControlsValues[i].intItemInfoFieldID));
                                            if (cnt != null)
                                            {
                                                Entry DO = new Entry();
                                                DO = cnt as Entry;
                                                if (ControlsValues[i].strItemInfoFieldValue != "")
                                                {

                                                    DO.Text = ControlsValues[i].strItemInfoFieldValue;

                                                    //string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                                                    //string Date = CommonConstants.DateFormatStringToString(ControlSchema[i].strItemInfoFieldValue, "MM/dd/yyyy", "yyyy/MM/dd");
                                                    //DateTime sd = Convert.ToDateTime(Date);
                                                    //var dt_c = FindQuestControl(Convert.ToString(ControlsValues[i].intItemInfoFieldID), "DatePicker") as DatePicker;
                                                    //if (!String.IsNullOrEmpty(Date))
                                                    //{
                                                    //    var Str = CommonConstants.DateFormatStringToString(Date);
                                                    //    DO.Text = Str;

                                                    //    Device.BeginInvokeOnMainThread(() =>
                                                    //    {
                                                    //        dt_c.Date = Convert.ToDateTime(Date);
                                                    //    });
                                                    //}
                                                    //DO.Date = Convert.ToDateTime(Date);
                                                }
                                                //string Dateval = (ControlSchema?.Where(c => c.intItemInfoFieldID == ControlsValues[i].intItemInfoFieldID)?.FirstOrDefault()?.strItemInfoFieldValue);
                                                //var dt_c = FindQuestControl(Convert.ToString(ControlsValues[i].intItemInfoFieldID), "DatePicker") as DatePicker;



                                                //DatePicker DO = new DatePicker();
                                                //DO = cnt as DatePicker;
                                                //DO.TextColor = Color.Gray;
                                                //string Dateval = (metadatacollection?.Where(c => c.AssociatedTypeID == item.ASSOC_TYPE_ID)?.FirstOrDefault()?.TextValue);
                                                //if (!String.IsNullOrEmpty(Dateval))
                                                //{
                                                //    var Str = CommonConstants.DateFormatStringToString(Dateval);
                                                //    DateTime dt = Convert.ToDateTime(Str);
                                                //    DO.Date = dt;
                                                //}
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                        break;
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }

                    if (lst.Count > 0)
                    {
                        var sd = lst?.Select(o => new { o.intItemCategoryID, o.strItemCategoryName })?.Distinct()?.ToList();


                        Dictionary<int, double> lstCategoryPointsEarned = new Dictionary<int, double>();
                        Dictionary<int, double> lstCategoryPointsAvail = new Dictionary<int, double>();
                        Dictionary<int, double> lstCategoryPointsScore = new Dictionary<int, double>();
                        Dictionary<int, double> lstMetadatid = new Dictionary<int, double>();
                        foreach (var category in sd)
                        {
                            //get sum of category ID on question
                            var QuestionObject = lst.Where(t => t.intItemCategoryID == category.intItemCategoryID).ToList();
                            var QuestionPointsEarned = 0.00;
                            var QuestionPointsAvail = 0.00;
                            var QuestionPointScore = 0.00;
                            var metadataid = "";

                            foreach (var questionScore in QuestionObject)
                            {
                                var earned = questionScore.strPointsEarned;
                                QuestionPointsEarned += Convert.ToDouble(earned);

                                var pointsAvail = questionScore.strPointsAvailable;
                                QuestionPointsAvail += Convert.ToDouble(pointsAvail);

                                var pointsScore = questionScore.Score;
                                QuestionPointScore += Convert.ToDouble(pointsScore);

                                metadataid = Convert.ToString(questionScore.intItemQuestionMetadataID);
                            }

                            lstCategoryPointsEarned.Add((category.intItemCategoryID), QuestionPointsEarned);
                            lstCategoryPointsAvail.Add((category.intItemCategoryID), QuestionPointsAvail);
                            lstCategoryPointsScore.Add((category.intItemCategoryID), QuestionPointScore);
                            lstMetadatid.Add((category.intItemCategoryID), Convert.ToDouble(metadataid));

                        }


                        List<QuestionDisplay> Questionlst = new List<QuestionDisplay>();
                        tottalpoints = 0.0;
                        totalpercentage = 0.0;
                        totalearnedpoints = 0.0;

                        for (int i = 0; i < sd.Count(); i++)
                        {

                            string score = (lstCategoryPointsEarned[Convert.ToInt32(sd[i].intItemCategoryID)] / lstCategoryPointsAvail[Convert.ToInt32(sd[i].intItemCategoryID)] * 100).ToString();
                            Double D1 = Convert.ToDouble(score == "NaN" ? "0" : score);
                            Double D2 = Math.Round(D1, 2);
                            string FinalScore = Convert.ToString(D2);
                            if (lstCategoryPointsEarned[Convert.ToInt32(sd[i].intItemCategoryID)] == 0 && lstCategoryPointsAvail[Convert.ToInt32(sd[i].intItemCategoryID)] == 0)
                            {
                                FinalScore = "0";
                            }

                            var avc = new QuestionDisplay(Convert.ToString(sd[i].intItemCategoryID), lstMetadatid[Convert.ToInt32(sd[i].intItemCategoryID)].ToString(), sd[i].strItemCategoryName, lstCategoryPointsAvail[Convert.ToInt32(sd[i].intItemCategoryID)].ToString(), lstCategoryPointsEarned[Convert.ToInt32(sd[i].intItemCategoryID)].ToString(), FinalScore);
                            Questionlst.Add(avc);
                        }

                        listdata.ItemsSource = Questionlst;

                        for (int i = 0; i < lstCategoryPointsAvail.Count; i++)
                        {
                            tottalpoints += lstCategoryPointsAvail[Convert.ToInt32(sd[i].intItemCategoryID)];
                            totalearnedpoints += lstCategoryPointsEarned[Convert.ToInt32(sd[i].intItemCategoryID)];
                        }

                        totalpercentage = (totalearnedpoints == 0 && tottalpoints == 0) ? 0 : totalearnedpoints / tottalpoints * 100;

                        totalpercentage = Math.Truncate(totalpercentage);
                        tottalpoints = Math.Truncate(tottalpoints);
                        totalearnedpoints = Math.Truncate(totalearnedpoints);

                        OverallPoints.Text = "Overall Points Available:" + tottalpoints;
                        EarnedPoints.Text = "Earned:" + totalearnedpoints;
                        TotalPercentage.Text = "Score:" + totalpercentage + "%";
                        statkicFooter.IsVisible = true;


                        int heightRowList = 90;
                        int iq = (Questionlst.Count * heightRowList);
                        listdata.HeightRequest = iq;

                    }
                    else
                    {
                        if (!App.Isonline)
                        {
                            TextFieldsLayout.Children.Clear();
                            await DisplayAlert("Alert", "Please Go Online To View Form!", "Ok");
                            await this.Navigation.PopAsync();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Security", "You don't have any rights to view this form!", "Ok");
                    await this.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            list.IsRefreshing = false;
        }

        private async void itm_look_button_Clicked(object sender, EventArgs e)
        {

            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

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
                string[] exdID = btn.StyleId.Split('_');
                iSelectedItemlookupId = int.Parse(exdID[1]);
                Button btn_cancel = new Button()
                {
                    Text = "Cancel",
                    WidthRequest = 100,
                    HeightRequest = 40,
                    Margin = new Thickness(0, 0, 10, 10),
                    TextColor = Color.Accent,
                    BackgroundColor = Color.Transparent,
                    HorizontalOptions = LayoutOptions.EndAndExpand,
                    VerticalOptions = LayoutOptions.EndAndExpand

                };
                btn_cancel.Clicked += Btn_cancel_Clicked;

                SearchBar ext_search = new SearchBar();
                ext_search.TextChanged += ext_serch;
                ext_search.HorizontalOptions = LayoutOptions.FillAndExpand;

                lstView.ItemsSource = null;


                int? EDS = 0;
                foreach (var item in ControlSchema)
                {
                    if (item.intItemInfoFieldID == iSelectedItemlookupId)
                    {
                        EDS = item.intExternalDatasourceID;
                        break;
                    }
                }

                List<ExternalDatasourceValue> lst_extdatasource = new List<ExternalDatasourceValue>();
                lstexternaldatasource = new List<ExternalDatasourceValue>();

                lstexternaldatasource.Add(ExdDefaultValue);

                List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDChild == iSelectedItemlookupId).ToList();



                if (infoFieldChild.Count > 0)
                {
                    var Child1 = ControlSchema.Where(t => t.intItemInfoFieldID == iSelectedItemlookupId).ToList();

                    foreach (var iChild in Child1)
                    {
                        Picker drp1 = FindQuestControl(iChild.intItemInfoFieldID.ToString(), "picker") as Picker;
                        Button btn1 = FindQuestControl(iChild.intItemInfoFieldID.ToString(), "button") as Button;
                        List<DataTypes.DataType.Quest.ExternalDatasourceInfo> lsEX = new List<DataTypes.DataType.Quest.ExternalDatasourceInfo>();

                        await Task.Run(() =>
                        {
                            var Response = QuestSyncAPIMethods.GetExternalDatasourceInfoByID(App.Isonline, iChild.intExternalDatasourceID.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                            lsEX = Response.Result;
                        });

                        string sConnectionString = string.Empty;
                        string sQuery = string.Empty;
                        string filterQueryOrg = string.Empty;
                        if (lsEX != null && lsEX.Count > 0)
                        {
                            sConnectionString = lsEX?.FirstOrDefault()?._CONNECTION_STRING;
                            sQuery = lsEX?.FirstOrDefault()?._QUERY;

                            await Task.Run(() =>
                             {
                                 var res = QuestSyncAPIMethods.GetFilterQuery_Quest(App.Isonline, iChild.intExternalDatasourceID.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                                 var d = res.Result;
                                 filterQueryOrg = d[0]._FILTER_QUERY;
                             });

                            List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldparent = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == iChild.intItemInfoFieldID)?.ToList();

                            if (infoFieldparent != null && infoFieldparent.Count > 0)
                            {
                                int parentCount = 1;
                                foreach (var p in infoFieldparent)
                                {
                                    string value = "-1";
                                    Picker drpParent = FindQuestControl(p.intItemInfoFieldIDParent.ToString(), "picker") as Picker;
                                    Button btn_ctrl = FindQuestControl(p.intItemInfoFieldIDParent.ToString(), "button") as Button;

                                    if (drpParent != null)
                                    {
                                        if (drpParent.SelectedItem != null)
                                        {
                                            value = (drpParent.SelectedItem as ExternalDatasourceValue).ID?.ToString();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }

                                    sQuery = GetQueryStringWithParamaters(sQuery, p.strExternalDatasourceNameParent, value, p.strInfoFieldDisplayNameParent);//6
                                                                                                                                                             //replace internal entity type 
                                    if (!string.IsNullOrEmpty(filterQueryOrg))
                                    {
                                        string filterQuery;
                                        filterQuery = filterQueryOrg.Replace("{'EXTERNAL_DATASOURCE_OBJECT_ID'}", "(" + value + ")");
                                        filterQuery = filterQuery.Replace("/*coalesce(CCS.CONNECTION_NAME,'BOXER_ENTITIES') like '%BOXER_ENTITIES%' */".ToUpper(), " and coalesce(CCS.CONNECTION_NAME, 'BOXER_ENTITIES') like '%BOXER_ENTITIES%'".ToUpper());
                                        sQuery = sQuery.Replace("/*{ENTITY_FILTER_QUERY_" + parentCount + "}*/", filterQuery);
                                        parentCount++;
                                    }

                                    if (sQuery.ToUpper().Contains("|CURRENT_USER"))
                                    {
                                        sQuery = GetQueryStringWithParamaters(sQuery, "CURRENT_USER", Functions.UserName);
                                    }

                                    try
                                    {
                                        await Task.Run(() =>
                                         {
                                             var qUesponse = QuestAPIMethods.GetExternalDatasourceByQuery(sQuery, Functions.GetDecodeConnectionString(sConnectionString));//API Need
                                             var qResult = qUesponse.GetValue("ResponseContent");
                                             List<ExternalDatasourceValue> l = JsonConvert.DeserializeObject<List<ExternalDatasourceValue>>(qResult.ToString());

                                             lstexternaldatasource.AddRange(l);
                                         });
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }

                        //if (sQuery.ToUpper().Contains("|CURRENT_USER"))
                        //{
                        //    sQuery = GetQueryStringWithParamaters(sQuery, "CURRENT_USER", Functions.UserName);
                        //}

                        //try
                        //{
                        //    var qUesponse = QuestAPIMethods.GetExternalDatasourceByQuery(sQuery, Functions.GetDecodeConnectionString(sConnectionString));//API Need
                        //    var qResult = qUesponse.GetValue("ResponseContent");
                        //    List<GetExternalDatasourceByIDResponse.ExternalDataSource> l = JsonConvert.DeserializeObject<List<GetExternalDatasourceByIDResponse.ExternalDataSource>>(qResult.ToString());

                        //    lstexternaldatasource.AddRange(l);
                        //    //drp.ItemsSource = null;
                        //    //drp.ItemsSource = l;
                        //    //drp.SelectedIndex = 0;
                        //}
                        //catch (Exception ex) { }

                    }
                }
                else
                {
                    await Task.Run(() =>
                    {
                        var temp_extdatasource = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, EDS.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, iSelectedItemlookupId.ToString());

                        temp_extdatasource.Wait();
                        if (temp_extdatasource.Result.Count > 0)
                        {
                            lstexternaldatasource.AddRange(temp_extdatasource.Result);
                        }
                    });

                }


                lstView.ItemsSource = lstexternaldatasource.Select(v => v.NAME);


                #region Popup initilize
                lstView.WidthRequest = 260;
                lstView.IsPullToRefreshEnabled = true;
                lstView.Refreshing += OnRefresh;
                lstView.ItemTapped += LstView_ItemTapped;
                lstView.HasUnevenRows = true;

                lstView.BackgroundColor = Color.White;

                var temp = new DataTemplate(typeof(TextViewCell));
                lstView.ItemTemplate = temp;

                popupLT.Children.Clear();
                popupLT.Children.Add(new StackLayout
                {
                    Children =
                    {
                        //Search Bar Stacklayout
                        new StackLayout{
                            Children =
                            {
                                ext_search
                            }
                        },
                        
                        //ListView StackLayout
                        new StackLayout
                        {
                            Children =
                            {
                                lstView
                            }
                        },

                        //Cancel button layout
                        new StackLayout
                        {
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

        private void LstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e.Item  == null)
                {
                    return;
                }
                try
                {
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
                }
                catch (Exception)
                {
                }
                Stack_Popup.IsVisible = false;
                masterGrid.IsVisible = true;

                var pik_cntrl = FindQuestControl(iSelectedItemlookupId.ToString(), "picker");
                var cntrl = FindQuestControl(iSelectedItemlookupId.ToString(), "button");

                RefreshDropDownsAndLookUp(iSelectedItemlookupId);

                if (cntrl != null)
                {
                    Button q_btn = cntrl as Button;
                    q_btn.Text = e.Item.ToString();

                    //if (e.SelectedItem.ToString() != "-- Select Item --")
                    {
                        Picker q_pik = pik_cntrl as Picker;

                        //var itmsrc = ((ListView)sender).ItemsSource as List<ExternalDatasourceValue>;

                        var lst = lstexternaldatasource.Where(t => t.NAME.ToLower().Contains(q_btn.Text.ToLower())).ToList();

                        if (lst?.FirstOrDefault().NAME != "-- Select Item --")
                        {
                            lst.Insert(0, ExdDefaultValue);
                            q_pik.ItemsSource = lst;
                            q_pik.SelectedIndex = 1;
                        }
                        else
                            q_pik.SelectedIndex = 0;

                        List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDParent == iSelectedItemlookupId).ToList();

                        foreach (var item in infoFieldChild)
                        {
                            try
                            {
                                Button btn = FindQuestControl(item.intItemInfoFieldIDChild.ToString(), "button") as Button;
                                if (q_pik.SelectedIndex != 0)
                                    btn.IsEnabled = true;
                                else
                                    btn.IsEnabled = false;
                            }
                            catch
                            { }
                        }
                    }
                }

              ((ListView)sender).SelectedItem = null;

            }
            catch (Exception)
            {
                ((ListView)sender).SelectedItem = null;
            }
        }

        private void Btn_cancel_Clicked(object sender, EventArgs e)
        {
            Stack_Popup.IsVisible = false;
            masterGrid.IsVisible = true;
        }

        private void ext_serch(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (lstexternaldatasource.Count > 0)
                {
                    if (string.IsNullOrEmpty(e.NewTextValue))
                    {
                        lstView.ItemsSource = lstexternaldatasource.Select(v => v.NAME);
                    }
                    else
                    {
                        var list = lstexternaldatasource.Where(v => v.NAME.ToLower().Contains(e.NewTextValue.ToString().ToLower())).ToList();
                        if (list.Count > 0)
                        {
                            lstView.ItemsSource = list.Select(v => v.NAME).ToList();
                        }
                        else
                        {
                            lstView.ItemsSource = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker cnt = (Picker)sender;

            try
            {
                if (App.Isonline)
                {
                    if (cnt.ItemsSource != null)
                    {
                        if (cnt.ItemsSource.Count > 0)
                        {
                            if (cnt.SelectedItem != null)
                            {
                                RefreshDropDownsAndLookUp(Convert.ToInt32(cnt.StyleId.Split('_')[1]));

                                if (Convert.ToInt32((cnt.SelectedItem as ExternalDatasourceValue).ID) > 0)
                                {
                                    fillchildControl(cnt.StyleId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception eq)
            { }
        }
        private void RefreshDropDownsAndLookUp(int itemInfofieldId)
        {
            try
            {
                List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDParent == itemInfofieldId).ToList();

                if (infoFieldChild.Count <= 0)
                    return;

                foreach (var item in infoFieldChild)
                {
                    Picker drp = FindQuestControl(item.intItemInfoFieldIDChild.ToString()) as Picker;
                    if (drp != null)
                    {
                        List<ExternalDatasourceValue> l = new List<ExternalDatasourceValue>();

                        l.Insert(0, ExdDefaultValue);
                        drp.ItemsSource = l;
                        drp.SelectedIndex = 0;
                    }

                    /*New Added For Button text*/

                    try
                    {
                        Button btn = FindQuestControl(item.intItemInfoFieldIDChild.ToString(), "button") as Button;
                        btn.Text = "-- Select Item --";
                        btn.IsEnabled = false;
                    }
                    catch (Exception)
                    {
                    }

                    RefreshDropDownsAndLookUp(item.intItemInfoFieldIDChild);
                }
            }
            catch (Exception ex)
            {


            }
        }

        List<GetItemInfoDependencyResponse.ItemInfoDependancy> lstItemInfoDependancy = new List<GetItemInfoDependencyResponse.ItemInfoDependancy>();
        public void fillchildControl(string CurrentStyleId)
        {
            string Fieldtype = CurrentStyleId.Split('_')[0];
            int itemInfofieldId = Convert.ToInt32(CurrentStyleId.Split('_')[1]);

            {
                try
                {
                    List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDParent == itemInfofieldId).ToList();

                    foreach (var item in infoFieldChild)
                    {
                        Picker drp = FindQuestControl(item.intItemInfoFieldIDChild.ToString()) as Picker;

                        var Response = QuestSyncAPIMethods.GetExternalDatasourceInfoByID(App.Isonline, item.intExternalDatasourceIDChild.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid);
                        List<DataTypes.DataType.Quest.ExternalDatasourceInfo> ls = Response.Result;

                        string sConnectionString = string.Empty;
                        string sQuery = string.Empty;
                        string filterQueryOrg = string.Empty;
                        if (ls != null && ls.Count > 0)
                        {
                            sConnectionString = ls?.FirstOrDefault()?._CONNECTION_STRING;
                            sQuery = ls?.FirstOrDefault()?._QUERY;

                            var res = QuestSyncAPIMethods.GetFilterQuery_Quest(App.Isonline, item.intExternalDatasourceIDChild.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, itemid);
                            var d = res.Result;
                            filterQueryOrg = d[0]._FILTER_QUERY;

                            List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldparent = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDChild == item.intItemInfoFieldIDChild).ToList();

                            if (infoFieldparent != null && infoFieldparent.Count > 0)
                            {
                                int parentCount = 1;
                                foreach (var p in infoFieldparent)
                                {
                                    string value = "-1";
                                    Picker drpParent = FindQuestControl(p.intItemInfoFieldIDParent.ToString()) as Picker;

                                    if (drpParent.SelectedItem != null)
                                    {
                                        if (drpParent.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                                            value = (drpParent.SelectedItem as ExternalDatasourceValue).ID?.ToString();
                                    }

                                    sQuery = GetQueryStringWithParamaters(sQuery, p.strExternalDatasourceNameParent, value, p.strInfoFieldDisplayNameParent);//6
                                                                                                                                                             //replace internal entity type
                                    if (!string.IsNullOrEmpty(filterQueryOrg))
                                    {
                                        string filterQuery;
                                        filterQuery = filterQueryOrg.Replace("{'EXTERNAL_DATASOURCE_OBJECT_ID'}", "(" + value + ")");
                                        filterQuery = filterQuery.Replace("/*coalesce(CCS.CONNECTION_NAME,'BOXER_ENTITIES') like '%BOXER_ENTITIES%' */".ToUpper(), " and coalesce(CCS.CONNECTION_NAME,'BOXER_ENTITIES') like '%BOXER_ENTITIES%'".ToUpper());
                                        sQuery = sQuery.Replace("/*{ENTITY_FILTER_QUERY_" + parentCount + "}*/", filterQuery);
                                        parentCount++;
                                    }
                                }
                            }
                        }

                        if (sQuery.ToUpper().Contains("|CURRENT_USER"))
                        {
                            sQuery = GetQueryStringWithParamaters(sQuery, "CURRENT_USER", Functions.UserName);
                        }
                        if (drp != null)
                        {
                            try
                            {
                                var qUesponse = QuestSyncAPIMethods.GetExternalDatasourceByQuery(App.Isonline, sQuery, Functions.GetDecodeConnectionString(sConnectionString), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);//API Need
                                List<ExternalDatasourceValue> l = qUesponse.Result;
                                ExternalDatasourceValue ed = new ExternalDatasourceValue();
                                ed.ID = -1;
                                ed.NAME = "-- Select Item --";
                                ed.DESCRIPTION = "-- Select Item --";
                                l.Insert(0, ed);
                                drp.ItemsSource = null;
                                drp.ItemsSource = l;
                                drp.SelectedIndex = 0;
                            }
                            catch (Exception ex) { }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
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

        private static string GetQueryStringWithParamaters(string query, string externalDatasourceName, string value, string fieldName = "")
        {
            {
                string results = query.ToUpper();

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
                        if (match.Split('|')[1].ToUpper().Trim() == externalDatasourceName.ToUpper().Trim())
                        {
                            results = results.Replace("/*{" + match + "}*/", "And " + match.Split('|')[0] + " = '" + match.Split('|')[1].Replace(externalDatasourceName.ToUpper(), value.ToString().Trim()) + "'");
                        }
                        else
                        {
                            if (fieldName != null && fieldName.Length > 0 && match.Split('|')[1].ToUpper().Trim() == fieldName.ToUpper().Trim())
                            {
                                results = results.Replace("/*{" + match + "}*/", "And " + match.Split('|')[0] + " = '" + match.Split('|')[1].Replace(fieldName.ToUpper(), value.ToString().Trim()) + "'");
                            }
                        }
                    }
                }
                return results.ToString();
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
                var dt_Entry = FindQuestControl(sty_id) as Entry;
                dt_Entry.Text = cnt.Date.ToString("MM/dd/yyyy");
            }
            catch (Exception)
            {
                dtp.Text = "";
            }
        }
        //public object FindQuestControl(string type, string cntType = "")
        //{
        //    foreach (StackLayout v in TextFieldsLayout.Children)
        //    {
        //        foreach (StackLayout item in v.Children)
        //        {
        //            foreach (var subitem in item.Children)
        //            {
        //                var xy = subitem;
        //                Type ty = xy.GetType();

        //                if (ty.Name != "StackLayout")
        //                {
        //                    if (xy.StyleId != null)
        //                    {
        //                        if (xy.StyleId.Contains(type))
        //                        {
        //                            if (cntType == "DatePicker")
        //                            {
        //                                if (ty.Name == "DatePicker")
        //                                {
        //                                    return subitem;
        //                                }
        //                            }
        //                            else
        //                                return subitem;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    return null;
        //}

        public object FindQuestControl(string type, string cntrlName = "")
        {
            if (string.IsNullOrEmpty(cntrlName))
            {
                foreach (StackLayout v in TextFieldsLayout.Children)
                {
                    foreach (StackLayout item in v.Children)
                    {
                        foreach (var subitem in item.Children)
                        {
                            var xy = subitem.StyleId;
                            Type ty = subitem.GetType();
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
            }
            else if (cntrlName.ToLower() == "picker")
            {
                foreach (StackLayout v in TextFieldsLayout.Children)
                {
                    foreach (StackLayout item in v.Children)
                    {
                        foreach (var subitem in item.Children)
                        {
                            var _styleId = subitem.StyleId;
                            Type ty = subitem.GetType();
                            if (_styleId != null)
                            {
                                if (ty.Name.ToLower() == "picker")
                                {
                                    if (_styleId.Contains(type))
                                    {
                                        return subitem;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (cntrlName.ToLower() == "button")
            {
                foreach (StackLayout v in TextFieldsLayout.Children)
                {
                    foreach (StackLayout item in v.Children)
                    {
                        foreach (var subitem in item.Children)
                        {
                            var _styleId = subitem.StyleId;
                            Type ty = subitem.GetType();
                            if (_styleId != null)
                            {
                                if (ty.Name.ToLower() == "button")
                                {
                                    if (_styleId.Contains(type))
                                    {
                                        return subitem;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (cntrlName.ToLower() == "datepicker")
            {
                foreach (StackLayout v in TextFieldsLayout.Children)
                {
                    foreach (StackLayout item in v.Children)
                    {
                        foreach (var subitem in item.Children)
                        {
                            var _styleId = subitem.StyleId;
                            Type ty = subitem.GetType();
                            if (_styleId != null)
                            {
                                if (ty.Name.ToLower() == "datepicker")
                                {
                                    if (_styleId.Contains(type))
                                    {
                                        return subitem;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }

        async void SaveClicked(object sender, System.EventArgs e)
        {
            try
            {
                dynamic action = null;
                if (_isEditable)
                {
                    action = await DisplayActionSheet("Select Option", "Cancel", null, "Save & Edit Later", "Save & Finalize");
                }
                else
                {
                    await DisplayAlert("Form Finalized", "Form is finalized,You can not update it.", "Ok");
                    this.Navigation.PopAsync();
                }

                switch (action)
                {
                    case "Save & Edit Later":
                        CreateJsonforUpdateForm(Iteminstancetranid, true);
                        break;

                    case "Save & Finalize":
                        CreateJsonforUpdateForm(Iteminstancetranid, false);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {

            try
            {
                ListView l = (ListView)sender;
                var sd = (QuestionDisplay)l.SelectedItem;
                bool iseditable = true;
                if (ControlSchema.Count > 0)
                {
                    iseditable = (bool)ControlSchema[0].blnIsEdit;
                }
                clickedCategoryId = string.Empty;
                clickedCategoryId = sd.QuestionCategoryId;


                if (CategoryIdList.Count > 0)
                {
                    if (CategoryIdList.Contains(clickedCategoryId))
                    {
                        int indexofcategory = (CategoryIdList.IndexOf(clickedCategoryId));
                        Functions.questObjectData = CategoryObjectlist[indexofcategory];
                    }
                    else
                    {
                        Functions.questObjectData = null;
                    }
                }
                this.Navigation.PushAsync(new QuestQuestionViewPage(Iteminstancetranid, sd.QuestionCategoryId, itemid, _isEditable, scatid, Convert.ToString(sd.QuestionMetadatId)));
            }
            catch (Exception ex)
            {
            }
        }

        public async void CreateJsonforUpdateForm(string Iteminstancetranid, bool isEditvalue)
        {
            int RequiredFieldCount = 0;
            try
            {
                string strobjid = string.Empty;
                string striteminfoFieldsIds = string.Empty;
                string striteminfoFieldsValues = string.Empty;
                UpdateFormRequest UpdateForm = new UpdateFormRequest()
                {
                    itemInstanceTranId = Convert.ToInt32(Iteminstancetranid),
                    isEdit = isEditvalue,
                    otherComments = "",
                    externalDatasourceObjectIDs = "",
                    itemInfoFieldIds = "",
                    itemInfoFieldValues = "",
                    itemQuestionFieldIDs = "",
                    meetsStandards = "",
                    pointsAvailable = Convert.ToString(tottalpoints),
                    pointsEarned = Convert.ToString(totalearnedpoints),
                    isCaseRequested = "",
                    notes = "",
                    modifiedBy = Functions.UserName
                };

                for (int i = 0; i < ControlSchema.Count(); i++)
                {
                    var _field_type = ControlSchema[i].strFieldType.ToLower();
                    switch (_field_type)
                    {
                        case "se":
                        case "el":
                        case "me":
                            //var cnt = findControl((_field_type) + "_" + ControlSchema[i].intItemInfoFieldID);
                            var cnt = FindQuestControl((_field_type) + "_" + ControlSchema[i].intItemInfoFieldID, "picker");

                            Type cnt_type = cnt.GetType();
                            var pick_Ext_datasrc = new Picker();
                            if (cnt_type.Name.ToLower() == "picker")
                            {
                                pick_Ext_datasrc = (Picker)cnt;
                            }

                            //if (pick_Ext_datasrc.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                            var picker_value = pick_Ext_datasrc.SelectedItem as ExternalDatasourceValue;

                            if (ControlSchema[i]._IS_REQUIRED == "Y")
                            {
                                if (Convert.ToInt32(pick_Ext_datasrc.SelectedIndex) == 0)
                                {
                                    this.DisplayAlert("Quest Form", "Please Eneter Valid Data in " + ControlSchema[i].strItemInfoFieldName, "Ok");
                                    RequiredFieldCount = RequiredFieldCount + 1;
                                    goto requiredJump;
                                }
                                else
                                {
                                    strobjid += picker_value?.ID + ",";
                                    striteminfoFieldsIds += ControlSchema[i]?.intItemInfoFieldID + ",";
                                    striteminfoFieldsValues += picker_value?.NAME + ",";
                                }
                            }
                            else
                            {
                                strobjid += picker_value?.ID + ",";
                                striteminfoFieldsIds += ControlSchema[i]?.intItemInfoFieldID + ",";
                                striteminfoFieldsValues += picker_value?.NAME + ",";
                            }
                            break;

                        case "do":
                        case "dt":
                        case "st":
                        case "hl":
                        case "mn":
                        case "ta":
                        case "sn":
                        case "et":
                        case "cl":

                            cnt = FindQuestControl((_field_type) + "_" + ControlSchema[i].intItemInfoFieldID);
                            cnt_type = cnt.GetType();
                            var date_pick = new DatePicker();
                            var ent = new Entry();
                            ent.FontSize = 16;
                            var edit = new BorderEditor();
                            edit.FontSize = 16;
                            if (cnt_type.Name.ToLower() == "datepicker")
                            {
                                date_pick = (DatePicker)cnt;
                                if (ControlSchema[i]._IS_REQUIRED == "Y")
                                {
                                    if (date_pick.Date != Convert.ToDateTime("01/01/1900"))
                                    {
                                        strobjid += Convert.ToString(ControlSchema[i].intExternalDatasourceID) + ",";
                                        striteminfoFieldsIds += ControlSchema[i].intItemInfoFieldID + ",";
                                        striteminfoFieldsValues += Convert.ToString(CommonConstants.DateFormatStringToString(date_pick.Date.Date.ToString("MM/dd/yyyy"))) + ",";
                                    }
                                    else
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + ControlSchema[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }
                                }
                            }
                            else if (cnt_type.Name.ToLower() == "entry")
                            {
                                ent = (Entry)cnt;

                                if (ControlSchema[i]._IS_REQUIRED == "Y")
                                {
                                    if (string.IsNullOrEmpty(ent.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + ControlSchema[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }

                                    else
                                    {
                                        strobjid += Convert.ToString(ControlSchema[i].intExternalDatasourceID) + ",";
                                        striteminfoFieldsIds += ControlSchema[i].intItemInfoFieldID + ",";
                                        striteminfoFieldsValues += Convert.ToString(ent.Text) + ",";
                                    }
                                }
                                else
                                {
                                    strobjid += Convert.ToString(ControlSchema[i].intExternalDatasourceID) + ",";
                                    striteminfoFieldsIds += ControlSchema[i].intItemInfoFieldID + ",";
                                    striteminfoFieldsValues += Convert.ToString(ent.Text) + ",";
                                }

                            }
                            else if (cnt_type.Name.ToLower() == "bordereditor")
                            {
                                edit = (BorderEditor)cnt;

                                if (ControlSchema[i]._IS_REQUIRED == "Y")
                                {
                                    if (string.IsNullOrEmpty(edit.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + ControlSchema[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }

                                    else
                                    {
                                        strobjid += Convert.ToString(ControlSchema[i].intExternalDatasourceID) + ",";
                                        striteminfoFieldsIds += ControlSchema[i].intItemInfoFieldID + ",";
                                        striteminfoFieldsValues += Convert.ToString(edit.Text) + ",";
                                    }
                                }
                                else
                                {
                                    strobjid += Convert.ToString(ControlSchema[i].intExternalDatasourceID) + ",";
                                    striteminfoFieldsIds += ControlSchema[i].intItemInfoFieldID + ",";
                                    striteminfoFieldsValues += Convert.ToString(edit.Text) + ",";
                                }
                            }

                            break;
                        default:

                            break;

                    }
                }

                requiredJump:
                int abc = 0;

                if (RequiredFieldCount == 0)
                {
                    UpdateForm.itemInfoFieldIds = striteminfoFieldsIds.TrimEnd(',');
                    UpdateForm.externalDatasourceObjectIDs += strobjid?.TrimEnd(',');
                    UpdateForm.itemInfoFieldValues += striteminfoFieldsValues.TrimEnd(',');

                    var ls = UpdateForm.externalDatasourceObjectIDs?.Split(',')?.ToList();
                    var lsInfoValues = UpdateForm.itemInfoFieldValues?.Split(',')?.ToList();
                    List<AddFormRequest.ItemInfoFieldInfo> LValue = new List<AddFormRequest.ItemInfoFieldInfo>();



                    for (int i = 0; i < ls.Count; i++)
                    {
                        for (int j = 0; i < lsInfoValues.Count; j++)
                        {
                            if (i == j)
                            {
                                AddFormRequest.InfoFieldValues iValue = new AddFormRequest.InfoFieldValues();
                                AddFormRequest.ItemInfoFieldInfo iValueinfo = new AddFormRequest.ItemInfoFieldInfo();
                                iValueinfo.ItemInfoFieldId = Convert.ToInt32(UpdateForm.itemInfoFieldIds.Split(',')[j]);
                                iValue.externalDatasourceObjectIDs = ls[j];
                                iValue.ItemInfoFieldText = lsInfoValues[j];
                                iValueinfo.ItemInfoFieldData = iValue;
                                LValue.Add(iValueinfo);
                                break;
                            }
                        }
                    }

                    AddFormRequest addForm = new AddFormRequest();
                    addForm.itemId = Convert.ToInt32(itemid);
                    addForm.isEdit = UpdateForm.isEdit;
                    addForm.otherComments = UpdateForm.otherComments;
                    addForm.itemQuestionFieldIDs = UpdateForm.itemInfoFieldIds;
                    addForm.meetsStandards = UpdateForm.meetsStandards;
                    addForm.pointsAvailable = UpdateForm.pointsAvailable;
                    addForm.pointsEarned = UpdateForm.pointsEarned;
                    addForm.isCaseRequested = UpdateForm.isCaseRequested;
                    addForm.notes = UpdateForm.notes;
                    addForm.ItemInfoFieldValues = LValue;
                    dynamic ApiCallResponse = null;

                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    await Task.Run(async () =>
                    {
                        var ApiCallUpdateForm = QuestSyncAPIMethods.StoreAndUpdate(App.Isonline, int.Parse(itemid), UpdateForm, "", Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, addForm, lst);
                        ApiCallResponse = ApiCallUpdateForm.Result;
                    });

                    int responseStatus = 0;

                    if (CategoryObjectlist.Count > 0)
                    {
                        for (int i = 0; i < CategoryObjectlist.Count; i++)
                        {
                            Add_Questions_MetadataRequest obj = new Add_Questions_MetadataRequest();
                            obj = CategoryObjectlist[i];

                            JObject Body = (JObject)JToken.FromObject(obj);
                            var jsonBody = JObject.Parse(Convert.ToString(Body));

                            //string transid = Convert.ToString(CategoryObjectlist[i].pITEM_INSTANCE_TRAN_ID);
                            //string transItemid = Convert.ToString(CategoryObjectlist[i].pITEM_ID);
                            //string transitemQuestionsId = Convert.ToString(CategoryObjectlist[i].pITEM_QUESTION_FIELD_IDs);
                            //string transStandardmeet = CategoryObjectlist[i].pMEETS_STANDARDS;
                            //string transPAvailable = CategoryObjectlist[i].pPOINTS_AVAILABLE;
                            //string transPEarned = CategoryObjectlist[i].pPOINTS_EARNED;
                            //string transCaseReq = CategoryObjectlist[i].pIS_CASE_REQUESTED;
                            //string transNotes = CategoryObjectlist[i].pNOTES;


                            var apicall = QuestSyncAPIMethods.AddQuestionsMetadata(App.Isonline, Iteminstancetranid, itemid, Convert.ToString(CategoryObjectlist[i].pITEM_QUESTION_FIELD_IDs), CategoryObjectlist[i].pMEETS_STANDARDS, CategoryObjectlist[i].pPOINTS_AVAILABLE, CategoryObjectlist[i].pPOINTS_EARNED, CategoryObjectlist[i].pIS_CASE_REQUESTED, "0", CategoryObjectlist[i].pNOTES, CategoryObjectlist[i].pCREATED_BY, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, jsonBody, "C");
                            responseStatus = apicall.Result;
                        }
                    }


                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                    if (!string.IsNullOrEmpty(Convert.ToString(ApiCallResponse)))
                    {
                        if (isEditvalue == false)
                        {
                            _isEditable = false;
                        }
                        await this.DisplayAlert("Quest", "Save Successful.", "Ok");
                        await this.Navigation.PopAsync();

                    }
                    else
                    {
                        await this.DisplayAlert("Quest", "Save Unsuccessful.", "Ok");
                    }
                }

            }
            catch (Exception ex)
            {
                //await this.Navigation.PopAsync();
            }
        }

        //public object findControl(string type)
        //{
        //    foreach (StackLayout v in TextFieldsLayout.Children)
        //    {
        //        foreach (StackLayout item in v.Children)
        //        {
        //            foreach (var subitem in item.Children)
        //            {
        //                var xy = subitem.StyleId;

        //                if (xy != null)
        //                {
        //                    if (xy.Contains(type))
        //                    {
        //                        return subitem;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}
    }
    public class QuestionDisplay
    {
        public string QuestionCategoryId { get; set; }
        public string QuestionMetadatId { get; set; }
        public string QuestionCategoryName { get; set; }
        public string QuestionAvailblePoints { get; set; }
        public string QuestionEarnedPoints { get; set; }
        public string QuestionOveralscore { get; set; }


        public QuestionDisplay(string QuestionCategoryid, string QuestionMetadatid, string QuestionCategoryname, string availpoints, string earnpoints, string score)
        {
            QuestionCategoryId = QuestionCategoryid;
            QuestionMetadatId = QuestionMetadatid;
            QuestionCategoryName = QuestionCategoryname;
            QuestionAvailblePoints = "Points Availbale:" + availpoints;
            QuestionEarnedPoints = "Earned:" + earnpoints;


            QuestionOveralscore = "Section Score:" + score + "%";
        }
    }
}
