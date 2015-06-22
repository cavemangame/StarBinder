using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XF.Core.Services
{
    public class DialogService : IDialogService
    {
        private readonly Func<Page> pageResolver;
        
        public DialogService(Func<Page> pageResolver)
        {
            this.pageResolver = pageResolver;
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
    }
}
