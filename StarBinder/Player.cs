using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using PCLStorage;

namespace StarBinder
{
	/// <summary>
	/// Player
	/// </summary>
	public class Player
	{
		/// <summary>
		/// Player's name 
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Levels, got by player
		/// value = step count (0 - not finished; else - step count)
		/// </summary>
		public Dictionary<int, int> Levels = new Dictionary<int, int>();

		public Player (string name)
		{
			Name = name;
			InitLevels ();
		}

		private void InitLevels()
		{
			Levels = new Dictionary<int, int>();
			for (int i = 1; i <= GameManager.LevelsCount; i++)
				Levels.Add (i, 0);
		}

		public async void LoadPlayer()
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			try
			{
				IFile file = await rootFolder.GetFileAsync(Name + ".txt");
				string text = await file.ReadAllTextAsync();
				string[] lines = text.Split ('\n');
				// skip [0,1] lines
				for (int i = 0; i <= lines.Length; i++)
				{
					if (i < 2) continue;
					string part = lines[i].Substring(lines[i].IndexOf("="));
					Levels[i-1] = Convert.ToInt32(part);
				}
			}
			catch (Exception ex) 
			{
				Levels [1] = 1;
			}
		}

		public async void SavePlayer()
		{
			StringBuilder sb = new StringBuilder ();
			sb.AppendFormat ("Player={0}\n", Name);
			sb.AppendFormat("[Levels]\n");

			for (int i = 1; i <= GameManager.LevelsCount; i++)
				sb.AppendFormat ("Level{0}={1}", i, Levels [i]);
			
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			IFile file = await rootFolder.CreateFileAsync(Name + ".txt", CreationCollisionOption.ReplaceExisting);
			await file.WriteAllTextAsync(sb.ToString());
		}

		/// <summary>
		/// Alls the player stars count.
		/// </summary>
		/// <returns>The stars count.</returns>
		public int AllStarsCount()
		{
			return StarsCount (1, 30);
		}

		/// <summary>
		/// player stars count in range levels
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="start">Start level.</param>
		/// <param name="end">End level.</param>
		public int StarsCount(int start, int end)
		{
			int stars = 0;
			for (int i = start; i <= end; i++)
			{
				int steps = Levels [i]; 
				if (steps > 0) 
				{
					if (steps <= GameManager.Instance.GameLevels [i].StepsGold)
						stars += 3;
					else if (steps <= GameManager.Instance.GameLevels [i].StepsSilver)
						stars += 2;
					else
						stars += 1;
				}
			}

			return stars;
		}
	}
}

