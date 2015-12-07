namespace DatMailReader.ViewModels.ViewModels
{
    using DatMailReader.Helpers.Providers;
    using DatMailReader.Models.Constants;
    using DatMailReader.Models.Model;
    using GalaSoft.MvvmLight.Command;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.AccessCache;

    public class RecentDatFilesViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<StorageFile> allDatFiles;

        public RecentDatFilesViewModel()
        {
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>(this.NavigateToDetailsPageExecute);
        }

        public RelayCommand<StorageFile> RecentlyOpenedMailClickCommand { get; private set; }

        public List<StorageFile> AllDatFiles
        {
            get
            {
                return this.allDatFiles;
            }

            set
            {
                this.allDatFiles = value;
                base.RaisePropertyChanged("AllDatFiles");
            }
        }

        public async Task Initialize()
        {
            this.AllDatFiles = await RecentFilesProvider.Instance.GetRecentlyExtractedMails();
        }

        private void NavigateToDetailsPageExecute(StorageFile fileToPass)
        {
            var parameter = new ExtractTextParameters();
            parameter.FileToExtractToken = Constants.FileToExtractToken;
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(Constants.FileToExtractToken, fileToPass);
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.ExtractedFilePageKey, parameter);
        }
    }
}
