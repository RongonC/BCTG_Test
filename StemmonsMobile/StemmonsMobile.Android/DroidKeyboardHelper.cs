using System;
using Xamarin.Forms;

using Xamarin.Forms.Platform.Android;
using Android.Views.InputMethods;
using Android.App;
using Android.Content;
using StemmonsMobile.Droid;

[assembly: Dependency(typeof(DroidKeyboardHelper))]
namespace StemmonsMobile.Droid
{
    public class DroidKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            var context = Android.App.Application.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            if (inputMethodManager != null && context is Activity)
            {
                var activity = context as Activity;
                var token = activity.CurrentFocus?.WindowToken;
                inputMethodManager.HideSoftInputFromWindow(token, HideSoftInputFlags.None);

                activity.Window.DecorView.ClearFocus();
            }
        }
    }
}