using DatMailReader.Common;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Winmaildat;

namespace DatMailReader.ViewModel
{
    [DataContract]
    public class MainViewModel : PropertyChange
    {
        [DataMember]
        public MimeMessage TnefMessage;
        public static StorageFile file;
        CoreApplicationView view = CoreApplication.GetCurrentView();
        private INavigationService _navigationService;

        private static ObservableCollection<string> _recentlyExtractedMailPath = new ObservableCollection<string>();
        private static ObservableCollection<FileInfo> _recentlyExtractedAttPath = new ObservableCollection<FileInfo>();

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private static ObservableCollection<string> _mailTitle = new ObservableCollection<string>();
        public ObservableCollection<string> MailTitle
        {
            get { return _mailTitle; }
            set
            {
                _mailTitle = value;
                RaisePropertyChangedEvent("MailTitle");
            }
        }

        private static ObservableCollection<string> _fileName;
        public ObservableCollection<string> FileName
        {
            get { return _fileName; }

            set
            {
                _fileName = value;
                RaisePropertyChangedEvent("FileName");
            }
        }

        public static async void init()
        {
            _recentlyExtractedAttPath = await ObjectSerializer.Deserialize(_recentlyExtractedAttPath, "WPAttData.xml");
            _recentlyExtractedMailPath = await ObjectSerializer.Deserialize(_recentlyExtractedMailPath, "WPMailData.xml");
            getFilesFromPath(_recentlyExtractedMailPath);
            getAttFilesFromPath(_recentlyExtractedAttPath);
        }

        private ObservableCollection<FileInfo> _file = new ObservableCollection<FileInfo>();
        [DataMember]
        public ObservableCollection<FileInfo> File
        {
            get { return _file; }
            set
            {
                _file = value;
                RaisePropertyChangedEvent("File");
            }
        }

        private static ObservableCollection<FileInfo> _recentlyEctractedAtt = new ObservableCollection<FileInfo>();
        public ObservableCollection<FileInfo> RecentlyExtractedAtt
        {
            get { return _recentlyEctractedAtt; }
            set
            {
                _recentlyEctractedAtt = value;
                RaisePropertyChangedEvent("RecentlyExtractedAtt");
            }
        }

        private static ObservableCollection<StorageFile> _recentlyEctractedFiles = new ObservableCollection<StorageFile>();
        public ObservableCollection<StorageFile> RecentlyExtractedFiles
        {
            get { return _recentlyEctractedFiles; }
            set
            {
                _recentlyEctractedFiles = value;
                RaisePropertyChangedEvent("RecentlyExtractedFiles");
            }
        }

        private RelayCommand _navigateCommand;

        public RelayCommand NavigateCommand
        {
            get
            {
                return _navigateCommand
                       ?? (_navigateCommand = new RelayCommand(
                           () => _navigationService.NavigateTo(ViewModelLocator.SecondPageKey1)));
            }
        }

        private RelayCommand _openFileCommand;
        public RelayCommand OpenFileCommand
        {
            get
            {
                if (_openFileCommand == null)
                {
                    _openFileCommand = new RelayCommand(() => OpenFileClick());
                }

                return _openFileCommand;
            }
        }

        private RelayCommand<FileInfo> _extractedItemClickCommand;
        public RelayCommand<FileInfo> ExtractedItemClickCommand
        {
            get
            {
                if (_extractedItemClickCommand == null)
                {
                    _extractedItemClickCommand = new RelayCommand<FileInfo>(
                        (item) =>
                        {
                            OpenExtractedFileBeDefaultProgram(item);
                        });
                }

                return _extractedItemClickCommand;
            }
        }

        private RelayCommand<StorageFile> _recentlyOpenedMailClickCommand;
        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand
        {
            get
            {
                if (_recentlyOpenedMailClickCommand == null)
                {
                    _recentlyOpenedMailClickCommand = new RelayCommand<StorageFile>(
                        (item) =>
                        {
                            ExtractAttAndNavigate(item);
                        });
                }

                return _recentlyOpenedMailClickCommand;
            }
        }

