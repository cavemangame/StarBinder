using System;
using System.Collections.Generic;
using CocosSharp;
using StarBinder.Core;

namespace StarBinder
{
	public class GameStartLayer : CCLayerColor, IPageScene
	{
		private CCSprite _background;
		private CCSprite _logo;
		private CCSprite _levelsBtn;
		private CCSprite _optionsBtn;
		private CCSprite _helpBtn;
		private CCSprite _achievementsBtn;
		private CCSprite _exitBtn;

		public GameStartLayer () : base ()
		{
			Color = CCColor3B.Black;
			Opacity = 255;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			_background = new CCSprite ("kosmos1");
			_background.Position = VisibleBoundsWorldspace.Center;

			_logo = new CCSprite ("logo");
			_logo.Position = VisibleBoundsWorldspace.Center.Offset(0f, 200f);
			CCRect rect = Utils.GetRect (0.2f, 0.4f, 1f, 0.6f); 
			_logo.Scale = rect.Size.Width / _logo.BoundingBox.Size.Width;
		

			GameManager.Instance.Player.LoadPlayer ();
			AddButtons();
			AddListener ();

			AddChild (_background);
			AddChild (_levelsBtn);
			AddChild (_optionsBtn);
			AddChild (_helpBtn);
			AddChild (_achievementsBtn);
			AddChild (_exitBtn);
			AddChild (_logo);

		}

		private void AddButtons ()
		{
			_levelsBtn = new CCSprite ("levels");
			CCRect rect = Utils.GetRect(0.1f, 0.1f, 0.14f, 0.14f); 
			_levelsBtn.Scale = rect.Size.Width / _levelsBtn.BoundingBox.Size.Width;
			_levelsBtn.Position = new CCPoint (rect.MinX, rect.MinY);



			_optionsBtn = new CCSprite ("options");
			rect = Utils.GetRect(0.3f, 0.1f, 0.14f, 0.14f); 
			_optionsBtn.Scale = rect.Size.Width / _optionsBtn.BoundingBox.Size.Width;
			_optionsBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_achievementsBtn = new CCSprite ("star");
			rect = Utils.GetRect(0.5f, 0.1f, 0.14f, 0.14f);
			_achievementsBtn.Scale = rect.Size.Width / _achievementsBtn.BoundingBox.Size.Width;
			_achievementsBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_helpBtn = new CCSprite ("help");
			rect = Utils.GetRect(0.7f, 0.1f, 0.14f, 0.14f);
			_helpBtn.Scale = rect.Size.Width / _helpBtn.BoundingBox.Size.Width;
			_helpBtn.Position = new CCPoint (rect.MinX, rect.MinY);

			_exitBtn = new CCSprite ("close");
			rect = Utils.GetRect(0.9f, 0.1f, 0.14f, 0.14f);
			_exitBtn.Scale = rect.Size.Width / _exitBtn.BoundingBox.Size.Width;
			_exitBtn.Position = new CCPoint (rect.MinX, rect.MinY);
		}

		private void AddListener ()
		{
			AddEventListener (new CCEventListenerTouchAllAtOnce () {
				OnTouchesEnded = (touches, ccevent) =>  {
					var location = touches [0].LocationOnScreen;
					location = WorldToScreenspace (location);

					if (_levelsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.LEVEL_SELECT);
					else if (_optionsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.OPTIONS);
					else if (_achievementsBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.ACHIEVEMENTS);
					else if (_helpBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (Scenes.HELP);
					else if (_exitBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (null);
				}
			});
		}

		public void NextScene(Scenes? nextScene)
		{
			if (nextScene == null)
				Application.ExitGame ();
			
			GameManager.Instance.OldScene = Scenes.START_GAME;
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
			
		public static CCScene GameStartLayerScene (CCWindow mainWindow)
		{
			var scene = new CCScene (mainWindow);
			var layer = new GameStartLayer ();

			scene.AddChild (layer);
			return scene;
		}
	}
}