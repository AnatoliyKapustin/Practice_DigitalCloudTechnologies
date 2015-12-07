namespace DatMailReader.Helpers.TemplateSelectors
{
    using DatMailReader.Models.Enums;
    using DatMailReader.Models.Model;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class FileIconSelector : DataTemplateSelector
    {
        public DataTemplate Any { get; set; }
        public DataTemplate Picture { get; set; }
        public DataTemplate Text { get; set; }
        public DataTemplate Archive { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        { 
            var result = this.Any;
            var fileItem = item as FileInfo;
            if (fileItem != null)
            {
                switch (fileItem.Thumbnail)
                {
                    case ImageStyles.Archive:
                        result = this.Archive;
                        break;
                    case ImageStyles.Picture:
                        result = this.Picture;
                        break;
                    case ImageStyles.Text:
                        result = this.Text;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
