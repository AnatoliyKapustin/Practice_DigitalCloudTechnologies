namespace DatMailReader.Helpers.Providers
{
    using DatMailReader.DataAccess.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.AccessCache;

    public class RecentFilesProvider
    {
        private const string MailDataFileName = "MailData";
        private readonly static Lazy<RecentFilesProvider> instance = new Lazy<RecentFilesProvider>(() => new RecentFilesProvider(), true);
        private List<string> recentlyExtractedMailTokens = new List<string>();
        private List<StorageFile> recentlyExtractedStorageFiles = new List<StorageFile>();

        public static RecentFilesProvider Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public async Task<List<StorageFile>> GetRecentlyExtractedMails()
        {
            var result = new List<StorageFile>(this.recentlyExtractedStorageFiles);
            if (this.recentlyExtractedStorageFiles.Count == 0)
            {
                var tokens = await ObjectSerializer.Deserialize<List<string>>(MailDataFileName);
                if (tokens != null)
                {
                    this.recentlyExtractedMailTokens = new List<string>(tokens);
                    result = await this.GetFilesByToken();
                }
            }

            return result;
        }

        public async Task AddDatFileToken(StorageFile file)
        {
            var uniqId = this.GetUniqIdForFile(file);
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(uniqId, file);
            if (!this.recentlyExtractedMailTokens.Contains(uniqId))
            {
                this.recentlyExtractedMailTokens.Add(uniqId);
                await this.GetSingleStorageFileByToken(uniqId);
                ObjectSerializer.Serialize(this.recentlyExtractedMailTokens, MailDataFileName);
            }
        }

        public void ClearRecentDatFilesCollection()
        {
            StorageApplicationPermissions.FutureAccessList.Clear();
            this.recentlyExtractedMailTokens.Clear();
            this.recentlyExtractedStorageFiles.Clear();
            ObjectSerializer.Serialize(this.recentlyExtractedMailTokens, MailDataFileName);
        }

        private async Task<List<StorageFile>> GetFilesByToken()
        {
            this.recentlyExtractedStorageFiles.Clear();
            foreach (var token in recentlyExtractedMailTokens)
            {
                var file = await this.GetSingleStorageFile(token);
                if (file != null)
                {
                    this.recentlyExtractedStorageFiles.Add(file);
                }
            }

            return this.recentlyExtractedStorageFiles;
        }

        private async Task<StorageFile> GetSingleStorageFileByToken(string id)
        {
            var file = await GetSingleStorageFile(id);
            if (file != null)
            {
                this.recentlyExtractedStorageFiles.Add(file);
            }

            return file;
        }

        private async Task<StorageFile> GetSingleStorageFile(string token)
        {
            var result = default(StorageFile);
            try
            {
                if (StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
                {
                    result = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
                }
            }
            catch (FileNotFoundException) { }

            return result;
        }

        private string GetUniqIdForFile(StorageFile file)
        {
            var temp = file.FolderRelativeId.ToCharArray();
            var index = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == '\\')
                {
                    index = i;
                }
            }

            return file.FolderRelativeId.Remove(index);
        }
    }
}
