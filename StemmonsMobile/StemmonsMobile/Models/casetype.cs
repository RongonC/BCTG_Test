using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;


namespace StemmonsMobile.Models
{
    public class casetype
    {

        public string casetypename { get; set; }
        public string tempimage { get; set; }


        public List<casetype> Getcasedata()
        {
            List<casetype> casedata = new List<casetype>()
            {
                new casetype(){
                    casetypename = "software development",
                    tempimage = "Assets/plussign.png",


                }, new casetype(){
                    casetypename = "Helpdesk",
                    tempimage = "Assets/plussign.png"
                }, new casetype(){
                    casetypename = "Adhoc Project",
                    tempimage = "Assets/plussign.png"
                }, new casetype(){
                    casetypename = "Maintanance Project",
                    tempimage = "Assets/plussign.png"
                }, new casetype(){
                    casetypename = "Property worker order",
                    tempimage = "Assets/plussign.png",

                },
            };
            return casedata;
        }

        internal object ToLower()
        {
            throw new NotImplementedException();
        }
    }

}

