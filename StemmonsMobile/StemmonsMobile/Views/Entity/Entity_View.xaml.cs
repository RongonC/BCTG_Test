using DataServiceBus.OfflineHelper.DataTypes;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OfflineHelper.DataTypes.Entity;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using PCLStorage;
using Plugin.Media;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Entity;
using StemmonsMobile.Models;
using StemmonsMobile.Views.LoginProcess;
using StemmonsMobile.Views.People_Screen;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static DataServiceBus.OfflineHelper.DataTypes.Common.ConstantsSync;

namespace StemmonsMobile.Views.Entity
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Entity_View : ContentPage
    {
        private ObservableCollection<ChillerEntityGroup> _allGroups = new ObservableCollection<ChillerEntityGroup>();
        private ObservableCollection<ChillerEntityGroup> _expandedGroups;
        ObservableCollection<EntityNotesGroup> Groups = new ObservableCollection<EntityNotesGroup>();
        BorderEditor txt_EntNotes = new BorderEditor();
        List<AssociationField> EntityAssocOrder = new List<AssociationField>();
        EntityClass EntityLists = new EntityClass();
        List<Entity_Notes> EntityListsNotes = new List<Entity_Notes>();
        string NavScreenname = string.Empty; // To manage Online and Offline View entity as per Screen name i.ee Associed To Me.
        public EntityListMBView _entityListMBView = new EntityListMBView();

        /// <summary>  Main Required Para is Below for this page Work
        ///     _entityListMBView.EntityDetails.EntityTypeID;
        ///     _entityListMBView.EntityDetails.EntityTypeName;
        ///    _entityListMBView.EntityDetails.EntityID; 
        /// </summary>
        /// <param name="_SelectedEntity"></param>
        /// 
        public Entity_View(EntityListMBView _SelectedEntity, string _navscreenname = "")
        {
            InitializeComponent();
            _entityListMBView = _SelectedEntity;
            Title = string.IsNullOrEmpty(_SelectedEntity.Title) ? "View Entity" : _SelectedEntity.Title;
            lbl_createname.Text = "";
            Groups.Clear();
            NavScreenname = _navscreenname;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            Functions.IsEditEntity = false;
            Groups.Clear();
            TextFieldsLayout.Children.Clear();
            _allGroups.Clear();
            GetEntityDetails();
        }
        public async void GetEntityDetails()
        {
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
            string EntityRelatedResponse = "";
            try
            {
                bool Onlineflag = false;
                if (!string.IsNullOrEmpty(NavScreenname))
                    Onlineflag = false;
                else
                    Onlineflag = App.Isonline;

                await Task.Run(async () =>
                {
                    try
                    {
                        EntityLists = await EntitySyncAPIMethods.GetEntityByEntityID(Onlineflag, _entityListMBView.EntityDetails.EntityID.ToString(), Functions.UserName, _entityListMBView.EntityDetails.EntityTypeID.ToString(), App.DBPath);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        EntityListsNotes = await EntitySyncAPIMethods.GetNotes(Onlineflag, _entityListMBView.EntityDetails.EntityID.ToString(), Functions.UserName, _entityListMBView.EntityDetails.EntityTypeID.ToString(), _entityListMBView.EntityDetails.EntityTypeName, App.DBPath);
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        /*PHASE II Implementation*/
                        EntityRelatedResponse = await EntitySyncAPIMethods.GetEntityRelatedApplications(Onlineflag, _entityListMBView.EntityDetails.EntityTypeName, _entityListMBView.EntityDetails.EntityID.ToString(), _entityListMBView.EntityDetails.EntityTypeID.ToString(), Functions.UserName, App.DBPath);
                    }
                    catch (Exception)
                    {
                    }
                });

                #region Get Entity By ID
                if (EntityLists != null)
                {
                    var lbl = new Label
                    { VerticalOptions = LayoutOptions.Start };
                    lbl.Text = _entityListMBView.Title;
                    lbl.HorizontalOptions = LayoutOptions.Start;
                    lbl.FontSize = 22;
                    lbl.Margin = new Thickness(0, 10, 0, 0);
                    lbl.FontAttributes = FontAttributes.Bold;

                    var Mainlayout = new StackLayout();
                    Mainlayout.Orientation = StackOrientation.Horizontal;
                    Mainlayout.Margin = new Thickness(10, 0, 0, 0);
                    Mainlayout.Children.Add(lbl);
                    TextFieldsLayout.Children.Add(Mainlayout);

                    EntityAssocOrder = EntityLists.AssociationFieldCollection.OrderBy(x => x.ItemDesktopPriorityValue).ToList();

                    for (int i = 0; i < EntityAssocOrder.Count; i++)
                    {
                        Mainlayout = new StackLayout();
                        Mainlayout.Orientation = StackOrientation.Horizontal;
                        Mainlayout.Margin = new Thickness(10, 0, 0, 0);

                        var LeftLyout = new StackLayout();
                        LeftLyout.HorizontalOptions = LayoutOptions.Start;
                        LeftLyout.WidthRequest = 200;

                        var RightLayout = new StackLayout();
                        RightLayout.HorizontalOptions = LayoutOptions.Start;
                        RightLayout.WidthRequest = 200;

                        var Label1 = new Label
                        { VerticalOptions = LayoutOptions.Start };
                        Label1.Text = EntityAssocOrder[i].AssocName;
                        Label1.HorizontalOptions = LayoutOptions.Start;
                        Label1.VerticalOptions = LayoutOptions.Center;
                        Label1.FontSize = 14;

                        var Label2 = new Label
                        { VerticalOptions = LayoutOptions.Start };

                        switch (EntityAssocOrder[i].FieldType.ToLower())
                        {
                            case "se":
                            case "el":
                            case "me":
                                if (EntityAssocOrder[i].AssocMetaData.Count != 0)
                                    Label2.Text = EntityAssocOrder[i].AssocMetaData[0].FieldValue;
                                break;
                            case "pg":

                                if (EntityAssocOrder[i].AssocMetaDataText.Count != 0)
                                {
                                    var data = EntityAssocOrder[i].AssocMetaDataText;
                                    if (data.Count != 0)
                                    {
                                        for (int j = 0; j < data.Count(); j++)
                                        {
                                            if (j < 1)
                                                Label2.Text = Functions.HTMLToText(data[j].TextValue);
                                            else
                                                Label2.Text += Environment.NewLine + Functions.HTMLToText(data[j].TextValue);
                                        }
                                        Label2.Text = Label2.Text.Trim();
                                    }
                                }
                                break;
                            default:

                                if (EntityAssocOrder[i].AssocMetaDataText.Count != 0)
                                {
                                    var data = EntityAssocOrder[i].AssocMetaDataText;
                                    if (data.Count != 0)
                                    {
                                        for (int j = 0; j < data.Count(); j++)
                                        {
                                            if (j < 1)
                                                Label2.Text = Functions.HTMLToText(data[j].TextValue);
                                            else
                                                Label2.Text += Environment.NewLine + Functions.HTMLToText(data[j].TextValue);
                                        }
                                        Label2.Text = Label2.Text.Trim();
                                    }
                                }
                                else if (EntityAssocOrder[i].AssocMetaData.Count != 0)
                                {
                                    var data = EntityAssocOrder[i].AssocMetaData;
                                    if (data.Count != 0)
                                    {
                                        for (int j = 0; j < data.Count(); j++)
                                        {
                                            if (j < 1)
                                                Label2.Text = data[j].FieldValue;
                                            else
                                                Label2.Text += Environment.NewLine + data[j].FieldValue;
                                        }
                                    }
                                }
                                break;
                        }

                        Label2.HorizontalOptions = LayoutOptions.Start;
                        Label2.FontSize = 14;
                        Label2.StyleId = EntityAssocOrder[i].FieldType + "_" + EntityAssocOrder[i].AssocTypeID;

                        LeftLyout.Children.Add(Label1);
                        RightLayout.Children.Add(Label2);
                        Mainlayout.Children.Add(LeftLyout);
                        Mainlayout.Children.Add(RightLayout);

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
                    Label11.FontSize = 14;

                    var layout14 = new StackLayout();
                    layout14.Orientation = StackOrientation.Vertical;
                    layout14.HorizontalOptions = LayoutOptions.Start;
                    layout14.WidthRequest = 200;

                    txt_EntNotes.VerticalOptions = LayoutOptions.Start;
                    txt_EntNotes.WidthRequest = 200;
                    txt_EntNotes.HeightRequest = 150;
                    txt_EntNotes.FontSize = 14;
                    txt_EntNotes.HorizontalOptions = LayoutOptions.Start;
                    txt_EntNotes.BorderColor = Color.Gray;
                    txt_EntNotes.BorderWidth = 1;
                    txt_EntNotes.CornerRadius = 5;
                    ;

                    var btn_addnotes = new Button { };
                    btn_addnotes.Text = "Add Notes";
                    btn_addnotes.FontSize = 14;
                    btn_addnotes.BackgroundColor = Color.Transparent;
                    btn_addnotes.Clicked += Btn_addnotes_Clicked;

                    layout14.Children.Add(txt_EntNotes);
                    layout14.Children.Add(btn_addnotes);

                    layout1.Children.Add(Label11);
                    layout.Children.Add(layout1);
                    layout.Children.Add(layout14);

                    TextFieldsLayout.Children.Add(layout);

                    Assign_Sam = EntityLists.EntitiyAssignedToUserName;
                    Create_Sam = EntityLists.EntityCreatedByUserName;
                    Modify_Sam = EntityLists.EntityModifiedByUserName;
                    Owner_Sam = EntityLists.EntitiyOwnedByUserName;

                    FormattedString s = new FormattedString();

                    #region Created By Name
                    try
                    {
                        if (EntityLists?.EntityCreatedByFullName != null)
                        {
                            s.Spans.Add(new Span { Text = EntityLists.EntityCreatedByFullName + "\r\n", FontSize = 14 });
                            s.Spans.Add(new Span { Text = (Convert.ToDateTime(EntityLists.EntityCreatedDateTime)).ToString(), FontSize = 14 });
                        }
                    }
                    catch (Exception)
                    {
                    }
                    lbl_createname.FormattedText = (s);

                    #endregion

                    #region Assign To Name
                    s = new FormattedString();
                    try
                    {
                        if (EntityLists?.EntityAssignedToFullName != null)
                        {
                            s.Spans.Add(new Span { Text = EntityLists.EntityAssignedToFullName + "\r\n", FontSize = 14 });
                            s.Spans.Add(new Span { Text = (Convert.ToDateTime(EntityLists.EntityAssignedToDateTime)).ToString(), FontSize = 14 });
                        }
                    }
                    catch (Exception)
                    {
                    }
                    lbl_assignto.FormattedText = (s);

                    #endregion

                    #region Owned By Name
                    s = new FormattedString();
                    try
                    {
                        if (EntityLists?.EntityOwnedByFullName != null)
                        {
                            s.Spans.Add(new Span { Text = EntityLists.EntityOwnedByFullName + "\r\n", FontSize = 14 });
                            s.Spans.Add(new Span { Text = (Convert.ToDateTime(EntityLists.EntityOwnedByDateTime)).ToString(), FontSize = 14 });
                        }
                    }
                    catch (Exception)
                    {
                    }
                    lbl_ownername.FormattedText = (s);


                    #endregion

                    #region Modified By Name
                    s = new FormattedString();
                    try
                    {
                        if (EntityLists?.EntityModifiedByFullName != null)
                        {
                            s.Spans.Add(new Span { Text = EntityLists.EntityModifiedByFullName + "\r\n", FontSize = 14 });
                            s.Spans.Add(new Span { Text = (Convert.ToDateTime(EntityLists.EntityModifiedDateTime)).ToString(), FontSize = 14 });
                        }
                    }
                    catch (Exception)
                    {
                    }
                    lbl_modifiedname.FormattedText = (s);

                    #endregion

                }
                else
                {
                    DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
                #endregion

                #region Get Entity notes

                ObservableCollection<EntityNotesGroup> Temp = new ObservableCollection<EntityNotesGroup>();
                if (EntityListsNotes != null)
                {
                    for (int i = 0; i < EntityListsNotes.Count; i++)
                    {
                        string st = App.DateFormatStringToString(EntityListsNotes[i].CreatedDatetime);
                        Temp.Add(new EntityNotesGroup("", st.ToString(), EntityListsNotes[i].CreatedBy)
                        {
                            new Entity_Notes { Note = EntityListsNotes[i].Note }
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
                            item.FirstOrDefault().Note = item.FirstOrDefault().Note;//Functions.HTMLToText(item.FirstOrDefault().Note);
                        }
                        else
                        {
                            item.FirstOrDefault().ImageVisible = false;
                            item.FirstOrDefault().LabelVisible = true;
                            item.FirstOrDefault().htmlNote = item.FirstOrDefault().Note;
                            item.FirstOrDefault().Note = item.FirstOrDefault().Note;//Functions.HTMLToText(item.FirstOrDefault().Note);
                        }
                        Groups.Add(item);
                    }
                }

                gridEntitynotes.ItemsSource = Groups;
                gridEntitynotes.HeightRequest = Groups.Count <= 0 ? 0 : 700;
                #endregion

                #region Get Entity Related Application
                ObservableCollection<ChillerEntityGroup> groupedItems = new ObservableCollection<ChillerEntityGroup>();
                int counter = 0;
                if (!string.IsNullOrEmpty(EntityRelatedResponse?.ToString()) && EntityRelatedResponse?.ToString() != "[]")
                {
                    var app = EntityRelatedResponse.ToString().Split(',');

                    ChillerEntityGroup group1 = new ChillerEntityGroup("Related Items", "", true);


                    groupedItems.Add(group1);
                    foreach (var item in app)
                    {
                        if (item.ToLower() == "departmentrolerelation" || item.ToLower() == "departmenttyperelation")
                            continue;

                        group1.Add(new ChillerEntity { Name = item.ToUpper(), Icon = "Assets/dropdowniconClose.png" });
                        counter++;
                    }

                    _allGroups = groupedItems;
                    UpdateListContent();
                }

                List_RelationalGrid.HeightRequest = (counter * 90);
                line_relatedgrid.IsVisible = (counter * 90) <= 0 ? false : true;
                #endregion
            }
            catch (Exception ex)
            {
            }

            grd_footer.IsVisible = true;
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            try
            {
                int selectedIndex = _expandedGroups.IndexOf(
                      ((ChillerEntityGroup)((Button)sender).CommandParameter));
                _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
            }
            catch (Exception)
            {
            }
            UpdateListContent();
        }

        private void UpdateListContent()
        {
            try
            {
                _expandedGroups = new ObservableCollection<ChillerEntityGroup>();
                foreach (ChillerEntityGroup group in _allGroups)
                {
                    ChillerEntityGroup newGroup = new ChillerEntityGroup(group.Title, group.ShortName, group.Expanded);
                    if (group.Expanded)
                    {
                        foreach (ChillerEntity food in group)
                        {
                            newGroup.Add(food);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                List_RelationalGrid.ItemsSource = _expandedGroups;
            }
            catch (Exception)
            {
            }
        }

        void EmailLink(EntityListMBView saveentity)
        {
            try
            {
                string shareurl = String.Empty;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    string subject = Functions.UserFullName + " wants to share this Entity with you: " + saveentity.EntityDetails.EntityTypeName;
                    subject = WebUtility.UrlEncode(subject).Replace("+", "%20");

                    string body = "Please Visit this URL:  " + App.EntityImgURL + "/EntityView.aspx?EntityID=" + _entityListMBView.EntityDetails.EntityID;

                    body = WebUtility.UrlEncode(body).Replace("+", "%20");

                    var email = Regex.Replace("", @"[^\u0000-\u00FF]", string.Empty);
                    shareurl = "mailto:?subject=" + WebUtility.UrlEncode(subject) + "&body=" + WebUtility.UrlEncode(body);
                }
                else
                {
                    string subject = Functions.UserFullName + " wants to share this Entity with you: " + saveentity.EntityDetails.EntityTypeName;
                    string body = "Please Visit this URL:  " + App.EntityImgURL + "/EntityView.aspx?EntityID=" + _entityListMBView.EntityDetails.EntityID;
                    shareurl = "mailto:?subject=" + subject + "&body=" + body;
                }
                Device.OpenUri(new Uri(shareurl));
            }
            catch (Exception)
            {
            }
        }

        async void Copylink(EntityListMBView _Entitydata)
        {
            string body = "http://entities-s-15.boxerproperty.com/EntityView.aspx?EntityID=" + _Entitydata.EntityDetails.EntityID;

            //CrossClipboard.Current.SetText(body);

            //To read the clipboard
            //string clipboardText = await CrossClipboard.Current.GetTextAsync();
        }

        private async void gridEntitynotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            //try
            //{
            //    var notes = gridEntitynotes.SelectedItem as Entity_Notes;
            //    if (notes.ImageVisible)
            //    {
            //        await Navigation.PushAsync(new ViewAttachment
            //        (notes.ImageURL));
            //    }
            //    gridEntitynotes.SelectedItem = null;
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private async void Btn_addnotes_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txt_EntNotes.Text))
                {
                    await EntityAddnotes(txt_EntNotes.Text);
                }
                else
                {
                    DisplayAlert(null, "Please enter notes.", "Ok");
                }
            }
            catch (Exception)
            {
                txt_EntNotes.Text = "";
            }
            txt_EntNotes.Text = "";
        }

        private async Task<string> EntityAddnotes(string EntityNotes)
        {
            string recID = string.Empty;
            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

            try
            {
                await Task.Run(async () =>
                {
                    recID = await EntitySyncAPIMethods.StoreEntityNotes(App.Isonline, _entityListMBView.EntityDetails.EntityTypeID, _entityListMBView.EntityDetails.EntityID.ToString(), EntityNotes, Functions.UserName, "Notes Type ID is Static", (Enum.GetNames(typeof(ActionTypes)))[1], App.DBPath, Functions.UserFullName);
                });
                if (!string.IsNullOrEmpty(recID?.ToString()) && recID.ToString() != "[]" && recID != "-1")
                {
                    if (Groups.Count != 0)
                    {
                        if (EntityNotes.Contains("<img"))
                        {
                            Groups.Insert(0, new EntityNotesGroup("", DateTime.Now.ToString(), Functions.UserFullName)
                            {
                                new Entity_Notes
                                {
                                    Note = Functions.HTMLToText( EntityNotes ),
                                    ImageURL = App.EntityImgURL + "/" + Functions.HTMLToText(EntityNotes.Replace("'", "\"").Split('\"')[1]),
                                    ImageVisible = true,
                                    LabelVisible = true
                                }
                            });
                        }
                        else
                        {
                            Groups.Insert(0, new EntityNotesGroup("", DateTime.Now.ToString(), Functions.UserFullName)
                            {
                                new Entity_Notes
                                {
                                    Note = Functions.HTMLToText( EntityNotes ),
                                    htmlNote = EntityNotes,
                                    ImageVisible = false,
                                    LabelVisible = true
                                }
                            });
                        }
                    }
                    else
                    {
                        if (EntityNotes.Contains("<img"))
                        {
                            Groups.Add(new EntityNotesGroup("", DateTime.Now.ToString(), Functions.UserFullName)
                            {
                                new Entity_Notes
                               {
                                    Note = Functions.HTMLToText( EntityNotes ),
                                    ImageURL = App.EntityImgURL + "/" + Functions.HTMLToText(EntityNotes.Replace("'", "\"").Split('\"')[1]),
                                    ImageVisible = true,
                                    LabelVisible = true
                                }
                            });
                        }
                        else
                        {
                            Groups.Add(new EntityNotesGroup("", DateTime.Now.ToString(), Functions.UserFullName)
                            {
                                new Entity_Notes
                                {
                                    Note = Functions.HTMLToText( EntityNotes ),
                                    htmlNote = EntityNotes,
                                    ImageVisible = false,
                                    LabelVisible = true
                                }
                            });
                        }

                        gridEntitynotes.ItemsSource = Groups;

                        FormattedString sa = new FormattedString();
                        try
                        {
                            sa.Spans.Add(new Span { Text = Functions.UserFullName + "\r\n", FontSize = 14 });
                            sa.Spans.Add(new Span { Text = (DateTime.Now).ToString(), FontSize = 14 });

                            EntityLists.EntityModifiedByFullName = Functions.UserFullName;
                            EntityLists.EntityModifiedDateTime = (DateTime.Now).ToString();
                        }
                        catch (Exception)
                        {
                        }
                        lbl_modifiedname.FormattedText = sa;
                    }
                }
            }
            catch (Exception)
            {
            }

            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);

            return recID;
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

        private async void btn_add_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreateEntityPage(_entityListMBView.EntityDetails.EntityTypeID, _entityListMBView.EntityDetails.EntityID, _entityListMBView.EntityDetails.EntityTypeName, _entityListMBView));
        }

        private async void btn_editentity_Clicked(object sender, EventArgs e)
        {
            Functions.IsEditEntity = true;
            await Navigation.PushAsync(new CreateEntityPage(_entityListMBView.EntityDetails.EntityTypeID, _entityListMBView.EntityDetails.EntityID, _entityListMBView.EntityDetails.EntityTypeName, _entityListMBView));
        }

        private async void btn_more_Clicked(object sender, EventArgs e)
        {
            string action = string.Empty;
            try
            {
                if (EntityLists.EntityTypeSecurityType != null)
                {
                    string[] buttons;
                    bool hasDeleteRightOnEntityType = EntityLists.EntityTypeSecurityType != null && (EntityLists.EntityTypeSecurityType.ToLowerInvariant().Equals("open") || EntityLists.EntityTypeSecurityType.ToLowerInvariant().Contains("'d"));

                    bool fields = EntityLists.EntityTypeSecurityType.ToLowerInvariant().Contains("u") || EntityLists.EntityTypeSecurityType.ToLowerInvariant().Equals("open");
                    bool CurrentuserOwner = false;
                    if (EntityLists.EntitiyOwnedByUserName == Functions.UserName)
                    {
                        CurrentuserOwner = true;
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.Append("Switch to ");
                    if (App.Isonline)
                        sb.Append("Offline Mode");
                    else
                        sb.Append("Online Mode");
                    if (fields)
                    {
                        if (hasDeleteRightOnEntityType)
                        {
                            buttons = new string[] { sb.ToString(), "Assign", "Take Ownership", "Forward Item", "Delete Item", "Email Link", "Add Attachment", "Activity Log", "About", "Logout" };
                        }
                        else
                        {
                            buttons = new string[] { sb.ToString(), "Assign", "Take Ownership", "Forward Item", "Email Link", "Add Attachment", "Activity Log", "About", "Logout" };
                        }
                    }
                    else
                    {
                        buttons = new string[] { sb.ToString(), "Email Link", "Activity Log", "About", "Logout" };
                    }

                    if (CurrentuserOwner)
                    {
                        var list = new List<string>(buttons);
                        list.RemoveAt(Array.FindIndex(buttons, s => s.Equals("Take Ownership")));
                        buttons = list.ToArray();
                    }

                    action = await DisplayActionSheet("", "Cancel", null, buttons);

                    if (action.ToLower().Contains("offline"))
                        action = "offline";
                    else if (action.ToLower().Contains("online"))
                        action = "online";
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
                        case "Logout":
                            App.Logout(this);
                            break;
                        case "About":
                            DisplayAlert("App Info", Functions.Appinfomsg, "Ok");
                            break;
                        case "Assign":
                            await Navigation.PushAsync(new EntitySearchUser(EntityLists, "Assign"));
                            break;
                        case "Activity Log":
                            await Navigation.PushAsync(new EntityActivityLogPage(Convert.ToString(_entityListMBView.EntityDetails.EntityID)));
                            break;
                        case "Delete Item":
                            try
                            {
                                switch (await DisplayActionSheet("Are you sure to delete this record?", "Cancel", null, "Yes", "No"))
                                {
                                    case "Yes":
                                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                        bool success = false;
                                        await Task.Run(() =>
                                        {
                                            var temp = EntitySyncAPIMethods.DeleteEntityItem(App.Isonline, _entityListMBView.EntityDetails.EntityID, _entityListMBView.EntityDetails.EntityTypeID, Functions.UserName, App.DBPath);
                                            temp.Wait();
                                            success = temp.Result;
                                        });

                                        if (success)
                                        {
                                            EntityOrgCenterList t = new EntityOrgCenterList
                                            {
                                                EntityTypeID = _entityListMBView.EntityDetails.EntityTypeID,
                                                EntityTypeName = _entityListMBView.EntityDetails.EntityTypeName.ToString()
                                            };
                                            await Navigation.PushAsync(new EntityDetailsSubtype(t, null));
                                        }
                                        Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                                        break;
                                }
                            }
                            catch (Exception)
                            {
                            }
                            break;

                        case "Take Ownership":
                            try
                            {
                                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                bool IsOwnerChangeed = false;
                                await Task.Run(() =>
                                {
                                    IsOwnerChangeed = EntitySyncAPIMethods.TakeOwnership(App.Isonline, Functions.UserName, _entityListMBView.EntityDetails.EntityID, _entityListMBView.EntityDetails.EntityTypeID, App.DBPath);
                                });
                                Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                                if (IsOwnerChangeed)
                                {
                                    Assign_Sam = EntityLists.EntitiyAssignedToUserName;
                                    Create_Sam = EntityLists.EntityCreatedByUserName;
                                    Modify_Sam = EntityLists.EntityModifiedByUserName;
                                    Owner_Sam = EntityLists.EntitiyOwnedByUserName;


                                    FormattedString s = new FormattedString();
                                    if (EntityLists.EntityOwnedByFullName != null)
                                    {
                                        EntityLists.EntityOwnedByFullName = Functions.UserFullName;
                                        EntityLists.EntityOwnedByDateTime = DateTime.Now.ToString();
                                        EntityLists.EntitiyOwnedByUserName = Functions.UserName;

                                        s.Spans.Add(new Span { Text = Functions.UserFullName + "\r\n", FontSize = 14 });
                                        s.Spans.Add(new Span { Text = DateTime.Now.ToString(), FontSize = 14 });
                                    }
                                    lbl_ownername.Text = Convert.ToString(s);

                                    s = new FormattedString();
                                    if (EntityLists.EntityModifiedByFullName != null)
                                    {
                                        EntityLists.EntityModifiedByFullName = Functions.UserFullName;
                                        EntityLists.EntityModifiedDateTime = DateTime.Now.ToString();
                                        EntityLists.EntityModifiedByUserName = Functions.UserName;
                                        s.Spans.Add(new Span { Text = Functions.UserFullName + "\r\n", FontSize = 14 });
                                        s.Spans.Add(new Span { Text = DateTime.Now.ToString(), FontSize = 14 });
                                    }
                                    lbl_modifiedname.Text = Convert.ToString(s);

                                    if (!string.IsNullOrEmpty(txt_EntNotes.Text))
                                    {
                                        await EntityAddnotes(txt_EntNotes.Text);
                                    }

                                    var TempList = DBHelper.GetAppTypeInfoByNameTypeIdScreenInfo(ConstantsSync.EntityInstance, EntityItemView, _entityListMBView.EntityDetails.EntityTypeID, App.DBPath, null);
                                    TempList.Wait();

                                    CommonConstants.AddRecordOfflineStore_AppTypeInfo(JsonConvert.SerializeObject(EntityLists), EntityInstance, EntityItemView, INSTANCE_USER_ASSOC_ID, App.DBPath, TempList.Result.APP_TYPE_INFO_ID, _entityListMBView.EntityDetails.EntityTypeID.ToString(), _entityListMBView.EntityDetails.TransactionType, _entityListMBView.EntityDetails.EntityID.ToString(), 0, _entityListMBView.EntityDetails.EntityTypeName, "").Wait();

                                }
                                else
                                {
                                    DisplayAlert(null, "Owner Not Changed. try again later.", "Cancel");
                                }
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case "Forward Item":
                            await Navigation.PushAsync(new EntitySearchUser(EntityLists, "Forward"));
                            break;
                        case "Edit Item":

                            break;
                        case "Add Attachment":

                            try
                            {
                                if (App.Isonline)
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
                                            {
                                                File_Name = file.Path.Substring(file.Path.LastIndexOf('/') + 1);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        string FileId = string.Empty;
                                        Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                        await Task.Run(() =>
                                        {
                                            var d = EntityAPIMethods.AddFileToEntity(_entityListMBView.EntityDetails.EntityID.ToString(), "", DateTime.Now.Date.ToString("MM/dd/yyyy"), File_Name, size.ToString(), fileBytes, "", 'N', _entityListMBView.EntityDetails.EntityTypeSystemCode, 'y', Functions.UserName);

                                            FileId = d.GetValue("ResponseContent").ToString();
                                        });

                                        if (!string.IsNullOrEmpty(FileId?.ToString()) && FileId.ToString() != "[]")
                                        {
                                            string Notes = "<a href =\"Download.aspx?FileID=" + FileId + "&amp;EntityId=" + _entityListMBView.EntityDetails.EntityID + "\"><img class='entity_note_image' src=\"Download.aspx?FileID=" + FileId + "&amp;EntityId=" + _entityListMBView.EntityDetails.EntityID + "\"/>  " + File_Name + "</a>";

                                            var Re = await EntityAddnotes(Notes);
                                            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);

                                            if (!string.IsNullOrEmpty(txt_EntNotes.Text))
                                            {
                                                await EntityAddnotes(txt_EntNotes.Text);
                                                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                            }

                                            Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                                            if (!string.IsNullOrEmpty(Re))
                                                Functions.ShowtoastAlert("File Attached Successfully.");
                                            else
                                                Functions.ShowtoastAlert("Something went wrong in File Attachment. Please try again later.");
                                        }
                                        else
                                        {
                                            Functions.ShowtoastAlert("Something went wrong in File Attachment. Please try again later.");
                                        }

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
                                    Functions.ShowtoastAlert(Functions.Goonline_forFunc);
                            }
                            catch (Exception)
                            {
                            }
                            txt_EntNotes.Text = "";
                            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
                            break;
                        case "Copy Link":
                            Copylink(_entityListMBView);
                            break;
                        case "Email Link":
                            EmailLink(_entityListMBView);
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private async void List_RelationalGrid_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (App.Isonline)
                {
                    var item = ((ListView)sender).SelectedItem;
                    if (item == null)
                        return;
                    else
                    {
                        await Navigation.PushAsync(new Entity_AppList(item as ChillerEntity, _entityListMBView));
                    }
                }
                else
                    DisplayAlert("", Functions.Goonline_forFunc, "Ok");
            }
            catch (Exception)
            {
            }
            ((ListView)sender).SelectedItem = null;
        }



        string Assign_Sam = string.Empty;
        string Create_Sam = string.Empty;
        string Modify_Sam = string.Empty;
        string Owner_Sam = string.Empty;

        private async void lbl_createname_tapped(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Create_Sam))
                {
                    await this.Navigation.PushAsync(new UserDetail(Create_Sam));
                    //   this.finish();
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
}