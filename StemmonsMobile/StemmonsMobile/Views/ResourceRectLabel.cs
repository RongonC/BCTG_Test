using System;

using Xamarin.Forms;

namespace StemmonsMobile
{
    public class RoundRectLabel : Label
    {
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create<RoundRectLabel, Color>(p => p.BorderColor, Color.Transparent);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create<RoundRectLabel, float>(p => p.BorderWidth, 0);

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create<RoundRectLabel, float>(p => p.CornerRadius, 0);

        public static readonly BindableProperty HiddenValueProperty =
            BindableProperty.Create<RoundRectLabel, string>(p => p.HiddenValue, null);

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            UpdateVisible();
        }

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

        public string HiddenValue
        {
            get
            {
                return (string)GetValue(HiddenValueProperty);
            }
            set
            {
                SetValue(HiddenValueProperty, value);
                UpdateVisible();
            }
        }

        private void UpdateVisible()
        {
            if (String.IsNullOrEmpty(Text) || Text != HiddenValue)
            {
                IsVisible = true;
                return;
            }

            IsVisible = false;
        }

        public RoundRectLabel()
        {
        }
    }
}


