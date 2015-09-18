using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Winmaildat;

namespace DatMailReader.Common
{
    public class ObjectSerializer
    {
        public static async Task<ObservableCollection<T>> Deserialize<T>(ObservableCollection<T> data, string fileName)
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.
                               GetFileAsync(fileName);
                if (file != null)
                {
                    using (IInputStream inStream = await file.OpenSequentialReadAsync())
                    {
                        DataContractJsonSerializer serializer =
                                new DataContractJsonSerializer(typeof(ObservableCollection<T>));
                        data = (ObservableCollection<T>)serializer
                                         .ReadObject(inStream.AsStreamForRead());
                    }
                }
            }
            catch { }
            return data;
        }

        public static async void Serialize<T>(ObservableCollection<T> data, string fileName)
        {
            MemoryStream sessionData = new MemoryStream();
            DataContractJsonSerializer serializer = new
                        DataContractJsonSerializer(typeof(ObservableCollection<T>));
            serializer.WriteObject(sessionData, data);
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                sessionData.Seek(0, SeekOrigin.Begin);
                await sessionData.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }
    }
}
