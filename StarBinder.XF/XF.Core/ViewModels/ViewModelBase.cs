using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace XF.Core.ViewModels
{
    public abstract class ViewModelBase : IViewModel
    {
        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isBusy;
        public bool IsBusy { get { return isBusy; } protected set { SetProperty(ref isBusy, value); } }

        public void NavigatedTo()
        {
            IsBusy = true;
            
            OnNavigatedTo();
            
            IsBusy = false;
        }

        protected virtual void OnNavigatedTo() { }

        public void NavigatedFrom()
        {
            IsBusy = true;

            OnNavigatedFrom();

            IsBusy = false;
        }

        protected virtual void OnNavigatedFrom() { }

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName);
        }
    }
}
