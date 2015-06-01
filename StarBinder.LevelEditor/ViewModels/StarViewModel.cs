using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using StarBinder.Core;
using StarBinder.LevelEditor.Utils;

namespace StarBinder.LevelEditor.ViewModels
{
    class StarViewModel : BindableBase
    {
        private readonly Star model;
        private readonly GalaxyState galaxyState;

        public StarViewModel(Star star, GalaxyState galaxyState)
        {
            model = star;
            this.galaxyState = galaxyState;
        }

        public double XRel
        {
            get { return model.XRel; }
            set
            {
                if (Math.Abs(model.XRel - value) < 0.001) return;
                model.XRel = value;
                OnPropertyChanged("XRel");
            }
        }

        public double YRel
        {
            get { return model.YRel; }
            set
            {
                if (Math.Abs(model.YRel - value) < 0.001) return;
                model.YRel = value;
                OnPropertyChanged("YRel");
            }
        }

        public Star Model { get { return model; } }

        //public string CurrentStateColor { get { return model.State.Color; } }
        //public string FinalStateColor { get { return model.FinalState.Color; } } 


        private ICommand dragCommand;
        public ICommand DragCommand { get { return dragCommand ?? (dragCommand = new DelegateCommand<DragDeltaEventArgs>(OnDragCommandExecuted)); } }

        private void OnDragCommandExecuted(DragDeltaEventArgs parametr)
        {
            XRel += galaxyState.GetdXRel(parametr.HorizontalChange);
            YRel += galaxyState.GetdYRel(parametr.VerticalChange);
        }


        private ICommand changeStateCommand;
        public ICommand ChangeStateCommand { get { return changeStateCommand ?? (changeStateCommand = new DelegateCommand<MouseButton?>(OnChangeStateCommandExecuted)); } }

        private void OnChangeStateCommandExecuted(MouseButton? button)
        {
            if (button == MouseButton.Left)
            {
                if (galaxyState.IsEditMode)
                    Model.NextInitialState();
                else
                    Model.ChangeAll();
            }
            else if (button == MouseButton.Right && galaxyState.IsEditMode)
            {
                Model.NextFinalState();
            }
        }





        private ICommand mouseRightCommand;
        public ICommand MouseRightCommand { get { return mouseRightCommand ?? (mouseRightCommand = new DelegateCommand<bool?>(OnMouseRightCommandExecuted)); } }

        private void OnMouseRightCommandExecuted(bool? mouseUp)
        {
            if (!mouseUp.HasValue) return;
            
            if (mouseUp == true)
            {
                if (galaxyState.InitialDragStar != null && galaxyState.InitialDragStar != this)
                {
                    
                }
            }
            else
            {
                
            }
        }
    }
}
