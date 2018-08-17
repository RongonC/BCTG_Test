using DataServiceBus.OfflineHelper.DataTypes.Cases;
using DataServiceBus.OfflineHelper.DataTypes.Common;
using DataServiceBus.OnlineHelper.DataTypes;
using Newtonsoft.Json;
using StemmonsMobile.Commonfiles;
using StemmonsMobile.DataTypes.DataType.Cases;
using StemmonsMobile.Models;
using StemmonsMobile.Views.LoginProcess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Cases
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseProperty : ContentPage
    {
        //  ObservableCollection<Group_Property> Master_list = new ObservableCollection<Group_Property>();
        Grp_PropertyData propertydata;

        private ObservableCollection<UserGroup> _allGroups;
        private ObservableCollection<UserGroup> _expandedGroups;

        public CaseProperty()
        {
            InitializeComponent();

            var result = CasesSyncAPIMethods.GetAllExternalDatasourceList(App.Isonline, "6547", "84915", Functions.UserName, ConstantsSync.INSTANCE_USER_ASSOC_ID, App.DBPath);
            //   var result1 = CasesSyncAPIMethods.Get

            //var temp = result.GetValue("ResponseContent");

            if (result.Result != null && result.Result.ToString() != "[]")
            {
                //ObservableCollection<standards> lst = new ObservableCollection<standards>();
                //lst.Add(JsonConvert.DeserializeObject<standards>(temp.ToString()));

                List<Grp_PropertyData> Property_List = new List<Grp_PropertyData>();
                ObservableCollection<UserGroup> Groups = new ObservableCollection<UserGroup>();
                ObservableCollection<UserGroup> Groups1 = new ObservableCollection<UserGroup>();

                ObservableCollection<Group> groupedItems = new ObservableCollection<Group>();
                Group group1 = new Group("Result - Frequently Used");
                groupedItems.Add(group1);

                Group group = new Group("Result - Full List");
                groupedItems.Add(group);

                foreach (var item in result.Result)
                {
                    propertydata = JsonConvert.DeserializeObject<Grp_PropertyData>(item.ToString());
                    Property_List.Add(propertydata);
                    //group.Add(propertydata);
                }


                listviewCasesProperty.ItemsSource = groupedItems;


                //foreach (var item in temp)
                //{
                //    var name = item.Path;
                //    name = name.Substring(name.IndexOf(".") + 1);

                //    //Frequent List
                //    var frequentList = new Group_Property { Heading = name };
                //    foreach (var collection in item)
                //    {
                //        //full list
                //        Property_List = new List<Grp_PropertyDetails>();
                //        foreach (var Child in collection)
                //        {

                //            frequentList.Add(JsonConvert.DeserializeObject<Grp_PropertyDetails>(Child.ToString()));
                //        }

                //    }
                //    Master_list.Add(frequentList);
                //}
            }
            else
            {
                DisplayAlert(null, result.Result.ToString(), "Cancel");
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

    public class Group : ObservableCollection<ExternalDatasourceList.EXTERNAL_DATASOURCE_LIST>
    {
        public String Title { get; private set; }


        public Group(String title)
        {
            this.Title = title;

        }

        // Whatever other properties
    }

    public class Grp_PropertyData
    {
        public string CASE_TYPE_ID { get; set; }
        public string SYSTEM_PRIORITY { get; set; }
        public string ASSOC_TYPE_ID { get; set; }
        public string ASSOC_TYPE_SYSTEM_CODE { get; set; }
        public string ASSOC_TYPE_DESCRIPTION { get; set; }
        public string ASSOC_TYPE_NAME { get; set; }
        public string EXTERNAL_DATASOURCE_ID { get; set; }
        public string EXTERNAL_DATASOURCE_NAME { get; set; }
        public string EXTERNAL_DATASOURCE_DESCRIPTION { get; set; }
        public string count { get; set; }
    }
}



//  PropertyData();

//   _allGroups = CaseData.AllProperty;
// UpdateListContent();



//private void HeaderTapped(object sender, EventArgs args)
//{
//    int selectedIndex = _expandedGroups.IndexOf(
//        ((CaseData)((Button)sender).CommandParameter));
//    _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
//    UpdateListContent();
//}

//private void UpdateListContent()
//{
//    _expandedGroups = new ObservableCollection<CaseData>();
//    foreach (CaseData group in _allGroups)
//    {
//        //Create new FoodGroups so we do not alter original list
//        CaseData newGroup = new CaseData(group.Title, group.ShortName, group.Expanded);
//        //Add the count of food items for Lits Header Titles to use
//        newGroup.FoodCount = group.Count;
//        if (group.Expanded)
//        {
//            foreach (Food food in group)
//            {
//                newGroup.Add(food);
//            }
//        }
//        _expandedGroups.Add(newGroup);
//    }
//    GroupedView.ItemsSource = _expandedGroups;
//}

//public static void PropertyData()
//{
//    ObservableCollection<CaseData> Groups = new ObservableCollection<CaseData>{
//        new CaseData("Frequently Used","C"){
//            new Food { Name = "123 Main Street", Description = "Carb Snakes",  Icon="right_arrow.png" },
//            new Food { Name = "234 Clover Dale Lane", Description = "The King of all Carbs", Icon="right_arrow.png" },
//            new Food { Name = "7742  Maple Street", Description = "Soft & Gentle", Icon="right_arrow.png" },
//          //  new Food { Name = "rice", Description = "Tiny grains of goodness", Icon="rice.png" },
//        },
//        new CaseData("Full List","D"){
//            new Food { Name = "8874 Peach Street", Description = "Molk", Icon="right_arrow.png"},
//            new Food { Name = "89 Apple Park Road", Description = "Cheese + Potato = <3", Icon="right_arrow.png"},
//            new Food { Name = "952 Frozen Street Lane", Description = "Because I couldn't find an icon for yoghurt", Icon="right_arrow.png"},
//             new Food { Name = "1 Mitech Lane", Description = "Because I couldn't find an icon for yoghurt", Icon="right_arrow.png"},
//        }

//    };
//    CaseData.AllProperty = Groups;
//}
