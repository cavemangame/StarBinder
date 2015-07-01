using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace XF.Core.Controls
{
    public class ItemsControl : ContentView
    {
        public ItemsControl()
        {
            Content = panel = new StackLayout() { Spacing = 10 };
        }

        private Layout<View> panel;
        public Layout<View> Panel
        {
            get { return panel; }
            set
            {
                if (value == panel) return;

                foreach (var child in panel.Children)
                {
                    value.Children.Add(child);
                }
                Content = panel = value;
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
            var ctrl = (ItemsControl)bindable;
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

            Panel.Children.Clear();

            foreach (var item in newValue)
            {
                Panel.Children.Add(CreateControl(item));
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null && args.OldItems != null && args.NewItems.Count != args.OldItems.Count)
                throw new ArgumentException("args.NewItems.Count != args.OldItems.Count");
            
            if (args.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < args.NewItems.Count; i++)
                {
                    Panel.Children.Insert(args.NewStartingIndex + i, CreateControl(args.NewItems[i]));
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Move)
            {
                if (sender.GetType().GetGenericTypeDefinition() == typeof(ObservableCollection<>) && args.NewStartingIndex < args.OldStartingIndex)
                    throw new NotImplementedException("In .net ObservableCollection.Move(graterIndex, leserIndex) has unexpetable behaviour, not tested on xamarin");
                
                for (int i = 0; i < args.OldItems.Count; i++)
                {
                    var index = args.NewStartingIndex < args.OldStartingIndex ? args.OldStartingIndex + 1 : args.OldStartingIndex;
                    var moved = Panel.Children[index];
                    Panel.Children.RemoveAt(index);
                    Panel.Children.Insert(args.NewStartingIndex + i, moved);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < args.OldItems.Count; i++)
                {
                    Panel.Children.RemoveAt(args.OldStartingIndex);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Replace)
            {
                for (int i = 0; i < args.NewItems.Count; i++)
                {
                    Panel.Children[args.NewStartingIndex + i] = CreateControl(args.NewItems[i]);
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                Panel.Children.Clear();
            }
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
