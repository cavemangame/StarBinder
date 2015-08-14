using System;
using System.Threading.Tasks;

namespace XF.Core.ViewModels
{
    public interface IDialogViewModel : IViewModel
    {
        void SetCloseAction(Action close);
    }
}

