using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace EtsySpy.Utils
{
    public class ArrayToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<string> passedValue = (List<string>)value;

            StringBuilder sb = new StringBuilder();
            foreach (string s in passedValue)
            {
                sb.Append(s + ",");
            }

            return sb.ToString().TrimEnd(",".ToCharArray());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}
