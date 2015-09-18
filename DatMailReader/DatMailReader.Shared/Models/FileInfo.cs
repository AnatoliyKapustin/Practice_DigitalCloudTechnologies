using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace DatMailReader.Models
{
    [DataContract]
    public class FileInfo
    {
        [IgnoreDataMember]
        public StorageFile storageFile { get; set; }

        [DataMember]
        public string filePath { get; set; }
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
            this.storageFile = file;
            this.DisplayName = file.DisplayName;
            this.DisplayType = file.DisplayType;
            this.CreationDate = date;
            this.UniqId = file.FolderRelativeId;
            this.Thumbnail = t;
            this.Size = size;
            this.From = from;
            this.Subject = subject;
            this.SourceFileName = fileName;
            this.filePath = file.Path;
        }
    }
}
