using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    public class IdToColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int? id = values[0] as int?;
            int? selectedId = values[1] as int?;
            Color clr;
            if (id == selectedId)
                clr = ColorConverter.ConvertFromString(values[3] as string) as Color? ?? Colors.Red;   
            else
                clr = ColorConverter.ConvertFromString(values[2] as string) as Color? ?? Colors.White;
            
            return new SolidColorBrush(clr);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
