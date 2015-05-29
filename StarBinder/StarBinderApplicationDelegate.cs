using System;
using CocosSharp;
using StarBinder;

namespace StarBinder
{
	public class StarBinderApplicationDelegate  : CCApplicationDelegate
	{
		public override void ApplicationDidFinishLaunching (CCApplication application, CCWindow mainWindow)
		{
			application.PreferMultiSampling = false;
			application.ContentRootDirectory = "Content";
			application.ContentSearchPaths.Add ("animations");
			application.ContentSearchPaths.Add ("fonts");
			application.ContentSearchPaths.Add ("sounds");

			//mainWindow.SupportedDisplayOrientations = CCDisplayOrientation.Portrait;


			float desiredWidth = 400.0f;
			float desiredHeight = 600.0f;
			CCSize windowSize = mainWindow.WindowSizeInPixels;
			CCScene.SetDefaultDesignResolution(windowSize.Width, windowSize.Height, CCSceneResolutionPolicy.ExactFit);
			application.ContentSearchPaths.Add ("images/hd");

		/*	if (desiredWidth < windowSize.Width) {
				application.ContentSearchPaths.Add ("images/hd");
				CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
			} else {
				application.ContentSearchPaths.Add ("images/ld");
				CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
			}*/
				
		
			//CCSimpleAudioEngine.SharedEngine.PreloadEffect ("Sounds/tap");

			ScreenResolutionManager.CreateResolutionManager (windowSize.Width, windowSize.Height);
			CCScene scene = GameStartLayer.GameStartLayerScene(mainWindow);
			mainWindow.RunWithScene (scene);
		}

		public override void ApplicationDidEnterBackground (CCApplication application)
		{
			application.Paused = true;
			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic ();
		}

		public override void ApplicationWillEnterForeground (CCApplication application)
		{
			application.Paused = false;
			//CCSimpleAudioEngine.SharedEngine.PauseBackgroundMusic ();
		}
	}
}
