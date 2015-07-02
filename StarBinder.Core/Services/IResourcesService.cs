using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public interface IResourcesService
    {
        int LastChapterIndex { get; set; }
        IEnumerable<Chapter> AllChapters();
        void UpdateChapter(Chapter chapter);
        Task<string> GetLevelBack(Galaxy level);
    }
}
