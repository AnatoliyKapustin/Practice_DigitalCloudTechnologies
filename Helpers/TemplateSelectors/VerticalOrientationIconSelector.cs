namespace DatMailReader.Helpers.TemplateSelectors
{
    using DatMailReader.Models.Enums;
    using DatMailReader.Models.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public class VerticalOrientationIconSelector : DataTemplateSelector
    {
        public DataTemplate AnyVertical { get; set; }
        public DataTemplate PictureVertical { get; set; }
        public DataTemplate TextVertical { get; set; }
        public DataTemplate ArchiveVertical { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var result = this.AnyVertical;
            var fileItem = item as FileInfo;
            if (fileItem != null)
            {
                switch (fileItem.Thumbnail)
                {
                    case ImageStyles.Archive:
                        result = this.ArchiveVertical;
                        break;
                    case ImageStyles.Picture:
                        result = this.PictureVertical;
                        break;
                    case ImageStyles.Text:
                        result = this.TextVertical;
                        break;
                    default:
                        break;
                }
            }

            return result;
        }
    }
}
