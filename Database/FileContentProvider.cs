using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Karma.Models;

namespace Karma.Database
{
    public class FileContentProvider : IFileContentProvider
    {
        public async Task<IEnumerable<T>> ReadFromFileAsync<T>()
        {
            List<T> list = new List<T>();
            string path = $"Database/{typeof(T).Name}.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                //TODO investigate why await takes so long (might be buffers)
                string jsonString = reader.ReadToEndAsync().Result;
                list.AddRange(JsonConvert.DeserializeObject<List<T>>(jsonString));
            }
            return list;
        }

        public async Task WriteToFileAsync<T>(IEnumerable<T> listToWrite)
        {
            string path = $"Database/{typeof(T).Name}.txt";
            string jsonString = JsonConvert.SerializeObject(listToWrite);
            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteAsync(jsonString);
            }
        }
    }
}
