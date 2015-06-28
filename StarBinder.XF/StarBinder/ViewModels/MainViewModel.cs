using Xamarin.Forms;
using XF.Core.Services;
using XF.Core.ViewModels;

namespace StarBinder.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        private readonly IDialogService dialog;
        private readonly INavigator navigator;

        public MainViewModel(IDialogService dialog, INavigator navigator)
        {
            this.dialog = dialog;
            this.navigator = navigator;
        }

        private Command gameCommand;
        public Command GameCommand { get { return gameCommand ?? (gameCommand = new Command(OnExecuteGame)); } }

        private async void OnExecuteGame()
        {
            await navigator.PushAsync<GameViewModel>();
        }


        private Command<string> testClickCommand;
        public Command<string> TestClickCommand { get { return testClickCommand ?? (testClickCommand = new Command<string>(OnExecuteTestClick)); } }

        private async void OnExecuteTestClick(string arg)
        {
            if (arg == "1")
            {
                await navigator.PushAsync<LevelsViewModel>();
                return;
            }

            await dialog.DisplayAlert("Test", arg, "Ok");
        }
    }
}
