using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Entity
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Add_newChillar : ContentPage
    {
        public Add_newChillar()
        {
            InitializeComponent();
        }

        private async void Tool_more_Clicked(object sender, EventArgs e)
        {
            //var action = await DisplayActionSheet("  Select Action want to Perform.", "Cancel", null, "Create", "Create & Exit");
            //switch (action)
            //{
            //    case "Create":
            //        await Navigation.PushAsync(new Entity_View());
            //        break;
            //    case "Create & Exit":

            //        break;
            //    case "Cancle":

            //        break;
            //        //...                 
            //}
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //var action = await DisplayActionSheet("  Select Action want to Perform.", "Cancel", null, "Create", "Create & Exit");
            //switch (action)
            //{
            //    case "Create":
            //        await Navigation.PushAsync(new Entity_View());
            //        break;
            //    case "Create & Exit":

            //        break;
            //    case "Cancle":

            //        break;
            //        //...                 
            //}
        }

        private async void menu_create_Clicked(object sender, System.EventArgs e)
        {
            //var temp = await DisplayActionSheet(null, "Cancel", null, "Create", "Create & Exit");
            //switch (temp)
            //{
            //    case "Create":
            //        await Navigation.PushAsync(new Entity_View());
            //        break;


            //    default:
            //        break;
            //}
        }
        public static void CreateChillar()
        {

        }
    }

}
