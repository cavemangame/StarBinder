using System;
using System.Linq;
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
            
            star.GeometryChanged += (s, a) =>
            {
                OnPropertyChanged("GeometryBack");
                OnPropertyChanged("GeometryFront");
                OnPropertyChanged("HalfWidth");
            };
        }

        public Geometry GeometryBack { get { return calculator.GetWpfPoints(Model, true).ToPathGeometry(); } }
        public Geometry GeometryFront { get { return calculator.GetWpfPoints(Model, false).ToPathGeometry(); } }

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

        public int HalfWidth
        {
            get { return calculator.RelToAbsByMinSize(star.HalfWidthRel); }
            set
            {
                star.HalfWidthRel = calculator.AbsToRelByMinSize(value);
                OnPropertyChanged("HalfWidth");
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

        private bool canDrop;

        private ICommand dragEnterCommand;
        public ICommand DragEnterCommand { get { return dragEnterCommand ?? (dragEnterCommand = new DelegateCommand<DragEventArgs>(OnExecuteDragEnter, CanExecuteDragEnter)); } }

        private void OnExecuteDragEnter(DragEventArgs arg)
        {
            galaxy.CreateTempLink(arg.Data.GetData(typeof(object)) as StarViewModel, this);
        }

        private bool CanExecuteDragEnter(DragEventArgs arg)
        {
            var source = arg.Data.GetData(typeof (object)) as StarViewModel;
            return (canDrop = galaxy.CanAddLink(this, source));
        }

        private ICommand dragOverCommand;
        public ICommand DragOverCommand { get { return dragOverCommand ?? (dragOverCommand = new DelegateCommand<DragEventArgs>(OnExecuteDragOver, CanExecuteDragOver)); } }

        private void OnExecuteDragOver(DragEventArgs arg)
        {
        }

        private bool CanExecuteDragOver(DragEventArgs arg)
        {
            return canDrop;
        }

        private ICommand dragLeaveCommand;
        public ICommand DragLeaveCommand { get { return dragLeaveCommand ?? (dragLeaveCommand = new DelegateCommand<DragEventArgs>(OnExecuteDragLeave)); } }

        private void OnExecuteDragLeave(DragEventArgs arg)
        {
            galaxy.RemoveTempLink();
            canDrop = false;
        }

        private ICommand dropCommand;
        public ICommand DropCommand { get { return dropCommand ?? (dropCommand = new DelegateCommand<DragEventArgs>(OnExecuteDrop, CanExecuteDrop)); } }

        private void OnExecuteDrop(DragEventArgs arg)
        {
            galaxy.ConfirmLincCreation();
            canDrop = false;
        }

        private bool CanExecuteDrop(DragEventArgs arg)
        {
            return galaxy.TempLink != null;
        }

        #endregion
    }
}
