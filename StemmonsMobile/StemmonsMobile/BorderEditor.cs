using System;
using Xamarin.Forms;

namespace StemmonsMobile.Views
{
    public class BorderEditor : Editor
    {
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create<BorderEditor, Color>(p => p.BorderColor, Color.Transparent);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create<BorderEditor, float>(p => p.BorderWidth, 0);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create<BorderEditor, float>(p => p.CornerRadius, 0);

        

        public static readonly BindableProperty SetCornerRadius =
            BindableProperty.Create<BorderEditor, Color>(p => p.BorderColor, Color.Transparent);

        public static readonly BindableProperty SetStroke =
            BindableProperty.Create<BorderEditor, float>(p => p.BorderWidth, 0);

        public static readonly BindableProperty SetColor =
            BindableProperty.Create<BorderEditor, float>(p => p.CornerRadius, 0);



        public Color BorderColor
        {
            get
            {
               
                return (Color)GetValue(BorderColorProperty);
            }
            set
            {
                SetValue(BorderColorProperty, value);
            }
        }

        public float BorderWidth
        {
            get
            {
                return (float)GetValue(BorderWidthProperty);
            }
            set
            {
                SetValue(BorderWidthProperty, value);
            }
        }

        public float CornerRadius
        {
            get
            {
                return (float)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }
    }


       
}
