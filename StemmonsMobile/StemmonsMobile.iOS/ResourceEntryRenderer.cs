using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using StemmonsMobile;
using StemmonsMobile.Views;

[assembly: ExportRenderer(typeof(EntryRender), typeof(StemmonsMobile.iOS.ResourceRectEditorRenderer))]
namespace StemmonsMobile.iOS
{
    public class ResourceRectEditorRenderer : EntryRender
    {
        public ResourceRectEditorRenderer()
        {
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            //          var label = Element as RoundRectLabel;

            if (e.PropertyName == EntryRender.BackgroundColorProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == EntryRender.BorderColorProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == EntryRender.BorderWidthProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }
            if (e.PropertyName == EntryRender.CornerRadiusProperty.PropertyName)
            {
                SetNeedsLayout();
                return;
            }

            if (e.PropertyName == EntryRender.H) {

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
