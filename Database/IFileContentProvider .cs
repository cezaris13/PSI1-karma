using System.Collections.Generic;
using System.Threading.Tasks;

namespace Karma.Database
{
    public interface IFileContentProvider
    {
        Task<IEnumerable<T>> ReadFromFileAsync<T>();

        Task WriteToFileAsync<T>(IEnumerable<T> listToWrite);
    }
}
