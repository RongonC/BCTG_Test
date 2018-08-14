using System;
using System.Collections.Generic;
using Xamarin.Forms;
using StemmonsMobile.Models;
using DataServiceBus.OfflineHelper.DataTypes;

namespace StemmonsMobile.ViewModels
{
    public class InstanceListViewModel : ContentPage
    {
        public List<InstanceList> appdata { get; set; }

        public InstanceListViewModel()
        {
            //appdata = new InstanceList().GetInstanceList();
        }
    }

}

