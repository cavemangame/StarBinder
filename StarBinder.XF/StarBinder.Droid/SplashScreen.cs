using System.Threading;

using Android.App;
using Android.OS;

namespace StarBinder.Droid
{
	[Activity(Theme="@style/Theme.Splash", MainLauncher = true, NoHistory = true)]
	public class SplashScreen : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			StartActivity(typeof(MainActivity));
		}
	}
}

