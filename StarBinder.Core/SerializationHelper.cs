using Newtonsoft.Json;

namespace StarBinder.Core
{
    public static class SerializationHelper
    {
        public static string ToJson(this Galaxy galaxy)
        {
            return JsonConvert.SerializeObject(new GalaxyData(galaxy));
        }

        public static Galaxy GalaxyFromJson(string json)
        {
            var data = JsonConvert.DeserializeObject<GalaxyData>(json);
            return Galaxy.Create(data);
        }

        public static string ToJson(this Chapter chapter)
        {
            return JsonConvert.SerializeObject(new ChapterData(chapter));
        }

        public static Chapter ChapterFromJson(string json)
        {
            var data = JsonConvert.DeserializeObject<ChapterData>(json);
            return Chapter.Create(data);
        }
    }
}
