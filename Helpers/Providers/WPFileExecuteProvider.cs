namespace DatMailReader.Helpers.Providers
{
    using DatMailReader.Helpers.Services;
    using DatMailReader.Helpers.Common;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Activation;
    using Windows.ApplicationModel.Core;
    using Windows.Storage;
    using Windows.Storage.AccessCache;
    using Windows.Storage.Pickers;

    public class WPStorageFileExecuteProvider : IFileSelectionService
    {
        /*
         public void Initialise()
         {
             // Feels a bit wrong to call into CoreApplication here without an
             // abstraction but for demo purposes...

             // This event can fire either if the app keeps running after a
             // file selection or if the app is killed/re-launched after a
             // file selection.
             CoreApplication.GetCurrentView().Activated += OnApplicationActivated;
         }
         void OnApplicationActivated(CoreApplicationView sender,
           IActivatedEventArgs args)
         {
             FileOpenPickerContinuationEventArgs continueArgs =
               args as FileOpenPickerContinuationEventArgs;

             if (continueArgs != null)
             {
                 // assumes we have one file here at least.
                 this.selectedFile = continueArgs.Files[0];

                 if (this.completionSource != null)
                 {
                     this.completionSource.SetResult(this.selectedFile);

                     this.completionSource = null;
                 }
             }
         }
         public async Task<StorageFile> DisplayPickerAsync(FileOpenPicker picker)
         {
             this.completionSource = new TaskCompletionSource<StorageFile>();

             picker.PickSingleFileAndContinue();

             StorageFile file = await this.completionSource.Task;

             return (file);
         }
         public StorageFile GetAndClearLastPickedFile()
         {
             StorageFile file = this.selectedFile;

             this.selectedFile = null;

             return (file);
         }
         TaskCompletionSource<StorageFile> completionSource;
         StorageFile selectedFile;
     }*/
}
