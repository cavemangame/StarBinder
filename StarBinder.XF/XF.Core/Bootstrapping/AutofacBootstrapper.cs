using System;
using Autofac;
using Xamarin.Forms;
using XF.Core.Factories;
using XF.Core.Services;

namespace XF.Core.Bootstrapping
{
    public abstract class AutofacBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            var container = builder.Build();
            var viewFactory = container.Resolve<IViewFactory>();

            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            
            RegisterNavigation(builder);

            // default page resolver
            builder.RegisterInstance<Func<Page>>(() =>
                {
                    // Check if we are using MasterDetailPage
                    var masterDetailPage = Application.Current.MainPage as MasterDetailPage;
                    
                    var page = masterDetailPage != null ? masterDetailPage.Detail : Application.Current.MainPage;

                    // Check if page is a NavigationPage
                    var navigationPage = page as IPageContainer<Page>;

                    return navigationPage != null ? navigationPage.CurrentPage : page;
                }
            );
        }

        protected virtual void RegisterNavigation(ContainerBuilder builder)
        {
            builder.RegisterType<Navigator>().As<INavigator>().SingleInstance();
        }

        protected abstract void RegisterViews(IViewFactory viewFactory);

        protected abstract void ConfigureApplication(IContainer container);
    }
}
