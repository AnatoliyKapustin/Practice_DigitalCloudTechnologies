namespace DatMailReader.Models.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Storage;

    public interface IFilesSaveService
    {
        void Initialize();
        Task LaunchFileSelectionServiceAsync();
        StorageFolder CompleteOutstandingSelectionService();
    }
}
