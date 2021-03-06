﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using StarBinder.Core;
using StarBinder.LevelEditor.Controls;
using StarBinder.LevelEditor.Utils;

namespace StarBinder.LevelEditor.ViewModels
{
    class GalaxyViewModel : BindableBase
    {
        private readonly Galaxy galaxy;
        private readonly SizeCalculator calculator;
        
        public GalaxyViewModel(Galaxy galaxy)
        {
            this.galaxy = galaxy;
            Width = 200;
            Height = 200;
            calculator = new SizeCalculator(Width, Height);
            
            InitStates();
            InitStars();
            InitLinks();
        }

        public Galaxy Galaxy { get { return galaxy; } }

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

        private int height;
        public int Height 
        {
            get { return height; }
            set { SetProperty(ref height, value); } 
        }

        private int width;
        public int Width 
        { 
            get { return width; }
            set { SetProperty(ref width, value); } 
        }

        private bool isLinksMode;
        public bool IsLinksMode
        {
            get { return isLinksMode; }
            set
            {
                SetProperty(ref isLinksMode, value);
                OnPropertyChanged("DragMode");
            }
        }

        public DragMode DragMode { get { return IsLinksMode ? DragMode.DragDrop : DragMode.Move; } }

        private ImageSource backImage;
        public ImageSource BackImage
        {
            get { return backImage; }
            internal set
            {
                SetProperty(ref backImage, value);
                
                calculator.Resize(Width, Height);
                Stars.ForEach(s => s.OnResize());
                AddStarCommand.RaiseCanExecuteChanged();
            }
        }

        private int steps = 10;
        public int Steps
        {
            get { return steps; }
            set
            {
                SetProperty(ref steps, value);
                MixCommand.RaiseCanExecuteChanged();
                ResolveCommand.RaiseCanExecuteChanged();
            }
        }

        private bool isGameMode;
        public bool IsGameMode
        {
            get { return isGameMode; }
            set { SetProperty(ref isGameMode, value); }
        }

        private ICommand loadBackCommand;
        public ICommand LoadBackCommand { get { return loadBackCommand ?? (loadBackCommand = new DelegateCommand(OnLoadBackCommandExecuted)); } }

