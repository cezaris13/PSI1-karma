using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Karma.Models;

namespace Karma.Database
{
    public class FileContentProvider : IFileContentProvider
    {
        const string PathToItemFile = "Database/AllItems.txt";
        const string PathToVolunteerFile = "Database/AllVolunteers.txt";
        public async Task<IEnumerable<IGenericKarmaItem>> ReadItemFromFileAsync()
        {
            List<IGenericKarmaItem> list = new List<IGenericKarmaItem>();
            using (StreamReader reader = new StreamReader(PathToItemFile))
            {
                //TODO investigate why await takes so long (might be buffers)
                string jsonString = reader.ReadToEndAsync().Result;
                list.AddRange(JsonConvert.DeserializeObject<List<CharityEvent>>(jsonString));
            }
            return list;
        }

        public async Task<IEnumerable<IVolunteerUser>> ReadVolunteerFromFileAsync()
        {
            List<IVolunteerUser> list = new List<IVolunteerUser>();
            using (StreamReader reader = new StreamReader(PathToVolunteerFile))
            {
                //TODO investigate why await takes so long (might be buffers)
                string jsonString = reader.ReadToEndAsync().Result;
                list.AddRange(JsonConvert.DeserializeObject<List<VolunteerUser>>(jsonString));
            }
            return list;
        }

        public async Task WriteItemToFileAsync(IEnumerable<IGenericKarmaItem> listToWrite)
        {
            string jsonString = JsonConvert.SerializeObject(listToWrite);
            using (StreamWriter writer = new StreamWriter(PathToItemFile))
            {
                await writer.WriteAsync(jsonString);
            }
        }

        public async Task WriteVolunteerToFileAsync(IEnumerable<IVolunteerUser> listToWrite)
        {
            string jsonString = JsonConvert.SerializeObject(listToWrite);
            using (StreamWriter writer = new StreamWriter(PathToVolunteerFile))
            {
                await writer.WriteAsync(jsonString);
            }
        }
    }
}
