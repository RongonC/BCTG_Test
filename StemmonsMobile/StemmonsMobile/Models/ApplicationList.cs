using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;


namespace StemmonsMobile.Models
{
    public class ApplicationList
    {

        public string ApplicationName { get; set; }
        public string ApplicationIcon { get; set; }
        public string My_Icon { get; set; }
        public string New_Icon { get; set; }
        public string Find_Icon { get; set; }
        public string Arrow_Icon { get; set; }


        public List<ApplicationList> GetApplicationList()
        {
            List<ApplicationList> applistdata = new List<ApplicationList>()
            {
                new ApplicationList(){
                    ApplicationName = "Cases",
                    ApplicationIcon = "Assets/Casesblack_1.png",
                    My_Icon = "Assets/actor1.png",
                    New_Icon = "Assets/plusGrey.png",
                    Find_Icon = "Assets/magnify.png",
                    Arrow_Icon = "Assets/downarrow.png"
                },
                new ApplicationList(){
                    ApplicationName = "Entities",
                    ApplicationIcon = "Assets/entitiesblack_1.png",
                    My_Icon = "Assets/actor1.png",
                    New_Icon = "Assets/plusGrey.png",
                    Find_Icon = "Assets/magnify.png",
                    Arrow_Icon = "Assets/downarrow.png"
                },
                new ApplicationList(){
                    ApplicationName = "Departments",
                    ApplicationIcon = "Assets/Departmentblack_1.png",
                    My_Icon = "Assets/actor1.png",
                    New_Icon = "Assets/plusGrey.png",
                    Find_Icon = "Assets/magnify.png",
                    Arrow_Icon = "Assets/downarrow.png"
                },
                new ApplicationList(){
                    ApplicationName = "Standards",
                    ApplicationIcon = "Assets/standardsblack.png",
                    My_Icon = "Assets/actor1.png",
                    New_Icon = "Assets/plusGrey.png",
                    Find_Icon = "Assets/magnify.png",
                    Arrow_Icon = "Assets/downarrow.png"
                },

                new ApplicationList(){
                    ApplicationName = "Quest",
                    ApplicationIcon = "Assets/Questblack_1.png",
                    My_Icon = "Assets/actor1.png",
                    New_Icon = "Assets/plusGrey.png",
                    Find_Icon = "Assets/magnify.png",
                    Arrow_Icon = "Assets/downarrow.png"
                },
            };
            return applistdata;
        }
    }
}

