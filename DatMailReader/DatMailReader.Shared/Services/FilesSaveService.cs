namespace DatMailReader.Services
{
    using DatMailReader.Models.Interfaces;
    using System;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Activation;
    using Windows.ApplicationModel.Core;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class FilesSaveService : IFilesSaveService
    {
        private readonly static Lazy<FilesSaveService> instance = new Lazy<FilesSaveService>(() => new FilesSaveService(), true);
        private TaskCompletionSource<int> completionSource;
        private StorageFolder storageFolder;

        public static FilesSaveService Instance
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
            FolderPickerContinuationEventArgs args = e as FolderPickerContinuationEventArgs;  
            if (args != null && args.Folder != null)  
            {  
                this.storageFolder = args.Folder;  
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
            FolderPicker savePicker = new FolderPicker();
            savePicker.FileTypeFilter.Add("*");
            savePicker.CommitButtonText = "Save All";
            Task task = null;

#if WINDOWS_PHONE_APP  

            this.completionSource = new TaskCompletionSource<int>();
            savePicker.PickFolderAndContinue();  
            task = this.completionSource.Task; 
 
#endif
#if WINDOWS_APP

            task = savePicker.PickSingleFolderAsync().AsTask().ContinueWith(
              fileTask =>
              {
                  this.storageFolder = fileTask.Result;
              });

#endif

            return task;
        }

        public StorageFolder CompleteOutstandingSelectionService()
        {
            var folder = this.storageFolder;
            this.storageFolder = null;
            this.completionSource = null;
            return folder;
        }
    }
}
