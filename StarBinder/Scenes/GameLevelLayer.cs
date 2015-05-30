using System;
using System.Collections.Generic;
using CocosSharp;

namespace StarBinder
{
	public class GameLevelLayer: CCLayerColor, IPageScene
	{
		// buttons
		private CCSprite _background;
		private CCSprite _backBtn;
		private CCSprite _optionsBtn;
		private CCSprite _helpBtn;
		private CCSprite _achievementsBtn;
		private CCSprite _restartBtn;

		// stars on screen 
		private Dictionary<int, CCSprite> _stars;
		// binds in screen
		private List<CCDrawingPrimitives> _binds;

		// current level in intial state 
		private Level _initLevel;
		// current level
		private Level _level;

		public GameLevelLayer() : base()
		{
			Color = CCColor3B.Black;
			Opacity = 255;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;
			_stars = new Dictionary<int, CCSprite> ();
			_binds = new List<CCDrawingPrimitives> ();

			_background = new CCSprite ("kosmos");
			_background.Position = VisibleBoundsWorldspace.Center;

			AddButtons ();
			AddListener ();

			AddChild (_background);
			AddChild (_backBtn);
			AddChild (_optionsBtn);
			AddChild (_helpBtn);
			AddChild (_achievementsBtn);

			InitLevel ();
		}

		private void AddButtons ()
		{
			_backBtn = new CCSprite ("back");
			CCRect rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.1f, 0.1f, 0.14f, 0.14f));
			_backBtn.Scale = rect.Size.Width / _backBtn.BoundingBox.Size.Width;
			_backBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_optionsBtn = new CCSprite ("options");
			rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.3f, 0.1f, 0.14f, 0.14f));
			_optionsBtn.Scale = rect.Size.Width / _optionsBtn.BoundingBox.Size.Width;
			_optionsBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_achievementsBtn = new CCSprite ("star");
			rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.5f, 0.1f, 0.14f, 0.14f));
			_achievementsBtn.Scale = rect.Size.Width / _achievementsBtn.BoundingBox.Size.Width;
			_achievementsBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_helpBtn = new CCSprite ("help");
			rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.7f, 0.1f, 0.14f, 0.14f));
			_helpBtn.Scale = rect.Size.Width / _helpBtn.BoundingBox.Size.Width;
			_helpBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_restartBtn = new CCSprite ("restart");
			rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.9f, 0.1f, 0.14f, 0.14f));
			_restartBtn.Scale = rect.Size.Width / _restartBtn.BoundingBox.Size.Width;
			_restartBtn.Position = new CCPoint (rect.MinX, rect.MinY);
		}

		private void AddListener ()
		{
			AddEventListener (new CCEventListenerTouchAllAtOnce () {
				OnTouchesEnded = (touches, ccevent) =>  {
					var location = touches [0].LocationOnScreen;
					location = WorldToScreenspace (location);

					if (_backBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.LEVEL_SELECT);
					else if (_optionsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.OPTIONS);
					else if (_achievementsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.ACHIEVEMENTS);
					else if (_helpBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.HELP);
					//else if (_restartBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
					//	NextScene (null);
				}
			});
		}

		private void InitLevel()
		{
			// load current level
			_initLevel = new Level (GameManager.Instance.CurrentLevel);
			_initLevel.InitLevel ();
			RefreshLevel ();
			DrawLevel ();
		}

		private void RefreshLevel()
		{
			_level = _initLevel.Clone();
		}

		private void DrawLevel()
		{
			// remove all
			foreach (var pair in _stars) 
			{
				RemoveChild (pair.Value);
			}

			// add stars
			foreach (var s in _level.Stars) 
			{
				CCSprite star = new CCSprite ("stars_0");
				CCRect rect = ScreenResolutionManager.Instance.GetRect (new CCRect (s.X, s.Y, 0.1f, 0.1f));
				star.Scale = rect.Size.Width / star.BoundingBox.Size.Width;
				star.Position = new CCPoint (rect.MinX, rect.MinY);

				AddChild (star);
			}
		}

		public void NextScene(Scenes? nextScene)
		{
			if (nextScene == null)
				Application.ExitGame ();

			GameManager.Instance.OldScene = Scenes.LEVEL;
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
			case Scenes.LEVEL_SELECT:
				Window.DefaultDirector.ReplaceScene (SelectLevelLayer.SelectLevelLayerScene (Window));
				break;
			}
		}

		public static CCScene GameLevelLayerScene (CCWindow mainWindow)
		{
			var scene = new CCScene (mainWindow);
			var layer = new GameLevelLayer ();

			scene.AddChild (layer);

			return scene;
		}
	}
}

