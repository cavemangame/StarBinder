using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public interface IResourcesService
    {
        IEnumerable<Chapter> AllChapters();
        Task<string> GetLevelBack(Galaxy level);
    }
}
