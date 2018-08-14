using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views.Cases
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseTypeDetails : ContentPage
    {
        public CaseTypeDetails()
        {
            InitializeComponent();
            App.SetConnectionFlag();
        }

        public CaseTypeDetails(string caseType) : this()
        {
            this.Title = caseType;
            this.entryPriority.Text = "3 - Medium";
            this.entryStatus.Text = "New";
            this.entryProperty.Text = "720 North Post Oak";
        }


        private void Property_Focused(object sender, FocusEventArgs e)
        {
            try
            {

                Device.BeginInvokeOnMainThread(e.VisualElement.Unfocus);
                this.Navigation.PushAsync(new CaseProperty());
            }
            catch (Exception ex)
            {
               //  
            }
        }

        private void Notes_Focused(object sender, FocusEventArgs e)
        {
            try
            {

                Device.BeginInvokeOnMainThread(e.VisualElement.Unfocus);
                //this.Navigation.PushAsync(new CaseNotes());
            }
            catch (Exception ex)
            {
                //// 
            }
        }
    }
}