using DatMailReader.Common;
using DatMailReader.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using MimeKit;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace DatMailReader.ViewModel
{
    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private RecentFilesProvider RecentFiles = new RecentFilesProvider();
        private LastExtractedFilesProvider RecentAttachments = new LastExtractedFilesProvider();
        private INavigationService NavigationService;
        private MimeMessage TnefMessage;
        private ObservableCollection<string> fileName;
        private ObservableCollection<string> RecentlyExtractedMailPath = new ObservableCollection<string>();
        private ObservableCollection<FileInfo> RecentlyExtractedAttPath = new ObservableCollection<FileInfo>();
        private ObservableCollection<string> ExtractedFromMail = new ObservableCollection<string>();
        
        public RelayCommand NavigateCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand<FileInfo> ExtractedItemClickCommand { get; private set; }
        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand { get; private set; }

        public MainViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            this.NavigateCommand = new RelayCommand(() => this.NavigationService.NavigateTo(ViewModelLocator.SecondPageKey));
            this.OpenFileCommand = new RelayCommand(() => this.OpenFileExecute());
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>((item) => { this.OpenExtractedFileBeDefaultProgram(item); });
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>((item) => { this.ExtractAttAndNavigate(item); });
        } 

        public ObservableCollection<string> MailTitle
        {
            get 
            { 
                return ExtractedFromMail; 
            }

            set
            {
                ExtractedFromMail = value;
                RaisePropertyChanged("MailTitle");
            }
        }

        public ObservableCollection<string> FileName
        {
            get 
            { 
                return fileName; 
            }

            set
            {
                fileName = value;
                RaisePropertyChanged("FileName");
            }
        }

        public async void Init()
        {
            try
            {
                RecentlyExtractedFiles = await RecentFiles.Get();
                RecentlyExtractedAtt = await RecentAttachments.GetFiles();
                /// MailTitle = RecentAttachments.GetFileFromExtracted();
                /// RecentlyExtractedAttPath = await ObjectSerializer.Deserialize(RecentlyExtractedAttPath, "AttData.json");
                /// RecentlyExtractedMailPath = await ObjectSerializer.Deserialize(RecentlyExtractedMailPath, "MailData.json");
                /// GetFilesFromPath(RecentlyExtractedMailPath);
                /// GetAttFilesFromPath(RecentlyExtractedAttPath);
            }
            catch { throw new Exception("Initialization error"); }
        }

        private ObservableCollection<FileInfo> file = new ObservableCollection<FileInfo>();
        public ObservableCollection<FileInfo> File
        {
            get 
            { 
                return this.file; 
            }

            set
            {
                this.file = value;
                RaisePropertyChanged("File");
            }
        }

        private ObservableCollection<FileInfo> recentlyExtractedAtt = new ObservableCollection<FileInfo>();

        public ObservableCollection<FileInfo> RecentlyExtractedAtt
        {
            get 
            { 
                return recentlyExtractedAtt; 
            }

            set
            {
                recentlyExtractedAtt = value;
                RaisePropertyChanged("RecentlyExtractedAtt");
            }
        }

        private ObservableCollection<StorageFile> recentlyEctractedFiles = new ObservableCollection<StorageFile>();

        public ObservableCollection<StorageFile> RecentlyExtractedFiles
        {
            get 
            { 
                return recentlyEctractedFiles; 
            }

            set
            {
                recentlyEctractedFiles = value;
                RaisePropertyChanged("RecentlyExtractedFiles");
            }
        }

        private async void OpenExtractedFileBeDefaultProgram(FileInfo FileToOpen)
        {
            try
            {
                if (FileToOpen.filePath != null)
                {
                    StorageFile temp = await StorageFile.GetFileFromPathAsync(FileToOpen.filePath);
                    var options = new Windows.System.LauncherOptions();
                    bool success = await Windows.System.Launcher.LaunchFileAsync(temp);
                    if(!success)
                    {
                        
                    }
                }
            }
            catch 
            { 
                throw new Exception("Can't open selected file"); 
            }
        }

       /* private void AddMailPathToCollection(string filePath)
        {
            RecentlyExtractedMailPath.Add(filePath);
            RecentlyExtractedMailPath = DeleteItems(RecentlyExtractedMailPath);
            ObjectSerializer.Serialize(RecentlyExtractedMailPath, "MailData.json");
        }*/

       /* private void AddAttPathToCollection(ObservableCollection<FileInfo> path)
        {
            ObservableCollection<FileInfo> temp = RecentlyExtractedAttPath;
            foreach (FileInfo f in path)
            {
                if (!(f._storageFile.DisplayName.Contains("Message_Body")))
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

            ObjectSerializer.Serialize(temp, "AttData.json");
        }

       /* private ObservableCollection<string> DeleteItems(ObservableCollection<string> path)
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
        }*/

       /* private static async void GetFilesFromPath(ObservableCollection<string> path)
        {
            recentlyEctractedFiles.Clear();
            foreach(String s in path)
            {
                try
                {
                    recentlyEctractedFiles.Add(await StorageApplicationPermissions.FutureAccessList.GetFileAsync(s));
                }
                catch 
                { 
                    new Exception("Ошибка инициализации списка последних открытых файлов"); 
                }
            }
        }*/

        /*private void GetAttFilesFromPath(ObservableCollection<FileInfo> path)
        {
            recentlyEctractedAtt.Clear();
            _mailTitle.Clear();
            foreach (FileInfo s in path)
            {
                try
                {
                    recentlyEctractedAtt.Add(s);
                    _mailTitle.Add(s.From);
                }
                catch 
                { 
                    throw new Exception("Ошибка инициализации списка последних извлеченных файлов"); 
                }
            }
        }*/

        private async void OpenFileExecute()
        {
            try
            {
                FileOpenPicker fileOpenPicker2 = new FileOpenPicker();
                fileOpenPicker2.ViewMode = PickerViewMode.List;
                fileOpenPicker2.FileTypeFilter.Add(".dat");
                StorageFile SelectedFile = await fileOpenPicker2.PickSingleFileAsync(); 
                if (SelectedFile != null)
                {
                    StorageApplicationPermissions.FutureAccessList.AddOrReplace(SelectedFile.Name, SelectedFile);
                    RecentFiles.Add(SelectedFile.Name);
                    this.file.Clear();
                    File.Clear();
                    File = await TnefToCollection(SelectedFile, this.file);
                    RecentAttachments.Add(File);
                    TnefMessage = DatParserProvider.getMessage();
                    NavigateToDetailsPage(File);
                }                 
            }
            catch (Exception ex)
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog(ex.Message, "Can not open selected file");
                messageDialog.ShowAsync();
            }    
        }

        private async void ExtractAttAndNavigate(StorageFile DatFile)
        {
            File.Clear();
            File = await TnefToCollection(DatFile, this.file);
            NavigateToDetailsPage(File);
        }

        private void NavigateToDetailsPage(ObservableCollection<FileInfo> collection)
        {
            NavigationService.NavigateTo(ViewModelLocator.SecondPageKey);
            Messenger.Default.Send(collection, "collection");  
        }

        public async Task<ObservableCollection<FileInfo>> TnefToCollection(StorageFile tnefFile, ObservableCollection<FileInfo> targetCollection)
        {
            this.file = await DatParserProvider.OpenTnef(tnefFile, targetCollection);
            return file;
        } 
    }
}
