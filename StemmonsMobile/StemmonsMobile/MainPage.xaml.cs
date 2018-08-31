using StemmonsMobile.Commonfiles;
using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            //var editText = new Entry
            //{
            //    Placeholder = "Select Date.",
            //};

            //var date = new DatePicker
            //{
            //    IsVisible = false,
            //    IsEnabled = false,
            //};

            //var stack = new StackLayout
            //{
            //    Orientation = StackOrientation.Vertical,
            //    Children = { editText, date }
            //};

            //editText.Focused += (sender, e) =>
            //{
            //    date.Focus();
            //    //date.IsVisible = true;
            //    //date.IsEnabled = true;
            //    //editText.Unfocus();
            //};
            //date.DateSelected += (sender, e) =>
            //{
            //    editText.Text = date.Date.ToString();
            //};

            //Content = stack;
        }

        private void En_Unfocused(object sender, FocusEventArgs e)
        {
            //foreach (var item in mainStack.Children)
            //{
            //    Type ty = item.GetType();
            //    if (ty.Name.ToLower() == "entry")
            //    {
            //        var Entry = item as Entry;
            //        Entry.IsVisible = false;
            //    }
            //    //else if (ty.Name.ToLower() == "datepicker")
            //    //{
            //    //    var datepicker = item as DatePicker;
            //    //    datepicker.IsVisible = true;
            //    //    datepicker.Focus();
            //    //}
            //}
        }

        private void En_Focused(object sender, FocusEventArgs e)
        {
            //foreach (var item in mainStack.Children)
            //{
            //    Type ty = item.GetType();
            //    if (ty.Name.ToLower() == "entry")
            //    {

            //    }
            //    else if (ty.Name.ToLower() == "datepicker")
            //    {
            //        var datepicker = item as DatePicker;
            //        datepicker.IsVisible = true;
            //        datepicker.Focus();

            //    }
            //}
        }

    }
}