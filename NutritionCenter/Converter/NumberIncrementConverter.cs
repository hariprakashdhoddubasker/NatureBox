using System;
using System.Globalization;
using System.Windows.Data;

namespace NatureBox.Converter
{
    public class NumberIncrementConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int number = (int)value;
         
            return ++number;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
