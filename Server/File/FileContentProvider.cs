using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Karma.Server.Models;
using Newtonsoft.Json;

namespace Karma.Server.File
{
    public class FileContentProvider : IFileContentProvider
    {
        public async Task<List<Item>> ReadFromFile ()
        {
            StreamReader reader = new StreamReader("File/AllItems.txt");
            string jsonString = await reader.ReadToEndAsync();
            List<Item> readList = JsonConvert.DeserializeObject<List<Item>>(jsonString);
            reader.Close();
            return readList;
        }

        public async Task WriteToFile (List<Item> listToWrite)
        {
            string jsonString = JsonConvert.SerializeObject(listToWrite);
            StreamWriter writer = new StreamWriter("File/AllItems.txt");
            await writer.WriteAsync(jsonString);
            writer.Close();
        }
    }
}
