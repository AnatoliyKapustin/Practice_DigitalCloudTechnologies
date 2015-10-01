namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using MimeKit;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class DetailViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private DatParserProvider datParser = new DatParserProvider();
        private MimeMessage tnefMessage;
        private string title;
        private string from;
        private string to;
        private string dateSent;
        private string fileSize;
        private string rtfString;
        private string htmlMessage;
        private List<FileInfo> file = new List<FileInfo>();
        public DetailViewModel()
        {
            Messenger.Default.Register<ObservableCollection<FileInfo>>(this, "collection", (o) => { this.FileInf = new List<FileInfo>(o);
            RtfFileExecute();
            Initialize();
            });
            
            this.MyItemClickCommand = new RelayCommand<FileInfo>(item => { this.OpenFileByDefaultProgram(item); });
            this.OpenFileWithCommand = new RelayCommand<FileInfo>(item => this.OpenWith(item));
            this.OpenFlyoutFileWithCommand = new RelayCommand<FileInfo>(item => this.OpenWith(item));
            this.SaveSelectedFlyoutItem = new RelayCommand<FileInfo>(item => this.SaveSelectedAttachment(item));
            this.SaveAllCommand = new RelayCommand(() => this.SaveAllAttachments(this.FileInf));
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
                base.RaisePropertyChanged("From");
            }
        }

        public string From
        {
            get
            {
                return this.from;
            }

            set
            {
                this.from = value;
                base.RaisePropertyChanged("From");
            }
        }

        public List<FileInfo> FileInf
        {
            get
            {
                return this.file;
            }

            set
            {
                this.file = value;
                base.RaisePropertyChanged("FileInf");
               
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

        public void Initialize()
        {
            if (this.file != null)
            {
                if (this.file[0].Subject != null)
                {
                    this.Title = this.file[1].Subject;
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
            FolderPicker folderPicker = new FolderPicker();
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
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                foreach (FileInfo f in fileCollection)
                {
                    StorageFile file = f.ExtractedStorageFile;
                    if (file != null)
                    {
                        StorageFile storageFile = await file.CopyAsync(folder, file.Name, NameCollisionOption.FailIfExists);
                    }
                }
            }
        }

        public async void RtfFileExecute()
        {
            foreach (FileInfo f in file)
            {
                if (f.ExtractedStorageFile.DisplayName.Contains("Message_Body") && (f.ExtractedStorageFile.DisplayName.Contains("rtf") || f.ExtractedStorageFile.DisplayName.Contains("txt")))
                {
                    this.RtfString = await Windows.Storage.FileIO.ReadTextAsync(f.ExtractedStorageFile);
                    this.HTMLMessage = null;
                    break;
                }
                else
                    if (f.ExtractedStorageFile.DisplayName.Contains("Message_Body") && f.ExtractedStorageFile.DisplayName.Contains("html"))
                    {
                        this.HTMLMessage = await Windows.Storage.FileIO.ReadTextAsync(f.ExtractedStorageFile);
                        this.RtfString = null;
                        break;
                    }
            }

           /// this.DeleterRtfFromCollection();
        }

        public void DeleterRtfFromCollection()
        {
            foreach (FileInfo f in file)
            {
                if (f.ExtractedStorageFile.DisplayName.Contains("Message_Body"))
                {
                    file.Remove(f);
                    break;
                }
            }
        }
    }
}
