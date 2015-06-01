using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace StarBinder.ViewModels
{
    class WellcomeViewModel : ViewModel
    {
        private readonly INavigationService navigationService;

        public WellcomeViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
    }
}
