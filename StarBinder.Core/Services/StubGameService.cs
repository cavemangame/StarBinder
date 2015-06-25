using System;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public class StubGameService : IGameService
    {
        private readonly IResourcesService resources;
        private readonly int levelsCount;
        private Galaxy current;
        
        public StubGameService(IResourcesService resources)
        {
            this.resources = resources;
            levelsCount = resources.GetLevelsCount();
        }

        public Task<Galaxy> GetCurrentLevel()
        {
            return Task.FromResult(current ?? (current = SerializationHelper.GalaxyFromJson(resources.GetLevel(1))));
        }

        public Task<bool> TryGetNextLevel(out Galaxy level)
        {
            level = null;
            return current.Number < levelsCount 
                ? Task.FromResult((level = current = SerializationHelper.GalaxyFromJson(resources.GetLevel(current.Number + 1))) != null) 
                : Task.FromResult(false);
        }

        public Task SaveLevelState(Galaxy level)
        {
            return Task.Factory.StartNew(() => {});
        }

        public Task SetLevelNumber(int number)
        {
            if (number < 1 && number > levelsCount)
                throw new ArgumentOutOfRangeException("number");
            
            return Task.Factory.StartNew(() =>
            {
                current = SerializationHelper.GalaxyFromJson(resources.GetLevel(number));
            });
        }
    }
}
