using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using StarBinder.Core;

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

        public int X
        {
            get { return calculator.XRelativeToAbs(star.XRel); }
            set
            {
                star.XRel = calculator.XAbsToRelative(value);
                OnPropertyChanged("X");
            }
        }

        public int Y
        {
            get { return calculator.YRelativeToAbs(star.YRel); }
            set
            {
                star.YRel = calculator.YAbsToRelative(value);
                OnPropertyChanged("Y");
            }
        }

        public Star Model { get { return star; } }

        public void OnResize()
        {
            OnPropertyChanged(null);
        }


        #region Commands

        private ICommand dragCommand;
        public ICommand DragCommand { get { return dragCommand ?? (dragCommand = new DelegateCommand<DragDeltaEventArgs>(OnDragCommandExecuted)); } }

        private void OnDragCommandExecuted(DragDeltaEventArgs args)
        {
            var dx = (int)args.HorizontalChange;
            var dy = (int)args.VerticalChange;
            if (dx != 0) X += dx;
            if (dy != 0) Y += dy;
        }


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

        private ICommand dragEnterCommand;
        public ICommand DragEnterCommand { get { return dragEnterCommand ?? (dragEnterCommand = new DelegateCommand<object>(OnExecuteDragEnter)); } }

        private void OnExecuteDragEnter(object arg)
        {
            
        }

        private ICommand dragLeaveCommand;
        public ICommand DragLeaveCommand { get { return dragLeaveCommand ?? (dragLeaveCommand = new DelegateCommand<object>(OnExecuteDragLeave)); } }

        private void OnExecuteDragLeave(object arg)
        {
            
        }

        private ICommand dragOverCommand;
        public ICommand DragOverCommand { get { return dragOverCommand ?? (dragOverCommand = new DelegateCommand<object>(OnExecuteDragOver)); } }

        private void OnExecuteDragOver(object arg)
        {
            
        }

        private ICommand dropCommand;
        public ICommand DropCommand { get { return dropCommand ?? (dropCommand = new DelegateCommand<object>(OnExecuteDrop)); } }

        private void OnExecuteDrop(object arg)
        {

        }

        #endregion
    }
}
