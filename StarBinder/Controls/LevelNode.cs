using System;
using CocosSharp;

namespace StarBinder
{
	public class LevelNode : CCNode
	{
		private int _x, _y;

		private CCSprite _bgSprite;
		private CCSprite _starSprite;
		private CCSprite _lockSprite;
		private int _levelNum;

		public int LevelNumber {get { return _levelNum; }}

		public LevelNode (int x, int y, int levelNum)
		{
			_x = x;
			_y = y;
			_levelNum = levelNum;
			AddSprites ();
		}

		private void AddSprites()
		{
			// test 
			string levelName = "level_";
			if (_levelNum == 1)
				levelName += "1";
			else if (_levelNum == 2)
				levelName += "2";
			else
				levelName += "1";

		
			CCRect rect1 = GetRect(0.15f + _x * 0.235f, 1.05f - _y * 0.28f, 0.2f, 0.2f);
			if (_levelNum <= 8) rect1 = GetRect(0.15f + _x * 0.235f, 1.05f - _y * 0.28f, 0.2f, 0.2f);
			else if (_levelNum <= 10) rect1 = GetRect(0.15f + (_x + 1) * 0.235f, 1.0f - _y * 0.28f, 0.2f, 0.2f);

			// set background sprite
			_bgSprite = new CCSprite (levelName);
			_bgSprite.Scale = rect1.Size.Width / _bgSprite.BoundingBox.Size.Width;

			// set bounding box for node
			Position = new CCPoint (rect1.MinX, rect1.MinY);
			Scale =  rect1.Size.Width / _bgSprite.BoundingBox.Size.Width;

			ContentSize = rect1.Size;

			// set player stars
			string starName = "stars_0";
			Player player = GameManager.Instance.Player;
			int steps = player.Levels [_levelNum];
			if (steps != 0) 
			{
				if (steps <= GameManager.Instance.GameLevels [_levelNum].StepsGold)
					starName = "stars_3";
				else if (steps <= GameManager.Instance.GameLevels [_levelNum].StepsSilver)
					starName = "stars_2";
				else starName = "stars_1";
			}
				
			CCRect r2 = GetRect(0f, 0.09f, 0.175f, 0.03f);
			_starSprite = new CCSprite (starName);
			_starSprite.Scale = r2.Size.Width / _starSprite.BoundingBox.Size.Width;
			_starSprite.Position = new CCPoint (r2.MinX, r2.MinY);

			CCRect r3 = GetRect (0f, 0f, 0.15f, 0.15f);
			_lockSprite = new CCSprite ("lock");
			_lockSprite.Scale = r3.Size.Width / _lockSprite.BoundingBox.Size.Width;
			_lockSprite.Position = new CCPoint (r3.MinX, r3.MinY);
			_lockSprite.Visible = steps == 0;

			AddChild (_bgSprite);
			AddChild (_starSprite);
			AddChild (_lockSprite);
		}

		public CCRect GetRect(float x, float y, float width, float height)
		{
			return GameManager.Instance.Calculator.RectRelToAbsByMinSize (x, y, width, height).Convert (); 
		}
	}
}

