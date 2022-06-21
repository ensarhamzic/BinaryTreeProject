using System;
using System.Globalization;
using System.Windows.Data;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    internal class BoolToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // if adding is active, show Cancel, otherwise, show Add
            bool isActive = (bool)value;
            if (isActive)
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
