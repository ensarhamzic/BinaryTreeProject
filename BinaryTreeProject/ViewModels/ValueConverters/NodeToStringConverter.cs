using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    public class NodeToStringConverter : IMultiValueConverter
    {
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            char? character = values[0] as char?;
            int? freq = values[1] as int?;
            string valueString;
            if (character != null)
                valueString = $"'{character}'({freq})";
            else
                valueString = $"{freq}";

            return valueString;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
