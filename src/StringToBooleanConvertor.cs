using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Mycoshiro.Dataverse.LINQPad
{
    public class StringToBooleanConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value?.Equals(parameter) ?? false;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            (bool)value ? parameter : Binding.DoNothing;
    }
}
