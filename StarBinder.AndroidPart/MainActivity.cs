using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Microsoft.Xna.Framework;

using CocosSharp;

namespace StarBinder.AndroidPart
{
	[Activity(
		Label = "StarBinder",
		AlwaysRetainTaskState = true,
		Icon = "@drawable/icon",
		Theme = "@android:style/Theme.NoTitleBar",
		LaunchMode = LaunchMode.SingleInstance,
		ScreenOrientation = ScreenOrientation.Portrait,
		MainLauncher = true,
		ConfigurationChanges =  ConfigChanges.Keyboard | 
		ConfigChanges.KeyboardHidden)
	]
	public class MainActivity : AndroidGameActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			AndroidEnvironment.UnhandledExceptionRaiser += HandleAndroidException;

			var application = new CCApplication ();
			application.ApplicationDelegate = new StarBinderApplicationDelegate ();
			SetContentView (application.AndroidContentView);
			application.StartGame ();
		}

		void HandleAndroidException(object sender, RaiseThrowableEventArgs e)
		{
			CCLog.Log("INTERNAL DEBUG", "PLEASE HANDLE MY EXCEPTION!");
			e.Handled = true;
			System.Console.Write("YOU'VE JUST BEEN HANDLED!");
		}
	}
}


