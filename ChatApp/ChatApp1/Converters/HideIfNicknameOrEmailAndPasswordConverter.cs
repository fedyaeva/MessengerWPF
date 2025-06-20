using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ChatApp.Converters
{
    public class HideIfNicknameOrEmailAndPasswordConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 3)
                return Visibility.Visible;

            string nickname = values[0]?.ToString();
            string email = values[1]?.ToString();
            string password = values[2]?.ToString();

            bool hasNickname = !string.IsNullOrWhiteSpace(nickname);
            bool hasEmailAndPassword = !string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password);

            return (hasNickname || hasEmailAndPassword) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}