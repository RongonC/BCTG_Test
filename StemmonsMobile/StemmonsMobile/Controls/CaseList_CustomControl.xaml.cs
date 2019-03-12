using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CaseList_CustomControl : ListView
    {
        public static event EventHandler HeaderTapEvent;

        public CaseList_CustomControl()
        {
            try
            {
                InitializeComponent();

                #region GroupHeader Code Behind (alternative to XAML)
                //@this.GroupHeaderTemplate = new DataTemplate(() =>
                //{
                //    var grid = new Grid();
                //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(35) });
                //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5) });
                //    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                //    var stIconBtn = new Button
                //    {
                //        BackgroundColor = Color.Transparent,
                //        BorderColor = Color.Transparent,
                //        BorderWidth = 0
                //    };

                //    Binding headerTapCmdBinding = new Binding()
                //    {
                //        Source = @this.BindingContext
                //    };

                //    stIconBtn.SetBinding(Button.ImageProperty, "StateIcon");
                //    stIconBtn.SetBinding(Button.CommandProperty, new Binding("HeaderTapCommand") { Source = @this.BindingContext});
                //    stIconBtn.SetBinding(Button.CommandParameterProperty, headerTapCmdBinding);

                //    var titleBtn = new Button
                //    {
                //        BackgroundColor = Color.Transparent,
                //        BorderColor = Color.Transparent,
                //        BorderWidth = 0,
                //        HorizontalOptions = LayoutOptions.StartAndExpand,
                //        VerticalOptions = LayoutOptions.Center,
                //        FontSize = 14
                //    };

                //    titleBtn.SetBinding(Button.TextProperty, new Binding("TitleWithItemCount"));

                //    //var ageLabel = new Label();
                //    //var locationLabel = new Label { HorizontalTextAlignment = TextAlignment.End };

                //    //nameLabel.SetBinding(Label.TextProperty, "Name");
                //    //ageLabel.SetBinding(Label.TextProperty, "Age");
                //    //locationLabel.SetBinding(Label.TextProperty, "Location");

                //    grid.Children.Add(stIconBtn, 0, 0);
                //    grid.Children.Add(titleBtn, 2, 0);

                //    return new ViewCell { View = grid };
                //}); 
                #endregion
            }
            catch (Exception ex)
            {

            }

            //@this.SetBinding(ListView.ItemsSourceProperty, new Binding(ListSourceProperty.PropertyName, source: this));
        }


        //public static readonly BindableProperty ListSourceProperty = BindableProperty.Create("ListSource", typeof(ViewModel_1), typeof(CaseList_CustomControl), defaultBindingMode: BindingMode.TwoWay, propertyChanged: ListSourcePropertyChanged);

        //public ViewModel_1 ListSource
        //{
        //    get
        //    {
        //        return (ViewModel_1)GetValue(ListSourceProperty);
        //    }
        //    set
        //    {
        //        SetValue(ListSourceProperty, value);
        //    }
        //}

        //private static void ListSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (CaseList_CustomControl)bindable;
        //    ViewModel_1 pVM = (ViewModel_1)newValue;
        //}

        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);

        //    if (propertyName == ListSourceProperty.PropertyName)
        //    {
        //        @this.ItemsSource = ListSource;
        //    }
        //}
    }
}