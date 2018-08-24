using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAttachment : ContentPage
    {
        public ViewAttachment(string imgurl)
        {
            InitializeComponent();

            BackgroundColor = Color.LightGray;
            img.VerticalOptions = LayoutOptions.Center;
            img.HorizontalOptions = LayoutOptions.Center;

            img.Source = new UriImageSource
            {
                Uri = new Uri(imgurl),
                CachingEnabled = true,
                CacheValidity = new TimeSpan(5, 0, 0, 0)

            };
        }
    }
}