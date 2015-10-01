using DatReaderPortable.Common;
using DatReaderPortable.Models;
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

namespace DatMailReader.ViewModel
{
    [DataContract]
    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private RecentFilesProvider RecentFiles = new RecentFilesProvider();
        private LastExtractedFilesProvider RecentAttachments = new LastExtractedFilesProvider();
        private DatParserProvider DatParser = new DatParserProvider();
        private INavigationService NavigationService;
        private MimeMessage TnefMessage;
        private ObservableCollection<string> fileName;
        private ObservableCollection<string> RecentlyExtractedMailPath = new ObservableCollection<string>();
        private ObservableCollection<FileInfo> RecentlyExtractedAttPath = new ObservableCollection<FileInfo>();
        private ObservableCollection<string> ExtractedFromMail = new ObservableCollection<string>();
        private string UniqId = string.Empty;

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
            catch { }
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
                    if (!success)
                    {

                    }
                }
            }
            catch
            {
                throw new Exception("Can't open selected file");
            }
        }

        private void OpenFileExecute()
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
