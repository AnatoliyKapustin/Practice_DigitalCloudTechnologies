namespace DatMailReader.Helpers.Providers
{
    using DatMailReader.Models.Model;
    using Helpers.Common;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

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

        public async Task<List<FileInfo>> GetAttFiles()
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
    }
}