        private void OnLoadBackCommandExecuted()
        {
            var dlg = new OpenFileDialog { CheckFileExists = true, DefaultExt = ".png", Multiselect = false };
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

        private ICommand loadBackSVGCommand;
        public ICommand LoadBackSVGCommand { get { return loadBackSVGCommand ?? (loadBackSVGCommand = new DelegateCommand(OnLoadBackSVGCommandExecuted)); } }
        private void OnLoadBackSVGCommandExecuted()
        {
            var dlg = new OpenFileDialog { CheckFileExists = true, DefaultExt = ".svg", Filter = "SVG Files|*.svg", Multiselect = false };
            if (dlg.ShowDialog() == true)
            {
                galaxy.BackPath = dlg.FileName;
                BackPath = dlg.FileName;
            }
        }

        private string backPath;
        public string BackPath
        {
            get { return galaxy.BackPath; }
            set { SetProperty(ref backPath, value); }
        }

        private ICommand testCommand;
        public ICommand TestCommand { get { return testCommand ?? (testCommand = new DelegateCommand(OnExecuteTest)); } }

        private void OnExecuteTest()
        {
            galaxy.ResetStarStates();
            IsGameMode = true;
        }

        private ICommand editCommand;
        public ICommand EditCommand { get { return editCommand ?? (editCommand = new DelegateCommand(OnExecuteEdit)); } }

        private void OnExecuteEdit()
        {
            IsGameMode = false;
            galaxy.ResetStarStates();
        }

        private DelegateCommand resolveCommand;
        public DelegateCommand ResolveCommand { get { return resolveCommand ?? (resolveCommand = new DelegateCommand(OnExecuteResolve, CanExecuteMixResolve)); } }

        private async void OnExecuteResolve()
        {
            IsEnabled = false;
            var res = await galaxy.Resolve(Steps);
            MessageBox.Show(res.Any() ? string.Join("; ", res) : string.Format("В пределах {0} ходов решений не найдено", Steps));
            IsEnabled = true;
        }

        private DelegateCommand mixCommand;
        public DelegateCommand MixCommand { get { return mixCommand ?? (mixCommand = new DelegateCommand(OnExecuteMix, CanExecuteMixResolve)); } }

        private async void OnExecuteMix()
        {
            IsEnabled = false;
            var res = await galaxy.Mix(Steps);
            MessageBox.Show(res.Any() ? string.Join("; ", res) : string.Format("Не удалось подобрать расстановку, \nрешающуюся за {0} ходов.", Steps));
            IsEnabled = true;
        }

        private bool CanExecuteMixResolve()
        {
            return Steps > 0 && Steps < 50;
        }


        #region Stars

        private Dictionary<Star, StarViewModel> stars;

        private void InitStars()
        {
            stars = new Dictionary<Star, StarViewModel>();
            Stars = new ObservableCollection<StarViewModel>(galaxy.Stars.Select(s => CreateStar(s)));
        }

        public ObservableCollection<StarViewModel> Stars { get; private set; }

        private StarViewModel CreateStar(Star star)
        {
            var svm = new StarViewModel(star, this, calculator);
            stars.Add(star, svm);
            
            return svm;
        }
        

        private DelegateCommand addStarCommand;
        public DelegateCommand AddStarCommand { get { return addStarCommand ?? (addStarCommand = new DelegateCommand(OnAddStarCommandExecuted, CanAddStarCommandExecuted)); } }

        private void OnAddStarCommandExecuted()
        {
            var star = CreateStar(galaxy.AddStar());
            star.Model.XRel += 0.02 * Stars.Count;
            Stars.Add(star);
        }

        private bool CanAddStarCommandExecuted()
        {
            return BackImage != null;
        }

        private ICommand delStarCommand;
        public ICommand DelStarCommand { get { return delStarCommand ?? (delStarCommand = new DelegateCommand<StarViewModel>(OnExecuteDelStar)); } }

        private void OnExecuteDelStar(StarViewModel star)
        {
            galaxy.RemoveStar(star.Model);
            Stars.Remove(star);
            stars.Remove(star.Model);
            InitLinks();
        }

        #endregion

        #region Links

        private void InitLinks()
        {
            Links = new ObservableCollection<LinkViewModel>(galaxy.Links.Select(l => CreateLink(l)));
            OnPropertyChanged("Links");
        }

        public ObservableCollection<LinkViewModel> Links { get; private set; }

        private LinkViewModel tempLink;
        public LinkViewModel TempLink
        {
            get { return tempLink; }
            set 
            { 
                if (tempLink == value) return;
                tempLink = value;
                OnPropertyChanged("TempLink");
            }
        }

        internal void CreateTempLink(StarViewModel firstStar, StarViewModel secondStar)
        {
            TempLink = new LinkViewModel(firstStar, secondStar, null);
        }

        internal void RemoveTempLink()
        {
            TempLink = null;
        }

        internal void ConfirmLincCreation()
        {
            if (TempLink == null)
                throw new InvalidOperationException("Temporary link not founded");

            Links.Add(CreateLink(galaxy.AddLink(TempLink.SourceStar.Model, TempLink.TargetStar.Model)));

            TempLink = null;
        }

        internal bool CanAddLink(StarViewModel firstStar, StarViewModel secondStar)
        {
            return galaxy.CanAddLink(firstStar.Model, secondStar.Model);
        }

        private LinkViewModel CreateLink(Link link)
        {
            return new LinkViewModel(stars[link.From], stars[link.To], link);
        }

        private ICommand delLinkCommand;
        public ICommand DelLinkCommand { get { return delLinkCommand ?? (delLinkCommand = new DelegateCommand<LinkViewModel>(OnExecuteDelLink)); } }

        private void OnExecuteDelLink(LinkViewModel link)
        {
            Links.Remove(link);
            galaxy.RemoveLink(link.Model);
        }

        #endregion

        #region States

        private void InitStates()
        {
            AllColors = typeof(Colors).GetProperties().Where(pi => pi.PropertyType == typeof(Color)).Select(pi => (pi.GetValue(null, null)).ToString()).ToList();
            States = new ObservableCollection<State>(galaxy.States);
        }

        public List<string> AllColors { get; set; }
        public ObservableCollection<State> States { get; private set; }


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
