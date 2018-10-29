using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using StemmonsMobile.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSKeyboardHelper))]
namespace StemmonsMobile.iOS
{
    public class iOSKeyboardHelper : IKeyboardHelper
    {
        public void HideKeyboard()
        {
            UIApplication.SharedApplication.KeyWindow.EndEditing(true);
        }
    }
}