using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Unity;
using StarBinder.Core.Services;
using StarBinder.ViewModels;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace StarBinder
{
    public sealed partial class App : MvvmAppBase
    {
        readonly IUnityContainer container = new UnityContainer();

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
#if WINDOWS_PHONE_APP
            Resources["PhoneOnly"] = Visibility.Visible;
            Resources["StoreOnly"] = Visibility.Collapsed;
#else
            Resources["PhoneOnly"] = Visibility.Collapsed;
            Resources["StoreOnly"] = Visibility.Visible;
#endif
            
            base.OnLaunched(args);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            //NavigationService.RestoreSavedNavigation();
            NavigationService.Navigate("Wellcome", null);
            return Task.FromResult<object>(null);
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            container.RegisterInstance(SessionStateService);
            container.RegisterInstance(NavigationService);
            container.RegisterType<WellcomePageViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<GamePageViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ILevelsService, MockLevelsService>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewModelFactory(viewModelType => container.Resolve(viewModelType));

            return Task.FromResult<object>(null);
        }
    }
}