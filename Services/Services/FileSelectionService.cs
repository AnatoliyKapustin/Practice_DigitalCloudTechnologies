namespace DatMailReader.Services.Shared
{
    using System;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Activation;
    using Windows.ApplicationModel.Core;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public class FileSelectionService : IFileSelectionService
    {
        public void Initialise()
        {
#if WINDOWS_PHONE_APP  
  
      CoreApplicationView view = CoreApplication.GetCurrentView();  
  
      view.Activated += (s, e) =>  
        {  
          FileOpenPickerContinuationEventArgs args = e as FileOpenPickerContinuationEventArgs;  
  
          if (args != null)  
          {  
            // assume there's one file here.  
            this.storageFile = args.Files[0];  
  
            if (this.completionSource != null)  
            {  
              this.completionSource.SetResult(0);  
            }  
          }  
        };  
 
#endif // WINDOWS_PHONE_APP
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
                  // NB: not checking success etc.  
                  this.storageFile = fileTask.Result;
              });

#endif // WINDOWS_PHONE_APP

            return (task);
        }

        public StorageFile CompleteOutstandingSelectionService()
        {
            var file = this.storageFile;
            this.storageFile = null;
            this.completionSource = null;
            return (file);
        }
        TaskCompletionSource<int> completionSource;
        StorageFile storageFile;
    }
}

