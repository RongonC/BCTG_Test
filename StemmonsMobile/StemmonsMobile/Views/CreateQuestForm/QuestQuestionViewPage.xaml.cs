using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Quest;
using Xamarin.Forms;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemQuestionDecodeByFieldIDResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemQuestionFieldsByItemCategoryIDResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemQuestionMetadataResponse;

namespace StemmonsMobile.Views.CreateQuestForm
{
    public partial class QuestQuestionViewPage : ContentPage
    {
        List<ItemQuestionDecode> standardMeet = new List<ItemQuestionDecode>();
        List<ItemQuestionDecode> standardMeetCopy = new List<ItemQuestionDecode>();
        List<GetItemQuestionMetadataResponse.ItemQuestionMetaData> listdata = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();
        ItemQuestionDecode Questiondecodevalues = new ItemQuestionDecode();
        List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase> extraNotes = new List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>();
        List<string> CommentsList = new List<string>();
        List<string> CommentsListId = new List<string>();
        List<string> MakeCaseList = new List<string>();
        List<string> MakeCaseId = new List<string>();
        string ItemId = string.Empty;
        string ItemQuestionFieldIdAttachment = string.Empty;
        string iteminstancetranid, ItemCategoryID;
        bool isedit = false;
        int ViewCount;
        int selectedfieldId, removeclick;
        int responseStatus = 0;
        Picker pk = new Picker();
        string scatid = "";
        string sQuestionMetadatId = "";
        string caseTypeId = string.Empty;


        List<string> pItemQuestionfieldid = new List<string>();
        List<string> pCASEREQUESTED = new List<string>();
        List<string> pMEETS_STANDARDS = new List<string>();
        List<string> pNOTES = new List<string>();



        public QuestQuestionViewPage(string instanceTranId, string ItemCategoryIDdata, string itemid, bool isEdit, string catid = "", string QuestionMetadatId = "")
        {
            InitializeComponent();
            ItemCategoryID = ItemCategoryIDdata;
            iteminstancetranid = instanceTranId;
            isedit = isEdit;
            CommentsList = new List<string>();
            standardMeet = new List<ItemQuestionDecode>();
            listdata = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();
            Questiondecodevalues = new ItemQuestionDecode();
            extraNotes = new List<GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase>();
            selectedfieldId = 0;
            ItemId = itemid;
            removeclick = 0;
            CommentsListId = new List<string>();

            MakeCaseList = new List<string>();
            MakeCaseId = new List<string>();
            ViewCount = 0;
            scatid = catid;
            sQuestionMetadatId = QuestionMetadatId;
        }


        //dynamic page code
        protected override void OnAppearing()
        {
            try
            {

                base.OnAppearing();
                App.SetConnectionFlag();

                if (ViewCount == 0)
                {
                    DynamicViewCall();
                }
            }
            catch (Exception ex)
            {

            }

        }



