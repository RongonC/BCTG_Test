using StemmonsMobile.Models;
using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.LoginProcess
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectCaseType : ContentPage
    {
        public static NavigationPage MyNavigationPage;
        CaseTypeViewModel cm;
        public SelectCaseType()
        {

            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            cm = new CaseTypeViewModel();
            listCaseType1.ItemsSource = cm.casedata;
            cm = new CaseTypeViewModel();
            listCaseType2.ItemsSource = cm.casedata;
        }
        void Handle_SearchButtonPressed(object sender, System.EventArgs e)
        {
            FavtButton.IsVisible = false;
            listCaseType1.IsVisible = false;
            var keyword = Searchbar1.Text;
            var suggestions1 = cm.casedata.Where(c => c.casetypename.Contains(keyword));
            listCaseType2.ItemsSource = suggestions1;

            var TempWidth = Application.Current.MainPage.Width - 40;





        }
        void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {


            String temp = Searchbar1.Text;

            listCaseType1.ItemsSource = null;
            listCaseType2.ItemsSource = null;

            cm = new CaseTypeViewModel();
            listCaseType1.ItemsSource = cm.casedata;
            cm = new CaseTypeViewModel();
            listCaseType2.ItemsSource = cm.casedata;

            FavtButton.IsVisible = true;
            listCaseType1.IsVisible = true;

        }
        void Handle_Clicked(object sender, System.EventArgs e)
        {



            var mi = sender as MenuItem;
            var vm = BindingContext as CaseTypeViewModel;
            var data = mi.BindingContext as casetype;
            var casedata12 = (data.casetypename) as string;


            var suggestions = cm.casedata.Where(c => c.casetypename.Contains(casedata12));
            var casetypeIndex = cm.casedata.IndexOf(data);
            cm.casedata.RemoveAt(casetypeIndex);
            listCaseType1.ItemsSource = null;
            listCaseType1.ItemsSource = cm.casedata;


        }

        void Handle_Clicked1(object sender, System.EventArgs e)
        {



            var mi = sender as MenuItem;
            var vm = BindingContext as CaseTypeViewModel;
            var data = mi.BindingContext as casetype;
            var casedata12 = (data.casetypename) as string;


            var suggestions = cm.casedata.Where(c => c.casetypename.Contains(casedata12));
            var casetypeIndex = cm.casedata.IndexOf(data);
            cm.casedata.RemoveAt(casetypeIndex);
            listCaseType2.ItemsSource = null;
            listCaseType2.ItemsSource = cm.casedata;


        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem;



            //throw new NotImplementedException();
        }

        async void Handle_ItemSelected1(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem;



            //throw new NotImplementedException();
        }

        async void BackButton_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void Handle_MyFavClick(object sender, System.EventArgs e)
        {
            if (listCaseType1.IsVisible)
            {
                listCaseType1.IsVisible = false;
                FavtButton.Image = "dropdowniconClose.png";
            }
            else
            {

                listCaseType1.IsVisible = true;
                FavtButton.Image = "dropdownicon.png";



            }
        }

        void Handle_CaseTypeClick(object sender, System.EventArgs e)
        {
            if (listCaseType2.IsVisible)
            {
                listCaseType2.IsVisible = false;
                CaseTypeButton.Image = "dropdowniconClose.png";
            }
            else
            {

                listCaseType2.IsVisible = true;
                CaseTypeButton.Image = "dropdownicon.png";
            }
        }

    }


}
