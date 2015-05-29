using System;
using CocosSharp;

namespace StarBinder
{
	public class PlayerName : CCNode
	{
		private float _x, _y;

		private CCLabel _label;
		private CCTextFieldTTF _playerText;
		  
		public PlayerName (float x, float y)
		{
			_x = x;
			_y = y;
			AddSprites ();
		}

		private void AddSprites()
		{
			_label = new CCLabel ("Enter your name:", "arial", 28) {
				PositionX = _x,
				PositionY = _y + 40,
				Color = CCColor3B.DarkGray,
				AnchorPoint =  CCPoint.AnchorUpperLeft,
			};

			_playerText = new CCTextFieldTTF ("Player", "arial", 28);

			_playerText.PositionX = _x;
			_playerText.PositionY = _y;
			_playerText.Color = CCColor3B.Green;
			_playerText.AnchorPoint = CCPoint.AnchorUpperLeft;
			_playerText.Text = "Player";

			AddChild (_label);
			AddChild (_playerText);
		}
	}
}

