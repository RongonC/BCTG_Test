using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
namespace StemmonsMobile.Models
{
    public class ChillerEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }

    public class ChillerEntityCell : ImageCell
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var food = (ChillerEntity)BindingContext;
        }
    }
}
