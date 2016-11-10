using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace SchoolsMailing.Converters
{
    public class IntegerToInitial : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int initialInt = (int)value;
            string initialString;
            switch (initialInt)
            {
                case 1:
                    initialString = "J";
                    break;
                case 2:
                    initialString = "F";
                    break;
                default:
                    initialString = "c";
                    break;
            }

            // Return the month value to pass to the target.
            return initialString;
        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
