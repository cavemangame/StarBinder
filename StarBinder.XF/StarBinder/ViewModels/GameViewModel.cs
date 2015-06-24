using System.Collections.Generic;
using System.Diagnostics;
using StarBinder.Core;
using StarBinder.Core.Services;
using Xamarin.Forms;
using XF.Core.Services;
using XF.Core.ViewModels;

namespace StarBinder.ViewModels
{
    class GameViewModel : ViewModelBase
    {
        private readonly IGameService gameService;
        private readonly IDialogService dialog;
        private Galaxy currentLevel;

        public GameViewModel(IGameService gameService, IDialogService dialog)
        {
            this.gameService = gameService;
            this.dialog = dialog;
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

        private Command<Star> starTapCommand;
        public Command<Star> StarTapCommand { get { return starTapCommand ?? (starTapCommand = new Command<Star>(OnExecuteStarTap)); } }

        private async void OnExecuteStarTap(Star star)
        {
            star.ChangeAll();

            Device.OnPlatform(
                iOS: () =>
                {
                    Links = new List<Link>(Links);
                    Stars = new List<Star>(Stars);
                    Debug.WriteLine("Refresh for iOS");
                });

            if (!currentLevel.IsComplete) return;
            
            //todo await gameService.SaveLevelState(currentLevel);
                
            if (await dialog.DisplayAlert("Success!", "Do you want restart?", "Yes", "No"))
            {
                currentLevel.ResetStarStates();
                StartLevel(currentLevel);
            }
        }

        private async void ChangeLevelIfNeed()
        {
            var cur = await gameService.GetCurrentLevel();

            if (cur == currentLevel) return;
            
            StartLevel(cur);
        }

        private void StartLevel(Galaxy level)
        {
            IsBusy = true;

            currentLevel = level;

            Links = new List<Link>(currentLevel.Links);
            Stars = new List<Star>(currentLevel.Stars);

            IsBusy = false;
        }
    }
}
