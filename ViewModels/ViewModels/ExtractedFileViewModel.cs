namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.DataAccess.Providers;
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.AccessCache;
    using Windows.Storage.Pickers;

    public class ExtractedFileViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private string rtfString;
        private string htmlMessage;
        private string recievedFileToken;
        private Message extractedMessage;
        private List<FileInfo> extractedFilesFromMessage = new List<FileInfo>();

        public ExtractedFileViewModel()
        {
            this.MyItemClickCommand = new RelayCommand<FileInfo>(this.OpenFileByDefaultProgram);
            this.OpenFileWithCommand = new RelayCommand<FileInfo>(this.OpenWith);
            this.OpenFlyoutFileWithCommand = new RelayCommand<FileInfo>(this.OpenWith);
            this.SaveAllCommand = new RelayCommand(() => this.SaveAllAttachments(ExtractedFilesFromMessage));
        }

        public RelayCommand<FileInfo> MyItemClickCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFileWithCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFlyoutFileWithCommand { get; private set; }
        public RelayCommand SaveAllCommand { get; private set; }

        public Message ExtractedMessage
        {
            get
            {
                return this.extractedMessage;
            }

            set
            {
                this.extractedMessage = value;
                base.RaisePropertyChanged("ExtractedMessage");
            }
        }

        public List<FileInfo> ExtractedFilesFromMessage
        {
            get
            {
                return this.extractedFilesFromMessage;
            }

            set
            {
                this.extractedFilesFromMessage = value;
                base.RaisePropertyChanged("ExtractedFilesFromMessage");
            }
        }

        public string RecievedFileToken
        {
            get
            {
                return this.recievedFileToken;
            }

            set
            {
                this.recievedFileToken = value;
                base.RaisePropertyChanged("RecievedFileToken");
            }
        }

        public string RtfString
        {
            get
            {
                return this.rtfString;
            }

            set
            {
                this.rtfString = value;
                base.RaisePropertyChanged("RtfString");
            }
        }

        public string HTMLMessage
        {
            get
            {
                return this.htmlMessage;
            }

            set
            {
                this.htmlMessage = value;
                base.RaisePropertyChanged("HTMLMessage");
            }
        }

        public async void Initialize()
        {
            this.extractedFilesFromMessage.Clear();
            var file = await this.GetFileByRecievedToken();
            this.ExtractedMessage = await this.TnefToCollection(file);
            this.SerializeFiles(file);
            this.RtfFileExecute();
        }

        private async Task<StorageFile> GetFileByRecievedToken()
        {
            var result = default(StorageFile);
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(this.recievedFileToken))
            {
                result = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(this.recievedFileToken);
            }

            return result;
        }

        private async void OpenFileByDefaultProgram(FileInfo FileToOpen)
        {
            if (FileToOpen.ExtractedStorageFile != null)
            {
                await Windows.System.Launcher.LaunchFileAsync(FileToOpen.ExtractedStorageFile);
            }
        }

        private async void OpenWith(FileInfo fileToOpen)
        {
            if (fileToOpen.ExtractedStorageFile != null)
            {
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = true;
                await Windows.System.Launcher.LaunchFileAsync(fileToOpen.ExtractedStorageFile, options);
            }
        }

        private async void SaveAllAttachments(List<FileInfo> fileCollection)
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                foreach (FileInfo singleFile in fileCollection)
                {
                    var file = singleFile.ExtractedStorageFile;
                    if (file != null)
                    {
                        var storageFile = await file.CopyAsync(folder, file.Name, NameCollisionOption.FailIfExists);
                    }
                }
            }
        }

        private void SerializeFiles(StorageFile file)
        {
            RecentFilesProvider.Instance.AddDatFileToken(file);
            this.GetFilesFromMessage();
            LastExtractedFilesProvider.Instance.AddAttachmentToRecentFiles(this.ExtractedFilesFromMessage);
        }

        public async Task<Message> TnefToCollection(StorageFile tnefFile)
        {
            var extractedMessage = await DatParserProvider.Instance.OpenTnef(tnefFile);
            return extractedMessage;
        }

        private void GetFilesFromMessage()
        {
            if (this.ExtractedMessage != null && this.ExtractedMessage.Files != null)
            {
                this.ExtractedFilesFromMessage = new List<FileInfo>(this.ExtractedMessage.Files);
            }
        }

        public async void RtfFileExecute()
        {
            if (this.ExtractedMessage != null && this.ExtractedMessage.MessageFile != null)
            {
                if (this.ExtractedMessage.MessageFile.DisplayName.Contains("Message_Body") && (this.ExtractedMessage.MessageFile.DisplayName.Contains("rtf") || this.ExtractedMessage.MessageFile.DisplayName.Contains("txt")))
                {
                    this.RtfString = await Windows.Storage.FileIO.ReadTextAsync(this.ExtractedMessage.MessageFile);
                    this.HTMLMessage = null;
                }
                if (this.ExtractedMessage.MessageFile.DisplayName.Contains("Message_Body") && this.ExtractedMessage.MessageFile.DisplayName.Contains("html"))
                {
                    this.HTMLMessage = await Windows.Storage.FileIO.ReadTextAsync(this.ExtractedMessage.MessageFile);
                    this.RtfString = null;
                }
            }
        }
    }
}
