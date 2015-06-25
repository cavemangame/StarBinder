using System;
using System.Threading.Tasks;

namespace StarBinder.Core.Services
{
    [Obsolete("удалить в проекте финфона")]
    public interface ILevelsService
    {
        Task<Galaxy> GetLevelModel(int level);
        Task<bool> IsNextLevel(Galaxy level);
        Task UpdateHighScore(int chapter, int level, int steps);
    }
}
