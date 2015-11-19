namespace DatMailReader.Helpers.Converters
{
    using DatMailReader.Models.Model;
    using System;
    using System.Collections;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class EmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var collection = value as ICollection;
            var result = false;
            if (value == null)
            {
                result = false;
            }
            else if (collection != null)
            {
                result = collection.Count != 0;
            }

            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
