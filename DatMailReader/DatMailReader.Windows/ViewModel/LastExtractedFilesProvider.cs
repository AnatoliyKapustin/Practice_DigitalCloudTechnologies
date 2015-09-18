using DatMailReader.Common;
using DatMailReader.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DatMailReader.ViewModel
{
    public class LastExtractedFilesProvider
    {
        ObservableCollection<FileInfo> TempCollection = new ObservableCollection<FileInfo>();
        ObservableCollection<FileInfo> RecentlyExtractedAttPath = new ObservableCollection<FileInfo>();
        private ObservableCollection<string> MailTitle = new ObservableCollection<string>();

        public async Task<ObservableCollection<FileInfo>> GetFiles()
        {
            this.TempCollection = await ObjectSerializer.Deserialize(this.TempCollection, "AttData");
            this.GetAttFilesFromPath(this.TempCollection);
            return this.RecentlyExtractedAttPath;         
        }

        public ObservableCollection<string> GetFileFromExtracted()
        {
            return this.MailTitle;
        }

        public void Add(ObservableCollection<FileInfo> AttFiles)
        {
            ObservableCollection<FileInfo> temp = new ObservableCollection<FileInfo>();
            foreach (FileInfo f in AttFiles)
            {
                if (!(f.storageFile.DisplayName.Contains("Message_Body")))
                {
                    temp.Add(f);
                }
            }

            if (temp.Count > 4)
            {
                for (int i = temp.Count - 5; i >= 0; i--)
                {
                    temp.RemoveAt(i);
                }
            }

            ObjectSerializer.Serialize(temp, "AttData");
        }

        private void GetAttFilesFromPath(ObservableCollection<FileInfo> path)
        {
            this.RecentlyExtractedAttPath.Clear();
            foreach (FileInfo s in path)
            {
                try
                {
                    this.RecentlyExtractedAttPath.Add(s);
                    this.MailTitle.Add(s.From);
                }
                catch 
                { 
                    throw new Exception("Ошибка инициализации списка последних извлеченных файлов"); 
                }
            }
        }

        private void AddAttPathToCollection(ObservableCollection<FileInfo> path)
        {
            
        }
    }
}
