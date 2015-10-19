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
        private const string AttachmentFileName = "AttData";
        private List<FileInfo> recentlyExtractedAttachment = new List<FileInfo>();

        private readonly static Lazy<LastExtractedFilesProvider> instance = new Lazy<LastExtractedFilesProvider>(() => new LastExtractedFilesProvider(), true);
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

        public void AddAttachmentToRecentFiles(List<FileInfo> AttFiles)
        {
            foreach (FileInfo addedFile in AttFiles)
            {
                var result = CheckEqualsFiles(addedFile);
                if (!addedFile.ExtractedStorageFile.DisplayName.Contains("Message_Body") && result == false)
                {
                    this.recentlyExtractedAttachment.Add(addedFile);
                }
            }

            ObjectSerializer.Serialize(this.recentlyExtractedAttachment, AttachmentFileName);
        }

        private bool CheckEqualsFiles(FileInfo addedFile)
        {
            var result = false;
            foreach (FileInfo recentFile in this.recentlyExtractedAttachment)
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
