namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.DataAccess.Providers;
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class MainViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private RecentFilesProvider recentFiles = RecentFilesProvider.Instance;
        private LastExtractedFilesProvider recentAttachments = LastExtractedFilesProvider.Instance;
        private List<FileInfo> recentlyExtractedAtt = new List<FileInfo>();
        private List<StorageFile> recentlyEctractedFiles = new List<StorageFile>();

        public MainViewModel()
        {
            this.NavigateCommand = new RelayCommand(() => ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.SecondPageKey));
            this.OpenFileCommand = new RelayCommand(() => this.OpenFileExecute());
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>((item) => { this.OpenExtractedFileBeDefaultProgram(item); });
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>((item) => { this.NavigateToDetailsPage(item); });
        }

        public RelayCommand NavigateCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand<FileInfo> ExtractedItemClickCommand { get; private set; }
        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand { get; private set; }

        public async void Initialize()
        {
            this.RecentlyExtractedFiles = new List<StorageFile>(await this.recentFiles.GetRecentlyExtractedMails());
            this.RecentlyExtractedAtt = new List<FileInfo>(await this.recentAttachments.GetAttFiles());
        }

        public List<FileInfo> RecentlyExtractedAtt
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

        public List<StorageFile> RecentlyExtractedFiles
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
                    var temp = await StorageFile.GetFileFromPathAsync(FileToOpen.FilePath);
                    var options = new Windows.System.LauncherOptions();
                    await Windows.System.Launcher.LaunchFileAsync(temp);
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new FileNotFoundException("Can't open selected file", ex);
            }
        }

        /* private async void OpenFileExecute()
         { 
             await this.fileSelectionService.LaunchFileSelectionServiceAsync();
             StorageFile SelectedFile = this.fileSelectionService.CompleteOutstandingSelectionService();
             if (SelectedFile != null)
             {
                 recentFiles.AddDatFileToken(SelectedFile);
                 this.GivenMessage = await TnefToCollection(SelectedFile, this.givenMessage);
                 GetFilesFromMessage();
                 recentAttachments.AddAttachmentToRecentFiles(this.file);
                 NavigateToDetailsPage(this.GivenMessage);
             }
         }
         */
        private async void OpenFileExecute()
        {
            FileOpenPicker fileOpenPicker = new FileOpenPicker();
            fileOpenPicker.ViewMode = PickerViewMode.List;
            fileOpenPicker.FileTypeFilter.Add(".dat");
            StorageFile SelectedFile = await fileOpenPicker.PickSingleFileAsync();
            if (SelectedFile != null)
            {
                this.NavigateToDetailsPage(SelectedFile);
            }
        }

        private void NavigateToDetailsPage(StorageFile fileToPass)
        {
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.SecondPageKey, fileToPass);
        }
    }
}
