using System;
using System.Collections.Generic;
using System.Linq;

namespace StarBinder.Core
{
	public class ChapterData
	{
		public int Number;
		public string Name;
		public string Description;
		public StateData FinalState;
		public List<GalaxyData> Levels;
	    public int LastLevel;

		public ChapterData()
		{

		}

		public ChapterData(Chapter chapter)
		{
			Name = chapter.Name;
			Number = chapter.Number;
			Description = chapter.Description;
			FinalState = new StateData(chapter.FinalState);
			Levels = new List<GalaxyData>(chapter.Levels.Select(level => new GalaxyData(level)));
            LastLevel = chapter.LastLevelIndex;
		}
	}

	public class Chapter : ViewObject
	{
		public string Name { get; private set; }
		public string Description { get; private set; }
		public int Number { get; set; }
		public State FinalState { get; private set; }
		public List<Galaxy> Levels { get; private set; }
        public int LastLevelIndex { get; set; }

		Chapter()
		{
		}

        public Galaxy CurrentLevel { get { return Levels[LastLevelIndex]; } }

		public Chapter Clone()
		{
			return Create(new ChapterData(this));
		}

		public static Chapter CreateNew()
		{
			return new Chapter
			{
                LastLevelIndex = 0,
                Number = 1,
				FinalState = State.CreateInitial(),
				Levels = new List<Galaxy>(),
			};
		}

		public static Chapter Create(ChapterData data)
		{
		    var chapter = new Chapter
		    {
		        Name = data.Name,
		        Number = data.Number,
		        Description = data.Description,
		        LastLevelIndex = data.LastLevel,
		        Levels = new List<Galaxy>(data.Levels.Select(levelData => levelData.CreateGalaxy()))
		    };

		    return chapter;
		}
	}
}

