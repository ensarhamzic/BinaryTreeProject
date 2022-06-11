using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    internal class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (bool)value;
            if (isVisible)
            {
                return "Cancel";
            }
            else
            {
                return "Add";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
