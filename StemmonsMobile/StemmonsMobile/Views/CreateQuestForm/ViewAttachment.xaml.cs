using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StemmonsMobile.Views.CreateQuestForm
{
    public partial class ViewAttachment : ContentPage
    {
        string FieldId = String.Empty;
        public ViewAttachment(string fieldid)
        {
            InitializeComponent();
            FieldId = fieldid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.SetConnectionFlag();

			AttachmentImage.Source = ImageSource.FromUri(new Uri("http://quest-s-15.boxerproperty.com/Download.aspx?FileID=" + FieldId));
            //AttachmentImage.Source = new UriImageSource
            //{
            //    Uri = new Uri("http://quest-s-15.boxerproperty.com/Download.aspx?FileID=" + FieldId),
            //    CachingEnabled = true,
            //    CacheValidity = new TimeSpan(5, 0, 0, 0)
            //};

            //AttachmentImage.Source = "Assets\alertIcon.png";

        }

    }
}
