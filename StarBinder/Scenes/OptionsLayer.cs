using System;
using CocosSharp;

namespace StarBinder
{
	public class OptionsLayer: CCLayerColor, IPageScene
	{
		private CCSprite _background;
		private CCSprite _backBtn;
		private CCSprite _applyBtn;
		private CCSprite _defaultBtn;
		private CCLabel _label;

		public OptionsLayer() : base()
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

			AddButtons ();
			AddListener ();

			_label = new CCLabel ("OPTIONS!", "arial", 40) {
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
			AddChild (_defaultBtn);
			AddChild (_applyBtn);
		}
			
		private void AddButtons()
		{
			_backBtn = new CCSprite ("back");
			CCRect rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.1f, 0.1f, 0.14f, 0.14f));
			_backBtn.Scale = rect.Size.Width / _backBtn.BoundingBox.Size.Width;
			_backBtn.Position = new CCPoint(rect.MinX, rect.MinY);

			_applyBtn = new CCSprite ("apply");
			rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.5f, 0.1f, 0.14f, 0.14f));
			_applyBtn.Scale = rect.Size.Width / _applyBtn.BoundingBox.Size.Width;
			_applyBtn.Position = new CCPoint(rect.MinX, rect.MinY);

			_defaultBtn = new CCSprite ("close");
			rect = ScreenResolutionManager.Instance.GetRect (new CCRect (0.7f, 0.1f, 0.14f, 0.14f));
			_defaultBtn.Scale = rect.Size.Width / _defaultBtn.BoundingBox.Size.Width;
			_defaultBtn.Position = new CCPoint(rect.MinX, rect.MinY);
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

		public static CCScene OptionsLayerScene (CCWindow mainWindow)
		{
			var scene = new CCScene (mainWindow);
			var layer = new OptionsLayer ();

			scene.AddChild (layer);

			return scene;
		}
	}

}

