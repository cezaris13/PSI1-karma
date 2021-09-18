using System.Collections.Generic;
using Karma.Server.Models;

namespace Karma.Server.File
{
    public interface IFileContentProvider
    {
        public List<Item> ReadFromFile ();

        public void WriteToFile (List<Item> listToWrite);
    }
}
