using System.Threading.Tasks;

namespace XF.Core.ViewModels
{
    public interface ICanNavigatableViewModel
    {
        Task<bool> CheckCanNavigate();
    }
}
