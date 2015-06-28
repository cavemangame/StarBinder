using Xamarin.Forms;
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

        private Chapter selected;
        public Chapter Selected 
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        protected override void OnNavigatedFrom()
        {
            base.OnNavigatedFrom();
        }

        protected async override void OnNavigatedTo()
        {
            base.OnNavigatedTo();

            if (Chapters == null)
            {
                Chapters = await gameService.GetAllChapters();
                Selected = await gameService.GetCurrentChapter();
            }
        }

        private Command<Galaxy> startCommand;
        public Command<Galaxy> StartCommand { get { return startCommand ?? (startCommand = new Command<Galaxy>(OnExecuteStart)); } }  

        private async void OnExecuteStart(Galaxy level)
        {
            await gameService.GoToLevel(level, Selected);
            await navigator.PushAsync<GameViewModel>();
        }

    }
}
