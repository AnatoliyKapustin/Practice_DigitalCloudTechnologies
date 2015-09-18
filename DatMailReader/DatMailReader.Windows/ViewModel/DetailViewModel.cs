using DatMailReader.Common;
using DatMailReader.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace DatMailReader.ViewModel
{
    public class DetailViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private MimeMessage tnefMessage = DatParserProvider.getMessage();
        private string title;
        private string from;
        private string To { get; set; }
        private string DateSend { get; set; }
        private string fileSize;
        private string rtfString;
        private string htmlMessage;
        private ObservableCollection<FileInfo> file = new ObservableCollection<FileInfo>();

        public RelayCommand<FileInfo> MyItemClickCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFileWithCommand { get; private set; }
        public RelayCommand<FileInfo> OpenFlyoutFileWithCommand { get; private set; }
        public RelayCommand<FileInfo> SaveSelectedFlyoutItem { get; private set; }
        public RelayCommand SaveAllCommand { get; private set; }
        public DetailViewModel()
        {
            Messenger.Default.Register<ObservableCollection<FileInfo>>(this, "collection", (o) => { this.FileInf = o; });
            this.MyItemClickCommand = new RelayCommand<FileInfo>(item => { this.OpenFileByDefaultProgram(item); });
            this.OpenFileWithCommand = new RelayCommand<FileInfo>(item => this.OpenWith(item));
            this.OpenFlyoutFileWithCommand = new RelayCommand<FileInfo>(item => this.OpenWith(item));
            this.SaveSelectedFlyoutItem = new RelayCommand<FileInfo>(item => this.SaveSelectedAttachment(item));
            this.SaveAllCommand = new RelayCommand(() => this.SaveAllAttachments(this.FileInf));

        }

        public string Title
        {
            get 
            { 
                return this.title; 
            }

            set
            {
                this.title = value;
                RaisePropertyChanged("From");
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
                RaisePropertyChanged("From");
            }
        }

        public ObservableCollection<FileInfo> FileInf
        {
            get 
            {
                return this.file; 
            }

            set
            {
                this.file = value;
                RaisePropertyChanged("FileInf");
                this.RtfFileExecute(this.file);
                this.Initialize();
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
                    RaisePropertyChanged("FileSize");
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
                RaisePropertyChanged("RtfString");
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
                RaisePropertyChanged("HTMLMessage");
            }
        }

        private void Initialize()
        {
            this.tnefMessage = DatParserProvider.getMessage();
            if (this.tnefMessage.Sender != null)
            {
                this.From = this.tnefMessage.Sender.Address;
            }
            else
            {
                this.From = "undefined";
            }

            if (this.tnefMessage.Subject != null)
            {
                this.Title = this.tnefMessage.Subject;
            }
            else
            {
                this.Title = "Subject not set";
            }
        }

        private async void OpenFileByDefaultProgram(FileInfo FileToOpen)
        {
            bool success = await Windows.System.Launcher.LaunchFileAsync(FileToOpen.storageFile);
            if (!success)
            {
                throw new Exception("Can't open selected file");
            }
        }

        private async void OpenWith(FileInfo FileToOpen)
        {
            var options = new Windows.System.LauncherOptions();
            options.DisplayApplicationPicker = true;
            bool success = await Windows.System.Launcher.LaunchFileAsync(FileToOpen.storageFile, options);
            if (!success)
            {
                throw new Exception("Can't open selected file");
            }
        }

        private async void SaveSelectedAttachment(FileInfo FileToSave)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                    StorageFile temp = FileToSave.storageFile;
                    if (temp != null)
                    {
                        StorageFile SaveFile = await temp.CopyAsync(folder, temp.Name, NameCollisionOption.FailIfExists);
                    } 
            }
        }

        private async void SaveAllAttachments(ObservableCollection<FileInfo> fileCollection)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                foreach (FileInfo f in fileCollection)
                {
                    StorageFile file = f.storageFile;
                    if (file != null)
                    {
                        StorageFile storageFile = await file.CopyAsync(folder, file.Name, NameCollisionOption.FailIfExists);
                    }
                }
            }
        }

        public async void RtfFileExecute(ObservableCollection<FileInfo> MessageFile)
        {
            foreach (FileInfo f in MessageFile)
            {
                if (f.storageFile.DisplayName.Contains("Message_Body") && (f.storageFile.DisplayName.Contains("rtf") || f.storageFile.DisplayName.Contains("txt")))
                {
                    this.RtfString = await Windows.Storage.FileIO.ReadTextAsync(f.storageFile);
                    this.HTMLMessage = null;
                    break;
                }
                else
                    if (f.storageFile.DisplayName.Contains("Message_Body") && f.storageFile.DisplayName.Contains("html"))
                    {
                        this.HTMLMessage = await Windows.Storage.FileIO.ReadTextAsync(f.storageFile);
                        this.RtfString = null;
                        break;
                    }
            }

            this.DeleterRtfFromCollection(MessageFile);
        }

        public void DeleterRtfFromCollection(ObservableCollection<FileInfo> DeleteRTFFile)
        {
            foreach (FileInfo f in DeleteRTFFile)
            {
                if (f.storageFile.DisplayName.Contains("Message_Body"))
                {
                    DeleteRTFFile.Remove(f);
                    break;
                }
            }
        }
    }
}
