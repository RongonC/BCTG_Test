using System;
using System.Collections.Generic;
using Xamarin.Forms;
using StemmonsMobile.Models;


namespace StemmonsMobile.ViewModels
{
    public class ApplicationListViewModel : ContentPage
    {
        public List<ApplicationList> appdata { get; set; }
        public ApplicationListViewModel()
        {
            appdata = new ApplicationList().GetApplicationList();
        }
    }

}

