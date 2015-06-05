using System;
using System.Collections.Generic;
using CocosSharp;
using StarBinder.Core;

namespace StarBinder
{
	public class GameManager
	{
		// Singleton standart implementation
		private static GameManager instance;
		public static GameManager Instance
		{
			get { return instance ?? (instance = new GameManager()); }
		}

		protected GameManager () 
		{
			GameLevels = new Dictionary<int, Galaxy> ();
			InitLevels ();
			InitPlayer ();
		}

		public const int LevelsCount = 30;

		// old scene (for return)
		public Scenes OldScene {get; set;}

		// player
		public Player Player { get; set; }

		// all game levels (lazy loading)
		public Dictionary<int, Galaxy> GameLevels {get; private set;}

		public int CurrentLevel { get; set; }

		private void InitLevels()
		{
			for (int i = 1; i <= LevelsCount; i++) 
			{
				Galaxy g = Galaxy.CreateNew ();
				g.Number = i;
				GameLevels.Add (i, g);
			}
			/*GameLevels [1].Steps1 = 1;
			GameLevels [1].Steps2 = 2;
	

			GameLevels [2].Steps1 = 2;
			GameLevels [2].Steps2 = 4;
		

			GameLevels [3].Steps1 = 3;
			GameLevels [3].Steps2 = 7;
		*/
		}

		private void InitPlayer()
		{
			Player = new Player ("Player1");
		}

		private bool HasPlayer()
		{
			return false;
		}
	}
}
