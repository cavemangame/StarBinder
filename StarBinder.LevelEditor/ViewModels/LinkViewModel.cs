using Microsoft.Practices.Prism.Mvvm;
using StarBinder.Core;

namespace StarBinder.LevelEditor.ViewModels
{
    class LinkViewModel : BindableBase
    {
        private readonly Link link;

        public LinkViewModel(StarViewModel sourceStar, StarViewModel targetStar, Link link)
        {
            this.sourceStar = sourceStar;
            this.targetStar = targetStar;
            this.link = link;
        }

        public Link Model { get { return link; } }

        private StarViewModel sourceStar;
        public StarViewModel SourceStar
        {
            get { return sourceStar; }
            set 
            { 
                if (sourceStar == value) return;
                sourceStar = value;
                OnPropertyChanged("SourceStar");
            }
        }

        private StarViewModel targetStar;
        public StarViewModel TargetStar
        {
            get { return targetStar; }
            set 
            { 
                if (targetStar == value) return;
                targetStar = value;
                OnPropertyChanged("TargetStar");
            }
        }
    }
}
