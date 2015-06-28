using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public interface IGameService
    {
        Task<Galaxy> GetCurrentLevel();

        Task<NextLevelInfo> GetNextLevelInfo();

        Task SaveState();
        
        Task<Galaxy> GoToLevel(int number);

        Task<Galaxy> GoToLevel(Galaxy level, Chapter chapter);

        Task<IEnumerable<Chapter>> GetAllChapters();

        Task<Chapter> GetCurrentChapter();
    }

    public class NextLevelInfo
    {
        private Func<Galaxy> next;

        public NextLevelInfo(bool isChapterComplete, bool hasNext, Func<Galaxy> next)
        {
            IsChapterComplete = isChapterComplete;
            HasNext = hasNext;
            this.next = next;
        }

        public bool IsChapterComplete { get; private set; }

        public bool HasNext { get; private set; }

        public Task<Galaxy> Next()
        {
            return Task.Run(next);
        }
    }
}
