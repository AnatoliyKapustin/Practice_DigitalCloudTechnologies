using System;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DatMailReader.ViewModel
{
    public class ThumbnailToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            BitmapImage image = null;
            var thumbnail = value as StorageItemThumbnail;
            if (thumbnail != null)
            {                
                image = new BitmapImage();
                image.SetSource(thumbnail);
            }

            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}