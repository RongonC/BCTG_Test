using System;
using Xamarin.Forms;

namespace StemmonsMobile.Controls
{
    public class NullableDatePicker : DatePicker
    {
        public NullableDatePicker()
        {
            Format = "MM/dd/yyyy";
           // NullableDate = null;
        }
        public string _originalFormat = null;

        public static readonly BindableProperty PlaceHolderProperty =
            BindableProperty.Create(nameof(PlaceHolder), typeof(string), typeof(NullableDatePicker), "Select Date");

        public string PlaceHolder
        {
            get { return (string)GetValue(PlaceHolderProperty); }
            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }


        public static readonly BindableProperty NullableDateProperty =
        BindableProperty.Create(nameof(NullableDate), typeof(DateTime?), typeof(NullableDatePicker), null, defaultBindingMode: BindingMode.TwoWay);

        public DateTime? NullableDate
        {
            get { return (DateTime?)GetValue(NullableDateProperty); }
            set { SetValue(NullableDateProperty, value); UpdateDate(); }
        }

        private void UpdateDate()
        {
            if (NullableDate != null)
            {
                if (_originalFormat != null)
                {
                    Format = _originalFormat;
                }
            }
            else
            {
                Format = PlaceHolder;

            }

        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext != null)
            {
                _originalFormat = Format;
                UpdateDate();
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == DateProperty.PropertyName || (propertyName == IsFocusedProperty.PropertyName && !IsFocused && (Date.ToString("MM/dd/yyyy") == DateTime.Now.ToString("MM/dd/yyyy"))))
            {
                AssignValue();
            }

            if (propertyName == NullableDateProperty.PropertyName && NullableDate.HasValue)
            {
                Date = NullableDate.Value;
                if (Date.ToString(_originalFormat) == DateTime.Now.ToString(_originalFormat))
                {
                    //this code was done because when date selected is the actual date the"DateProperty" does not raise  
                    UpdateDate();
                }
            }
        }

        public void CleanDate()
        {
            NullableDate = null;
            UpdateDate();
        }

        //public void CleanDate(DateTime? pNullableDate)
        //{
        //    NullableDate = pNullableDate;
        //    UpdateDate();
        //}
        public void AssignValue()
        {
           // var dateAndTime = Date.Date;
           // int year = Date.Year;
           // int month = Date.Month;
           // int day = Date.Day;

           //string string.Format("{0}/{1}/{2}", month, day, year);


            NullableDate = Date;
            UpdateDate();

        }
    }
}