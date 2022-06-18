using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    public class CharToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            char character = (char)(value as char?);
            if(Char.IsWhiteSpace(character))
                return "space";
            else
                return $"'{character}'";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
