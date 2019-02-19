using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using StemmonsMobile;
using StemmonsMobile.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(RoundRectLabel), typeof( RoundRectLabelRenderer))]

namespace StemmonsMobile.iOS.CustomRenderers
{
    public class RoundRectLabelRenderer : LabelRenderer
    {
        public RoundRectLabelRenderer()
        {
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //          var label = Element as RoundRectLabel;

            if (e.PropertyName == RoundRectLabel.BackgroundColorProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == RoundRectLabel.BorderColorProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == RoundRectLabel.BorderWidthProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == RoundRectLabel.CornerRadiusProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == RoundRectLabel.HiddenValueProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var label = Element as RoundRectLabel;

            Layer.BorderWidth = label.BorderWidth;
            Layer.CornerRadius = label.CornerRadius;
            Layer.BackgroundColor = label.BackgroundColor.ToCGColor();
            Layer.BorderColor = label.BorderColor.ToCGColor();
        }
    }
}


