using System;
using System.Collections.Generic;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.DataTypes.DataType.Quest;
using Xamarin.Forms;
using StemmonsMobile.DataTypes.DataType.Quest;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemQuestionDecodeByFieldIDResponse;
using static StemmonsMobile.DataTypes.DataType.Quest.GetItemQuestionFieldsByItemCategoryIDResponse;
using StemmonsMobile.Commonfiles;
using DataServiceBus.OfflineHelper.DataTypes.Quest;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace StemmonsMobile.Views.CreateQuestForm
{
    public partial class QuestQuestionForm : ContentPage
    {

        List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField> QuestionList = new List<GetItemQuestionFieldsByItemCategoryIDResponse.ItemQuestionField>();
        ItemQuestionField sd;
        List<ItemQuestionDecode> ls = new List<ItemQuestionDecode>();
        List<ItemQuestionDecode> standardMeet = new List<ItemQuestionDecode>();
        List<ItemQuestionDecode> standardMeetCopy = new List<ItemQuestionDecode>();
        ItemQuestionDecode dt = new ItemQuestionDecode();
        List<string> CommentsList = new List<string>();
        List<string> CommentsListId = new List<string>();
        int removeComment;
        List<string> pItemQuestionfieldid = new List<string>();
        List<string> pCASEREQUESTED = new List<string>();
        List<string> pMEETS_STANDARDS = new List<string>();
        List<string> pNOTES = new List<string>();

        List<string> MakeCaseList = new List<string>();
        List<string> MakeCaseId = new List<string>();
        string iteminstancetranid = string.Empty;
        string itemcategoryid = string.Empty;
        string ItemID = string.Empty;
        int ViewCount;
        List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> lstcatbyitemidData;
        AddFormRequest addForm1;
        int responseStatus = 0;
        string scatid = "";
        public QuestQuestionForm(string category, string ItemCategoryID, string instanceTranId, string intItemID, string catid, AddFormRequest addForm, List<GetItemCategoriesByItemIDResponse.ItemCategoryByItemId> lstcatbyitemid)
        {
            InitializeComponent();
            this.Title = category;
            addForm1 = addForm;
            lstcatbyitemidData = lstcatbyitemid;
            iteminstancetranid = instanceTranId;
            itemcategoryid = ItemCategoryID;
            CommentsList = new List<string>();
            CommentsListId = new List<string>();
            ItemID = intItemID;
            ViewCount = 0;
            removeComment = 0;
            scatid = catid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

            try
            {
                if (ViewCount == 0)
                {
                    ViewCount = ViewCount + 1;
                    DynamicView.Children.Clear();
                    DynamicViewDisplay(itemcategoryid);
                }
            }
            catch (Exception ex)
            {
            }
        }

        async void DynamicViewDisplay(String StrId)
        {


            if (removeComment == 0)
            {
                pItemQuestionfieldid = new List<string>();
                pCASEREQUESTED = new List<string>();
                pMEETS_STANDARDS = new List<string>();
                pNOTES = new List<string>();

            }
            DynamicView.Children.Clear();



            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            stk1.Opacity = 0;
            stk2.Opacity = 0;
            try
            {
                await Task.Run(() =>
                {
                    var QuestionApiCall = QuestSyncAPIMethods.GetItemQuestionFieldsByItemCategoryID(App.Isonline, StrId, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemID, scatid);
                    QuestionApiCall.Wait();
                    QuestionList = QuestionApiCall.Result;
                });

                if (QuestionList?.Count > 0)
                {


                    if (removeComment == 0)
                    {
                        if (Functions.questObjectData != null)
                        {
                            pItemQuestionfieldid = stringArray(Functions.questObjectData.pITEM_QUESTION_FIELD_IDs);
                            pCASEREQUESTED = stringArray(Functions.questObjectData.pIS_CASE_REQUESTED);
                            pMEETS_STANDARDS = stringArray(Functions.questObjectData.pMEETS_STANDARDS);
                            pNOTES = stringArray(Functions.questObjectData.pNOTES);


                        }
                    }

                    for (int i = 0; i < QuestionList.Count; i++)
                    {
                        var QuestionLayout = new StackLayout();
                        QuestionLayout.Margin = new Thickness(0, 10, 0, 0);

                        Label QuestionString = new Label();
                        QuestionString.Margin = new Thickness(10, 0, 10, 0);
                        QuestionString.Text = QuestionList[i].strItemQuestionFieldName;
                        QuestionString.StyleId = "STL_" + QuestionList[i].intItemQuestionFieldID;
                        QuestionLayout.Children.Add(QuestionString);


                        Label selectlabel = new Label();
                        selectlabel.Margin = new Thickness(10, 0, 10, 0);
                        selectlabel.Text = "Select Choice:";
                        selectlabel.FontSize = 12;
                        QuestionLayout.Children.Add(selectlabel);

                        if (removeComment == 0)
                        {
                            if (pCASEREQUESTED.Count > 0)
                            {
                                MakeCaseId.Add(Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                                MakeCaseList.Add(pCASEREQUESTED[i]);
                            }
                            if (pNOTES.Count > 0)
                            {
                                CommentsListId.Add(Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                                CommentsList.Add(pNOTES[i]);
                            }

                        }

                        standardMeet = new List<ItemQuestionDecode>();
                        Picker pk = new Picker();

                        await Task.Run(() =>
                         {
                             var AnswerApiCall = QuestSyncAPIMethods.GetItemQuestionDecodeByFieldID(App.Isonline, Convert.ToString(QuestionList[i].intItemQuestionFieldID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, Convert.ToString(ItemID), iteminstancetranid, Convert.ToString(QuestionList[i].intItemCategoryID), scatid);
                             standardMeet = AnswerApiCall.Result;
                         });

                        pk.ItemsSource = standardMeet;
                        pk.Margin = new Thickness(10, 0, 10, 0);
                        pk.ItemDisplayBinding = new Binding("strMeetsStandards");

                        pk.StyleId = "PK_" + QuestionList[i].intItemQuestionFieldID;
                        pk.TextColor = Color.Gray;
                        pk.SelectedIndexChanged += Pk_SelectedIndexChanged;

                        QuestionLayout.Children.Add(pk);


                        var pointslabel = new Label();
                        pointslabel.FontSize = 16;
                        pointslabel.Margin = new Thickness(10, 0, 10, 0);
                        string pointsEarned = "";


                        pointsEarned = "0";

                        string points = string.Empty;
                        if (standardMeet.Count > 0)
                        {
                            points = "Points Available:" + standardMeet[0].dcPointsAvailable + " Earned:" + pointsEarned;
                        }
                        else
                        {
                            points = "Points Available:" + "0" + " Earned:" + pointsEarned;
                        }

                        pointslabel.Text = points;
                        pointslabel.FontSize = 12;
                        pointslabel.StyleId = "lb_" + QuestionList[i].intItemQuestionFieldID;
                        pointslabel.HorizontalTextAlignment = TextAlignment.End;
                        QuestionLayout.Children.Add(pointslabel);


                        var CommentLayout = new StackLayout();
                        var TA = new Entry();
                        TA.Margin = new Thickness(10, 0, 10, 0);
                        TA.FontSize = 16;
                        TA.Placeholder = "Write Note..";

                        if (CommentsListId.Contains(Convert.ToString(QuestionList[i].intItemQuestionFieldID)))
                        {
                            int commentindx = CommentsListId.IndexOf(Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                            if (string.IsNullOrEmpty(CommentsList[i]))
                            {
                                TA.IsEnabled = true;
                            }
                            else
                            {
                                TA.IsEnabled = false;
                            }
                        }
                        else
                        {
                            TA.IsEnabled = true;
                        }

                        TA.StyleId = "TA_" + QuestionList[i].intItemQuestionFieldID;
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
                        MakeCase.StyleId = "makecase_" + QuestionList[i].intItemQuestionFieldID;
                        MakeCase.Clicked += MakeCase_Clicked;
                        MakeCase.CommandParameter = QuestionList[i];
                        grid.Children.Add(MakeCase, 0, 0);

                        Button Attachment = new Button();
                        Attachment.IsEnabled = false;
                        Attachment.BackgroundColor = Color.Transparent;
                        Attachment.TextColor = Color.Blue;
                        Attachment.Text = "Attachment";
                        Attachment.Clicked += Attachment_Clicked;
                        Attachment.CommandParameter = QuestionList[i];
                        grid.Children.Add(Attachment, 1, 0);
                        //  Button NewLine = new Button();
                        //  NewLine.BackgroundColor = Color.Transparent;
                        //  NewLine.TextColor = Color.Blue;
                        //  NewLine.Text = "Add Commnet";
                        ////  NewLine.Clicked += NewLine_Clicked;
                        //NewLine.CommandParameter = QuestionList[i];
                        //grid.Children.Add(NewLine, 2, 0);

                        CommentLayout.Children.Add(grid);
                        QuestionLayout.Children.Add(CommentLayout);




                        // new code for comment data

                        var AttachmentDLayout = new StackLayout();
                        AttachmentDLayout.Margin = new Thickness(10, 5, 0, 0);

                        if (pNOTES.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(pNOTES[i]))
                            {
                                Label ExtraNotes = new Label();
                                ExtraNotes.Text = pNOTES[i];
                                ExtraNotes.FontSize = 16;
                                ExtraNotes.Margin = new Thickness(10, 5, 5, 0);
                                QuestionLayout.Children.Add(ExtraNotes);

                                Button ExtraViewCase = new Button();
                                ExtraViewCase.BackgroundColor = Color.Transparent;
                                ExtraViewCase.TextColor = Color.Blue;
                                ExtraViewCase.Text = "ViewCase";
                                ExtraViewCase.FontSize = 13;
                                ExtraViewCase.IsEnabled = false;
                                //  ExtraViewCase.Clicked += ExtraViewCase_Clicked;
                                ExtraViewCase.Margin = new Thickness(15, 0, 0, 0);
                                AttachmentDLayout.Children.Add(ExtraViewCase);



                                Button ExtraAttachment = new Button();
                                ExtraAttachment.BackgroundColor = Color.Transparent;
                                ExtraAttachment.TextColor = Color.Blue;
                                ExtraAttachment.Text = "Attachment";
                                ExtraAttachment.FontSize = 13;
                                ExtraViewCase.IsEnabled = false;
                                ExtraAttachment.Margin = new Thickness(20, 0, 0, 0);
                                ExtraAttachment.HorizontalOptions = LayoutOptions.CenterAndExpand;
                                //  ExtraAttachment.Clicked += ExtraAttachment_Clicked;
                                //  ExtraAttachment.CommandParameter = listdata[i];
                                AttachmentDLayout.Children.Add(ExtraAttachment);


                                Button ExtraRemove = new Button();
                                ExtraRemove.BackgroundColor = Color.Transparent;
                                ExtraRemove.TextColor = Color.Blue;
                                ExtraRemove.Text = "Remove";
                                ExtraRemove.FontSize = 13;
                                ExtraRemove.IsEnabled = true;
                                ExtraRemove.Margin = new Thickness(0, 0, 15, 0);
                                ExtraRemove.HorizontalOptions = LayoutOptions.EndAndExpand;
                                ExtraRemove.Clicked += Remove_Clicked;
                                ExtraRemove.CommandParameter = QuestionList[i];

                                AttachmentDLayout.Children.Add(ExtraRemove);

                                AttachmentDLayout.Orientation = StackOrientation.Horizontal;
                                AttachmentDLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                                QuestionLayout.Children.Add(AttachmentDLayout);
                            }

                        }



                        DynamicView.Children.Add(QuestionLayout);

                        if (pMEETS_STANDARDS.Count > 0)
                        {
                            for (int j = 0; j < standardMeet.Count; j++)
                            {
                                if (pMEETS_STANDARDS[i] == standardMeet[j].strMeetsStandards)
                                {
                                    pk.SelectedIndex = j;
                                }
                            }
                        }
                        else
                        {
                            pk.SelectedIndex = 1;
                        }
                    }

                    removeComment = 0;
                }

                else
                {
                    DisplayAlert("Quest Form", "Questionaries form data not available please edit it later.", "Ok");
                    this.Navigation.PopAsync();
                }
            }
            catch (Exception ex)
            {
            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            stk1.Opacity = 1;
            stk2.Opacity = 1;
        }


        void Remove_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button removebtn = (Button)sender;
                var value = removebtn.CommandParameter as ItemQuestionField;



                if (CommentsListId.Contains(Convert.ToString(value.intItemQuestionFieldID)))
                {
                    for (int j = 0; j < CommentsList.Count; j++)
                    {
                        if (Convert.ToInt16(CommentsListId[j]) == value.intItemQuestionFieldID)
                        {

                            CommentsList.RemoveAt(j);
                            CommentsListId.RemoveAt(j);
                            if (pNOTES.Count > 0)
                            {
                                pNOTES[j] = "";
                            }
                            if (pCASEREQUESTED.Count > 0)
                            {
                                if (MakeCaseId.Count >= j)
                                {
                                    MakeCaseId.RemoveAt(j);
                                    MakeCaseList.RemoveAt(j);
                                }
                            }

                            goto NextPage;
                        }
                    }
                }

                NextPage:
                int a = 0;
                removeComment = 1;
                DynamicViewDisplay(itemcategoryid);

            }
            catch (Exception ex)
            { }
        }

        public List<string> stringArray(string parameter)
        {

            string[] objectvalues = parameter.Split(',');
            List<string> convertStringToList = new List<string>(objectvalues);

            return convertStringToList;
        }

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
        //void NewLine_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var pick = (Button)sender;
        //        var value = pick.CommandParameter as ItemQuestionField;
        //        var stack = (StackLayout)pick.Parent.Parent;
        //        int CommentCount = 0;

        //        var cnt1 = findControl("TA_" + value.intItemQuestionFieldID);
        //        if (cnt1 != null)
        //        {
        //            Type cnt_type1 = cnt1.GetType();
        //            var Comment = new Entry();
        //            Comment = (Entry)cnt1;
        //            string commentdata = "";
        //            if (cnt_type1.Name.ToLower() == "entry")
        //            {
        //                commentdata = Comment.Text;
        //            }
        //            stack.Children.Add(new Label() { Text = commentdata, Margin = new Thickness(10, 0, 10, 0), FontSize = 14 });


        //            var NewAttachmentDLayout = new StackLayout();
        //            NewAttachmentDLayout.Margin = new Thickness(10, 5, 0, 0);

        //            Button ExtraViewCase = new Button();
        //            ExtraViewCase.BackgroundColor = Color.Transparent;
        //            ExtraViewCase.TextColor = Color.Blue;
        //            ExtraViewCase.Text = "ViewCase";
        //            ExtraViewCase.FontSize = 13;

        //            ExtraViewCase.IsEnabled = false;

        //            ExtraViewCase.Clicked += ExtraViewCase1_Clicked;

        //            ExtraViewCase.Margin = new Thickness(15, 0, 0, 0);

        //            Button ExtraAttachment = new Button();
        //            ExtraAttachment.BackgroundColor = Color.Transparent;
        //            ExtraAttachment.TextColor = Color.Blue;
        //            ExtraAttachment.Text = "Attachment";
        //            ExtraAttachment.FontSize = 13;
        //            ExtraAttachment.Clicked += Attachment1_Clicked;
        //            ExtraAttachment.CommandParameter = value;
        //            ExtraAttachment.HorizontalOptions = LayoutOptions.CenterAndExpand;
        //            ExtraAttachment.Margin = new Thickness(15, 0, 0, 0);



        //            Button ExtraRemove = new Button();
        //            ExtraRemove.BackgroundColor = Color.Transparent;
        //            ExtraRemove.TextColor = Color.Blue;
        //            ExtraRemove.Text = "Remove";
        //            ExtraRemove.FontSize = 13;
        //            ExtraRemove.IsEnabled = false;
        //            ExtraRemove.HorizontalOptions = LayoutOptions.EndAndExpand;
        //            ExtraRemove.Margin = new Thickness(0, 0, 15, 0);



        //            NewAttachmentDLayout.Children.Add(ExtraViewCase);
        //            NewAttachmentDLayout.Children.Add(ExtraAttachment);
        //            NewAttachmentDLayout.Children.Add(ExtraRemove);

        //            NewAttachmentDLayout.Orientation = StackOrientation.Horizontal;
        //            NewAttachmentDLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
        //            stack.Children.Add(NewAttachmentDLayout);



        //            CommentsListId.Add(Convert.ToString(value.intItemQuestionFieldID));
        //            CommentsList.Add(commentdata);

        //            pick.IsEnabled = false;

        //            if (CommentsListId.Contains(Convert.ToString(value.intItemQuestionFieldID)))
        //            {
        //                for (int i = 0; i < CommentsListId.Count; i++)
        //                {

        //                    if (CommentsListId[i] == Convert.ToString(value.intItemQuestionFieldID))
        //                    {
        //                        CommentCount = CommentCount + 1;
        //                    }
        //                }
        //            }


        //            if (CommentCount > 1)
        //            {
        //                try
        //                {
        //                    GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase obj = new GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase()
        //                    {
        //                        strNotes = commentdata,
        //                        itemInstanceTranID = iteminstancetranid
        //                    };
        //                    UpdateCaseNotesToQuestionRequest objcasenotes = new UpdateCaseNotesToQuestionRequest()
        //                    {
        //                        notes = commentdata,
        //                        modifiedBy = Functions.UserName
        //                    };

        //                    var APICall = QuestSyncAPIMethods.UpdateCaseNotesToQuestion(App.Isonline, iteminstancetranid, ItemID, commentdata, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, objcasenotes, obj);
        //                    //var Response = APICall.GetValue("ResponseContent");
        //                    var Response = APICall.Result;

        //                    if (MakeCaseId.Contains(Convert.ToString(value.intItemQuestionFieldID)))
        //                    {
        //                        AddCaseToQuestionRequest objAddCaseToQuestion = new AddCaseToQuestionRequest()
        //                        {
        //                            createdBy = Functions.UserName,
        //                            itemInstanceTranID = Convert.ToInt32(iteminstancetranid),
        //                            itemQuestionFieldID = Convert.ToInt32(value.intItemQuestionFieldID),
        //                            notes = commentdata
        //                        };

        //                        var APICall1 = QuestSyncAPIMethods.AddCaseToQuestion(App.Isonline, iteminstancetranid, ItemID, Convert.ToString(value.intItemQuestionFieldID), commentdata, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, obj, objAddCaseToQuestion);
        //                        var Response1 = APICall1.Result;

        //                    }
        //                }
        //                catch (Exception ex)
        //                {


        //                }
        //            }
        //            Comment.Text = "";
        //        }

        //        UncheckMakeCase();
        //    }
        //    catch (Exception ex)
        //    {


        //    }

        //}




        void SaveCommnets()
        {
            if (QuestionList?.Count > 0)
            {
                for (int i = 0; i < QuestionList.Count; i++)
                {

                    var cnt1 = findControl("TA_" + QuestionList[i].intItemQuestionFieldID);
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

                        if (!string.IsNullOrEmpty(commentdata))
                        {

                            if (CommentsListId.Contains(Convert.ToString(QuestionList[i].intItemQuestionFieldID)))
                            {
                                int indexofcomment = CommentsListId.IndexOf(Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                                CommentsList[i] = commentdata;
                            }
                            else
                            {
                                CommentsListId.Add(Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                                CommentsList.Add(commentdata);
                            }

                        }



                        //if (CommentsListId.Contains(Convert.ToString(QuestionList[i].intItemQuestionFieldID)))
                        //                {
                        //                    for (int i = 0; i < CommentsListId.Count; i++)
                        //                    {

                        //if (CommentsListId[i] == Convert.ToString(QuestionList[i].intItemQuestionFieldID))
                        //        {
                        //            CommentCount = CommentCount + 1;
                        //        }
                        //    }
                        //}


                        //if (CommentCount > 1)
                        //{
                        //    try
                        //    {
                        //        GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase obj = new GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase()
                        //        {
                        //            strNotes = commentdata,
                        //            itemInstanceTranID = iteminstancetranid
                        //        };
                        //        UpdateCaseNotesToQuestionRequest objcasenotes = new UpdateCaseNotesToQuestionRequest()
                        //        {
                        //            notes = commentdata,
                        //            modifiedBy = Functions.UserName
                        //        };

                        //        var APICall = QuestSyncAPIMethods.UpdateCaseNotesToQuestion(App.Isonline, iteminstancetranid, ItemID, commentdata, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, objcasenotes, obj);
                        //        //var Response = APICall.GetValue("ResponseContent");
                        //        var Response = APICall.Result;

                        //        if (MakeCaseId.Contains(Convert.ToString(value.intItemQuestionFieldID)))
                        //        {
                        //            AddCaseToQuestionRequest objAddCaseToQuestion = new AddCaseToQuestionRequest()
                        //            {
                        //                createdBy = Functions.UserName,
                        //                itemInstanceTranID = Convert.ToInt32(iteminstancetranid),
                        //                itemQuestionFieldID = Convert.ToInt32(value.intItemQuestionFieldID),
                        //                notes = commentdata
                        //            };

                        //            var APICall1 = QuestSyncAPIMethods.AddCaseToQuestion(App.Isonline, iteminstancetranid, ItemID, Convert.ToString(value.intItemQuestionFieldID), commentdata, Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, obj, objAddCaseToQuestion);
                        //            var Response1 = APICall1.Result;

                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {


                        //    }
                        //}
                        //Comment.Text = "";
                    }

                }
            }

        }


        void ExtraViewCase1_Clicked(object sender, EventArgs e)
        {
            try
            {


                Button showcasebtn = (Button)sender;
                var value = showcasebtn.CommandParameter as GetItemQuestionMetadataCaseResponse.ItemQuestionMetadataCase;
                this.Navigation.PushAsync(new Cases.ViewCasePage(Convert.ToString(value.intCaseID), "", ""));
                // this.Navigation.PushAsync(new Attachment(Convert.ToString(value.intItemQuestionFieldID), iteminstancetranid, Convert.ToString(value.intItemQuestionMetadataID)));
            }
            catch (Exception ex)
            {


            }
        }
        void Attachment_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionField;
                if (App.Isonline)
                {
                    this.Navigation.PushAsync(new Attachment(Convert.ToString(value.intItemQuestionFieldID), iteminstancetranid, Convert.ToString(value.intItemQuestionMetaDataID)));
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

        void Attachment1_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionField;
                if (App.Isonline)
                {
                    this.Navigation.PushAsync(new Attachment(Convert.ToString(value.intItemQuestionFieldID), iteminstancetranid, Convert.ToString(value.intItemQuestionMetaDataID)));
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

        void MakeCase_Clicked(object sender, EventArgs e)
        {
            try
            {
                Button attchmentbtn = (Button)sender;
                var value = attchmentbtn.CommandParameter as ItemQuestionField;

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
            {


            }
        }


        void UncheckMakeCase()
        {
            try
            {
                //MakeCase.StyleId = "makecase_" + QuestionList[i].intItemQuestionFieldID;
                for (int i = 0; i < QuestionList.Count; i++)
                {
                    var cnt1 = findControl("makecase_" + QuestionList[i].intItemQuestionFieldID);

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

        void Pk_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var pick = (Picker)sender;
                var tm = pick.SelectedItem as ItemQuestionDecode;

                //var AnswerApiCall = QuestAPIMethods.GetItemQuestionDecodeByFieldID(Convert.ToString(tm.intItemQuestionFieldID));
                //var Result = AnswerApiCall.GetValue("ResponseContent");

                var AnswerApiCall = QuestSyncAPIMethods.GetItemQuestionDecodeByFieldID(App.Isonline, Convert.ToString(tm.intItemQuestionFieldID), ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemID, iteminstancetranid, Convert.ToString(tm.intItemQuestionFieldID), scatid);
                ls = AnswerApiCall.Result;
                //var Result = AnswerApiCall.Result;

                //ls = new List<ItemQuestionDecode>();
                //ls = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemQuestionDecode>>(Result.ToString());
                var cnt1 = findControl("lb_" + tm.intItemQuestionFieldID);

                Type cnt_type1 = cnt1.GetType();
                var Pointslbl = new Label();
                Pointslbl.FontSize = 16;
                Pointslbl = (Label)cnt1;

                if (cnt_type1.Name.ToLower() == "label")
                {
                    for (int i = 0; i < ls.Count; i++)
                    {
                        if (tm.strMeetsStandards == ls[i].strMeetsStandards)
                        {
                            Pointslbl.Text = "Available Point:" + Convert.ToString(ls[i].dcPointsAvailable) + " Earned :" + Convert.ToString(ls[i].dcPointsEarned);
                            break;
                        }
                        else
                        {
                            Pointslbl.Text = "Available Point:" + Convert.ToString(tm.dcPointsAvailable) + " Earned :" + "0";

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }


        }

        public string pointsReturn(string searchtype, string CurrentStandardMeet, string itemquestionfieldid)
        {
            string PointsEarned = "";
            standardMeetCopy = new List<ItemQuestionDecode>();
            // Task.Run(async () =>
            //{


            var AnswerApiCall = QuestSyncAPIMethods.GetItemQuestionDecodeByFieldID(App.Isonline, itemquestionfieldid, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath, ItemID, iteminstancetranid, itemcategoryid, scatid);
            AnswerApiCall.Wait();
            standardMeetCopy = AnswerApiCall.Result;
            //}).Wait();




            if (searchtype == "Earned")
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
            return PointsEarned;
        }


        void questionariesSaveClick()
        {

            try
            {
                string sitemId = ItemID;
                List<int> AddedComentId = new List<int>();

                List<int> AddedMakeCaseId = new List<int>();
                var iteminstancetraanid = iteminstancetranid;
                int count1 = QuestionList.Count;
                string itemQuestionIds = "";
                string standardmeets = "";
                string pointsavailable = "";
                string pointsearned = "";
                string caserequire = "";
                string Notes = "";
                string createdby = Functions.UserName;
                for (int i = 0; i < QuestionList.Count; i++)
                {

                    var cnt1 = findControl("PK_" + QuestionList[i].intItemQuestionFieldID);

                    Type cnt_type1 = cnt1.GetType();

                    var pick_Ext_datasrc = new Picker();
                    if (cnt_type1.Name.ToLower() == "picker")
                    {
                        pick_Ext_datasrc = (Picker)cnt1;
                    }

                    var picker_value = pick_Ext_datasrc.SelectedItem as ItemQuestionDecode;

                    var cnt2 = findControl("TA_" + QuestionList[i].intItemQuestionFieldID);

                    Type cnt_type2 = cnt2.GetType();
                    var ent = new Entry();
                    ent = (Entry)cnt2;


                    if (i == 0)
                    {
                        itemQuestionIds = Convert.ToString(QuestionList[0].intItemQuestionFieldID);

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
                            pointsearned = pointsReturn("Earned", meetvalue, Convert.ToString(QuestionList[0].intItemQuestionFieldID));
                            pointsavailable = pointsReturn("Avail", meetvalue, Convert.ToString(QuestionList[0].intItemQuestionFieldID));
                        }

                        if (MakeCaseId.Contains(Convert.ToString(QuestionList[0].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < MakeCaseList.Count; j++)
                            {
                                if (Convert.ToInt16(MakeCaseId[j]) == QuestionList[i].intItemQuestionFieldID)
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
                            AddedMakeCaseId.Add(QuestionList[0].intItemQuestionFieldID);
                            goto NextPage1;
                        }

                        NextPage1:
                        int a1 = 0;

                        if (CommentsListId.Contains(Convert.ToString(QuestionList[0].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < CommentsList.Count; j++)
                            {
                                if (Convert.ToInt16(CommentsListId[j]) == QuestionList[i].intItemQuestionFieldID)
                                {
                                    if (!AddedComentId.Contains(Convert.ToInt16(CommentsListId[0])))
                                    {
                                        Notes = CommentsList[j];
                                        AddedComentId.Add(Convert.ToInt16(CommentsListId[0]));
                                        goto NextPage;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Notes = "";
                            AddedComentId.Add(QuestionList[0].intItemQuestionFieldID);
                            goto NextPage;
                        }
                        NextPage:
                        int a = 0;
                    }
                    else
                    {
                        itemQuestionIds = itemQuestionIds + "," + Convert.ToString(QuestionList[i].intItemQuestionFieldID);
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
                        //pointsearned = pointsearned + "," + PointsEarned(meetvalue);
                        //pointsavailable = pointsavailable + "," + PointsAvailable(meetvalue);

                        if (string.IsNullOrEmpty(meetvalue))
                        {
                            pointsearned = pointsearned + "," + "0";
                            pointsavailable = pointsavailable + "," + "0";
                        }
                        else
                        {


                            pointsearned = pointsearned + "," + pointsReturn("Earned", meetvalue, Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                            pointsavailable = pointsavailable + "," + pointsReturn("Avail", meetvalue, Convert.ToString(QuestionList[i].intItemQuestionFieldID));
                        }


                        if (MakeCaseId.Contains(Convert.ToString(QuestionList[i].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < MakeCaseList.Count; j++)
                            {
                                if (Convert.ToInt16(MakeCaseId[j]) == QuestionList[i].intItemQuestionFieldID)
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
                            AddedMakeCaseId.Add(QuestionList[i].intItemQuestionFieldID);
                            goto NextPage1;
                        }
                        NextPage1:
                        int a1 = 0;

                        if (CommentsListId.Contains(Convert.ToString(QuestionList[i].intItemQuestionFieldID)))
                        {
                            for (int j = 0; j < CommentsList.Count; j++)
                            {
                                if (Convert.ToInt16(CommentsListId[j]) == QuestionList[i].intItemQuestionFieldID)
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
                            AddedComentId.Add(QuestionList[i].intItemQuestionFieldID);
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
                    pITEM_ID = int.Parse(ItemID),
                    pIS_CASE_REQUESTED = caserequire,
                    pITEM_INSTANCE_TRAN_ID = 0,
                    pITEM_QUESTION_FIELD_IDs = itemQuestionIds,
                    pMEETS_STANDARDS = standardmeets,
                    pNOTES = Notes,
                    pPOINTS_AVAILABLE = pointsavailable,
                    pPOINTS_EARNED = pointsearned,
                    pWEIGHT = 0
                };

                JObject Body = (JObject)JToken.FromObject(obj);
                //var jsonBody = JObject.Parse(Convert.ToString(Body));


                Functions.questObjectData = obj;

                this.Navigation.PopAsync();
            }
            catch (Exception ex)
            { }
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

        void unfocusTextarea()
        {
            try
            {
                for (int i = 0; i < QuestionList.Count; i++)
                {
                    var cnt1 = findControl("TA_" + QuestionList[i].intItemQuestionFieldID);
                    if (cnt1 != null)
                    {
                        Type cnt_type1 = cnt1.GetType();
                        var Comment = new Entry();
                        Comment.Unfocus();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        async void Save_Clicked(object sender, System.EventArgs e)
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            try
            {
                SaveCommnets();
            }
            catch (Exception ex)
            {

            }
            questionariesSaveClick();
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }
    }
}
