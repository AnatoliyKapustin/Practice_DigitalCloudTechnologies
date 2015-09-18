using DatMailReader.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace DatMailReader.ViewModel
{
    public class RecentFilesProvider
    {
        ObservableCollection<string> RecentlyExtractedMailPath = new ObservableCollection<string>();
        ObservableCollection<StorageFile> RecentlyExtractedStorageFiles = new ObservableCollection<StorageFile>();

        public async Task<ObservableCollection<StorageFile>> Get()
        {
            this.RecentlyExtractedMailPath = await ObjectSerializer.Deserialize(RecentlyExtractedMailPath, "MailData");
            this.GetFilesFromPath(RecentlyExtractedMailPath);
            return this.RecentlyExtractedStorageFiles;
        }

        public void Add(string file)
        {
            this.RecentlyExtractedMailPath.Add(file);
            this.DeleteItems(RecentlyExtractedMailPath);
            ObjectSerializer.Serialize(this.RecentlyExtractedMailPath, "MailData");
        }

        private ObservableCollection<string> DeleteItems(ObservableCollection<string> path)
        {
            ObservableCollection<string> temp = path;
            if (temp.Count > 4)
            {
                for (int i = temp.Count - 5; i >= 0; i--)
                {
                    temp.RemoveAt(i);
                }
            }

            temp = new ObservableCollection<string>(temp.Distinct());
            return temp;
        }

        private async void GetFilesFromPath(ObservableCollection<string> path)
        {
            this.RecentlyExtractedStorageFiles.Clear();
            foreach(String s in path)
            {
                try
                {
                    this.RecentlyExtractedStorageFiles.Add(await StorageApplicationPermissions.FutureAccessList.GetFileAsync(s));
                }
                catch 
                { 
                    new Exception("Ошибка инициализации списка последних открытых файлов"); 
                }
            }
        }
    }
}
