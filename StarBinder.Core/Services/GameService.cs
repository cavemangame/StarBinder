using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public class GameService : IGameService
    {
        private readonly Player player;

        private readonly Lazy<List<Chapter>> chapters;

    
        public GameService(IResourcesService resources, Player player)
        {
            Debug.WriteLine("GameService ctor");
            this.player = player;
            chapters = new Lazy<List<Chapter>>(() => resources.AllChapters().ToList());
        }

        private Galaxy CurrentLevel {
            get
            {
                return CurrentChapter.Levels.Count() >= player.CurrentLevelIndex
                    ? CurrentChapter.Levels[player.CurrentLevelIndex]
                    : CurrentChapter.AdditionalLevels[player.CurrentLevelIndex - CurrentChapter.Levels.Count() - 1];
            } 
        }

        private Galaxy LastLevel
        {
            get
            {
                return LastChapter.Levels.Count() >= player.LastChapterIndex
                    ? LastChapter.Levels[player.LastChapterIndex]
                    : LastChapter.AdditionalLevels[player.LastChapterIndex - LastChapter.Levels.Count() - 1];
            }
        }
        private Chapter CurrentChapter { get { return Chapters[player.CurrentChapterIndex]; } }
        private Chapter LastChapter { get { return Chapters[player.LastChapterIndex]; } }



        private List<Chapter> Chapters { get { return chapters.Value; } }

        public Player Player { get { return player; } }

        public Task<Galaxy> GetCurrentLevel()
        {
            return Task.Run(() => CurrentLevel);
        }

        public Task<Galaxy> GetLastLevel()
        {
            return Task.Run(() => LastLevel);
        }


		public Task<NextLevelInfo> GetNextLevelInfo()
		{
		/*	return Task.Run(() =>
			{
				var isComplete = CurrentLevelIndex == CurrentChapter.Levels.Count + CurrentChapter.AdditionalLevels.Count - 1; // is chapter complete?
				var hasNext = !isComplete || LastChapterIndex < Chapters.Count - 1;
				Func<Galaxy> next = () => 
				{
					if (!hasNext)
					{
						LastChapterIndex = 0;
						LastLevelIndex = 0;
					}
					else if (isComplete)
					{
						LastChapterIndex++;
						CurrentChapter.LastLevelIndex = 0;
					}
					else
					{
						CurrentChapter.LastLevelIndex++;
					}

					return CurrentLevel;
				};

				return new NextLevelInfo(isComplete, hasNext, next);
			});*/
		    return null;
		}

        public Task SaveState(int steps)
        {
            return Task.Run(() => player.UpdateSteps(steps));
        }

        public Task<Galaxy> GoToLevel(int number)
        {
            if (number < 1 || number > CurrentChapter.Levels.Count + CurrentChapter.AdditionalLevels.Count)
                throw new ArgumentOutOfRangeException("number");

            return Task.Run(() =>
            {
                player.CurrentLevelIndex = number - 1;
                return CurrentLevel;
            });
        }

        public Task<Galaxy> GoToLevel(Chapter chapter, int number)
		{
			return Task.Run(() => 
			{
                player.CurrentChapterIndex = Chapters.IndexOf(chapter);
                player.CurrentLevelIndex = number;
				return CurrentLevel;
			});
		}

        public Task<IEnumerable<Chapter>> GetAllChapters()
        {
            return Task.Run(() => Chapters.AsEnumerable());
        }

		public Task<Chapter> GetCurrentChapter()
		{
			return Task.Run(() => CurrentChapter);
		}
    }
}
