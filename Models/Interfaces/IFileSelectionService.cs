namespace DatMailReader.Models.Interfaces
{
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Pickers;

    public interface IFileSelectionService
    {
        void Initialize();
        Task LaunchFileSelectionServiceAsync();
        StorageFile CompleteOutstandingSelectionService();
    }
}
