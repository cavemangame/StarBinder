using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using StarBinder.Core;

namespace StarBinder.LevelEditor.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            OnNewCommandExecuted();
        }

        private GalaxyViewModel galaxyViewModel;
        public GalaxyViewModel GalaxyViewModel
        {
            get { return galaxyViewModel; }
            set { SetProperty(ref galaxyViewModel, value); }
        }

        private ICommand newCommand;
        public ICommand NewCommand { get { return newCommand ?? (newCommand = new DelegateCommand(OnNewCommandExecuted)); } }

        private void OnNewCommandExecuted()
        {
            GalaxyViewModel = new GalaxyViewModel(Galaxy.CreateNew());
        }

        private ICommand saveCommand;
        public ICommand SaveCommand { get { return saveCommand ?? (saveCommand = new DelegateCommand(OnSaveCommandExecuted)); } }

        private void OnSaveCommandExecuted()
        {
            throw new NotImplementedException();
        }
    }
}
