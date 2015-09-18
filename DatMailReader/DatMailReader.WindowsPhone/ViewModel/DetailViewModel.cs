using DatMailReader.Common;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Winmaildat;

namespace DatMailReader.ViewModel
{
    public class DetailViewModel : PropertyChange
    {
        private MimeMessage TnefMessage;
        CoreApplicationView view = CoreApplication.GetCurrentView();


        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChangedEvent("From");
            }
        }

        private string _from;

        public string From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                RaisePropertyChangedEvent("From");
            }
        }
        private string To { get; set; }
        private string DateSend { get; set; }

        private void initialize()
        {
            TnefMessage = MyReadTnef.getMessage();
            if (this.TnefMessage.Sender != null)
            {
                From = this.TnefMessage.Sender.Address;
            }
            else From = "undefined";
            if (this.TnefMessage.Subject != null)
            {
                Title = TnefMessage.Subject;
            }
            else Title = "Subject not set";
        }

        public DetailViewModel()
        {
            Messenger.Default.Register<ObservableCollection<FileInfo>>(this, "collection", (o) => { this.FileInf = o; });
        }

        private ObservableCollection<FileInfo> _file = new ObservableCollection<FileInfo>();

        public ObservableCollection<FileInfo> FileInf
        {
            get
            {
                return _file;
            }
            set
            {
                _file = value;
                this.RaisePropertyChangedEvent("FileInf");
                RtfFileExecute(_file);
                initialize();
            }
        }

        private string _fileSize;

        public string FileSize
        {
            get
            {
                return _fileSize;
            }
            set
            {
                if (FileSize != value)
                {
                    _fileSize = value;
                    RaisePropertyChangedEvent("FileSize");
                }
            }
        }

        private FileInfo _selectedFile;

        public FileInfo SelectedFile
        {
            get
            {
                return _selectedFile;
            }
            set
            {
                if (_selectedFile != value)
                {
                    _selectedFile = value;
                    RaisePropertyChangedEvent("SelectedFile");
                }
            }
        }

        private string _rtfString;

        public string RtfString
        {
            get
            {
                return _rtfString;
            }
            set
            {
                _rtfString = value;
                RaisePropertyChangedEvent("RtfString");
            }
        }

        private string _htmlMessage;

        public string HTMLMessage
        {
            get
            {
                return _htmlMessage;
            }
            set
            {
                _htmlMessage = value;
                RaisePropertyChangedEvent("HTMLMessage");
            }
        }

        private int _num;

        public int Num
        {
            get
            {
                return _num;
            }
            set
            {
                _num = value;
                RaisePropertyChangedEvent("Num");
            }
        }

        private RelayCommand<FileInfo> myItemClickCommand;

        public RelayCommand<FileInfo> MyItemClickCommand
        {
            get
            {
                if (myItemClickCommand == null)
                {
                    myItemClickCommand = new RelayCommand<FileInfo>(
                        (item) =>
                        {
                            OpenFileBeDefaultProgram(item);
                        });
                }

                return myItemClickCommand;
            }
        }

        private RelayCommand<FileInfo> _openFileWithCommand;

        public RelayCommand<FileInfo> OpenFileWithCommand
        {
            get
            {
                if (_openFileWithCommand == null)
                {
                    _openFileWithCommand = new RelayCommand<FileInfo>(
                           item => OpenWith(item));
                }

                return _openFileWithCommand;
            }
        }

        private RelayCommand<FileInfo> _openFlyoutFileWithCommand;

        public RelayCommand<FileInfo> OpenFlyoutFileWithCommand
        {
            get
            {
                if (_openFlyoutFileWithCommand == null)
                {
                    _openFlyoutFileWithCommand = new RelayCommand<FileInfo>(
                           item => OpenWith(item));
                }
                return _openFlyoutFileWithCommand;
            }
        }

        private RelayCommand<FileInfo> _saveSelectedFlyoutItem;

        public RelayCommand<FileInfo> SaveSelectedFlyoutItem
        {
            get
            {
                if (_saveSelectedFlyoutItem == null)
                {
                    _saveSelectedFlyoutItem = new RelayCommand<FileInfo>(
                        item => SaveSelectedAttachment(item));
                }
                return _saveSelectedFlyoutItem;

            }
        }

        private RelayCommand _saveAllCommand;

        public RelayCommand SaveAllCommand
        {
            get
            {
                if (_saveAllCommand == null)
                {
                    _saveAllCommand = new RelayCommand(
                           () => SaveAllAttachments(FileInf));
                }

                return _saveAllCommand;
            }
        }

        private async void OpenFileBeDefaultProgram(FileInfo file)
        {
            bool success = await Windows.System.Launcher.LaunchFileAsync(file.storageFile);
            if (!success)
            {
                throw new Exception("Can't open selected file");
            }
        }

        private async void OpenWith(FileInfo file)
        {
            var options = new Windows.System.LauncherOptions();
            options.DisplayApplicationPicker = true;
            bool success = await Windows.System.Launcher.LaunchFileAsync(file.storageFile, options);
            if (!success)
            {
                throw new Exception("Can't open selected file");
            }
        }

        private async void SaveSelectedAttachment(FileInfo file)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            folderPicker.CommitButtonText = "Save All";
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                StorageFile temp = file.storageFile;
                if (temp != null)
                {
                    StorageFile storageFile = await temp.CopyAsync(folder, temp.Name, NameCollisionOption.FailIfExists);
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

        public async void RtfFileExecute(ObservableCollection<FileInfo> file)
        {
            foreach (FileInfo f in file)
            {
                if (f.storageFile.DisplayName.Contains("Message_Body") && (f.storageFile.DisplayName.Contains("rtf") || f.storageFile.DisplayName.Contains("txt")))
                {
                    RtfString = await Windows.Storage.FileIO.ReadTextAsync(f.storageFile);
                    HTMLMessage = null;
                    break;
                }
                else
                    if (f.storageFile.DisplayName.Contains("Message_Body") && f.storageFile.DisplayName.Contains("html"))
                    {
                        HTMLMessage = await Windows.Storage.FileIO.ReadTextAsync(f.storageFile);
                        RtfString = null;
                        break;
                    }
            }

            DeleterRtfFromCollection(file);
        }

        public void DeleterRtfFromCollection(ObservableCollection<FileInfo> file)
        {
            foreach (FileInfo f in file)
            {
                if (f.storageFile.DisplayName.Contains("Message_Body"))
                {
                    file.Remove(f);
                    break;
                }
            }
        }
    }
}
