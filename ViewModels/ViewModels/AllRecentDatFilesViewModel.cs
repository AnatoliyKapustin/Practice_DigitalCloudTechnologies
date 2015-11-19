namespace DatMailReader.ViewModels.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Storage;

    public class AllRecentDatFilesViewModel : GalaSoft.MvvmLight.ViewModelBase
    {
        private List<StorageFile> allDatFiles = new List<StorageFile>();

        public AllRecentDatFilesViewModel()
        {
            this.RecentlyOpenedMailClickCommand = new RelayCommand<StorageFile>(this.NavigateToDetailsPage);
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

        private void NavigateToDetailsPage(StorageFile fileToPass)
        {
            ViewModelLocatorPCL.NavigationService.NavigateTo(ViewModelLocatorPCL.ExtractedFilePageKey, fileToPass);
        }
    }
}
