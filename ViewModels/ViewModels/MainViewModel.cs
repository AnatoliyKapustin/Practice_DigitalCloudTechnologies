namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Constants;
    using DatMailReader.Models.Interfaces;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.AccessCache;

    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private ObservableCollection<FileInfo> recentlyExtractedAttachments = new ObservableCollection<FileInfo>();
        private ObservableCollection<StorageFile> recentlyExtractedFiles = new ObservableCollection<StorageFile>();
        public IFileSelectionService FileOpenService;

        public MainViewModel()
        {
            this.OpenFileCommand = new RelayCommand(this.OpenFileExecute);
            this.GotoAllDatFilesPageCommand = new RelayCommand(this.NavigateToAllDatFilesPageExecute);
            this.GoToAllAttachmentsPageCommand = new RelayCommand(this.NavigateToAllAttachmentFilesPageExecute);
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>(this.OpenExtractedFileBeDefaultProgramExecute);
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>(this.NavigateToDetailsPageExecute);
            this.ClearLastAttachmentsButtonClickCommand = new RelayCommand(this.ClearAttachmentsCollectionExecute);
            this.ClearLastDatFilesButtonClickCommand = new RelayCommand(this.ClearDatFilesCollectionExecute);
        }

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand GotoAllDatFilesPageCommand { get; private set; }
        public RelayCommand GoToAllAttachmentsPageCommand { get; private set; }
        public RelayCommand<FileInfo> ExtractedItemClickCommand { get; private set; }
        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand { get; private set; }
        public RelayCommand ClearLastDatFilesButtonClickCommand { get; private set; }
        public RelayCommand ClearLastAttachmentsButtonClickCommand { get; private set; }

        public ObservableCollection<FileInfo> RecentlyExtractedAttachments
        {
            get
            {
                return recentlyExtractedAttachments;
            }

            set
            {
                this.recentlyExtractedAttachments = value;
                base.RaisePropertyChanged("RecentlyExtractedAttachments");
            }
        }

        public ObservableCollection<StorageFile> RecentlyExtractedFiles
        {
            get
            {
                return this.recentlyExtractedFiles;
            }

            set
            {
                this.recentlyExtractedFiles = value;
                base.RaisePropertyChanged("RecentlyExtractedFiles");
            }
        }
        public async Task Initialize()
        {
            await DeserializeRecentDatFiles();
            await DeserializeRecentAttachments();
        }

        private async Task DeserializeRecentDatFiles()
        {
            this.RecentlyExtractedFiles = new ObservableCollection<StorageFile>(await RecentFilesProvider.Instance.GetRecentlyExtractedMails());
        }

        private async Task DeserializeRecentAttachments()
        {
            this.RecentlyExtractedAttachments = new ObservableCollection<FileInfo>(await LastExtractedFilesProvider.Instance.GetAttachmentFiles());
        }

        private void ClearDatFilesCollectionExecute()
        {
            this.RecentlyExtractedFiles.Clear();
            RecentFilesProvider.Instance.ClearRecentDatFilesCollection();
        }

        private void ClearAttachmentsCollectionExecute()
        {
            this.RecentlyExtractedAttachments.Clear();
            LastExtractedFilesProvider.Instance.ClearLastAttachmentsCollection();
        }

        private async void OpenExtractedFileBeDefaultProgramExecute(FileInfo fileToOpen)
        {            
            if (!String.IsNullOrEmpty(fileToOpen.FilePath))
            {
                var temp = default(StorageFile);
                try
                {
                    temp = await StorageFile.GetFileFromPathAsync(fileToOpen.FilePath);
                    var options = new Windows.System.LauncherOptions();
                    await Windows.System.Launcher.LaunchFileAsync(temp);
                }
                catch (FileNotFoundException) { };  
            }
        }

        private async void OpenFileExecute()
        {
            await this.FileOpenService.LaunchFileSelectionServiceAsync();
            var selectedFile = this.FileOpenService.CompleteOutstandingSelectionService();
            if (selectedFile != null)
            {
                this.NavigateToDetailsPageExecute(selectedFile);
            }
        }

        private void NavigateToDetailsPageExecute(StorageFile file)
        {
            var parameter = new ExtractTextParameters();
            parameter.FileToExtractToken = Constants.FileToExtractToken;
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(Constants.FileToExtractToken, file);
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.ExtractedFilePageKey, parameter);
        }

        private void NavigateToAllDatFilesPageExecute()
        {
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.AllDatFilesPage);
        }

        private void NavigateToAllAttachmentFilesPageExecute()
        {
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.AllAttachmentsPage);
        }
    }
}
