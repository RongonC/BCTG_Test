using System;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using App;
using App.Droid;
using StemmonsMobile;

[assembly: ExportRenderer(typeof(RoundRectLabel), typeof(RoundRectLabelRenderer))]

namespace App.Droid
{
    public class RoundRectLabelRenderer : LabelRenderer
    {
        public RoundRectLabelRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
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

            if (e.PropertyName == RoundRectLabel.TextProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == RoundRectLabel.BackgroundColorProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == RoundRectLabel.BorderColorProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == RoundRectLabel.BorderWidthProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == RoundRectLabel.CornerRadiusProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
            if (e.PropertyName == RoundRectLabel.HiddenValueProperty.PropertyName)
            {
                Control.Invalidate();
                return;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var label = Element as RoundRectLabel;

            var background = new Android.Graphics.Drawables.GradientDrawable();
            background.SetCornerRadius((int)label.CornerRadius);
            background.SetStroke((int)label.BorderWidth, label.BorderColor.ToAndroid());
            background.SetColor(label.BackgroundColor.ToAndroid());
            SetBackgroundColor(Android.Graphics.Color.Transparent);
            SetBackgroundDrawable(background);
        }
    }
}


