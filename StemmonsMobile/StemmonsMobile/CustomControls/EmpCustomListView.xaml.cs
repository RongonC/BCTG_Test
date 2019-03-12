using StemmonsMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmpCustomListView : ListView
    {
        public event EventHandler ListItemAppearing;

        public EmpCustomListView()
        {
            InitializeComponent();
        }

        public EmpCustomListView(EmpInfoViewModel ERVM = null)
        {
            InitializeComponent();
            listview.SetBinding(ListView.ItemsSourceProperty, new Binding(ListSourceProperty.PropertyName, source: this));
            if (ERVM != null && BindingContext != null)
            {
                BindingContext = ERVM;
            }
        }


        public EmpInfoViewModel ListSource
        {
            get
            {
                return (EmpInfoViewModel)GetValue(ListSourceProperty);
            }
            set
            {
                SetValue(ListSourceProperty, value);
            }
        }

        public static readonly BindableProperty ListSourceProperty = BindableProperty.Create(
                                                                    "ListSource",
                                                                    typeof(EmpInfoViewModel),
                                                                    typeof(EmpCustomListView),
                                                                    defaultBindingMode: BindingMode.TwoWay,
                                                                    propertyChanged: ListSourcePropertyChanged);

        private static void ListSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EmpCustomListView)bindable;
            EmpInfoViewModel ERVM = (EmpInfoViewModel)newValue;
        }
    }
}
