using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using static StemmonsMobile.DataTypes.DataType.Quest.AddFormRequest;
//using StemmonsMobile.DataTypes.DataType.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using StemmonsMobile.Views.LoginProcess;
using System.Globalization;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemCategoriesByItemIDResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemQuestionFieldsByItemCategoryID_ViewScoresResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemInfoDependencyResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse;

namespace StemmonsMobile.Views.CreateQuestForm
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewQuestForm : ContentPage
    {

        string ItemInstanceTranId;
        int itemid = 0;
        int RequiredFieldCount = 0;
        int ViewCount, SaveClick;
        string clickedCategoryId = string.Empty;
        List<ItemInfoField> Control_Schema = new List<ItemInfoField>();
        AddFormRequest addForm = new AddFormRequest();
        string jsonaddForm = string.Empty;
        double avpoints = 0.0;
        //List<GetItemInfoDependencyResponse.ItemInfoDependancy> lstItemInfoDependancy = new List<GetItemInfoDependencyResponse.ItemInfoDependancy>();
        string aredID = string.Empty;
        List<string> CategoryIdList = new List<string>();
        List<Add_Questions_MetadataRequest> CategoryObjectlist = new List<Add_Questions_MetadataRequest>();
        List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> lstcatbyitemid = new List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>();


        GetExternalDatasourceByIDResponse.ExternalDataSource ExdDefaultValue = new GetExternalDatasourceByIDResponse.ExternalDataSource
        {
            strObjectID = "-1",
            strName = "-- Select Item --",
            strDescription = "-- Select Item --"
        };
        //ArpanB Start
        public static ObservableCollection<string> Exditems { get; set; }
        public Dictionary<int, List<GetExternalDatasourceByIDResponse.ExternalDataSource>> lstexnaldatasouce = new Dictionary<int, List<GetExternalDatasourceByIDResponse.ExternalDataSource>>();
        List<GetExternalDatasourceByIDResponse.ExternalDataSource> lstexternaldatasource = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
        int iSelectedItemlookupId = 0;
        ListView lstView = new ListView();
        //ArpanB End

        public NewQuestForm(int _itemid, string arID = "")
        {
            InitializeComponent();
            itemid = _itemid;
            ViewCount = 0;
            SaveClick = 0;
            aredID = arID;
            Functions.questObjectData = null;
            CategoryIdList = new List<string>();
            CategoryObjectlist = new List<Add_Questions_MetadataRequest>();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            try
            {
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
            }
            catch (Exception ex)
            {
            }

            if (ViewCount == 0)
            {
                ViewCount = ViewCount + 1;
                DynamicFields.Children.Clear();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                try
                {
                    // List<ItemCategoryByItemId> itemCatList = new List<ItemCategoryByItemId>();
                    await Task.Run(action: () =>
                    {
                        var result = QuestSyncAPIMethods.AssignControlsAsync(App.Isonline, Convert.ToString(itemid), Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                        Control_Schema = result.Result;

                        var getInfoDepeResult = QuestSyncAPIMethods.GetItemInfoDependency(App.Isonline, "0", itemid.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, "0");
                        lstItemInfoDependancy = getInfoDepeResult.Result;

                        var getitemcategoryCall = QuestSyncAPIMethods.GetItemCategoriesByItemID(App.Isonline, Convert.ToString(itemid), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, aredID);
                        getitemcategoryCall.Wait();
                        lstcatbyitemid = getitemcategoryCall.Result;
                    });

                    #region Draw Control Dynamically

                    for (int i = 0; i < Control_Schema.Count; i++)
                    {
                        var layout = new StackLayout();
                        layout.Orientation = StackOrientation.Horizontal;

                        var layout1 = new StackLayout();
                        layout1.HorizontalOptions = LayoutOptions.Start;
                        layout1.WidthRequest = 200;

                        var Label1 = new Xamarin.Forms.Label
                        {
                            VerticalOptions = LayoutOptions.Start,
                        };

                        FormattedString frmtText = new FormattedString();
                        frmtText.Spans.Add(new Span { Text = Control_Schema[i].strItemInfoFieldDesc + ":" });

                        if (Control_Schema[i].strIsRequired.ToLower() == "y")
                            frmtText.Spans.Add(new Span { Text = " *", ForegroundColor = Color.Red });

                        Label1.FormattedText = frmtText;

                        // Label1.Text = lst_NewQuestFormFields[i].strItemInfoFieldDesc;//strItemInfoFieldName
                        Label1.HorizontalOptions = LayoutOptions.Start;
                        Label1.FontSize = 16;

                        layout1.Children.Add(Label1);
                        layout.Children.Add(layout1);

                        var Rightlayout = new StackLayout();
                        Rightlayout.HorizontalOptions = LayoutOptions.Start;

                        switch (Control_Schema[i].strFieldType.ToLower())
                        {
                            #region -- SE -- EL -- ME --
                            case "se":
                            case "el":
                            case "me":
                                Picker pk = new Picker();
                                pk.WidthRequest = 200;
                                List<GetExternalDatasourceByIDResponse.ExternalDataSource> view = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();

                                #region Old Code

                                //if (App.Isonline)
                                //{
                                //    List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == lst_NewQuestFormFields[i].intItemInfoFieldID).ToList();

                                //    if (infoFieldChild?.Count < 1)
                                //    {
                                //        var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                                //        view = ExternalCall.Result;
                                //    }
                                //}
                                //else
                                //{
                                //    var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                                //    view = ExternalCall.Result;
                                //} 
                                #endregion

                                view.Insert(0, ExdDefaultValue);
                                pk.ItemsSource = view;
                                //pk.TextColor = Color.Gray;

                                pk.ItemDisplayBinding = new Binding("strName");
                                pk.SelectedIndex = 0;
                                pk.IsVisible = false;
                                pk.StyleId = Control_Schema[i].strFieldType.ToLower() + "_" + Control_Schema[i].intItemInfoFieldID;

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
                                    itm_look_button.BorderRadius = 5;
                                    itm_look_button.BorderColor = Color.Gray;
                                    itm_look_button.CornerRadius = 5;
                                }

                                itm_look_button.IsVisible = true;


                                List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == Control_Schema[i].intItemInfoFieldID).ToList();

                                if (infoFieldChild.Count >= 1)
                                    itm_look_button.IsEnabled = false;


                                itm_look_button.StyleId = Control_Schema[i].strFieldType.ToLower() + "_" + Control_Schema[i].intItemInfoFieldID;

                                itm_look_button.Clicked += itm_look_button_Clicked;
                                itm_look_button.Text = "-- Select Item --";
                                itm_look_button.WidthRequest = 200;

                                Rightlayout.Children.Add(pk);
                                Rightlayout.Children.Add(itm_look_button);
                                Rightlayout.BackgroundColor = Color.Gray;

                                break;
                            #endregion

                            #region -- DO -- DT --
                            case "do":
                            case "dt":
                                Entry txt_Date = new Entry();
                                txt_Date.Placeholder = "Select Date";
                                txt_Date.WidthRequest = 170;
                                txt_Date.TextColor = Color.Gray;
                                txt_Date.Keyboard = Keyboard.Numeric;
                                txt_Date.Text = "";
                                txt_Date.StyleId = Control_Schema[i].strFieldType.ToLower() + "_" + Control_Schema[i].intItemInfoFieldID;

                                Image img_clr = new Image();
                                img_clr.StyleId = "imgdo_" + Control_Schema[i].intItemInfoFieldID;
                                img_clr.Source = ImageSource.FromFile("Assets/erase16.png");
                                img_clr.WidthRequest = 25;
                                img_clr.HeightRequest = 25;
                                #region date_pick
                                DatePicker date_pick = new DatePicker();
                                date_pick.IsVisible = false;
                                date_pick.Format = "MM/dd/yyyy";
                                date_pick.WidthRequest = 200;
                                date_pick.TextColor = Color.Gray;
                                date_pick.StyleId = "do_" + Control_Schema[i].intItemInfoFieldID;
                                #endregion

                                try
                                {

                                    if (!(Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("C") || Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        txt_Date.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                                Rightlayout.Orientation = StackOrientation.Horizontal;
                                Rightlayout.Children.Add(txt_Date);
                                Rightlayout.Children.Add(img_clr);
                                Rightlayout.Children.Add(date_pick);
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

                                var clear_triger = new TapGestureRecognizer();
                                clear_triger.Tapped += (s, e) =>
                                {
                                    Entry C_ent = new Entry();
                                    try
                                    {
                                        var ct = (Image)s;
                                        var sty_id = ct.StyleId?.Split('_')[1];
                                        C_ent = FindQuestControl(sty_id) as Entry;
                                        C_ent.Text = "";
                                    }
                                    catch (Exception)
                                    {
                                        C_ent.Text = "";
                                    }
                                };
                                img_clr.GestureRecognizers.Add(clear_triger);

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

                            #region -- TA -- ET --
                            case "et":
                            case "ta":
                                BorderEditor TA = new BorderEditor();
                                TA.HeightRequest = 100;
                                TA.WidthRequest = 200;
                                TA.BorderWidth = 1;
                                TA.CornerRadius = 5;
                                TA.FontSize = 16;
                                TA.Keyboard = Keyboard.Default;

                                TA.StyleId = Control_Schema[i].strFieldType.ToLower() + "_" + Control_Schema[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("C") || Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
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

                            #region -- ST -- HL -- MN -- SN --
                            case "st":
                            case "hl":
                            case "mn":
                            case "sn":
                                Entry ST = new Entry();
                                ST.WidthRequest = 200;
                                ST.FontSize = 16;

                                if (Control_Schema[i].strFieldType.ToLower() == "st" || Control_Schema[i].strFieldType.ToLower() == "hl")
                                    ST.Keyboard = Keyboard.Default;
                                else
                                    ST.Keyboard = Keyboard.Numeric;

                                ST.StyleId = Control_Schema[i].strFieldType.ToLower() + "_" + Control_Schema[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("C") || Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        ST.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                                Rightlayout.Children.Add(ST);
                                break;
                            #endregion

                            #region -- CL --
                            case "cl":
                                Entry CL = new Entry();
                                CL.WidthRequest = 200;
                                CL.FontSize = 16;
                                CL.IsEnabled = false;
                                CL.StyleId = Control_Schema[i].strFieldType.ToLower() + "_" + Control_Schema[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("C") || Control_Schema[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
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
                        DynamicFields.Children.Add(layout);
                    }

                    #endregion

                    #region Set Question Category Grid
                    for (int i = 0; i < lstcatbyitemid.Count; i++)
                    {
                        ItemCategoryByItemId itemCatByid = lstcatbyitemid[i];

                        List<ItemQuestionField_ViewScoresModel> itemcategoryscroreList = new List<ItemQuestionField_ViewScoresModel>();
                        await Task.Run(() =>
                        {
                            var getitemcategoryscrore = QuestSyncAPIMethods.GetItemQuestionFieldsByItemCategoryIDviewscores(App.Isonline, Convert.ToString(itemCatByid.intItemCategoryID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                            var tempscores = getitemcategoryscrore.Result;
                            getitemcategoryscrore.Wait();
                            itemcategoryscroreList = getitemcategoryscrore.Result;
                        });

                        if (itemcategoryscroreList != null)
                        {
                            itemCatByid.strAvaliablePoints = Convert.ToString(itemcategoryscroreList.Where(a => a.intItemCategoryID == itemCatByid.intItemCategoryID).Sum(v => v.dcPointsAvailable));

                            avpoints += Convert.ToDouble(itemcategoryscroreList.Where(a => a.intItemCategoryID == itemCatByid.intItemCategoryID).Sum(v => v.dcPointsAvailable));
                        }
                    }
                    #endregion

                    listQuesCategorydata.ItemsSource = lstcatbyitemid;

                    overallpoints.Text = "Overall Points Available:-" + Convert.ToString(avpoints);
                    StaticFooter.IsVisible = true;

                    int heightRowList = 90;
                    int iq = (lstcatbyitemid.Count * heightRowList);
                    listQuesCategorydata.HeightRequest = iq;

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                // Called When Come back from Question Detail Page to show Updated Score
                try
                {
                    var availpoints = Functions.questObjectData.pPOINTS_AVAILABLE.Split(',');
                    decimal Aval = 0;
                    foreach (var _item in availpoints)
                    {
                        Aval += Convert.ToInt32(_item);
                    }

                    var Earnedpoints = Functions.questObjectData.pPOINTS_EARNED.Split(',');
                    decimal Earn = 0;
                    foreach (var _item in Earnedpoints)
                    {
                        Earn += Convert.ToInt32(_item);
                    }

                    decimal totalpercentage = Math.Round((Earn == 0 && Aval == 0) ? 0 : ((Earn / Aval) * 100), 0);

                    List<ItemCategoryByItemId> tem = new List<ItemCategoryByItemId>();

                    foreach (var item in lstcatbyitemid)
                    {
                        if (item.intItemCategoryID == Convert.ToInt32(clickedCategoryId))
                        {
                            item.strAvaliablePoints = Convert.ToString(Aval);
                            item.strEarned = Convert.ToString(Earn);
                            item.strScore = Convert.ToString(totalpercentage);
                        }
                        tem.Add(item);
                    }

                    listQuesCategorydata.ItemsSource = tem;

                    var totaval = Convert.ToDecimal(lstcatbyitemid.Sum(v => Convert.ToDecimal(v.strAvaliablePoints)));
                    overallpoints.Text = "Overall Points Available:-" + Convert.ToString(totaval);
                    var totEarn = Convert.ToDecimal(lstcatbyitemid.Sum(v => Convert.ToDecimal(v.strEarned)));
                    txtEarnedpoints.Text = "Earned:-" + totEarn;

                    decimal totdalpercentage = Math.Round((totEarn == 0 && totaval == 0) ? 0 : (totEarn / totaval * 100), 2);

                    txtScore.Text = "Score:-" + totdalpercentage;
                }
                catch (Exception)
                {
                }
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

                lstView.ItemsSource = null;

                var btn = sender as Button;
                string[] exdID = btn.StyleId.Split('_');
                iSelectedItemlookupId = int.Parse(exdID[1]);
                int? EDS = 0;
                foreach (var item in Control_Schema)
                {
                    if (item.intItemInfoFieldID == iSelectedItemlookupId)
                    {
                        EDS = item.intExternalDatasourceID;
                        break;
                    }
                }

                List<GetExternalDatasourceByIDResponse.ExternalDataSource> lst_extdatasource = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
                lstexternaldatasource = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();

                lstexternaldatasource.Add(ExdDefaultValue);

                List<ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDChild == iSelectedItemlookupId).ToList();

                if (infoFieldChild.Count > 0)
                {
                    //FindQuestControl -- picker

                    var Child1 = Control_Schema.Where(t => t.intItemInfoFieldID == iSelectedItemlookupId).ToList();

                    foreach (var iChild in Child1)
                    {
                        Picker drp1 = FindQuestControl(iChild.intItemInfoFieldID.ToString(), "picker") as Picker;
                        Button btn1 = FindQuestControl(iChild.intItemInfoFieldID.ToString(), "button") as Button;

                        var Response = QuestSyncAPIMethods.GetExternalDatasourceInfoByID(App.Isonline, iChild.intExternalDatasourceID.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                        List<ExternalDatasourceInfo> ls = Response.Result;

                        string sConnectionString = string.Empty;
                        string sQuery = string.Empty;
                        string filterQueryOrg = string.Empty;
                        if (ls != null && ls.Count > 0)
                        {
                            sConnectionString = ls?.FirstOrDefault()?._CONNECTION_STRING;
                            sQuery = ls?.FirstOrDefault()?._QUERY;

                            var res = QuestSyncAPIMethods.GetFilterQuery_Quest(App.Isonline, iChild.intExternalDatasourceID.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                            var d = res.Result;
                            filterQueryOrg = d[0]._FILTER_QUERY;

                            List<ItemInfoDependancy> infoFieldparent = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == iChild.intItemInfoFieldID)?.ToList();

                            if (infoFieldparent != null && infoFieldparent.Count > 0)
                            {
                                int parentCount = 1;
                                foreach (var p in infoFieldparent)
                                {
                                    string value = "-1";
                                    Picker drpParent = FindQuestControl(p.intItemInfoFieldIDParent.ToString(), "picker") as Picker;
                                    Button btn_ctrl = FindQuestControl(p.intItemInfoFieldIDParent.ToString(), "button") as Button;

                                    //if (btn_ctrl.Text.ToLower() != "-- select item --")
                                    {
                                        if (drpParent != null)
                                        {
                                            if (drpParent.SelectedItem != null)
                                            {
                                                if (drpParent.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                                                    value = (drpParent.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource).strObjectID;
                                                else
                                                    value = (drpParent.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource).strObjectID;
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
                                            var qUesponse = QuestAPIMethods.GetExternalDatasourceByQuery(sQuery, Functions.GetDecodeConnectionString(sConnectionString));//API Need
                                            var qResult = qUesponse.GetValue("ResponseContent");
                                            List<GetExternalDatasourceByIDResponse.ExternalDataSource> l = JsonConvert.DeserializeObject<List<GetExternalDatasourceByIDResponse.ExternalDataSource>>(qResult.ToString());

                                            lstexternaldatasource.AddRange(l);
                                        }
                                        catch (Exception ex) { }

                                    }
                                }
                            }
                        }
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

                lstView.ItemsSource = lstexternaldatasource.Select(v => v.strName);

                #region Popup initilize
                lstView.WidthRequest = 260;
                lstView.IsPullToRefreshEnabled = true;
                lstView.Refreshing += OnRefresh;
                lstView.ItemSelected += OnSelection;
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



        private void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                {
                    return;
                }

                Stack_Popup.IsVisible = false;
                masterGrid.IsVisible = true;

                var pik_cntrl = FindQuestControl(iSelectedItemlookupId.ToString(), "picker");
                var cntrl = FindQuestControl(iSelectedItemlookupId.ToString(), "button");

                RefreshDropDownsAndLookUp(iSelectedItemlookupId);

                if (cntrl != null)
                {
                    Button q_btn = cntrl as Button;
                    q_btn.FontSize = 16;
                    q_btn.Text = e.SelectedItem.ToString();

                    Picker q_pik = pik_cntrl as Picker;

                    var lst = lstexternaldatasource.Where(t => t.strName.ToLower().Contains(q_btn.Text.ToLower())).ToList();

                    if (lst?.FirstOrDefault().strName != "-- Select Item --")
                    {
                        lst.Insert(0, ExdDefaultValue);
                        q_pik.ItemsSource = lst;
                        q_pik.SelectedIndex = 1;
                    }
                    else
                        q_pik.SelectedIndex = 0;


                    List<ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDParent == iSelectedItemlookupId).ToList();

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
                        //lstView.ItemsSource = lstexternaldatasource.Where(v => v.strName.ToLower().Contains(e.NewTextValue.ToString())).ToList();
                        lstView.ItemsSource = lstexternaldatasource.Select(v => v.strName);
                    }
                    else
                    {
                        var list = lstexternaldatasource.Where(v => v.strName.ToLower().Contains(e.NewTextValue.ToString().ToLower())).ToList();
                        if (list.Count > 0)
                        {
                            lstView.ItemsSource = list.Select(v => v.strName).ToList();
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
                dt_Entry.Text = cnt.Date.ToString("d");
            }
            catch (Exception)
            {
                dtp.Text = "";
            }
        }

        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker cnt = (Picker)sender;
            if (App.Isonline)
            {
                listQuesCategorydata.IsEnabled = false;

                try
                {
                    if (cnt.ItemsSource != null)
                    {
                        if (cnt.ItemsSource.Count > 0)
                        {
                            if (cnt.SelectedItem != null)
                            {
                                RefreshDropDownsAndLookUp(Convert.ToInt32(cnt.StyleId.Split('_')[1]));
                                if (cnt.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                                {
                                    if (Convert.ToInt32((cnt.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource).strObjectID) > 0)
                                    {
                                        var getInfoDepeResult = QuestSyncAPIMethods.GetItemInfoDependency(App.Isonline, "0", itemid.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                                        lstItemInfoDependancy = getInfoDepeResult.Result;

                                        fillchildControl(cnt.StyleId);
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32((cnt.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource).strObjectID) > 0)
                                    {
                                        var getInfoDepeResult = QuestSyncAPIMethods.GetItemInfoDependency(App.Isonline, "0", itemid.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                                        lstItemInfoDependancy = getInfoDepeResult.Result;
                                        //RefreshDropDownsAndLookUp(Convert.ToInt32(cnt.StyleId.Split('_')[1]));
                                        fillchildControl(cnt.StyleId);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception eq)
                {
                    throw eq;
                }

                listQuesCategorydata.IsEnabled = true;

            }
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
                        List<ExternalDataSource> l = new List<ExternalDataSource>();

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

            //using (SEMonitoredScope scope = new SEMonitoredScope("BP.Quest", "_NewForm - fillchildControl"))
            {
                try
                {

                    List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy.Where(t => t.intItemInfoFieldIDParent == itemInfofieldId).ToList();

                    foreach (var item in infoFieldChild)
                    {
                        Picker drp = FindQuestControl(item.intItemInfoFieldIDChild.ToString()) as Picker;


                        var Response = QuestSyncAPIMethods.GetExternalDatasourceInfoByID(App.Isonline, item.intExternalDatasourceIDChild.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                        List<ExternalDatasourceInfo> ls = Response.Result;

                        string sConnectionString = string.Empty;
                        string sQuery = string.Empty;
                        string filterQueryOrg = string.Empty;
                        if (ls != null && ls.Count > 0)
                        {
                            sConnectionString = ls?.FirstOrDefault()?._CONNECTION_STRING;
                            sQuery = ls?.FirstOrDefault()?._QUERY;

                            var res = QuestSyncAPIMethods.GetFilterQuery_Quest(App.Isonline, item.intExternalDatasourceIDChild.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
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
                                            value = (drpParent.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource).strObjectID;
                                        else
                                            value = (drpParent.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource).strObjectID;
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
                                var qUesponse = QuestAPIMethods.GetExternalDatasourceByQuery(sQuery, Functions.GetDecodeConnectionString(sConnectionString));//API Need
                                var qResult = qUesponse.GetValue("ResponseContent");
                                List<GetExternalDatasourceByIDResponse.ExternalDataSource> l = JsonConvert.DeserializeObject<List<GetExternalDatasourceByIDResponse.ExternalDataSource>>(qResult.ToString());

                                l.Insert(0, ExdDefaultValue);
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

        private static string GetQueryStringWithParamaters(string query, string externalDatasourceName, string value, string fieldName = "")
        {
            //using (SEMonitoredScope scope = new SEMonitoredScope("BP.Quest", "_NewForm - GetQueryStringWithParamaters"))

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



        //public object FindQuestControl(string type, string cntType = "")
        //{
        //    foreach (StackLayout v in DynamicFields.Children)
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
                foreach (StackLayout v in DynamicFields.Children)
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
                foreach (StackLayout v in DynamicFields.Children)
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
                foreach (StackLayout v in DynamicFields.Children)
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
                foreach (StackLayout v in DynamicFields.Children)
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


        public async void CreateJsonforAddForm(string itemid, bool IsEditable)
        {
            RequiredFieldCount = 0;
            string strobjid = string.Empty;
            dynamic pickervalue = null;
            try
            {
                AddFormRequest.ItemInfoFieldInfo infoobj = new AddFormRequest.ItemInfoFieldInfo();
                addForm = new AddFormRequest()
                {
                    itemId = (Convert.ToInt32(itemid)),
                    isEdit = IsEditable,
                    otherComments = "",
                    ItemInfoFieldValues = new List<AddFormRequest.ItemInfoFieldInfo>(),
                    itemQuestionFieldIDs = "",
                    meetsStandards = "",
                    pointsAvailable = Convert.ToString(avpoints),
                    pointsEarned = "0",
                    isCaseRequested = "",
                    createdBy = Functions.UserName,
                    notes = ""
                };
                infoobj.ItemInfoFieldData = new AddFormRequest.InfoFieldValues();


                for (int i = 0; i < Control_Schema.Count; i++)
                {
                    AddFormRequest.InfoFieldValues iValue = new AddFormRequest.InfoFieldValues();
                    infoobj = new AddFormRequest.ItemInfoFieldInfo();
                    var _field_type = Control_Schema[i].strFieldType.ToLower();
                    switch (_field_type)
                    {
                        case "se":
                        case "el":
                        case "me":
                            var cnt = FindQuestControl((_field_type) + "_" + Control_Schema[i].intItemInfoFieldID);
                            Type cnt_type = cnt.GetType();
                            var pick_Ext_datasrc = new Picker();
                            if (cnt_type.Name.ToLower() == "picker")
                            {
                                pick_Ext_datasrc = (Picker)cnt;
                            }

                            //if (lst_NewQuestFormFields[i].strIsRequired == "Y")
                            {
                                if (Control_Schema[i].strIsRequired == "Y" && Convert.ToInt32(pick_Ext_datasrc.SelectedIndex) == 0)
                                {
                                    DisplayAlert("Field Required.", "Please enter valid data in " + Control_Schema[i].strItemInfoFieldName, "OK");
                                    RequiredFieldCount = RequiredFieldCount + 1;
                                    goto requiredJump;
                                }
                                else
                                {
                                    //if (pick_Ext_datasrc.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                                    {
                                        pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource;
                                        strobjid = Convert.ToString(pickervalue.strObjectID);
                                    }
                                    iValue.ItemInfoFieldText = pickervalue.strName;
                                    //iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(strobjid);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(Control_Schema[i].intItemInfoFieldID));
                                }
                            }
                            //else
                            //{

                            //    //if (pick_Ext_datasrc.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                            //    {

                            //        pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource;
                            //        strobjid = Convert.ToString(pickervalue.strObjectID);

                            //    }

                            //    iValue.ItemInfoFieldText = pickervalue.strName;
                            //    //iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i]);
                            //    iValue.externalDatasourceObjectIDs = Convert.ToString(strobjid);
                            //    infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                            //}

                            break;

                        case "do":
                        case "st":
                        case "hl":
                        case "mn":
                        case "dt":
                        case "ta":
                        case "sn":
                        case "et":
                        case "cl":

                            cnt = FindQuestControl((_field_type) + "_" + Control_Schema[i].intItemInfoFieldID);
                            cnt_type = cnt.GetType();
                            if (cnt_type.Name.ToLower() == "datepicker")
                            {
                                var date_pick = new DatePicker();
                                date_pick = (DatePicker)cnt;
                                if (date_pick.Date != Convert.ToDateTime("01/01/1900"))
                                {
                                    iValue.ItemInfoFieldText += Convert.ToDateTime(App.DateFormatStringToString(date_pick.Date.ToString())).Date.ToString("MM/dd/yyyy");
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(Control_Schema[i].intExternalDatasourceID);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(Control_Schema[i].intItemInfoFieldID));
                                }
                            }
                            else if (cnt_type.Name.ToLower() == "entry")
                            {
                                var ent = new Entry();
                                ent = (Entry)cnt;

                                if (Control_Schema[i].strIsRequired == "Y")
                                {
                                    if (string.IsNullOrEmpty(ent.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + Control_Schema[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        iValue.ItemInfoFieldText = ent.Text;
                                        iValue.externalDatasourceObjectIDs = Convert.ToString(Control_Schema[i].intExternalDatasourceID);
                                        infoobj.ItemInfoFieldId = (Convert.ToInt32(Control_Schema[i].intItemInfoFieldID));
                                    }
                                }
                                else
                                {
                                    iValue.ItemInfoFieldText = ent.Text;
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(Control_Schema[i].intExternalDatasourceID);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(Control_Schema[i].intItemInfoFieldID));
                                }
                            }
                            else if (cnt_type.Name.ToLower() == "bordereditor")
                            {
                                var edit = new BorderEditor();
                                edit = (BorderEditor)cnt;
                                if (Control_Schema[i].strIsRequired == "Y")
                                {

                                    if (string.IsNullOrEmpty(edit.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + Control_Schema[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        iValue.ItemInfoFieldText = edit.Text;
                                        iValue.externalDatasourceObjectIDs = Convert.ToString(Control_Schema[i].intExternalDatasourceID);
                                        infoobj.ItemInfoFieldId = (Convert.ToInt32(Control_Schema[i].intItemInfoFieldID));
                                    }
                                }
                                else
                                {
                                    iValue.ItemInfoFieldText = edit.Text;
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(Control_Schema[i].intExternalDatasourceID);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(Control_Schema[i].intItemInfoFieldID));
                                }

                            }

                            break;
                        default:
                            break;
                    }
                    infoobj.ItemInfoFieldData = iValue;
                    addForm.ItemInfoFieldValues.Add(infoobj);
                }
                requiredJump:
                int abc = 0;
                int ApiCallResponse = 0;
                if (RequiredFieldCount == 0)
                {
                    jsonaddForm = JsonConvert.SerializeObject(addForm);

                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    await Task.Run(async () =>
                    {
                        var ApiCallAddForm = QuestSyncAPIMethods.StoreAndcreate(App.Isonline, int.Parse(itemid), addForm, "", Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, listQuesCategorydata.ItemsSource);
                        ApiCallResponse = ApiCallAddForm.Result;
                        ItemInstanceTranId = Convert.ToString(ApiCallAddForm.Result);
                    });
                    int responseStatus = 0;

                    if (CategoryObjectlist.Count > 0)
                    {
                        for (int i = 0; i < CategoryObjectlist.Count; i++)
                        {
                            Add_Questions_MetadataRequest obj = new Add_Questions_MetadataRequest();
                            obj = CategoryObjectlist[i];

                            obj.pITEM_INSTANCE_TRAN_ID = Convert.ToInt32(ItemInstanceTranId);

                            JObject Body = (JObject)JToken.FromObject(obj);
                            var jsonBody = JObject.Parse(Convert.ToString(Body));

                            string transid = Convert.ToString(CategoryObjectlist[i].pITEM_INSTANCE_TRAN_ID);
                            string transItemid = Convert.ToString(CategoryObjectlist[i].pITEM_ID);
                            string transitemQuestionsId = Convert.ToString(CategoryObjectlist[i].pITEM_QUESTION_FIELD_IDs);
                            string transStandardmeet = CategoryObjectlist[i].pMEETS_STANDARDS;
                            string transPAvailable = CategoryObjectlist[i].pPOINTS_AVAILABLE;
                            string transPEarned = CategoryObjectlist[i].pPOINTS_EARNED;
                            string transCaseReq = CategoryObjectlist[i].pIS_CASE_REQUESTED;
                            string transNotes = CategoryObjectlist[i].pNOTES;


                            var apicall = QuestSyncAPIMethods.AddQuestionsMetadata(App.Isonline, ItemInstanceTranId, Convert.ToString(CategoryObjectlist[i].pITEM_ID), Convert.ToString(CategoryObjectlist[i].pITEM_QUESTION_FIELD_IDs), CategoryObjectlist[i].pMEETS_STANDARDS, CategoryObjectlist[i].pPOINTS_AVAILABLE, CategoryObjectlist[i].pPOINTS_EARNED, CategoryObjectlist[i].pIS_CASE_REQUESTED, "0", CategoryObjectlist[i].pNOTES, Functions.UserName, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, jsonBody);
                            apicall.Wait();
                            responseStatus = apicall.Result;
                        }


                        if (ApiCallResponse >= 0)
                        {
                            try
                            {
                                GenerateCaseQueueRequest obj = new GenerateCaseQueueRequest();
                                obj.currentUser = Functions.UserName;
                                obj.itemInstanceTranId = Convert.ToInt32(ItemInstanceTranId);
                                QuestSyncAPIMethods.GenerateCaseQueue(App.Isonline, obj);
                            }
                            catch (Exception E)
                            {
                            }
                            await this.DisplayAlert("Quest", "Save Successful.", "Ok");
                            this.Navigation.PopAsync();
                        }
                        else
                        {
                            await this.DisplayAlert("Quest", "Save Unsuccessful. try again later.", "Ok");
                            this.Navigation.PopAsync();
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                    }
                    else
                    {
                        if (ApiCallResponse >= 0)
                        {
                            try
                            {
                                GenerateCaseQueueRequest obj = new GenerateCaseQueueRequest();
                                obj.currentUser = Functions.UserName;
                                obj.itemInstanceTranId = Convert.ToInt32(ItemInstanceTranId);
                                QuestSyncAPIMethods.GenerateCaseQueue(App.Isonline, obj);
                            }
                            catch (Exception E)
                            {
                            }
                            await this.DisplayAlert("Quest", "Save Successful.", "Ok");
                            this.Navigation.PopAsync();
                        }
                        else
                        {
                            await this.DisplayAlert("Quest", "Save Unsuccessful. try again later.", "Ok");
                            this.Navigation.PopAsync();
                        }
                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        void ListQuesCategorydata_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            try
            {
                ListView l = (ListView)sender;
                var sd = (GetItemCategoriesByItemIDResponse.ItemCategoryByItemId)l.SelectedItem;
                clickedCategoryId = Convert.ToString(sd.intItemCategoryID);

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

                this.Navigation.PushAsync(new QuestQuestionForm(sd.strItemCategoryName, Convert.ToString(sd.intItemCategoryID), ItemInstanceTranId, Convert.ToString(sd.intItemID), aredID, addForm, lstcatbyitemid));
            }
            catch (Exception ex)
            {
                this.DisplayAlert("Item Tapped", ex.Message.ToString(), "Ok");

            }
        }


        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                this.Unfocus();
                var action = await this.DisplayActionSheet(null, "Cancel", null, "Save & Edit Later", "Save & Finalize");
                switch (action)
                {
                    case "Save & Edit Later":
                        try
                        {

                            CreateJsonforAddForm(Convert.ToString(itemid), true);
                            Functions.questObjectData = null;
                        }
                        catch
                        {

                        }

                        break;

                    case "Save & Finalize":
                        try
                        {

                            CreateJsonforAddForm(Convert.ToString(itemid), false);
                            Functions.questObjectData = null;
                        }
                        catch
                        {

                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                this.Unfocus();
                this.DisplayAlert("Item Tapped", ex.Message.ToString(), "Ok");
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
    }

    public class Newquestformdata
    {
        public string FormName { get; set; }
        public string Status { get; set; }
        public string Points { get; set; }
        public string Bonus { get; set; }
        public string Score { get; set; }

        public Newquestformdata(String fname, String status, string points, string bonus, string score)
        {
            FormName = fname;
            Status = status;
            Points = points;
            Bonus = bonus;
            Score = score;
        }
    }

    public class NewQuestFormFields
    {
        public string intItemInfoFieldID { get; set; }
        public string intItemID { get; set; }
        public string strItemInfoFieldName { get; set; }
        public string strItemInfoFieldDesc { get; set; }
        public string intDisplayOrder { get; set; }
        public string strExtraField1 { get; set; }
        public string strExtraField2 { get; set; }
        public string strItemName { get; set; }
        public string strSupervisorEmail { get; set; }
        public string blnSuppressAlert { get; set; }
        public string blnHidePoints { get; set; }
        public string intExternalDatasourceID { get; set; }
        public string strDisplayName { get; set; }
        public string intChildCount { get; set; }
        public string intParentCount { get; set; }
        public string strExternalDatasourceName { get; set; }
        public string strConnectionString { get; set; }
        public string strExternalDatasourceQuery { get; set; }
        public string intSelectedVal { get; set; }
        public string trIsRequired { get; set; }
        public string strShowOnPage { get; set; }
        public string strIsProcessCMECase { get; set; }
        public string strIsActive { get; set; }
        public string strIsEmployeeLookup { get; set; }
        public string ItemInfoFieldParentID { get; set; }
        public string strIsShowCMECheckbox { get; set; }
        public string strIsShowAdditionalNotes { get; set; }
        public string strCreateCaseOnSaveForm { get; set; }
        public string strFieldType { get; set; }
    }

    public class Getitemcategory
    {
        public string intItemCategoryID { get; set; }
        public string intItemID { get; set; }
        public string strItemCategoryName { get; set; }
        public string strItemCategoryDesc { get; set; }
        public string strExtraField1 { get; set; }
        public string intDisplayOrder { get; set; }
        public string strSupervisorEmail { get; set; }
        public string blnSuppressAlert { get; set; }
        public string blnHidePoint { get; set; }
        public string strIsProcessCMECase { get; set; }
        public string strisActive { get; set; }
        public string strHeader1 { get; set; }
        public string strHeader2 { get; set; }
        public string strHeader3 { get; set; }
        public string strIsShowCMECheckBox { get; set; }
    }

    public class Externaladatasourcedata1
    {
        public string intExternalDataSourceID { get; set; }
        public string strName { get; set; }
        public string strDescription { get; set; }
        public string strConnectionString { get; set; }
        public string strQuery { get; set; }
        public string strObjectID { get; set; }
        public string strObjectDisplay { get; set; }
        public string strObjectDescription { get; set; }
        public string URLDrillInto { get; set; }
        public string strSystemCode { get; set; }
        public string strIsActive { get; set; }
        public string dtCreatedDataTime { get; set; }
        public string strCreatedBy { get; set; }
        public string dtModifiedDatetime { get; set; }
        public string strModifiedBy { get; set; }
    }
}