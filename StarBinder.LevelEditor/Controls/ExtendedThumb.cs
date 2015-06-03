using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace StarBinder.LevelEditor.Controls
{
    public enum DragMode
    {
        Move,
        DragDrop
    }
    
    public class ExtendedThumb : Thumb
    {
        static ExtendedThumb()
        {
            EventManager.RegisterClassHandler(typeof(Thumb), DragDrop.DragEnterEvent, new DragEventHandler(OnDragEnter));
            EventManager.RegisterClassHandler(typeof(Thumb), DragDrop.DragLeaveEvent, new DragEventHandler(OnDragLeave));
            EventManager.RegisterClassHandler(typeof(Thumb), DragDrop.DropEvent, new DragEventHandler(OnDrop));
            EventManager.RegisterClassHandler(typeof(Thumb), Thumb.DragDeltaEvent, new DragDeltaEventHandler(OnDragDelta));
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(
            "Mode", typeof (DragMode), typeof (ExtendedThumb), new PropertyMetadata(DragMode.Move));
        public DragMode Mode
        {
            get { return (DragMode) GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty DropCommandProperty = DependencyProperty.Register(
            "DropCommand", typeof (ICommand), typeof (ExtendedThumb), new PropertyMetadata(default(ICommand)));

        public ICommand DropCommand
        {
            get { return (ICommand) GetValue(DropCommandProperty); }
            set { SetValue(DropCommandProperty, value); }
        }

        public static readonly DependencyProperty DragEnterCommandProperty = DependencyProperty.Register(
            "DragEnterCommand", typeof (ICommand), typeof (ExtendedThumb), new PropertyMetadata(default(ICommand)));

        public ICommand DragEnterCommand
        {
            get { return (ICommand) GetValue(DragEnterCommandProperty); }
            set { SetValue(DragEnterCommandProperty, value); }
        }

        public static readonly DependencyProperty DragLeaveCommandProperty = DependencyProperty.Register(
            "DragLeaveCommand", typeof (ICommand), typeof (ExtendedThumb), new PropertyMetadata(default(ICommand)));

        public ICommand DragLeaveCommand
        {
            get { return (ICommand) GetValue(DragLeaveCommandProperty); }
            set { SetValue(DragLeaveCommandProperty, value); }
        }

        public static readonly DependencyProperty DragDeltaCommandProperty = DependencyProperty.Register(
            "DragDeltaCommand", typeof (ICommand), typeof (ExtendedThumb), new PropertyMetadata(default(ICommand)));

        public ICommand DragDeltaCommand
        {
            get { return (ICommand) GetValue(DragDeltaCommandProperty); }
            set { SetValue(DragDeltaCommandProperty, value); }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (Mode == DragMode.Move)
            {
                base.OnMouseLeftButtonDown(e);
            }
            else
            {
                DragDrop.DoDragDrop(this, new DataObject(typeof(object), DataContext), DragDropEffects.Link);
            }
        }

        private static void OnDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            var thumb = (ExtendedThumb)sender;

            if (dragEventArgs.Data.GetData(typeof(object)) == thumb.DataContext) return;

            if (thumb.DragEnterCommand != null && thumb.DragEnterCommand.CanExecute(dragEventArgs))
            {
                thumb.DragEnterCommand.Execute(dragEventArgs);
                dragEventArgs.Handled = true;
            }
        }

        private static void OnDragLeave(object sender, DragEventArgs dragEventArgs)
        {
            var thumb = (ExtendedThumb)sender;

            if (dragEventArgs.Data.GetData(typeof(object)) == thumb.DataContext) return;

            if (thumb.DragLeaveCommand != null && thumb.DragLeaveCommand.CanExecute(dragEventArgs))
            {
                thumb.DragLeaveCommand.Execute(dragEventArgs);
                dragEventArgs.Handled = true;
            }
        }

        private static void OnDrop(object sender, DragEventArgs dragEventArgs)
        {
            var thumb = (ExtendedThumb)sender;

            if (dragEventArgs.Data.GetData(typeof(object)) == thumb.DataContext) return;
            
            if (thumb.DropCommand != null && thumb.DropCommand.CanExecute(dragEventArgs))
            {
                thumb.DropCommand.Execute(dragEventArgs);
                dragEventArgs.Handled = true;
            }
        }

        private static void OnDragDelta(object sender, DragDeltaEventArgs darDeltaEventArgs)
        {
            var thumb = (ExtendedThumb)sender;

            if (thumb.DragDeltaCommand != null && thumb.DragDeltaCommand.CanExecute(darDeltaEventArgs))
            {
                thumb.DragDeltaCommand.Execute(darDeltaEventArgs);
                darDeltaEventArgs.Handled = true;
            }
        }
    }
}
