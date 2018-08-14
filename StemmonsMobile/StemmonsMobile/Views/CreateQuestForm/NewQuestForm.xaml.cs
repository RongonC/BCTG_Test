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
using StemmonsMobile.Views.LoginProcess;
using System.Globalization;

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
        List<ItemInfoField> lst_NewQuestFormFields = new List<ItemInfoField>();
        AddFormRequest addForm = new AddFormRequest();
        string jsonaddForm = string.Empty;
        double avpoints = 0.0;
        //List<GetItemInfoDependencyResponse.ItemInfoDependancy> lstItemInfoDependancy = new List<GetItemInfoDependencyResponse.ItemInfoDependancy>();
        string catid = string.Empty;
        List<string> CategoryIdList = new List<string>();
        List<Add_Questions_MetadataRequest> CategoryObjectlist = new List<Add_Questions_MetadataRequest>();
        List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> lstcatbyitemid = new List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId>();

        public NewQuestForm(int _itemid, string scatid = "")
        {
            InitializeComponent();
            itemid = _itemid;
            ViewCount = 0;
            SaveClick = 0;
            catid = scatid;
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
                    await Task.Run(action: () =>
                    {
                        var result = QuestSyncAPIMethods.AssignControlsAsync(App.Isonline, Convert.ToString(itemid), Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                        lst_NewQuestFormFields = result.Result;


                        var getInfoDepeResult = QuestSyncAPIMethods.GetItemInfoDependency(App.Isonline, "0", itemid.ToString(), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, "0");
                        lstItemInfoDependancy = getInfoDepeResult.Result;
                    });

                    for (int i = 0; i < lst_NewQuestFormFields.Count; i++)
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
                        Label1.Text = lst_NewQuestFormFields[i].strItemInfoFieldDesc;//strItemInfoFieldName
                        Label1.HorizontalOptions = LayoutOptions.Start;
                        Label1.FontSize = 16;
                        Label1.FontFamily = "Soin Sans Neue";



                        layout1.Children.Add(Label1);
                        layout.Children.Add(layout1);

                        var Rightlayout = new StackLayout();
                        Rightlayout.HorizontalOptions = LayoutOptions.Start;

                        Picker pk = new Picker();
                        pk.WidthRequest = 200;
                        pk.TextColor = Color.Gray;

                        DatePicker DO = new DatePicker();
                        DO.WidthRequest = 200;
                        DO.Date = Convert.ToDateTime("01/01/1900");
                        DO.Format = "MM/dd/yyyy";
                        DO.TextColor = Color.Gray;

                        Entry ST = new Entry();
                        ST.WidthRequest = 200;
                        ST.FontSize = 16;
                        ST.FontFamily = "Soin Sans Neue";

                        ST.Keyboard = Keyboard.Default;

                        Entry MN = new Entry();
                        MN.WidthRequest = 200;
                        MN.FontSize = 16;
                        MN.Keyboard = Keyboard.Numeric;
                        MN.FontFamily = "Soin Sans Neue";

                        Entry SN = new Entry();
                        SN.WidthRequest = 150;
                        SN.FontSize = 16;
                        SN.Keyboard = Keyboard.Numeric;
                        SN.FontFamily = "Soin Sans Neue";

                        BorderEditor TA = new BorderEditor();
                        TA.HeightRequest = 100;
                        TA.WidthRequest = 200;
                        TA.BorderWidth = 1;
                        TA.CornerRadius = 5;
                        TA.FontSize = 16;
                        TA.BorderColor = Color.LightGray;
                        TA.FontFamily = "Soin Sans Neue";

                        TA.Keyboard = Keyboard.Default;

                        BorderEditor ET = new BorderEditor();
                        ET.HeightRequest = 100;
                        ET.WidthRequest = 200;
                        ET.FontSize = 16;
                        ET.Keyboard = Keyboard.Default;
                        ET.FontFamily = "Soin Sans Neue";


                        Entry CL = new Entry();
                        CL.WidthRequest = 200;
                        CL.FontSize = 16;
                        CL.IsEnabled = false;
                        CL.FontFamily = "Soin Sans Neue";


                        switch (lst_NewQuestFormFields[i].strFieldType.ToLower())
                        {
                            case "se":
                            case "el":
                            case "me":
                                List<GetExternalDatasourceByIDResponse.ExternalDataSource> view = new List<GetExternalDatasourceByIDResponse.ExternalDataSource>();
                                GetExternalDatasourceByIDResponse.ExternalDataSource dt = new GetExternalDatasourceByIDResponse.ExternalDataSource();
                                dt.strObjectID = "-1";
                                dt.strName = "-- Select Item --";
                                dt.strDescription = "-- Select Item --";


                                //var ExternalCall = QuestAPIMethods.GetExternalDatasourceByID(Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID));
                                //var ExternalResponse = ExternalCall.GetValue("ResponseContent");
                                //view = JsonConvert.DeserializeObject<List<GetExternalDatasourceInfoByIDResponse.ExternalDataSource>>(ExternalResponse.ToString());
                                //var ExternalCall = QuestAPIMethods.GetExternalDatasourceByID(Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID));
                                if (App.Isonline)
                                {
                                    List<GetItemInfoDependencyResponse.ItemInfoDependancy> infoFieldChild = lstItemInfoDependancy?.Where(t => t.intItemInfoFieldIDChild == lst_NewQuestFormFields[i].intItemInfoFieldID).ToList();

                                    if (infoFieldChild?.Count < 1)
                                    {
                                        var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                                        view = ExternalCall.Result;
                                    }
                                }
                                else
                                {
                                    var ExternalCall = QuestSyncAPIMethods.GetExternalDatasourceByID(App.Isonline, Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(itemid));
                                    view = ExternalCall.Result;
                                }

                                view.Insert(0, dt);
                                pk.ItemsSource = view;
                                pk.TextColor = Color.Gray;

                                pk.ItemDisplayBinding = new Binding("strName");
                                pk.SelectedIndex = 0;
                                pk.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                pk.SelectedIndexChanged += Pk_SelectedIndexChanged;
                                Rightlayout.Children.Add(pk);
                                break;

                            case "do":
                                DO.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {

                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        DO.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(DO);
                                break;
                            case "et":
                                TA.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;

                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        TA.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(TA);
                                break;
                            case "st":
                                ST.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        ST.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(ST);
                                break;
                            case "mn":
                                MN.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        MN.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(MN);
                                break;
                            case "dt":
                                DO.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        DO.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(DO);
                                break;
                            case "ta":
                                TA.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        TA.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(TA);
                                break;
                            case "sn":
                                SN.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        DO.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(SN);
                                break;
                            case "cl":
                                CL.StyleId = lst_NewQuestFormFields[i].strFieldType.ToLower() + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID;
                                try
                                {
                                    if (!(lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("C") || lst_NewQuestFormFields[i].FIELD_SECURITY.ToUpper().Contains("OPEN")))
                                    {
                                        DO.IsEnabled = false;
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                Rightlayout.Children.Add(CL);
                                break;
                            default:
                                //        if (EntityLists.AssocType[i].AssocMetaDataText.Count != 0)
                                //            Label2.Text = EntityLists.AssocType[i].AssocMetaDataText[0].TextValue;
                                break;
                        }
                        layout.Children.Add(Rightlayout);
                        DynamicFields.Children.Add(layout);
                    }

                    List<Getitemcategory> itemcategories = new List<Getitemcategory>();
                    var getitemcategoryCall = QuestSyncAPIMethods.GetItemCategoriesByItemID(App.Isonline, Convert.ToString(itemid), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, catid);
                    getitemcategoryCall.Wait();

                    var tempgetitemcategory = getitemcategoryCall.Result;
                    GetItemCategoriesByItemIDResponse.ItemCategoryByItemId itm = new GetItemCategoriesByItemIDResponse.ItemCategoryByItemId();
                    int ii = 0;
                    if (getitemcategoryCall.Result != null)
                    {
                        foreach (var item in getitemcategoryCall?.Result)
                        {
                            var getitemcategoryscrore = QuestSyncAPIMethods.GetItemQuestionFieldsByItemCategoryIDviewscores(App.Isonline, Convert.ToString(item.intItemCategoryID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
                            var tempscores = getitemcategoryscrore.Result;
                            getitemcategoryscrore.Wait();

                            if (getitemcategoryscrore.Result != null)
                            {
                                getitemcategoryCall.Result[ii++].strAvaliablePoints = Convert.ToString(getitemcategoryscrore.Result.Where(a => a.intItemCategoryID == item.intItemCategoryID).Sum(v => v.dcPointsAvailable));

                                avpoints += Convert.ToDouble(getitemcategoryscrore.Result.Where(a => a.intItemCategoryID == item.intItemCategoryID).Sum(v => v.dcPointsAvailable));
                            }
                        }
                        listdata.ItemsSource = getitemcategoryCall.Result;
                        lstcatbyitemid = getitemcategoryCall.Result;
                        overallpoints.Text = "Overall Points Available:-" + Convert.ToString(avpoints);
                        StaticFooter.IsVisible = true;

                        int heightRowList = 90;
                        int iq = (getitemcategoryCall.Result.Count * heightRowList);
                        listdata.HeightRequest = iq;
                    }
                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);


                }
                catch (Exception ex)
                {

                }
            }
        }

        private void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker cnt = (Picker)sender;
            if (App.Isonline)
            {
                listdata.IsEnabled = false;

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

                listdata.IsEnabled = true;

            }
        }

        private void RefreshDropDownsAndLookUp(int itemInfofieldId)
        {
            // using (SEMonitoredScope scope = new SEMonitoredScope("BP.Quest", "_NewForm - RefreshDropDownsAndLookUp"))
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

                        List<GetExternalDatasourceInfoByIDResponse.ExternalDataSource> l = new List<GetExternalDatasourceInfoByIDResponse.ExternalDataSource>();

                        GetExternalDatasourceInfoByIDResponse.ExternalDataSource ed = new GetExternalDatasourceInfoByIDResponse.ExternalDataSource
                        {
                            strObjectID = "-1",
                            strName = "-- Select Item --",
                            strDescription = "-- Select Item --"
                        };

                        l.Insert(0, ed);
                        drp.ItemsSource = l;
                        drp.SelectedIndex = 0;
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
                                List<GetExternalDatasourceInfoByIDResponse.ExternalDataSource> l = JsonConvert.DeserializeObject<List<GetExternalDatasourceInfoByIDResponse.ExternalDataSource>>(qResult.ToString());
                                GetExternalDatasourceInfoByIDResponse.ExternalDataSource ed = new GetExternalDatasourceInfoByIDResponse.ExternalDataSource();

                                ed.strObjectID = "-1";
                                ed.strName = "-- Select Item --";
                                ed.strDescription = "-- Select Item --";
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



        public object FindQuestControl(string type)
        {
            foreach (StackLayout v in DynamicFields.Children)
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


                for (int i = 0; i < lst_NewQuestFormFields.Count; i++)
                {
                    AddFormRequest.InfoFieldValues iValue = new AddFormRequest.InfoFieldValues();
                    infoobj = new AddFormRequest.ItemInfoFieldInfo();
                    var _field_type = lst_NewQuestFormFields[i].strFieldType.ToLower();
                    switch (_field_type)
                    {
                        case "se":
                        case "el":
                        case "me":
                            var cnt = FindQuestControl((_field_type) + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID);
                            Type cnt_type = cnt.GetType();
                            var pick_Ext_datasrc = new Picker();
                            if (cnt_type.Name.ToLower() == "picker")
                            {
                                pick_Ext_datasrc = (Picker)cnt;
                            }

                            if (lst_NewQuestFormFields[i].strIsRequired == "Y")
                            {


                                if (Convert.ToInt32(pick_Ext_datasrc.SelectedIndex) == 0)
                                {
                                    DisplayAlert("Field Required.", "Please enter valid data in " + lst_NewQuestFormFields[i].strItemInfoFieldName, "OK");
                                    RequiredFieldCount = RequiredFieldCount + 1;
                                    goto requiredJump;
                                }
                                else
                                {
                                    if (pick_Ext_datasrc.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                                    {
                                        pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource;
                                        strobjid = Convert.ToString(pickervalue.strObjectID);
                                        if (pickervalue == null)
                                        {
                                            pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource;
                                        }
                                    }
                                    else
                                    {
                                        pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource;
                                        strobjid = Convert.ToString(pickervalue.strObjectID);
                                        if (pickervalue == null)
                                        {
                                            pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource;
                                        }

                                    }
                                    iValue.ItemInfoFieldText = pickervalue.strName;
                                    //iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(strobjid);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                                }
                            }
                            else
                            {

                                if (pick_Ext_datasrc.SelectedItem?.GetType()?.FullName == "StemmonsMobile.DataTypes.DataType.Quest.GetExternalDatasourceByIDResponse+ExternalDataSource")
                                {

                                    pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceByIDResponse.ExternalDataSource;
                                    strobjid = Convert.ToString(pickervalue.strObjectID);
                                    if (pickervalue == null)
                                    {
                                        pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource;
                                    }
                                }
                                else
                                {
                                    pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource;
                                    strobjid = Convert.ToString(pickervalue.strObjectID);
                                    if (pickervalue == null)
                                    {
                                        pickervalue = pick_Ext_datasrc.SelectedItem as GetExternalDatasourceInfoByIDResponse.ExternalDataSource;
                                    }

                                }

                                iValue.ItemInfoFieldText = pickervalue.strName;
                                //iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i]);
                                iValue.externalDatasourceObjectIDs = Convert.ToString(strobjid);
                                infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                            }

                            break;

                        case "do":
                        case "st":
                        case "mn":
                        case "dt":
                        case "ta":
                        case "sn":
                        case "et":
                        case "cl":

                            cnt = FindQuestControl((_field_type) + "_" + lst_NewQuestFormFields[i].intItemInfoFieldID);
                            cnt_type = cnt.GetType();
                            var date_pick = new DatePicker();
                            var ent = new Entry();
                            ent.FontSize = 16;
                            var edit = new BorderEditor();
                            edit.FontSize = 16;
                            if (cnt_type.Name.ToLower() == "datepicker")
                            {
                                date_pick = (DatePicker)cnt;
                                if (date_pick.Date != Convert.ToDateTime("01/01/1900"))
                                {
                                    iValue.ItemInfoFieldText += App.DateFormatStringToString(date_pick.Date.ToString(), CultureInfo.InvariantCulture.DateTimeFormat.ShortDatePattern, "MM/dd/yyyy");
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                                }
                            }
                            else if (cnt_type.Name.ToLower() == "entry")
                            {
                                ent = (Entry)cnt;

                                if (lst_NewQuestFormFields[i].strIsRequired == "Y")
                                {

                                    if (string.IsNullOrEmpty(ent.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + lst_NewQuestFormFields[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        iValue.ItemInfoFieldText = ent.Text;
                                        iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                        infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                                    }
                                }
                                else
                                {
                                    iValue.ItemInfoFieldText = ent.Text;
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                                }
                            }
                            else if (cnt_type.Name.ToLower() == "bordereditor")
                            {
                                edit = (BorderEditor)cnt;


                                if (lst_NewQuestFormFields[i].strIsRequired == "Y")
                                {

                                    if (string.IsNullOrEmpty(edit.Text))
                                    {
                                        DisplayAlert("Field Required.", "Please enter valid data in " + lst_NewQuestFormFields[i].strItemInfoFieldName, "OK");
                                        RequiredFieldCount = RequiredFieldCount + 1;
                                        goto requiredJump;
                                    }
                                    else
                                    {
                                        iValue.ItemInfoFieldText = edit.Text;
                                        iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                        infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                                    }
                                }
                                else
                                {
                                    iValue.ItemInfoFieldText = edit.Text;
                                    iValue.externalDatasourceObjectIDs = Convert.ToString(lst_NewQuestFormFields[i].intExternalDatasourceID);
                                    infoobj.ItemInfoFieldId = (Convert.ToInt32(lst_NewQuestFormFields[i].intItemInfoFieldID));
                                }

                            }

                            break;
                        default:
                            //        if (EntityLists.AssocType[i].AssocMetaDataText.Count != 0)
                            //            Label2.Text = EntityLists.AssocType[i].AssocMetaDataText[0].TextValue;
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
                        var ApiCallAddForm = QuestSyncAPIMethods.StoreAndcreate(App.Isonline, int.Parse(itemid), addForm, "", Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, listdata.ItemsSource);
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
                this.Navigation.PopAsync();
            }
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
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

                this.Navigation.PushAsync(new QuestQuestionForm(sd.strItemCategoryName, Convert.ToString(sd.intItemCategoryID), ItemInstanceTranId, Convert.ToString(sd.intItemID), catid, addForm, lstcatbyitemid));
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