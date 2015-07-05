using System;
using System.Collections.Generic;
using System.Text;

namespace StarBinder.Core
{
	/// <summary>
	/// Player
	/// </summary>
	public class Player
	{
		/// <summary>
		/// имя игрока
		/// </summary>
		public string Name { get; set; }

        /// <summary>
        /// докуда игрок дошел, глава
        /// </summary>
        public int LastChapterIndex { get; set; }
        
        /// <summary>
        ///  докуда игрок дошел, уровень в главе
        /// </summary>
        public int LastLevelIndex { get; set; }
        
        /// <summary>
        /// последний левел, в который игрок играл
        /// </summary>
        public int CurrentLevelIndex { get; set; }
        
        /// <summary>
        /// последняя глава, в которой игрок был
        /// </summary>
        public int CurrentChapterIndex { get; set; }

		/// <summary>
		/// уровни, в которые игрок может поиграть и их состояние
		/// value = мин. число шагов, 0 - если не пройдено
		/// </summary>
        public Dictionary<int, Dictionary<int, int>> Levels = new Dictionary<int, Dictionary<int, int>>();

		public Player (string name)
		{
			Name = name;
		}

	    public void Save()
	    {
	        // TODO: save state in pcl
	    }

	    public void Load()
	    {
	        // TODO: load state from pcl
	    }

        /// <summary>
        /// Обновить достижения (шаги) игрока в уровне.
        /// Вызывать после окончания (удачного) уровня.
        /// </summary>
        /// <param name="steps">число шагов</param>
	    public void UpdateSteps(int steps)
	    {
	        if (!Levels.ContainsKey(CurrentChapterIndex))
	        {
                Levels.Add(CurrentChapterIndex, new Dictionary<int, int>());
	        }
            if (!Levels[CurrentChapterIndex].ContainsKey(CurrentLevelIndex))
	        {
                Levels[CurrentChapterIndex].Add(CurrentLevelIndex, 0);
	        }

            if (Levels[CurrentChapterIndex][CurrentLevelIndex] == 0)
	        {
                Levels[CurrentChapterIndex][CurrentLevelIndex] = steps;
	        }
            else if (Levels[CurrentChapterIndex][CurrentLevelIndex] > steps)
            {
                Levels[CurrentChapterIndex][CurrentLevelIndex] = steps;
            }
            Save();
	    }
	}
}

