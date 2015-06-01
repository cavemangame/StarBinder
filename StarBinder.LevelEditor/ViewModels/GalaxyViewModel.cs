using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using StarBinder.Core;
using StarBinder.LevelEditor.Utils;
using Color = System.Windows.Media.Color;

namespace StarBinder.LevelEditor.ViewModels
{
    class GalaxyViewModel : BindableBase
    {
        private readonly Galaxy galaxy;
        private readonly GalaxyState galaxyState;
        
        public GalaxyViewModel(Galaxy galaxy)
        {
            this.galaxy = galaxy;
            this.galaxyState = new GalaxyState(this);
            AllColors = typeof(Colors).GetProperties().Where(pi => pi.PropertyType == typeof (Color)).Select(pi => (pi.GetValue(null, null)).ToString()).ToList(); 
            
            States = new ObservableCollection<State>(galaxy.States);
            Stars = new ObservableCollection<StarViewModel>(galaxy.Stars.Select(s => new StarViewModel(s, galaxyState)));
            Width = 200;
            Height = 200;
        }

        public List<string> AllColors { get; set; }
        public ObservableCollection<StarViewModel> Stars { get; private set; }
        public ObservableCollection<State> States { get; private set; }

        private double height;
        public double Height 
        {
            get { return height; }
            set { SetProperty(ref height, value); } 
        }
        
        private double width;
        public double Width 
        { 
            get { return width; }
            set { SetProperty(ref width, value); } 
        }

        private ImageSource backImage;
        public ImageSource BackImage
        {
            get { return backImage; }
            private set
            {
                SetProperty(ref backImage, value);
                AddStarCommand.RaiseCanExecuteChanged();
            }
        }

        #region Commands

        private ICommand loadBackCommand;
        public ICommand LoadBackCommand { get { return loadBackCommand ?? (loadBackCommand = new DelegateCommand(OnLoadBackCommandExecuted)); } }

        private void OnLoadBackCommandExecuted()
        {
            var dlg = new OpenFileDialog{ CheckFileExists = true, DefaultExt = ".png", Multiselect = false };
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var bmp = dlg.FileName.LoadBitmapSourceFromFile(0.5);
                    Width = bmp.PixelWidth;
                    Height = bmp.PixelHeight;
                    BackImage = bmp;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Не удалось загрузить рисунок", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private DelegateCommand addStarCommand;
        public DelegateCommand AddStarCommand { get { return addStarCommand ?? (addStarCommand = new DelegateCommand(OnAddStarCommandExecuted, CanAddStarCommandExecuted)); } }

        private void OnAddStarCommandExecuted()
        {
            var star = galaxy.AddStar();
            var starVm = new StarViewModel(star, galaxyState);
            Stars.Add(starVm);
        }

        private bool CanAddStarCommandExecuted()
        {
            return BackImage != null;
        }

        private ICommand addStateCommand;
        public ICommand AddStateCommand { get { return addStateCommand ?? (addStateCommand = new DelegateCommand<State>(OnAddStateCommandExecuted)); } }

        private void OnAddStateCommandExecuted(State previous)
        {
            var index = States.IndexOf(previous);
            var state = galaxy.AddState(previous);
            States.Insert(index + 1, state);
        }

        private ICommand delStateCommand;
        public ICommand DelStateCommand { get { return delStateCommand ?? (delStateCommand = new DelegateCommand<State>(OnDelStateCommandExecuted)); } }

        private void OnDelStateCommandExecuted(State state)
        {
            States.Remove(state);
            galaxy.RemoveState(state);
        }

        #endregion
    }
}
