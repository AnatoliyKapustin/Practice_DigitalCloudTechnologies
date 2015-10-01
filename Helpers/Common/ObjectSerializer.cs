namespace DatMailReader.Helpers.Common
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Threading.Tasks;
    using Windows.Storage;
    using Windows.Storage.Streams;

    public class ObjectSerializer
    {
        public static async Task<T> Deserialize<T>(string fileName)
        {
            var result = default(T);
            try
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                if (file != null)
                {
                    using (IInputStream inStream = await file.OpenReadAsync())
                    {
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                        result = (T)serializer.ReadObject(inStream.AsStreamForRead());
                    }
                }
            }
            catch(FileNotFoundException) 
            { 
               /// throw new FileNotFoundException("Deserialization error", ex); 
            }

            return result;
        }

        public static async void Serialize<T>(T data, string fileName)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if (file != null)
                {
                    using (Stream fileStream = await file.OpenStreamForWriteAsync())
                    {
                        serializer.WriteObject(fileStream, data);
                    }
                }
            }
            catch(FileNotFoundException ex) 
            {
                throw new FileNotFoundException("Serialization error", ex);
            }
        }
    }
}
