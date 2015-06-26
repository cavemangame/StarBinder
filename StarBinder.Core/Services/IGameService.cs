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
        Task<Galaxy> GetNextLevel();

        Task SaveState();
        
        Task<Galaxy> GoToLevel(int number);
        Task<IEnumerable<Chapter>> GetAllChapters();
    }
}
