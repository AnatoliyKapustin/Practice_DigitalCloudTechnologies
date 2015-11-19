using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace DatMailReader.Helpers.Converters
{
    public class NameToStyleConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var name = value as string;

            return Application.Current.Resources[name] as Style;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
