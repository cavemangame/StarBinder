using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    public interface IGameService
    {
        Task<Galaxy> GetCurrentLevel();
        Task<bool> TryGetNextLevel(out Galaxy level);
        Task SaveLevelState(Galaxy level);
    }
}
