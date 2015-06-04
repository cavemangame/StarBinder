using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using StarBinder.Core;
using StarBinder.LevelEditor.Utils;

namespace StarBinder.LevelEditor.ViewModels
{
    class StarViewModel : BindableBase
    {
        private readonly Star star;
        private readonly GalaxyViewModel galaxy;
        private readonly SizeCalculator calculator;

        public StarViewModel(Star star, GalaxyViewModel galaxy, SizeCalculator calculator)
        {
            this.star = star;
            this.galaxy = galaxy;
            this.calculator = calculator;
        }

        public Geometry Geometry { get { return calculator.GetWpfPoints(Model).ToPathGeometry(); } }

        public Point Point { get { return new Point(X, Y);} }

        public int X
        {
            get { return calculator.XRelToAbs(star.XRel); }
            set
            {
                star.XRel = calculator.XAbsToRel(value);
                OnPropertyChanged("X");
            }
        }

        public int Y
        {
            get { return calculator.YRelToAbs(star.YRel); }
            set
            {
                star.YRel = calculator.YAbsToRel(value);
                OnPropertyChanged("Y");
            }
        }

        public int Width
        {
            get { return calculator.RelToAbsByMinSize(star.WRel); }
            set
            {
                star.WRel = calculator.AbsToRelByMinSize(value);
                OnPropertyChanged("Width");
            }
        }

        public Star Model { get { return star; } }

        public void OnResize()
        {
            OnPropertyChanged(null);
        }

        #region Commands

        private ICommand changeStateCommand;
        public ICommand ChangeStateCommand { get { return changeStateCommand ?? (changeStateCommand = new DelegateCommand<MouseButton?>(OnExecuteChangeState)); } }

        private void OnExecuteChangeState(MouseButton? button)
        {
            if (button == MouseButton.Left)
            {
                Model.NextInitialState();
            }
            else if (button == MouseButton.Right)
            {
                Model.NextFinalState();
            }
        }

        private ICommand dragCommand;
        public ICommand DragCommand { get { return dragCommand ?? (dragCommand = new DelegateCommand<DragDeltaEventArgs>(OnDragCommandExecuted)); } }

        private void OnDragCommandExecuted(DragDeltaEventArgs args)
        {
            var dx = (int)args.HorizontalChange;
            var dy = (int)args.VerticalChange;
            if (dx != 0) X += dx;
            if (dy != 0) Y += dy;

            OnPropertyChanged("Point");
        }
        

        private ICommand dragEnterCommand;
        public ICommand DragEnterCommand { get { return dragEnterCommand ?? (dragEnterCommand = new DelegateCommand<DragEventArgs>(OnExecuteDragEnter)); } }

        private void OnExecuteDragEnter(DragEventArgs arg)
        {
            galaxy.CreateTempLink(arg.Data.GetData(typeof(object)) as StarViewModel, this);
        }

        private ICommand dragLeaveCommand;
        public ICommand DragLeaveCommand { get { return dragLeaveCommand ?? (dragLeaveCommand = new DelegateCommand<DragEventArgs>(OnExecuteDragLeave)); } }

        private void OnExecuteDragLeave(DragEventArgs arg)
        {
            galaxy.RemoveTempLink();
        }

        private ICommand dropCommand;
        public ICommand DropCommand { get { return dropCommand ?? (dropCommand = new DelegateCommand<DragEventArgs>(OnExecuteDrop)); } }

        private void OnExecuteDrop(DragEventArgs arg)
        {
            galaxy.ConfirmLincCreation();
        }

        #endregion
    }
}
