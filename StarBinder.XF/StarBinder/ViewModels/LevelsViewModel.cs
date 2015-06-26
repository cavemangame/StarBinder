using System.Collections.Generic;
using StarBinder.Core;
using StarBinder.Core.Services;
using XF.Core.Services;
using XF.Core.ViewModels;

namespace StarBinder.ViewModels
{
    class LevelsViewModel : ViewModelBase
    {
        private readonly IGameService gameService;
        private readonly INavigator navigator;

        public LevelsViewModel(IGameService gameService, INavigator navigator)
        {
            this.gameService = gameService;
            this.navigator = navigator;
        }
        
        private IEnumerable<Chapter> chapters;
        public IEnumerable<Chapter> Chapters
        {
            get { return chapters; }
            set { SetProperty(ref chapters, value); }
        }

        protected override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        protected override void OnNavigatedTo()
        {
            base.OnNavigatedTo();
        }
    }
}
