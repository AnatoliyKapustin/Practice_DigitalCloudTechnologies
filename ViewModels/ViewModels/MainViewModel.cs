namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Constants;
    using DatMailReader.Models.Interfaces;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Windows.Storage;
    using Windows.Storage.AccessCache;

    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<FileInfo> recentlyExtractedAttachments = new List<FileInfo>();
        private List<StorageFile> recentlyExtractedFiles = new List<StorageFile>();
        public IFileSelectionService fileOpenService;

        public MainViewModel()
        {
            this.OpenFileCommand = new RelayCommand(this.OpenFileExecute);
            this.GotoAllDatFilesPageCommand = new RelayCommand(this.NavigateToAllDatFilesPage);
            this.GoToAllAttachmentsPageCommand = new RelayCommand(this.NavigateToAllAttachmentFilesPage);
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>(this.OpenExtractedFileBeDefaultProgram);
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>(this.NavigateToDetailsPage);
        }

        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand GotoAllDatFilesPageCommand { get; private set; }
        public RelayCommand GoToAllAttachmentsPageCommand { get; private set; }
        public RelayCommand<FileInfo> ExtractedItemClickCommand { get; private set; }
        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand { get; private set; }

        public List<FileInfo> RecentlyExtractedAttachments
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

        public List<StorageFile> RecentlyExtractedFiles
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

        public async void Initialize()
        {
            this.RecentlyExtractedFiles = new List<StorageFile>(await RecentFilesProvider.Instance.GetRecentlyExtractedMails());
            this.RecentlyExtractedAttachments = new List<FileInfo>(await LastExtractedFilesProvider.Instance.GetAttFiles());
        }

        private async void OpenExtractedFileBeDefaultProgram(FileInfo FileToOpen)
        {
            var temp = default(StorageFile);
            if (FileToOpen.FilePath != null)
            {
                try
                {
                    temp = await StorageFile.GetFileFromPathAsync(FileToOpen.FilePath);
                }
                catch (FileNotFoundException) { };
                var options = new Windows.System.LauncherOptions();
                await Windows.System.Launcher.LaunchFileAsync(temp);
            }
        }

        private async void OpenFileExecute()
        {
            await this.fileOpenService.LaunchFileSelectionServiceAsync();
            var selectedFile = this.fileOpenService.CompleteOutstandingSelectionService();
            if (selectedFile != null)
            {
                this.NavigateToDetailsPage(selectedFile);
            }
        }

        private void NavigateToDetailsPage(StorageFile file)
        {
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(Tokens.fileToExtractToken, file);
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.ExtractedFilePageKey, Tokens.fileToExtractToken);
        }

        private void NavigateToAllDatFilesPage()
        {
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.AllDatFilesPage);
        }

        private void NavigateToAllAttachmentFilesPage()
        {
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.AllAttachmentsPage);
        }
    }
}
