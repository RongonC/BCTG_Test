using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StemmonsMobile.Controls
{
    public class CustomCell : ViewCell
    {
        public CustomCell()
        {
            //instantiate each of our views.
            var image = new Image();
            StackLayout horizontalLayout = new StackLayout();
            Label right = new Label();

            //set bindings
            right.SetBinding(Label.TextProperty, "CaseTypeName");
            image.Source = ImageSource.FromFile("Assets/dropdowniconClose.png");
            //Set properties for desired design
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            right.HorizontalOptions = LayoutOptions.StartAndExpand;

            //add views to the view hierarchy
            horizontalLayout.Children.Add(right);
            horizontalLayout.Children.Add(image);
            View = horizontalLayout;
        }
    }
}
