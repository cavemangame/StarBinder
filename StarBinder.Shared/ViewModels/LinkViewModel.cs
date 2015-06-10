using Microsoft.Practices.Prism.Mvvm;

namespace StarBinder.ViewModels
{
    class LinkViewModel : ViewModel
    {
        public StarViewModel Source { get; private set; }
        public StarViewModel Target { get; private set; }

        public LinkViewModel(StarViewModel source, StarViewModel target)
        {
            Source = source;
            Target = target;
        }
    }
}
