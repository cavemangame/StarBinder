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

		
			CCRect rect1 = new CCRect (0.15f + _x * 0.235f, 0.75f - _y * 0.2f, 0.2f, 0.2f);
			if (_levelNum <= 8) rect1 = new CCRect (0.15f + _x * 0.235f, 0.75f - _y * 0.2f, 0.2f, 0.2f);
			else if (_levelNum <= 10) rect1 = new CCRect (0.15f + (_x + 1) * 0.235f, 0.72f - _y * 0.2f, 0.2f, 0.2f);

			CCRect r1 = ScreenResolutionManager.Instance.GetRect (rect1);
			// set background sprite
			_bgSprite = new CCSprite (levelName);
			_bgSprite.Scale = r1.Size.Width / _bgSprite.BoundingBox.Size.Width;

			// set bounding box for node
			Position = new CCPoint (r1.MinX, r1.MinY);
			Scale =  r1.Size.Width / _bgSprite.BoundingBox.Size.Width;

			ContentSize = r1.Size;

			// set player stars
			string starName = "stars_0";
			Player player = GameManager.Instance.Player;
			int steps = player.Levels [_levelNum];
			if (steps != 0) 
			{
				if (steps <= GameManager.Instance.GameLevels [_levelNum].Steps1)
					starName = "stars_3";
				else if (steps <= GameManager.Instance.GameLevels [_levelNum].Steps2)
					starName = "stars_2";
				else starName = "stars_1";
			}

			CCRect rect2 = new CCRect (0f, 0.062f, 0.175f, 0.03f);
			CCRect r2 = ScreenResolutionManager.Instance.GetRect (rect2);
			_starSprite = new CCSprite (starName);
			_starSprite.Scale = r2.Size.Width / _starSprite.BoundingBox.Size.Width;
			_starSprite.Position = new CCPoint (r2.MinX, r2.MinY);

			CCRect rect3 = new CCRect (0.004f, 0.004f, 0.15f, 0.15f);
			CCRect r3 = ScreenResolutionManager.Instance.GetRect (rect3);
			_lockSprite = new CCSprite ("lock");
			_lockSprite.Scale = r3.Size.Width / _lockSprite.BoundingBox.Size.Width;
			_lockSprite.Position = new CCPoint (r3.MinX, r3.MinY);
			_lockSprite.Visible = steps == 0;

			AddChild (_bgSprite);
			AddChild (_starSprite);
			AddChild (_lockSprite);
		}
	}
}

