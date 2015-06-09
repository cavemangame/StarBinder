using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace StarBinder.ViewModels
{
    class WellcomePageViewModel : ViewModel
    {
        private readonly INavigationService navigationService;

        public WellcomePageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        private ICommand startNewCommand;
        public ICommand StartNewCommand { get { return startNewCommand ?? (startNewCommand = new DelegateCommand(OnExecuteStartNew)); } }

        private void OnExecuteStartNew()
        {
            navigationService.Navigate("Game", 13);
        }
    }
}
