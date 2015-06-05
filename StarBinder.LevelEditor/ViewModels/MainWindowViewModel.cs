using System;
using System.IO;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using StarBinder.Core;

namespace StarBinder.LevelEditor.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private Galaxy galaxy;

        public MainWindowViewModel()
        {
            OnNewCommandExecuted();
        }

        private GalaxyViewModel galaxyViewModel;
        public GalaxyViewModel GalaxyViewModel
        {
            get { return galaxyViewModel; }
            set { SetProperty(ref galaxyViewModel, value); }
        }

        private ICommand newCommand;
        public ICommand NewCommand { get { return newCommand ?? (newCommand = new DelegateCommand(OnNewCommandExecuted)); } }

        private void OnNewCommandExecuted()
        {
            galaxy = Galaxy.CreateNew();
            var vm = new GalaxyViewModel(galaxy);
            if (GalaxyViewModel != null)
            {
                vm.BackImage = GalaxyViewModel.BackImage;
                vm.Width = GalaxyViewModel.Width;
                vm.Height = GalaxyViewModel.Height;
            }
            GalaxyViewModel = vm;
        }

        private ICommand saveCommand;
        public ICommand SaveCommand { get { return saveCommand ?? (saveCommand = new DelegateCommand(OnSaveCommandExecuted)); } }

        private void OnSaveCommandExecuted()
        {
            var json = galaxy.ToJson();
            var dlg = new SaveFileDialog();

            if (dlg.ShowDialog() == true)
            {
                using (var writer = new StreamWriter(dlg.FileName, false))
                {
                    writer.Write(json);
                }
            }
        }

        private ICommand loadCommand;
        public ICommand LoadCommand { get { return loadCommand ?? (loadCommand = new DelegateCommand(OnExecuteLoad)); } }

        private void OnExecuteLoad()
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                string json;
                using (var reader = new StreamReader(dlg.FileName))
                {
                    json = reader.ReadToEnd();
                }
               
                galaxy = SerializationHelper.GalaxyFromJson(json);
                GalaxyViewModel = new GalaxyViewModel(galaxy)
                {
                    Width = GalaxyViewModel.Width,
                    Height = GalaxyViewModel.Height,
                    BackImage = GalaxyViewModel.BackImage
                };
            }
        }
    }
}
