using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using StarBinder.LevelEditor.ViewModels;
using StarBinder.LevelEditor.Views;

namespace StarBinder.LevelEditor
{
    public partial class App
    {
        readonly IUnityContainer container = new UnityContainer();
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            container.RegisterType<MainWindowViewModel>();
            ViewModelLocationProvider.SetDefaultViewModelFactory(viewModelType => container.Resolve(viewModelType));

            ShutdownMode = ShutdownMode.OnMainWindowClose;
            
            MainWindow = new MainWindow();
            MainWindow.Show();
        }
    }
}
