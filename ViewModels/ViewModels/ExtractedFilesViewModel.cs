namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.DataAccess.Providers;
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Interfaces;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.AccessCache;
    using Windows.Storage.Streams;

    public class ExtractedFilesViewModel : GalaSoft.MvvmLight.ViewModelBase, INavigable
    {
        private string rtfString;
        private string htmlMessage;
        private string recievedFileToken;
        private Message extractedMessage;
        private List<FileInfo> extractedFilesFromMessage = new List<FileInfo>();
        private bool isVisibleAttachments = true;
        public IFilesSaveService FileSaveService;

        public ExtractedFilesViewModel()
        {
            this.AttachmentsVisibilityCommand = new RelayCommand(this.ChangeAtachmentsVisibilityExecute);
            this.ItemClickCommand = new RelayCommand<FileInfo>(this.OpenFileByDefaultProgramExecute);
            this.OpenFileWithCommand = new RelayCommand<FileInfo>(this.OpenWithExecute);
            this.SaveAllCommand = new RelayCommand(this.SaveAllAttachmentsExecute);
        }

        public RelayCommand AttachmentsVisibilityCommand { get; private set; }
        public RelayCommand<FileInfo> ItemClickCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFileWithCommand { get; private set; }
        public RelayCommand SaveAllCommand { get; private set; }

        public bool IsVisibleAttachments
        {
            get
            {
                return this.isVisibleAttachments;
            }

            set
            {
                this.isVisibleAttachments = value;
                base.RaisePropertyChanged("IsVisibleAttachments");
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

        private void ChangeAtachmentsVisibilityExecute()
        {
            this.IsVisibleAttachments = !this.IsVisibleAttachments;
        }

        public async void Activate(object parameter)
        {
            var extractTextParameters = parameter as ExtractTextParameters;
            if (extractTextParameters != null)
            {
                var recievedFileToken = extractTextParameters.FileToExtractToken;
                if (!String.IsNullOrEmpty(recievedFileToken))
                {
                    var file = await this.GetFileByRecievedToken(recievedFileToken);
                    await this.Initialize(file);
                }
            }
        }

        private async Task Initialize(StorageFile file)
        {
            if (file != null)
            {
                this.ExtractedMessage = await this.TnefToCollection(file);
                await this.SerializeFiles(file);
                this.RtfFileTextExtraction();
            }
        }

        private async Task<StorageFile> GetFileByRecievedToken(string token)
        {
            var result = default(StorageFile);
            if (StorageApplicationPermissions.FutureAccessList.ContainsItem(token))
            {
                result = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
            }

            return result;
        }

        private async void OpenFileByDefaultProgramExecute(FileInfo fileToOpen)
        {
            if (fileToOpen.ExtractedStorageFile != null)
            {
                await Windows.System.Launcher.LaunchFileAsync(fileToOpen.ExtractedStorageFile);
            }
        }

        private async void OpenWithExecute(FileInfo fileToOpen)
        {
            if (fileToOpen.ExtractedStorageFile != null)
            {
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = true;
                await Windows.System.Launcher.LaunchFileAsync(fileToOpen.ExtractedStorageFile, options);
            }
        }

        private async void SaveAllAttachmentsExecute()
        {
            await this.FileSaveService.LaunchFileSelectionServiceAsync();
            var folder = this.FileSaveService.CompleteOutstandingSelectionService();
            if (folder != null)
            {
                foreach (var singleFile in ExtractedFilesFromMessage)
                {
                    var file = singleFile.ExtractedStorageFile;
                    if (file != null)
                    {
                        await file.CopyAsync(folder, file.Name, NameCollisionOption.GenerateUniqueName);
                    }
                }
            }
        }

        private async Task SerializeFiles(StorageFile file)
        {
            await RecentFilesProvider.Instance.AddDatFileToken(file);
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

        public async void RtfFileTextExtraction()
        {
            if (this.ExtractedMessage != null && this.ExtractedMessage.MessageFile != null)
            {
                if (this.ExtractedMessage.MessageFile.DisplayName.Contains("Message_Body") && (this.ExtractedMessage.MessageFile.DisplayName.Contains("rtf") || this.ExtractedMessage.MessageFile.DisplayName.Contains("txt")))
                {
                    using (var inputStream = await this.ExtractedMessage.MessageFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    {
                        using (var readStream = inputStream.GetInputStreamAt(0))
                        {
                            var reader = new DataReader(readStream);
                            uint fileLength = await reader.LoadAsync((uint)inputStream.Size);
                            this.RtfString = reader.ReadString(fileLength);
                        }
                    }

                    this.RtfString = await Windows.Storage.FileIO.ReadTextAsync(this.ExtractedMessage.MessageFile);
                    this.HTMLMessage = null;
                }
                if (this.ExtractedMessage.MessageFile.DisplayName.Contains("Message_Body") && this.ExtractedMessage.MessageFile.DisplayName.Contains("html"))
                {
                    using (var inputStream = await this.ExtractedMessage.MessageFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                    {
                        using (var readStream = inputStream.GetInputStreamAt(0))
                        {
                            var reader = new DataReader(readStream);
                            uint fileLength = await reader.LoadAsync((uint)inputStream.Size);
                            this.HTMLMessage = reader.ReadString(fileLength);
                        }
                    }
                    
                    this.RtfString = null;
                }
            }
        }
    }
}
