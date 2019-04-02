using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServiceBus.OnlineHelper.DataTypes;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StemmonsMobile.DataTypes.DataType.Standards;
using StemmonsMobile.DataTypes.DataType.Standards.DataTypes;
using static StemmonsMobile.Commonfiles.Functions;
using DataServiceBus.OfflineHelper.DataTypes.Standards;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using StemmonsMobile.Views.LoginProcess;
using System.ComponentModel;

namespace StemmonsMobile.Views.Standards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandardsBookPage : ContentPage
    {
        ObservableCollection<Group_Standards> Master_list = new ObservableCollection<Group_Standards>();
        private ObservableCollection<Group_Standards> _expandedGroups;
        ObservableCollection<Group_Standards> Search_List = new ObservableCollection<Group_Standards>();
        string Screenname = string.Empty;
        public StandardsBookPage(string _screenName)
        {
            InitializeComponent();
            Screenname = _screenName;

            if (string.IsNullOrEmpty(Screenname))
            {
                Title = "Standards Book List";
            }
            else if (Screenname == "MostPopular")
            {
                Title = "Most Popular Book List";
            }
            else if (Screenname == "Whatsnew")
            {
                Title = "Recent Book List";
            }
            UpdateListContent();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();
            Txt_SearchBar.Text = "";
            Master_list.Clear();
            JToken Response = null;
            try
            {
                Functions.ShowOverlayView_Grid(overlay, true, masterGrid);
                await Task.Run(() =>
                {
                    var ResultSyncOffline = StandardsSyncAPIMethods.GetBookList(App.Isonline, ConstantsSync.StandardInstance, Functions.UserName, App.DBPath).Result;
                    Response = JToken.Parse(JsonConvert.SerializeObject(ResultSyncOffline));
                });

                if (!string.IsNullOrEmpty(Response.ToString()) && Response.ToString() != "[]")
                {
                    int i = 0;
                    foreach (var Bookitem in Response)
                    {
                        var name = Enum.GetNames(typeof(BookCategoryTypes))[i];
                        i++;

                        if (string.IsNullOrEmpty(Screenname))
                        {
                            if (name == "All")
                                name = "All";
                            if (name == "MostPopular")
                                name = "Most Popular";
                            if (name == "Whatsnew")
                                name = "Whats New";
                            if (name == "ForMe")
                                name = "For Me";
                        }
                        else if (Screenname == "MostPopular")
                        {
                            if (name == "MostPopular")
                                name = "Most Popular";
                            else
                                continue;

                        }
                        else if (Screenname == "Whatsnew")
                        {
                            if (name == "Whatsnew")
                                name = "Whats New";
                            else
                                continue;
                        }

                        // Root name

                        Group_Standards Bookitems = new Group_Standards(name);

                        foreach (var collection in Bookitem)
                        {
                            foreach (var Child in collection)
                            {
                                Bookitems.Add(JsonConvert.DeserializeObject<BookCollection>(Child.ToString()));
                            }
                        }
                        try
                        {
                            if (Bookitems.Count > 0)
                                Master_list.Add(Bookitems);
                            else
                            {
                                if (!string.IsNullOrEmpty(Screenname))
                                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    listViewStandards.ItemsSource = Master_list;
                }
                else
                {
                    await DisplayAlert(null, App.Isonline ? Functions.nRcrdOnline : Functions.nRcrdOffline, "Ok");
                }
            }
            catch (Exception ex)
            {

            }
            Functions.ShowOverlayView_Grid(overlay, false, masterGrid);
            UpdateListContent();
        }
        private void UpdateListContent()
        {
            try
            {
                _expandedGroups = new ObservableCollection<Group_Standards>();
                foreach (Group_Standards group in Master_list)
                {
                    //Create new FoodGroups so we do not alter original list
                    Group_Standards newGroup = new Group_Standards(group.Title, group.Expanded);
                    //Add the count of food items for Lits Header Titles to use

                    if (group.Expanded)
                    {
                        foreach (BookCollection food in group)
                        {
                            newGroup.Add(food);
                        }
                    }
                    _expandedGroups.Add(newGroup);
                }
                listViewStandards.ItemsSource = _expandedGroups;
            }
            catch (Exception)
            {
            }
        }

        private async void Standards_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView lst = (ListView)sender;
            Grp_StandardDetails _SelectedNodes = new Grp_StandardDetails();
            _SelectedNodes.APP_ID = (e.Item as BookCollection).APP_ID;
            _SelectedNodes.APP_NAME = (e.Item as BookCollection).APP_NAME;
            await Navigation.PushAsync(new BookContentsPage(_SelectedNodes));
            lst.SelectedItem = null;
        }

        private void Txt_SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search_List.Clear();
            try
            {
                if (string.IsNullOrEmpty(e.NewTextValue))
                {
                    listViewStandards.ItemsSource = Master_list;
                }
                else
                {
                    int i = 0;
                    foreach (var item in Master_list)
                    {
                        var name = Enum.GetNames(typeof(BookCategoryTypes))[i];
                        i++;
                        if (name == "All")
                            name = "All";
                        if (name == "MostPopular")
                            name = "Most Popular";
                        if (name == "Whatsnew")
                            name = "What’s New";
                        if (name == "ForMe")
                            name = "For Me";
                        // Root name
                        var forMe = new Group_Standards(Title = name);
                        var t = item.Where(x => x.APP_NAME.ToLower().Contains(e.NewTextValue.ToLower())).ToList();
                        //forMe.Add(item);
                        foreach (var TT in t)
                        {
                            forMe.Add(TT);
                        }
                        Search_List.Add(forMe);
                    }
                    listViewStandards.ItemsSource = Search_List;
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

        private void Btn_more_Clicked(object sender, EventArgs e)
        {
            try
            {
                App.BtnHumburger(this);

            }
            catch (Exception)
            {
            }
        }

        private void HeaderTapped(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = _expandedGroups.IndexOf(
               ((Group_Standards)((Button)sender).CommandParameter));
                Master_list[selectedIndex].Expanded = !Master_list[selectedIndex].Expanded;
                UpdateListContent();

            }
            catch (Exception ex)
            {

            }
        }
    }
}