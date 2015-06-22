using System.Diagnostics;
using Autofac.Core;
using Xamarin.Forms;

namespace StarBinder
{
    public partial class App : Application
    {
        public App(IModule platform = null)
        {
            InitializeComponent();
            var bootstrapper = new Bootstrapper(this, platform);
            bootstrapper.Run();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            Debug.WriteLine("OnStart");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Debug.WriteLine("OnSleep");
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Debug.WriteLine("OnResume");
        }
    }
}