        private async void OpenExtractedFileBeDefaultProgram(FileInfo file)
        {
            try
            {
                if (file.filePath != null)
                {
                    StorageFile temp = await StorageFile.GetFileFromPathAsync(file.filePath);
                    var options = new Windows.System.LauncherOptions();
                    bool success = await Windows.System.Launcher.LaunchFileAsync(temp);
                }
            }
            catch { throw new Exception("Can't open selected file"); }
        }

        private void addMailPathToCollection(string filePath)
        {
            _recentlyExtractedMailPath.Add(filePath);
            _recentlyExtractedMailPath = deleteItems(_recentlyExtractedMailPath);
            ObjectSerializer.Serialize(_recentlyExtractedMailPath, "WPMailData.xml");
        }

        private void addAttPathToCollection(ObservableCollection<FileInfo> path)
        {
            ObservableCollection<FileInfo> temp = _recentlyExtractedAttPath;
            if (temp != null)
            {
                foreach (FileInfo f in path)
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
                ObjectSerializer.Serialize(temp, "WPAttData.xml");
            }
        }

        private ObservableCollection<string> deleteItems(ObservableCollection<string> path)
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

        private static async void getFilesFromPath(ObservableCollection<string> path)
        {
            _recentlyEctractedFiles.Clear();
            foreach(String s in path)
            {
                try
                {
                    _recentlyEctractedFiles.Add(await StorageApplicationPermissions.FutureAccessList.GetFileAsync(s));
                }
                catch { new Exception("Ошибка инициализации списка последних открытых файлов"); }
            }
        }
        


        private static void getAttFilesFromPath(ObservableCollection<FileInfo> path)
        {
            try
            {
                _recentlyEctractedAtt.Clear();
                _mailTitle.Clear();
                foreach (FileInfo s in path)
                {

                    _recentlyEctractedAtt.Add(s);//await Windows.Storage.StorageFile.GetFileFromPathAsync(s.filePath));
                    _mailTitle.Add(s.From);

                }
            }
            catch {/* throw new Exception("Ошибка инициализации списка последних извлеченных файлов"); */}
        }

        private void OpenFileClick()
        {
            try
            {
                FileOpenPicker fileOpenPicker2 = new FileOpenPicker();
                fileOpenPicker2.ViewMode = PickerViewMode.List;
                fileOpenPicker2.FileTypeFilter.Add(".dat");
                fileOpenPicker2.PickSingleFileAndContinue(); 
                view.Activated += viewActivated;              
            }
            catch (Exception ex)
            {
            }    
        }

        private async void viewActivated(CoreApplicationView sender, IActivatedEventArgs args1)
        {
            FileOpenPickerContinuationEventArgs args = args1 as FileOpenPickerContinuationEventArgs;

            if (args != null)
            {
                if (args.Files.Count == 0) return;

                view.Activated -= viewActivated;
                file = args.Files[0];
            }
            if (file != null)
            {
                StorageApplicationPermissions.FutureAccessList.AddOrReplace(file.Name, file);
                addMailPathToCollection(file.Name);
                _file.Clear();
                File.Clear();
                File = await TnefToCollection(file, _file);
                addAttPathToCollection(File);
                TnefMessage = MyReadTnef.getMessage();
                NavigateToDetailsPage(File);
            } 
        }

        private async void ExtractAttAndNavigate(StorageFile datFile)
        {
            File.Clear();
            File = await TnefToCollection(datFile, _file);
            NavigateToDetailsPage(File);
        }

        private void NavigateToDetailsPage(ObservableCollection<FileInfo> collection)
        {
            _navigationService.NavigateTo(ViewModelLocator.SecondPageKey1);
            Messenger.Default.Send<ObservableCollection<FileInfo>>(collection, "collection");  
        }

        public async Task<ObservableCollection<FileInfo>> TnefToCollection(StorageFile tnefFile, ObservableCollection<FileInfo> targetCollection)
        {
            _file = await MyReadTnef.OpenTnef(tnefFile, targetCollection);
            return _file;
        } 
    }
}
