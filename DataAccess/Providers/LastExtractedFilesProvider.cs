namespace DatMailReader.Helpers.Providers
{
    using DatMailReader.DataAccess.Common;
    using DatMailReader.Models.Model;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;

    public class LastExtractedFilesProvider
    {
        private readonly static Lazy<LastExtractedFilesProvider> instance = new Lazy<LastExtractedFilesProvider>(() => new LastExtractedFilesProvider(), true);

        private const string AttachmentFileName = "AttData";

        private List<FileInfo> recentlyExtractedAttachment = new List<FileInfo>();
                
        public static LastExtractedFilesProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public async Task<List<FileInfo>> GetAttachmentFiles()
        {
            var result = new List<FileInfo>(this.recentlyExtractedAttachment);
            if (this.recentlyExtractedAttachment.Count == 0)
            {
                var files = await ObjectSerializer.Deserialize<List<FileInfo>>(AttachmentFileName);
                if (files != null)
                {
                    this.recentlyExtractedAttachment = new List<FileInfo>(files);
                    result = new List<FileInfo>(files);
                }
            }

            return result;
        }

        public void ClearLastAttachmentsCollection()
        {
            this.recentlyExtractedAttachment.Clear();
            ObjectSerializer.Serialize(this.recentlyExtractedAttachment, AttachmentFileName);
        }

        public void AddAttachmentToRecentFiles(List<FileInfo> attachments)
        {
            var isChanged = false;
            foreach (var attachment in attachments)
            {
                var result = this.CheckEqualsFiles(attachment);
                if (!attachment.ExtractedStorageFile.DisplayName.Contains("Message_Body") && !result)
                {
                    isChanged = true;
                    this.recentlyExtractedAttachment.Add(attachment);
                }
            }

            if (isChanged)
            {
                ObjectSerializer.Serialize(this.recentlyExtractedAttachment, AttachmentFileName);
            }
        }

        private bool CheckEqualsFiles(FileInfo addedFile)
        {
            var result = false;
            foreach (var recentFile in this.recentlyExtractedAttachment)
            {
                if(recentFile.FilePath.Equals(addedFile.FilePath) && recentFile.SourceFileName.Equals(addedFile.SourceFileName))
                {
                    result = true;
                }
            }

            return result;
        }

        public static async Task<StorageFile> WriteFileToIsoStorage(string fileName, Stream content)
        {
            var file = await CreateFileInLocalStorage(fileName);
            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                await content.CopyToAsync(stream.AsStream());
            }

            //using (var memoryStream = new MemoryStream())
            //{
            //    content.CopyTo(memoryStream);
            //    await FileIO.WriteBytesAsync(file, memoryStream.ToArray());
            //}

            return file;
        }

        public static async Task<StorageFile> WriteFileToIsoStorage(string fileName, string content)
        {
            var file = await CreateFileInLocalStorage(fileName);
            await FileIO.WriteTextAsync(file, content);

            return file;
        }

        private static async Task<StorageFile> CreateFileInLocalStorage(string fileName)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
            var localFile = await localFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            return localFile;
        } 
    }
}