        async void DynamicViewCall()
        {
            try
            {
                pItemQuestionfieldid = new List<string>();
                pCASEREQUESTED = new List<string>();
                pMEETS_STANDARDS = new List<string>();
                pNOTES = new List<string>();

                ViewCount = ViewCount + 1;
                DynamicView.Children.Clear();
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                DynamicView.Opacity = 0;
                listdata = new List<GetItemQuestionMetadataResponse.ItemQuestionMetaData>();

                if (Functions.questObjectData != null)
                {
                    pItemQuestionfieldid = stringArray(Functions.questObjectData.pITEM_QUESTION_FIELD_IDs);
                    pCASEREQUESTED = stringArray(Functions.questObjectData.pIS_CASE_REQUESTED);
                    pMEETS_STANDARDS = stringArray(Functions.questObjectData.pMEETS_STANDARDS);
                    pNOTES = stringArray(Functions.questObjectData.pNOTES);
                }

                await Task.Run(() =>
                 {
                     var ApiCall = QuestSyncAPIMethods.GetItemQuestionMetaData(App.Isonline, Convert.ToString(iteminstancetranid), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemId, "V", scatid);
                     ApiCall.Wait();

                     listdata = ApiCall.Result;
                 });

                caseTypeId = listdata.Find(v => v.strCaseTypeId != null)?.strCaseTypeId;

                listdata = listdata.FindAll(v => v.intItemCategoryID == Convert.ToInt32(ItemCategoryID));

                for (int i = 0; i < listdata.Count; i++)
                {
                    var QuestionLayout = new StackLayout();
                    QuestionLayout.Margin = new Thickness(0, 10, 0, 0);
                    selectedfieldId = i;

                    Label Question = new Label();
                    Question.FontSize = 16;
                    Question.Text = listdata[i].strQuestion;

                    Question.Margin = new Thickness(10, 5, 5, 0);
                    QuestionLayout.Children.Add(Question);


                    Label selectlabel = new Label();
                    selectlabel.Margin = new Thickness(10, 0, 10, 0);
                    selectlabel.Text = "Select Choice:";
                    selectlabel.FontSize = 12;
                    QuestionLayout.Children.Add(selectlabel);

                    standardMeet = new List<ItemQuestionDecode>();
                    pk = new Picker();
                    pk.TextColor = Color.LightGray;
                    var AnswerApiCall = QuestSyncAPIMethods.GetItemQuestionDecodeByFieldID(App.Isonline, Convert.ToString(listdata[i].intItemQuestionFieldID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(ItemId), Convert.ToString(listdata[i].intItemInstanceTranID), Convert.ToString(listdata[i].intItemCategoryID), scatid);
                    standardMeet = AnswerApiCall.Result;
                    // standardMeet.Add(dt);

                    if (!isedit)
                    {
                        pk.IsEnabled = false;
                    }
                    standardMeet.Add(new ItemQuestionDecode
                    {
                        intItemQuestionFieldID = 0,
                        dcPointsAvailable = 0,
                        strMeetsStandards = "--- Select Item ---"
                    });

                    pk.ItemsSource = standardMeet;
                    pk.Margin = new Thickness(10, 0, 10, 0);
                    pk.ItemDisplayBinding = new Binding("strMeetsStandards");
                    pk.StyleId = "PK_" + listdata[i].intItemQuestionFieldID;

                    pk.SelectedIndexChanged += Pk_SelectedIndexChanged;
                    QuestionLayout.Children.Add(pk);

                    var pointslabel = new Label();
                    pointslabel.FontSize = 16;
                    pointslabel.Margin = new Thickness(10, 0, 10, 0);
                    string pointsEarned = "";

                    pointsEarned = "0";

                    string points = "Points Available:" + listdata[i].PointsAvail + " Earned:" + listdata[i].PointsEarned;
                    pointslabel.Text = points;
                    pointslabel.FontSize = 12;
                    pointslabel.StyleId = "lb_" + listdata[i].intItemQuestionFieldID;
                    pointslabel.HorizontalTextAlignment = TextAlignment.End;
                    QuestionLayout.Children.Add(pointslabel);

                    var CommentLayout = new StackLayout();
                    var TA = new Entry();
                    TA.Margin = new Thickness(10, 0, 10, 0);
                    TA.Placeholder = "Write Note..";
                    TA.FontSize = 16;
                    TA.StyleId = "TA_" + listdata[i].intItemQuestionFieldID;
                    CommentLayout.Children.Add(TA);

                    var grid = new Grid();
                    grid.Margin = new Thickness(10, 5, 5, 0);
                    grid.RowDefinitions.Add(new RowDefinition());

                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.ColumnDefinitions.Add(new ColumnDefinition());

                    Button MakeCase = new Button();
                    MakeCase.BackgroundColor = Color.Transparent;
                    MakeCase.TextColor = Color.Blue;
                    MakeCase.FontSize = 13;
                    MakeCase.Text = "Make Case";
                    MakeCase.Image = "Assets/Unchecked.png";
                    MakeCase.StyleId = "makecase_" + listdata[i].intItemQuestionFieldID;
                    if (!isedit)
                    {
                        MakeCase.IsEnabled = false;
                    }
                    MakeCase.Clicked += MakeCase_Clicked;
                    MakeCase.CommandParameter = listdata[i];
                    grid.Children.Add(MakeCase, 0, 0);

                    Button Attachment = new Button();
                    Attachment.BackgroundColor = Color.Transparent;
                    Attachment.TextColor = Color.Blue;
                    Attachment.Text = "Attachment";
                    Attachment.FontSize = 13;
                    Attachment.Clicked += Attachment_Clicked;
                    Attachment.CommandParameter = listdata[i];
                    grid.Children.Add(Attachment, 1, 0);

                    Button NewLine = new Button();
                    NewLine.BackgroundColor = Color.Transparent;
                    NewLine.TextColor = Color.Blue;
                    NewLine.Text = "Add Comment";
                    NewLine.FontSize = 13;
                    if (!isedit)
                    {
                        NewLine.IsEnabled = false;
                    }
                    //NewLine.Clicked += NewLine_Clicked;
                    NewLine.CommandParameter = listdata[i];
                    //  grid.Children.Add(NewLine, 2, 0);

                    CommentLayout.Children.Add(grid);
                    QuestionLayout.Children.Add(CommentLayout);


                    var AttachmentDLayout = new StackLayout();
                    AttachmentDLayout.Margin = new Thickness(10, 5, 0, 0);

                    if (!string.IsNullOrEmpty(listdata[i].strNotes))
                    {

                        Label ExtraNotes = new Label();
                        ExtraNotes.Text = listdata[i].strNotes;
                        ExtraNotes.FontSize = 16;
                        ExtraNotes.Margin = new Thickness(10, 5, 5, 0);
                        QuestionLayout.Children.Add(ExtraNotes);

                        MakeCaseId.Add(Convert.ToString(listdata[i].intItemQuestionFieldID));
                        MakeCaseList.Add(listdata[i].strIsCaseRequested.TrimEnd());

                        CommentsListId.Add(Convert.ToString(listdata[i].intItemQuestionFieldID));
                        CommentsList.Add(listdata[i].strNotes);



                        Button ExtraViewCase = new Button();
                        ExtraViewCase.BackgroundColor = Color.Transparent;
                        ExtraViewCase.TextColor = Color.Blue;
                        ExtraViewCase.Text = "ViewCase";
                        ExtraViewCase.FontSize = 13;
                        if (listdata[i].intCaseID == null || listdata[i].intCaseID == 0)
                        {
                            ExtraViewCase.IsEnabled = false;
                        }
                        ExtraViewCase.Clicked += ExtraViewCase_Clicked;
                        ExtraViewCase.CommandParameter = listdata[i];
                        ExtraViewCase.Margin = new Thickness(15, 0, 0, 0);
                        AttachmentDLayout.Children.Add(ExtraViewCase);



                        Button ExtraAttachment = new Button();
                        ExtraAttachment.BackgroundColor = Color.Transparent;
                        ExtraAttachment.TextColor = Color.Blue;
                        ExtraAttachment.Text = "Attachment";
                        ExtraAttachment.FontSize = 13;
                        ExtraAttachment.Margin = new Thickness(20, 0, 0, 0);
                        ExtraAttachment.HorizontalOptions = LayoutOptions.CenterAndExpand;
                        ExtraAttachment.Clicked += ExtraAttachment_Clicked;
                        ExtraAttachment.CommandParameter = listdata[i];


                        AttachmentDLayout.Children.Add(ExtraAttachment);


                        Button ExtraRemove = new Button();
                        ExtraRemove.BackgroundColor = Color.Transparent;
                        ExtraRemove.TextColor = Color.Blue;
                        ExtraRemove.Text = "Remove";
                        ExtraRemove.FontSize = 13;
                        ExtraRemove.Margin = new Thickness(0, 0, 15, 0);
                        ExtraRemove.HorizontalOptions = LayoutOptions.EndAndExpand;
                        ExtraRemove.Clicked += ExtraRemove_Clicked;
                        ExtraRemove.CommandParameter = listdata[i];

                        AttachmentDLayout.Children.Add(ExtraRemove);




                        AttachmentDLayout.Orientation = StackOrientation.Horizontal;
                        AttachmentDLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                        QuestionLayout.Children.Add(AttachmentDLayout);

                    }
                    else
                    {
                        if (listdata[i].intCaseID != null)
                        {
                            Button ExtraViewCase = new Button();
                            ExtraViewCase.BackgroundColor = Color.Transparent;
                            ExtraViewCase.TextColor = Color.Blue;
                            ExtraViewCase.Text = "ViewCase";
                            ExtraViewCase.FontSize = 13;
                            if (listdata[i].intCaseID == null || listdata[i].intCaseID == 0)
                            {
                                ExtraViewCase.IsEnabled = false;
                            }
                            ExtraViewCase.Clicked += ExtraViewCase_Clicked;
                            ExtraViewCase.CommandParameter = listdata[i];
                            AttachmentDLayout.Children.Add(ExtraViewCase);
                            AttachmentDLayout.Orientation = StackOrientation.Horizontal;
                            AttachmentDLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

                            QuestionLayout.Children.Add(AttachmentDLayout);

                        }
                        else if (listdata[i].intCaseID == null && (listdata[i].strIsCaseRequested).TrimEnd() == "Y")
                        {


                            MakeCaseId.Add(Convert.ToString(listdata[i].intItemQuestionFieldID));
                            MakeCaseList.Add("Y");

                            CommentsListId.Add(Convert.ToString(listdata[i].intItemQuestionFieldID));
                            CommentsList.Add("");


                            Button ExtraViewCase = new Button();
                            ExtraViewCase.BackgroundColor = Color.Transparent;
                            ExtraViewCase.TextColor = Color.Blue;
                            ExtraViewCase.Text = "ViewCase";
                            ExtraViewCase.FontSize = 13;

                            ExtraViewCase.IsEnabled = false;

                            ExtraViewCase.Clicked += ExtraViewCase_Clicked;
                            ExtraViewCase.CommandParameter = listdata[i];
                            AttachmentDLayout.Children.Add(ExtraViewCase);
                            AttachmentDLayout.Orientation = StackOrientation.Horizontal;
                            AttachmentDLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

                            QuestionLayout.Children.Add(AttachmentDLayout);
                        }

                    }
                    ItemQuestionFieldIdAttachment = Convert.ToString(listdata[i].intItemQuestionFieldID);


                    var result = QuestSyncAPIMethods.GetItemQuestionMetadataCase(App.Isonline, Convert.ToString(listdata[i].intItemQuestionMetadataID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemId, iteminstancetranid, ItemCategoryID, Convert.ToInt32(listdata[i].intItemQuestionFieldID), scatid);
                    extraNotes = result.Result;


                    if (extraNotes != null)
                    {

                        for (int ik = 0; ik < extraNotes.Count; ik++)
                        {
                            Label ExtraNotes = new Label();
                            ExtraNotes.Text = Convert.ToString(extraNotes[ik].strNotes);
                            ExtraNotes.FontSize = 16;
                            ExtraNotes.Margin = new Thickness(10, 5, 10, 0);
                            QuestionLayout.Children.Add(ExtraNotes);

                            CommentsListId.Add(Convert.ToString(listdata[i].intItemQuestionFieldID));
                            CommentsList.Add(extraNotes[ik].strNotes);


                            var ExtraAttachmentDLayout = new StackLayout();
                            ExtraAttachmentDLayout.Margin = new Thickness(10, 5, 0, 0);


                            Button ExtraViewCase = new Button();
                            ExtraViewCase.BackgroundColor = Color.Transparent;
                            ExtraViewCase.TextColor = Color.Blue;
                            ExtraViewCase.Text = "ViewCase";
                            ExtraViewCase.FontSize = 13;

                            if (extraNotes[ik].intCaseID == null || extraNotes[ik].intCaseID == 0)
                            {
                                ExtraViewCase.IsEnabled = false;
                            }
                            ExtraViewCase.Clicked += ExtraViewCase1_Clicked;
                            ExtraViewCase.CommandParameter = extraNotes[ik];
                            ExtraViewCase.Margin = new Thickness(15, 0, 0, 0);

                            Button ExtraAttachment = new Button();
                            ExtraAttachment.BackgroundColor = Color.Transparent;
                            ExtraAttachment.TextColor = Color.Blue;
                            ExtraAttachment.Text = "Attachment";
                            ExtraAttachment.FontSize = 13;
                            if (isedit)
                            {
                                ExtraAttachment.IsEnabled = true;
                            }
                            else
                            {
                                ExtraAttachment.IsEnabled = false;
                            }
                            ExtraAttachment.Clicked += ExtraAttachment1_Clicked;
                            ExtraAttachment.CommandParameter = extraNotes[ik];
                            ExtraAttachment.HorizontalOptions = LayoutOptions.CenterAndExpand;
                            ExtraAttachment.Margin = new Thickness(20, 0, 0, 0);


                            Button ExtraRemove = new Button();
                            ExtraRemove.BackgroundColor = Color.Transparent;
                            ExtraRemove.TextColor = Color.Blue;
                            ExtraRemove.Text = "Remove";
                            ExtraRemove.FontSize = 13;
                            if (isedit)
                            {
                                ExtraRemove.IsEnabled = true;
                            }
                            else
                            {
                                ExtraRemove.IsEnabled = false;
                            }
                            ExtraRemove.Clicked += ExtraRemove1_Clicked;
                            ExtraRemove.CommandParameter = extraNotes[ik];
                            ExtraRemove.HorizontalOptions = LayoutOptions.EndAndExpand;
                            ExtraRemove.Margin = new Thickness(0, 0, 15, 0);


                            ExtraAttachmentDLayout.Children.Add(ExtraViewCase);
                            ExtraAttachmentDLayout.Children.Add(ExtraAttachment);
                            ExtraAttachmentDLayout.Children.Add(ExtraRemove);

                            ExtraAttachmentDLayout.Orientation = StackOrientation.Horizontal;
                            ExtraAttachmentDLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                            QuestionLayout.Children.Add(ExtraAttachmentDLayout);
                        }
                    }


                    Label devider = new Label();
                    devider.BackgroundColor = Color.Black;
                    devider.HeightRequest = 1;
                    devider.Margin = new Thickness(10, 0, 3, 0);

                    QuestionLayout.Children.Add(devider);
                    DynamicView.Children.Add(QuestionLayout);

                    if (pMEETS_STANDARDS.Count > 0)
                    {
                        for (int j = 0; j < standardMeet.Count; j++)
                        {
                            if (pMEETS_STANDARDS[i] == standardMeet[j].strMeetsStandards)
                            {
                                pk.SelectedIndex = j;
                                break;
                            }
                            else
                            {
                                pk.SelectedIndex = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < standardMeet.Count; j++)
                        {
                            if (listdata[i].strMeetsStandards == standardMeet[j].strMeetsStandards)
                            {
                                pk.SelectedIndex = j;
                                break;
                            }
                            else
                            {
                                pk.SelectedIndex = 1;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            DynamicView.Opacity = 1;
        }

        public List<string> stringArray(string parameter)
        {

            string[] objectvalues = parameter.Split(',');
            List<string> convertStringToList = new List<string>(objectvalues);

            return convertStringToList;
        }
        //Remove Checked icon after adding Cooment
        void UncheckMakeCase()
        {
            try
            {
                for (int i = 0; i < listdata.Count; i++)
                {
                    var cnt1 = findControl("makecase_" + listdata[i].intItemQuestionFieldID);

                    Type cnt_type1 = cnt1.GetType();
                    var MakeCase = new Button();
                    MakeCase = (Button)cnt1;

                    MakeCase.Image = "Assets/Unchecked.png";
                }
            }
            catch (Exception ex)
            {

            }
        }


        //Find control
        public object findControl(string type)
        {
            try
            {
                foreach (StackLayout v in DynamicView.Children)
                {
                    foreach (var item in v.Children)
                    {
                        if (item.GetType().ToString() == "Xamarin.Forms.StackLayout")
                        {
                            StackLayout st = (StackLayout)item;
                            foreach (var item1 in st.Children)
                            {
                                if (item1.GetType().ToString() == "Xamarin.Forms.Grid")
                                {
                                    Grid grd = (Grid)item1;
                                    foreach (var item2 in grd.Children)
                                    {
                                        var xy = item2.StyleId;

                                        if (xy != null)
                                        {
                                            if (xy.Contains(type))
                                            {
                                                return item2;
                                            }
                                        }
                                    }

                                }
                                else
                                {

                                    var xy = item1.StyleId;

                                    if (xy != null)
                                    {
                                        if (xy.Contains(type))
                                        {
                                            return item1;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var xy = item.StyleId;

                            if (xy != null)
                            {
                                if (xy.Contains(type))
                                {
                                    return item;
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

        void SaveComment()
        {
            try
            {
                for (int i = 0; i < listdata.Count; i++)
                {
                    int CommentCount = 0;
                    var cnt1 = findControl("TA_" + listdata[i].intItemQuestionFieldID);
                    if (cnt1 != null)
                    {
                        Type cnt_type1 = cnt1.GetType();
                        var Comment = new Entry();
                        Comment = (Entry)cnt1;
                        string commentdata = "";
                        if (cnt_type1.Name.ToLower() == "entry")
                        {
                            commentdata = Comment.Text;
                        }

                        Comment.Unfocus();

                        string makecasevalue = makecaseacknowledgement("makecase_" + listdata[i].intItemQuestionFieldID);


                        if (makecasevalue.ToUpper() != "N" && !string.IsNullOrEmpty(Comment.Text) || (makecasevalue.ToUpper() == "N" && !string.IsNullOrEmpty(Comment.Text)) || (makecasevalue.ToUpper() == "Y" && string.IsNullOrEmpty(Comment.Text)))
                        {
                            CommentsList.Add(Comment.Text);
                            CommentsListId.Add(Convert.ToString(listdata[i].intItemQuestionFieldID));
                        }

                        if (CommentsListId.Contains(Convert.ToString(listdata[i].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < CommentsListId.Count; j++)
                            {
                                if (CommentsListId[j] == Convert.ToString(listdata[i].intItemQuestionFieldID))
                                {
                                    CommentCount = CommentCount + 1;
                                }
                            }
                        }

                        if (makecasevalue.ToUpper() != "N" && !string.IsNullOrEmpty(Comment.Text) || (makecasevalue.ToUpper() == "N" && !string.IsNullOrEmpty(Comment.Text)) || (makecasevalue.ToUpper() == "Y" && string.IsNullOrEmpty(Comment.Text)))
                        {
                            if (CommentCount > 1)
                            {
                                GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase obj = new GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase()
                                {
                                    strNotes = commentdata,
                                    intItemQuestionMetadataID = listdata[i].intItemQuestionMetadataID,
                                    usreName = Functions.UserName,
                                    itemInstanceTranID = iteminstancetranid,
                                    strCaseTypeID = caseTypeId
                                };
                                UpdateCaseNotesToQuestionRequest objcasenotes = new UpdateCaseNotesToQuestionRequest()
                                {
                                    notes = commentdata,
                                    modifiedBy = Functions.UserName,
                                    itemQuestionMetadataCaseID = listdata[i].intItemQuestionMetadataID,
                                };
                                var APICall = QuestSyncAPIMethods.UpdateCaseNotesToQuestion(App.Isonline, iteminstancetranid, ItemId, commentdata, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, objcasenotes, obj);

                                AddCaseToQuestionRequest objAddCaseToQuestion = new AddCaseToQuestionRequest()
                                {
                                    createdBy = Functions.UserName,
                                    itemInstanceTranID = Convert.ToInt32(iteminstancetranid),
                                    itemQuestionFieldID = Convert.ToInt32(listdata[i].intItemQuestionFieldID),
                                    notes = commentdata != null ? commentdata : "",
                                    IsCaseRequested = makecasevalue
                                };

                                var APICall1 = QuestSyncAPIMethods.AddCaseToQuestion(App.Isonline, iteminstancetranid, ItemId, Convert.ToString(listdata[i].intItemQuestionFieldID), commentdata, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, obj, objAddCaseToQuestion);
                                var Response1 = APICall1.Result;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        #region Attchment Button Click
        void Attachment_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionMetaData;
                if (App.Isonline)
                {
                    this.Navigation.PushAsync(new Attachment(Convert.ToString(value.intItemQuestionFieldID), iteminstancetranid, Convert.ToString(value.intItemQuestionMetadataID)));
                }
                else
                {
                    DisplayAlert("Alert", "Attachment functionality not available in offline", "Ok");
                }

            }
            catch (Exception ex)
            { }
        }

        void ExtraAttachment_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionMetaData;
                if (App.Isonline)
                {
                    this.Navigation.PushAsync(new Attachment(Convert.ToString(value.intItemQuestionFieldID), iteminstancetranid, Convert.ToString(value.intItemQuestionMetadataID)));
                }
                else
                {
                    DisplayAlert("Alert", "Attachment functionality not available in offline", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }
        void ExtraAttachment1_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase;
                if (App.Isonline)
                {
                    this.Navigation.PushAsync(new Attachment(ItemQuestionFieldIdAttachment, iteminstancetranid, Convert.ToString(value.intItemQuestionMetadataID)));

                }
                else
                {
                    DisplayAlert("Alert", "Attachment functionality not available in offline", "Ok");
                }
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region ViewCase Region
        void ExtraViewCase_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button showcasebtn = (Button)sender;
                var value = showcasebtn.CommandParameter as ItemQuestionMetaData;
                this.Navigation.PushAsync(new Cases.ViewCasePage(Convert.ToString(value.intCaseID), value.strCaseTypeId, ""));
                // this.Navigation.PushAsync(new Attachment(Convert.ToString(value.intItemQuestionFieldID), iteminstancetranid, Convert.ToString(value.intItemQuestionMetadataID)));
            }
            catch (Exception ex)
            { }
        }

        void ExtraViewCase1_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button showcasebtn = (Button)sender;
                var value = showcasebtn.CommandParameter as GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase;
                this.Navigation.PushAsync(new Cases.ViewCasePage(Convert.ToString(value.intCaseID), value.strCaseTypeID, ""));
            }
            catch (Exception ex)
            { }
        }
        #endregion

        #region Remove Comment
        void ExtraRemove_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionMetaData;



                if (CommentsListId.Contains(Convert.ToString(value.intItemQuestionFieldID)))
                {
                    for (int j = 0; j < CommentsList.Count; j++)
                    {
                        if (Convert.ToInt16(CommentsListId[j]) == value.intItemQuestionFieldID)
                        {

                            CommentsList.RemoveAt(j);
                            CommentsListId.RemoveAt(j);
                            goto NextPage;
                        }
                    }
                }

            NextPage:
                int a = 0;
                removeclick = 1;
                saveFormCall();


            }
            catch (Exception ex)
            { }
        }

        void ExtraRemove1_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button RemoveCommentbtn = (Button)sender;
                var stack = (StackLayout)RemoveCommentbtn.Parent.Parent;
                var value = RemoveCommentbtn.CommandParameter as GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase;

                DeleteItemQuestionMetadataCaseRequest obj = new DeleteItemQuestionMetadataCaseRequest()
                {
                    itemInstanceTranID = Convert.ToInt32(iteminstancetranid),
                    itemQuestionMetadataCaseID = Convert.ToInt32(value.intItemQuestionMetadataCaseID)
                };

                dynamic DeleteAPiCall = null;
                Task.Run(() =>
               {

                   DeleteAPiCall = QuestSyncAPIMethods.DeleteItemQuestionMetadataCase(App.Isonline, iteminstancetranid, ItemId, Convert.ToString(value.intItemQuestionMetadataCaseID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, value, obj);
                   var Response = DeleteAPiCall.Result;
               });
                //var DeleteAPiCall = QuestAPIMethods.DeleteItemQuestionMetadataCase(iteminstancetranid, Convert.ToString(value.intItemQuestionMetadataCaseID));
                //var result = DeleteAPiCall.GetValue("Success");


                if (!string.IsNullOrEmpty(DeleteAPiCall.Result.ToString()))
                {
                    removeclick = 1;
                    saveFormCall();
                    DynamicViewCall();
                }
            }
            catch (Exception ex)
            { }
        }

        #endregion

        string makecaseacknowledgement(string styleid)
        {

            string returnvalue = string.Empty;
            returnvalue = "N";
            try
            {
                var cnt1 = findControl(styleid);
                if (cnt1 != null)
                {
                    Type cnt_type1 = cnt1.GetType();
                    var makecase = new Button();
                    makecase = (Button)cnt1;

                    if (makecase.Image == "Assets/Unchecked.png")
                    {
                        returnvalue = "N";
                    }
                    else
                    {
                        returnvalue = "Y";
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return returnvalue;


        }


        void MakeCase_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionMetaData;
                if (attchmentbtn.Image == "Assets/Unchecked.png")
                {
                    MakeCaseId.Add(Convert.ToString(value.intItemQuestionFieldID));
                    MakeCaseList.Add("Y");
                    attchmentbtn.Image = "Assets/Checked.png";
                }
                else
                {
                    int postion = MakeCaseId.IndexOf(Convert.ToString(value.intItemQuestionFieldID));
                    MakeCaseId.RemoveAt(postion);
                    MakeCaseList.RemoveAt(postion);
                    attchmentbtn.Image = "Assets/Unchecked.png";
                }

            }
            catch (Exception ex)
            { }
        }

        async void Save_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                if (isedit)
                {
                    SaveComment();
                }
                saveFormCall();

                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            }
            catch (Exception ex)
            {

            }
        }

        void saveFormCall()
        {
            try
            {
                //if (isedit)
                //{
                List<int> AddedComentId = new List<int>();

                List<int> AddedMakeCaseId = new List<int>();
                var iteminstancetraanid = iteminstancetranid;
                int count1 = listdata.Count;
                string itemQuestionIds = "";
                string standardmeets = "";
                string pointsavailable = "";
                string pointsearned = "";
                string caserequire = "";
                string Notes = "";
                string createdby = Functions.UserName;
                for (int i = 0; i < listdata.Count; i++)
                {

                    var cnt1 = findControl("PK_" + listdata[i].intItemQuestionFieldID);

                    Type cnt_type1 = cnt1.GetType();

                    var pick_Ext_datasrc = new Picker();
                    if (cnt_type1.Name.ToLower() == "picker")
                    {
                        pick_Ext_datasrc = (Picker)cnt1;
                    }



                    var picker_value = pick_Ext_datasrc.SelectedItem as ItemQuestionDecode;



                    var cnt2 = findControl("TA_" + listdata[i].intItemQuestionFieldID);

                    Type cnt_type2 = cnt2.GetType();
                    var ent = new Entry();
                    ent = (Entry)cnt2;


                    if (i == 0)
                    {
                        itemQuestionIds = Convert.ToString(listdata[0].intItemQuestionFieldID);


                        string meetvalue = string.Empty;
                        if (picker_value == null)
                        {
                            meetvalue = "";
                        }
                        else
                        {
                            meetvalue = picker_value.strMeetsStandards;
                        }

                        standardmeets = meetvalue;


                        if (string.IsNullOrEmpty(meetvalue))
                        {
                            pointsearned = "0";
                            pointsavailable = "0";
                        }
                        else
                        {


                            pointsearned = pointsReturn("Earned", meetvalue, Convert.ToString(listdata[0].intItemQuestionFieldID));
                            pointsavailable = pointsReturn("Avail", meetvalue, Convert.ToString(listdata[0].intItemQuestionFieldID));
                        }




                        if (MakeCaseId.Contains(Convert.ToString(listdata[0].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < MakeCaseList.Count; j++)
                            {
                                if (Convert.ToInt16(MakeCaseId[j]) == listdata[i].intItemQuestionFieldID)
                                {
                                    if (!AddedMakeCaseId.Contains(Convert.ToInt16(MakeCaseId[0])))
                                    {
                                        caserequire = MakeCaseList[j];
                                        AddedMakeCaseId.Add(Convert.ToInt16(MakeCaseId[0]));
                                        goto NextPage1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            caserequire = "N";
                            AddedMakeCaseId.Add(listdata[0].intItemQuestionFieldID);
                            goto NextPage1;
                        }
                    NextPage1:
                        int a1 = 0;

                        if (CommentsListId.Contains(Convert.ToString(listdata[0].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < CommentsList.Count; j++)
                            {
                                if (Convert.ToInt32(CommentsListId[j]) == listdata[i].intItemQuestionFieldID)
                                {
                                    if (!AddedComentId.Contains(Convert.ToInt16(CommentsListId[0])))
                                    {
                                        Notes = CommentsList[i];
                                        AddedComentId.Add(Convert.ToInt16(CommentsListId[0]));
                                        goto NextPage;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Notes = "";
                            AddedComentId.Add(listdata[0].intItemQuestionFieldID);
                            goto NextPage;
                        }
                    NextPage:
                        int a = 0;
                    }
                    else
                    {
                        itemQuestionIds = itemQuestionIds + "," + Convert.ToString(listdata[i].intItemQuestionFieldID);


                        string meetvalue = string.Empty;
                        if (picker_value == null)
                        {
                            meetvalue = "";
                        }
                        else
                        {
                            meetvalue = picker_value.strMeetsStandards;
                        }



                        standardmeets = standardmeets + "," + meetvalue;



                        if (string.IsNullOrEmpty(meetvalue))
                        {
                            pointsearned = pointsearned + "," + "0";
                            pointsavailable = pointsavailable + "," + "0";
                        }
                        else
                        {

                            pointsearned = pointsearned + "," + pointsReturn("Earned", meetvalue, Convert.ToString(listdata[i].intItemQuestionFieldID));
                            pointsavailable = pointsavailable + "," + pointsReturn("Avail", meetvalue, Convert.ToString(listdata[i].intItemQuestionFieldID));

                        }



                        if (MakeCaseId.Contains(Convert.ToString(listdata[i].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < MakeCaseList.Count; j++)
                            {
                                if (Convert.ToInt16(MakeCaseId[j]) == listdata[i].intItemQuestionFieldID)
                                {
                                    if (!AddedMakeCaseId.Contains(Convert.ToInt16(MakeCaseId[j])))
                                    {
                                        caserequire = caserequire + "," + MakeCaseList[j];
                                        AddedMakeCaseId.Add(Convert.ToInt16(MakeCaseId[j]));
                                        goto NextPage1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            caserequire = caserequire + "," + "N";
                            AddedMakeCaseId.Add(listdata[i].intItemQuestionFieldID);
                            goto NextPage1;
                        }
                    NextPage1:
                        int a1 = 0;

                        if (CommentsListId.Contains(Convert.ToString(listdata[i].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < CommentsList.Count; j++)
                            {
                                if (Convert.ToInt16(CommentsListId[j]) == listdata[i].intItemQuestionFieldID)
                                {
                                    if (!AddedComentId.Contains(Convert.ToInt16(CommentsListId[j])))
                                    {
                                        Notes = Notes + "," + CommentsList[j];
                                        AddedComentId.Add(Convert.ToInt16(CommentsListId[j]));
                                        goto NextPage;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Notes = Notes + "," + "";
                            AddedComentId.Add(listdata[i].intItemQuestionFieldID);
                            goto NextPage;
                        }
                    NextPage:
                        int a = 0;
                    }


                }

                //var apicall = QuestAPIMethods.AddQuestionsMetadata(iteminstancetraanid, ItemID, itemQuestionIds, standardmeets, pointsavailable, pointsearned, caserequire, "0", Notes, createdby);
                Add_Questions_MetadataRequest obj = new Add_Questions_MetadataRequest()
                {
                    pCREATED_BY = createdby,
                    pITEM_ID = int.Parse(ItemId),
                    pIS_CASE_REQUESTED = caserequire,
                    pITEM_INSTANCE_TRAN_ID = int.Parse(iteminstancetraanid),
                    pITEM_QUESTION_FIELD_IDs = itemQuestionIds,
                    pMEETS_STANDARDS = standardmeets,
                    pNOTES = Notes,
                    pPOINTS_AVAILABLE = pointsavailable,
                    pPOINTS_EARNED = pointsearned,
                    pWEIGHT = 0
                };

                JObject Body = (JObject)JToken.FromObject(obj);
                var jsonBody = JObject.Parse(Convert.ToString(Body));

                Functions.questObjectData = obj;

                if (removeclick == 1)
                {
                    var apicall = QuestSyncAPIMethods.AddQuestionsMetadata(App.Isonline, iteminstancetraanid, ItemId, itemQuestionIds, standardmeets, pointsavailable, pointsearned, caserequire, "0", Notes, createdby, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, jsonBody, "C");
                    ////var response = apicall.GetValue("ResponseContent");
                    ////var responseStatus = apicall.GetValue("Success");
                    responseStatus = apicall.Result;
                    DynamicViewCall();
                }
                else
                {
                    this.Navigation.PopAsync();
                }


                removeclick = 0;
                //}
                //else
                //{
                //    this.DisplayAlert("Form Finalized", "Form is Finalied you can not edit it.", "Ok");
                //}
            }
            catch (Exception ex)
            {

            }
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (isedit)
                {
                    responseStatus = 0;
                    Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                    await Task.Run(async () =>
                    {
                        saveFormCall();
                    });
                    Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                }
                else
                {
                    await DisplayAlert("Quest", "Form is finalized. {n} You cannot edit it now.", "Ok");
                    await this.Navigation.PopAsync();
                }


            }
            catch (Exception ex)
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

        public string pointsReturn(string searchtype, string CurrentStandardMeet, string itemquestionfieldid)
        {
            string PointsEarned = "";
            standardMeetCopy = new List<ItemQuestionDecode>();

            var AnswerApiCall = QuestSyncAPIMethods.GetItemQuestionDecodeByFieldID(App.Isonline, itemquestionfieldid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemId, iteminstancetranid, ItemCategoryID, scatid);

            standardMeetCopy = AnswerApiCall.Result;



            if (searchtype == "Earned")
            {

                if (standardMeetCopy.Count > 0)
                {
                    for (int i = 0; i < standardMeetCopy.Count; i++)
                    {
                        if (CurrentStandardMeet == standardMeetCopy[i].strMeetsStandards)
                        {
                            PointsEarned = Convert.ToString(standardMeetCopy[i].dcPointsEarned);
                            break;
                        }
                        else
                        {
                            PointsEarned = "";
                        }
                    }
                }
                else
                {
                    PointsEarned = "0";
                }
            }
            else
            {
                if (standardMeetCopy.Count > 0)
                {
                    for (int i = 0; i < standardMeetCopy.Count; i++)
                    {
                        if (CurrentStandardMeet == standardMeetCopy[i].strMeetsStandards)
                        {
                            PointsEarned = Convert.ToString(standardMeetCopy[i].dcPointsAvailable);
                            break;
                        }
                        else
                        {
                            PointsEarned = "";
                        }
                    }
                }
                else
                {
                    PointsEarned = "0";
                }
            }
            return PointsEarned;
        }

        void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var pick = (Picker)sender;
                var tm = pick.SelectedItem as ItemQuestionDecode;

                if (tm != null)
                {
                    var AnswerApiCall = QuestSyncAPIMethods.GetItemQuestionDecodeByFieldID(App.Isonline, Convert.ToString(tm.intItemQuestionFieldID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemId, iteminstancetranid, ItemCategoryID, scatid);
                    standardMeet = AnswerApiCall.Result;

                    var cnt1 = findControl("lb_" + tm.intItemQuestionFieldID);

                    Type cnt_type1 = cnt1.GetType();
                    var Pointslbl = new Label();
                    Pointslbl.FontSize = 16;
                    Pointslbl = (Label)cnt1;

                    if (cnt_type1.Name.ToLower() == "label")
                    {
                        for (int i = 0; i < standardMeet.Count; i++)
                        {
                            if (tm.strMeetsStandards == standardMeet[i].strMeetsStandards)
                            {
                                Pointslbl.Text = "Available Point:" + Convert.ToString(standardMeet[i].dcPointsAvailable) + " Earned :" + Convert.ToString(standardMeet[i].dcPointsEarned);
                                break;
                            }
                            else
                            {
                                Pointslbl.Text = "Available Point:" + Convert.ToString(tm.dcPointsAvailable) + " Earned :" + "0";
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
}
