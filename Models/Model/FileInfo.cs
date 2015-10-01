namespace DatMailReader.Models.Model
{
    using System;
    using System.Runtime.Serialization;
    using Windows.Storage;
    using Windows.Storage.FileProperties;

    [DataContract]
    public class FileInfo
    {
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

        [IgnoreDataMember]
        public StorageItemThumbnail Thumbnail { get; set; }

        [IgnoreDataMember]
        public DateTimeOffset CreationDate { get; set; }

        [IgnoreDataMember]
        public string UniqId { get; set; }

        [DataMember]
        public string From { get; set; }

        [IgnoreDataMember]
        public string Subject { get; set; }

        [IgnoreDataMember]
        public string SourceFileName { get; set; }

        public FileInfo(StorageFile file, StorageItemThumbnail t, DateTimeOffset date, string size, string from, string subject, string fileName)
        {
            this.ExtractedStorageFile = file;
            this.DisplayName = file.DisplayName;
            this.DisplayType = file.DisplayType;
            this.CreationDate = date;
            this.UniqId = file.FolderRelativeId;
            this.Thumbnail = t;
            this.Size = size;
            this.From = from;
            this.SourceFileName = fileName;
            this.FilePath = file.Path;
            this.Subject = subject;
        }
    }
}
