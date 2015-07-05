using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using StarBinder.Core;
using StarBinder.LevelEditor.Utils;

namespace StarBinder.LevelEditor.ViewModels
{
    class ChapterViewModel : BindableBase
    {
        private Chapter chapter;

        public ChapterViewModel(Chapter chapter)
        {
            this.chapter = chapter;
        }

        public ChapterViewModel()
        {
            IsEnabled = false;
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

        public Chapter Chapter
        {
            get { return chapter; }
            internal set
            {
                SetProperty(ref chapter, value);
            }
        }

        private int levelCount;
        public int LevelCount
        {
            get { return levelCount; }
            set { SetProperty(ref levelCount, value); }
        }

        private int addLevelCount;
        public int AddLevelCount
        {
            get { return addLevelCount; }
            set { SetProperty(ref addLevelCount, value); }
        }


        private ICommand loadBackCommand;
        public ICommand LoadBackCommand { get { return loadBackCommand ?? (loadBackCommand = new DelegateCommand(OnLoadBackCommandExecuted)); } }
        private void OnLoadBackCommandExecuted()
        {
            var dlg = new OpenFileDialog { CheckFileExists = true, DefaultExt = ".svg", Filter = "SVG Files|*.svg", Multiselect = false };
            if (dlg.ShowDialog() == true)
            {
                chapter.BackPath = dlg.FileName;
            }
        }

        public void OnNewChapterCommandExecuted()
        {
            LevelCount = 8;
            AddLevelCount = 2;
            OnRefreshListsCommandExecuted();
            IsEnabled = true;
        }

        private ICommand loadLevelCommand;
        public ICommand LoadLevelCommand { get { return loadLevelCommand ?? (loadLevelCommand = new DelegateCommand<Galaxy>(OnLoadLevelCommandExecuted)); } }
        private void OnLoadLevelCommandExecuted(Galaxy gal)
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                string json;
                using (var reader = new StreamReader(dlg.FileName))
                {
                    json = reader.ReadToEnd();
                }

                Galaxy galaxy = SerializationHelper.GalaxyFromJson(json);
                galaxy.Number = gal.Number;

                Chapter.Levels[Chapter.Levels.IndexOf(gal)] = galaxy;
            }
        }

        private ICommand loadAddLevelCommand;
        public ICommand LoadAddLevelCommand { get { return loadAddLevelCommand ?? (loadAddLevelCommand = new DelegateCommand<Galaxy>(OnLoadAddLevelCommandExecuted)); } }
        private void OnLoadAddLevelCommandExecuted(Galaxy gal)
        {
            var dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == true)
            {
                string json;
                using (var reader = new StreamReader(dlg.FileName))
                {
                    json = reader.ReadToEnd();
                }

                Galaxy galaxy = SerializationHelper.GalaxyFromJson(json);
                galaxy.Number = gal.Number;

                Chapter.AdditionalLevels[Chapter.AdditionalLevels.IndexOf(gal)] = galaxy;
            }
        }

        private ICommand refreshListsCommand;
        public ICommand RefreshListsCommand { get { return refreshListsCommand ?? (refreshListsCommand = new DelegateCommand(OnRefreshListsCommandExecuted)); } }
        private void OnRefreshListsCommandExecuted()
        {
            if (Chapter.Levels.Count != LevelCount)
            {
                Chapter.Levels = new List<Galaxy>(); // TODO: алгоритм, чтобы не затирал лишнее
                for (int i = 0; i < LevelCount; i++)
                {
                    Galaxy newGalaxy = Galaxy.CreateNew();
                    newGalaxy.Number = i + 1;
                    Chapter.Levels.Add(newGalaxy);
                }
            }
            if (Chapter.AdditionalLevels.Count != AddLevelCount)
            {
                Chapter.AdditionalLevels = new List<Galaxy>(); // TODO: алгоритм, чтобы не затирал лишнее
                for (int i = 0; i < AddLevelCount; i++)
                {
                    Galaxy newGalaxy = Galaxy.CreateNew();
                    newGalaxy.Number = LevelCount + i + 1;
                    Chapter.AdditionalLevels.Add(newGalaxy);
                }
            }
        }
        
    }
}
