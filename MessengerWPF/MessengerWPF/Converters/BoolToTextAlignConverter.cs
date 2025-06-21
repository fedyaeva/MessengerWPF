using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChatApp.Converters
{
    public class BoolToTextAlignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (bool)value ? TextAlignment.Right : TextAlignment.Left;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
