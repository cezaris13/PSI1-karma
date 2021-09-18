using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Karma.Server.Models;

namespace Karma.Server.File
{
    public class FileContentProvider : IFileContentProvider
    {
        public List<Item> ReadFromFile ()
        {
            StreamReader reader = new StreamReader("File/AllItems.txt");
            string jsonString = reader.ReadToEnd();
            List<Item> readList = JsonSerializer.Deserialize<List<Item>>(jsonString);
            reader.Close();
            return readList;
        }

        public void WriteToFile (List<Item> listToWrite)
        {
            string jsonString = JsonSerializer.Serialize(listToWrite);
            StreamWriter writer = new StreamWriter("File/AllItems.txt");
            writer.Write(jsonString);
            writer.Close();
        }
    }
}
