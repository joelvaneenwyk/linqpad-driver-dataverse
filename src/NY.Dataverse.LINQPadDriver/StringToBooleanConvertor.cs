using System;
using System.Globalization;
using System.Windows.Data;

namespace NY.Dataverse.LINQPadDriver
{
    public class StringToBooleanConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value?.Equals(parameter) ?? false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? parameter : Binding.DoNothing;
    }
}
