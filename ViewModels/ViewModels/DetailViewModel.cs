namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.DataAccess.Providers;
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class DetailViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private RecentFilesProvider recentFiles = RecentFilesProvider.Instance;
        private LastExtractedFilesProvider recentAttachments = LastExtractedFilesProvider.Instance;
        private DatParserProvider datParser = DatParserProvider.Instance;
        private string title;
        private string sender;
        private string to;
        private string dateSent;
        private string fileSize;
        private string rtfString;
        private string htmlMessage;
        private List<FileInfo> file;
        private StorageFile recievedFile;
        private Message extractedMessage;
        private List<FileInfo> extractedFilesFromMessage = new List<FileInfo>();
      
        public DetailViewModel()
        {
            /*Messenger.Default.Register<StorageFile>(this, "datFile", (o) =>
            {
                this.RecievedFile = (o);                     
            });*/

            this.MyItemClickCommand = new RelayCommand<FileInfo>(item => { this.OpenFileByDefaultProgram(item); });
            this.OpenFileWithCommand = new RelayCommand<FileInfo>(item => this.OpenWith(item));
            this.OpenFlyoutFileWithCommand = new RelayCommand<FileInfo>(item => this.OpenWith(item));
            this.SaveSelectedFlyoutItem = new RelayCommand<FileInfo>(item => this.SaveSelectedAttachment(item));
            this.SaveAllCommand = new RelayCommand(() => this.SaveAllAttachments(this.ExtractedFilesFromMessage));
        }

        public RelayCommand<FileInfo> MyItemClickCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFileWithCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFlyoutFileWithCommand { get; private set; }
        public RelayCommand<FileInfo> SaveSelectedFlyoutItem { get; private set; }
        public RelayCommand SaveAllCommand { get; private set; }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
                base.RaisePropertyChanged("Title");
            }
        }

        public string Sender
        {
            get
            {
                return this.sender;
            }

            set
            {
                this.sender = value;
                base.RaisePropertyChanged("Sender");
            }
        }

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

        public string FileSize
        {
            get
            {
                return this.fileSize;
            }

            set
            {
                if (this.fileSize != value)
                {
                    this.fileSize = value;
                    base.RaisePropertyChanged("FileSize");
                }
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

        public StorageFile RecievedFile
        {
            get
            {
                return this.recievedFile;
            }

            set
            {
                this.recievedFile = value;
                base.RaisePropertyChanged("RecievedFile");
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
            this.ExtractedMessage = await this.TnefToCollection(this.RecievedFile, this.ExtractedMessage);
            this.SerializeFiles();
            this.RtfFileExecute();
            if (this.ExtractedMessage != null)
            {
                this.Sender = this.ExtractedMessage.Sender;
                if (this.ExtractedMessage.Subject != string.Empty)
                {
                    this.Title = this.ExtractedMessage.Subject;
                }
                else
                {
                    this.Title = "Subject not set";
                }
            }
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

        private async void SaveSelectedAttachment(FileInfo fileToSave)
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageFile temp = fileToSave.ExtractedStorageFile;
                if (temp != null)
                {
                    StorageFile SaveFile = await temp.CopyAsync(folder, temp.Name, NameCollisionOption.FailIfExists);
                }
            }
        }

        private async void SaveAllAttachments(List<FileInfo> fileCollection)
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                foreach (FileInfo singleFile in fileCollection)
                {
                    StorageFile file = singleFile.ExtractedStorageFile;
                    if (file != null)
                    {
                        StorageFile storageFile = await file.CopyAsync(folder, file.Name, NameCollisionOption.FailIfExists);
                    }
                }
            }
        }

        private void SerializeFiles()
        {
            this.recentFiles.AddDatFileToken(this.RecievedFile);
            this.GetFilesFromMessage();
            this.recentAttachments.AddAttachmentToRecentFiles(this.ExtractedFilesFromMessage);
        }

        public async Task<Message> TnefToCollection(StorageFile tnefFile, Message targetCollection)
        {
            var extractedMessage = await datParser.OpenTnef(tnefFile, targetCollection);
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
