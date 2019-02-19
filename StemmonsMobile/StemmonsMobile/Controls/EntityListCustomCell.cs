using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StemmonsMobile.Controls
{
    public class EntityListCustomCell : ViewCell
    {
        public EntityListCustomCell()
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
