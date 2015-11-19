namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Windows.Storage;

    public class AllExtractedAttachmentsViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<FileInfo> allExtractedAttachments = new List<FileInfo>();

        public AllExtractedAttachmentsViewModel()
        {
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>(this.OpenExtractedFileBeDefaultProgram);     
        }

        public RelayCommand<FileInfo> ExtractedItemClickCommand { get; private set; }

        public List<FileInfo> AllExtractedAttachments
        {
            get
            {
                return this.allExtractedAttachments;
            }

            set
            {
                this.allExtractedAttachments = value;
                base.RaisePropertyChanged("AllExtractedAttachments");
            }
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
    }
}
