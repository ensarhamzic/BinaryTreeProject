using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    public class ValueToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if number is long, shows the .. after first 3 digits
            int nodeValue = (int)value;
            string valueString;
            if (nodeValue < 1000 && nodeValue > -1000)
                valueString = nodeValue.ToString();
            else
                valueString = $"{nodeValue.ToString().Substring(0, 3)}..";
            return valueString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
