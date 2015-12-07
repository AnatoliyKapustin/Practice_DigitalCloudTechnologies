namespace DatMailReader.Models.Model
{
    using DatMailReader.Models.Enums;
    using System;
    using System.Runtime.Serialization;
    using Windows.Storage;
    using Windows.Storage.FileProperties;
    using Windows.UI.Xaml.Media.Imaging;

    [DataContract]
    public class FileInfo
    {
        public FileInfo(StorageFile file, ImageStyles thumb, string size, string fileName)
        {
            this.ExtractedStorageFile = file;
            this.DisplayName = file.DisplayName;
            this.DisplayType = file.DisplayType;
            this.Thumbnail = thumb;
            this.Size = size;
            this.SourceFileName = fileName;
            this.FilePath = file.Path;
        }

        [IgnoreDataMember]
        public StorageFile ExtractedStorageFile { get; set; }

        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public string DisplayName { get; set; }

        [IgnoreDataMember]
        public string DisplayType { get; set; }

        [IgnoreDataMember]
        public string Size { get; set; }

        [DataMember]
        public ImageStyles Thumbnail { get; set; }

        [DataMember]
        public string SourceFileName { get; set; }
    }
}
