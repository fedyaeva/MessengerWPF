using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ChatApp.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isOwn = (bool)value;
            return isOwn ? new SolidColorBrush(Color.FromRgb(209, 255, 214)) : new SolidColorBrush(Color.FromRgb(232, 244, 255));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
