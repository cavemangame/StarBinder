using System;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public class GameService : IGameService
    {
        public GameService()
        {
        }
        
        public Task<Galaxy> GetCurrentLevel()
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryGetNextLevel(out Galaxy level)
        {
            throw new NotImplementedException();
        }

        public Task SaveLevelState(Galaxy level)
        {
            throw new NotImplementedException();
        }

        public Task SetLevelNumber(int number)
        {
            throw new NotImplementedException();
        }
    }
}
