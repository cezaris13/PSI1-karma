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
            StreamReader Reader = new StreamReader("File/AllItems.txt");
            string JsonString = Reader.ReadToEnd();
            List<Item> ReadList = JsonSerializer.Deserialize<List<Item>>(JsonString);
            Reader.Close();
            return ReadList;
        }

        public void WriteToFile (List<Item> ListToWrite)
        {
            string JsonString = JsonSerializer.Serialize(ListToWrite);
            StreamWriter Writer = new StreamWriter("File/AllItems.txt");
            Writer.Write(JsonString)
            Writer.Close();
        }
    }
}
