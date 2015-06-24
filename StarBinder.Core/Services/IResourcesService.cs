namespace StarBinder.Core.Services
{
    public interface IResourcesService
    {
        int GetLevelsCount();

        string GetLevel(int level);

        string GetLevelBack(int level);
    }
}
