using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace XF.Core.Controls
{
    public class ItemsControl : ContentView
    {
        public ItemsControl()
        {
            Content = new StackLayout() { Spacing = 10 };
        }

        public Layout<View> Layout
        {
            get { return (Layout<View>) Content; }
            set
            {
                if (value == Content) return;
                
                foreach (var child in Layout.Children)
                {
                    value.Children.Add(child);
                }
                
                Content = value;
                ForceLayout();
            }
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<ItemsControl, IEnumerable<object>>(p => p.ItemsSource, new List<object>(), BindingMode.OneWay, null, ItemsSourceChanged);

        public IEnumerable<object> ItemsSource
        {
            get { return (IEnumerable<object>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, IEnumerable<object> oldValue, IEnumerable<object> newValue)
        {
            var ctrl = (ItemsControl) bindable;
            ctrl.OnItemsSourceChanged(oldValue, newValue);
        }

        private void OnItemsSourceChanged(IEnumerable<object> oldValue, IEnumerable<object> newValue)
        {
            var observable = oldValue as INotifyCollectionChanged;
            if (observable != null)
            {
                observable.CollectionChanged -= OnItemsCollectionChanged;
            }
            
            observable = newValue as INotifyCollectionChanged;
            if (observable != null)
            {
                observable.CollectionChanged += OnItemsCollectionChanged;
            }

            Layout.Children.Clear();

            foreach (var item in newValue)
            {
                Layout.Children.Add(CreateControl(item));
            }

            ForceLayout();
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < args.NewItems.Count; i++)
                {
                    Layout.Children.Insert(args.NewStartingIndex + i, CreateControl(args.NewItems[i]));
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Move)
            {
                throw new NotImplementedException("Move not implemented");
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < args.OldItems.Count; i++)
                {
                    Layout.Children.RemoveAt(args.OldStartingIndex + i);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Replace)
            {
                throw new NotImplementedException("Replace not implemented");
            }
            else if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                throw new NotImplementedException("Reset not implemented");
            }
            
            ForceLayout();
        }

        private View CreateControl(object viewModel)
        {
            var content = (View)ItemTemplate.CreateContent();
            content.BindingContext = viewModel;
            return content;
        }


        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<ItemsControl, DataTemplate>(p => p.ItemTemplate, 
            new DataTemplate(() =>
            {
                var label = new Label();
                label.SetBinding(Label.TextProperty, ".");
                return label;
            }), BindingMode.OneWay, null, ItemTemplateChanged);

        private static void ItemTemplateChanged(BindableObject bindable, DataTemplate oldValue, DataTemplate newValue)
        {
            ((ItemsControl)bindable).ForceLayout();
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

    }
}
