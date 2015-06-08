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
		private List<CCDrawNode> _links;

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
			_links = new List<CCDrawNode> ();

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
			CCRect rect = Utils.GetRect(0.1f, 0.1f, 0.14f, 0.14f);
			_backBtn.Scale = rect.Size.Width / _backBtn.BoundingBox.Size.Width;
			_backBtn.Position = new CCPoint (rect.MinX, rect.MinY);

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

			_restartBtn = new CCSprite ("refresh");
			rect = Utils.GetRect(0.9f, 0.1f, 0.14f, 0.14f);
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
					_level.Star(pair.Key).ChangeAll();
					DrawLevel ();
					break;
				}
			}
		}

		string json = "{\"Name\":null,\"Description\":null,\"Number\":0,\"StepsSilver\":0,\"StepsGold\":0,\"BestSolve\":[0,1,4],\"States\":[{\"Id\":\"8747147b-c2a6-4096-8b32-799d2480af87\",\"Color\":\"#FF000000\"},{\"Id\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"Color\":\"#FFF0FFFF\"}],\"Stars\":[{\"StateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"FinalStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"InitialStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"RotateAngle\":15.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":true,\"Beams\":6,\"HalfWidthRel\":0.1,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.5,\"YRel\":0.48333333333333334,\"Number\":0},{\"StateId\":\"8747147b-c2a6-4096-8b32-799d2480af87\",\"FinalStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"InitialStateId\":\"8747147b-c2a6-4096-8b32-799d2480af87\",\"RotateAngle\":0.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":false,\"Beams\":5,\"HalfWidthRel\":0.07,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.255,\"YRel\":0.62333333333333329,\"Number\":1},{\"StateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"FinalStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"InitialStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"RotateAngle\":30.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":false,\"Beams\":5,\"HalfWidthRel\":0.07,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.8625,\"YRel\":0.36,\"Number\":2},{\"StateId\":\"8747147b-c2a6-4096-8b32-799d2480af87\",\"FinalStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"InitialStateId\":\"8747147b-c2a6-4096-8b32-799d2480af87\",\"RotateAngle\":15.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":false,\"Beams\":5,\"HalfWidthRel\":0.09,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.7225,\"YRel\":0.59,\"Number\":3},{\"StateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"FinalStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"InitialStateId\":\"b1d9070f-9e91-4efd-8743-949c2b4c5e5e\",\"RotateAngle\":-15.0,\"SubBeamsCoeff\":0.5,\"InnerCoeff\":0.3,\"IsSubBeams\":false,\"Beams\":5,\"HalfWidthRel\":0.08,\"FrontAngle\":2.0,\"FrontScale\":0.7,\"XRel\":0.1825,\"YRel\":0.425,\"Number\":4}],\"Links\":[{\"From\":4,\"To\":1,\"Direction\":0},{\"From\":0,\"To\":1,\"Direction\":0},{\"From\":0,\"To\":3,\"Direction\":0},{\"From\":3,\"To\":2,\"Direction\":0}]}";
		private void InitLevel()
		{
			// load current level
			_initLevel = SerializationHelper.GalaxyFromJson(json);
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
			foreach (var path in _links) 
			{
				RemoveChild (path);
			}
			_stars = new Dictionary<int, CCDrawNode> ();
			_links = new List<CCDrawNode> ();

			var r = new System.Random ();
			// add stars
			foreach (var s in _level.Stars) 
			{
				CCDrawNode pStar = new CCDrawNode ();
				Point<int>[] pts = GameManager.Instance.Calculator.GetCocosPoints (s, true);
				Point<int>[] pts2 = GameManager.Instance.Calculator.GetCocosPoints (s, false);
				List<CCPoint> ccpts = new List<CCPoint> ();
				List<CCPoint> ccpts2 = new List<CCPoint> ();

				for (int i = 0; i < pts.Length-1; i++) 
				{
					ccpts.Add(new CCPoint (pts[i].X, pts[i].Y));
				}
				for (int i = 0; i < pts2.Length-1; i++) 
				{
					ccpts2.Add(new CCPoint (pts2[i].X, pts2[i].Y));
				}

				Triangulator tris = new Triangulator(ccpts.ToArray());
				int [] indices = tris.Triangulate();
				CCPoint [] newVerts = new CCPoint[3];
			
				CCColor4B clr = Utils.GetColor(s.FinalState.Color);
				for (int i = 0; i < indices.Length; i += 3)
				{
					newVerts[0] = ccpts[indices[i]];
					newVerts[1] = ccpts[indices[i + 1]];
					newVerts[2] = ccpts[indices[i + 2]];
					pStar.DrawPolygon(newVerts, 3, clr, 0, CCColor4B.White);
				}

				tris = new Triangulator(ccpts2.ToArray());
				indices = tris.Triangulate();
				CCColor4B clr1 = Utils.GetColor(s.State.Color);
				newVerts = new CCPoint[3];
				for (int i = 0; i < indices.Length; i += 3)
				{
					newVerts[0] = ccpts2[indices[i]];
					newVerts[1] = ccpts2[indices[i + 1]];
					newVerts[2] = ccpts2[indices[i + 2]];
		
					pStar.DrawPolygon(newVerts, 3, clr1, 0, CCColor4B.White);
				}

				_stars.Add (s.Number, pStar);

	
				foreach (Link link in _level.Links)
				{
					CCDrawNode pBind = new CCDrawNode ();

					CCPoint pt1 = new CCPoint (GameManager.Instance.Calculator.RelToAbsByMinSize (link.From.XRel), GameManager.Instance.Calculator.RelToAbsByMinSize (1-link.From.YRel));
					CCPoint pt2 = new CCPoint (GameManager.Instance.Calculator.RelToAbsByMinSize (link.To.XRel), GameManager.Instance.Calculator.RelToAbsByMinSize (1-link.To.YRel));

					pBind.DrawLine (pt1, pt2, 3.0f, CCColor4B.Green);
					_links.Add (pBind);
				}
			}

			foreach (var path in _links) 
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

