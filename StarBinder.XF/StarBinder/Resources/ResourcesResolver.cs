using System.IO;
using System.Linq;
using System.Reflection;
using StarBinder.Core.Services;

namespace StarBinder.Resources
{
    class ResourcesResolver : IResourcesService
    {
        public int GetLevelsCount()
        {
            var assembly = typeof(ResourcesResolver).GetTypeInfo().Assembly;
            var names = assembly.GetManifestResourceNames();
            return names.Count(r => r.Contains(".Resources.Levels."));
        }

        public string GetLevel(int level)
        {
            var assembly = typeof(ResourcesResolver).GetTypeInfo().Assembly;
            var fname = string.Format("StarBinder.Resources.Levels.level{0}", level);
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(fname)))
            {
                return reader.ReadToEnd();
            }
        }

        public string GetLevelBack(int level)
        {
            var assembly = typeof(ResourcesResolver).GetTypeInfo().Assembly;
            var fname = string.Format("StarBinder.Resources.Graphics.back_{0}.svg", level);
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(fname)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
