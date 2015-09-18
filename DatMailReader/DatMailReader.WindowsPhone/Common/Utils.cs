using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.Storage.Pickers;

/*namespace DatMailReader.Common
{
    internal class Utils
    {

        public static StorageFolder pickFolder()
        {
            FolderPicker folderPicker = new FolderPicker();
            //folderPicker.SuggestedStartLocation(1);
            folderPicker.FileTypeFilter.Add(".dat");
            return (StorageFolder)null;
        }

        public static void copyToPickedFolder(List<IStorageItem> files)
        {
            Utils.copySelectedToFolder(Utils.pickFolder(), files);
        }

        public static void copySelectedToFolder(StorageFolder targetFolder, List<IStorageItem> files)
        {
            if (targetFolder == null)
                return;
            using (List<IStorageItem>.Enumerator enumerator = files.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    (enumerator.Current as StorageFile).CopyAsync(targetFolder);
            }
        }

        internal static string getResultMessageForCount(int resultCount)
        {
            ResourceLoader resourceLoader = new ResourceLoader();
            if (resultCount == -1)
                return resourceLoader.GetString("ResultCouldNotOpen");
            if (resultCount == 0)
                return resourceLoader.GetString("ResultNoFilesFound");
            return (string)(object)resultCount + (object)" " + resourceLoader.GetString("ResultFilesFound");
        }
    }
}
*/