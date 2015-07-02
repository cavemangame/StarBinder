using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StarBinder.Core;
using StarBinder.Core.Services;

namespace StarBinder.Resources
{
    class TestResourcesResolver : IResourcesService
    {
        public TestResourcesResolver()
        {
            LastChapterIndex = 0;
        }
        
        public Task<string> GetLevelBack(Galaxy level)
        {
            var assembly = typeof(TestResourcesResolver).GetTypeInfo().Assembly;
            var fname = string.Format("StarBinder.Resources.Graphics.back_{0}.svg", level.Number);
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(fname)))
            {
                return reader.ReadToEndAsync();
            }
        }

        public int LastChapterIndex { get; set; }


        //todo
        public IEnumerable<Chapter> AllChapters()
        {
            
            Debug.WriteLine("All chapters call");
            var n = GetLevelsCount();

            var strings = new string[n];
            for (int i = 0; i < n; i++)
            {
                strings[i] = GetLevel(i + 1);
            }
            
            var chapters = new List<Chapter>();

            for (int i = 0; i < 3; i++)
            {
                var ch = new ChapterData();
                ch.Name = "Chapter " + (i + 1);
                ch.Description = "Chapter Description " + (i + 1);
                ch.LastLevel = i*2;
                ch.Levels = new List<GalaxyData>();
                for (int j = 0; j < n; j++)
                {
                    ch.Levels.Add(JsonConvert.DeserializeObject<GalaxyData>(strings[j]));
                }

                chapters.Add(Chapter.Create(ch));
            }

            Debug.WriteLine("All chapters call ended");
            return chapters;
        }

        public void UpdateChapter(Chapter chapter)
        {
            //todo
        }



        private int GetLevelsCount()
        {
            var assembly = typeof(TestResourcesResolver).GetTypeInfo().Assembly;
            var names = assembly.GetManifestResourceNames();
            return names.Count(r => r.Contains(".Resources.Levels."));
        }

        private string GetLevel(int level)
        {
            var assembly = typeof(TestResourcesResolver).GetTypeInfo().Assembly;
            var fname = string.Format("StarBinder.Resources.Levels.level{0}", level);
            using (var reader = new StreamReader(assembly.GetManifestResourceStream(fname)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
