using System;
using System.Collections.Generic;
using System.IO;
using PCLStorage;

namespace StarBinder
{
	/// <summary>
	/// Star on field
	/// </summary>
	public class Star
	{
		/// <summary>
		/// X position (in relative coords)
		/// </summary>
		/// <value>The x.</value>
		public float X { get; set; }

		/// <summary>
		/// Y position (in relative coords)
		/// </summary>
		/// <value>The y.</value>
		public float Y {get;set;}

		/// <summary>
		/// Current color of star
		/// </summary>
		/// <value>The state.</value>
		public int State { get; set; }

		/// <summary>
		/// Order number in the level
		/// </summary>
		/// <value>The number.</value>
		public int Number {get;set;}

		public Star(int number, int initState, float x, float y)
		{
			Number = number;
			State = initState;
			X = x;
			Y = y;
		}
	}

	/// <summary>
	/// Bind between two stars
	/// </summary>
	public class Bind 
	{
		/// <summary>
		/// Number of 1st star in bind
		/// </summary>
		/// <value>The star num1.</value>
		public int StarNum1 { get; set; }

		/// <summary>
		/// Number of 2nd star in bind
		/// </summary>
		/// <value>The star num2.</value>
		public int StarNum2 {get;set;}

		/// <summary>
		/// Type of starbind (0 - all, 1 - in, 2 -out)
		/// </summary>
		/// <value>The star bind1.</value>
		public int StarBind1 { get; set; }

		/// <summary>
		/// Type of starbind (0 - all, 1 - in, 2 -out)
		/// </summary>
		/// <value>The star bind2.</value>
		public int StarBind2 { get; set; }

		public Bind(int starNum1, int starNum2)
		{
			StarNum1 = starNum1;
			StarNum2 = starNum2;
		}
	}

	/// <summary>
	/// Level data in the game
	/// </summary>
	public class Level
	{
		/// <summary>
		/// Level order number (across all chapters)
		/// </summary>
		/// <value>The number.</value>
		public int Number { get; set; }

		/// <summary>
		/// Level name
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Level description
		/// </summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>
		/// Level's chapter
		/// </summary>
		/// <value>The chapter.</value>
		public int Chapter { get; set; }

		/// <summary>
		/// Count of steps for 3 stars solution
		/// </summary>
		/// <value>The steps1.</value>
		public int Steps1 { get; set; }

		/// <summary>
		/// Count of steps for 2 stars solution
		/// </summary>
		/// <value>The steps2.</value>
		public int Steps2 { get; set; }

		// stars and binds
		public List<Star> Stars {get;set;}

		public List<Bind> Binds { get; set; }

		public Level (int number)
		{
			Number = number;
			Chapter = GetChapter (Number);
		}

		public void InitLevel()
		{
			
			LoadFromFile ();
		}

		// publics
		private int GetChapter(int levelNum)
		{
			if (levelNum <= 10)
				return 1;
			if (levelNum <= 20)
				return 2;
			if (levelNum <= 30)
				return 3;
			return 4;
		}

		private async void LoadFromFile()
		{
			//InputStream input = Assets.Open ("my_asset.txt");
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			try
			{
				IFile file = await rootFolder.GetFileAsync(String.Format("/levels/level{0}.lvl", Number));
				string text = await file.ReadAllTextAsync();
				string[] lines = text.Split ('\n');
			
			}
			catch (Exception ex) 
			{
				//
			}
		}

		private const string TestLevelInfo =
@"LEVEL_NUM=1
NAME=THREE STARS
DESCRIPTION=Test level
STEPS1=1
STEPS2=3
WIN_STATE=0
[STARS]
NUM=1;X=0.3;Y=0.6;STATE=1
NUM=2;X=0.75;Y=0.4;STATE=1
NUM=3;X=0.25;Y=0.25;STATE=0
[BINDS]
NUM1=1;NUM2=2;BIND1=0;BIND2=0
NUM1=2;NUM2=3;BIND1=0;BIND2=0
[SOLUTION]=1";

	}
}

