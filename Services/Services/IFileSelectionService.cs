namespace DatMailReader.Services.Shared
{
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public interface IFileSelectionService
    {
        void Initialise();
        Task LaunchFileSelectionServiceAsync();
        StorageFile CompleteOutstandingSelectionService();
    }
}
