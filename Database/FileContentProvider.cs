using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Karma.Server.Models;

namespace Karma.Server.Database
{
    public class FileContentProvider : IFileContentProvider
    {
        const string pathToFile = "Database/AllItems.txt";
        public async Task<IEnumerable<IGenericKarmaItem>> ReadFromFile ()
        {
            List<IGenericKarmaItem> list = new List<IGenericKarmaItem>();
            using (StreamReader reader = new StreamReader(pathToFile))
            {
                //TODO investigate why await takes so long (might be buffers)
                string jsonString = reader.ReadToEndAsync().Result;
                list.AddRange(JsonConvert.DeserializeObject<List<CharityEvent>>(jsonString));
            }
            return list;
        }

        public async Task WriteToFile (IEnumerable<IGenericKarmaItem> listToWrite)
        {
            string jsonString = JsonConvert.SerializeObject(listToWrite);
            using (StreamWriter writer = new StreamWriter(pathToFile))
            {
                await writer.WriteAsync(jsonString);
            }
        }
    }
}
