namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using GalaSoft.MvvmLight.Views;
    using MimeKit;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private RecentFilesProvider recentFiles = new RecentFilesProvider();
        private LastExtractedFilesProvider recentAttachments = new LastExtractedFilesProvider();
        private DatParserProvider datParser = new DatParserProvider();
        private INavigationService navigationService;
        private ObservableCollection<FileInfo> file = new ObservableCollection<FileInfo>();
        private ObservableCollection<FileInfo> recentlyExtractedAtt = new ObservableCollection<FileInfo>();
        private ObservableCollection<StorageFile> recentlyEctractedFiles = new ObservableCollection<StorageFile>();
        private string uniqId = string.Empty;
        public MainViewModel(INavigationService navService)
        {
            this.navigationService = navService;
            this.NavigateCommand = new RelayCommand(() => this.navigationService.NavigateTo(ViewModelLocatorPCL.SecondPageKey));
            this.OpenFileCommand = new RelayCommand(() => this.OpenFileExecute());
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>((item) => { this.OpenExtractedFileBeDefaultProgram(item); });
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>((item) => { this.ExtractAttAndNavigate(item); });
        }

        public RelayCommand NavigateCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand<FileInfo> ExtractedItemClickCommand { get; private set; }
        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand { get; private set; }

        public async void Init()
        {
            this.RecentlyExtractedFiles = new ObservableCollection<StorageFile>(await this.recentFiles.GetRecentlyExtractedMails());
            this.RecentlyExtractedAtt = new ObservableCollection<FileInfo>(await this.recentAttachments.GetAttFiles());
        }

        public ObservableCollection<FileInfo> File
        {
            get
            {
                return this.file;
            }

            set
            {
                this.file = value;
                base.RaisePropertyChanged("File");
            }
        }

        public ObservableCollection<FileInfo> RecentlyExtractedAtt
        {
            get
            {
                return recentlyExtractedAtt;
            }

            set
            {
                this.recentlyExtractedAtt = value;
                base.RaisePropertyChanged("RecentlyExtractedAtt");
            }
        }

        public ObservableCollection<StorageFile> RecentlyExtractedFiles
        {
            get
            {
                return this.recentlyEctractedFiles;
            }

            set
            {
                this.recentlyEctractedFiles = value;
                base.RaisePropertyChanged("RecentlyExtractedFiles");
            }
        }

        private async void OpenExtractedFileBeDefaultProgram(FileInfo FileToOpen)
        {
            try
            {
                if (FileToOpen.FilePath != null)
                {
                    StorageFile temp = await StorageFile.GetFileFromPathAsync(FileToOpen.FilePath);
                    var options = new Windows.System.LauncherOptions();
                    await Windows.System.Launcher.LaunchFileAsync(temp);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("Can't open selected file", ex);
            }
        }

        private async void OpenFileExecute()
        {
            FileOpenPicker fileOpenPicker2 = new FileOpenPicker();
            fileOpenPicker2.ViewMode = PickerViewMode.List;
            fileOpenPicker2.FileTypeFilter.Add(".dat");
            StorageFile SelectedFile = await fileOpenPicker2.PickSingleFileAsync();
            if (SelectedFile != null)
            {
                recentFiles.AddDatFileToken(SelectedFile);
                this.file.Clear();
                this.File.Clear();
                this.File = await TnefToCollection(SelectedFile, this.file);
                recentAttachments.AddAttachmentToRecentFiles(this.File);
                NavigateToDetailsPage(this.File);
            }
        }

        private async void ExtractAttAndNavigate(StorageFile DatFile)
        {
            this.File.Clear();
            this.File = await TnefToCollection(DatFile, this.file);
            NavigateToDetailsPage(File);
        }

        private void NavigateToDetailsPage(ObservableCollection<FileInfo> collection)
        {
            this.navigationService.NavigateTo(ViewModelLocatorPCL.SecondPageKey);
            Messenger.Default.Send(collection, "collection");
        }

        public async Task<ObservableCollection<FileInfo>> TnefToCollection(StorageFile tnefFile, ObservableCollection<FileInfo> targetCollection)
        {
            this.file = await datParser.OpenTnef(tnefFile, targetCollection);
            return file;
        }
    }
}
