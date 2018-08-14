using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StemmonsMobile.Views.Search
{
    public partial class SearchList : ContentPage
    {
        public SearchList()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            InstanceList.ItemsSource = new string[]{ "Search People", "Search Cases", "Search Entities", "Search Standards" };
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            this.Navigation.PushAsync(new SearchPage());
        }

        void BackClicked_Clicked(object sender, System.EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}
