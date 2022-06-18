using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    public class NodeToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            char? character = values[0] as char?;
            Color clr;
            if (character != null)
                clr = (Color)(ColorConverter.ConvertFromString("#BCAF9F") as Color?);
            else
                clr = (Color)(ColorConverter.ConvertFromString("#fff4ed") as Color?);

            return new SolidColorBrush(clr);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
