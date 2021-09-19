using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Karma.Database;
using Karma.Models;

namespace Karma.Data
{
    public class DataModificator : IDataModificator
    {
        private readonly IFileContentProvider m_fileContentProvider;
        public DataModificator (
            IFileContentProvider fileContentprovider)
        {
            m_fileContentProvider = fileContentprovider;
        }

        public async Task AddCharityEvent (IGenericKarmaItem newCharityEvent)
        {
            List<IGenericKarmaItem> charityEventList = (await m_fileContentProvider.ReadFromFile()).ToList();
            charityEventList.Add(newCharityEvent);
            await m_fileContentProvider.WriteToFile(charityEventList);
        }

        public async Task RemoveCharityEvent (int charityEventId)
        {
            List<IGenericKarmaItem> charityEventList = (await m_fileContentProvider.ReadFromFile()).ToList();
            IEnumerable<IGenericKarmaItem> newCharityList = charityEventList.Where(p => p.Id != charityEventId);
            await m_fileContentProvider.WriteToFile(newCharityList);
        }

        public async Task UpdateCharityEvent (IGenericKarmaItem updatedItem)
        {
            List<IGenericKarmaItem> charityEventList = (await m_fileContentProvider.ReadFromFile()).ToList();
            int updatedElementId = charityEventList.FindIndex(p => p.Id != updatedItem.Id);
            if (updatedElementId != -1)
            {
                charityEventList[updatedElementId] = updatedItem;
                await m_fileContentProvider.WriteToFile(charityEventList);
            }
            else
            {
                throw(new IndexOutOfRangeException($"Item with given id :{updatedItem.Id} was not found")); 
            }
        }
    }
}
