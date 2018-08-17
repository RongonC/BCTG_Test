using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StemmonsMobile.Commonfiles;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;
using Newtonsoft.Json;
using DataServiceBus.OnlineHelper.DataTypes;
using StemmonsMobile.Views.View_Case_Origination_Center;

namespace StemmonsMobile.Views.Search
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchResult : ContentPage
    {
        public SearchResult(string EmpName)
        {
            InitializeComponent();
            // Calling this constructor
            var result = CasesAPIMethods.GetEmployeesBySearch(EmpName);
            var temp = result.GetValue("ResponseContent");

            if (temp != null && temp.ToString() != "[]")
            {
                UserDataCall userdata;
                List<UserDataCall> d = new List<UserDataCall>();

                foreach (var item in temp)
                {
                    userdata = JsonConvert.DeserializeObject<UserDataCall>(item.ToString());
                    d.Add(userdata);
                }
                SearchDataList.ItemsSource = d;
            }
            else
            {
                DisplayAlert("Error!", result.GetValue("Message").ToString(), "Ok");
            }
        }
    }


}
