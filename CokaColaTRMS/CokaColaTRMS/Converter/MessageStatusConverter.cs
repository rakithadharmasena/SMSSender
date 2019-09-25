using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace CokaColaTRMS.Converter
{
   public class MessageStatusConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return "No";

                bool isSent = (bool)value;

                if (isSent)
                {
                    return "Yes";
                }
                else
                {
                    return "No";
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
