namespace DatMailReader.Shared.Services
{
    using DatMailReader.Models.Interfaces;
    using System;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class FileSelectionService : IFileSelectionService
    {
        private TaskCompletionSource<int> completionSource;
        private StorageFile storageFile;
        private readonly static Lazy<FileSelectionService> instance = new Lazy<FileSelectionService>(() => new FileSelectionService(), true);
        public static FileSelectionService Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public void Initialize()
        {
#if WINDOWS_PHONE_APP  
  
        CoreApplicationView view = CoreApplication.GetCurrentView();  
        view.Activated += (s, e) =>  
        {  
            FileOpenPickerContinuationEventArgs args = e as FileOpenPickerContinuationEventArgs;  
            if (args != null && args.Files.Count != 0)  
            {  
                this.storageFile = args.Files[0];  
                if (this.completionSource != null)  
                {  
                    this.completionSource.SetResult(0);  
                }  
            }  
         };  
 
#endif
        }

        public Task LaunchFileSelectionServiceAsync()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".dat");
            Task task = null;

#if WINDOWS_PHONE_APP  

            this.completionSource = new TaskCompletionSource<int>();  
            picker.PickSingleFileAndContinue();  
            task = this.completionSource.Task; 
 
#endif
#if WINDOWS_APP

            task = picker.PickSingleFileAsync().AsTask().ContinueWith(
              fileTask =>
              {
                  this.storageFile = fileTask.Result;
              });

#endif

            return task;
        }

        public StorageFile CompleteOutstandingSelectionService()
        {
            var file = this.storageFile;
            this.storageFile = null;
            this.completionSource = null;
            return file;
        }
    }
}

