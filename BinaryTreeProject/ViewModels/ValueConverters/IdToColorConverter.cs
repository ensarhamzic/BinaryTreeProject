using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace BinaryTreeProject.ViewModels.ValueConverters
{
    internal class IdToColorConverter : IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int? id = values[0] as int?;
            int? selectedId = values[1] as int?;
            if (id == selectedId)
            {
                return Brushes.Aqua;
            }
            else
            {
                return Brushes.White;
            }
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
