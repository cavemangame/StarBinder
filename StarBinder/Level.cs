﻿using System;
using System.Collections.Generic;
using System.IO;
using PCLStorage;
using System.Linq;
using CocosSharp;

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

		public CCPoint GetPoint ()
		{
			return new CCPoint (X, Y);
		}

		public Star Clone()
		{
			return new Star (this.Number, this.State, this.X, this.Y);
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
			StarBind1 = 0;
			StarBind2 = 0;
		}

		public Bind Clone()
		{
			Bind bind = new Bind (this.StarNum1, this.StarNum2);
			bind.StarBind1 = this.StarBind1;
			bind.StarBind2 = this.StarBind2;
			return bind;
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

		/// <summary>
		/// Level's stars.
		/// </summary>
		/// <value>The stars.</value>
		public List<Star> Stars {get;set;}

		/// <summary>
		/// Level binds between stars
		/// </summary>
		/// <value>The binds.</value>
		public List<Bind> Binds { get; set; }

		/// <summary>
		/// Max number of star state
		/// </summary>
		/// <value>The state of the max.</value>
		public int MaxState { get; set; }

		/// <summary>
		/// Winning state of level (all stars should be in this state to win)
		/// </summary>
		/// <value>The state of the window.</value>
		public int WinState { get; set; }

		/// <summary>
		/// Solution path (star nums)
		/// </summary>
		/// <value>The solution path.</value>
		public string SolutionPath { get; set; }

		public Level (int number)
		{
			Number = number;
			Stars = new List<Star> ();
			Binds = new List<Bind> ();
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
				/*IFile file = await rootFolder.GetFileAsync(String.Format("/levels/level{0}.lvl", Number));
				string text = await file.ReadAllTextAsync();
				LoadLevel(text);*/
				LoadLevel(TestLevelInfo);
			}
			catch (Exception ex) 
			{
				
			}
		}

		private void LoadLevel(string text)
		{
			string[] lines = text.Split ('\n');
			// base level data
			this.Number = Convert.ToInt32 (RightValue(lines [0]));
			this.Name = String.Format ("{0}", RightValue(lines [1]));
			this.Description = String.Format ("{0}", RightValue(lines [2]));
			this.Steps1 = Convert.ToInt32 (RightValue(lines [3]));
			this.Steps2 = Convert.ToInt32 (RightValue(lines [4]));
			this.WinState = Convert.ToInt32 (RightValue(lines [5]));
			this.MaxState = Convert.ToInt32 (RightValue(lines [6]));

			// stars
			int idx = 8;
			Stars = new List<Star> ();
			while (idx < lines.Length) 
			{
				if (lines [idx].StartsWith("[BINDS]"))
					break;
				LoadStar (lines[idx]);
				idx++;
			}

			idx++;
			// binds
			Binds = new List<Bind>();
			while (idx < lines.Length) 
			{
				if (lines [idx] == "[SOLUTION]")
					break;
				LoadBind (lines[idx]);
				idx++;
			}
				
			GetSolution (lines [++idx]);
		}

		private string RightValue(string line)
		{
			return line.Substring (line.LastIndexOf ('=') + 1);
		}

		private void LoadStar(string text)
		{
			string[] strs = text.Split(';');
			if (strs.Length != 4)
				throw new ArgumentException ("Invalid data for the star!");
			Stars.Add (new Star (
				            Convert.ToInt32 (RightValue (strs [0])),
				            Convert.ToInt32 (RightValue (strs [3])),
				            Convert.ToSingle (RightValue (strs [1])),
							Convert.ToSingle (RightValue (strs [2]))));
		}

		private void LoadBind(string text)
		{
			string[] strs = text.Split(';');
			if (strs.Length != 4)
				throw new ArgumentException ("Invalid data for the bind!");
			Binds.Add (new Bind (
				Convert.ToInt32 (RightValue (strs [0])),
				Convert.ToInt32 (RightValue (strs [1]))) 
				{
					StarBind1 = Convert.ToInt32 (RightValue (strs [2])),
					StarBind2 = Convert.ToInt32 (RightValue (strs [2]))
				});
		}

		private void GetSolution(string text)
		{
			if (text != null)
				SolutionPath = RightValue (text);
		}


		private const string TestLevelInfo =
			@"LEVEL_NUM=1
NAME=THREE STARS
DESCRIPTION=Test level
STEPS1=1
STEPS2=3
WIN_STATE=0
MAX_STATE=1
[STARS]
NUM=1;X=0.4;Y=0.75;STATE=1
NUM=2;X=0.75;Y=0.5;STATE=1
NUM=3;X=0.25;Y=0.35;STATE=0
[BINDS]
NUM1=1;NUM2=2;BIND1=0;BIND2=0
NUM1=2;NUM2=3;BIND1=0;BIND2=0
[SOLUTION]=1";

		/// <summary>
		/// Gets the neighbors stars of current star.
		/// </summary>
		/// <returns>The neighbors.</returns>
		/// <param name="star">Star.</param>
		public List<Star> GetNeighbors(Star star)
		{
			int num = star.Number;
			List<Star> neighbors = new List<Star> ();
			foreach (Bind bind in Binds) 
			{
				if (bind.StarNum1 == star.Number)
					neighbors.Add (Stars.FirstOrDefault (s => s.Number == bind.StarNum2));
				if (bind.StarNum2 == star.Number)
					neighbors.Add (Stars.FirstOrDefault (s => s.Number == bind.StarNum1));
			}
			return neighbors;
		}

		public List<Star> GetBindStars(Bind bind)
		{
			List<Star> stars = new List<Star> ();
			foreach (Star star in Stars) 
			{
				if (star.Number == bind.StarNum1 || star.Number == bind.StarNum2)
					stars.Add (star);
			}
			return stars;
		}

		/// <summary>
		/// Set all neighbors of star and star itself to next state
		/// </summary>
		/// <param name="star">Star number.</param>
		public void NextState (int number)
		{
			Star star = GetStar (number);
			if (star == null)
				return;
			
			var neighbors = GetNeighbors (star);
			foreach (Star n in neighbors) 
			{
				if (n != null)
				{
					n.State++;
					if (n.State > MaxState)
						n.State = 0;
				}
			}
			star.State++;
			if (star.State > MaxState)
				star.State = 0;
		}

		public Star GetStar(int number)
		{
			foreach (Star star in Stars) 
			{
				if (star.Number == number)
					return star;
			}
			return null;
		}

		public bool IsWinLevel()
		{
			foreach (Star s in Stars)
			{
				if (s.State != WinState)
					return false;
			}
			return true;
		}

		public Level Clone()
		{
			Level level = new Level (this.Number);
			level.Chapter = this.Chapter;
			level.Name = String.Format("{0}", this.Name);
			level.Description =  String.Format("{0}", this.Description);
			level.MaxState = this.MaxState;
			level.Number = this.Number;
			level.Steps1 = this.Steps1;
			level.Steps2 = this.Steps2;
			level.WinState = this.WinState;

			level.Stars = new List<Star> ();
			foreach (Star star in this.Stars)
			{
				level.Stars.Add (star.Clone ());
			}

			level.Binds = new List<Bind> ();
			foreach (Bind bind in this.Binds)
			{
				level.Binds.Add (bind.Clone ());
			}

			return level;
		}
	}
}

