using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using StemmonsMobile.Controls;
using StemmonsMobile.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PdfWebView), typeof(PdfViewRenderer))]
namespace StemmonsMobile.iOS.CustomRenderers
{
    public class PdfViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (NativeView != null && e.NewElement != null)
            {
                var pdfControl = NativeView as UIWebView;

                if (pdfControl == null)
                    return;

                pdfControl.ScalesPageToFit = true;
            }
        }
    }
}