using System.Collections.Generic;

namespace StarBinder.Core.Services
{
    public interface IResourcesService
    {
        int LastChapterIndex { get; set; }
        IEnumerable<Chapter> AllChapters();
        void UpdateChapter(Chapter chapter);
        string GetLevelBack(Galaxy level);
    }
}
