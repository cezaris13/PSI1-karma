using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Karma.Database
{
    //Leaving this code in case it's needed for 1st presentation. Will be deleted afterwards
    public class FileContentProvider //: IFileContentProvider
    {
        /*
        public async Task<IEnumerable<T>> ReadFromFileAsync<T>()
        {
            var list = new List<T>();
            var path = $"Database/{typeof(T).Name}.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                //TODO investigate why await takes so long (might be buffers)
                var jsonString = reader.ReadToEndAsync().Result;
                list.AddRange(JsonConvert.DeserializeObject<List<T>>(jsonString));
            }
            return list;
        }

        public async Task WriteToFileAsync<T>(IEnumerable<T> listToWrite)
        {
            var path = $"Database/{typeof(T).Name}.txt";
            var jsonString = JsonConvert.SerializeObject(listToWrite);
            using (StreamWriter writer = new StreamWriter(path))
            {
                await writer.WriteAsync(jsonString);
            }
        }*/
    }
}
