﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public class GameService : IGameService
    {
        private readonly IResourcesService resources;
        
        public GameService(IResourcesService resources)
        {
            Debug.WriteLine("GameService ctor");
			this.resources = resources;
            chapters = new Lazy<List<Chapter>>(() => resources.AllChapters().ToList());
        }

        private int LastChapterIndex
        {
            get { return resources.LastChapterIndex; }
            set { resources.LastChapterIndex = value; }
        }

        private Galaxy CurrentLevel { get { return CurrentChapter.CurrentLevel; } }
        private Chapter CurrentChapter { get { return Chapters[LastChapterIndex]; } }

        private readonly Lazy<List<Chapter>> chapters;
        private List<Chapter> Chapters { get { return chapters.Value; } }

        
        public Task<Galaxy> GetCurrentLevel()
        {
            return Task.Run(() => CurrentLevel);
        }

        public Task<Galaxy> GetNextLevel()
        {
            return CurrentChapter.Levels.Count > CurrentLevel.Number ?
                Task.FromResult<Galaxy>(null) :
                Task.Run(() =>
                {
                    CurrentChapter.LastLevelIndex++;
                    return CurrentLevel;
                });
        }

        public Task SaveState()
        {
            return Task.Run(() => { resources.UpdateChapter(CurrentChapter); });
        }

        public Task<Galaxy> GoToLevel(int number)
        {
            if (number < 1 && number > CurrentChapter.Levels.Count)
                throw new ArgumentOutOfRangeException("number");

            return Task.Run(() =>
            {
                CurrentChapter.LastLevelIndex = number - 1;
                return CurrentLevel;
            });
        }

        public Task<IEnumerable<Chapter>> GetAllChapters()
        {
            return Task.Run(() => Chapters.AsEnumerable());
        }
    }
}
