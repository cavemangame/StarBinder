using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StarBinder.LevelEditor.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public bool Not { get; set; }

        public bool Hidden { get; set; }

        private object VisibilityToBool(object value)
        {
            if (!(value is Visibility))
                return DependencyProperty.UnsetValue;
            return (((Visibility)value) == Visibility.Visible) ^ Not;
        }

        private object BoolToVisibility(object value)
        {
            if (!(value is bool))
                return DependencyProperty.UnsetValue;
            return ((bool)value ^ Not) ? Visibility.Visible : (Hidden ? Visibility.Hidden : Visibility.Collapsed);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BoolToVisibility(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return VisibilityToBool(value);
        }
    }
}
