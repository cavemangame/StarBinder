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

        public GameViewModel(IGameService gameService, IResourcesService resources, IDialogService dialog, INavigator navigator)
        {
            this.gameService = gameService;
            this.resources = resources;
            this.dialog = dialog;
            this.navigator = navigator;
        }

        public override void NavigatedFrom()
        {
            ChangeLevelIfNeed();
        }

        public override void NavigatedTo()
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

            if (!CurrentLevel.IsComplete) return;
            
            //todo await gameService.SaveLevelState(currentLevel);

            Galaxy next;
            if (await gameService.TryGetNextLevel(out next))
            {
                next = await dialog.DisplayAlert("Success!", "Next level ? =)", "Next", "Restart") ? next : CurrentLevel;
                StartLevel(next);
            }
            else
            {
                await dialog.DisplayAlert("Success!", "The End =(", "Ok");
                await gameService.SetLevelNumber(1);
                await navigator.PopAsync();
            }
        }

        private async void ChangeLevelIfNeed()
        {
            var level = await gameService.GetCurrentLevel();

            if (level == CurrentLevel) return;

            StartLevel(level);
        }

        private void StartLevel(Galaxy level)
        {
            IsBusy = true;

            CurrentLevel = null;
            BackImageSvg = resources.GetLevelBack(level.Number);
            level.ResetStarStates();
            CurrentLevel = level;

            IsBusy = false;
        }
    }
}
