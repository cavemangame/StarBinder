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
	    public string BackPath;
		public StateData FinalState;
		public List<GalaxyData> Levels;
        public List<GalaxyData> AdditionalLevels;

		public ChapterData()
		{

		}

		public ChapterData(Chapter chapter)
		{
			Name = chapter.Name;
			Number = chapter.Number;
			Description = chapter.Description;
		    BackPath = chapter.BackPath;
			FinalState = new StateData(chapter.FinalState);
			Levels = new List<GalaxyData>(chapter.Levels.Select(level => new GalaxyData(level)));
            Levels = new List<GalaxyData>(chapter.AdditionalLevels.Select(level => new GalaxyData(level)));
		}
	}

	public class Chapter : ViewObject
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int Number { get; set; }
        public string BackPath { get; set; }
		public State FinalState { get; set; }
		public List<Galaxy> Levels { get; set; }
        public List<Galaxy> AdditionalLevels { get; set; }

		Chapter()
		{
		}

		public Chapter Clone()
		{
			return Create(new ChapterData(this));
		}

		public static Chapter CreateNew()
		{
			return new Chapter
			{
                Number = 1,
                BackPath = "",
				FinalState = State.CreateInitial(),
				Levels = new List<Galaxy>(),
                AdditionalLevels = new List<Galaxy>()
			};
		}

		public static Chapter Create(ChapterData data)
		{
		    var chapter = new Chapter
		    {
		        Name = data.Name,
		        Number = data.Number,
		        Description = data.Description,
                BackPath = data.BackPath,
		        Levels = new List<Galaxy>(data.Levels.Select(levelData => levelData.CreateGalaxy())),
                //AdditionalLevels = new List<Galaxy>(data.AdditionalLevels.Select(levelData => levelData.CreateGalaxy()))
		    };

		    return chapter;
		}
	}
}

