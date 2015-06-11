using Windows.Foundation;
using Microsoft.Practices.Prism.Mvvm;

namespace StarBinder.ViewModels
{
    class LinkViewModel : ViewModel
    {
        public LinkViewModel(StarViewModel source, StarViewModel target)
        {
            Source = source;
            Target = target;
        }

        public Point GradientStartPoint { get { return new Point(Source.Model.XRel > Target.Model.XRel ? 1 : 0, Source.Model.YRel > Target.Model.YRel ? 1 : 0); } }
        public Point GradientStopPoint { get { return new Point(Source.Model.XRel > Target.Model.XRel ? 0 : 1, Source.Model.YRel > Target.Model.YRel ? 0 : 1); } }

        public StarViewModel Source { get; private set; }
        public StarViewModel Target { get; private set; }
    }
}
