using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace DatMailReader.Common
{
    public class ObjectSerializer
    {
        public static async Task<ObservableCollection<T>> Deserialize<T>(ObservableCollection<T> data, string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                if (file != null)
                {
                    using (IInputStream inStream = await file.OpenSequentialReadAsync())
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<T>));
                        data = (ObservableCollection<T>)serializer.ReadObject(inStream.AsStreamForRead());
                    }
                }
            }
            catch { throw new Exception("Deserialization error"); }
            return data;
        }

        public static async void Serialize<T>(ObservableCollection<T> data, string fileName)
        {
            try
            {
                MemoryStream sessionData = new MemoryStream();
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ObservableCollection<T>));
                serializer.WriteObject(sessionData, data);
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                using (Stream fileStream = await file.OpenStreamForWriteAsync())
                {
                    sessionData.Seek(0, SeekOrigin.Begin);
                    await sessionData.CopyToAsync(fileStream);
                    await fileStream.FlushAsync();
                }
            }
            catch { throw new Exception("Serialization error"); }
        }
    }
}
