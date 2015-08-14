using System;
using System.Threading.Tasks;

namespace XF.Core.Views
{
    public interface IDialogPage
    {
        Task ShowAsync();

        Task CloseAsync();

        event EventHandler Closed;
    }
}

