using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Graphics;
using Android.Views;
using StemmonsMobile;
using StemmonsMobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(MainPage), typeof(SpaceNavigationPageRenderer))]
namespace StemmonsMobile.Droid.Renderers
{
    public class SpaceNavigationPageRenderer : NavigationPageRenderer
    {

        private Android.Support.V7.Widget.Toolbar toolbar;

        public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if(child.GetType() == typeof(Android.Support.V7.Widget.Toolbar)){

                toolbar = (Android.Support.V7.Widget.Toolbar)child;
                toolbar.ChildViewAdded += Toolbar_ChildViewAdded;

            }

        }

        void Toolbar_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {

            var view = e.Child.GetType();


            if(e.Child.GetType() == typeof(Android.Widget.TextView))
            {

                var textView = (Android.Widget.TextView)e.Child;
                var spaceFont = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "Stawix - SoinSansNeue-Roman.otf");
                textView.Typeface = spaceFont;
                toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;

            }

            // If your app is not using the AppCompatTextView, you don't need this check
            if (e.Child.GetType() == typeof(Android.Support.V7.Widget.AppCompatTextView))
            {
                var textView = (Android.Support.V7.Widget.AppCompatTextView)e.Child;
                var spaceFont = Typeface.CreateFromAsset(Xamarin.Forms.Forms.Context.ApplicationContext.Assets, "Stawix - SoinSansNeue-Roman.otf");
                textView.Typeface = spaceFont;
                toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
            }


        }
    }
}