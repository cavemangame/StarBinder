﻿using System;
using CocosSharp;

namespace StarBinder
{
	public class HelpLayer: CCLayerColor, IPageScene
	{
		private CCSprite _background;
		private CCSprite _backBtn;
		private CCLabel _label;

		public HelpLayer() : base()
		{
			Color = CCColor3B.Black;
			Opacity = 255;
		}

		protected override void AddedToScene ()
		{
			base.AddedToScene ();

			Scene.SceneResolutionPolicy = CCSceneResolutionPolicy.ShowAll;

			_background = new CCSprite ("kosmos2");
			_background.Position = VisibleBoundsWorldspace.Center;

			AddButtons ();
			AddListener ();

			_label = new CCLabel ("HELP!", "arial", 40) {
				Position = VisibleBoundsWorldspace.Center,
				Color = CCColor3B.Green,
				HorizontalAlignment = CCTextAlignment.Center,
				VerticalAlignment = CCVerticalTextAlignment.Center,
				AnchorPoint = CCPoint.AnchorMiddle,
				Dimensions = ContentSize
			};

			AddChild (_background);
			AddChild (_label);
			AddChild (_backBtn);
		}

		private void AddButtons()
		{
			_backBtn = new CCSprite ("back");
			CCRect rect = GetRect(0.1f, 0.1f, 0.14f, 0.14f);
			_backBtn.Scale = rect.Size.Width / _backBtn.BoundingBox.Size.Width;
			_backBtn.Position = new CCPoint(rect.MinX, rect.MinY);
		}

		private void AddListener ()
		{
			AddEventListener (new CCEventListenerTouchAllAtOnce () {
				OnTouchesEnded = (touches, ccevent) =>  {
					var location = touches [0].LocationOnScreen;
					location = WorldToScreenspace (location);

					if (_backBtn.BoundingBoxTransformedToWorld.ContainsPoint (location))
						NextScene (null);				
				}
			});
		}

		public void NextScene(Scenes? nextScene)
		{
			if (GameManager.Instance.OldScene == Scenes.START_GAME)
				Window.DefaultDirector.ReplaceScene (GameStartLayer.GameStartLayerScene (Window));
			else if (GameManager.Instance.OldScene == Scenes.LEVEL_SELECT)
				Window.DefaultDirector.ReplaceScene (SelectLevelLayer.SelectLevelLayerScene (Window));
			else if (GameManager.Instance.OldScene == Scenes.START_GAME)
				Window.DefaultDirector.ReplaceScene (GameLevelLayer.GameLevelLayerScene (Window));
		}

		public static CCScene HelpLayerScene (CCWindow mainWindow)
		{
			var scene = new CCScene (mainWindow);
			var layer = new HelpLayer ();

			scene.AddChild (layer);

			return scene;
		}

		public CCRect GetRect(float x, float y, float width, float height)
		{
			return GameManager.Instance.Calculator.RectRelToAbsByMinSize (x, y, width, height).Convert (); 
		}
	}

}

