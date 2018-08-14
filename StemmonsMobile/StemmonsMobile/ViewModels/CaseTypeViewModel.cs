using System;

using System.Collections.Generic;

using Xamarin.Forms;
using StemmonsMobile.Models;


namespace StemmonsMobile.ViewModels
{
    public class CaseTypeViewModel : ContentPage
    {
        public List<casetype> casedata { get; set; }

        public CaseTypeViewModel()
        {

            casedata = new casetype().Getcasedata();
        }




        public Command<casetype> RemoveCommand
        {

            get
            {

                return new Command<casetype>((casetype1) =>
                {
                    casedata.Remove(casetype1);
                 
                });
            }
        }
    }

}

