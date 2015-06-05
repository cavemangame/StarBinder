using System;
using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using StarBinder.Core;

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
		private Dictionary<int, CCDrawNode> _stars;
		// binds in screen
		private List<CCDrawNode> _binds;

		// current level in intial state 
		private Galaxy _initLevel;
		// current level
		private Galaxy _level;

		public GameLevelLayer() : base()
		{
			Color = CCColor3B.Black;
			Opacity = 255;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;
			_stars = new Dictionary<int, CCDrawNode> ();
			_binds = new List<CCDrawNode> ();

			_background = new CCSprite ("kosmos");
			_background.Position = VisibleBoundsWorldspace.Center;

			AddButtons ();
			AddListener ();

			AddChild (_background);
			AddChild (_backBtn);
			AddChild (_optionsBtn);
			AddChild (_helpBtn);
			AddChild (_achievementsBtn);
			AddChild (_restartBtn);

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

			_restartBtn = new CCSprite ("refresh");
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
					else if (_restartBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (null);
					else CheckTouchGame(location);
				}
			});
		}

		private void CheckTouchGame(CCPoint location)
		{
			foreach (var pair in _stars) 
			{
				if (pair.Value.BoundingBoxTransformedToWorld.ContainsPoint (location)) 
				{
					_level.NextState (pair.Key);
					DrawLevel ();
					break;
				}
			}
		}

		string json = "{\"Name\":null,\"Description\":null,\"Number\":0,\"StepsSilver\":0,\"StepsGold\":0,\"BestSolve\":[],\"States\":[{\"Id\":\"691f3970-1c9f-45a9-8217-d8687f841424\",\"Color\":\"#FFF0FFF0\"},{\"Id\":\"d36569aa-cc0d-4b61-8e0d-66a080236d48\",\"Color\":\"#FF808080\"}],\"Stars\":[{\"StateId\":\"691f3970-1c9f-45a9-8217-d8687f841424\",\"FinalStateId\":\"d36569aa-cc0d-4b61-8e0d-66a080236d48\",\"InitialStateId\":\"691f3970-1c9f-45a9-8217-d8687f841424\",\"RotateAngle\":30.0,\"SubBeamsCoeff\":1.0,\"InnerCoeff\":0.3,\"IsSubBeams\":false,\"Beams\":5,\"HalfWidthRel\":0.1,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.2925,\"YRel\":0.24833333333333332,\"Number\":0},{\"StateId\":\"d36569aa-cc0d-4b61-8e0d-66a080236d48\",\"FinalStateId\":\"d36569aa-cc0d-4b61-8e0d-66a080236d48\",\"InitialStateId\":\"d36569aa-cc0d-4b61-8e0d-66a080236d48\",\"RotateAngle\":15.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":true,\"Beams\":4,\"HalfWidthRel\":0.1,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.46,\"YRel\":0.59166666666666667,\"Number\":1},{\"StateId\":\"691f3970-1c9f-45a9-8217-d8687f841424\",\"FinalStateId\":\"d36569aa-cc0d-4b61-8e0d-66a080236d48\",\"InitialStateId\":\"691f3970-1c9f-45a9-8217-d8687f841424\",\"RotateAngle\":15.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":false,\"Beams\":6,\"HalfWidthRel\":0.12,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.7325,\"YRel\":0.27333333333333332,\"Number\":2}],\"Links\":[{\"From\":2,\"To\":1,\"Direction\":0},{\"From\":2,\"To\":0,\"Direction\":0}]}";
		private void InitLevel()
		{
			// load current level
			_initLevel = SerializationHelper.GalaxyFromJson(json);
			RefreshLevel ();
			DrawLevel ();
		}

		private void RefreshLevel()
		{
			//_level = Galaxy.Create(_initLevel.;
		}

		private void DrawLevel()
		{
			// remove all
			foreach (var pair in _stars) 
			{
				RemoveChild (pair.Value);
			}
			foreach (var path in _binds) 
			{
				RemoveChild (path);
			}
			_stars = new Dictionary<int, CCDrawNode> ();
			_binds = new List<CCDrawNode> ();

			var r = new System.Random ();
			// add stars
			foreach (var s in _level.Stars) 
			{
				CCRect rect = ScreenResolutionManager.Instance.GetRect (new CCRect (s.X, s.Y, 0.1f, 0.1f));
				CCDrawNode pStar = new CCDrawNode ();
				float k = 3;
				List<CCPoint> points = new List<CCPoint> ();
				points.Add(new CCPoint (rect.MinX + 22 * k, rect.MinY + 12 * k));
				points.Add(new CCPoint (rect.MinX + 36 * k, rect.MinY + 12 * k));
				points.Add(new CCPoint (rect.MinX + 24 * k, rect.MinY + 22 * k));
				points.Add(new CCPoint (rect.MinX + 28 * k, rect.MinY + 34 * k));
				points.Add(new CCPoint (rect.MinX + 18 * k, rect.MinY + 24 * k));
				points.Add(new CCPoint (rect.MinX + 8 * k, rect.MinY + 34 * k));
				points.Add(new CCPoint (rect.MinX + 12 * k, rect.MinY + 22 * k));
				points.Add(new CCPoint (rect.MinX + 0 * k, rect.MinY + 12 * k));
				points.Add(new CCPoint (rect.MinX + 14 * k, rect.MinY + 12 * k));
				points.Add(new CCPoint (rect.MinX + 18 * k, rect.MinY + 0 * k));

				pStar.DrawPolygon (points.ToArray(), points.Count(), CCColor4B.Green, 2, CCColor4B.Gray);
				_stars.Add (s.Number, pStar);
	
				foreach (Bind bind in _level.Binds)
				{
					//M 18,0 L 14,12 L 0,12 L 12,22 L 8,34 L 18,24 L 28,34 L 24,22 L 36,12 L 22,12 L 18,0

					var stars = _level.GetBindStars (bind);
					CCDrawNode pBind = new CCDrawNode ();

					CCRect rect1 = ScreenResolutionManager.Instance.GetRect (new CCRect (stars.First ().X, stars.First ().Y, 0.1f, 0.1f));
					CCRect rect2 = ScreenResolutionManager.Instance.GetRect (new CCRect (stars.Last ().X, stars.Last ().Y, 0.1f, 0.1f));

					pBind.DrawLine (new CCPoint(rect1.MinX, rect1.MinY), new CCPoint(rect2.MinX, rect2.MinY), 3.0f, CCColor4B.Green);
					_binds.Add (pBind);
				}
			}

			foreach (var path in _binds) 
			{
				AddChild (path);
			}
			foreach (var pair in _stars) 
			{
				AddChild (pair.Value);
			}

		}

		public void NextScene(Scenes? nextScene)
		{
			if (nextScene == null) 
			{
				RefreshLevel ();
				DrawLevel ();
			}

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

