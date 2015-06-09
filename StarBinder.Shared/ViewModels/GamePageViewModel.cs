using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using StarBinder.Core;
using StarBinder.Core.Services;

namespace StarBinder.ViewModels
{
    class GamePageViewModel : ViewModel
    {
        private readonly INavigationService navigation;
        private readonly ILevelsService levels;
        private Galaxy galaxy;

        public GamePageViewModel(INavigationService navigation, ILevelsService levels)
        {
            this.navigation = navigation;
            this.levels = levels;
        }

        private List<Star> stars;
        public List<Star> Stars
        {
            get { return stars; }
            set { SetProperty(ref stars, value); }
        }

        private List<Link> links;
        public List<Link> Links
        {
            get { return links; }
            set { SetProperty(ref links, value); }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
        }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);

            if (navigationParameter is int)
            {
                NewGame((int)navigationParameter);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private async void NewGame(int level)
        {
            galaxy = await levels.GetLevelModel(level);
            Stars = galaxy.Stars.ToList();
            Links = galaxy.Links.ToList();
        }

        private ICommand starClickCommand;
        public ICommand StarClickCommand { get { return starClickCommand ?? (starClickCommand = new DelegateCommand<Star>(OnExecuteStarClick)); } }

        private async void OnExecuteStarClick(Star star)
        {
            star.ChangeAll();
            
            if (!galaxy.IsComplete) return;

            //todo сообщить что уровень пройден

            if (await levels.IsNextLevel(galaxy))
            {
                NewGame(galaxy.Number + 1);
            }
            else
            {
                //todo выйти в главное меню или меню глав
            }
        }
    }
}
