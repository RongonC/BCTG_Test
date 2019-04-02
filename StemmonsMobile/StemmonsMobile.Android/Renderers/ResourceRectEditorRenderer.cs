using System;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using App;
using App.Droid;
using StemmonsMobile;
using StemmonsMobile.Views;
using App.Droid.Renderers;

[assembly: ExportRenderer(typeof(BorderEditor), typeof(ResourceRectEditorRenderer))]

namespace App.Droid.Renderers
{
    public class ResourceRectEditorRenderer : EditorRenderer
    {
        public ResourceRectEditorRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
            {
                return;
            }

            if (Control != null)
            {
                Control.Invalidate();
            }
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //          var label = Element as RoundRectLabel;

            if (e.PropertyName == BorderEditor.TextProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == BorderEditor.BackgroundColorProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == BorderEditor.BorderColorProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == BorderEditor.BorderWidthProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == BorderEditor.CornerRadiusProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            //if (e.PropertyName == BorderEditor.)
            //{
            //    Control.Invalidate();
            //    return;
            //}
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var label = Element as BorderEditor;

            var background = new Android.Graphics.Drawables.GradientDrawable();
            background.SetCornerRadius((int)label.CornerRadius);
            background.SetStroke((int)label.BorderWidth, label.BorderColor.ToAndroid());
            background.SetColor(label.BackgroundColor.ToAndroid());
            SetBackgroundColor(Android.Graphics.Color.Transparent);
            SetBackgroundDrawable(background);
        }
    }
}

