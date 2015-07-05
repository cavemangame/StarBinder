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
        private readonly IResourcesService resources;
        private readonly IDialogService dialog;
        private readonly INavigator navigator;
        private int steps;

        public GameViewModel(IGameService gameService, IResourcesService resources, IDialogService dialog, INavigator navigator)
        {
            this.gameService = gameService;
            this.resources = resources;
            this.dialog = dialog;
            this.navigator = navigator;
        }

        protected override void OnNavigatedFrom()
        {
            ChangeLevelIfNeed();
        }

        protected override void OnNavigatedTo()
        {
            ChangeLevelIfNeed();
        }

        private Galaxy currentLevel;
        public Galaxy CurrentLevel
        {
            get { return currentLevel; }
            set { SetProperty(ref currentLevel, value); }
        }
        
        private string backImageSvg;
        public string BackImageSvg
        {
            get { return backImageSvg; }
            set { SetProperty(ref backImageSvg, value); }
        }

        private Command<Star> starTapCommand;
        public Command<Star> StarTapCommand { get { return starTapCommand ?? (starTapCommand = new Command<Star>(OnExecuteStarTap)); } }

        private async void OnExecuteStarTap(Star star)
        {
            star.ChangeAll();
            steps++;

            if (!CurrentLevel.IsComplete) return;

            await gameService.SaveState(steps);

            var next = await gameService.GetNextLevelInfo();
            if (!next.HasNext && await dialog.DisplayAlert("Success!", "The End!", "Ok" ,"Restart"))
            {
                await navigator.PopAsync();
                CurrentLevel.ResetStarStates();
                return;
            }

            var noRestart = next.IsChapterComplete 
                ? await dialog.DisplayAlert("Success!", "Chapter complete!\nStart new?", "Next", "Restart")
                : await dialog.DisplayAlert("Success!", "Next level?", "Next", "Restart");

            StartLevel(noRestart ? await next.Next() : CurrentLevel);
        }

        private async void ChangeLevelIfNeed()
        {
            var level = await gameService.GetCurrentLevel();

            if (level == CurrentLevel) return;

            StartLevel(level);
        }

        private async void StartLevel(Galaxy level)
        {
            IsBusy = true;

            if (level != CurrentLevel) 
                BackImageSvg = await resources.GetLevelBack(level);
            
            level.ResetStarStates();
            CurrentLevel = level;
            steps = 0;

            IsBusy = false;
        }
    }
}
