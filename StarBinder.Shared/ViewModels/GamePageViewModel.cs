﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Windows.Foundation;
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
        private readonly SizeCalculator calculator;
        private readonly Stack<Star> clicks = new Stack<Star>();
        private readonly Stack<Star> redo = new Stack<Star>();
        private Galaxy galaxy;

        public GamePageViewModel(INavigationService navigation, ILevelsService levels)
        {
            this.navigation = navigation;
            this.levels = levels;
            calculator = new SizeCalculator(0, 0);
        }

        private Size gameGridSize;
        public Size GameGridSize
        {
            get { return gameGridSize; }
            set
            {
                SetProperty(ref gameGridSize, value);
                calculator.Resize((int)value.Width, (int)value.Height);
                foreach (var star in Stars)
                {
                    star.UpdatePosition();
                }
            }
        }

        private List<StarViewModel> stars;
        public List<StarViewModel> Stars
        {
            get { return stars; } 
            set { SetProperty(ref stars, value); }
        }

        private List<LinkViewModel> links;
        public List<LinkViewModel> Links
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
            clicks.Clear();
            redo.Clear();

            galaxy = await levels.GetLevelModel(level);
            Stars = galaxy.Stars.Select(s => new StarViewModel(s, calculator)).ToList();

            var starsDictionary = Stars.ToDictionary(s => s.Model);
            Links = galaxy.Links.Select(l => new LinkViewModel(starsDictionary[l.From], starsDictionary[l.To])).ToList();
        }

        private ICommand starClickCommand;
        public ICommand StarClickCommand { get { return starClickCommand ?? (starClickCommand = new DelegateCommand<StarViewModel>(OnExecuteStarClick)); } }

        private async void OnExecuteStarClick(StarViewModel star)
        {
            clicks.Push(star.Model);
            redo.Clear();
            OnClicksChanged();

            star.Model.ChangeAll();
            
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

        private DelegateCommand undoCommand;
        public DelegateCommand UndoCommand { get { return undoCommand ?? (undoCommand = new DelegateCommand(OnExecuteUndo, CanExecuteUndo)); } }

        private void OnExecuteUndo()
        {
            var star = clicks.Pop();
            redo.Push(star);
            star.RevertAll();
            
            OnClicksChanged();
        }

        private bool CanExecuteUndo()
        {
            return clicks.Count > 0;
        }

        private DelegateCommand redoCommand;
        public DelegateCommand RedoCommand { get { return redoCommand ?? (redoCommand = new DelegateCommand(OnExecuteRedo, CanExecuteRedo)); } }

        private void OnExecuteRedo()
        {
            var star = redo.Peek();
            clicks.Push(star);
            star.ChangeAll();
            
            OnClicksChanged();
        }

        private bool CanExecuteRedo()
        {
            return redo.Count > 0;
        }

        private DelegateCommand restartCommand;
        public DelegateCommand RestartCommand { get { return restartCommand ?? (restartCommand = new DelegateCommand(OnExecuteRestart, CanExecuteRestart)); } }

        private void OnExecuteRestart()
        {
            galaxy.ResetStarStates();
            clicks.Clear();
            redo.Clear();

            OnClicksChanged();
        }

        private bool CanExecuteRestart()
        {
            return clicks.Count > 0;
        }

        private void OnClicksChanged()
        {
            RestartCommand.RaiseCanExecuteChanged();
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }
    }
}
