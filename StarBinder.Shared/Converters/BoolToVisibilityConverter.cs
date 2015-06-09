using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace StarBinder.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Not { get; set; }
        
        private object VisibilityToBool(object value)
        {
            if (!(value is Visibility)) return DependencyProperty.UnsetValue;
            return (((Visibility)value) == Visibility.Visible) ^ Not;
        }

        private object BoolToVisibility(object value)
        {
            if (!(value is bool)) return DependencyProperty.UnsetValue;
            return ((bool)value ^ Not) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return BoolToVisibility(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return VisibilityToBool(value);
        }
    }
}
