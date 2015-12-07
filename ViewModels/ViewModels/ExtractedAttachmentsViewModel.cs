namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Storage;

    public class ExtractedAttachmentsViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<FileInfo> allExtractedAttachments;

        public ExtractedAttachmentsViewModel()
        {
            this.ExtractedItemClickCommand = new RelayCommand<FileInfo>(this.OpenExtractedFileBeDefaultProgramExecute);     
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

        public async void Initialize()
        {
            this.AllExtractedAttachments = await LastExtractedFilesProvider.Instance.GetAttachmentFiles();
        }

        private async void OpenExtractedFileBeDefaultProgramExecute(FileInfo fileToOpen)
        { 
            if (fileToOpen.FilePath != null)
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
    }
}
