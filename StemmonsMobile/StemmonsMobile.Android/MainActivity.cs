using System;

using Android.App;
using Android.Content.PM;
using Android.Views;
using Android.OS;
using Acr.UserDialogs;
using System.Linq;
using StemmonsMobile.Views.Cases;
using Android.Content;
using StemmonsMobile.Commonfiles;

namespace StemmonsMobile.Droid
{
    [Activity(Label = "StemmonsMobile", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            UserDialogs.Init(this);
            ImageCircleRenderer.Init();
            LoadApplication(new App());

        }

       


    }
}

