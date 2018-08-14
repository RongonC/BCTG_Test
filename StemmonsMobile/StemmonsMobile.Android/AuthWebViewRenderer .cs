using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Android.Graphics;
using Android.Webkit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WebView = Xamarin.Forms.WebView;

//[assembly: ExportRenderer(typeof(AuthWebView), typeof(AuthWebViewRenderer))]
namespace StemmonsMobile.Droid
{
    public class AuthWebViewRenderer : Xamarin.Forms.Platform.Android.WebViewRenderer
    {
        AuthWebViewClient _authWebClient = null;
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);
            if (_authWebClient == null)
            {
                _authWebClient = new AuthWebViewClient("", "");
            }
            Control.SetWebViewClient(_authWebClient);
        }
    }
    public class AuthWebViewClient : WebViewClient
    {
        private string Username
        {
            get;
        }
        private string Password
        {
            get;
        }
        public AuthWebViewClient(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public override void OnReceivedHttpAuthRequest(Android.Webkit.WebView view, HttpAuthHandler handler, string host, string realm)
        {
            handler.Proceed(
                Username,
                Password);
        }

        public override bool ShouldOverrideUrlLoading(Android.Webkit.WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }
    }
}