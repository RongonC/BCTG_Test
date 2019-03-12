using StemmonsMobile.ViewModels.EntityViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StemmonsMobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntityListCustomControl : ListView
    {
        public event EventHandler ListItemAppearing;

        public event EventHandler ListItemTapped;

        public EntityListCustomControl()
        {
            InitializeComponent();

            //listView.SetBinding(ListView.ItemsSourceProperty, new Binding(ListSourceProperty.PropertyName, source: this));
            //listView.ItemAppearing += (sender, e) =>
            //{
            //    MessagingCenter.Send(this, "Onappearing", e.Item as EntityListMBView);
            //};

        }

        public EntityListCustomControl(EntityListViewModel _entityVM = null)
        {
            InitializeComponent();
            listView.SetBinding(ListView.ItemsSourceProperty, new Binding(ListSourceProperty.PropertyName, source: this));
            if (_entityVM != null && BindingContext != null)
            {
                BindingContext = _entityVM;
            }
        }

        #region ListSourceProperty
        public static readonly BindableProperty ListSourceProperty = BindableProperty.Create("ListSource", typeof(EntityListViewModel), typeof(EntityListCustomControl), defaultBindingMode: BindingMode.TwoWay, propertyChanged: ListSourcePropertyChanged);

        public EntityListViewModel ListSource
        {
            get
            {
                return (EntityListViewModel)GetValue(ListSourceProperty);
            }
            set
            {
                SetValue(ListSourceProperty, value);
            }
        }

        private static void ListSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (EntityListCustomControl)bindable;
            EntityListViewModel pVM = (EntityListViewModel)newValue;
        }
        //public ObservableCollection<EntityListViewModel> ListSource
        //{
        //    get
        //    {
        //        return (ObservableCollection<EntityListViewModel>)GetValue(ListSourceProperty);
        //    }
        //    set
        //    {
        //        SetValue(ListSourceProperty, value);
        //    }
        //}

        //private static void ListSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (EntityList_ListControl)bindable;
        //    EntityListViewModel pVM = (EntityListViewModel)newValue;
        //}
        #endregion

        //#region ListSourceProperty
        //public static readonly BindableProperty ListSourceProperty = BindableProperty.Create("ListSource", typeof(ObservableCollection<EntityListMBView>), typeof(EntityList_ListControl), defaultBindingMode: BindingMode.TwoWay);

        //public ObservableCollection<EntityListMBView> ListSource
        //{
        //    get
        //    {
        //        return (ObservableCollection<EntityListMBView>)GetValue(ListSourceProperty);
        //    }
        //    set
        //    {
        //        SetValue(ListSourceProperty, value);
        //    }
        //}
        //#endregion

        //#region IsRefreshingProperty

        //public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create<EntityList_ListControl, bool>(p => p.IsRefreshing, false);


        //public bool IsRefreshing
        //{
        //    get => (bool)GetValue(IsRefreshingProperty);
        //    set { SetValue(IsRefreshingProperty, value); }
        //}
        //#endregion



        //#region RefreshCommandProperty
        //public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create<EntityList_ListControl, Command>(p => p.RefreshCommand, null);

        //public Command RefreshCommand
        //{
        //    get => (Command)GetValue(RefreshCommandProperty);
        //    set { SetValue(RefreshCommandProperty, value); }
        //} 
        //#endregion


        //protected override void OnPropertyChanged(string propertyName = null)
        //{
        //    base.OnPropertyChanged(propertyName);

        //    if (propertyName == ListSourceProperty.PropertyName)
        //    {
        //        listView.ItemsSource = ListSource;
        //    }
        //}

    }



    public class EventToCommandBehavior : BindableBehavior<View>
    {
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create<EventToCommandBehavior, string>(p => p.EventName, null);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<EventToCommandBehavior, ICommand>(p => p.Command, null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create<EventToCommandBehavior, object>(p => p.CommandParameter, null);
        public static readonly BindableProperty EventArgsConverterProperty = BindableProperty.Create<EventToCommandBehavior, IValueConverter>(p => p.EventArgsConverter, null);
        public static readonly BindableProperty EventArgsConverterParameterProperty = BindableProperty.Create<EventToCommandBehavior, object>(p => p.EventArgsConverterParameter, null);

        private Delegate _handler;
        private EventInfo _eventInfo;

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }

        public object EventArgsConverterParameter
        {
            get { return GetValue(EventArgsConverterParameterProperty); }
            set { SetValue(EventArgsConverterParameterProperty, value); }
        }

        protected override void OnAttachedTo(View visualElement)
        {
            base.OnAttachedTo(visualElement);

            var events = AssociatedObject.GetType().GetRuntimeEvents().ToArray();
            if (events.Any())
            {
                _eventInfo = events.FirstOrDefault(e => e.Name == EventName);
                if (_eventInfo == null)
                    throw new ArgumentException(String.Format("EventToCommand: Can't find any event named '{0}' on attached type", EventName));

                AddEventHandler(_eventInfo, AssociatedObject, OnFired);
            }
        }

        protected override void OnDetachingFrom(View view)
        {
            if (_handler != null)
                _eventInfo.RemoveEventHandler(AssociatedObject, _handler);

            base.OnDetachingFrom(view);
        }

        private void AddEventHandler(EventInfo eventInfo, object item, Action<object, EventArgs> action)
        {
            var eventParameters = eventInfo.EventHandlerType
                .GetRuntimeMethods().First(m => m.Name == "Invoke")
                .GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType))
                .ToArray();

            var actionInvoke = action.GetType()
                .GetRuntimeMethods().First(m => m.Name == "Invoke");

            _handler = Expression.Lambda(
                eventInfo.EventHandlerType,
                Expression.Call(Expression.Constant(action), actionInvoke, eventParameters[0], eventParameters[1]),
                eventParameters
            )
            .Compile();

            eventInfo.AddEventHandler(item, _handler);
        }

        private void OnFired(object sender, EventArgs eventArgs)
        {
            if (Command == null)
                return;

            var parameter = CommandParameter;

            if (eventArgs != null && eventArgs != EventArgs.Empty)
            {
                parameter = eventArgs;

                if (EventArgsConverter != null)
                {
                    parameter = EventArgsConverter.Convert(eventArgs, typeof(object), EventArgsConverterParameter, CultureInfo.CurrentUICulture);
                }
            }

            if (Command.CanExecute(parameter))
            {
                Command.Execute(parameter);
            }
        }
    }

    public class BindableBehavior<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);

            AssociatedObject = visualElement;

            if (visualElement.BindingContext != null)
                BindingContext = visualElement.BindingContext;

            visualElement.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= OnBindingContextChanged;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}