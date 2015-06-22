using System.ComponentModel;

namespace XF.Core.ViewModels
{
    public interface IViewModel : INotifyPropertyChanged
    {
        string Title { get; set; }

        bool IsBusy { get; }

        void NavigatedTo();

        void NavigatedFrom();
    }
}
