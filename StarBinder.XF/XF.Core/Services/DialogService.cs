using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Core.Factories;
using XF.Core.ViewModels;
using XF.Core.Views;

namespace XF.Core.Services
{
    public class DialogService : IDialogService
    {
        private readonly Func<Page> pageResolver;
        private readonly IViewFactory viewFactory;
        
        public DialogService(Func<Page> pageResolver, IViewFactory viewFactory)
        {
            this.pageResolver = pageResolver;
            this.viewFactory = viewFactory;
        }

        public async Task DisplayAlert(string title, string message, string cancel)
        {
            await pageResolver().DisplayAlert(title, message, cancel);
        }

        public async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
        {
            return await pageResolver().DisplayAlert(title, message, accept, cancel);
        }

        public async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await pageResolver().DisplayActionSheet(title, cancel, destruction, buttons);
        }


        public Task<TViewModel> ShowDialog<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IDialogViewModel
        {
            TViewModel viewModel;
            var view = viewFactory.Resolve<TViewModel>(out viewModel, setStateAction) as IDialogPage;

            if (view == null)
                throw new InvalidOperationException("view must implement IDialogPage");   

            return ShowDialog(view, viewModel);
        }

        public Task<TViewModel> ShowDialog<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IDialogViewModel
        {
            var view = viewFactory.Resolve(viewModel) as IDialogPage;

            if (view == null)
                throw new InvalidOperationException("view must implement IDialogPage");   

            return ShowDialog(view, viewModel);
        }


        private Task<TViewModel> ShowDialog<TViewModel>(IDialogPage page, TViewModel viewModel)
            where TViewModel : class, IDialogViewModel
        {
            viewModel.SetCloseAction(async () => await page.CloseAsync());
            viewModel.NavigatedTo();

            var task = new TaskCompletionSource<TViewModel>();
            page.Closed += (s, a) => task.SetResult(viewModel);
            page.ShowAsync();

            return task.Task;
        }
    }
}
