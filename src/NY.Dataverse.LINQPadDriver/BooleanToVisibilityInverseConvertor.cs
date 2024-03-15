using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NY.Dataverse.LINQPadDriver
{
    public class BooleanToVisibilityInverseConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? Visibility.Collapsed : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            !(value?.Equals(parameter)).GetValueOrDefault() ? parameter : Binding.DoNothing;
    }
}
