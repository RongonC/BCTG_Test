using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using StemmonsMobile;
using StemmonsMobile.Views;
using StemmonsMobile.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(BorderEditor), typeof(ResourceRectEditorRenderer))]
namespace StemmonsMobile.iOS.CustomRenderers
{
    public class ResourceRectEditorRenderer : EditorRenderer
    {
        public ResourceRectEditorRenderer()
        {
        }
      
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //          var label = Element as RoundRectLabel;

            if (e.PropertyName == BorderEditor.BackgroundColorProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == BorderEditor.BorderColorProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == BorderEditor.BorderWidthProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == BorderEditor.CornerRadiusProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }

        }
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var label = Element as BorderEditor;

            Layer.BorderWidth = label.BorderWidth;
            Layer.CornerRadius = label.CornerRadius;
            Layer.BackgroundColor = label.BackgroundColor.ToCGColor();
            Layer.BorderColor = label.BorderColor.ToCGColor();
        }
    }
 }

