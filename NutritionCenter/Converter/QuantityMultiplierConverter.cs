using NatureBox.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace NatureBox.Converter
{
    public class QuantityMultiplierConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invoice = (Invoice)value;
            switch (parameter)
            {
                case "Cost":
                    return invoice.Product.Cost * invoice.Quantity;
                case "MRP":
                    return invoice.Product.MRP * invoice.Quantity;
                case "Expense":
                    return invoice.Product.Expense * invoice.Quantity;
                case "VolumePoint":
                    return invoice.Product.VolumePoint * invoice.Quantity;
                default:
                    break;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
