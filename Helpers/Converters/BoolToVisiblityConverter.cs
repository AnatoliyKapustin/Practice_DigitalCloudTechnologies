namespace DatMailReader.Helpers.Converters
{
    using DatMailReader.Models.Model;
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var result = false;
            var message = value as Message;
            if(message != null && message.Files.Count != 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
