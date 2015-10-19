namespace DatMailReader.Helpers.Providers
{
    using DatMailReader.Helpers.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.AccessCache;

    public class RecentFilesProvider
    {
        private const string MailDataFileName = "MailData";
        private List<string> recentlyExtractedMailTokens = new List<string>();
        private List<StorageFile> recentlyExtractedStorageFiles = new List<StorageFile>();

        private readonly static Lazy<RecentFilesProvider> instance = new Lazy<RecentFilesProvider>(() => new RecentFilesProvider(), true);
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

        public async void AddDatFileToken(StorageFile file)
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

        private async Task<List<StorageFile>> GetFilesByToken()
        {
            this.recentlyExtractedStorageFiles.Clear();
            foreach (String s in recentlyExtractedMailTokens)
            {
                var file = await GetSingleStorageFile(s);
                this.recentlyExtractedStorageFiles.Add(file);
            }

            return recentlyExtractedStorageFiles;
        }

        private async Task<StorageFile> GetSingleStorageFileByToken(string id)
        {
            var file = await GetSingleStorageFile(id);
            this.recentlyExtractedStorageFiles.Add(file);

            return file;
        }

        private async Task<StorageFile> GetSingleStorageFile(string token)
        {
            StorageFile result = null;
            try
            {
                if (StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
                {
                    result = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("Can't open file", ex);
            }

            return result;
        }

        private string GetUniqIdForFile(StorageFile file)
        {
            char[] temp = file.FolderRelativeId.ToCharArray();
            int index = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] == '\\')
                {
                    index = i;
                }
            }
            var result = file.FolderRelativeId.Remove(index);
            return result;
        }
    }
}
