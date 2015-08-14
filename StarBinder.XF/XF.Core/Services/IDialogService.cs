using System;
using System.Threading.Tasks;
using XF.Core.ViewModels;

namespace XF.Core.Services
{
    public interface IDialogService
    {
        Task DisplayAlert(string title, string message, string cancel);

        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);


        Task<TViewModel> ShowDialog<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IDialogViewModel;

        Task<TViewModel> ShowDialog<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IDialogViewModel;
    }
}
