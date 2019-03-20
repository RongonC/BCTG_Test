using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StemmonsMobile.Behavior
{
    public class InfiniteScroll : Behavior<ListView>
    {
        public static readonly BindableProperty LoadMoreCommandProperty = BindableProperty.Create("LoadMoreCommand", typeof(ICommand), typeof(InfiniteScroll), null);
        public ICommand LoadMoreCommand
        {
            get
            {
                return (ICommand)GetValue(LoadMoreCommandProperty);
            }
            set
            {
                SetValue(LoadMoreCommandProperty, value);
            }
        }

        public static readonly BindableProperty ItemTapCommandProperty = BindableProperty.Create("ItemTapCommand", typeof(ICommand), typeof(InfiniteScroll), null);
        public ICommand ItemTapCommand
        {
            get
            {
                return (ICommand)GetValue(ItemTapCommandProperty);
            }
            set
            {
                SetValue(ItemTapCommandProperty, value);
            }
        }

        public ListView AssociatedObject
        {
            get;
            private set;
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            AssociatedObject = bindable;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            bindable.ItemAppearing += InfiniteListView_ItemAppearing;
            //bindable.ItemTapped += InfiniteListView_ItemTapping;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.ItemAppearing -= InfiniteListView_ItemAppearing;
            //bindable.ItemTapped += InfiniteListView_ItemTapping;
        }

        void InfiniteListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var items = AssociatedObject.ItemsSource as IList;
            if (items != null && e.Item == items[items.Count - 1])
            {
                if (LoadMoreCommand != null && LoadMoreCommand.CanExecute(null))
                    LoadMoreCommand.Execute(e);
            }
        }

        void InfiniteListView_ItemTapping(object sender, ItemTappedEventArgs e)
        {
            //Item Tapped Command Event
            ItemTapCommand.CanExecute(e.Item);
        }
    }
}