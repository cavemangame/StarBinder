using System;
using System.Collections.Generic;
using CocosSharp;
using StarBinder.Core;

namespace StarBinder
{
	public class SelectLevelLayer: CCLayerColor, IPageScene
	{
		private CCSprite _background;
		private CCSprite _backBtn;
		private CCSprite _optionsBtn;
		private CCSprite _helpBtn;
		private CCSprite _achievementsBtn;
		private CCLabel _label;

		private CCSprite _leftBtn;
		private CCSprite _rightBtn;
		private int _currentChapter = 1;

		private List<LevelNode> _levelNodes;

		public SelectLevelLayer () : base ()
		{
			Color = CCColor3B.Black;
			Opacity = 255;
		}
			
		protected override void AddedToScene ()
		{
			base.AddedToScene ();
			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			_levelNodes = new List<LevelNode> ();

			_background = new CCSprite ("kosmos");
			_background.Position = VisibleBoundsWorldspace.Center;

			AddButtons ();	
			AddListener ();

			string starText = String.Format ("Gained {0} stars!", GameManager.Instance.Player.AllStarsCount ());
			_label = new CCLabel (starText, "arial", 28) {
				PositionX = GameManager.Instance.Calculator.RelToAbsByMinSize(0.1),
				PositionY = GameManager.Instance.Calculator.RelToAbsByMaxSize(0.98),
				Color = CCColor3B.Green,
				AnchorPoint =  CCPoint.AnchorUpperLeft,
			};

			AddChild (_background);
			AddChild (_label);
			AddChild (_backBtn);
			AddChild (_optionsBtn);
			AddChild (_helpBtn);
			AddChild (_achievementsBtn);
			AddChild (_leftBtn);
			AddChild (_rightBtn);
			AddLevels ();
			UpdateArrowVisible ();
		}

		static void TestSavePlayer ()
		{
			GameManager.Instance.Player.Levels [1] = 1;
			GameManager.Instance.Player.SavePlayer ();
			GameManager.Instance.Player.Levels [1] = 0;
			GameManager.Instance.Player.LoadPlayer ();
		}

		private void AddButtons ()
		{
			_backBtn = new CCSprite ("back");
			CCRect rect = GetRect(0.2f, 0.1f, 0.14f, 0.14f);
			_backBtn.Scale = rect.Size.Width / _backBtn.BoundingBox.Size.Width;
			_backBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_optionsBtn = new CCSprite ("options");
			rect = GetRect(0.4f, 0.1f, 0.14f, 0.14f);
			_optionsBtn.Scale = rect.Size.Width / _optionsBtn.BoundingBox.Size.Width;
			_optionsBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_achievementsBtn = new CCSprite ("star");
			rect = GetRect(0.6f, 0.1f, 0.14f, 0.14f);
			_achievementsBtn.Scale = rect.Size.Width / _achievementsBtn.BoundingBox.Size.Width;
			_achievementsBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_helpBtn = new CCSprite ("help");
			rect = GetRect(0.8f, 0.1f, 0.14f, 0.14f);
			_helpBtn.Scale = rect.Size.Width / _helpBtn.BoundingBox.Size.Width;
			_helpBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_leftBtn = new CCSprite ("left");
			rect = GetRect(0.1f, 0.2f, 0.12f, 0.12f);
			_leftBtn.Scale = rect.Size.Width / _leftBtn.BoundingBox.Size.Width;
			_leftBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_rightBtn = new CCSprite ("right");
			rect = GetRect(0.9f, 0.2f, 0.12f, 0.12f);
			_rightBtn.Scale = rect.Size.Width / _rightBtn.BoundingBox.Size.Width;
			_rightBtn.Position = new CCPoint (rect.MinX, rect.MinY);
		}

		private void AddLevels()
		{
			foreach (var node in _levelNodes) 
			{
				RemoveChild(node);
			}
			_levelNodes.Clear ();

			int i = 0;
			foreach (Galaxy lvl in GameManager.Instance.GameLevels.Values)
			{
				if ((lvl.Number - 1) / 10 == _currentChapter - 1)
				{
					int x = i % 4;
					int y = i / 4;
					AddChapter (x, y, lvl.Number);
					i++;
				}
			}
		}

		private void AddChapter(int x, int y, int lvlNumber)
		{
			LevelNode node = new LevelNode (x, y, lvlNumber);
			_levelNodes.Add (node);
			AddChild(node);
		}

		private void UpdateArrowVisible()
		{
			_leftBtn.Visible = _currentChapter > 1;
			_rightBtn.Visible = _currentChapter < 3;
		}

		#region Listener and navigation

		private void AddListener ()
		{
			AddEventListener (new CCEventListenerTouchAllAtOnce () {
				OnTouchesEnded = (touches, ccevent) =>  {
					var location = touches [0].LocationOnScreen;
					location = WorldToScreenspace (location);

					if (_backBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.START_GAME);
					else if (_optionsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.OPTIONS);
					else if (_achievementsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.ACHIEVEMENTS);
					else if (_helpBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.HELP);
					else if (_leftBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
					{
						if (_currentChapter > 1) 
						{
							_currentChapter--;
							UpdateArrowVisible();
							AddLevels();
						}
					}
					else if (_rightBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
					{
						if (_currentChapter < 3) 
						{
							_currentChapter++;
							UpdateArrowVisible();
							AddLevels();
						}
					}
					else 
					{
						CheckTouchLevel(location);
					}
				}
			});
		}

		private void CheckTouchLevel(CCPoint location)
		{
			foreach (LevelNode level in _levelNodes) 
			{
				if (level.BoundingBoxTransformedToWorld.ContainsPoint (location)) 
				{
					GameManager.Instance.CurrentLevel = level.LevelNumber;
					NextScene (Scenes.LEVEL);
				}
			}
		}

		public void NextScene(Scenes? nextScene)
		{
			if (nextScene == null)
				Application.ExitGame ();

			GameManager.Instance.OldScene = Scenes.LEVEL_SELECT;
			switch (nextScene) 
			{
			case Scenes.ACHIEVEMENTS:
				Window.DefaultDirector.ReplaceScene (AchievementsLayer.AchievementsLayerScene (Window));
				break;
			case Scenes.HELP:
				Window.DefaultDirector.ReplaceScene (HelpLayer.HelpLayerScene (Window));
				break;
			case Scenes.OPTIONS:
				Window.DefaultDirector.ReplaceScene (OptionsLayer.OptionsLayerScene (Window));
				break;
			case Scenes.START_GAME:
				Window.DefaultDirector.ReplaceScene (GameStartLayer.GameStartLayerScene (Window));
				break;
			case Scenes.LEVEL:
				Window.DefaultDirector.ReplaceScene (GameLevelLayer.GameLevelLayerScene (Window));
				break;
			}
		}

		public static CCScene SelectLevelLayerScene (CCWindow mainWindow)
		{
			var scene = new CCScene (mainWindow);
			var layer = new SelectLevelLayer ();

			scene.AddChild (layer);

			return scene;
		}

		#endregion


		public CCRect GetRect(float x, float y, float width, float height)
		{
			return GameManager.Instance.Calculator.RectRelToAbsByMinSize (x, y, width, height).Convert (); 
		}
	}
}

