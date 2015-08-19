using System;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private Chapter chapter;

        public MainWindowViewModel()
        {
            OnNewLevelCommandExecuted();
            OnNewChapterCommandExecuted();
        }

        private GalaxyViewModel galaxyViewModel;
        public GalaxyViewModel GalaxyViewModel
        {
            get { return galaxyViewModel; }
            set { SetProperty(ref galaxyViewModel, value); }
        }

        private ChapterViewModel chapterViewModel;
        public ChapterViewModel ChapterViewModel
        {
            get { return chapterViewModel; }
            set { SetProperty(ref chapterViewModel, value); }
        }


        #region Chapters
        private ICommand newChapterCommand;
        public ICommand NewChapterCommand { get { return newChapterCommand ?? (newChapterCommand = new DelegateCommand(OnNewChapterCommandExecuted)); } }

        private void OnNewChapterCommandExecuted()
        {
            chapter = Chapter.CreateNew();
            var vm = new ChapterViewModel(chapter);
            vm.OnNewChapterCommandExecuted();
            ChapterViewModel = vm;
        }

        private ICommand saveChapterCommand;
        public ICommand SaveChapterCommand { get { return saveChapterCommand ?? (saveChapterCommand = new DelegateCommand(OnSaveChapterCommandExecuted)); } }

        private void OnSaveChapterCommandExecuted()
        {
            var json = chapter.ToJson();
            var dlg = new SaveFileDialog();


            if (dlg.ShowDialog() == true)
            {
                ReplaceBack(Path.GetDirectoryName(dlg.FileName));
                ResolvePaths();
                using (var writer = new StreamWriter(dlg.FileName, false))
                {
                    writer.Write(json);
                }
            }
        }

        private void ResolvePaths()
        {
            chapter.BackPath = Path.GetFileName(chapter.BackPath);

            foreach (var level in chapter.Levels.Union(chapter.AdditionalLevels))
            {
                level.BackPath = Path.GetFileName(level.BackPath);
            }
        }

        private void ReplaceBack(string dir)
        {
            if (!String.IsNullOrEmpty(chapter.BackPath.Trim()))
            {
                if (dir != Path.GetDirectoryName(chapter.BackPath))
                {
                    File.Copy(chapter.BackPath, Path.Combine(dir, Path.GetFileName(chapter.BackPath)), true);
                }
            } 
        }

        private ICommand loadChapterCommand;
        public ICommand LoadChapterCommand { get { return loadChapterCommand ?? (loadChapterCommand = new DelegateCommand(OnLoadChapterExecuted)); } }

        private void OnLoadChapterExecuted()
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                string json;
                using (var reader = new StreamReader(dlg.FileName))
                {
                    json = reader.ReadToEnd();
                }

                chapter = SerializationHelper.ChapterFromJson(json);
                ChapterViewModel = new ChapterViewModel(chapter);
                ChapterViewModel.IsEnabled = true;
            }
        }
        #endregion


        #region Level

        private ICommand newLevelCommand;
        public ICommand NewLevelCommand { get { return newLevelCommand ?? (newLevelCommand = new DelegateCommand(OnNewLevelCommandExecuted)); } }

        private void OnNewLevelCommandExecuted()
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

        private ICommand saveLevelCommand;
        public ICommand SaveLevelCommand { get { return saveLevelCommand ?? (saveLevelCommand = new DelegateCommand(OnSaveLevelCommandExecuted)); } }

        private void OnSaveLevelCommandExecuted()
        {
            var json = galaxy.ToJson();
            var dlg = new SaveFileDialog();

            if (dlg.ShowDialog() == true)
            {
                CopyBackIfNeeded(Path.GetDirectoryName(dlg.FileName));
                using (var writer = new StreamWriter(dlg.FileName, false))
                {
                    writer.Write(json);
                }
            }
        }

        private void CopyBackIfNeeded(string dir)
        {
            if (galaxy.BackPath != null && !String.IsNullOrEmpty(galaxy.BackPath.Trim()))
            {
                File.Copy(galaxy.BackPath, Path.Combine(dir, Path.GetFileName(galaxy.BackPath)), true);
            }
        }

        private ICommand loadLevelCommand;
        public ICommand LoadLevelCommand { get { return loadLevelCommand ?? (loadLevelCommand = new DelegateCommand(OnLoadLevelExecuted)); } }

        private void OnLoadLevelExecuted()
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

        #endregion

    }
}
