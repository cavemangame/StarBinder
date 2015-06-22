using Autofac;
using Autofac.Core;
using StarBinder.Core.Services;
using StarBinder.ViewModels;
using StarBinder.Views;
using Xamarin.Forms;
using XF.Core.Bootstrapping;
using XF.Core.Factories;

namespace StarBinder
{
    class Bootstrapper : AutofacBootstrapper
    {
        private readonly Application app;
        private readonly IModule platform;

        public Bootstrapper(Application app, IModule platform)
        {
            this.app = app;
            this.platform = platform;
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<GameViewModel, GamePage>();
            viewFactory.Register<MainViewModel, MainPage>();
            viewFactory.Register<LevelsViewModel, LevelsPage>();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            if (platform != null) builder.RegisterModule(platform);

            builder.RegisterType<StubGameService>().As<IGameService>().SingleInstance();

            builder.RegisterType<MainViewModel>().SingleInstance();
            builder.RegisterType<GameViewModel>().SingleInstance();
            builder.RegisterType<LevelsViewModel>().SingleInstance();

            builder.RegisterType<MainPage>().SingleInstance();
            builder.RegisterType<GamePage>().SingleInstance();
            builder.RegisterType<LevelsPage>().SingleInstance();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            var mainPage = viewFactory.Resolve<MainViewModel>();
            var navigationPage = new NavigationPage(mainPage);
            app.MainPage = navigationPage;
        }
    }
}
