using Xamarin.Forms;

namespace StemmonsMobile.Commonfiles
{
    public class TextViewCell : ViewCell
    {
        public TextViewCell()
        {
            StackLayout layout = new StackLayout();
            layout.BackgroundColor = Color.Transparent;
            layout.Padding = new Thickness(15, 0, 15, 0);
            Label label = new Label();
            label.TextColor = Color.Black;

            label.SetBinding(Label.TextProperty, ".");
            layout.Children.Add(label);
            View = layout;
        }
    }
}
