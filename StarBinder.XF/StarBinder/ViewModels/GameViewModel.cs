using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StarBinder.Core;
using StarBinder.Core.Services;
using XF.Core.ViewModels;

namespace StarBinder.ViewModels
{
    class GameViewModel : ViewModelBase
    {
        private readonly IGameService gameService;
        private Galaxy currentLevel;

        public GameViewModel(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public override void NavigatedFrom()
        {
            ChangeLevelIfNeed();
        }

        public override void NavigatedTo()
        {
            ChangeLevelIfNeed();
        }

        private List<Star> stars;
        public List<Star> Stars
        {
            get { return stars; }
            set { SetProperty(ref stars, value); }
        }

        private List<Link> links;
        public List<Link> Links
        {
            get { return links; }
            set { SetProperty(ref links, value); }
        }

        private async void ChangeLevelIfNeed()
        {
            var cur = await gameService.GetCurrentLevel();

            if (cur == currentLevel) return;
            
            IsBusy = true;
    
            currentLevel = cur;

            Links = new List<Link>(currentLevel.Links);
            Stars = new List<Star>(currentLevel.Stars);

            IsBusy = false;
        }
    }
}
